namespace Bumblebee.buddy.compiler.model.instructions.german {
    /// <summary>
    /// Implements <see cref="IBuddyTranslationInstruction"/> for the 'assert' action.
    /// </summary>
    public class AssertBuddyTranslationInstruction : AbstractBuddyTranslationInstruction {
        /// <summary>
        /// Returns id of the action step.
        /// </summary>
        public override string InstructionId {
            get { return "Prüfe"; }
        }

        /// <summary>
        /// Returns the pattern description of the action step.
        /// </summary>
        public override string InstructionPattern {
            get { return "Prüfe <{name:alias,type:alias}> {name:value,type:param,mandatory:false} {name:condition,type:condition}"; }
        }

        /// <summary>
        /// Returns the resulting TDIL pattern of the action step.
        /// </summary>
        public override string TdilPattern {
            get { return "assert(~alias, ~condition.Name, ~(value|condition.Value))"; }
        }
    }
}