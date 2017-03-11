using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Bumblebee.buddy.compiler.collectiontools;
using Bumblebee.buddy.compiler.model.evaluation;
using Bumblebee.buddy.compiler.model.patternparameters;
using Xcite.Collections;

namespace Bumblebee.buddy.compiler.model {
    /// <summary>
    /// Implements an instruction evaluator.
    /// </summary>
    public class InstructionEvaluator {
        private static readonly string ValueSequenceToken = string.Format("\"{0}\"", (char)8116);

        /// <summary>
        /// Evaluates the given <paramref name="instruction"/> using the given <paramref name="instructionInfo"/>. Any resolved parameter is added 
        /// to <paramref name="paramCache"/>.
        /// </summary>
        /// <param name="instruction">Directive to evaluate</param>
        /// <param name="instructionInfo">Instruction that provides the meta data</param>
        /// <param name="paramCache">Parameter cache</param>
        /// <returns>Evaluation result</returns>
        public IInstructionEvaluationResult Evaluate(string instruction, InstructionTranslationInfo instructionInfo, IDictionary<string, IPatternParameter> paramCache) {
            if (string.IsNullOrEmpty(instruction)) throw new ArgumentNullException("instruction");
            if (instructionInfo == null) throw new ArgumentNullException("instructionInfo");

            string wordSequence = instruction;

            // Remove dot
            if (wordSequence.EndsWith("."))
                wordSequence = wordSequence.Substring(0, wordSequence.Length - 1);

            // Value sequence recognition
            string valueSequenceString = null;
            wordSequence = Regex.Replace(wordSequence,
                @"(?<top>""[^""]+"")(?<nav>\s(?<child>""[^""]+""))*\s(?<value>""[^""]+"")",
                match => {
                    string prime = match.Groups["top"].Value;
                    string sslValue = string.Join(", ", match.Groups["child"].Captures.ToArray());
                    string value = match.Groups["value"].Value;

                    string[] joinSet = string.IsNullOrEmpty(sslValue) ? new[] {prime, value} : new[] {prime, sslValue, value};
                    valueSequenceString = string.Join(", ", joinSet);

                    return ValueSequenceToken;
                });

            // Split to words
            string[] words = wordSequence.SplitToBuddyTokens();
            if (words.Length == 0) return EvaluationResult.Error(wordSequence, instructionInfo.Instruction, "Instruction is empty after split");
            
            words = words.StripSpecialCharacters();
            if (words.Length == 0) return EvaluationResult.Error(wordSequence, instructionInfo.Instruction, "Instruction is empty after strip");

            // Normalize variants
            IVariantInstruction variantInstruction = instructionInfo.Instruction as IVariantInstruction;
            if (variantInstruction != null)
                words = variantInstruction.Normalize(words);

            // Iterate words and check for match
            int wordIndex = 0;
            for (IEnumerator<IInstructionPatternToken> tokenItr = instructionInfo.GetInstructionPatternTokenEnumerator(); tokenItr.MoveNext();) {
                IInstructionPatternToken token = tokenItr.Current;
                string word;

                // Take the word from the sequence
                if (wordIndex < words.Length) {
                    word = words[wordIndex];
                } else { // We have more tokens than words
                    // All remaining tokens must be not mandatory or we have an error
                    // So they must handle an unset value
                    word = PatternParameter.Unset;
                }

                // If we have a match, everything is fine
                if (token.DoesMatch(word)) {
                    wordIndex++;

                    // Resolve parametrized tokens
                    IParametrizedInstructionPatternToken parametrizedToken = token as IParametrizedInstructionPatternToken;
                    if (parametrizedToken != null) {
                        IPatternParameter patternParameter = parametrizedToken.Value;
                        string parameterName = patternParameter.Name;

                        // If the value has been replaced by a ValueSequenceToken before, we have to re-replace it now
                        if (valueSequenceString != null) { // Something must have been replaced
                            if (patternParameter.Value.Value.Equals(ValueSequenceToken)) { // Is this the correct parameter?
                                ((AbstractPatternParameter)patternParameter).SetValue(valueSequenceString);
                            }
                        }
                        

                        // Add value to cache
                        paramCache.Add(parameterName, patternParameter);
                    }

                    continue;
                }
                
                // If the token is not mandatory, we can go forward
                if (!token.IsMandatory) continue;

                return EvaluationResult.Error(wordSequence, instructionInfo.Instruction, 
                    string.Format("{0} word of instruction does not match expected token '{1}'. Word is '{2}'", FormatWordIndex(wordIndex), token.ToHumanReadableString(), word));
            }                

            return EvaluationResult.Ok;
        }

        /// <summary>
        /// Returns a human readable string representation of the given <paramref name="wordIndex"/>.
        /// </summary>
        /// <param name="wordIndex">Word index</param>
        /// <returns>string</returns>
        private string FormatWordIndex(int wordIndex) {
            int usrWrdIdx = wordIndex + 1;
            string suffix = "th";
            if (usrWrdIdx == 1)
                suffix = "st";
            if (usrWrdIdx == 2)
                suffix = "nd";
            if (usrWrdIdx == 3)
                suffix = "rd";

            return string.Format("{0}{1}", usrWrdIdx, suffix);
        }
    }
}