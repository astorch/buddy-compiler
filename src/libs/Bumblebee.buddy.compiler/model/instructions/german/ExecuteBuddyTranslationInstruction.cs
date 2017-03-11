namespace Bumblebee.buddy.compiler.model.instructions.german {
    /// <summary>
    /// Implements the <see cref="IBuddyTranslationInstruction"/> for the 'gosub' action.
    /// </summary>
    public class ExecuteBuddyTranslationInstruction : AbstractBuddyTranslationInstruction {
        /// <summary>
        /// Returns id of the action step.
        /// </summary>
        public override string InstructionId {
            get { return "Führe"; }
        }

        /// <summary>
        /// Returns the pattern description of the action step.
        /// </summary>
        public override string InstructionPattern {
            get { return "Führe [{name:unitName,type:unitReference}] {name:parameterSet,type:parameterSet,mandatory:false}"; }
        }

        /// <summary>
        /// Returns the resulting TDIL pattern of the action step.
        /// </summary>
        public override string TdilPattern {
            get { return "gosub ~unitName:~parameterSet"; }
        }
    }
}