namespace Bumblebee.buddy.compiler.model.patternparameters.types {
    /// <summary>
    /// Provides an implementation of <see cref="IPatternParameter"/> to support the alias type.
    /// </summary>
    public class AliasPatternParameter : AbstractPatternParameter {
        /// <summary>
        /// Creates a new instance based on the given parameters.
        /// </summary>
        /// <param name="name">Name of the parameter</param>
        public AliasPatternParameter(string name) : base(name, "alias") {
            // Currently nothing to do here
        }

        /// <inheritdoc />
        protected override IPatternParameterValue OnConvertValue(string runtimeExpression) {

            if (CompilingContext.Current != null)
                CompilingContext.Current.AddAliasReference(runtimeExpression);

            return base.OnConvertValue(runtimeExpression);
        }
    }
}