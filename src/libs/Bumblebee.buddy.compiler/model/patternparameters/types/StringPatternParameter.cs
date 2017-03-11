using Bumblebee.buddy.compiler.model.patternparameters.values;

namespace Bumblebee.buddy.compiler.model.patternparameters.types {
    /// <summary>
    /// Provides an implementation of <see cref="IPatternParameter"/> to support the string type.
    /// </summary>
    public class StringPatternParameter : AbstractPatternParameter {
        /// <summary>
        /// Creates a new instance based on the given parameters.
        /// </summary>
        /// <param name="name">Name of the parameter</param>
        public StringPatternParameter(string name) : base(name, "string") {
            // Currently nothing to do here
        }

        /// <summary>
        /// Notifies the instance that the given value may be converted to a specific value type. This method is intended to be 
        /// overriden by clients. The default implementation simply creates an instance of <see cref="StringPatternParameterValue"/> 
        /// with the given <paramref name="runtimeExpression"/> and returns it.
        /// </summary>
        /// <param name="runtimeExpression">Expression that describes the runtime value of this parameter</param>
        /// <returns>Instance of <see cref="IPatternParameterValue"/> based on the given expression <paramref name="runtimeExpression"/></returns>
        protected override IPatternParameterValue OnConvertValue(string runtimeExpression) {
            if (!(runtimeExpression.StartsWith("\"") && runtimeExpression.EndsWith("\"")))
                ThrowValueConversionException(runtimeExpression);

            return new StringPatternParameterValue(runtimeExpression);
        }
    }
}