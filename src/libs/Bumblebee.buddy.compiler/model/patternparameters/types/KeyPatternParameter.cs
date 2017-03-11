using System;
using System.Linq;
using Bumblebee.buddy.compiler.model.patternparameters.values;

namespace Bumblebee.buddy.compiler.model.patternparameters.types {
    /// <summary>
    /// Provides an implementation of <see cref="IPatternParameter"/> to support the key type.
    /// </summary>
    public class KeyPatternParameter : AbstractPatternParameter {

        private static readonly string[] KeyModifier = {"shift", "strg", "alt", "altgr"};
        private static readonly string[] UnaryKeys = {"tab", "enter", "space", "left", "up", "right", "down"};

        /// <inheritdoc />
        public KeyPatternParameter(string name) : base(name, "key") {
            // Currently nothing to do here
        }

        /// <inheritdoc />
        protected override IPatternParameterValue OnConvertValue(string runtimeExpression) {
            string expression = runtimeExpression.ToLower();
            string result = string.Format("\"{0}\"", expression.ToUpper());

            // A single unary key is ok
            if (UnaryKeys.Any(_ => _ == expression)) return new GenericPatternParameterValue(null, result);

            // There must be at least one modifier
            string[] expressionParts = expression.Split(new[] {'+'}, StringSplitOptions.RemoveEmptyEntries);
            if (expressionParts.Length < 2) return ThrowValueConversionException(runtimeExpression);

            // Check if n-1 expressions are modifier
            int max = expressionParts.Length - 1;
            for (int i = -1; ++i != max;) {
                string modifier = expressionParts[i];
                if (KeyModifier.All(_ => _ != modifier)) return ThrowValueConversionException(runtimeExpression);
            }

            return new GenericPatternParameterValue(null, result);
        }
    }
}