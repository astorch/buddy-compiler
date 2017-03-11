namespace Bumblebee.buddy.compiler.model.instructions.german {
    /// <summary>
    /// Implements the <see cref="IBuddyTranslationInstruction"/> for the 'start' action.
    /// </summary>
    public class StartBuddyTranslationInstruction : AbstractBuddyTranslationInstruction, IReferableInstruction {
        /// <summary>
        /// Returns id of the action step.
        /// </summary>
        public override string InstructionId {
            get { return "Starte"; }
        }

        /// <summary>
        /// Returns the pattern description of the action step.
        /// </summary>
        public override string InstructionPattern {
            get { return "Starte {name:path,type:string}"; }
        }

        /// <summary>
        /// Returns the resulting TDIL pattern of the action step.
        /// </summary>
        public override string TdilPattern {
            get { return "start(,, ~path)"; }
        }

        /// <summary>
        /// Returns the pattern that is used as action result reference.
        /// </summary>
        public string ResultReferencePattern {
            get { return "processHandle#"; }
        }
    }
}