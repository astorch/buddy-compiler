﻿using System.Collections.Generic;
using Bumblebee.buddy.compiler.datastructures;

namespace Bumblebee.buddy.compiler {
    /// <summary>
    /// Implements a registry of <see cref="SynMap"/>.
    /// </summary>
    public class SynMapRegistry {
        private static readonly List<SynMap> iRegisteredSynonymMaps = new List<SynMap>();

        /// <summary>
        /// Static initializer.
        /// </summary>
        static SynMapRegistry() {
            Initialize();
        }

        /// <summary>
        /// Returns an enumerable collection of all registered synonym maps.
        /// </summary>
        public IEnumerable<SynMap> RegisteredSynonymMaps {
            get {
                return iRegisteredSynonymMaps;
            }
        }

        /// <summary>
        /// Returns TRUE if the registry contains a synonym map that contains the given <paramref name="word"/> 
        /// as synonym. Additionally, the corresponding <paramref name="rootWord"/> is returned in this case. Otherwise, 
        /// the given <paramref name="word"/> is returned as <paramref name="rootWord"/>.
        /// </summary>
        /// <param name="word">Word to check</param>
        /// <param name="rootWord">Found root word or the given word</param>
        /// <returns>TRUE or FALSE</returns>
        public bool TryGetRootWord(string word, out string rootWord) {
            rootWord = word;
            for (int i = -1; ++i != iRegisteredSynonymMaps.Count;) {
                SynMap synMap = iRegisteredSynonymMaps[i];
                if (synMap.IsSynonym(word)) {
                    rootWord = synMap.RootWord;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Performs a static initialization of the internal collection.
        /// </summary>
        private static void Initialize() {
            SynMap clickMap = new SynMap("Klicke");
            iRegisteredSynonymMaps.Add(clickMap);

            SynMap buttonMap = new SynMap("Button");
            iRegisteredSynonymMaps.Add(buttonMap);

            SynMap assertMap = new SynMap("Prüfe");
            iRegisteredSynonymMaps.Add(assertMap);

            SynMap closeMap = new SynMap("Schließe");
            iRegisteredSynonymMaps.Add(closeMap);

            // Fill click map
            clickMap.AddSynonym("klick");
//            clickMap.AddSynonym("drücke");

            // Fill button map
            buttonMap.AddSynonym("Knopf");
            buttonMap.AddSynonym("Schaltfläche");
            buttonMap.AddSynonym("Schalter");

            // Fill assert map
            assertMap.AddSynonym("Prüfen");
            assertMap.AddSynonym("Überprüfe");
            assertMap.AddSynonym("Teste");
            assertMap.AddSynonym("Testen");

            // Fill close map
            closeMap.AddSynonym("Schliesse");
            closeMap.AddSynonym("Beende");
        }
    }
}