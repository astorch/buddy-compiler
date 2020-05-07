using System;
using System.Diagnostics;
using Bumblebee.buddy.compiler.collectiontools;
using Bumblebee.buddy.compiler.exceptions;
using Bumblebee.buddy.compiler.model.functions;
using Bumblebee.buddy.compiler.packaging;
using Bumblebee.buddy.compiler.simplex;
using Bumblebee.buddy.compiler.writers;
using xcite.csharp.diagnostics;

namespace Bumblebee.buddy.compiler {
    /// <summary>
    /// Implements a compiler for the buddy language.
    /// </summary>
    public class BuddyCompiler {
        private readonly BuddyTextProcessor _buddyTextProcessor;
        private readonly InstructionTranslator _instructionTranslator;
        private IImportPathProvider _importPathProvider;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <exception cref="BuddyCompilerException"/>
        public BuddyCompiler() {
            try {
                _buddyTextProcessor = new BuddyTextProcessor();
                _instructionTranslator = new InstructionTranslator();
                _importPathProvider = NullImportPathProvider.Instance;
            } catch (Exception ex) {
                throw new BuddyCompilerException("Error on initializing Buddy Compiler", ex);
            }
        }

        /// <summary>
        /// Returns the currently used import path provider or does set.
        /// </summary>
        public IImportPathProvider ImportPathProvider {
            get { return _importPathProvider; }
            set { _importPathProvider = value ?? NullImportPathProvider.Instance; }
        }

        /// <summary>
        /// Returns the currently used TDIL extension function registry.
        /// </summary>
        public TdilExtensionFunctionRegistry TdilExtensionFunctionRegistry {
            get { return TdilExtensionFunctionRegistry.Default; }
        }

