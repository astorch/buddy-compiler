namespace Bumblebee.buddy.compiler.model.patternparameters.values {
    /// <summary>
    /// Provides a generic implementation of <see cref="IPatternParameterValue"/> for values.
    /// </summary>
    public class GenericPatternParameterValue : AbstractPatternParameterValue<string> {
        /// <summary>
        /// Creates a new instance based on the given parameters.
        /// </summary>
        /// <param name="name">Name of the attribute</param>
        /// <param name="value">Value of the attribute</param>
        public GenericPatternParameterValue(string name, string value) : base(name, value) {
            // Currently nothing to do here
        }
    }
}