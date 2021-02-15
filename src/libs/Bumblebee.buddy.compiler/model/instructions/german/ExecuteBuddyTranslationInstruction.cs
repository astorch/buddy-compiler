namespace Bumblebee.buddy.compiler.model.instructions.german {
    /// <summary> Implements the <see cref="IBuddyTranslationInstruction"/> for the 'gosub' action. </summary>
    public class ExecuteBuddyTranslationInstruction : AbstractBuddyTranslationInstruction {
        
        /// <inheritdoc />
        public override string InstructionId { get; } = "Führe";

        /// <inheritdoc />
        public override string InstructionPattern { get; } = "Führe [{name:unitName,type:unitReference}] {name:parameterSet,type:parameterSet,mandatory:false}";

        /// <inheritdoc />
        public override string TdilPattern { get; } = "gosub ~unitName:~parameterSet";
        
    }
}