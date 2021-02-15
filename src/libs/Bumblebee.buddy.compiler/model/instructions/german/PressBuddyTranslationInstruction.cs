namespace Bumblebee.buddy.compiler.model.instructions.german {
    /// <summary> Implements <see cref="IBuddyTranslationInstruction"/> for the 'press' action. </summary>
    public class PressBuddyTranslationInstruction : AbstractBuddyTranslationInstruction {
        
        /// <inheritdoc />
        public override string InstructionId { get; } = "Drücke";

        /// <inheritdoc />
        public override string InstructionPattern { get; } = "Drücke {name:keyName,type:key} {name:kind,type:frequence,mandatory:false}";

        /// <inheritdoc />
        public override string TdilPattern { get; } = "pressKey(, KeyType, ~keyName.Value, ~kind.Value)";
        
    }
}