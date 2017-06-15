using System;
using System.Collections.Generic;
using System.Text;
using Bumblebee.buddy.compiler.lang;
using xcite.collections;

namespace Bumblebee.buddy.compiler.collectiontools {
    /// <summary>
    /// Provides some string extension methods.
    /// </summary>
    public static class StringExtensionMethods {
        /// <summary>
        /// Returns an instance of <see cref="WordIterator"/> for this string.
        /// </summary>
        /// <param name="str">String to use for the word iterator</param>
        /// <returns>Newly created instance of <see cref="WordIterator"/></returns>
        public static WordIterator GetWordIterator(this string str) {
            return new WordIterator(str);
        }

        /// <summary>
        /// Returns an iterable collection of all words of the current string.
        /// </summary>
        /// <param name="str">String to process</param>
        /// <returns>Iterable collection of all words</returns>
        public static IEnumerable<WordIterator.Word> GetWords(this string str) {
            return new WordIterator(str).Words();
        }

        /// <summary>
        /// Removes all characters that are listed in the given collection <paramref name="chrToDrop"/> from the <paramref name="value"/> 
        /// string if they are not enclosed by special characters (parenthesis, brackets, quotes).
        /// </summary>
        /// <param name="value">String to process</param>
        /// <param name="chrToDrop">Collection of characters to drop</param>
        /// <returns><paramref name="value"/> string without <paramref name="chrToDrop"/> except those that have been quoted</returns>
        public static string RemoveCharactersBuddyStyle(this string value, char[] chrToDrop) {
            if (string.IsNullOrEmpty(value)) return value;
            
            StringBuilder stringBuilder = new StringBuilder(value.Length);
            
            char[] valueCharacters = value.ToCharArray();
            
            bool escapeMode = false;
            char escapeExitChar = default(char);

            for (int i = -1; ++i != valueCharacters.Length;) {
                char c = value[i];
                bool takeChar = false;

                // In escape mode just add and go on
                if (escapeMode) {
                    takeChar = true;
                    escapeMode = c != escapeExitChar;
                } else {
                    // Take if it's not on the index
                    if (Array.IndexOf(chrToDrop, c) == -1)
                        takeChar = true;

                    // Toggle escape mode
                    if (c.IsEscapeChar()) {
                        escapeMode = true;
                        escapeExitChar = c.GetEscapeExitChar();
                    }
                }

                if (takeChar) 
                    stringBuilder.Append(c);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Splits the string into buddy relevant tokens. Whitespaces keep preserved if they are 
        /// enclosed by special buddy characters.
        /// </summary>
        /// <param name="value">String to split</param>
        /// <returns>Buddy relevant tokens</returns>
        public static string[] SplitToBuddyTokens(this string value) {
            StringBuilder stringBuilder = new StringBuilder(50);
            LinearList<string> buddyTokens = new LinearList<string>();
            char[] charArray = value.ToCharArray();

            bool escapeMode = false;
            char escapeExitChar = default(char);

            for (int i = -1; ++i != charArray.Length;) {
                char c = charArray[i];

                // Handle quote mode
                if (c == BuddyLangTokens.QUOTE) {
                    stringBuilder.Append(c);

                    // Play forward until the quotation ends
                    while ((c = charArray[++i]) != BuddyLangTokens.QUOTE)
                        stringBuilder.Append(c);

                    stringBuilder.Append(c);

                    // Go back to the normal loop
                    continue;
                }

                if (escapeMode) {
                    stringBuilder.Append(c);

                    if (c == escapeExitChar) {
                        escapeMode = false;
                        AddIfNotEmpty(stringBuilder, buddyTokens);
                    }
                } else {
                    if (c == BuddyLangTokens.WS) {
                        AddIfNotEmpty(stringBuilder, buddyTokens);
                        continue;
                    }

                    stringBuilder.Append(c);

                    if (c.IsEscapeChar()) {
                        escapeMode = true;
                        escapeExitChar = c.GetEscapeExitChar();
                    }
                }
            }

            // Handle remaining fragments
            AddIfNotEmpty(stringBuilder, buddyTokens);

            // Return result
            return buddyTokens.ToArray();
        }

        /// <summary>
        /// Adds the current buffer of <paramref name="stringBuilder"/> to <paramref name="resultSet"/> if it's not empty 
        /// and clears it afterwards.
        /// </summary>
        /// <param name="stringBuilder">String builder</param>
        /// <param name="resultSet">Result set</param>
        private static void AddIfNotEmpty(StringBuilder stringBuilder, ICollection<string> resultSet) {
            string token = stringBuilder.ToString();
            stringBuilder.Clear();
            if (token.Length == 0) return;
            resultSet.Add(token);
        }

        /// <summary>
        /// Returns a new word sequence that contains buddy special characters as separate "words".
        /// </summary>
        /// <param name="wordSequence">Array of words to process</param>
        /// <returns>Extended array of words</returns>
        public static string[] StripSpecialCharacters(this string[] wordSequence) {
            LinearList<string> resultSet = new LinearList<string>();
            for (int i = -1; ++i != wordSequence.Length;) {
                string word = wordSequence[i];
                char head = word[0];

                if (head == BuddyLangTokens.LAB || head == BuddyLangTokens.LSB) {
                    string headStr = word.Substring(0, 1);
                    string coreStr = word.Substring(1, word.Length - 2);
                    string tailStr = word.Substring(word.Length - 1);

                    resultSet.Add(headStr);
                    resultSet.Add(coreStr);
                    resultSet.Add(tailStr);
                } else {
                    resultSet.Add(word);
                }
            }

            return resultSet.ToArray();
        }

        /// <summary>
        /// Returns TRUE if the character indicates a buddy language escape token.
        /// </summary>
        /// <param name="c">Character to check</param>
        /// <returns>TRUE or FALSE</returns>
        public static bool IsEscapeChar(this char c) {
            return c == BuddyLangTokens.QUOTE 
                || c == BuddyLangTokens.LAB 
                || c == BuddyLangTokens.LSB
                || c == BuddyLangTokens.LRB
                ;
        }

        /// <summary>
        /// Returns the buddy language escape exit token for the character.
        /// </summary>
        /// <param name="c">Character to check</param>
        /// <returns>Escape exit character</returns>
        public static char GetEscapeExitChar(this char c) {
            if (c == BuddyLangTokens.QUOTE) return BuddyLangTokens.QUOTE;
            if (c == BuddyLangTokens.LAB) return BuddyLangTokens.RAB;
            if (c == BuddyLangTokens.LSB) return BuddyLangTokens.RSB;
            if (c == BuddyLangTokens.LRB) return BuddyLangTokens.RRB;

            throw new InvalidOperationException("There is no escape exit token for the given char "+c);
        }
    }
}