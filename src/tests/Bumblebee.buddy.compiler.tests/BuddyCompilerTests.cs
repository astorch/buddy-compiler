using System;
using System.Text;
using Bumblebee.buddy.compiler.packaging;
using Bumblebee.buddy.compiler.simplex;
using NUnit.Framework;

namespace Bumblebee.buddy.compiler.tests {
    [TestFixture]
    public class BuddyCompilerTests {

        [Test]
        public void Compile_Ok_FullFeatured() {
            // Arrange
            StringBuilder buddyTextBuilder = new StringBuilder();
            buddyTextBuilder
                .AppendLine("Anwendung: Verwaltung (9.511-*)")
                .AppendLine()
                .AppendLine("Anwendungsfall: Compilertest")
                .AppendLine()
                .AppendLine("Szenario: Fully Featured Test (p1 = \"wert1\", p2 = \"18\")")
                .AppendLine("benutzt Verwaltung.s.paket.NullEins")
                .AppendLine("benutzt Verwaltung.9d511us.paket.Nullzwo")
                .AppendLine()
                .AppendLine("Schritte:")
                .AppendLine("Führe [Null eins in Zwo bei dem] ($p1, $p2) aus.")
                .AppendLine("Führe [Null eins in Zwo bei dem] ($p1, \"test\") aus.")
                .AppendLine("Wähle \"000\" in <ListboxPinr> aus.")
                .AppendLine("Wähle $p2 in <ListboxPinr> aus.")
                .AppendLine("Setze \"Passw000\" in <TextboxPass> ein.")
                .AppendLine("Drücke STRG+S einmal.");

            string buddyText = buddyTextBuilder.ToString();
            
            // Act
            BuddyCompiler buddyCompiler = new BuddyCompiler {ImportPathProvider = new _ImportPathProviderImpl()};
            DateTime now = DateTime.Now;
            string compiledTdilText = buddyCompiler.Compile(buddyText);

            // Assert
            Assert.IsNotNullOrEmpty(compiledTdilText);

            StringBuilder expectedTdilTextBuilder = new StringBuilder();
            expectedTdilTextBuilder
                .AppendLine("// Compiler generated file")
                .AppendLine("// Buddy Compiler version 0.1.1")
                .AppendLine("// Generated on {date}")
                .AppendLine()
                .AppendLine("#alias \"ListboxPinr\"")
                .AppendLine("#alias \"TextboxPass\"")
                .AppendLine()
                .AppendLine("#include \"test.unkown.none.Null_eins_in_Zwo_bei_dem\"")
                .AppendLine()
                .AppendLine("Unit Verwaltung.9d511us.Compilertest.Fully_Featured_Test")
                .AppendLine()
                .AppendLine("Main:")
                .AppendLine("p1 = \"wert1\"")
                .AppendLine("p2 = \"18\"")
                .AppendLine("start(,, \"{Verwaltung}\")")
                .AppendLine("gosub Fully_Featured_Test:(p1, p2)")
                .AppendLine("close(_Application,, Default)")
                .AppendLine("kill(_Application,, 3000)")
                .AppendLine("close(\"AcroRd32\",, Default)")
                .AppendLine("kill(\"AcroRd32\",, 3000)")
                .AppendLine("End")
                .AppendLine()
                .AppendLine("Fully_Featured_Test:(p1, p2)")
                .AppendLine("gosub Null_eins_in_Zwo_bei_dem:(p1, p2)")
                .AppendLine("gosub Null_eins_in_Zwo_bei_dem:(p1, \"test\")")
                .AppendLine("select(ListboxPinr, Value, \"000\")")
                .AppendLine("select(ListboxPinr, Value, p2)")
                .AppendLine("set(TextboxPass, Text, \"Passw000\")")
                .AppendLine("pressKey(, KeyType, \"STRG+S\", Single)")
                .AppendLine("End")
                .AppendLine()
                .AppendLine("End")
                .AppendLine();

            string expectedTdilText = expectedTdilTextBuilder.ToString();
            expectedTdilText = expectedTdilText.Replace("{date}", now.ToString());

            Assert.AreEqual(expectedTdilText, compiledTdilText, "compiledTdilText");
        }

