using System;

namespace Bumblebee.buddy.compiler.exceptions {
    /// <summary>
    /// Defines an exception that indicates that a given expression could not be evaluated during 
    /// the compiling process.
    /// </summary>
    public class ExpressionEvaluationException : Exception {
        /// <summary>
        /// Creates a new instance with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="message">Exception message</param>
        public ExpressionEvaluationException(string message) : base(message) {
            // Currently nothing to do here
        }

        /// <summary>
        /// Creates a new instance with the given <paramref name="message"/> and predecessing <paramref name="innerException"/>.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Predecessing exception</param>
        public ExpressionEvaluationException(string message, Exception innerException) : base(message, innerException) {
            // Currently nothing to do here
        }
    }
}