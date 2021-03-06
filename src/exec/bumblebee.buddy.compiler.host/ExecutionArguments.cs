﻿using System.IO;
using CommandLine;
using CommandLine.Text;

[assembly: AssemblyUsage("Usage: blc.exe -s testcase.txt -t testcase.txt.tdil")]
namespace bumblebee.buddy.compiler.host {
    /// <summary> Describes the execution arguments of the buddy compiler host program. </summary>
    public class ExecutionArguments {
        
        /// <summary> Returns the specified path to the source file. </summary>
        [Option('s', "source", HelpText = "Path to the source file that will be compiled.", Required = true)]
        public string SourceFilePath {
            get => SourceFile?.ToString();
            set => SourceFile = new FileInfo(value);
        }

        /// <summary> Returns the specified path to the target file. </summary>
        [Option('t', "target", HelpText = "Path to the target file that will be created as compile result.",
             Required = true)]
        public string TargetFilePath {
            get => TargetFile?.ToString();
            set => TargetFile = new FileInfo(value);
        }

        /// <summary> Returns TRUE if the application shall run in debug mode. </summary>
        [Option('d', "debug")]
        public bool Debug { get; set; }

        /// <summary> Returns a pointer to the source file that shall be compiled. </summary>
        public FileInfo SourceFile { get; private set; }

        /// <summary> Returns a pointer to the target file that shall be compiled. </summary>
        public FileInfo TargetFile { get; private set; }
        
    }
}