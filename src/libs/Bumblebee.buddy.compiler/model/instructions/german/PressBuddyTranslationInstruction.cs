namespace Bumblebee.buddy.compiler.model.instructions.german {
    /// <summary>
    /// Implements <see cref="IBuddyTranslationInstruction"/> for the 'press' action.
    /// </summary>
    public class PressBuddyTranslationInstruction : AbstractBuddyTranslationInstruction {
        /// <summary>
        /// Returns id of the action step.
        /// </summary>
        public override string InstructionId {
            get { return "Drücke"; }
        }

        /// <summary>
        /// Returns the pattern description of the action step.
        /// </summary>
        public override string InstructionPattern {
            get { return "Drücke {name:keyName,type:key} {name:kind,type:frequence,mandatory:false}"; }
        }

        /// <summary>
        /// Returns the resulting TDIL pattern of the action step.
        /// </summary>
        public override string TdilPattern {
            get { return "pressKey(, KeyType, ~keyName.Value, ~kind.Value)"; }
        }
    }
}