        /// <summary>
        /// Compiles the given <paramref name="buddyText"/> into a TDIL file as string.
        /// </summary>
        /// <param name="buddyText">Buddy language text to compile</param>
        /// <returns>Compiled buddy text</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="buddyText"/> is NULL or empty</exception>
        /// <exception cref="BuddyCompilerException">If something went wrong during the compilation process</exception>
        public virtual string Compile(string buddyText) {
            if (string.IsNullOrEmpty(buddyText)) throw new ArgumentNullException("buddyText");

            StopwatchLog compilerStopwatch = StopwatchLog.StartNew(LogCompilerPerformance);
            TdilFileWriter tdilFileWriter = new TdilFileWriter("0.1.1");

            try {
                // TODO Remove workaround add 'Vorbedingung' 
                if (!buddyText.Contains("Vorbedingung: -") && buddyText.Contains("Anwendung: ")) {
                    int insertPoint = buddyText.IndexOf("Schritte:");
                    buddyText = buddyText.Insert(insertPoint, "Vorbedingung: -\r\n\r\n");
                }

                // TODO Remove workaround add final linefeeds
                if (!buddyText.EndsWith("\r\n"))
                    buddyText += "\r\n";

                // Process text
                StopwatchLog textProcessorStopwatch = StopwatchLog.StartNew(LogBuddyTextProcessorPerformance);
                BuddyTextInfo buddyTextInfo = _buddyTextProcessor.ProcessText(buddyText);
                textProcessorStopwatch.Dispose(buddyTextInfo);

                // In the case of an short form, create standard names
                if (buddyTextInfo.IsShortForm) {
                    buddyTextInfo.ApplicationText = "untitled";
                    buddyTextInfo.UseCaseText = "untitled";
                    buddyTextInfo.ScenarioText = "untitled";
                }

                // TODO Check if this is a workaround and remove it
                if (string.IsNullOrEmpty(buddyTextInfo.VersionText))
                    buddyTextInfo.VersionText = "*";

                // Little verification
                ushort expectedNoOfSteps = CalculateNoOfSteps(buddyText);
                if (expectedNoOfSteps != buddyTextInfo.Steps.Length)
                    ThrowUncompilableDirectiveException(buddyText, expectedNoOfSteps, buddyTextInfo.Steps.Length);

                string[] buddyTextSteps = buddyTextInfo.Steps;

                // Normalize directives
                string[] normalizedBuddyTextSteps;
                using (StopwatchLog.StartNew(LogNormalizingPerformance)) {
                    normalizedBuddyTextSteps = NormalizeSteps(buddyTextSteps, buddyTextInfo.Parameters);
                }

                // Create unit name
                UnitName unitName = new UnitName(buddyTextInfo.ApplicationText, buddyTextInfo.VersionText, buddyTextInfo.UseCaseText, buddyTextInfo.ScenarioText);
                string qualifiedUnitName = unitName.ToQualifiedString();
                        
                using (CompilingContext compilingContext = new CompilingContext()) {
                    // Compile directives
                    string[] directiveSet = new string[1000];
                    int pr = 0;
                    using (StopwatchLog.StartNew(LogInstructionTranslationPerformance)) {
                        for (int i = -1; ++i != normalizedBuddyTextSteps.Length;) {
                            string actionLine = normalizedBuddyTextSteps[i];
                            string directive = _instructionTranslator.ToDirective(actionLine);
                            directiveSet[pr++] = directive;
                        }
                        Array.Resize(ref directiveSet, pr);
                    }

                    // Add alias references
                    string[] aliasReferenceSet = compilingContext.AliasReferenceSet;
                    tdilFileWriter.AddAlias(aliasReferenceSet);
                    
                    // Add unit references
                    string[] unitReferenceSet = compilingContext.UnitReferenceSet;
                    string[] qualifiedUnitNames = ToQualifiedUnitNames(unitReferenceSet);
                    tdilFileWriter.AddImport(qualifiedUnitNames);

                    // Write unit
                    using (StopwatchLog.StartNew(LogTdilUnitWritingPerformance)) {
                        using (TdilUnitWriter tdilUnitWriter = tdilFileWriter.CreateUnit(qualifiedUnitName)) {
                            string executeSectionName = string.Format("{0}", unitName.GetEncodedScenarioName());
                            BuddyTextParameter[] unitParameters = buddyTextInfo.Parameters;
                            string[] parameterNames = new string[unitParameters.Length];
                            for (int l = -1; ++l != parameterNames.Length;)
                                parameterNames[l] = unitParameters[l].Name;

                            // Write main section
                            using (TdilSectionWriter tdilSectionWriter = tdilUnitWriter.CreateSection("Main")) {
                                // Declare parameters
                                for (int i = -1; ++i != unitParameters.Length;) {
                                    BuddyTextParameter unitParameter = unitParameters[i];
                                    tdilSectionWriter.AppendLine("{0} = {1}", unitParameter.Name, unitParameter.DefaultValue);
                                }

                                // Add start statement
                                tdilSectionWriter.AppendLine("start(,, \"{{{0}}}\")", buddyTextInfo.ApplicationText);

                                // Add argument invocation line if there are some
                                string argInvLine = null;
                                if (parameterNames.Length != 0) {
                                    string argSetLine = string.Join(", ", parameterNames);
                                    argInvLine = string.Format("({0})", argSetLine);
                                }

                                tdilSectionWriter.AppendLine("gosub {0}:{1}", executeSectionName, argInvLine);

                                // Add close statement
                                tdilSectionWriter.AppendLine("close(_Application,, Default)");
                                tdilSectionWriter.AppendLine("kill(_Application,, 3000)");

                                tdilSectionWriter.AppendLine("close(\"AcroRd32\",, Default)");
                                tdilSectionWriter.AppendLine("kill(\"AcroRd32\",, 3000)");
                            }

                            // Write execute section
                            using (TdilSectionWriter tdilSectionWriter = tdilUnitWriter.CreateSection(executeSectionName, parameterNames)) {
                                for (int l = -1; ++l != directiveSet.Length;)
                                    tdilSectionWriter.AppendLine(directiveSet[l]);
                            }
                        }
                    }
                }

            } catch (BuddyCompilerException) {
                throw;
            } catch (Exception ex) {
                throw new BuddyCompilerException("Error on compiling buddy language unit.", ex);
            }

            string tdilFileContent = tdilFileWriter.WriteToString();
            compilerStopwatch.Dispose();

            return tdilFileContent;
        }

        [Obsolete]
        private ushort CalculateNoOfSteps(string buddyText) {
            const string stepsToken = "Schritte:";
            int startingPoint = buddyText.IndexOf(stepsToken, StringComparison.Ordinal);
            if (startingPoint == -1) return 0;
            int stepTextBegin = startingPoint + stepsToken.Length;

            char[] instructionText = buddyText.Substring(stepTextBegin).ToCharArray();
            ushort instructionCount = 0;

            bool escapeMode = false;
            for (int i = -1; ++i != instructionText.Length;) {
                char c = instructionText[i];
                
                // Toggle escape mode
                if (c == '"')
                    escapeMode = !escapeMode;

                // We don't count anything in escape mode
                if (escapeMode) continue;

                // Check significant char
                if (c == '.')
                    instructionCount++;
            }

            return instructionCount;
        }

