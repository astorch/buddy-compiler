namespace Bumblebee.buddy.compiler.model.patternparameters.values {
    /// <summary>
    /// Provides an abstract implementation of <see cref="IPatternParameterValue"/>. Each method can be 
    /// overridden.
    /// </summary>
    /// <typeparam name="TValue">Type of the provided value</typeparam>
    public abstract class AbstractPatternParameterValue<TValue> : IPatternParameterValue {
        /// <summary>
        /// Creates a new instance based on the given parameters.
        /// </summary>
        /// <param name="name">Name of the attribute</param>
        /// <param name="value">Value of the attribute</param>
        protected AbstractPatternParameterValue(string name, TValue value) {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Returns the name of the value type.
        /// </summary>
        public virtual string Name { get; private set; }

        /// <summary>
        /// Returns the value of the pattern parameter.
        /// </summary>
        object IPatternParameterValue.Value { get { return Value; } }

        /// <summary>
        /// Returns the value of the pattern parameter.
        /// </summary>
        public virtual TValue Value { get; private set; }

        /// <summary>
        /// Returns a string that represents the value of the pattern parameter.
        /// </summary>
        /// <returns>Value as string</returns>
        public virtual string RenderToString() {
            return Value.ToString();
        }

        /// <summary>
        /// Gibt einen <see cref="T:System.String"/> zurück, der das aktuelle <see cref="T:System.Object"/> darstellt.
        /// </summary>
        /// <returns>
        /// Ein <see cref="T:System.String"/>, der das aktuelle <see cref="T:System.Object"/> darstellt.
        /// </returns>
        public override string ToString() {
            string result = string.Format("{0} Name '{1}' Value '{2}'", GetType().Name, Name, Value);
            return result;
        }
    }
}