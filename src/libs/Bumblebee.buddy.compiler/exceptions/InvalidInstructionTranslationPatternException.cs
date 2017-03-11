using System;

namespace Bumblebee.buddy.compiler.exceptions {
    /// <summary>
    /// Indicates an error that may occur during the evaluation of a directive pattern.
    /// </summary>
    public class InvalidInstructionTranslationPatternException : BuddyCompilerException {
        /// <summary>
        /// Creates a new instance with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="message">Exception message</param>
        public InvalidInstructionTranslationPatternException(string message) : base(message) {
            // Currently nothing to do here
        }

        /// <summary>
        /// Creates a new instance with the given <paramref name="message"/> and predecessing <paramref name="innerException"/>.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Predecessing exception</param>
        public InvalidInstructionTranslationPatternException(string message, Exception innerException) : base(message, innerException) {
            // Currently nothing to do here
        }
    }
}