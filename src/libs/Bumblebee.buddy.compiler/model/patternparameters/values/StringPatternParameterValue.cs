namespace Bumblebee.buddy.compiler.model.patternparameters.values {
    /// <summary>
    /// Provides an implementation of <see cref="IPatternParameterValue"/> for string values.
    /// </summary>
    public class StringPatternParameterValue : AbstractPatternParameterValue<string> {
        /// <summary>
        /// Creates a new instance based on the given parameters.
        /// </summary>
        /// <param name="value">Value of the attribute</param>
        public StringPatternParameterValue(string value) : base(null, value) {
            // Currently nothing to do here
        }
    }
}