        [Test]
        public void Compile_Ok_StartSelectSet() {
            // Arrange
            StringBuilder buddyTextBuilder = new StringBuilder();
            buddyTextBuilder
                .AppendLine("Schritte:")
                .AppendLine("Starte \"C:\\EGUB\\EGUB.exe\".")
                .AppendLine("Wähle \"000\" in <ListboxPinr> aus.")
                .AppendLine("Setze \"Passw000\" in <TextboxPass> ein.");

            string buddyText = buddyTextBuilder.ToString();
            
            // Act
            BuddyCompiler buddyCompiler = new BuddyCompiler();
            DateTime now = DateTime.Now;
            string compiledTdilText = buddyCompiler.Compile(buddyText);

            // Assert
            Assert.IsNotNullOrEmpty(compiledTdilText);

            StringBuilder expectedTdilTextBuilder = new StringBuilder();
            expectedTdilTextBuilder
                .AppendLine("// Compiler generated file")
                .AppendLine("// Buddy Compiler version 0.1.1")
                .AppendLine("// Generated on {date}")
                .AppendLine()
                .AppendLine("#alias \"ListboxPinr\"")
                .AppendLine("#alias \"TextboxPass\"")
                .AppendLine()
                .AppendLine("Unit untitled.s.untitled.untitled")
                .AppendLine()
                .AppendLine("Main:")
                .AppendLine("start(,, \"{untitled}\")")
                .AppendLine("gosub untitled:")
                .AppendLine("close(_Application,, Default)")
                .AppendLine("kill(_Application,, 3000)")
                .AppendLine("close(\"AcroRd32\",, Default)")
                .AppendLine("kill(\"AcroRd32\",, 3000)")
                .AppendLine("End")
                .AppendLine()
                .AppendLine("untitled:")
                .AppendLine("processHandle1 = start(,, \"C:\\EGUB\\EGUB.exe\")")
                .AppendLine("select(ListboxPinr, Value, \"000\")")
                .AppendLine("set(TextboxPass, Text, \"Passw000\")")
                .AppendLine("End")
                .AppendLine()
                .AppendLine("End")
                .AppendLine();

            string expectedTdilText = expectedTdilTextBuilder.ToString();
            expectedTdilText = expectedTdilText.Replace("{date}", now.ToString());

            Assert.AreEqual(expectedTdilText, compiledTdilText, "compiledTdilText");
        }
        
        [Test]
        public void Compile_Ok_StartSelectSetSwitch() {
            // Arrange
            StringBuilder buddyTextBuilder = new StringBuilder();
            buddyTextBuilder
                .AppendLine("Schritte:")
                .AppendLine("Starte \"C:\\EGUB\\EGUB.exe\".")
                .AppendLine("Wähle \"000\" in <ListboxPinr> aus.")
                .AppendLine("Setze \"Passw000\" in <TextboxPass> ein.")
                .AppendLine("Wechsle zu \"HIT\".");

            string buddyText = buddyTextBuilder.ToString();
            
            // Act
            BuddyCompiler buddyCompiler = new BuddyCompiler();
            DateTime now = DateTime.Now;
            string compiledTdilText = buddyCompiler.Compile(buddyText);

            // Assert
            Assert.IsNotNullOrEmpty(compiledTdilText);

            StringBuilder expectedTdilTextBuilder = new StringBuilder();
            expectedTdilTextBuilder
                .AppendLine("// Compiler generated file")
                .AppendLine("// Buddy Compiler version 0.1.1")
                .AppendLine("// Generated on {date}")
                .AppendLine()
                .AppendLine("#alias \"ListboxPinr\"")
                .AppendLine("#alias \"TextboxPass\"")
                .AppendLine()
                .AppendLine("Unit untitled.s.untitled.untitled")
                .AppendLine()
                .AppendLine("Main:")
                .AppendLine("start(,, \"{untitled}\")")
                .AppendLine("gosub untitled:")
                .AppendLine("close(_Application,, Default)")
                .AppendLine("kill(_Application,, 3000)")
                .AppendLine("close(\"AcroRd32\",, Default)")
                .AppendLine("kill(\"AcroRd32\",, 3000)")
                .AppendLine("End")
                .AppendLine()
                .AppendLine("untitled:")
                .AppendLine("processHandle1 = start(,, \"C:\\EGUB\\EGUB.exe\")")
                .AppendLine("select(ListboxPinr, Value, \"000\")")
                .AppendLine("set(TextboxPass, Text, \"Passw000\")")
                .AppendLine("setsut(,, \"HIT\")")
                .AppendLine("End")
                .AppendLine()
                .AppendLine("End")
                .AppendLine();

            string expectedTdilText = expectedTdilTextBuilder.ToString();
            expectedTdilText = expectedTdilText.Replace("{date}", now.ToString());

            Assert.AreEqual(expectedTdilText, compiledTdilText, "compiledTdilText");
        }

