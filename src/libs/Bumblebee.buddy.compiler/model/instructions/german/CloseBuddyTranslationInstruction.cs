namespace Bumblebee.buddy.compiler.model.instructions.german {
    /// <summary>
    /// Implements the <see cref="IBuddyTranslationInstruction"/> for the 'close' action.
    /// </summary>
    public class CloseBuddyTranslationInstruction : AbstractBuddyTranslationInstruction {
        /// <summary>
        /// Returns id of the action step.
        /// </summary>
        public override string InstructionId {
            get { return "Schließe"; }
        }

        /// <summary>
        /// Returns the pattern description of the action step.
        /// </summary>
        public override string InstructionPattern {
            get { return "Schließe {name:ref,type:keyword}"; }
        }

        /// <summary>
        /// Returns the resulting TDIL pattern of the action step.
        /// </summary>
        public override string TdilPattern {
            get { return "close(~ref, , Default)"; }
        }
    }
}