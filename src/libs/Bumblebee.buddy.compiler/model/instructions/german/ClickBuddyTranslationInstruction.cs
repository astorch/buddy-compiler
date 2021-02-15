namespace Bumblebee.buddy.compiler.model.instructions.german {
    /// <summary> Implements <see cref="IBuddyTranslationInstruction"/> for the 'click' action. </summary>
    public class ClickBuddyTranslationInstruction : AbstractBuddyTranslationInstruction {
        
        /// <inheritdoc />
        public override string InstructionId { get; } = "Klicke";

        /// <inheritdoc />
        public override string InstructionPattern { get; } = "Klicke <{name:alias,type:alias}> {name:value,type:param,mandatory:false} {name:kind,type:frequence,mandatory:false}";

        /// <inheritdoc />
        public override string TdilPattern { get; } = "click(~alias, , ~(value&kind.Value))";
        
    }
}