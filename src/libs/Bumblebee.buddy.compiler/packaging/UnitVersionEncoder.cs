using System;
using System.Collections.Generic;
using System.Linq;

namespace Bumblebee.buddy.compiler.packaging {
    /// <summary>
    /// Provides methods to en- and decode version bounds of TDIL units.
    /// </summary>
    public static class UnitVersionEncoder {
        /// <summary>
        /// Returns TRUE if the given <paramref name="value"/> has been encode.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <returns>TRUE or FALSE</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="value"/> is NULL</exception>
        public static bool IsEncoded(string value) {
            if (value == null) throw new ArgumentNullException("value");

            char[] characters = value.ToCharArray();
            for (int i = -1; ++i != characters.Length;) {
                if (iEncodingTable.ContainsKey(characters[i])) return false;
            }

            return true;
        }

        /// <summary>
        /// Returns the decoded form of the given <paramref name="version"/>. If the version 
        /// is NULL or empty, NULL is returned.
        /// </summary>
        /// <param name="version">Version to decode</param>
        /// <returns>Decoded form of the version</returns>
        public static string Decode(string version) {
            if (string.IsNullOrEmpty(version)) return null;

            char[] encodedSet = version.ToCharArray();
            char[] plainSet = new char[encodedSet.Length];

            for (int i = -1; ++i != encodedSet.Length;) {
                char e = encodedSet[i];
                char c = DecodeCharacter(e);
                plainSet[i] = c;
            }

            return new string(plainSet);
        }

        /// <summary>
        /// Returns an encoded form of the given <paramref name="version"/>. If the version 
        /// is NULL or empty, NULL is returned.
        /// </summary>
        /// <param name="version">Version to encode</param>
        /// <returns>Encoded form of the version</returns>
        public static string Encode(string version) {
            if (string.IsNullOrEmpty(version)) return null;

            char[] originalSet = version.ToCharArray();
            char[] encodedSet = new char[originalSet.Length];

            for (int i = -1; ++i != originalSet.Length;) {
                char c = originalSet[i];
                char e = EncodeCharacter(c);
                encodedSet[i] = e;
            }

            return new string(encodedSet);
        }

        /// <summary>
        /// Returns the encoded form of the given character <paramref name="c"/>.
        /// </summary>
        /// <param name="c">Character to encode</param>
        /// <returns>Encoded form</returns>
        private static char EncodeCharacter(char c) {
            char e;
            if (!iEncodingTable.TryGetValue(c, out e)) return c;
            return e;
        }

        /// <summary>
        /// Returns the decoded form of the given character <paramref name="e"/>.
        /// </summary>
        /// <param name="e">Character to decode</param>
        /// <returns>Decoded form</returns>
        private static char DecodeCharacter(char e) {
            if (!iEncodingTable.ContainsValue(e)) return e;
            return iEncodingTable.First(entry => entry.Value == e).Key;
        }

        private static readonly Dictionary<char, char> iEncodingTable = new Dictionary<char, char> {
            {UnitVersionBounds.UnrestrainedToken.ToCharArray()[0], 's'}, {'.', 'd'}, {'-', 'u'} 
        };
    }
}