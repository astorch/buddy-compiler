using Bumblebee.buddy.compiler.model.evaluation;

namespace Bumblebee.buddy.compiler.tests.model.instructions {
    /// <summary>
    /// Provides a speciale implementation of <see cref="EvaluationResult"/> that is used to easily compare 
    /// instances of this class within unit tests.
    /// </summary>
    class _ErrorEvaluationResult : EvaluationResult {
        /// <summary>
        /// Creates a new instance with the given message.
        /// </summary>
        /// <param name="message">Message</param>
        public _ErrorEvaluationResult(string message) {
            IsError = true;
            Message = message;
        }

        /// <summary>Determines whether the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />.</summary>
        /// <returns>true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />; otherwise, false.</returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj) {
            EvaluationResult evaluationResult = (EvaluationResult) obj;

            if (!evaluationResult.IsError) return false;
            if (!Equals(Message, evaluationResult.Message)) return false;

            return true;
        }

        /// <summary>Serves as a hash function for a particular type. </summary>
        /// <returns>A hash code for the current <see cref="T:System.Object" />.</returns>
        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}