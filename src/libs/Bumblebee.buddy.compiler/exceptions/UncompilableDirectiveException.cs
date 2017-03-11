using System;

namespace Bumblebee.buddy.compiler.exceptions {
    /// <summary>
    /// Defines an exception that indicates an error during the compilation of 
    /// buddy language unit. In detail, this error indicates that at least one directive could not be compiled.
    /// </summary>
    public class UncompilableDirectiveException : BuddyCompilerException, ILocatableError {
        /// <summary>
        /// Creates a new instance with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="message">Exception message</param>
        public UncompilableDirectiveException(string message) : base(message) {
            // Currently nothing to do here
        }

        /// <summary>
        /// Creates a new instance with the given <paramref name="message"/> and predecessing <paramref name="innerException"/>.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Predecessing exception</param>
        public UncompilableDirectiveException(string message, Exception innerException) : base(message, innerException) {
            // Currently nothing to do here
        }

        /// <summary>
        /// Returns the column index the error is located.
        /// </summary>
        public uint ColumnIndex { get; set; }

        /// <summary>
        /// Returns the line index the error is located.
        /// </summary>
        public uint LineIndex { get; set; }
    }
}