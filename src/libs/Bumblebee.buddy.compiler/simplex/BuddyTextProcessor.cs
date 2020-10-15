using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Bumblebee.buddy.compiler.exceptions;
using Simplex.grammar;
using Simplex.lexer;

namespace Bumblebee.buddy.compiler.simplex {
    /// <summary>
    /// Provides a buddy text processor. This class extracts the essential information from a buddy text and provides 
    /// an object with the resolved data. <br/> <br/>
    /// Note:<br/>
    /// The first initialization of a class instance may take more time because the buddy grammar is loaded.<br/>
    /// </summary>
    public class BuddyTextProcessor {
        private static IGrammar _buddyGrammar;

        private ILexer _buddyTextLexer;
        
        private BuddyTextInfo _currentBuddyTextInfo;
        private List<string> _currentActionLines;
        private List<BuddyTextParameter> _currentParameters; 

        /// <summary>
        /// Creates a new instance. Parses the buddy grammar the first time this class is instantiated.
        /// </summary>
        /// <exception cref="BuddyTextProcessorException"/>
        public BuddyTextProcessor() {
            Initialize();
        }

        /// <summary>
        /// Processes the given text and returns an object with the resolved data. If the given <paramref name="buddyText"/> is NULL, 
        /// NULL is returned. 
        /// </summary>
        /// <param name="buddyText">Buddy text to process</param>
        /// <returns>Resolved data from the given <paramref name="buddyText"/></returns>
        /// <exception cref="BuddyTextProcessorException">If an error occurred during the processing</exception>
        public BuddyTextInfo ProcessText(string buddyText) {
            if (buddyText == null) return null;
            
            // Ensure the instance has been set up correctly
            SetUp();
            if (_buddyTextLexer == null) throw new BuddyTextProcessorException("SetUp() hasn't been called before!");

            // Initialize
            _currentBuddyTextInfo = new BuddyTextInfo();
            _currentActionLines = new List<string>(20);
            _currentParameters = new List<BuddyTextParameter>(10);

            // Process
            try {
                _buddyTextLexer.Process(buddyText);
            } catch (Exception ex) {
                throw new BuddyTextProcessorException("Error on processing the given buddy text", ex);
            }

            // Set up results
            _currentBuddyTextInfo.Steps = _currentActionLines.ToArray();
            _currentBuddyTextInfo.Parameters = _currentParameters.ToArray();

            return _currentBuddyTextInfo;
        }

        /// <summary>
        /// Configures the instance to process a buddy text correctly. Though this method may be called more than once, 
        /// the configuration is done just the first time. Note that this method has to be called once per instance 
        /// before <see cref="ProcessText"/> is invoked.
        /// </summary>
        private void SetUp() {
            if (_buddyTextLexer != null) return;
            _buddyTextLexer = new Lexer(_buddyGrammar);

            _buddyTextLexer.AddSymbolMatchHandler("app_title", OnApplicationTitleResolved);
            _buddyTextLexer.AddSymbolMatchHandler("app_version", OnApplicationVersionResolved);

            _buddyTextLexer.AddSymbolMatchHandler("usecase_descr", OnUseCaseDeclarationResolved);
            _buddyTextLexer.AddSymbolMatchHandler("scene_descr", OnScenarioDeclarationResolved);

            _buddyTextLexer.AddSymbolMatchHandler("import_statement", OnImportStatementResolved);
            
            _buddyTextLexer.AddSymbolMatchHandler("precond_descr", OnPreconditionDeclarationResolved);

            _buddyTextLexer.AddSymbolMatchHandler("step_text", OnStepTextResolved);
            _buddyTextLexer.AddSymbolMatchHandler("assignment", OnAssignmentResolved);
        }

        /// <summary>
        /// Is invoked when an assignment has been resolved.
        /// </summary>
        /// <param name="match">Text match</param>
        private void OnAssignmentResolved(string match) {
            string trimmedAssign = match.Trim();
            string[] parts = trimmedAssign.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
            string paramName = parts[0].Trim();
            string paramDfltValue = parts[1].Trim();

            _currentParameters.Add(new BuddyTextParameter(paramName, paramDfltValue));
        }