        [Test]
        public void Compile_Ok_EgubSampleFull() {
            // Arrange
            StringBuilder buddyTextBuilder = new StringBuilder();
            buddyTextBuilder
                .AppendLine("Schritte:")
                .AppendLine("Starte \"C:\\EGUB\\EGUB.exe\".")
                .AppendLine("Warte bis <WindowAnmeldung> sichtbar ist.")
                .AppendLine("Wähle \"000\" in <ListboxPinr> aus.")
                .AppendLine("Setze \"Passw000\" in <TextboxPass> ein.")
                .AppendLine("Klicke den <ButtonAnmelden> einfach.")
                .AppendLine("Warte bis <MainWindowEgub> sichtbar ist.")
                .AppendLine("Klicke den <ButtonMainWindowClose> einfach.")
                .AppendLine("Warte bis <MainWindowEgub> geschlossen ist.");

            string buddyText = buddyTextBuilder.ToString();

            // Act
            BuddyCompiler buddyCompiler = new BuddyCompiler();
            DateTime now = DateTime.Now;
            string compiledTdilText = buddyCompiler.Compile(buddyText);

            // Assert
            Assert.IsNotNullOrEmpty(compiledTdilText);

            StringBuilder expectedTdilTextBuilder = new StringBuilder();
            expectedTdilTextBuilder
                .AppendLine("// Compiler generated file")
                .AppendLine("// Buddy Compiler version 0.1.1")
                .AppendLine("// Generated on {date}")
                .AppendLine()
                .AppendLine("#alias \"WindowAnmeldung\"")
                .AppendLine("#alias \"ListboxPinr\"")
                .AppendLine("#alias \"TextboxPass\"")
                .AppendLine("#alias \"ButtonAnmelden\"")
                .AppendLine("#alias \"MainWindowEgub\"")
                .AppendLine("#alias \"ButtonMainWindowClose\"")
                .AppendLine()
                .AppendLine("Unit untitled.s.untitled.untitled")
                .AppendLine()
                .AppendLine("Main:")
                .AppendLine("start(,, \"{untitled}\")")
                .AppendLine("gosub untitled:")
                .AppendLine("close(_Application,, Default)")
                .AppendLine("kill(_Application,, 3000)")
                .AppendLine("close(\"AcroRd32\",, Default)")
                .AppendLine("kill(\"AcroRd32\",, 3000)")
                .AppendLine("End")
                .AppendLine()
                .AppendLine("untitled:")
                .AppendLine("processHandle1 = start(,, \"C:\\EGUB\\EGUB.exe\")")
                .AppendLine("wait(WindowAnmeldung, IsVisible, True, 60000)")
                .AppendLine("select(ListboxPinr, Value, \"000\")")
                .AppendLine("set(TextboxPass, Text, \"Passw000\")")
                .AppendLine("click(ButtonAnmelden, , Single)")
                .AppendLine("wait(MainWindowEgub, IsVisible, True, 60000)")
                .AppendLine("click(ButtonMainWindowClose, , Single)")
                .AppendLine("wait(MainWindowEgub, Close, True, 60000)")
                .AppendLine("End")
                .AppendLine()
                .AppendLine("End")
                .AppendLine();

            string expectedTdilText = expectedTdilTextBuilder.ToString();
            expectedTdilText = expectedTdilText.Replace("{date}", now.ToString());

            Assert.AreEqual(expectedTdilText, compiledTdilText, "compiledTdilText");
        }

