﻿using Bumblebee.buddy.compiler.exceptions;
using Bumblebee.buddy.compiler.model.patternparameters;

namespace Bumblebee.buddy.compiler.model.tokens {
    /// <summary>
    /// Implements <see cref="IInstructionPatternToken"/> for a parameter token.
    /// </summary>
    public class ParameterToken : IParametrizedInstructionPatternToken {
        /// <summary>
        /// Returns the value or does set it.
        /// </summary>
        public IPatternParameter Value { get; set; }

        /// <summary>
        /// Returns TRUE if the given <paramref name="word"/> matches the token.
        /// </summary>
        /// <param name="word">Word to check</param>
        /// <returns>TRUE or FALSE</returns>
        public bool DoesMatch(string word) {
            try {
                ((AbstractPatternParameter) Value).SetValue(word);
                return true;
            } catch (ValueConversionException) {
                return false;
            }
        }

        /// <summary>
        /// Returns TRUE if the token is mandatory.
        /// </summary>
        public bool IsMandatory { get { return Value.Mandatory; } }

        /// <summary>
        /// Returns a string representation of this token that is human readable.
        /// </summary>
        /// <returns>A human readable string representation of this token</returns>
        public string ToHumanReadableString() {
            return string.Format("Parameter reference '{0}' ({1})", Value.Name, Value.Type);
        }
    }
}