        /// <summary>
        /// Normalizes the given collection of <paramref name="buddyTextSteps"/>. This means the following steps are done:
        /// <list type="number">
        ///     <item>Articles are removed</item>
        ///     <item>ambigiuous words are unified</item>
        ///     <item>Parameters (e.g., words starting with '$') are replaced with actual values</item>
        /// </list>
        /// </summary>
        /// <param name="buddyTextSteps">Collection of buddy text steps to process</param>
        /// <param name="buddyTextParameters">Collection of available/declared buddy text parameters</param>
        /// <returns>Normalized collection of <paramref name="buddyTextSteps"/></returns>
        /// <exception cref="BuddyCompilerException">If there is a parameter reference in the text without with declaration</exception>
        public virtual string[] NormalizeSteps(string[] buddyTextSteps, BuddyTextParameter[] buddyTextParameters) {
            string[] normalizedSteps = new string[buddyTextSteps.Length];
            for (int i = -1; ++i != normalizedSteps.Length;) {
                string step = buddyTextSteps[i];
                step = StripPunctuationMarks(step);
                step = StripArticles(step);
                step = StripPrepositions(step);
                step = StripAuxiliaryVerbs(step);
                step = ResolveAmbiguity(step);
                step = StripSubstantives(step);
                normalizedSteps[i] = step;
            }
            return normalizedSteps;
        }

        /// <summary>
        /// Normalizes the given <paramref name="buddyText"/>. This means the following steps are done: 
        /// <list type="number">
        ///     <item>Line feeds are removed</item>
        ///     <item>Articles are removed</item>
        ///     <item>ambigiuous words are unified</item>
        /// </list>
        /// </summary>
        /// <param name="buddyText">Buddy text to process</param>
        /// <returns>Normalized <paramref name="buddyText"/></returns>
        [Obsolete]
        public virtual string Normalize(string buddyText) {
            buddyText = buddyText.Replace("\r\n", "\n");
            buddyText = StripPunctuationMarks(buddyText);
            buddyText = StripArticles(buddyText);
            buddyText = StripPrepositions(buddyText);
            buddyText = StripAuxiliaryVerbs(buddyText);
            buddyText = StripSubstantives(buddyText);
            buddyText = ResolveAmbiguity(buddyText);
            return buddyText;
        }

        /// <summary>
        /// Strips standard articles from the given <paramref name="buddyText"/>.
        /// </summary>
        /// <param name="buddyText">Buddy text to process</param>
        /// <returns><paramref name="buddyText"/> without articles</returns>
        public virtual string StripArticles(string buddyText) {
            // Articles to remove
            string[] articles = {"der", "die", "das", "den", "dem"};

            string strippedBuddyText = buddyText;
            for (WordIterator itr = buddyText.GetWordIterator(); itr.MoveNext();) {
                WordIterator.Word word = itr.Current;
                if (word == null) break;

                // Check if the word is an article
                string wordText = word.Text;
                bool isArticle = Array.FindIndex(articles,
                                     item => string.Equals(item, wordText, StringComparison.InvariantCultureIgnoreCase)) != -1;
                if (!isArticle) continue;
                word.Remove();

                // Update return value
                strippedBuddyText = itr.GetStringData();
            }

            return strippedBuddyText;
        }

        /// <summary>
        /// Strips standard prepositions from the given <paramref name="buddyText"/>.
        /// </summary>
        /// <param name="buddyText">Buddy text to process</param>
        /// <returns><paramref name="buddyText"/> without articles</returns>
        public virtual string StripPrepositions(string buddyText) {
            // Prepositions to remove
            string[] prepositions = {"in", "im", "aus", "ein", "ob", "bis", "auf", "zu"};

            string strippedBuddyText = buddyText;
            for (WordIterator itr = buddyText.GetWordIterator(); itr.MoveNext();) {
                WordIterator.Word word = itr.Current;
                if (word == null) break;

                // Check if the word is an preposition
                string wordText = word.Text;
                bool isPreposition = Array.FindIndex(prepositions,
                                         item => string.Equals(item, wordText, StringComparison.InvariantCultureIgnoreCase)) != -1; 
                if (!isPreposition) continue;
                word.Remove();

                // Update return value
                strippedBuddyText = itr.GetStringData();
            }

            return strippedBuddyText;
        }