        [Test]
        public void Compile_Ok_OneLiner() {
            // Arrange
            StringBuilder buddyTextBuilder = new StringBuilder();
            buddyTextBuilder
                .AppendLine("Anwendung: Verwaltung")
                .AppendLine()
                .AppendLine("Anwendungsfall: Anmeldung")
                .AppendLine()
                .AppendLine("Szenario: Erfolgreiche Anmeldung (piNr = \"001\", pass = \"\")")
                .AppendLine()
                .AppendLine("Vorbedingung: -")
                .AppendLine()
                .AppendLine("Schritte:")
                .AppendLine("Starte \"C:\\EGUB\\Verwaltung.exe\".")
                ;

            string buddyText = buddyTextBuilder.ToString();

            // Act 
            BuddyCompiler buddyCompiler = new BuddyCompiler();
            DateTime now = DateTime.Now;
            string compiledTdilText = buddyCompiler.Compile(buddyText);

            // Assert
            Assert.IsNotNullOrEmpty(compiledTdilText);

            StringBuilder expectedTdilTextBuilder = new StringBuilder();
            expectedTdilTextBuilder
                .AppendLine("// Compiler generated file")
                .AppendLine("// Buddy Compiler version 0.1.1")
                .AppendLine("// Generated on {date}")
                .AppendLine()
                .AppendLine("Unit Verwaltung.s.Anmeldung.Erfolgreiche_Anmeldung")
                .AppendLine()
                .AppendLine("Main:")
                .AppendLine("piNr = \"001\"")
                .AppendLine("pass = \"\"")
                .AppendLine("start(,, \"{Verwaltung}\")")
                .AppendLine("gosub Erfolgreiche_Anmeldung:(piNr, pass)")
                .AppendLine("close(_Application,, Default)")
                .AppendLine("kill(_Application,, 3000)")
                .AppendLine("close(\"AcroRd32\",, Default)")
                .AppendLine("kill(\"AcroRd32\",, 3000)")
                .AppendLine("End")
                .AppendLine()
                .AppendLine("Erfolgreiche_Anmeldung:(piNr, pass)")
                .AppendLine("processHandle1 = start(,, \"C:\\EGUB\\Verwaltung.exe\")")
                .AppendLine("End")
                .AppendLine()
                .AppendLine("End")
                .AppendLine();

            string expectedTdilText = expectedTdilTextBuilder.ToString();
            expectedTdilText = expectedTdilText.Replace("{date}", now.ToString());

            Assert.AreEqual(expectedTdilText, compiledTdilText, "compiledTdilText");
        }

        [Test]
        public void Compile_Ok_SimpleAssert() {
            // Arrange
            StringBuilder buddyTextBuilder = new StringBuilder();
            buddyTextBuilder
                .AppendLine("Anwendung: Verwaltung")
                .AppendLine()
                .AppendLine("Anwendungsfall: Anmeldung")
                .AppendLine()
                .AppendLine("Szenario: Erfolgreiche Anmeldung (piNr = \"001\", pass = \"\")")
                .AppendLine()
                .AppendLine("Vorbedingung: -")
                .AppendLine()
                .AppendLine("Schritte:")
                .AppendLine("Prüfen, ob <ButtonAnmelden> auswählbar ist.")
                ;

            string buddyText = buddyTextBuilder.ToString();

            // Act 
            BuddyCompiler buddyCompiler = new BuddyCompiler();
            DateTime now = DateTime.Now;
            string compiledTdilText = buddyCompiler.Compile(buddyText);

            // Assert
            Assert.IsNotNullOrEmpty(compiledTdilText);

            StringBuilder expectedTdilTextBuilder = new StringBuilder();
            expectedTdilTextBuilder
                .AppendLine("// Compiler generated file")
                .AppendLine("// Buddy Compiler version 0.1.1")
                .AppendLine("// Generated on {date}")
                .AppendLine()
                .AppendLine("#alias \"ButtonAnmelden\"")
                .AppendLine()
                .AppendLine("Unit Verwaltung.s.Anmeldung.Erfolgreiche_Anmeldung")
                .AppendLine()
                .AppendLine("Main:")
                .AppendLine("piNr = \"001\"")
                .AppendLine("pass = \"\"")
                .AppendLine("start(,, \"{Verwaltung}\")")
                .AppendLine("gosub Erfolgreiche_Anmeldung:(piNr, pass)")
                .AppendLine("close(_Application,, Default)")
                .AppendLine("kill(_Application,, 3000)")
                .AppendLine("close(\"AcroRd32\",, Default)")
                .AppendLine("kill(\"AcroRd32\",, 3000)")
                .AppendLine("End")
                .AppendLine()
                .AppendLine("Erfolgreiche_Anmeldung:(piNr, pass)")
                .AppendLine("assert(ButtonAnmelden, IsEnabled, True)")
                .AppendLine("End")
                .AppendLine()
                .AppendLine("End")
                .AppendLine();

            string expectedTdilText = expectedTdilTextBuilder.ToString();
            expectedTdilText = expectedTdilText.Replace("{date}", now.ToString());

            Assert.AreEqual(expectedTdilText, compiledTdilText, "compiledTdilText");
        }

