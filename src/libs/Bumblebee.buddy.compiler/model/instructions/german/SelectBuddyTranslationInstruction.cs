using System;
using System.Collections.Generic;
using Bumblebee.buddy.compiler.exceptions;
using Bumblebee.buddy.compiler.model.patternparameters.types;
using Xcite.Collections;

namespace Bumblebee.buddy.compiler.model.instructions.german {
    /// <summary>
    /// Implements <see cref="IBuddyTranslationInstruction"/> for the 'select' action.
    /// </summary>
    public class SelectBuddyTranslationInstruction : AbstractBuddyTranslationInstruction, IVariantInstruction {
        /// <summary>
        /// Returns id of the action step.
        /// </summary>
        public override string InstructionId {
            get { return "Wähle"; }
        }

        /// <summary>
        /// Returns the pattern description of the action step.
        /// </summary>
        public override string InstructionPattern {
            get { return "Wähle {name:value,type:param} <{name:alias,type:alias}>"; }
        }

        /// <summary>
        /// Returns the resulting TDIL pattern of the action step.
        /// </summary>
        public override string TdilPattern {
            get { return "select(~alias, Value, ~value)"; }
        }

        /// <summary>
        /// Returns the normalized word sequence for given <paramref name="words"/>.
        /// </summary>
        /// <param name="words">Word sequence to normalize</param>
        /// <returns>Normalized word sequence</returns>
        public string[] Normalize(string[] words) {
            Queue<string> originalSequence = new Queue<string>(words);
            LinearList<string> normalizedSequence = new LinearList<string>();
            int tokenIndex = 0;

            ParamPatternParameter paramPatternParameter = new ParamPatternParameter("value");
            AliasPatternParameter aliasPatternParameter = new AliasPatternParameter("alias");

            while (originalSequence.Count != 0 || tokenIndex < 5) {
                string word = originalSequence.Dequeue();

                if (tokenIndex == 0) {
                    if (string.Equals(word, InstructionId, StringComparison.InvariantCultureIgnoreCase)) {
                        tokenIndex++;
                        normalizedSequence.Add(word);
                    } else {
                        originalSequence.Enqueue(word);
                    }
                } else if (tokenIndex == 1) {
                    if (word.StartsWith("$") || word.StartsWith("\"")) {
                        tokenIndex++;
                        normalizedSequence.Add(word);
                    } else {
                        originalSequence.Enqueue(word);
                    }
                } else if (tokenIndex == 2) {
                    if (word == "<") {
                        tokenIndex++;
                        normalizedSequence.Add(word);
                    } else {
                        originalSequence.Enqueue(word);
                    }
                } else if (tokenIndex == 3) {
                    try {
                        aliasPatternParameter.SetValue(word);
                        tokenIndex++;
                        normalizedSequence.Add(word);
                    } catch (ValueConversionException) {
                        originalSequence.Enqueue(word);
                    }
                } else if (tokenIndex == 4) {
                    if (word == ">") {
                        tokenIndex++;
                        normalizedSequence.Add(word);
                        break;
                    } else {
                        originalSequence.Enqueue(word);
                    }
                } else {
                    throw new Exception(); // TODO
                }
            }

            string[] resultingOrder = normalizedSequence.ToArray();
            return resultingOrder;
        }
    }
}