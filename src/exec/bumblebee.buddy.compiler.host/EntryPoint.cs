using System;
using System.IO;
using System.Runtime.InteropServices;
using Bumblebee.buddy.compiler;
using log4net;

namespace bumblebee.buddy.compiler.host {
    /// <summary>
    /// Provides the entry point of the application.
    /// </summary>
    public static class EntryPoint {
        /// <summary>
        /// Implements the main method of the application.
        /// </summary>
        /// <param name="args">Execution arguments</param>
        /// <returns>Execution result code</returns>
        public static int Main(string[] args) {
            ExecutionArguments execArgs = new ExecutionArguments();
            if (!CommandLine.Parser.Default.ParseArguments(args, execArgs)) return -1;

            ILog logWriter = LogManager.GetLogger("BLC");
            logWriter.InfoFormat("LogWriter initialized");

            FileInfo sourceFile = execArgs.SourceFile;
            FileInfo targetFile = execArgs.TargetFile;

            logWriter.InfoFormat("Source file: {0}", sourceFile.FullName);
            logWriter.InfoFormat("Target file: {0}", targetFile.FullName);

            if (!sourceFile.Exists) {
                logWriter.ErrorFormat("Source file does not exist!");
                return -1;
            }

            try {
                logWriter.InfoFormat("Reading source file ...");
                string buddyText = File.ReadAllText(sourceFile.FullName);
                if (string.IsNullOrEmpty(buddyText)) throw new InvalidOperationException("Source file is empty!");

                logWriter.InfoFormat("Compiling source file ...");
                BuddyCompiler buddyCompiler = new BuddyCompiler();
                string ilText = buddyCompiler.Compile(buddyText);

                logWriter.InfoFormat("Writing compilation to target file ...");
                File.WriteAllText(targetFile.FullName, ilText);
            } catch (Exception ex) {
                logWriter.ErrorFormat("Error on compiling source file. Reason: {0}", ex);
                return Marshal.GetHRForException(ex);
            }

            logWriter.InfoFormat("Compilation successfully finished");
            return 0;
        }
    }
}
