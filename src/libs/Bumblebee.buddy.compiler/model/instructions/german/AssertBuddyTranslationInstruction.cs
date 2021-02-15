namespace Bumblebee.buddy.compiler.model.instructions.german {
    /// <summary> Implements <see cref="IBuddyTranslationInstruction"/> for the 'assert' action. </summary>
    public class AssertBuddyTranslationInstruction : AbstractBuddyTranslationInstruction {
        
        /// <inheritdoc />
        public override string InstructionId {
            get { return "Prüfe"; }
        }
        
        /// <inheritdoc />
        public override string InstructionPattern {
            get { return "Prüfe <{name:alias,type:alias}> {name:value,type:param,mandatory:false} {name:condition,type:condition}"; }
        }

        /// <inheritdoc />
        public override string TdilPattern {
            get { return "assert(~alias, ~condition.Name, ~(value|condition.Value))"; }
        }
        
    }
}