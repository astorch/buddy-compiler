namespace Bumblebee.buddy.compiler.model {
    /// <summary> Defines a buddy translation instruction. </summary>
    public interface IBuddyTranslationInstruction {
        /// <summary> Returns id of the directive. </summary>
        string InstructionId { get; }

        /// <summary> Returns the pattern of the instruction. </summary>
        string InstructionPattern { get; }

        /// <summary> Returns the resulting TDIL pattern of the directive. </summary>
        string TdilPattern { get; }
    }
}