        /// <summary>
        /// Strips standard auxiliary verbs from the given <paramref name="buddyText"/>.
        /// </summary>
        /// <param name="buddyText">Buddy text to process</param>
        /// <returns><paramref name="buddyText"/> without auxiliary verbs</returns>
        public virtual string StripAuxiliaryVerbs(string buddyText) {
            // Auxiliary verbs to remove
            string[] auxiliaryVerbs = {"ist"};

            string strippedBuddyText = buddyText;
            for (WordIterator itr = buddyText.GetWordIterator(); itr.MoveNext();) {
                WordIterator.Word word = itr.Current;
                if (word == null) break;

                // Check if the word is an preposition
                string wordText = word.Text;
                bool isAuxiliaryVerb = Array.FindIndex(auxiliaryVerbs,
                                           item => string.Equals(item, wordText, StringComparison.InvariantCultureIgnoreCase)) != -1; 
                if (!isAuxiliaryVerb) continue;
                word.Remove();

                // Update return value
                strippedBuddyText = itr.GetStringData();
            }

            return strippedBuddyText;
        }

        /// <summary>
        /// Strips typically unneeded substantives from the given <paramref name="buddyText"/>.
        /// </summary>
        /// <param name="buddyText">Buddy text to process</param>
        /// <returns><paramref name="buddyText"/> without substantives</returns>
        public virtual string StripSubstantives(string buddyText) { // TODO Add unit test
            // Substantives to remove
            string[] substantives = {"Wert", "Button", "Schaltfläche", "Navigation"};

            string strippedBuddyText = buddyText;
            for (WordIterator itr = buddyText.GetWordIterator(); itr.MoveNext(); ) {
                WordIterator.Word word = itr.Current;
                if (word == null) break;

                // Check if the word is an preposition
                string wordText = word.Text;
                bool isAuxiliaryVerb = Array.FindIndex(substantives,
                                           item => string.Equals(item, wordText, StringComparison.InvariantCultureIgnoreCase)) != -1;
                if (!isAuxiliaryVerb) continue;
                word.Remove();

                // Update return value
                strippedBuddyText = itr.GetStringData();
            }

            return strippedBuddyText;
        }

        /// <summary>
        /// Strips punctuation marks from the given <paramref name="buddyText"/>.
        /// </summary>
        /// <param name="buddyText">Buddy text to process</param>
        /// <returns><paramref name="buddyText"/> without punctuation marks</returns>
        public virtual string StripPunctuationMarks(string buddyText) {
            char[] punctuationMarks = {','};

            string strippedBuddyText = buddyText.RemoveCharactersBuddyStyle(punctuationMarks);
            return strippedBuddyText;
        }

        /// <summary>
        /// Replaces all known synonyms of action pattern key words with their base form.
        /// </summary>
        /// <param name="buddyText">Buddy text to process</param>
        /// <returns>Unambigiuous <paramref name="buddyText"/></returns>
        public virtual string ResolveAmbiguity(string buddyText) {
            SynMapRegistry synMapRegistry = new SynMapRegistry();

            // TODO Hack entfernen
            buddyText = buddyText.Replace("nicht sichtbar", "verschwunden");

            string unambigiuousBuddyText = buddyText;
            for (WordIterator itr = buddyText.GetWordIterator(); itr.MoveNext();) {
                WordIterator.Word word = itr.Current;
                if (word == null) break;

                string wordText = word.Text;

                // Check for synonyms
                string rootWord;
                if (!synMapRegistry.TryGetRootWord(wordText, out rootWord)) continue;
                word.Replace(rootWord);

                // Update return value
                unambigiuousBuddyText = itr.GetStringData();
            }

            return unambigiuousBuddyText;
        }

        /// <summary>
        /// Logs the measured duration of the buddy text processor using the given stopwatch.
        /// </summary>
        /// <param name="textProcessorStopwatch">Stopwatch that measured the processor</param>
        /// <param name="data">Additional data object</param>
        private void LogBuddyTextProcessorPerformance(Stopwatch textProcessorStopwatch, object data) {
            BuddyTextInfo buddyTextInfo = (BuddyTextInfo)data;

            Console.WriteLine("[BuddyCompiler] Text Processor duration: {0}ms ({1}s) - {2} LOS",
                    textProcessorStopwatch.ElapsedMilliseconds, (textProcessorStopwatch.ElapsedMilliseconds / 1000), buddyTextInfo.Steps.Length);
        }

        /// <summary>
        /// Logs the measured duration of the buddy text directive normalizing using the given stopwatch.
        /// </summary>
        /// <param name="normalizingStopwatch">Stopwatch that measured the normalizing</param>
        /// <param name="data">Additional data object</param>
        private void LogNormalizingPerformance(Stopwatch normalizingStopwatch, object data) {
            Console.WriteLine("[BuddyCompiler] Normalizing duration: {0}ms ({1}s)",
                normalizingStopwatch.ElapsedMilliseconds, (normalizingStopwatch.ElapsedMilliseconds / 1000));
        }

