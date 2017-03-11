using System.IO;
using System.Reflection;
using Bumblebee.buddy.compiler.simplex;
using NUnit.Framework;

namespace Bumblebee.buddy.compiler.tests.simplex {
    [TestFixture]
    public class BuddyTextProcessorTests {

        [Test]
        public void NullTextProcessing_Ok() {
            BuddyTextProcessor processor = new BuddyTextProcessor();
            BuddyTextInfo result = processor.ProcessText(null);
            Assert.IsNull(result, "result != null");
        }

        [Test]
        public void StepsOnly_Ok() {
            // Arrange
            Stream prgStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Bumblebee.buddy.compiler.tests.resources.BuddySimpleTestCase.btc");
            if (prgStream == null) Assert.Fail("'BuddySimpleTestCase.btc' not found!");

            string testCase = new StreamReader(prgStream).ReadToEnd();

            // Act
            BuddyTextProcessor processor = new BuddyTextProcessor();
            BuddyTextInfo result = processor.ProcessText(testCase);

            // Assert
            Assert.IsNotNull(result, "result != null");
            Assert.AreEqual(8, result.Steps.Length, "result.Steps.Length != 8");
            
            Assert.IsNull(result.ApplicationText, "result.ApplicationText != null");
            Assert.IsNull(result.UseCaseText, "result.UseCaseText != null");
            Assert.IsNull(result.ScenarioText, "result.ScenarioText != null");
            Assert.IsNull(result.Precondition, "result.Precondition != null");
            CollectionAssert.AreEqual(new BuddyTextParameter[0], result.Parameters, "result.Parameters != new[0]");

            Assert.AreEqual("Starte \"C:\\EGUB\\EGUB.exe\".", result.Steps[0]);
            Assert.AreEqual("Warte bis Fenster <WindowAnmeldung> sichtbar ist.", result.Steps[1]);
            Assert.AreEqual("Wähle in <ListboxPinr> den Wert \"000\" aus.", result.Steps[2]);
            Assert.AreEqual("Setze in <TextboxPass> den Wert \"Passw001\" ein.", result.Steps[3]);
            Assert.AreEqual("Klicke den Button <ButtonAnmelden> einfach.", result.Steps[4]);
            Assert.AreEqual("Warte bis Fenster <MainWindowEgub> sichtbar ist.", result.Steps[5]);
            Assert.AreEqual("Klicke den Button <MainWindowClose> einfach.", result.Steps[6]);
            Assert.AreEqual("Warte bis Anwendung geschlossen ist.", result.Steps[7]);
        }

        [Test]
        public void Standard_Ok() {
            // Arrange
            Stream prgStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Bumblebee.buddy.compiler.tests.resources.BuddyStandardTestCase.btc");
            if (prgStream == null) Assert.Fail("'BuddyStandardTestCase.btc' not found!");

            string testCase = new StreamReader(prgStream).ReadToEnd();

            // Act
            BuddyTextProcessor processor = new BuddyTextProcessor();
            BuddyTextInfo result = processor.ProcessText(testCase);

            // Assert
            Assert.IsNotNull(result, "result != null");
            Assert.AreEqual(8, result.Steps.Length, "result.Steps.Length != 8");

            Assert.AreEqual("EGUB", result.ApplicationText, "result.ApplicationText != 'EGUB'");
            Assert.AreEqual("Anmeldung an EGUB", result.UseCaseText, "result.UseCaseText != 'Anmeldung an EGUB'");
            Assert.AreEqual("Erfolgreiche Anmeldung", result.ScenarioText, "result.ScenarioText != 'Erfolgreiche Anmeldung'");
            Assert.AreEqual("-", result.Precondition, "result.Precondition != '-'");
            CollectionAssert.AreEqual(new BuddyTextParameter[0], result.Parameters, "result.Parameters != new[0]");

            Assert.AreEqual("Starte \"C:\\EGUB\\EGUB.exe\".", result.Steps[0]);
            Assert.AreEqual("Warte bis Fenster <WindowAnmeldung> sichtbar ist.", result.Steps[1]);
            Assert.AreEqual("Wähle in <ListboxPinr> den Wert \"000\" aus.", result.Steps[2]);
            Assert.AreEqual("Setze in <TextboxPass> den Wert \"Passw001\" ein.", result.Steps[3]);
            Assert.AreEqual("Klicke den Button <ButtonAnmelden> einfach.", result.Steps[4]);
            Assert.AreEqual("Warte bis Fenster <MainWindowEgub> sichtbar ist.", result.Steps[5]);
            Assert.AreEqual("Klicke den Button <MainWindowClose> einfach.", result.Steps[6]);
            Assert.AreEqual("Warte bis Anwendung geschlossen ist.", result.Steps[7]);
        }

