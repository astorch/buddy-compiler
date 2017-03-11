using System.Collections.Generic;
using Bumblebee.buddy.compiler.collectiontools;
using Bumblebee.buddy.compiler.exceptions;
using Bumblebee.buddy.compiler.model.patternparameters;
using Bumblebee.buddy.compiler.model.tokens;
using Xcite.Collections;

namespace Bumblebee.buddy.compiler.model {
    /// <summary>
    /// Provides meta data for a <see cref="IBuddyTranslationInstruction"/>.
    /// </summary>
    public class InstructionTranslationInfo {
        private readonly IBuddyTranslationInstruction iInstruction;
        private readonly LinearList<IInstructionPatternToken> iInstructionTokens = new LinearList<IInstructionPatternToken>();

        /// <summary>
        /// Creates a new instance for the given <paramref name="instruction"/>.
        /// </summary>
        /// <param name="instruction">Instruction to provide meta data</param>
        /// <exception cref="InvalidInstructionTranslationPatternException">If the instruction pattern is invalid</exception>
        public InstructionTranslationInfo(IBuddyTranslationInstruction instruction) {
            iInstruction = instruction;
            CreateMetaInfo(instruction);
        }

        /// <summary>
        /// Returns the associated instruction.
        /// </summary>
        public IBuddyTranslationInstruction Instruction { get { return iInstruction; } }

        /// <summary>
        /// Returns an enumerator for all pattern tokens that are associated with the instruction.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IInstructionPatternToken> GetInstructionPatternTokenEnumerator() {
            return iInstructionTokens.GetEnumerator();
        }

        /// <summary>
        /// Adds the given <paramref name="token"/> to pattern token sequence.
        /// </summary>
        /// <param name="token">Token to add</param>
        private void AddToken(IInstructionPatternToken token) {
            iInstructionTokens.Add(token);
        }

        /// <summary>
        /// Processes and evaluates the given <paramref name="instruction"/> to create the instruction meta data.
        /// </summary>
        /// <param name="instruction">Instruction to process</param>
        /// <exception cref="InvalidInstructionTranslationPatternException">If the instruction pattern is invalid</exception>
        private void CreateMetaInfo(IBuddyTranslationInstruction instruction) {
            string directivePattern = instruction.InstructionPattern;
            if (string.IsNullOrEmpty(directivePattern))
                throw new InvalidInstructionTranslationPatternException(string.Format("Directive pattern of instruction '{0}' is NULL or empty", instruction.GetType()));

            string[] directiveTokens = directivePattern.SplitToBuddyTokens();
            if (directiveTokens.Length == 0)
                throw new InvalidInstructionTranslationPatternException(string.Format("Directive pattern of instruction '{0}' has no tokens. Pattern is '{1}'", instruction.GetType(), directivePattern));

            string leadingWord = directiveTokens[0];
            if (string.IsNullOrEmpty(leadingWord))
                throw new InvalidInstructionTranslationPatternException(string.Format("Directive pattern of instruction '{0}' has an empty leading word. Pattern is '{1}'", instruction.GetType(), directivePattern));

            InstructionTranslationInfo instructionInfo = this;

            // Add leading word token
            instructionInfo.AddToken(new WordToken { Value = leadingWord });

            // Process remaining tokens
            for (int i = 0; ++i != directiveTokens.Length; ) {
                string token = directiveTokens[i];
                ProcessToken(token, instructionInfo);
            }
        }

        /// <summary>
        /// Processes the given instruction pattern <paramref name="token"/> and creates a meta information object.
        /// </summary>
        /// <param name="token">Token to process</param>
        /// <param name="instructionInfo">Instruction meta info to extend</param>
        /// <exception cref="InvalidInstructionTranslationPatternException">If the instruction pattern is invalid</exception>
        private void ProcessToken(string token, InstructionTranslationInfo instructionInfo) {
            if (string.IsNullOrEmpty(token))
                throw new InvalidInstructionTranslationPatternException(string.Format("Token of directive pattern of instruction '{0}' is empty. ", instructionInfo.Instruction.GetType()));

            char tokenHead = token[0];

            // Token is special char
            if (tokenHead == '<') {
                CreateSpecialCharacterToken(token, instructionInfo, "<", ">");
                return;
            }
            if (tokenHead == '[') {
                CreateSpecialCharacterToken(token, instructionInfo, "[", "]");
                return;
            }

            // Token is a parameter
            if (tokenHead == '{') {
                CreateParameterToken(token, instructionInfo);
                return;
            }

            throw new InvalidInstructionTranslationPatternException(string.Format("Token of instruction pattern '{0}' contains an unexpected character '{1}'", instructionInfo.Instruction.GetType(), tokenHead));
        }

        /// <summary>
        /// Processes the given <paramref name="token"/> in the case of a special character token.
        /// </summary>
        /// <param name="token">Token to process</param>
        /// <param name="instructionInfo">Instruction info to extend</param>
        /// <param name="tokenHead">Token head</param>
        /// <param name="tokenTail">Token tail</param>
        private void CreateSpecialCharacterToken(string token, InstructionTranslationInfo instructionInfo, string tokenHead, string tokenTail) {
            if (!token.EndsWith(tokenTail)) 
                throw new InvalidInstructionTranslationPatternException(string.Format("Directive pattern token does not finish with '{1}'. Token is '{0}'", token, tokenTail));

            string tokenCore = token.Substring(1, token.Length - 2);

            // Head
            instructionInfo.AddToken(new SpecialCharacterToken { Value = tokenHead });

            // Content
            ProcessToken(tokenCore, instructionInfo);

            // Tail
            instructionInfo.AddToken(new SpecialCharacterToken { Value = tokenTail });
        }

        /// <summary>
        /// Processes the given <paramref name="token"/> in the case of a parameter token.
        /// </summary>
        /// <param name="token">Token to process</param>
        /// <param name="instructionInfo">Instruction info to extend</param>
        private void CreateParameterToken(string token, InstructionTranslationInfo instructionInfo) {
            if (!token.EndsWith("}")) 
                throw new InvalidInstructionTranslationPatternException(string.Format("Directive pattern token does not finish with '}}'. Token is '{0}'", token));

            string tokenCore = token.Substring(1, token.Length - 2);

            string[] parameterParts = tokenCore.Split(',');
            if (parameterParts.Length < 2) 
                throw new InvalidInstructionTranslationPatternException(string.Format("Directive pattern token does not match expected token structure. Token is '{0}'", token));

            Dictionary<string, string> valueSet = new Dictionary<string, string>();
            for (int i = -1; ++i != parameterParts.Length; ) {
                string parameterPart = parameterParts[i];
                string[] keyValueSet = parameterPart.Split(':');
                if (keyValueSet.Length != 2) throw new InvalidInstructionTranslationPatternException(string.Format("Directive pattern parameter token does not declare values in 'key:value' format!. Token is '{0}'", token));
                string key = keyValueSet[0];
                string value = keyValueSet[1];
                valueSet.Add(key, value);
            }

            IPatternParameter patternParameter = new PatternParameterFactory().CreatePatternParameter(valueSet);
            instructionInfo.AddToken(new ParameterToken {Value = patternParameter});
        }
    }
}