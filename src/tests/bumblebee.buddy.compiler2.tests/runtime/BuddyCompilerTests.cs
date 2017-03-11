using System.Text;
using bumblebee.buddy.compiler2.runtime;
using NUnit.Framework;

namespace bumblebee.buddy.compiler2.tests.runtime {
    [TestFixture]
    public class BuddyCompilerTests {
        [Test]
        public void Compile() {
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
            new BuddyCompiler().Compile(buddyText);
        }
    }
}
