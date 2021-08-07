using System;
using System.Collections.Generic;
using System.Linq;
using Bumblebee.buddy.compiler.exceptions;
using Bumblebee.buddy.compiler.model;
using Bumblebee.buddy.compiler.model.evaluation;
using Bumblebee.buddy.compiler.model.instructions.german;
using Bumblebee.buddy.compiler.model.patternparameters.adjustments;
using xcite.csharp.assertions;

namespace Bumblebee.buddy.compiler {
    /// <summary>
    /// Provides methods to map action line strings to processable action steps which will be converted into TDIL directives.
    /// </summary>
    public class InstructionTranslator {
        private readonly Dictionary<IBuddyTranslationInstruction, InstructionTranslationInfo> _instructionTable = new Dictionary<IBuddyTranslationInstruction, InstructionTranslationInfo>(20);

        private readonly InstructionFormatter _instructionFormatter = new InstructionFormatter();
        private readonly InstructionEvaluator _instructionEvaluator = new InstructionEvaluator();

        /// <summary> Creates a new instance. </summary>
        public InstructionTranslator() {
            // TODO Remove workaround
            AddTranslationInstruction(new AssertBuddyTranslationInstruction());
            AddTranslationInstruction(new ClickBuddyTranslationInstruction());
            AddTranslationInstruction(new CloseBuddyTranslationInstruction());
            AddTranslationInstruction(new ExecuteBuddyTranslationInstruction());
            AddTranslationInstruction(new SelectBuddyTranslationInstruction());
            AddTranslationInstruction(new SetBuddyTranslationInstruction());
            AddTranslationInstruction(new StartBuddyTranslationInstruction());
            AddTranslationInstruction(new SwitchBuddyTranslationInstruction());
            AddTranslationInstruction(new WaitBuddyTranslationInstruction());
            AddTranslationInstruction(new PressBuddyTranslationInstruction());
        }

        /// <summary> Adds the given <paramref name="instruction"/> to the translator. </summary>
        /// <param name="instruction">Instruction to add</param>
        /// <exception cref="ArgumentNullException">If <paramref name="instruction"/> is null</exception>
        /// <exception cref="InvalidInstructionTranslationPatternException">If the <paramref name="instruction"/> declares an invalid instruction pattern</exception>
        public void AddTranslationInstruction(IBuddyTranslationInstruction instruction) {
            Assert.NotNull(() => instruction);
            InstructionTranslationInfo instructionInfo = new InstructionTranslationInfo(instruction);
            _instructionTable.Add(instruction, instructionInfo);
        }

        /// <summary>
        /// Returns an instance of <see cref="IBuddyTranslationInstruction"/> that is derived by the action of the given action <paramref name="instruction"/>. 
        /// If the line is null or empty, an <see cref="ArgumentNullException"/> is thrown. Additionally, there is an 
        /// <see cref="BuddyLanguageFormatException"/> if the line doesn't start with an action word.
        /// If no associated action step is found, NULL is returned.
        /// </summary>
        /// <param name="instruction">Action line to process</param>
        /// <returns>Instance of <see cref="IBuddyTranslationInstruction"/> or NULL</returns>
        public IBuddyTranslationInstruction GetTranslationInstruction(string instruction) {
            if (string.IsNullOrEmpty(instruction)) throw new ArgumentNullException(nameof(instruction), "Must not be NULL or empty!");
            string[] instructionWords = instruction.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            string leadingWord = instructionWords.FirstOrDefault();
            if (leadingWord == null) throw new BuddyLanguageFormatException("Given instruction must start with a commanding word!");

            IBuddyTranslationInstruction actionStep = _instructionTable.FirstOrDefault(entry => entry.Key.InstructionId == leadingWord).Key;
            return actionStep;
        }

        /// <summary>
        /// Returns a TDIL directive that represents the given action <paramref name="instruction"/> string based on the given 
        /// associated <paramref name="translationInstruction"/>.
        /// </summary>
        /// <param name="instruction">Instruction string to convert</param>
        /// <param name="translationInstruction">Translation instruction to convert the instruction</param>
        /// <returns>TDIL directive</returns>
        /// <exception cref="ArgumentNullException">If any argument is NULL</exception>
        public string ToDirective(string instruction, IBuddyTranslationInstruction translationInstruction) {
            if (string.IsNullOrEmpty(instruction)) throw new ArgumentNullException(nameof(instruction));
            if (translationInstruction == null) throw new ArgumentNullException(nameof(translationInstruction));

            using (ParameterAdjustmentTable pat = ParameterAdjustmentTable.Create()) {
                string processedInstruction = pat.Process(instruction);

                Dictionary<string, IPatternParameter> paramCache = new Dictionary<string, IPatternParameter>();
                IInstructionEvaluationResult evaRslt = _instructionEvaluator.Evaluate(processedInstruction, _instructionTable[translationInstruction], paramCache);
                if (evaRslt.IsError) {
                    // TODO
                }

                string result = _instructionFormatter.ToString(translationInstruction, paramCache);
                return result;
            } 
        }

        /// <summary>
        /// Returns a TDIL directive that represents the given action <paramref name="instruction"/> string. If the declared action is not known, 
        /// an <see cref="UnknownTranslationInstructionException"/> is thrown.
        /// </summary>
        /// <param name="instruction">Action line string to convert</param>
        /// <returns>TDIL directive</returns>
        /// <exception cref="UnknownTranslationInstructionException">If the declared action is not known.</exception>
        public string ToDirective(string instruction) {
            IBuddyTranslationInstruction translationInstruction = GetTranslationInstruction(instruction);
            if (translationInstruction == null)
                throw new UnknownTranslationInstructionException($"Cannot map the given line '{instruction}' to an action step");

            return ToDirective(instruction, translationInstruction);
        }
    }
}
