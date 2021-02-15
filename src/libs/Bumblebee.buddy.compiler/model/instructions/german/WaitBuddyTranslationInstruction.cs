namespace Bumblebee.buddy.compiler.model.instructions.german {
    /// <summary>
    /// Implements <see cref="IBuddyTranslationInstruction"/> for the 'wait' action.
    /// </summary>
    public class WaitBuddyTranslationInstruction : AbstractBuddyTranslationInstruction {
        
        /// <inheritdoc />
        public override string InstructionId { get; } = "Warte";
        
        /// <inheritdoc />
        public override string InstructionPattern { get; } = "Warte <{name:alias,type:aliaskeyword}> {name:condition,type:condition}";
        
        /// <inheritdoc />
        public override string TdilPattern { get; } = "wait(~alias, ~condition.Name, ~condition.Value, 60000)"; // 1min
        
    }
}