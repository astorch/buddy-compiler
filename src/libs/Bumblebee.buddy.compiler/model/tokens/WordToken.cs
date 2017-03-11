using System;

namespace Bumblebee.buddy.compiler.model.tokens {
    /// <summary>
    /// Implements <see cref="IInstructionPatternToken"/> for a word token.
    /// </summary>
    public class WordToken : IInstructionPatternToken {
        /// <summary>
        /// Returns the value or does set it.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Returns TRUE if the given <paramref name="word"/> matches the token.
        /// </summary>
        /// <param name="word">Word to check</param>
        /// <returns>TRUE or FALSE</returns>
        public bool DoesMatch(string word) {
            return string.Equals(word, Value, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Returns TRUE if the token is mandatory.
        /// </summary>
        public bool IsMandatory { get { return true; } }

        /// <summary>
        /// Returns a string representation of this token that is human readable.
        /// </summary>
        /// <returns>A human readable string representation of this token</returns>
        public string ToHumanReadableString() {
            return Value;
        }
    }
}