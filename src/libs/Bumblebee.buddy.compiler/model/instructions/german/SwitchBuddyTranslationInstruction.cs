namespace Bumblebee.buddy.compiler.model.instructions.german {
    /// <summary>
    /// Implements the <see cref="IBuddyTranslationInstruction"/> for the 'switch' action.
    /// </summary>
    public class SwitchBuddyTranslationInstruction : AbstractBuddyTranslationInstruction {
        /// <inheritdoc />
        public override string InstructionId { get; } = "Wechsle";

        /// <inheritdoc />
        public override string InstructionPattern { get; } = "Wechsle {name:path,type:string}";

        /// <inheritdoc />
        public override string TdilPattern { get; } = "setsut(,, ~path)";
    }
}