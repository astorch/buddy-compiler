using System;

namespace Bumblebee.buddy.compiler.exceptions {
    /// <summary>
    /// Defines an exception that indicates an error during the compilation of 
    /// buddy language unit.
    /// </summary>
    public class BuddyCompilerException : Exception {
        /// <summary>
        /// Creates a new instance with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="message">Exception message</param>
        public BuddyCompilerException(string message) : base(message) {
            // Currently nothing to to here
        }

        /// <summary>
        /// Creates a new instance with the given <paramref name="message"/> and predecessing <paramref name="innerException"/>.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Predecessing exception</param>
        public BuddyCompilerException(string message, Exception innerException) : base(message, innerException) {
            // Currently nothing to do here
        }
    }
}