        /// <summary>
        /// Logs the measured duration of the instruction formatter using the given stopwatch.
        /// </summary>
        /// <param name="formatterStopwatch">Stopwatch that measured the instruction formatter</param>
        /// <param name="data">Additional data object</param>
        private void LogInstructionTranslationPerformance(Stopwatch formatterStopwatch, object data) {
            Console.WriteLine("[BuddyCompiler] Instruction translation duration: {0}ms ({1}s)",
                formatterStopwatch.ElapsedMilliseconds, (formatterStopwatch.ElapsedMilliseconds/1000));
        }

        /// <summary>
        /// Logs the measured duration of the TDIL unit writing using the given stopwatch.
        /// </summary>
        /// <param name="writerStopwatch">Stopwatch that measured the unit writer</param>
        /// <param name="data">Additional data object</param>
        private void LogTdilUnitWritingPerformance(Stopwatch writerStopwatch, object data) {
            Console.WriteLine("[BuddyCompiler] Unit writing duration: {0}ms ({1}s)",
                writerStopwatch.ElapsedMilliseconds, (writerStopwatch.ElapsedMilliseconds / 1000));
        }

        /// <summary>
        /// Logs the measured duration of the buddy compiler using the given stopwatch.
        /// </summary>
        /// <param name="compilerStopwatch">Stopwatch that measured the compiler</param>
        /// <param name="data">Additional data object</param>
        private void LogCompilerPerformance(Stopwatch compilerStopwatch, object data) {
            Console.WriteLine("[BuddyCompiler] Total duration: {0}ms ({1}s)",
                    compilerStopwatch.ElapsedMilliseconds, (compilerStopwatch.ElapsedMilliseconds / 1000));
        }

        /// <summary>
        /// Throws an <see cref="UncompilableDirectiveException"/> that specifies the directive that could not have been 
        /// compiled.
        /// </summary>
        /// <param name="buddyText">Buddy text where the error occurred</param>
        /// <param name="expectedCount">Expected count of directives</param>
        /// <param name="actualCount">Actual count of directives</param>
        private void ThrowUncompilableDirectiveException(string buddyText, int expectedCount, int actualCount) {
            if (buddyText == null)
                buddyText = string.Empty;

            string[] lines = buddyText.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            int indexOfFirstDirective = Array.IndexOf(lines, "Schritte:");
            int errorLineIndex = indexOfFirstDirective + actualCount + 4; // TODO Handle sentences without linefeed!

            throw new UncompilableDirectiveException("Could not compile directive") {
                ColumnIndex = 1,
                LineIndex = (uint)errorLineIndex
            };
        }

        /// <summary>
        /// Transforms each unit reference of the given <paramref name="unitReferenceSet"/> into a full qualified 
        /// unit name.
        /// </summary>
        /// <param name="unitReferenceSet">Collection of unit references</param>
        /// <returns>Collection of full qualified unit names</returns>
        /// <exception cref="BuddyCompilerException">If a unit name could not be found</exception>
        private string[] ToQualifiedUnitNames(string[] unitReferenceSet) {
            if (unitReferenceSet == null) return new string[0];
            if (unitReferenceSet.Length == 0) return new string[0];

            IImportPathProvider importPathProvider = ImportPathProvider;
            string[] qualifiedUnitNames = new string[unitReferenceSet.Length];
            for (int i = -1; ++i != unitReferenceSet.Length;) {
                string unitReference = unitReferenceSet[i].Replace('_', ' ');
                UnitName unitName = importPathProvider.GetUnitName(unitReference);
                if (unitName == null) throw new BuddyCompilerException(string.Format("There is no unit registered for the given reference '{0}'", unitReference));
                string qualifiedUnitName = unitName.ToQualifiedString();
                qualifiedUnitNames[i] = qualifiedUnitName;
            }

            return qualifiedUnitNames;
        }

        /// <summary>
        /// Provides an anonymous implementation of <see cref="IImportPathProvider"/> that always 
        /// throws an <see cref="BuddyCompilerException"/>.
        /// </summary>
        class NullImportPathProvider : IImportPathProvider {
            /// <summary>
            /// Returns the default instance of this class.
            /// </summary>
            public static readonly IImportPathProvider Instance = new NullImportPathProvider();

            /// <inheritdoc />
            public string GetPath(UnitName unitName) {
                throw new BuddyCompilerException("Import path provider hasn't been set");
            }

            /// <inheritdoc />
            public UnitName GetUnitName(string scenarioName) {
                throw new BuddyCompilerException("Import path provider hasn't been set");
            }
        }
    }
}