namespace Bumblebee.buddy.compiler.model.evaluation {
    /// <summary>
    /// Provides an implementation of <see cref="IInstructionEvaluationResult"/>.
    /// </summary>
    public class EvaluationResult : IInstructionEvaluationResult {
        /// <summary>
        /// Defines an evaluation result that indicates no error.
        /// </summary>
        public static readonly EvaluationResult Ok = new EvaluationResult { 
            IsError = false,
            Instruction = string.Empty,
            Message = string.Empty,
            TranslationInstruction = new DumpTranslationInstruction() // Avoid NULL pointer
        };

        /// <summary>
        /// Returns TRUE if the result indicates an error.
        /// </summary>
        public bool IsError { get; set; }

        /// <summary>
        /// Returns the evaluated instruction.
        /// </summary>
        public string Instruction { get; set; }

        /// <summary>
        /// Returns an evaluation result message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Returns the translation instruction that has been used for evaluation.
        /// </summary>
        public IBuddyTranslationInstruction TranslationInstruction { get; private set; }

        /// <summary>Determines whether the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />.</summary>
        /// <returns>true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />; otherwise, false.</returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj) {
            if (ReferenceEquals(this, obj)) return true;
            EvaluationResult evaRslt = obj as EvaluationResult;
            if (evaRslt == null) return false;

            if (!Equals(IsError, evaRslt.IsError)) return false;
            if (!Equals(Instruction, evaRslt.Instruction)) return false;
            if (!Equals(TranslationInstruction, evaRslt.TranslationInstruction)) return false;
            if (!Equals(Message, evaRslt.Message)) return false;

            return true;
        }

        /// <summary>Serves as a hash function for a particular type. </summary>
        /// <returns>A hash code for the current <see cref="T:System.Object" />.</returns>
        public override int GetHashCode() {
            int result = 31;
            result = 17*result + IsError.GetHashCode();
            result = 17*result + Instruction.GetHashCode();
            result = 17*result + TranslationInstruction.GetHashCode();
            result = 17*result + Message.GetHashCode();
            return result;
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            string result = string.Format("{0} '{1}' ({2}) [{3}]", (IsError ? "ERROR" : "OK"), Message, Instruction, TranslationInstruction);
            return result;
        }

        /// <summary>
        /// Creates an evaluation result that indicates an error.
        /// </summary>
        /// <param name="instruction">Evaluated instruction</param>
        /// <param name="translationInstruction">Translation instruction</param>
        /// <param name="message">Evaluation result message</param>
        /// <returns></returns>
        public static EvaluationResult Error(string instruction, IBuddyTranslationInstruction translationInstruction, string message) {
            return new EvaluationResult {
                IsError = true,
                Instruction = instruction,
                TranslationInstruction = translationInstruction,
                Message = message
            };
        }

        /// <summary>
        /// Anonymous implementation of <see cref="IBuddyTranslationInstruction"/> to avoid NULL pointers when 
        /// working with <see cref="EvaluationResult.Ok"/>.
        /// </summary>
        class DumpTranslationInstruction : IBuddyTranslationInstruction {
            /// <summary>
            /// Returns id of the directive.
            /// </summary>
            public string InstructionId { get { return string.Empty; } }

            /// <summary>
            /// Returns the pattern of the instruction.
            /// </summary>
            public string InstructionPattern { get { return string.Empty; } }

            /// <summary>
            /// Returns the resulting TDIL pattern of the directive.
            /// </summary>
            public string TdilPattern { get { return string.Empty; } }
        }
    }
}