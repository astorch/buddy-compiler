using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Bumblebee.buddy.compiler.packaging;
using NUnit.Framework;

namespace Bumblebee.buddy.compiler.tests.packaging {
    [TestFixture]
    public class UnitBundlerTests {

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_ArgumentNullException() {
            new UnitBundler(null);
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void Bundle_ArgumentNullException() {
            new UnitBundler(new _ImportPathProvider()).Bundle(null);
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void Bundle_ArgumentException() {
            new UnitBundler(new _ImportPathProvider()).Bundle(new FileInfo("noway.out"));
        }

        [Test]
        public void Bundle_Ok_WithoutIncludes() {
            // Arrange
            FileInfo mainTdilFile = CreateTmpFile();
            byte[] fileData = File.ReadAllBytes(mainTdilFile.FullName);

            // Act
            UnitBundleTable unitBundleTable = new UnitBundler(new _ImportPathProvider()).Bundle(mainTdilFile);

            // Assert
            Assert.NotNull(unitBundleTable, "unitBundleTable != null");

            UnitBundleTableEntry[] unitBundleTableEntries;
            using (IEnumerator<UnitBundleTableEntry> itr = unitBundleTable.GetEnumerator())
                unitBundleTableEntries = itr.ToArray();

            Assert.AreEqual(1, unitBundleTableEntries.Length, "unitBundleTableEntries.Length != 1");
            Assert.AreEqual("main.btc.tdil", unitBundleTableEntries[0].Name, "unitBundleTableEntries[0].Name != \"main.btc.tdil\"");
            CollectionAssert.AreEqual(fileData, unitBundleTableEntries[0].Data, "unitBundleTableEntries[0].Data != fileData");
        }

        [Test]
        public void Bundle_Ok_WithIncludes() {
            // Arrange
            string fileContent = new StringBuilder()
                .AppendLine("#include \"v.s.w.t1\"")
                .AppendLine("#include \"v.s.w.t2\"")
                .AppendLine()
                .AppendLine("Unit v.s.n.t3")
                .AppendLine()
                .AppendLine("Main:")
                .AppendLine("End")
                .AppendLine()
                .AppendLine("End")
                .ToString();

            FileInfo mainTdilFile = CreateTmpFile(fileContent);
            FileInfo t1 = CreateTmpFile();
            FileInfo t2 = CreateTmpFile();

            byte[] fileData = Encoding.UTF8.GetBytes(
                fileContent
                    .Replace("#include \"v.s.w.t1\"", string.Format("#include \"{0}\"", t1.Name))
                    .Replace("#include \"v.s.w.t2\"", string.Format("#include \"{0}\"", t2.Name))
                    );

            
            // Act
            UnitBundleTable unitBundleTable = new UnitBundler(new _ImportPathProvider(new []{t1, t2})).Bundle(mainTdilFile);

            // Assert
            Assert.NotNull(unitBundleTable, "unitBundleTable != null");

            UnitBundleTableEntry[] unitBundleTableEntries;
            using (IEnumerator<UnitBundleTableEntry> itr = unitBundleTable.GetEnumerator())
                unitBundleTableEntries = itr.ToArray();

            Assert.AreEqual(3, unitBundleTableEntries.Length, "unitBundleTableEntries.Length != 1");
            Assert.AreEqual("main.btc.tdil", unitBundleTableEntries[2].Name, "unitBundleTableEntries[0].Name != \"main.btc.tdil\"");
            CollectionAssert.AreEqual(fileData, unitBundleTableEntries[2].Data, "unitBundleTableEntries[0].Data != fileData");

            Assert.AreEqual(t1.Name, unitBundleTableEntries[0].Name, "unitBundleTableEntries[0].Name != t1.Name");
            CollectionAssert.AreEqual(File.ReadAllBytes(t1.FullName), unitBundleTableEntries[0].Data, "unitBundleTableEntries[0].Data != t1.Data");

            Assert.AreEqual(t2.Name, unitBundleTableEntries[1].Name, "unitBundleTableEntries[1].Name != t2.Name");
            CollectionAssert.AreEqual(File.ReadAllBytes(t2.FullName), unitBundleTableEntries[1].Data, "unitBundleTableEntries[1].Data != t1.Data");
        }

        private static FileInfo CreateTmpFile(string content = null) {
            string tmpFileName = Path.GetTempFileName();
            File.WriteAllText(tmpFileName, content ?? Guid.NewGuid().ToString());
            return new FileInfo(tmpFileName);
        }

        class _ImportPathProvider : IImportPathProvider {

            private readonly FileInfo[] iFileSet;

            /// <inheritdoc />
            public _ImportPathProvider(FileInfo[] fileSet = null) {
                iFileSet = fileSet ?? new FileInfo[0];
            }


            /// <inheritdoc />
            public string GetPath(UnitName unitName) {
                if (unitName.ToQualifiedString() == "v.s.w.t1") return iFileSet[0].FullName;
                return iFileSet[1].FullName;
            }

            /// <inheritdoc />
            public UnitName GetUnitName(string scenarioName) {
                throw new NotImplementedException();
            }
        }
    }

    static class Enumerator {
        public static TItem[] ToArray<TItem>(this IEnumerator<TItem> itr) {
            List<TItem> elements = new List<TItem>();
            while (itr.MoveNext()) {
                elements.Add(itr.Current);
            }

            return elements.ToArray();
        }
    }
}