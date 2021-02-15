using System.Collections.Generic;
using System.Linq;
using Bumblebee.buddy.compiler.collectiontools;
using NUnit.Framework;

namespace Bumblebee.buddy.compiler.tests.collectiontools {
    [TestFixture]
    public class WordIteratorTests {

        [Test, TestCaseSource(typeof(TestCaseFactory), nameof(TestCaseFactory.TestCasesWords))]
        public IEnumerable<string> Words(string text) {
            return new WordIterator(text).Words().Select(wrd => wrd.Text);
        }

        class TestCaseFactory {
            public static IEnumerable<TestCaseData> TestCasesWords {
                get {
                    yield return new TestCaseData("Starte Anwendung.")
                        .Returns(new[] {"Starte", "Anwendung"});
                    
                    yield return new TestCaseData("Führe meine Anweisungen korrekt aus.\r\nFrag nicht.")
                        .Returns(new[]{"Führe", "meine", "Anweisungen", "korrekt", "aus", "Frag", "nicht"});

                    yield return new TestCaseData("Früher, war alles besser, ok.")
                        .Returns(new[] {"Früher", "war", "alles", "besser", "ok"});

                    yield return new TestCaseData("Führe [Mängel in dem Mangelbaum auswählen] aus.")
                        .Returns(new[] {"Führe", "[Mängel in dem Mangelbaum auswählen]", "aus"});

                    yield return new TestCaseData("Führe [Neustart, ohne Foto] (\"5,12\", \"a, b\", $a, $b) aus.")
                        .Returns(new[] { "Führe", "[Neustart, ohne Foto]", "(\"5,12\", \"a, b\", $a, $b)", "aus" });
                }
            }
        }
    }
}