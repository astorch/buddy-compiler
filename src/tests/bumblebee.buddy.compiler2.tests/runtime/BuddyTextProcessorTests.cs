using System.Text;
using bumblebee.buddy.compiler2.model;
using bumblebee.buddy.compiler2.runtime;
using NUnit.Framework;

namespace bumblebee.buddy.compiler2.tests.runtime {
    [TestFixture]
    public class BuddyTextProcessorTests {

        [Test]
        public void ProcessText() {
            // Arrange
            string buddyText = GetBuddyTextForTest();

            // Act
            BuddyTextProcessor buddyTextProcessor = new BuddyTextProcessor();
            TestcaseContext testcaseContext = buddyTextProcessor.ProcessText(buddyText);

            // Assert
            Assert.IsNotNull(testcaseContext, "testcaseContext != null");

            ApplicationContext appCtx = testcaseContext.Application;
            Assert.IsNotNull(appCtx, "appCtx != null");
            Assert.AreEqual("Verwaltung", appCtx.Name, "appCtx.Name != Verwaltung");

            Version appVrsn = appCtx.Version;
            Assert.IsNotNull(appVrsn, "appVrsn != null");
            Assert.AreEqual("9.512", appVrsn.Min, "appVrsn.Min != 9.512");
            Assert.AreEqual("*", appVrsn.Max, "appVrsn.Max != *");

            UsecaseContext usecaseCtx = testcaseContext.Usecase;
            Assert.IsNotNull(usecaseCtx, "usecaseCtx != null");
            Assert.AreEqual("Mitarbeitermodul von GTÜ", usecaseCtx.Name, "usecaseCtx.Name has unexpected value");

            ScenarioContext sceneCtx = testcaseContext.Scenario;
            Assert.IsNotNull(sceneCtx, "sceneCtx != null");
            Assert.AreEqual("1. Grundtest ", sceneCtx.Name, "sceneCtx.Name has unexpected value");

            ScenarioParameter[] sceneArgs = sceneCtx.ScenarioParameters;
            Assert.IsNotNull(sceneArgs, "sceneArgs != null");
            Assert.AreEqual(3, sceneArgs.Length, "sceneArgs.Length != 3");

            Assert.AreEqual("piNr", sceneArgs[0].Name, "sceneArgs[0].Name != piNr");
            Assert.AreEqual("\"001\"", sceneArgs[0].Value, "sceneArgs[0].Value != \"001\"");
            Assert.AreEqual("piPass", sceneArgs[1].Name, "sceneArgs[1].Name != piPass");
            Assert.AreEqual("\"Passw001\"", sceneArgs[1].Value, "sceneArgs[1].Value != \"Passw001\"");
            Assert.AreEqual("tabelle", sceneArgs[2].Name, "sceneArgs[2].Name != tabelle");
            Assert.AreEqual("\"C:\\daten\\lfz.xls\"", sceneArgs[2].Value, "sceneArgs[2].Value != \"C:\\daten\\lfz.xls\"");

            Import[] importSet = testcaseContext.Imports;
            Assert.IsNotNull(importSet, "importSet != null");
            Assert.AreEqual(2, importSet.Length, "importSet.Length != 2");

            Assert.AreEqual("Verwaltung.s.Mitarbeitermodul.Meine_Daten_bearbeiten", importSet[0].Name, "importSet[0].Name has unexpected value");
            Assert.AreEqual("Verwaltung.s.Mitarbeitermodul.Rechte_wiederherstellen", importSet[1].Name, "importSet[1].Name has unexpected value");

            StepsContext stepsContext = testcaseContext.Steps;
            Assert.IsNotNull(stepsContext, "stepsContext != null");

            DirectiveContext[] directiveSet = stepsContext.Directives;
            Assert.IsNotNull(directiveSet, "directiveSet != null");
            Assert.AreEqual(8, directiveSet.Length, "directiveSet.Length != 8");

            DirectiveContext frstDir = directiveSet[0];
            Assert.AreEqual(5, frstDir.References.Length, "[1] directive: Unexpected reference count");
            Assert.AreEqual("Wähle", frstDir.References[0].Value, "[1] directive: Unexpected verb");
            Assert.IsAssignableFrom<ParameterReference>(frstDir.References[1], "[1] directive: Unexpected reference type");
            Assert.AreEqual("piNr", frstDir.References[1].Value, "[1] directive: Unexpected reference value");
        }

        private string GetBuddyTextForTest() {
            string buddyText = new StringBuilder()
                    .AppendLine("Anwendung: Verwaltung (9.512-*)")
                    .AppendLine("Anwendungsfall: Mitarbeitermodul von GTÜ")
                    .AppendLine("Szenario: 1. Grundtest (piNr = \"001\", piPass = \"Passw001\", tabelle = \"C:\\daten\\lfz.xls\")")
                    .AppendLine("benutzt Verwaltung.s.Mitarbeitermodul.Meine_Daten_bearbeiten")
                    .AppendLine("benutzt Verwaltung.s.Mitarbeitermodul.Rechte_wiederherstellen")
                    .AppendLine("Schritte:")
                    .AppendLine("Wähle $piNr in <Combo:Mitarbeiter> aus.")
                    .AppendLine("Führe [Meine Daten bearbeiten] mit piNr=\"801\", piPass=\"Passw801\" aus.")
                    .AppendLine("Führe [Rechte wiederherstellen] mit piNr=$piNr, piPass=$piPass aus.")
                    .AppendLine("Wähle in <ListView:Mitarbeiter> \"Meine Daten\" aus.")
                    .AppendLine("Klicke auf <Button:Berechtigungen_Loeschen> einfach.")
                    .AppendLine("Prüfe ob <Combo:RechteAdressen> den Wert \"Adressen lesen, bearbeiten\" enthält.")
                    .AppendLine("Wähle in <ListView:Mitarbeiter> unter \"Mitarbeiter\", \"Verwaltungskraft\" \"801 - Stefan E.\" aus.")
                    .Append("Warte bis Anwendung beendet ist.")
                    .ToString();
            return buddyText;
        }
    }
}