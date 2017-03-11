using System;

namespace Bumblebee.buddy.compiler.exceptions {
    /// <summary>
    /// Defines a base implementation of exceptions in the context of the buddy compiler
    /// </summary>
    [Serializable]
    public abstract class BuddyCompilerBaseException : Exception {
        /// <summary>
        /// Creates a new instance with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="message">Exception message</param>
        public BuddyCompilerBaseException(string message) : base(message) {
            // Currently nothing to do here
        }

        /// <summary>
        /// Creates a new instance with the given <paramref name="message"/> and predecessing <paramref name="innerException"/>.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Predecessing exception</param>
        public BuddyCompilerBaseException(string message, Exception innerException) : base(message, innerException) {
            // Currently nothing to do here
        }
    }
}