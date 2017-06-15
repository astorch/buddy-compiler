namespace bumblebee.buddy.compiler2.exceptions {
    public class EBuddyCompileError : xcite.csharp.exceptions.EErrorReason {

        public static readonly EBuddyCompileError Unknown = new EBuddyCompileError("Unknown", 0, "Unknown error.");

        public static readonly EBuddyCompileError MissingGrammar = new EBuddyCompileError("MissingGrammar", 1, "Could not process buddy grammar file.");

        public static readonly EBuddyCompileError EmptyGrammar = new EBuddyCompileError("EmptyGrammar", 2, "Buddy grammar file is empty (or NULL).");

        public static readonly EBuddyCompileError GrammarParsingFailed = new EBuddyCompileError("GrammarParsingFailed", 3, "Error on parsing buddy grammar file.");

        public static readonly EBuddyCompileError InvalidGrammar = new EBuddyCompileError("InvalidGrammar", 4, "Constructed grammar is invalid.");

        public static readonly EBuddyCompileError InvalidRootRule = new EBuddyCompileError("InvalidRootRule", 5, "Invalid root rule. Constructed grammar is not valid.");

        public static readonly EBuddyCompileError BuddyTextParsingFailed = new EBuddyCompileError("BuddyTextParsingFailed", 6, "Error on parsing the buddy text.");

        public static readonly EBuddyCompileError EmptyBuddyText = new EBuddyCompileError("EmptyBuddyText", 7, "Buddy text to parse is empty (or NULL).");

        #region Internal

        /// <inheritdoc />
        public EBuddyCompileError(string name, int code, string hint = null) : base(name, code, hint) {
            // Nothing to do here
        }

        /// <inheritdoc />
        public override string TLC { get { return "BCE"; } }

        #endregion
    }
}