        [Test]
        public void StripArticles_Ok() {
            // Arrange
            StringBuilder buddyTextBuilder = new StringBuilder();
            buddyTextBuilder
                .AppendLine("Starte \"C:\\EGUB\\EGUB.exe\".")
                .AppendLine("Warte bis Fenster <WindowAnmeldung> sichtbar ist.")
                .AppendLine("Wähle in <ListboxPinr> den Wert \"000\" aus.")
                .AppendLine("Setze in <TextboxPass> den Wert \"Passw000\" ein.")
                .AppendLine("Klicke den Button <ButtonAnmelden> einfach.")
                .AppendLine("Warte bis Fenster <MainWindowEgub> sichtbar ist.")
                .AppendLine("Klicke den Button <ButtonMainWindowClose> einfach.")
                .AppendLine("Warte bis das Fenster <MainWindowEgub> geschlossen ist.");

            string buddyText = buddyTextBuilder.ToString();

            // Act
            BuddyCompiler buddyCompiler = new BuddyCompiler();
            string strippedBuddyText = buddyCompiler.StripArticles(buddyText);

            // Assert
            Assert.IsNotNullOrEmpty(strippedBuddyText);

            StringBuilder expectedStrippedBuddyText = new StringBuilder();
            expectedStrippedBuddyText
                .AppendLine("Starte \"C:\\EGUB\\EGUB.exe\".")
                .AppendLine("Warte bis Fenster <WindowAnmeldung> sichtbar ist.")
                .AppendLine("Wähle in <ListboxPinr> Wert \"000\" aus.")
                .AppendLine("Setze in <TextboxPass> Wert \"Passw000\" ein.")
                .AppendLine("Klicke Button <ButtonAnmelden> einfach.")
                .AppendLine("Warte bis Fenster <MainWindowEgub> sichtbar ist.")
                .AppendLine("Klicke Button <ButtonMainWindowClose> einfach.")
                .AppendLine("Warte bis Fenster <MainWindowEgub> geschlossen ist.");

            string expectedTdilText = expectedStrippedBuddyText.ToString();

            Assert.AreEqual(expectedTdilText, strippedBuddyText, "strippedBuddyText");
        }

        [Test]
        public void StripPrepositions_Ok() {
            // Arrange
            StringBuilder buddyTextBuilder = new StringBuilder();
            buddyTextBuilder
                .AppendLine("Starte \"C:\\EGUB\\EGUB.exe\".")
                .AppendLine("Warte bis Fenster <WindowAnmeldung> sichtbar ist.")
                .AppendLine("Wähle in <ListboxPinr> den Wert \"000\" aus.")
                .AppendLine("Setze in <TextboxPass> den Wert \"Passw000\" ein.")
                .AppendLine("Klicke den Button <ButtonAnmelden> einfach.")
                .AppendLine("Warte bis Fenster <MainWindowEgub> sichtbar ist.")
                .AppendLine("Klicke den Button <ButtonMainWindowClose> einfach.")
                .AppendLine("Warte bis das Fenster <MainWindowEgub> geschlossen ist.");

            string buddyText = buddyTextBuilder.ToString();

            // Act
            BuddyCompiler buddyCompiler = new BuddyCompiler();
            string strippedBuddyText = buddyCompiler.StripPrepositions(buddyText);

            // Assert
            Assert.IsNotNullOrEmpty(strippedBuddyText);

            StringBuilder expectedStrippedBuddyText = new StringBuilder();
            expectedStrippedBuddyText
                .AppendLine("Starte \"C:\\EGUB\\EGUB.exe\".")
                .AppendLine("Warte Fenster <WindowAnmeldung> sichtbar ist.")
                .AppendLine("Wähle <ListboxPinr> den Wert \"000\".")
                .AppendLine("Setze <TextboxPass> den Wert \"Passw000\".")
                .AppendLine("Klicke den Button <ButtonAnmelden> einfach.")
                .AppendLine("Warte Fenster <MainWindowEgub> sichtbar ist.")
                .AppendLine("Klicke den Button <ButtonMainWindowClose> einfach.")
                .AppendLine("Warte das Fenster <MainWindowEgub> geschlossen ist.");

            string expectedTdilText = expectedStrippedBuddyText.ToString();

            Assert.AreEqual(expectedTdilText, strippedBuddyText, "strippedBuddyText");
        }

