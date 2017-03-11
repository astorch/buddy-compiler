using System;

namespace Bumblebee.buddy.compiler.exceptions {
    /// <summary>
    /// Defines an exception that indicates an invalid unit name.
    /// </summary>
    public class UnitNameFormatException : Exception {
        /// <summary>Initializes a new instance of the <see cref="UnitNameFormatException" /> class with a specified error message.</summary>
        /// <param name="message">The message that describes the error. </param>
        public UnitNameFormatException(string message) : base(message) {
            // Nothing to do here
        }

        /// <summary>Initializes a new instance of the <see cref="UnitNameFormatException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
        /// <param name="message">The error message that explains the reason for the exception. </param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified. </param>
        public UnitNameFormatException(string message, Exception innerException) : base(message, innerException) {
            // Nothing to do here
        }
    }
}