using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Bumblebee.buddy.compiler.collectiontools {
    /// <summary> Implements an <see cref="IEnumerator{T}"/> to iterate all words of a string. </summary>
    public class WordIterator : IEnumerator<WordIterator.Word> {
        private string _stringData;
        private readonly StringBuilder _wordStringBuilder = new StringBuilder();

        /// <summary> Initializes the new instance and binds it to the given <paramref name="stringData"/>. </summary>
        /// <exception cref="ArgumentNullException">When <paramref name="stringData"/> is NULL.</exception>
        public WordIterator(string stringData) {
            _stringData = stringData ?? throw new ArgumentNullException(nameof(stringData));
            CursorIndex = 0;
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose() {
            // Currently nothing to do here
        }

        /// <inheritdoc />
        object IEnumerator.Current 
            => Current;

        /// <summary>Gets the element in the collection at the current position of the enumerator.</summary>
        /// <returns>The element in the collection at the current position of the enumerator.</returns>
        public Word Current { get; private set; }

        /// <summary>Advances the enumerator to the next element of the collection.</summary>
        /// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
        /// <filterpriority>2</filterpriority>
        public bool MoveNext() {
            if (!CanMoveNext) return false;
            Current = NextWord();
            return (Current != null);
        }

        /// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
        /// <filterpriority>2</filterpriority>
        public void Reset() {
            CursorIndex = 0;
        }

        /// <summary>
        /// Returns the current index of the string cursor.
        /// </summary>
        public int CursorIndex { get; private set; }

        /// <summary>
        /// Returns TRUE if the cursor can move to a next word.
        /// </summary>
        private bool CanMoveNext 
            => CursorIndex < _stringData.Length;

        /// <summary>
        /// Returns the next word based on the current cursor index.
        /// </summary>
        /// <returns>Next word or NULL</returns>
        private Word NextWord() {
            _wordStringBuilder.Clear();

            int begin = CursorIndex;
            
            bool escapeMode = false;
            char escapeExitChar = default(char);

            while (CanMoveNext) {
                char c = _stringData[CursorIndex++];
                if (c == '\r') continue; // Ignore

                // Check for exit
                if (escapeMode) {
                    escapeMode = c != escapeExitChar;
                } else {
                    // Check for word finalizer
                    if (c == ' ' || c == '.' || c == ',' || c == '\n') {
                        string word = _wordStringBuilder.ToString();
                        if (string.IsNullOrEmpty(word)) continue;
                        Word wordObj = new Word(this, word, begin, CursorIndex - 1);
                        return wordObj;
                    }

                    // Toggle escape mode
                    if (c.IsEscapeChar()) {
                        escapeMode = true;
                        escapeExitChar = c.GetEscapeExitChar();
                    }
                }

                _wordStringBuilder.Append(c);
            }

            return null;
        }

        /// <summary>
        /// Returns an enumerable collection of all words of the iterated string.
        /// </summary>
        /// <returns>Collection of all words</returns>
        public IEnumerable<Word> Words() {
            while (CanMoveNext)
                yield return NextWord();
        } 

        /// <summary>
        /// Replaces the occurrence of the given <paramref name="word"/> with the given <paramref name="value"/>. Note that 
        /// the underlying string is changed, so that this operation may influence further results.
        /// </summary>
        /// <param name="word">Word to replace</param>
        /// <param name="value">Value to set instead</param>
        public void Replace(Word word, string value) {
            if (word == null) return;

            string originalWord = word.Text;
            _stringData = ReplaceFirst(_stringData, originalWord, value, word.Begin);
            int newCursorIndex = word.Begin + value.Length;
            CursorIndex = newCursorIndex;
        }

        /// <summary>
        /// Replaces the first occurrence of the given <paramref name="search"/> string with the given <paramref name="replace"/> string 
        /// beginning at the given <paramref name="begin"/> index.
        /// </summary>
        /// <param name="text">Text to process</param>
        /// <param name="search">Search string to replace</param>
        /// <param name="replace">Replace string to set instead</param>
        /// <param name="begin">Index to begin the replacement</param>
        /// <returns>Text with replaced value</returns>
        private string ReplaceFirst(string text, string search, string replace, int begin) {
            int pos = text.IndexOf(search, begin);
            if (pos == -1) return text;
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        /// <summary>
        /// Removes the occurrence of the given <paramref name="word"/>. Note that 
        /// the underlying string is changed, so that this operation may influence further results.
        /// </summary>
        /// <param name="word">Word to remove</param>
        public void Remove(Word word) {
            if (word == null) return;

            string originalWord = word.Text;
            _stringData = RemoveFirst(_stringData, originalWord, word.Begin);
            int newCursorIndex = word.Begin;
            CursorIndex = newCursorIndex;
        }

        /// <summary>
        /// Removes the first occurrence of the given <paramref name="search"/> string from the given <paramref name="text"/>.
        /// </summary>
        /// <param name="text">Text to process</param>
        /// <param name="search">Search string to remove</param>
        /// <param name="begin">Index to begin the removement</param>
        /// <returns>Text with stripped value</returns>
        private string RemoveFirst(string text, string search, int begin) {
            int pos = text.IndexOf(search, begin);
            if (pos == -1) return text;

            // Don't cut off sentence control characters (.,)
            int afterWordIndex = pos + search.Length;
            char succeedingChar = text[afterWordIndex];
            if (succeedingChar == '.' || succeedingChar == ',')
                pos--; // -1 grab preceeding whitespace
            else
                afterWordIndex++; // +1 grab succeeding whitespace

            return text.Substring(0, pos) + text.Substring(afterWordIndex);
        }

        /// <summary>
        /// Returns the current string data. This contains any replace operations that may have been done.
        /// </summary>
        /// <returns>Underlying string data</returns>
        public string GetStringData() {
            return _stringData;
        }

        /// <summary>
        /// Describes a word within a string.
        /// </summary>
        public class Word {
            private readonly WordIterator _wordIterator;

            /// <summary> Creates a new instance. </summary>
            /// <param name="wordIterator">Associated word iterator</param>
            /// <param name="text">Word text</param>
            /// <param name="begin">Begin index of the word</param>
            /// <param name="end">End index of the word</param>
            public Word(WordIterator wordIterator, string text, int begin, int end) {
                _wordIterator = wordIterator;
                Text = text;
                Begin = begin;
                End = end;
            }

            /// <summary>
            /// Returns the text of the word.
            /// </summary>
            public string Text { get; }

            /// <summary>
            /// Returns the begin index of the word.
            /// </summary>
            public int Begin { get; }

            /// <summary>
            /// Returns the end index of the word.
            /// </summary>
            public int End { get; }

            /// <summary>
            /// Replaces the word within the underlying string with the given <paramref name="value"/>.
            /// </summary>
            /// <param name="value">Value to set instead</param>
            public void Replace(string value) {
                _wordIterator.Replace(this, value);
            }

            /// <summary>
            /// Removes the word from the underlying string.
            /// </summary>
            public void Remove() {
                _wordIterator.Remove(this);
            }
        }   
    }
}