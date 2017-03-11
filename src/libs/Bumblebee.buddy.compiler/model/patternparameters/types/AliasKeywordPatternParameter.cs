using Bumblebee.buddy.compiler.model.patternparameters.values;

namespace Bumblebee.buddy.compiler.model.patternparameters.types {
    /// <summary>
    /// Provides an implementation of <see cref="IPatternParameter"/> to support the alias keyword type.
    /// </summary>
    public class AliasKeywordPatternParameter : AliasPatternParameter {
        /// <inheritdoc />
        public AliasKeywordPatternParameter(string name) : base(name) {
            // Currently nothing to do here
        }

        /// <inheritdoc />
        protected override IPatternParameterValue OnConvertValue(string runtimeExpression) {
            if (runtimeExpression == "Anwendung")
                return new StringPatternParameterValue("_Application");
            
            return base.OnConvertValue(runtimeExpression);
        }
    }
}