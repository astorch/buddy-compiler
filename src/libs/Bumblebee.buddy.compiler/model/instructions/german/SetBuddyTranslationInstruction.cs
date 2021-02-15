namespace Bumblebee.buddy.compiler.model.instructions.german {
    /// <summary> Implements <see cref="IBuddyTranslationInstruction"/> for the 'set' action. </summary>
    public class SetBuddyTranslationInstruction : AbstractBuddyTranslationInstruction {
        
        /// <inheritdoc />
        public override string InstructionId { get; } = "Setze";

        /// <inheritdoc />
        public override string InstructionPattern { get; } = "Setze {name:value,type:param} <{name:alias,type:alias}>";

        /// <inheritdoc />
        public override string TdilPattern { get; } = "set(~alias, Text, ~value)";
    }
}