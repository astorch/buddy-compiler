namespace Bumblebee.buddy.compiler.model {
    /// <summary>
    /// Defines a token of a buddy translation instruction pattern.
    /// </summary>
    public interface IInstructionPatternToken {
        /// <summary>
        /// Returns TRUE if the given <paramref name="word"/> matches the token.
        /// </summary>
        /// <param name="word">Word to check</param>
        /// <returns>TRUE or FALSE</returns>
        bool DoesMatch(string word);

        /// <summary>
        /// Returns TRUE if the token is mandatory.
        /// </summary>
        bool IsMandatory { get; }

        /// <summary>
        /// Returns a string representation of this token that is human readable.
        /// </summary>
        /// <returns>A human readable string representation of this token</returns>
        string ToHumanReadableString();
    }
}