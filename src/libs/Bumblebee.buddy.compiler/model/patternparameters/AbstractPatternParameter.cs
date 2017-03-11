using Bumblebee.buddy.compiler.exceptions;
using Bumblebee.buddy.compiler.model.patternparameters.values;

namespace Bumblebee.buddy.compiler.model.patternparameters {
    /// <summary>
    /// Provides an abstract implementation of <see cref="IPatternParameter"/>.
    /// </summary>
    public abstract class AbstractPatternParameter : IPatternParameter {
        
        /// <summary>
        /// Creates a new instance based on the given parameters.
        /// </summary>
        /// <param name="name">Name of the parameter</param>
        /// <param name="type">Type of the parameter</param>
        public AbstractPatternParameter(string name, string type) {
            Name = name;
            Type = type;
        }

        /// <summary>
        /// Returns the name of the parameter.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Returns the type of the parameter.
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// Returns TRUE if the parameter is mandatory.
        /// </summary>
        public bool Mandatory { get; set; }

        /// <summary>
        /// Returns the resolved runtime value of this parameter. May be NULL!
        /// </summary>
        public IPatternParameterValue Value { get; private set; }

        /// <summary>
        /// Evaluates the given <paramref name="expression"/> and returns an appropriated value as string. If the expression cannot be 
        /// evaluated, an <see cref="ExpressionEvaluationException"/> is thrown.
        /// </summary>
        /// <param name="expression">Expression to evaluate</param>
        /// <returns>Value as string</returns>
        /// <exception cref="ExpressionEvaluationException">If the given expression is not supported.</exception>
        public virtual string EvaluateExpression(string expression) {
            // TODO Check all

            // Default behaviour
            if (string.IsNullOrEmpty(expression)) return Value.RenderToString();

            if (expression.EndsWith(".Name")) return Value.Name;

            string product = Value.RenderToString();
            if (expression == "$" + Name) return product;
            if (expression.EndsWith(".Value")) return product;

            return ThrowEvaluationException(expression);
        }

        /// <summary>
        /// Sets the value of this pattern parameter based on the given <paramref name="runtimeExpression"/>. Clients may 
        /// override this implementation, but it's not intended. Instead, override <see cref="OnConvertValue"/> to add custom 
        /// value conversion logic, because this method delegates the invocation to it.
        /// </summary>
        /// <param name="runtimeExpression">Expression that describes the runtime value of this parameter</param>
        public virtual void SetValue(string runtimeExpression) {
            Value = OnConvertValue(runtimeExpression);
        }

        /// <summary>
        /// Notifies the instance that the given value may be converted to a specific value type. This method is intended to be 
        /// overriden by clients. The default implementation simply creates an instance of <see cref="StringPatternParameterValue"/> 
        /// with the given <paramref name="runtimeExpression"/> and returns it.
        /// </summary>
        /// <param name="runtimeExpression">Expression that describes the runtime value of this parameter</param>
        /// <returns>Instance of <see cref="IPatternParameterValue"/> based on the given expression <paramref name="runtimeExpression"/></returns>
        protected virtual IPatternParameterValue OnConvertValue(string runtimeExpression) {
            return new StringPatternParameterValue(runtimeExpression);
        }

        /// <summary>
        /// Notifies the instance that a property based on the given parameters shall be added to this instance. This method 
        /// method is intended to be overriden by clients. The default implementation simply creates an instance of <see cref="StringPatternParameterValue"/> 
        /// with the given <paramref name="propertyArgument"/> and returns it.
        /// </summary>
        /// <param name="propertyName">Name of the property to create</param>
        /// <param name="propertyArgument">Property argument to set</param>
        /// <returns>Instance of <see cref="IPatternParameterValue"/> based on the given invocation parameters</returns>
        protected virtual IPatternParameterValue OnPropertyAddition(string propertyName, string propertyArgument) {
            return new StringPatternParameterValue(propertyArgument);
        }

        /// <summary>
        /// Throws an <see cref="ExpressionEvaluationException"/> based on the given <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">Expression to embed into the exception messages</param>
        /// <returns>NEVER</returns>
        protected virtual string ThrowEvaluationException(string expression) {
            throw new ExpressionEvaluationException(string.Format("The given expression '{0}' is not supported by this type!", expression));
        }

        /// <summary>
        /// Throws an <see cref="ValueConversionException"/> based on the given <paramref name="runtimeExpression"/>.
        /// </summary>
        /// <param name="runtimeExpression">Expression to embed into the exception message</param>
        /// <returns>NEVER</returns>
        protected virtual IPatternParameterValue ThrowValueConversionException(string runtimeExpression) {
            throw new ValueConversionException(string.Format("The given expression '{0}' cannot be converted into an processable value by this pattern parameter '{1}'!", runtimeExpression, this));
        }

        /// <summary>
        /// Returns a string representation of this object.
        /// </summary>
        /// <returns>String representation of this object</returns>
        public override string ToString() {
            string result = string.Format("{0} '{1}' Value: '{2}'", GetType().Name, Name, (Value == null ? null : Value.RenderToString()));
            return result;
        }
    }
}