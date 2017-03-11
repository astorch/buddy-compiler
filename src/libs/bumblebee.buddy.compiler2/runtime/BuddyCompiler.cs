namespace bumblebee.buddy.compiler2.runtime {
    public class BuddyCompiler {

        // Preprocessing: doppelte Whitespaces, doppelte Linefeeds; WS vor LF; Nach Punkt Zeilenumbruch
        //                  leeren Sätze nach LF, Komma im Satz => Nur Satzanfang!

        public virtual string Compile(string buddyText) {
            BuddyTextProcessor buddyTextProcessor = new BuddyTextProcessor();
            buddyTextProcessor.ProcessText(buddyText);
            return null;
        }
    }
}