        [Test]
        public void FullFeaturedText_Ok() {
            // Arrange
            Stream prgStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Bumblebee.buddy.compiler.tests.resources.BuddyFullFeaturedTestCase.btc");
            if (prgStream == null) Assert.Fail("'BuddyFullFeaturedTestCase.btc' not found!");

            string testCase = new StreamReader(prgStream).ReadToEnd();

            // Act
            BuddyTextProcessor processor = new BuddyTextProcessor();
            BuddyTextInfo result = processor.ProcessText(testCase);

            // Assert
            Assert.IsNotNull(result, "result != null");
            Assert.AreEqual(8, result.Steps.Length, "result.Steps.Length != 8");

            Assert.AreEqual("EGUB", result.ApplicationText, "result.ApplicationText != 'EGUB'");
            Assert.AreEqual("Anmeldung an EGUB", result.UseCaseText, "result.UseCaseText != 'Anmeldung an EGUB'");
            Assert.AreEqual("Erfolgreiche Anmeldung", result.ScenarioText, "result.ScenarioText != 'Erfolgreiche Anmeldung'");
            Assert.AreEqual("-", result.Precondition, "result.Precondition != '-'");
            CollectionAssert.AreEqual(new[] {new BuddyTextParameter("piNr", "\"000\""), new BuddyTextParameter("pass", "\"Passw001\""), }, result.Parameters);

            Assert.AreEqual("Starte \"C:\\EGUB\\EGUB.exe\".", result.Steps[0]);
            Assert.AreEqual("Warte bis Fenster <WindowAnmeldung> sichtbar ist.", result.Steps[1]);
            Assert.AreEqual("Wähle in <ListboxPinr> den Wert $piNr aus.", result.Steps[2]);
            Assert.AreEqual("Setze in <TextboxPass> den Wert $pass ein.", result.Steps[3]);
            Assert.AreEqual("Klicke den Button <ButtonAnmelden> einfach.", result.Steps[4]);
            Assert.AreEqual("Warte bis Fenster <MainWindowEgub> sichtbar ist.", result.Steps[5]);
            Assert.AreEqual("Klicke den Button <MainWindowClose> einfach.", result.Steps[6]);
            Assert.AreEqual("Warte bis Anwendung geschlossen ist.", result.Steps[7]);
        }

