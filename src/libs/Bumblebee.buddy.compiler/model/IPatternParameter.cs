using Bumblebee.buddy.compiler.exceptions;

namespace Bumblebee.buddy.compiler.model {
    /// <summary>
    /// Describes a pattern expression parameter.
    /// </summary>
    public interface IPatternParameter {
        /// <summary>
        /// Returns the name of the parameter.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Returns the type of the parameter.
        /// </summary>
        string Type { get; }

        /// <summary>
        /// Returns TRUE if the parameter is mandatory.
        /// </summary>
        bool Mandatory { get; }

        /// <summary>
        /// Returns the resolved runtime value of this parameter. May be NULL!
        /// </summary>
        IPatternParameterValue Value { get; }

        /// <summary>
        /// Evaluates the given <paramref name="expression"/> and returns an appropriated value as string. If the expression cannot be 
        /// evaluated, an <see cref="ExpressionEvaluationException"/> is thrown.
        /// </summary>
        /// <param name="expression">Expression to evaluate</param>
        /// <returns>Value as string</returns>
        /// <exception cref="ExpressionEvaluationException">If the given expression is not supported.</exception>
        string EvaluateExpression(string expression);
    }
}