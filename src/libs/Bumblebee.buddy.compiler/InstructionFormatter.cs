using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Bumblebee.buddy.compiler.exceptions;
using Bumblebee.buddy.compiler.model;

namespace Bumblebee.buddy.compiler {
    /// <summary>
    /// Provides methods to create strings describing a directive using a given <see cref="IBuddyTranslationInstruction"/> and 
    /// a parameter dictionary.
    /// </summary>
    public class InstructionFormatter {

        private static readonly Regex iTdilPatternParameterRegex = new Regex(@"(?<paramRef>~((?<closure>\([^)]+\))|(?<name>(\w+\.\w+)|(\w+))))");

        /// <summary>
        /// Creates a string representing a directive based on the given <paramref name="actionStep"/> and the 
        /// given <paramref name="parameters"/>.
        /// </summary>
        /// <param name="actionStep">Action step to format</param>
        /// <param name="parameters">Parameters to use</param>
        /// <returns>String representing a directive based on the given action step</returns>
        /// <exception cref="ArgumentNullException">If any parameter is NULL</exception>
        /// <exception cref="InstructionFormattingException">If not all declared parameters could be replaced</exception>
        public virtual string ToString(IBuddyTranslationInstruction actionStep, IDictionary<string, IPatternParameter> parameters) {
            if (actionStep == null) throw new ArgumentNullException(nameof(actionStep));
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            string tdilPattern = actionStep.TdilPattern;
            string result = tdilPattern;

            if (actionStep is IReferableInstruction referableInstruction) {
                string resultReference = referableInstruction.ResultReferencePattern;
                resultReference = resultReference.Replace("#", "1");
                result = $"{resultReference} = {result}";
            }

            // Process all parameter references
            MatchCollection matches = iTdilPatternParameterRegex.Matches(result);
            for (IEnumerator matchItr = matches.GetEnumerator(); matchItr.MoveNext();) {
                Match match = (Match) matchItr.Current;
                Group paramRefGrp = match.Groups["paramRef"];
                Group closureGrp = match.Groups["closure"];
                Group nameGrp = match.Groups["name"];

                string parameterExpression = paramRefGrp.Value;
                string[] pureParamNames;

                // Do we have a closure with alternatives?
                if (closureGrp.Success) {
                    string coreFunction = parameterExpression.Substring(2, parameterExpression.Length - 3);
                    string[] alternatives = coreFunction.Split('|');
                    pureParamNames = alternatives;
                } else { // Plain old reference
                    pureParamNames = new[] {nameGrp.Value};
                }

                // Look up referenced pattern parameter
                string parameterValueFunction = null;
                IPatternParameter patternParameter = null;
                for (int i = -1; ++i != pureParamNames.Length;) {
                    string pureParamName = pureParamNames[i];
                    string reducedParamName = pureParamName.Split('.')[0];
                    if (parameters.TryGetValue(reducedParamName, out patternParameter)) {
                        parameterValueFunction = $"${pureParamName}";
                        break;
                    }
                }

                // Do we have one?
                if (patternParameter == null)
                    throw new InstructionFormattingException($"The TDIL pattern '{tdilPattern}' contains a mandatory parameter '{parameterExpression}' that could not be resolved!");

                // Calculate value
                string valueStr = patternParameter.EvaluateExpression(parameterValueFunction);
                if (valueStr == null) throw new InstructionFormattingException($"Expression '{parameterExpression}' could not be evaluated by pattern parameter of '{patternParameter}'");

                // Insert and replace
                result = result.Replace(parameterExpression, valueStr);
            }

            // Ensure all parameters have been replaced
            if (result.Contains('~')) 
                throw new InstructionFormattingException($"At least one parameter could not be set. Pattern is '{tdilPattern}'. Result is '{result}'.");

            return result;
        }
    }
}