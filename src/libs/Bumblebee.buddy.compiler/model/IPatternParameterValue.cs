namespace Bumblebee.buddy.compiler.model {
    /// <summary>
    /// Describes a pattern parameter value.
    /// </summary>
    public interface IPatternParameterValue {
        /// <summary>
        /// Returns the name of the value type.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Returns the value of the pattern parameter.
        /// </summary>
        object Value { get; }

        /// <summary>
        /// Returns a string that represents the value of the pattern parameter.
        /// </summary>
        /// <returns>Value as string</returns>
        string RenderToString();
    }
}