        [Test]
        public void StripAuxiliaryVerbs_Ok() {
            // Arrange
            StringBuilder buddyTextBuilder = new StringBuilder();
            buddyTextBuilder
                .AppendLine("Starte \"C:\\EGUB\\EGUB.exe\".")
                .AppendLine("Warte bis Fenster <WindowAnmeldung> sichtbar ist.")
                .AppendLine("Wähle in <ListboxPinr> den Wert \"000\" aus.")
                .AppendLine("Setze in <TextboxPass> den Wert \"Passw000\" ein.")
                .AppendLine("Klicke den Button <ButtonAnmelden> einfach.")
                .AppendLine("Warte bis Fenster <MainWindowEgub> sichtbar ist.")
                .AppendLine("Klicke den Button <ButtonMainWindowClose> einfach.")
                .AppendLine("Warte bis das Fenster <MainWindowEgub> geschlossen ist.");

            string buddyText = buddyTextBuilder.ToString();

            // Act
            BuddyCompiler buddyCompiler = new BuddyCompiler();
            string strippedBuddyText = buddyCompiler.StripAuxiliaryVerbs(buddyText);

            // Assert
            Assert.IsNotNullOrEmpty(strippedBuddyText);

            StringBuilder expectedStrippedBuddyText = new StringBuilder();
            expectedStrippedBuddyText
                .AppendLine("Starte \"C:\\EGUB\\EGUB.exe\".")
                .AppendLine("Warte bis Fenster <WindowAnmeldung> sichtbar.")
                .AppendLine("Wähle in <ListboxPinr> den Wert \"000\" aus.")
                .AppendLine("Setze in <TextboxPass> den Wert \"Passw000\" ein.")
                .AppendLine("Klicke den Button <ButtonAnmelden> einfach.")
                .AppendLine("Warte bis Fenster <MainWindowEgub> sichtbar.")
                .AppendLine("Klicke den Button <ButtonMainWindowClose> einfach.")
                .AppendLine("Warte bis das Fenster <MainWindowEgub> geschlossen.");

            string expectedTdilText = expectedStrippedBuddyText.ToString();

            Assert.AreEqual(expectedTdilText, strippedBuddyText, "strippedBuddyText");
        }

        [Test]
        public void StripPunctuationMarks_Ok() {
            // Arrange
            StringBuilder buddyTextBuilder = new StringBuilder();
            buddyTextBuilder
                .AppendLine("Starte \"C:\\EGUB\\EGUB.exe\".")
                .AppendLine("Warte, bis Fenster <WindowAnmeldung> sichtbar ist.")
                .AppendLine("Wähle in <ListboxPinr> den Wert \"000\" aus.")
                .AppendLine("Setze in <TextboxPass> den Wert \"Passw000\" ein.")
                .AppendLine("Klicke den Button <ButtonAnmelden> einfach.")
                .AppendLine("Warte, bis Fenster <MainWindowEgub> sichtbar ist.")
                .AppendLine("Klicke den Button <ButtonMainWindowClose> einfach.")
                .AppendLine("Warte, bis das Fenster <MainWindowEgub> geschlossen ist.")
                .AppendLine("Wähle den Wert \"Adresse lesen, bearbeiten\" in <ListBox:RechteAdressen> aus.")
                .AppendLine("Führe [Meine Daten bearbeiten] ($pi, $name) aus.")
                .AppendLine("Führe [Meine Daten bearbeiten] ($pi, \"test, test\") aus.")
                ;

            string buddyText = buddyTextBuilder.ToString();

            // Act
            BuddyCompiler buddyCompiler = new BuddyCompiler();
            string strippedBuddyText = buddyCompiler.StripPunctuationMarks(buddyText);

            // Assert
            Assert.IsNotNullOrEmpty(strippedBuddyText);

            StringBuilder expectedStrippedBuddyText = new StringBuilder();
            expectedStrippedBuddyText
                .AppendLine("Starte \"C:\\EGUB\\EGUB.exe\".")
                .AppendLine("Warte bis Fenster <WindowAnmeldung> sichtbar ist.")
                .AppendLine("Wähle in <ListboxPinr> den Wert \"000\" aus.")
                .AppendLine("Setze in <TextboxPass> den Wert \"Passw000\" ein.")
                .AppendLine("Klicke den Button <ButtonAnmelden> einfach.")
                .AppendLine("Warte bis Fenster <MainWindowEgub> sichtbar ist.")
                .AppendLine("Klicke den Button <ButtonMainWindowClose> einfach.")
                .AppendLine("Warte bis das Fenster <MainWindowEgub> geschlossen ist.")
                .AppendLine("Wähle den Wert \"Adresse lesen, bearbeiten\" in <ListBox:RechteAdressen> aus.")
                .AppendLine("Führe [Meine Daten bearbeiten] ($pi, $name) aus.")
                .AppendLine("Führe [Meine Daten bearbeiten] ($pi, \"test, test\") aus.")
                ;

            string expectedTdilText = expectedStrippedBuddyText.ToString();

            Assert.AreEqual(expectedTdilText, strippedBuddyText, "strippedBuddyText");
        }

