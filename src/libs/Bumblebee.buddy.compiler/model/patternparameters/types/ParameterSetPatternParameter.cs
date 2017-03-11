using System.Text;
using Bumblebee.buddy.compiler.model.functions;
using Bumblebee.buddy.compiler.model.patternparameters.values;

namespace Bumblebee.buddy.compiler.model.patternparameters.types {
    /// <summary>
    /// Provides an implementation of <see cref="IPatternParameter"/> to support the parameter set type.
    /// </summary>
    public class ParameterSetPatternParameter : AbstractPatternParameter {
        /// <inheritdoc />
        public ParameterSetPatternParameter(string name) : base(name, "parameterSet") {
            // Currently nothing to do here
        }

        /// <inheritdoc />
        protected override IPatternParameterValue OnConvertValue(string runtimeExpression) {
            if (runtimeExpression == PatternParameter.Unset) return new StringPatternParameterValue(string.Empty);

            string trimmedExpression = runtimeExpression.Trim();
            if (!trimmedExpression.StartsWith("(")) return ThrowValueConversionException(runtimeExpression);

            // TODO Remove HACK
            string sanitizedExpression = UnquoteFunctionCalls(trimmedExpression);

            string stripppedExpression = sanitizedExpression.Replace("$", string.Empty);
            return base.OnConvertValue(stripppedExpression);
        }

        /// <summary>
        /// Unquotes all function calls within the given <paramref name="expression"/>. If the expression 
        /// does not contain a function call, it's returned unmodified.
        /// </summary>
        /// <param name="expression">Expression to check</param>
        /// <returns>Expression with unquoted function calls or unmodified expression</returns>
        private string UnquoteFunctionCalls(string expression) {
            StringBuilder result = new StringBuilder();
            result.Append('(');                        

            string unblockedString = expression.Trim('(', ')');
            string[] args = unblockedString.Split(',');
            int maxItr = args.Length - 1;
            for (int i = -1; ++i != args.Length;) {
                string arg = args[i].Trim();

                // If so, we have a function call
                if (arg.EndsWith(")\"")) 
                {
                    string functionCall = arg.Trim('"');
                    arg = TdilExtensionFunctionRegistry.Default
                                                      .GetInvocationFormatter()
                                                      .FormatFunctionInvocation(functionCall);
                }

                result.Append(arg);
                if (i != maxItr)
                    result.Append(", ");
            }

            result.Append(')');
            return result.ToString();
        }
    }
}