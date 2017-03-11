using System;
using System.Text;
using Bumblebee.buddy.compiler.writers;
using NUnit.Framework;

namespace Bumblebee.buddy.compiler.tests.writers {
    [TestFixture]
    public class TdilFileWriterTests {
        [Test]
        public void WriteFullUnit() {
            // Arrange
            string expectedTdilFileContent = new StringBuilder()
                .AppendLine("// Compiler generated file")
                .AppendLine("// Buddy Compiler version 0.1")
                .AppendLine("// Generated on {date}")
                .AppendLine()
                .AppendLine("Unit untitled ($prgName, $prgPath)")
                .AppendLine()
                .AppendLine("Main:")
                .AppendLine("s1")
                .AppendLine("s2")
                .AppendLine("goto Foo:(3,2)")
                .AppendLine("End")
                .AppendLine()
                .AppendLine("Foo:($p1, $p2)")
                .AppendLine("$p1 <-> $p2")
                .AppendLine("$p2 <-> $p1")
                .AppendLine("End")
                .AppendLine()
                .AppendLine("End")
                .AppendLine()
                .ToString();

            // Act
            DateTime now = DateTime.Now;
            TdilFileWriter tdilFileWriter = new TdilFileWriter("0.1");
            using (TdilUnitWriter unitWriter = tdilFileWriter.CreateUnit("untitled", new[] { "$prgName", "$prgPath" })) {
                
                // Main section
                using (TdilSectionWriter sectionWriter = unitWriter.CreateSection("Main")) {
                    sectionWriter.AppendLine("s1");
                    sectionWriter.AppendLine("s2");
                    sectionWriter.AppendLine("goto Foo:(3,2)");
                }

                // Foo section
                using (TdilSectionWriter sectionWriter = unitWriter.CreateSection("Foo", new[] {"$p1", "$p2"})) {
                    sectionWriter.AppendLine("$p1 <-> $p2");
                    sectionWriter.AppendLine("$p2 <-> $p1");
                }
            }

            string tdilFileContent = tdilFileWriter.WriteToString();

            // Assert
            // Insert date before
            expectedTdilFileContent = expectedTdilFileContent.Replace("{date}", now.ToString());

            Assert.AreEqual(expectedTdilFileContent, tdilFileContent, "expecedTdilFileContent != tdilFileContent");
        }

        [Test]
        public void WriteUnitWithAliasAndImports() {
            // Arrange
            string expectedTdilFileContent = new StringBuilder()
                .AppendLine("// Compiler generated file")
                .AppendLine("// Buddy Compiler version 0.1")
                .AppendLine("// Generated on {date}")
                .AppendLine()
                .AppendLine("#include \"Helper.tdil\"")
                .AppendLine("#include \"Math.tdil\"")
                .AppendLine()
                .AppendLine("#alias \"ListBox:Name\"")
                .AppendLine("#alias \"ListBox:Pinr\"")
                .AppendLine("#alias \"ListBox:Passwort\"")
                .AppendLine()
                .AppendLine("Unit untitled ($prgName, $prgPath)")
                .AppendLine()
                .AppendLine("Main:")
                .AppendLine("close(_Application, , default)")
                .AppendLine("End")
                .AppendLine()
                .AppendLine("End")
                .AppendLine()
                .ToString();

            // Act
            DateTime now = DateTime.Now;
            TdilFileWriter tdilFileWriter = new TdilFileWriter("0.1");

            // Imports
            tdilFileWriter.AddImport(new[] {"Helper.tdil", "Math.tdil"});

            // Alias
            tdilFileWriter.AddAlias(new[] {"ListBox:Name", "ListBox:Pinr", "ListBox:Passwort"});

            using (TdilUnitWriter unitWriter = tdilFileWriter.CreateUnit("untitled", new[] {"$prgName", "$prgPath"})) {
                // Main section
                using (TdilSectionWriter sectionWriter = unitWriter.CreateSection("Main")) {
                    sectionWriter.AppendLine("close(_Application, , default)");
                }
            }

            string tdilFileContent = tdilFileWriter.WriteToString();

            // Assert
            // Insert date before
            expectedTdilFileContent = expectedTdilFileContent.Replace("{date}", now.ToString());

            Assert.AreEqual(expectedTdilFileContent, tdilFileContent, "expecedTdilFileContent != tdilFileContent");
        }
    }
}