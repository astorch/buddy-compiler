namespace Bumblebee.buddy.compiler.model.instructions.german {
    /// <summary> Implements the <see cref="IBuddyTranslationInstruction"/> for the 'close' action. </summary>
    public class CloseBuddyTranslationInstruction : AbstractBuddyTranslationInstruction {
        
        /// <inheritdoc />
        public override string InstructionId { get; } = "Schließe";

        /// <inheritdoc />
        public override string InstructionPattern { get; } = "Schließe {name:ref,type:keyword}";

        /// <inheritdoc />
        public override string TdilPattern { get; } = "close(~ref, , Default)";
        
    }
}