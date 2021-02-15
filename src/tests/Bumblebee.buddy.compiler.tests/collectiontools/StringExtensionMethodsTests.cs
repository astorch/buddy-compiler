using System.Collections.Generic;
using Bumblebee.buddy.compiler.collectiontools;
using NUnit.Framework;

namespace Bumblebee.buddy.compiler.tests.collectiontools {
    [TestFixture]
    public class StringExtensionMethodsTests {
        [Test, TestCaseSource(typeof(TestCaseFactory), nameof(TestCaseFactory.TestCaseTexts))]
        public string[] SplitToBuddyTokens(string text) {
            return text.SplitToBuddyTokens();
        }

        class TestCaseFactory {
            public static IEnumerable<TestCaseData> TestCaseTexts {
                get {
                    yield return new TestCaseData("Führe [Login] (\"001\", \"Passw001\", \"0001-00\") aus")
                        .Returns(new[] { "Führe", "[Login]", "(\"001\", \"Passw001\", \"0001-00\")", "aus" });

                    yield return new TestCaseData("Führe [FahrzeugNeuanlage] (\"FZKENNZEICHEN()\", \"1110\", \"FIN()\", \"Mustermann\", \"Max\", \"8252\", \"ACN\", \"M1\", \"AF\", \"6\", \"11/2016\") aus")
                        .Returns(new[] { "Führe", "[FahrzeugNeuanlage]", "(\"FZKENNZEICHEN()\", \"1110\", \"FIN()\", \"Mustermann\", \"Max\", \"8252\", \"ACN\", \"M1\", \"AF\", \"6\", \"11/2016\")", "aus" });

                    yield return new TestCaseData("Führe [Kilometerstand eintragen] (\"1234\") aus")
                        .Returns(new[]{"Führe", "[Kilometerstand eintragen]", "(\"1234\")", "aus"});

                    yield return new TestCaseData("Führe [HU ausklappen] aus")
                        .Returns(new[] {"Führe", "[HU ausklappen]", "aus"});

                    yield return new TestCaseData("Führe [Bremswerte eintragen] (\"5\", \"5\", \"5\", \"5\", \"5\", \"5\", \"5\", \"5\") aus")
                        .Returns(new[] { "Führe", "[Bremswerte eintragen]", "(\"5\", \"5\", \"5\", \"5\", \"5\", \"5\", \"5\", \"5\")", "aus" });
                }
            }
        }
    }
}