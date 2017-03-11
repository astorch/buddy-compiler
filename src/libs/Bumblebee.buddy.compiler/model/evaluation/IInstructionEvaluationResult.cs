namespace Bumblebee.buddy.compiler.model.evaluation {
    /// <summary>
    /// Describes the result of an instruction evaluation.
    /// </summary>
    public interface IInstructionEvaluationResult {
        /// <summary>
        /// Returns TRUE if the result indicates an error.
        /// </summary>
        bool IsError { get; }

        /// <summary>
        /// Returns the evaluated instruction.
        /// </summary>
        string Instruction { get; }

        /// <summary>
        /// Returns the translation instruction that has been used for evaluation.
        /// </summary>
        IBuddyTranslationInstruction TranslationInstruction { get; }

        /// <summary>
        /// Returns an evaluation result message.
        /// </summary>
        string Message { get; }
    }
}