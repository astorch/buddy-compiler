using System;

namespace Bumblebee.buddy.compiler.exceptions {
    /// <summary>
    /// Defines an exception that indicates the request of a translation instruction that isn't known by the translator.
    /// </summary>
    public class UnknownTranslationInstructionException : Exception {
        /// <summary>
        /// Creates a new instance with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="message">Exception message</param>
        public UnknownTranslationInstructionException(string message) : base(message) {
            // Currently nothing to do here
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="T:System.Exception"/>-Klasse mit einer angegebenen Fehlermeldung und einem Verweis auf die innere Ausnahme, die diese Ausnahme verursacht hat.
        /// </summary>
        /// <param name="message">Die Fehlermeldung, in der die Ursache der Ausnahme erklärt wird. </param><param name="innerException">Die Ausnahme, die die aktuelle Ausnahme ausgelöst hat, oder ein Nullverweis (Nothing in Visual Basic), wenn keine innere Ausnahme angegeben ist. </param>
        public UnknownTranslationInstructionException(string message, Exception innerException) : base(message, innerException) {
            // Currently nothing to do here
        }
    }
}