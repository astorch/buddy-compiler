namespace Bumblebee.buddy.compiler.model.instructions.german {
    /// <summary> Implements <see cref="IBuddyTranslationInstruction"/> for the 'click' action. </summary>
    public class ClickBuddyTranslationInstruction : AbstractBuddyTranslationInstruction {
        /// <summary> Returns id of the action step. </summary>
        public override string InstructionId {
            get { return "Klicke"; }
        }

        /// <summary> Returns the pattern description of the action step. </summary>
        public override string InstructionPattern {
            get { return "Klicke <{name:alias,type:alias}> {} {name:kind,type:frequence,mandatory:false}"; }
        }

        /// <summary> Returns the resulting TDIL pattern of the action step. </summary>
        public override string TdilPattern {
            get { return "click(~alias, , ~kind.Value)"; }
        }
    }
}