        [Test]
        public void FullFeaturedVersionedText_Ok() {
            // Arrange
            Stream prgStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Bumblebee.buddy.compiler.tests.resources.BuddyFullFeaturedVersionedTestCase.btc");
            if (prgStream == null) Assert.Fail("'BuddyFullFeaturedVersionedTestCase.btc' not found!");

            string testCase = new StreamReader(prgStream).ReadToEnd();

            // Act
            BuddyTextProcessor processor = new BuddyTextProcessor();
            BuddyTextInfo result = processor.ProcessText(testCase);

            // Assert
            Assert.IsNotNull(result, "result != null");
            Assert.AreEqual(8, result.Steps.Length, "result.Steps.Length != 8");

            Assert.AreEqual("EGUB", result.ApplicationText, "result.ApplicationText != 'EGUB'");
            Assert.AreEqual("9.507-*", result.VersionText, "result.VersionText != '9.507-*'");
            Assert.AreEqual("Anmeldung an EGUB", result.UseCaseText, "result.UseCaseText != 'Anmeldung an EGUB'");
            Assert.AreEqual("Erfolgreiche Anmeldung", result.ScenarioText, "result.ScenarioText != 'Erfolgreiche Anmeldung'");
            Assert.AreEqual("-", result.Precondition, "result.Precondition != '-'");
            CollectionAssert.AreEqual(new[] { new BuddyTextParameter("piNr", "\"000\""), new BuddyTextParameter("pass", "\"Passw001\""), }, result.Parameters);

            Assert.AreEqual("Starte \"C:\\EGUB\\EGUB.exe\".", result.Steps[0]);
            Assert.AreEqual("Warte bis Fenster <WindowAnmeldung> sichtbar ist.", result.Steps[1]);
            Assert.AreEqual("Wähle in <ListboxPinr> den Wert $piNr aus.", result.Steps[2]);
            Assert.AreEqual("Setze in <TextboxPass> den Wert $pass ein.", result.Steps[3]);
            Assert.AreEqual("Klicke den Button <ButtonAnmelden> einfach.", result.Steps[4]);
            Assert.AreEqual("Warte bis Fenster <MainWindowEgub> sichtbar ist.", result.Steps[5]);
            Assert.AreEqual("Klicke den Button <MainWindowClose> einfach.", result.Steps[6]);
            Assert.AreEqual("Warte bis Anwendung geschlossen ist.", result.Steps[7]);
        }

        [Test]
        public void FullFeaturedImportingText_Ok() {
            // Arrange
            Stream prgStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Bumblebee.buddy.compiler.tests.resources.BuddyFullFeaturedImportingTestCase.btc");
            if (prgStream == null) Assert.Fail("'BuddyFullFeaturedImportingTestCase.btc' not found!");

            string testCase = new StreamReader(prgStream).ReadToEnd();

            // Act
            BuddyTextProcessor processor = new BuddyTextProcessor();
            BuddyTextInfo result = processor.ProcessText(testCase);

            // Assert
            Assert.IsNotNull(result, "result != null");
            Assert.AreEqual(7, result.Steps.Length, "result.Steps.Length != 8");

            Assert.AreEqual("EGUB", result.ApplicationText, "result.ApplicationText != 'EGUB'");
            Assert.AreEqual("*", result.VersionText, "result.VersionText != '*'");
            Assert.AreEqual("Anmeldung an EGUB", result.UseCaseText, "result.UseCaseText != 'Anmeldung an EGUB'");
            Assert.AreEqual("Erfolgreiche Anmeldung", result.ScenarioText, "result.ScenarioText != 'Erfolgreiche Anmeldung'");
            Assert.AreEqual("-", result.Precondition, "result.Precondition != '-'");
            CollectionAssert.AreEqual(new[] { new BuddyTextParameter("piNr", "\"000\""), new BuddyTextParameter("pass", "\"Passw001\""), }, result.Parameters);

            Assert.AreEqual("Starte \"C:\\EGUB\\EGUB.exe\".", result.Steps[0]);
            Assert.AreEqual("Führe [Anmeldung] ($piNr, $pass) aus.", result.Steps[1]);
            Assert.AreEqual("Führe [Öffnen und Schließen] aus.", result.Steps[2]);
            Assert.AreEqual("Führe [Neustart aus dem Modul heraus] (\"Adresse\", \"Mitarbeiter\", $modul) aus.", result.Steps[3]);
            Assert.AreEqual("Warte bis Fenster <MainWindowEgub> sichtbar ist.", result.Steps[4]);
            Assert.AreEqual("Klicke den Button <MainWindowClose> einfach.", result.Steps[5]);
            Assert.AreEqual("Warte bis Anwendung geschlossen ist.", result.Steps[6]);
        }
    }
}