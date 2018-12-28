using Bumblebee.buddy.compiler.model.functions;

namespace Bumblebee.buddy.compiler.model.patternparameters.types {
    /// <summary>
    /// Provides an implementation of <see cref="IPatternParameter"/> to support the param type.
    /// </summary>
    public class ParamPatternParameter : AbstractPatternParameter {
        /// <summary>
        /// Creates a new instance based on the given parameters.
        /// </summary>
        /// <param name="name">Name of the parameter</param>
        public ParamPatternParameter(string name) : base(name, "param") {
            // Currently nothing to do here
        }

        /// <inheritdoc />
        protected override IPatternParameterValue OnConvertValue(string runtimeExpression) {
            string expression = runtimeExpression;
            if (expression[0] != '$' && expression[0] != '"') ThrowValueConversionException(runtimeExpression);

            if (expression[0] == '$')
                expression = expression.Substring(1);

            expression = TdilExtensionFunctionRegistry.Default
                                                      .GetInvocationFormatter()
                                                      .FormatFunctionInvocation(expression);

            return base.OnConvertValue(expression);
        }
    }
}