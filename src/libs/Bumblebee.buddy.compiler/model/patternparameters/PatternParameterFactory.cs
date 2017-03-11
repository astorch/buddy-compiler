using System.Collections.Generic;
using Bumblebee.buddy.compiler.exceptions;
using Bumblebee.buddy.compiler.model.patternparameters.types;

namespace Bumblebee.buddy.compiler.model.patternparameters {
    /// <summary>
    /// Provides a factory to create instances of <see cref="AbstractPatternParameter"/> based on given pattern type names.
    /// </summary>
    public class PatternParameterFactory {
        /// <summary>
        /// Creates a new instance of <see cref="IPatternParameter"/> based on the given property set. The set must contain the properties 
        /// for 'name' and 'type' at least.
        /// </summary>
        /// <param name="keyValueSet">Property set</param>
        /// <returns>New instance of <see cref="IPatternParameter"/></returns>
        /// <exception cref="UnkownPatternParameterTypeNameException">If the requested type name is not known</exception>
        public IPatternParameter CreatePatternParameter(Dictionary<string, string> keyValueSet) {
            string parameterName = keyValueSet["name"];
            string typeName = keyValueSet["type"];

            AbstractPatternParameter patternParameter = CreatePatternParameter(parameterName, typeName);
            patternParameter.Mandatory = true; // By default

            // Apply custom set value
            string mandatoryStr;
            if (keyValueSet.TryGetValue("mandatory", out mandatoryStr))
                patternParameter.Mandatory = bool.Parse(mandatoryStr);

            return patternParameter;
        }

        /// <summary>
        /// Creates an instance of <see cref="AbstractPatternParameter"/> depending of the given <paramref name="typeName"/>. If the 
        /// type name cannot be mapped, an <see cref="UnkownPatternParameterTypeNameException"/> is thrown.
        /// </summary>
        /// <param name="parameterName">Name of the pattern parameter</param>
        /// <param name="typeName">Type of the pattern parameter</param>
        /// <returns>An instance of <see cref="AbstractPatternParameter"/></returns>
        /// <exception cref="UnkownPatternParameterTypeNameException">If the requested type name is not known</exception>
        public AbstractPatternParameter CreatePatternParameter(string parameterName, string typeName) {
            if (typeName == "alias") return new AliasPatternParameter(parameterName);
            if (typeName == "condition") return new ConditionPatternParameter(parameterName);
            if (typeName == "string") return new StringPatternParameter(parameterName);
            if (typeName == "frequence") return new FrequencePatternParameter(parameterName);
            if (typeName == "param") return new ParamPatternParameter(parameterName);
            if (typeName == "keyword") return new KeywordPatternParameter(parameterName);
            if (typeName == "unitReference") return new UnitReferencePatternParameter(parameterName);
            if (typeName == "key") return new KeyPatternParameter(parameterName);
            if (typeName == "parameterSet") return new ParameterSetPatternParameter(parameterName);
            if (typeName == "aliaskeyword") return new AliasKeywordPatternParameter(parameterName);
            
            throw new UnkownPatternParameterTypeNameException(string.Format("The given type name '{0}' cannot be mapped! Parameter name is '{1}'", typeName, parameterName));
        }
    }
}