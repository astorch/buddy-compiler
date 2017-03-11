using System;
using System.Text.RegularExpressions;
using Xcite.Collections;
using Xcite.Csharp.assertions;

namespace Bumblebee.buddy.compiler.packaging {
    /// <summary>
    /// Describes a routine of a unit.
    /// </summary>
    public class UnitRoutineInfo {
        /// <summary>
        /// Creates a new instance with the given values.
        /// </summary>
        /// <param name="name">Name of the unit method</param>
        /// <param name="arguments">Unit method parameters</param>
        public UnitRoutineInfo(string name, UnitRoutineArgumentInfo[] arguments) {
            Name = name;
            Arguments = arguments;
        }

        /// <summary>
        /// Returns the name of the routine.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Returns the arguments of the routine. 
        /// </summary>
        public UnitRoutineArgumentInfo[] Arguments { get; private set; }

        /// <summary>
        /// Returns a set of <see cref="UnitRoutineInfo"/> for each found routine within the given 
        /// unit <paramref name="fileContent"/>.
        /// </summary>
        /// <param name="fileContent">Unit file content to evaluate</param>
        /// <returns>A set of <see cref="UnitRoutineInfo"/></returns>
        /// <exception cref="ArgumentNullException">If <paramref name="fileContent"/> is NULL or empty</exception>
        public static UnitRoutineInfo[] ParseFromFile(string fileContent) {
            Assert.NotNullOrEmpty(() => fileContent);

            LinearList<UnitRoutineInfo> methodInfoSet = new LinearList<UnitRoutineInfo>();
            MatchCollection matchCollection = Regex.Matches(fileContent, "^(?<name>\\w+):(\\((?<args>[^\\)]+)\\))?", RegexOptions.Multiline);
            for (int i = -1; ++i != matchCollection.Count;) {
                Match match = matchCollection[i];
                string methodName = match.Groups["name"].Value;
                if (methodName == "Main") continue;
                string arguments = match.Groups["args"].Value;
                UnitRoutineArgumentInfo[] parameterSet = ParseUnitRoutineArguments(arguments);
                methodInfoSet.Add(new UnitRoutineInfo(methodName, parameterSet));
            }

            return methodInfoSet.ToArray();
        }

        /// <summary>
        /// Parses the given <paramref name="argumentLine"/> and returns all resolved unit routine arguments. 
        /// If the <paramref name="argumentLine"/> is NULL or empty, an empty set is returned.
        /// </summary>
        /// <param name="argumentLine">Arguments to parse</param>
        /// <returns>Set of <see cref="UnitRoutineArgumentInfo"/>. May be empty</returns>
        private static UnitRoutineArgumentInfo[] ParseUnitRoutineArguments(string argumentLine) {
            if (string.IsNullOrEmpty(argumentLine)) return new UnitRoutineArgumentInfo[0];

            string[] parts = argumentLine.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
            UnitRoutineArgumentInfo[] arguments = parts.Index().Select((i, str) => new UnitRoutineArgumentInfo(i, str.Trim())).ToArray();

            return arguments;
        }
    }

    /// <summary>
    /// Describes a parameter of a unit routine.
    /// </summary>
    public class UnitRoutineArgumentInfo {
        /// <summary>
        /// Creates a new instance with the given values.
        /// </summary>
        /// <param name="name">Name of the parameter</param>
        /// <param name="index">Index of the parameter</param>
        public UnitRoutineArgumentInfo(int index, string name) {
            Name = name;
            Index = index;
        }

        /// <summary>
        /// Returns the name of the parameter.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Returns the index of the parameter.
        /// </summary>
        public int Index { get; private set; }
    }
}