        [Test]
        public void ResolveAmbiguity_Ok() {
            // Arrange
            StringBuilder buddyTextBuilder = new StringBuilder();
            buddyTextBuilder
                .AppendLine("Starte \"C:\\EGUB\\EGUB.exe\".")
                .AppendLine("Warte bis Fenster <WindowAnmeldung> sichtbar ist.")
                .AppendLine("Wähle in <ListboxPinr> den Wert \"000\" aus.")
                .AppendLine("Setze in <TextboxPass> den Wert \"Passw000\" ein.")
                .AppendLine("Klicke die Schaltfläche <ButtonAnmelden> einfach.")
                .AppendLine("Warte bis Fenster <MainWindowEgub> sichtbar ist.")
                .AppendLine("Klick den Button <ButtonMainWindowClose> einfach.")
                .AppendLine("Warte bis das Fenster <MainWindowEgub> geschlossen ist.")
                .AppendLine("Warte bis das Fenster <MainWindowEgub> nicht sichtbar ist.");

            string buddyText = buddyTextBuilder.ToString();

            // Act
            BuddyCompiler buddyCompiler = new BuddyCompiler();
            string unambigiousBuddyText = buddyCompiler.ResolveAmbiguity(buddyText);

            // Assert
            Assert.IsNotNullOrEmpty(unambigiousBuddyText);

            StringBuilder expectedUnambigiousBuddyText = new StringBuilder();
            expectedUnambigiousBuddyText
                .AppendLine("Starte \"C:\\EGUB\\EGUB.exe\".")
                .AppendLine("Warte bis Fenster <WindowAnmeldung> sichtbar ist.")
                .AppendLine("Wähle in <ListboxPinr> den Wert \"000\" aus.")
                .AppendLine("Setze in <TextboxPass> den Wert \"Passw000\" ein.")
                .AppendLine("Klicke die Button <ButtonAnmelden> einfach.")
                .AppendLine("Warte bis Fenster <MainWindowEgub> sichtbar ist.")
                .AppendLine("Klicke den Button <ButtonMainWindowClose> einfach.")
                .AppendLine("Warte bis das Fenster <MainWindowEgub> geschlossen ist.")
                .AppendLine("Warte bis das Fenster <MainWindowEgub> verschwunden ist.");

            string expectedTdilText = expectedUnambigiousBuddyText.ToString();

            Assert.AreEqual(expectedTdilText, unambigiousBuddyText, "expectedUnambigiousBuddyText");
        }

        [Test]
        public void Normalize_Ok() {
            // Arrange
            StringBuilder buddyTextBuilder = new StringBuilder();
            buddyTextBuilder
                .AppendLine("Starte \"C:\\EGUB\\EGUB.exe\".")
                .AppendLine("Warte bis Fenster <WindowAnmeldung> sichtbar ist.")
                .AppendLine("Wähle in <ListboxPinr> den Wert \"000\" aus.")
                .AppendLine("Setze in <TextboxPass> den Wert \"Passw000\" ein.")
                .AppendLine("Klicke die Schaltfläche <ButtonAnmelden> einfach.")
                .AppendLine("Warte bis Fenster <MainWindowEgub> sichtbar ist.")
                .AppendLine("Klick den Button <ButtonMainWindowClose> einfach.")
                .AppendLine("Warte bis das Fenster <MainWindowEgub> geschlossen ist.");

            string buddyText = buddyTextBuilder.ToString();

            // Act
            BuddyCompiler buddyCompiler = new BuddyCompiler();
            string normalizeBuddyText = buddyCompiler.Normalize(buddyText);

            // Assert
            Assert.IsNotNullOrEmpty(normalizeBuddyText);

            StringBuilder expectedUnambigiousBuddyText = new StringBuilder();
            expectedUnambigiousBuddyText
                .Append("Starte \"C:\\EGUB\\EGUB.exe\".").Append('\n')
                .Append("Warte Fenster <WindowAnmeldung> sichtbar.").Append('\n')
                .Append("Wähle <ListboxPinr> \"000\".").Append('\n')
                .Append("Setze <TextboxPass> \"Passw000\".").Append('\n')
                .Append("Klicke <ButtonAnmelden> einfach.").Append('\n')
                .Append("Warte Fenster <MainWindowEgub> sichtbar.").Append('\n')
                .Append("Klicke <ButtonMainWindowClose> einfach.").Append('\n')
                .Append("Warte Fenster <MainWindowEgub> geschlossen.").Append('\n');

            string expectedBuddyText = expectedUnambigiousBuddyText.ToString();

            Assert.AreEqual(expectedBuddyText, normalizeBuddyText, "expectedBuddyText != normalizedBuddyText");
        }

