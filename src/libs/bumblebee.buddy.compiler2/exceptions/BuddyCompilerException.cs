using System;

namespace bumblebee.buddy.compiler2.exceptions {
    /// <summary>
    /// Indicates an error during the buddy compilation process.
    /// </summary>
    [Serializable]
    public class BuddyCompilerException : Xcite.Csharp.exceptions.Xception<EBuddyCompileError> {
        /// <inheritdoc />
        public BuddyCompilerException(EBuddyCompileError errorReason) : base(errorReason) {
            // Nothing to do here
        }

        /// <inheritdoc />
        public BuddyCompilerException(EBuddyCompileError errorReason, string message) : base(errorReason, message) {
            // Nothing to do here
        }

        /// <inheritdoc />
        public BuddyCompilerException(EBuddyCompileError errorReason, string message, Exception innerException) : base(errorReason, message, innerException) {
            // Nothing to do here
        }
    }
}