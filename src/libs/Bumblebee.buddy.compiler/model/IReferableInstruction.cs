namespace Bumblebee.buddy.compiler.model {
    /// <summary>
    /// Declares that the translated instruction returns a value that may be referenced to be used again 
    /// for other directives.
    /// </summary>
    public interface IReferableInstruction : IBuddyTranslationInstruction {
        /// <summary>
        /// Returns the pattern that is used as action result reference.
        /// </summary>
        string ResultReferencePattern { get; }
    }
}