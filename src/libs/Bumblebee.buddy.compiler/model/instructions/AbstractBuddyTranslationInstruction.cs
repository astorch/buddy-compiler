namespace Bumblebee.buddy.compiler.model.instructions {
    /// <summary> Provides an abstract implementation of <see cref="IBuddyTranslationInstruction"/>. </summary>
    public abstract class AbstractBuddyTranslationInstruction : IBuddyTranslationInstruction {
        /// <inheritdoc />
        public abstract string InstructionId { get; }
        
        /// <inheritdoc />
        public abstract string InstructionPattern { get; }
        
        /// <inheritdoc />
        public abstract string TdilPattern { get; }
    }
}