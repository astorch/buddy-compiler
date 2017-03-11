using System;
using System.Collections.Generic;
using System.Linq;

namespace Bumblebee.buddy.compiler.datastructures {
    /// <summary>
    /// Defines a set of synonyms that are mapped to a root word.
    /// </summary>
    public class SynMap {
        private readonly List<string> iSynonyms = new List<string>(); 

        /// <summary>
        /// Creates a new instance for the given <paramref name="rootWord"/>.
        /// </summary>
        /// <param name="rootWord">Synonym set root word</param>
        public SynMap(string rootWord) {
            if (string.IsNullOrEmpty(rootWord)) throw new ArgumentNullException("rootWord");
            RootWord = rootWord;
        }

        /// <summary>
        /// Returns the root word of this set.
        /// </summary>
        public string RootWord { get; private set; }

        /// <summary>
        /// Adds the given <paramref name="word"/> as synonym of the root word of this set.
        /// </summary>
        /// <param name="word">Word to add as synonym</param>
        public void AddSynonym(string word) {
            iSynonyms.Add(word);
        }

        /// <summary>
        /// Returns TRUE if the given <paramref name="word"/> is a synonym of the root word and known by this set. 
        /// If the word is NULL or empty, FALSE ist returned.
        /// </summary>
        /// <param name="word">Word to check</param>
        /// <returns>TRUE or FALSE</returns>
        public bool IsSynonym(string word) {
            if (string.IsNullOrEmpty(word)) return false;
            bool any = iSynonyms.Any(syn => string.Equals(syn, word, StringComparison.InvariantCultureIgnoreCase));
            return any;
        }
    }
}