using System.Collections.Generic;
using System.Text.RegularExpressions;
using Bumblebee.buddy.compiler.model.patternparameters.adjustments;

namespace Bumblebee.buddy.compiler.model.functions {
    /// <summary>
    /// Implements a registry of TDIL extension functions.
    /// </summary>
    public class TdilExtensionFunctionRegistry {
        /// <summary>
        /// Returns the default instance of this class.
        /// </summary>
        public static readonly TdilExtensionFunctionRegistry Default = new TdilExtensionFunctionRegistry();

        private readonly List<TdilExtensionFunctionInfo> iFunctionTable = new List<TdilExtensionFunctionInfo>(17);
        private TdilExtensionInvocationFormatter iInvocationFormatter;

        /// <summary>
        /// Adds the given <paramref name="functionInfo"/> to the registry.
        /// </summary>
        /// <param name="functionInfo">Function to add</param>
        public void Add(TdilExtensionFunctionInfo functionInfo) {
            iFunctionTable.Add(functionInfo);
        }

        /// <summary>
        /// Returns an instance of <see cref="ITdilExtensionFunctionInvocationFormatter"/>.
        /// </summary>
        /// <returns>Instance of <see cref="ITdilExtensionFunctionInvocationFormatter"/></returns>
        public ITdilExtensionFunctionInvocationFormatter GetInvocationFormatter() {
            return iInvocationFormatter ?? (iInvocationFormatter = new TdilExtensionInvocationFormatter(this));
        }

        /// <summary>
        /// Provides an anonymous implementation of <see cref="ITdilExtensionFunctionInvocationFormatter"/>.
        /// </summary>
        class TdilExtensionInvocationFormatter : ITdilExtensionFunctionInvocationFormatter {
            private readonly TdilExtensionFunctionRegistry iExtensionFunctionRegistry;

            /// <summary>
            /// Creates a new instance that utilizes the given <paramref name="extensionFunctionRegistry"/>.
            /// </summary>
            /// <param name="extensionFunctionRegistry">Registry that provides data about available functions</param>
            public TdilExtensionInvocationFormatter(TdilExtensionFunctionRegistry extensionFunctionRegistry) {
                iExtensionFunctionRegistry = extensionFunctionRegistry;
            }

            /// <inheritdoc />
            public bool ContainsFunctionInvocation(string expression) {
                TdilExtensionFunctionInfo fInfo;
                Match match;
                return IsFunctionInvocation(expression, out match, out fInfo);
            }

            /// <inheritdoc />
            public bool TryFormatFunctionInvocation(string expression, out string processedExpression) {
                processedExpression = expression;
                TdilExtensionFunctionInfo function;
                Match match;
                if (!IsFunctionInvocation(expression, out match, out function)) return false;
                processedExpression = AdjustFunctionCall(expression, match, function);
                return true;
            }

            /// <inheritdoc />
            public string FormatFunctionInvocation(string expression) {
                TdilExtensionFunctionInfo function;
                Match match;
                if (!IsFunctionInvocation(expression, out match, out function)) return expression;
                return AdjustFunctionCall(expression, match, function);
            }

            /// <summary>
            /// Returns TRUE if the given <paramref name="expression"/> contains a textual TDIL extension function invocation. 
            /// If so, the out parameters <paramref name="match"/> describes the match of the regular expression and
            /// <paramref name="function"/> describes the function to invoke.
            /// </summary>
            /// <param name="expression">Expression to check</param>
            /// <param name="match">Found match. May be NULL</param>
            /// <param name="function">Identified function. May be NULL</param>
            /// <returns>TRUE if a function invocation has been found</returns>
            private bool IsFunctionInvocation(string expression, out Match match, out TdilExtensionFunctionInfo function) {
                List<TdilExtensionFunctionInfo> functionTable = iExtensionFunctionRegistry.iFunctionTable;
                
                function = null;
                match = null;
                if (string.IsNullOrEmpty(expression)) return false;
                string trdExpr = expression.Trim('\"');
                if (string.IsNullOrEmpty(trdExpr)) return false;

                for (int i = -1; ++i != functionTable.Count; ) {
                    TdilExtensionFunctionInfo funcInfo = functionTable[i];
                    Match regexMatch = Regex.Match(expression, funcInfo.RegularExpression);
                    if (regexMatch.Success) {
                        function = funcInfo;
                        match = regexMatch;
                        return true;
                    }
                }

                return false;
            }

            /// <summary>
            /// Replaces the given <paramref name="match"/> within the containing <paramref name="expression"/> 
            /// with the given <paramref name="function"/> invocation.
            /// </summary>
            /// <param name="expression">Expression that contains the match</param>
            /// <param name="match">Match of a function invocation</param>
            /// <param name="function">Function that is being called</param>
            /// <returns></returns>
            private string AdjustFunctionCall(string expression, Match match, TdilExtensionFunctionInfo function) {
                string resultFormat = function.ResultFormat;

                // Process function arguments
                TdilExtensionFunctionArgumentInfo[] arguments = function.Arguments;
                for (int i = -1; ++i != arguments.Length; ) {
                    TdilExtensionFunctionArgumentInfo argument = arguments[i];
                    string argName = argument.Name;
                    Group matchGroup = match.Groups[argName];
                    string adjustment = SelectAdjustmentValue(argument.Adjustment);
                    string replacement = SelectReplacement(matchGroup.Success, 
                        matchGroup.Value, adjustment, argument.DefaultValue);
                    resultFormat = resultFormat.Replace(argName, replacement);
                }

                // Update expression
                string expr = expression.Replace(match.Value, resultFormat);

                // Remove leading and trailing quotes
                string trmdExpr = expr.Trim('\"');
                return trmdExpr;
            }

            private string SelectReplacement(bool groupMatch, string matchValue, string adjustmentValue, string defaultValue) {
                if (groupMatch) return matchValue;
                if (adjustmentValue != null) return adjustmentValue;
                return defaultValue;
            }

            private string SelectAdjustmentValue(string adjustmentReference) {
                if (string.IsNullOrEmpty(adjustmentReference)) return null;
                
                ParameterAdjustmentTable pat = ParameterAdjustmentTable.Current;
                if (pat == null) return null;

                string adjustmentValue = pat.GetValue(adjustmentReference);
                return adjustmentValue;
            }
        }
    }
}