        [Test]
        public void NormalizeSteps_Ok() {
            // Arrange
            BuddyTextParameter p1 = new BuddyTextParameter("piNr", "\"001\"");
            BuddyTextParameter p2 = new BuddyTextParameter("piPass", "\"Passw001\"");

            StringBuilder buddyTextBuilder = new StringBuilder();
            buddyTextBuilder
                .AppendLine("Starte \"C:\\EGUB\\EGUB.exe\".")
                .AppendLine("Warte bis Fenster <WindowAnmeldung> sichtbar ist.")
                .AppendLine("Wähle in <ListboxPinr> den Wert $piNr aus.")
                .AppendLine("Setze in <TextboxPass> den Wert $piPass ein.")
                .AppendLine("Klicke die Schaltfläche <ButtonAnmelden> einfach.")
                .AppendLine("Warte bis Fenster <MainWindowEgub> sichtbar ist.")
                .AppendLine("Klick den Button <ButtonMainWindowClose> einfach.")
                .AppendLine("Warte bis das Fenster <MainWindowEgub> geschlossen ist.")
                .AppendLine("Führe [Neustart, ohne Foto] (\"5,12\", \"a, b\", $a, $b) aus.")
                ;

            string buddyText = buddyTextBuilder.ToString();
            string[] buddyTextSteps = buddyText.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);

            // Act
            BuddyCompiler buddyCompiler = new BuddyCompiler();
            string[] normalizedBuddyTextSteps = buddyCompiler.NormalizeSteps(buddyTextSteps, new[] {p1, p2});

            // Assert
            Assert.IsNotNull(normalizedBuddyTextSteps, "normalizedBuddyTextSteps != null");
            Assert.AreEqual(buddyTextSteps.Length, normalizedBuddyTextSteps.Length, "Count of steps");

            StringBuilder expectedUnambigiousBuddyText = new StringBuilder();
            expectedUnambigiousBuddyText
                .Append("Starte \"C:\\EGUB\\EGUB.exe\".").Append("\r\n")
                .Append("Warte Fenster <WindowAnmeldung> sichtbar.").Append("\r\n")
                .Append("Wähle <ListboxPinr> $piNr.").Append("\r\n")
                .Append("Setze <TextboxPass> $piPass.").Append("\r\n")
                .Append("Klicke <ButtonAnmelden> einfach.").Append("\r\n")
                .Append("Warte Fenster <MainWindowEgub> sichtbar.").Append("\r\n")
                .Append("Klicke <ButtonMainWindowClose> einfach.").Append("\r\n")
                .Append("Warte Fenster <MainWindowEgub> geschlossen.").Append("\r\n")
                .AppendLine("Führe [Neustart, ohne Foto] (\"5,12\", \"a, b\", $a, $b).").Append("\r\n")
                ;

            string expectedTdilText = expectedUnambigiousBuddyText.ToString();
            string[] expectedNormalizedBuddyTextSteps = expectedTdilText.Split(new []{"\r\n"}, StringSplitOptions.RemoveEmptyEntries);

            Assert.AreEqual(expectedNormalizedBuddyTextSteps.Length, normalizedBuddyTextSteps.Length, "Expected count of steps");

            for (int i = -1; ++i != expectedNormalizedBuddyTextSteps.Length;) {
                string normalizedStep = normalizedBuddyTextSteps[i];
                string expectedStep = expectedNormalizedBuddyTextSteps[i];
                Assert.AreEqual(expectedStep, normalizedStep, "expectedStep != normalizedStep");
            }
        }

        class _ImportPathProviderImpl : IImportPathProvider {
            /// <inheritdoc />
            public string GetPath(UnitName unitName) {
                return string.Format("test://{0}", unitName.ToQualifiedString());
            }

            /// <inheritdoc />
            public UnitName GetUnitName(string scenarioName) {
                return new UnitName("test", "unkown", "none", scenarioName);
            }
        }
    }
}