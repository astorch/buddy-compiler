namespace Bumblebee.buddy.compiler.simplex {
    /// <summary> Defines information about a processed buddy text. </summary>
    public class BuddyTextInfo {
        /// <summary> Returns the application text or does set it. </summary>
        public string ApplicationText { get; set; }

        /// <summary> Returns the use case text or does set it. </summary>
        public string UseCaseText { get; set; }

        /// <summary> Returns the scenario text or does set it. </summary>
        public string ScenarioText { get; set; }

        /// <summary> Returns the precondition or does set it. </summary>
        public string Precondition { get; set; }

        /// <summary> Returns the version or does set it. </summary>
        public string VersionText { get; set; }

        /// <summary> Returns the collection of action steps. </summary>
        public string[] Steps { get; set; }

        /// <summary> Returns the collection of parameters. May be empty! </summary>
        public BuddyTextParameter[] Parameters { get; set; }

        /// <summary> Returns TRUE if the buddy is written in short form. </summary>
        public bool IsShortForm {
            get { return ApplicationText == null; }
        }
    }
}