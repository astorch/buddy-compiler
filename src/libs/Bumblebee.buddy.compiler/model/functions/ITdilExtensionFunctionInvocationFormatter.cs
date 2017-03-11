namespace Bumblebee.buddy.compiler.model.functions {
    /// <summary>
    /// Provides methods to format formal TDIL extension function invocations 
    /// based on textual descriptions.
    /// </summary>
    public interface ITdilExtensionFunctionInvocationFormatter {
        /// <summary>
        /// Returns TRUE if the given <paramref name="expression"/> contains a 
        /// textual TDIL extension function invocation.
        /// </summary>
        /// <param name="expression">Expression to check</param>
        /// <returns>TRUE if the expression contains a function invocation</returns>
        bool ContainsFunctionInvocation(string expression);

        /// <summary>
        /// Returns TRUE if a textual TDIL extension function invocation has been found and replaced 
        /// within the given <paramref name="expression"/> with the formal invocation. 
        /// If no invocation is found, the <paramref name="expression"/> is returned 
        /// using <paramref name="processedExpression"/>. Otherwise, this references the 
        /// processed expression.
        /// </summary>
        /// <param name="expression">Expression to process</param>
        /// <param name="processedExpression">Original or modified expression</param>
        /// <returns>TRUE if the expression has been modified</returns>
        bool TryFormatFunctionInvocation(string expression, out string processedExpression);

        /// <summary>
        /// Replaces any textual TDIL extension function invocation within the given <paramref name="expression"/> 
        /// with the formal invocation. If no invocation is found, the <paramref name="expression"/> is returned 
        /// unmodified.
        /// </summary>
        /// <param name="expression">Expression to process</param>
        /// <returns>Processed expression</returns>
        string FormatFunctionInvocation(string expression);
    }
}