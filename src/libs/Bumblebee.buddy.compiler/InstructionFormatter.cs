using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Bumblebee.buddy.compiler.exceptions;
using Bumblebee.buddy.compiler.model;

namespace Bumblebee.buddy.compiler {
    /// <summary>
    /// Provides methods to create strings describing a directive using a given <see cref="IBuddyTranslationInstruction"/> and 
    /// a parameter dictionary.
    /// </summary>
    public class InstructionFormatter {

        private static readonly Regex _tdilPatternParameterRegex = new Regex(@"(?<paramRef>~((?<closure>\([^)]+\))|(?<name>(\w+\.\w+)|(\w+))))");

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
            MatchCollection matches = _tdilPatternParameterRegex.Matches(result);
            for (IEnumerator matchItr = matches.GetEnumerator(); matchItr.MoveNext();) {
                Match match = (Match) matchItr.Current;
                if (match == null) continue;
                
                Group paramRefGrp = match.Groups["paramRef"];
                Group closureGrp = match.Groups["closure"];
                Group nameGrp = match.Groups["name"];

                EClosureType closureType = EClosureType.None;
                string parameterExpression = paramRefGrp.Value;
                string[] pureParamNames;

                // Do we have a closure with alternatives?
                if (closureGrp.Success) {
                    string coreFunction = parameterExpression.Substring(2, parameterExpression.Length - 3);
                    if (coreFunction.IndexOf('|') != -1) {
                        closureType = EClosureType.Alternative;
                        pureParamNames = coreFunction.Split('|');
                    } else {
                        closureType = EClosureType.Conjunction;
                        pureParamNames = coreFunction.Split('&');
                    }
                } else { // Plain old reference
                    pureParamNames = new[] {nameGrp.Value};
                }

                // Look up referenced pattern parameter
                string parameterValueFunction = null;
                IPatternParameter patternParameter = null;
                for (int i = -1; ++i != pureParamNames.Length;) {
                    string pureParamName = pureParamNames[i];
                    string reducedParamName = pureParamName.Split('.')[0];
                    if (parameters.TryGetValue(reducedParamName, out var locPatternParameter)) {
                        string assignedValueFunction = $"${pureParamName}";
                        
                        // When this is a conjunction, we have to combine all pattern parameter
                        if (closureType == EClosureType.Conjunction) {
                            parameterValueFunction = string.Concat(parameterValueFunction ?? string.Empty, ",", assignedValueFunction);
                            patternParameter = Combine(patternParameter, locPatternParameter);
                            continue;
                        }

                        // In all other cases, just resolve the first matching pattern parameter
                        patternParameter = locPatternParameter;
                        parameterValueFunction = assignedValueFunction;
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

        /// <summary> Closure type enumeration values </summary>
        enum EClosureType {
            
            /// <summary> No closure </summary>
            None,
            
            /// <summary> Alternative closure </summary>
            Alternative,
            
            /// <summary> Conjunction closure </summary>
            Conjunction
            
        }

        /// <summary>
        /// Returns an instance of <see cref="IPatternParameter"/> that combines the given
        /// pattern parameter instances.
        /// </summary>
        private static IPatternParameter Combine(IPatternParameter pp1, IPatternParameter pp2) {
            if (pp1 == null) return pp2;
            if (pp2 == null) return pp1;
            
            if (pp1 is CombinedPatternParameter cpp1)
                cpp1.Add(pp2);
            else if (pp2 is CombinedPatternParameter cpp2)
                cpp2.Add(pp1);

            return new CombinedPatternParameter(pp1, pp2);
        }

        /// <summary>
        /// Implementation of <see cref="IPatternParameter"/> that combines and evaluates
        /// multiple <see cref="IPatternParameter"/> in sequence.
        /// </summary>
        class CombinedPatternParameter : IPatternParameter {
            private readonly List<IPatternParameter> _sequence = new List<IPatternParameter>(5);

            /// <summary> Creates a new instance by combining the given instances. </summary>
            public CombinedPatternParameter(IPatternParameter pp1, IPatternParameter pp2) {
                _sequence.Add(pp1);
                _sequence.Add(pp2);
            }

            /// <summary> Adds the given instance to the sequence. </summary>
            public void Add(IPatternParameter pp) {
                _sequence.Add(pp);
            }

            /// <inheritdoc />
            public string Name { get; } = null;

            /// <inheritdoc />
            public string Type { get; } = null;

            /// <inheritdoc />
            public bool Mandatory { get; } = false;

            /// <inheritdoc />
            public IPatternParameterValue Value { get; } = null;

            /// <inheritdoc />
            public string EvaluateExpression(string expression) {
                string[] exprParts = expression.Split(new []{','}, StringSplitOptions.RemoveEmptyEntries);
                StringBuilder sb = new StringBuilder(1024);

                for (int i = -1, ilen = exprParts.Length; ++i != ilen;) {
                    string exprPart = exprParts[i];
                    IPatternParameter patternParameter = _sequence[i];
                    string result = patternParameter.EvaluateExpression(exprPart);
                    sb.Append($"{result}, ");
                }

                return sb.Length == 0
                        ? string.Empty
                        : sb.ToString(0, sb.Length - 2)
                    ;
            }
            
        }
    }
}