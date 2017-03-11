using System;

namespace Bumblebee.buddy.compiler.packaging {
    /// <summary>
    /// Provides methods to en- and decode names of TDIL units.
    /// </summary>
    public static class UnitNameEncoder {
        private const char Whitespace = ' ';
        private const char Underscore = '_';

        /// <summary>
        /// Encodes the given <paramref name="value"/>. An encoded unit name can be processed by 
        /// an TDIL execution engine.
        /// </summary>
        /// <param name="value">Value to encode</param>
        /// <returns>Encoded value</returns>
        public static string Encode(string value) {
            return value.Replace(Whitespace, Underscore);
        }

        /// <summary>
        /// Decodes the given <paramref name="value"/>. This operation is the opposite of <see cref="Encode"/>.
        /// </summary>
        /// <param name="value">Value to decode</param>
        /// <returns>Decoded value</returns>
        public static string Decode(string value) {
            return value.Replace(Underscore, Whitespace);
        }

        /// <summary>
        /// Returns TRUE if the given <paramref name="value"/> has been encode.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <returns>TRUE or FALSE</returns>
        public static bool IsEncoded(string value) {
            if (value == null) throw new ArgumentNullException("value");
            return !value.Contains(string.Empty + Whitespace);
        }
    }
}