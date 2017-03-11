namespace Bumblebee.buddy.compiler.model.instructions {
    /// <summary>
    /// Provides an abstract implementation of <see cref="IBuddyTranslationInstruction"/>.
    /// </summary>
    public abstract class AbstractBuddyTranslationInstruction : IBuddyTranslationInstruction {
        /// <summary>
        /// Returns id of the action step.
        /// </summary>
        public abstract string InstructionId { get; }

        /// <summary>
        /// Returns the pattern description of the action step.
        /// </summary>
        public abstract string InstructionPattern { get; }

        /// <summary>
        /// Returns the resulting TDIL pattern of the action step.
        /// </summary>
        public abstract string TdilPattern { get; }
    }
}