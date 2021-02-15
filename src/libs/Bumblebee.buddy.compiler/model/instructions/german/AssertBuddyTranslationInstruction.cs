namespace Bumblebee.buddy.compiler.model.instructions.german {
    /// <summary> Implements <see cref="IBuddyTranslationInstruction"/> for the 'assert' action. </summary>
    public class AssertBuddyTranslationInstruction : AbstractBuddyTranslationInstruction {
        
        /// <inheritdoc />
        public override string InstructionId { get; } = "Prüfe";

        /// <inheritdoc />
        public override string InstructionPattern { get; } = "Prüfe <{name:alias,type:alias}> {name:value,type:param,mandatory:false} {name:condition,type:condition}";

        /// <inheritdoc />
        public override string TdilPattern { get; } = "assert(~alias, ~condition.Name, ~(value|condition.Value))";
        
    }
}