namespace Bumblebee.buddy.compiler.model.instructions.german {
    /// <summary>
    /// Implements <see cref="IBuddyTranslationInstruction"/> for the 'wait' action.
    /// </summary>
    public class WaitBuddyTranslationInstruction : AbstractBuddyTranslationInstruction {
        /// <summary>
        /// Returns id of the action step.
        /// </summary>
        public override string InstructionId {
            get { return "Warte"; }
        }

        /// <summary>
        /// Returns the pattern description of the action step.
        /// </summary>
        public override string InstructionPattern {
            get { return "Warte <{name:alias,type:aliaskeyword}> {name:condition,type:condition}"; }
        }

        /// <summary>
        /// Returns the resulting TDIL pattern of the action step.
        /// </summary>
        public override string TdilPattern {
            get { return "wait(~alias, ~condition.Name, ~condition.Value, 60000)"; } // 1min
        }
    }
}