        /// <summary>
        /// Is invoked when an action line (called step) has been resolved.
        /// </summary>
        /// <param name="match">Text match</param>
        private void OnStepTextResolved(string match) {
            int dotIndex = match.LastIndexOf('.');
            string step = match.Substring(0, dotIndex + 1);
            _currentActionLines.Add(step);
        }

        /// <summary>
        /// Is invoked when a precondition has been resolved.
        /// </summary>
        /// <param name="match">Text match</param>
        private void OnPreconditionDeclarationResolved(string match) {
            _currentBuddyTextInfo.Precondition = "-";
        }

        /// <summary>
        /// Is invoked when a scenario declaration has been resolved.
        /// </summary>
        /// <param name="match">Text match</param>
        private void OnScenarioDeclarationResolved(string match) {
            string scenarioText = match.Substring("Szenario:".Length).Trim();

            // Cut off parameters
            int lbraceIndex = scenarioText.IndexOf('(');
            if (lbraceIndex != -1) {
                short shift = 1;

                if (scenarioText[lbraceIndex - 1] == ' ')
                    shift = -1;
                    
                scenarioText = scenarioText.Substring(0, lbraceIndex + shift);
            }

            _currentBuddyTextInfo.ScenarioText = scenarioText;
        }

        /// <summary>
        /// Is invoked when an import statement has been resolved.
        /// </summary>
        /// <param name="match">Text match</param>
        private void OnImportStatementResolved(string match) {
            // Currently not used
        }

        /// <summary>
        /// Is invoked when an use case declaration has been resolved.
        /// </summary>
        /// <param name="match">Text match</param>
        private void OnUseCaseDeclarationResolved(string match) {
            string useCaseText = match.Substring("Anwendungsfall:".Length).Trim();
            _currentBuddyTextInfo.UseCaseText = useCaseText;
        }

        /// <summary>
        /// Is invoked when an application declaration has been resolved.
        /// </summary>
        /// <param name="match">Text match</param>
        private void OnApplicationTitleResolved(string match) {
            string applicationText = match.Trim();
            _currentBuddyTextInfo.ApplicationText = applicationText;
        }

        /// <summary>
        /// Is invoked when a version declaration has been resolved.
        /// </summary>
        /// <param name="match">Text match</param>
        private void OnApplicationVersionResolved(string match) {
            string versionText = match.Substring(1, match.Length - 2).Trim();
            _currentBuddyTextInfo.VersionText = versionText;
        }

        /// <summary>
        /// Initializes the buddy grammar. Though this method can be called more than once, the grammar is just parsed one time.
        /// </summary>
        /// <exception cref="BuddyTextProcessorException">If the during the parsing process occurs an error</exception>
        private static void Initialize() {
            if (_buddyGrammar != null) return;

            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Bumblebee.buddy.compiler.simplex.Buddy.gr");
            if (stream == null) throw new BuddyTextProcessorException("Could not open a stream to the buddy grammar file");

            string grammar;
            using (StreamReader sr = new StreamReader(stream)) {
                grammar = sr.ReadToEnd();
            }

            if (string.IsNullOrEmpty(grammar)) throw new BuddyTextProcessorException("Grammar from stream is NULL or empty!");

            try {
                GrammarParser parser = new GrammarParser(grammar);
                _buddyGrammar = parser.Parse();
            } catch (Exception ex) {
                throw new BuddyTextProcessorException("Error on parsing buddy grammar", ex);
            }

            if (!_buddyGrammar.IsValid) throw new BuddyTextProcessorException("Constructed grammar is not valid!");
            if (_buddyGrammar.Root.Id != "file") throw new BuddyTextProcessorException("Invalid root rule. Constructed grammar is not valid!");
        }
    }
}