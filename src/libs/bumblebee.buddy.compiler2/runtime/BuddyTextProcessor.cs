using System;
using System.IO;
using System.Reflection;
using bumblebee.buddy.compiler2.exceptions;
using bumblebee.buddy.compiler2.model;
using simplex;
using simplex.grammar;

namespace bumblebee.buddy.compiler2.runtime {
    /// <summary>
    /// Provides methods to process or parse a given buddy text and create an object model.
    /// </summary>
    public class BuddyTextProcessor {
        private readonly ReflexiveLexer iReflexiveLexer;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <exception cref="BuddyCompilerException">If an error occurs</exception>
        public BuddyTextProcessor() {
            iReflexiveLexer = CreateReflexiveLexer();
        }

        /// <summary>
        /// Parses the given <paramref name="buddyText"/> and creates an instance of <see cref="TestcaseContext"/>.
        /// </summary>
        /// <param name="buddyText">Buddy text to parse. Must not be NULL or empty!</param>
        /// <returns>New instance of <see cref="TestcaseContext"/></returns>
        /// <exception cref="BuddyCompilerException">If an error occurs</exception>
        public TestcaseContext ProcessText(string buddyText) {
            if (string.IsNullOrEmpty(buddyText)) throw new BuddyCompilerException(EBuddyCompileError.EmptyBuddyText);

            try {
                return (TestcaseContext) iReflexiveLexer.Process(buddyText);
            } catch (Exception ex) {
                throw new BuddyCompilerException(EBuddyCompileError.BuddyTextParsingFailed, null, ex);
            }
        }

        /// <summary>
        /// Reads the buddy grammar file and creates an instance of <see cref="ReflexiveLexer"/>.
        /// </summary>
        /// <returns>Newly created instance of <see cref="ReflexiveLexer"/></returns>
        /// <exception cref="BuddyCompilerException">If an error occurs</exception>
        private ReflexiveLexer CreateReflexiveLexer() {
            string grammar;
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("bumblebee.buddy.compiler2.model.Buddy.gr")) {
                if (stream == null) throw new BuddyCompilerException(EBuddyCompileError.MissingGrammar);
                using (StreamReader streamReader = new StreamReader(stream)) {
                    grammar = streamReader.ReadToEnd();
                }
            }

            if (string.IsNullOrEmpty(grammar)) throw new BuddyCompilerException(EBuddyCompileError.EmptyGrammar);

            IGrammar buddyGrammar;
            try {
                GrammarParser parser = new GrammarParser(grammar);
                buddyGrammar = parser.Parse();
            } catch (Exception ex) {
                throw new BuddyCompilerException(EBuddyCompileError.GrammarParsingFailed, null, ex);
            }
            
            if (!buddyGrammar.IsValid) throw new BuddyCompilerException(EBuddyCompileError.InvalidGrammar);
            if (buddyGrammar.Root.Id != "testcase") throw new BuddyCompilerException(EBuddyCompileError.InvalidRootRule);

            return new ReflexiveLexer(new BuddyModelFactory(), buddyGrammar);
        }
    }
}