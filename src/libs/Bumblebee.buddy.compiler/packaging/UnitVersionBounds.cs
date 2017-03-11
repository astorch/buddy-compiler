using System;

namespace Bumblebee.buddy.compiler.packaging {
    /// <summary>
    /// Describes version bounds of a unit.
    /// </summary>
    public class UnitVersionBounds {
        /// <summary>
        /// Token that indicates an unrestrained version.
        /// </summary>
        public static readonly string UnrestrainedToken = "*";

        /// <summary>
        /// Localized text token of <see cref="UnrestrainedToken"/>.
        /// </summary>
        public static readonly string UnrestrainedLocalizedText = "Unbegrenzt";

        /// <summary>
        /// Returns the lower version bound of a test case.
        /// </summary>
        public string LowerBound { get; private set; }

        /// <summary>
        /// Returns the upper version bound of a test case.
        /// </summary>
        public string UpperBound { get; private set; }

        /// <summary>
        /// Returns the encoded form of the version.
        /// </summary>
        /// <returns>Encoded form of the version</returns>
        public string GetEncoded() {
            return UnitVersionEncoder.Encode(ToText());
        }

        /// <summary>
        /// Returns a localized text representation of the version.
        /// </summary>
        /// <returns>Localized text representation</returns>
        public string ToLocalizedText() {
            return ToText().Replace(UnrestrainedToken, UnrestrainedLocalizedText);
        }

        /// <summary>
        /// Returns a short localized text representation of the version. In this case, 
        /// just the <see cref="UnrestrainedLocalizedText"/> is returned if the upper bound is 
        /// unrestrained.
        /// </summary>
        /// <returns>Short localized text representation</returns>
        public string ToShortLocalizedText() {
            if (UpperBound == UnrestrainedToken) return UnrestrainedLocalizedText;
            return ToLocalizedText();
        }

        /// <summary>
        /// Returns a text representation of the version.
        /// </summary>
        /// <returns>Text representation</returns>
        public string ToText() {
            if (LowerBound == UpperBound) return LowerBound;
            return string.Format("{0}-{1}", LowerBound, UpperBound);
        }

        /// <inheritdoc />
        public override string ToString() {
            string result = string.Format("{0} ({1})", GetType().Name, ToText());
            return result;
        }

        /// <summary>
        /// Creates a new instance based on the given <paramref name="textValue"/>. The given value may be NULL or 
        /// empty. In this case, an unbound test case version is returned.
        /// </summary>
        /// <param name="textValue">Text value to evaluate</param>
        /// <returns>New instance</returns>
        public static UnitVersionBounds ParsePlain(string textValue) {
            if (string.IsNullOrEmpty(textValue))
                textValue = UnrestrainedToken;

            textValue = textValue.Replace(" ", string.Empty);
            string[] parts = textValue.Split(new[] {'-'}, StringSplitOptions.RemoveEmptyEntries);

            return new UnitVersionBounds {
                LowerBound = parts[0],
                UpperBound = (parts.Length == 2 ? parts[1] : parts[0])
            };
        }
    }
}