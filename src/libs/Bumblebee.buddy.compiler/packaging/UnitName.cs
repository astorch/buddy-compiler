using System;
using System.Text.RegularExpressions;
using Bumblebee.buddy.compiler.exceptions;
using xcite.csharp;
using xcite.csharp.assertions;

namespace Bumblebee.buddy.compiler.packaging {
    /// <summary>
    /// Describes the name of a TDIL unit.
    /// </summary>
    public class UnitName {
        private const char Separator = '.';

        private static readonly string UnitNameRegexPattern = string.Format(@"Unit (?<unitName>[\w-_]+\{0}[\d\a-z]+\{0}[\w-_]+\{0}[\w-_]+)", Separator);

        /// <summary>
        /// Creates a new instance based on the given full qualified unit name. If the given <paramref name="fullQualifiedUnitName"/> is NULL or empty, 
        /// a <see cref="ArgumentNullException"/> is thrown. Additionally, an <see cref="UnitNameFormatException"/> is thrown when the value 
        /// does not match the unit name format.
        /// </summary>
        /// <param name="fullQualifiedUnitName">Full qualified unit name</param>
        /// <exception cref="ArgumentNullException">If <paramref name="fullQualifiedUnitName"/> is NULL or empty</exception>
        /// <exception cref="UnitNameFormatException">If the unit name format is invalid</exception>
        public UnitName(string fullQualifiedUnitName) {
            string[] nameFragments = Assert.NotNullOrEmpty(() => fullQualifiedUnitName).Split(new[] { Separator }, StringSplitOptions.RemoveEmptyEntries);
            if (nameFragments.Length != 4) throw new UnitNameFormatException(string.Format("Unit name '{0}' does not match the unit name format", fullQualifiedUnitName));
            if (!UnitNameEncoder.IsEncoded(nameFragments[0])) throw new UnitNameFormatException(String.Format("Unit name '{0}' does not match the unit name format", fullQualifiedUnitName));
            if (!UnitVersionEncoder.IsEncoded(nameFragments[1])) throw new UnitNameFormatException(String.Format("Unit name '{0}' does not match the unit name format", fullQualifiedUnitName));

            ApplicationFragment = UnitNameEncoder.Decode(nameFragments[0]);
            VersionFragment = UnitVersionEncoder.Decode(nameFragments[1]);
            UseCaseFragment = UnitNameEncoder.Decode(nameFragments[2]);
            ScenarioFragment = UnitNameEncoder.Decode(nameFragments[3]);
        }

        /// <summary>
        /// Creates a new instance based on the given arguments.
        /// </summary>
        /// <param name="applicationName">Application fragment value</param>
        /// <param name="versionName">Version fragment value</param>
        /// <param name="useCaseName">Use case fragment value</param>
        /// <param name="scenarioName">Scenario fragment value</param>
        /// <exception cref="ArgumentNullException">If any argument is NULL</exception>
        public UnitName(string applicationName, string versionName, string useCaseName, string scenarioName) {
            ApplicationFragment = Assert.NotNullOrEmpty(() => applicationName);
            VersionFragment = Assert.NotNullOrEmpty(() => versionName);
            UseCaseFragment = Assert.NotNullOrEmpty(() => useCaseName);
            ScenarioFragment = Assert.NotNullOrEmpty(() => scenarioName);
        }

        /// <summary>
        /// Returns the application fragment value.
        /// </summary>
        public string ApplicationFragment { get; private set; }

        /// <summary>
        /// Returns the version fragment value.
        /// </summary>
        public string VersionFragment { get; private set; }

        /// <summary>
        /// Returns the use case fragment value.
        /// </summary>
        public string UseCaseFragment { get; private set; }

        /// <summary>
        /// Returns the scenario fragment value.
        /// </summary>
        public string ScenarioFragment { get; private set; }

        /// <summary>
        /// Returns the full qualified name of the unit.
        /// </summary>
        /// <returns>Full qualified unit name</returns>
        public string ToQualifiedString() {
            string encodedApplicationName = UnitNameEncoder.Encode(ApplicationFragment);
            string encodedVersion = UnitVersionEncoder.Encode(VersionFragment);
            string encodedUseCaseName = UnitNameEncoder.Encode(UseCaseFragment);
            string encodedScenarioName = UnitNameEncoder.Encode(ScenarioFragment);
            string fullName = string.Format("{0}{4}{1}{4}{2}{4}{3}", 
                encodedApplicationName, encodedVersion, encodedUseCaseName, encodedScenarioName, Separator);
            return fullName;
        }

        /// <summary>
        /// Returns the encoded form of the <see cref="ScenarioFragment"/>.
        /// </summary>
        /// <returns>Encoded form of the <see cref="ScenarioFragment"/></returns>
        public string GetEncodedScenarioName() {
            return UnitNameEncoder.Encode(ScenarioFragment);
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return string.Format("{0} '{1}'", GetType().Name, ToQualifiedString());
        }

        /// <inheritdoc />
        public override bool Equals(object obj) {
            if (ReferenceEquals(obj, null)) return false;
            if (ReferenceEquals(obj, this)) return true;

            UnitName unitName = obj as UnitName;
            if (unitName == null) return false;

            if (!Equals(ApplicationFragment, unitName.ApplicationFragment)) return false;
            if (!Equals(VersionFragment, unitName.VersionFragment)) return false;
            if (!Equals(UseCaseFragment, unitName.UseCaseFragment)) return false;
            if (!Equals(ScenarioFragment, unitName.ScenarioFragment)) return false;

            return true;
        }

        /// <inheritdoc />
        public override int GetHashCode() {
            return new HashCodeBuilder()
                .AddObjectHash(ApplicationFragment)
                .AddObjectHash(VersionFragment)
                .AddObjectHash(UseCaseFragment)
                .AddObjectHash(ScenarioFragment)
                .GetResult();
        }

        /// <summary>
        /// Returns a new instance of <see cref="UnitName"/> that is being parsed from a string that must contain a full TDIL unit.
        /// </summary>
        /// <param name="fileContent">String containing a full TDIL unit</param>
        /// <returns>New instance of <see cref="UnitName"/></returns>
        /// <exception cref="ArgumentNullException">If <paramref name="fileContent"/> is NULL or empty</exception>
        public static UnitName ParseFromFile(string fileContent) {
            Assert.NotNullOrEmpty(() => fileContent);
            string unitNameString = Regex.Match(fileContent, UnitNameRegexPattern).Groups["unitName"].Value;
            return new UnitName(unitNameString);
        }
    }
}