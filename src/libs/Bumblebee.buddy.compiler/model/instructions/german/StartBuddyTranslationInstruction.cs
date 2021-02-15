namespace Bumblebee.buddy.compiler.model.instructions.german {
    /// <summary> Implements the <see cref="IBuddyTranslationInstruction"/> for the 'start' action. </summary>
    public class StartBuddyTranslationInstruction : AbstractBuddyTranslationInstruction, IReferableInstruction {
        
        /// <inheritdoc />
        public override string InstructionId { get; } = "Starte";

        /// <inheritdoc />
        public override string InstructionPattern { get; } = "Starte {name:path,type:string}";

        /// <inheritdoc />
        public override string TdilPattern { get; } = "start(,, ~path)";
        
        /// <inheritdoc />
        public string ResultReferencePattern { get; } = "processHandle#";
        
    }
}