using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Bumblebee.buddy.compiler.exceptions;

namespace Bumblebee.buddy.compiler.packaging {
    /// <summary>
    /// Provides methods to build an atomic unit bundle which contains the unit itself and all imports. An atomic 
    /// unit bundle can be execute by a test driver without further resources.
    /// </summary>
    public class UnitBundler {
        private readonly IImportPathProvider iImportPathProvider;

        /// <summary>
        /// Creates a new instance that operates with the given <paramref name="importPathProvider"/>.
        /// </summary>
        /// <param name="importPathProvider">Import path provider</param>
        /// <exception cref="ArgumentNullException">If <paramref name="importPathProvider"/> is NULL</exception>
        public UnitBundler(IImportPathProvider importPathProvider) {
            if (importPathProvider == null) throw new ArgumentNullException("importPathProvider");
            iImportPathProvider = importPathProvider;
        }

        /// <summary>
        /// Bundles the given <paramref name="tdilUnit"/> and all referenced units. A bundle table is returned 
        /// which contains all mandatory units (and additional resources) to execute the test case. The bundle 
        /// table contains the name of the referenced units as well as each unit content (as byte array). This 
        /// <paramref name="tdilUnit"/> is used and internally declared as main unit.
        /// </summary>
        /// <param name="tdilUnit">Reference to the main TDIL unit to pack</param>
        /// <returns>Created instance of <see cref="UnitBundleTable"/></returns>
        /// <exception cref="ArgumentNullException">If <paramref name="tdilUnit"/> is NULL</exception>
        /// <exception cref="ArgumentException">If <paramref name="tdilUnit"/> does not exist</exception>
        /// <exception cref="UnitBundlerException">If something went wrong during the package process</exception>
        public UnitBundleTable Bundle(FileInfo tdilUnit) {
            if (tdilUnit == null) throw new ArgumentNullException("tdilUnit");
            if (!tdilUnit.Exists) throw new ArgumentException(string.Format("Given '{0}' does not exist", tdilUnit.FullName));

            string fileContent;
            try {
                fileContent = File.ReadAllText(tdilUnit.FullName);
            } catch (Exception ex) {
                throw new UnitBundlerException(string.Format("Error on reading file '{0}'", tdilUnit.FullName), ex);
            }

            UnitBundleTable unitBundleTable = new UnitBundleTable();
            
            // Convert full qualified includes to file names and add them to the unit bundle table
            fileContent = ConvertQualifiedIncludesToFilenames(fileContent, unitBundleTable); // TODO Add support of n-th generation inheritance
            unitBundleTable.AddEntry("main.btc.tdil", fileContent);

            return unitBundleTable;
        }

        /// <summary>
        /// Parses all full qualified includes from the given <paramref name="fileContent"/>. Each include is added to the given 
        /// <paramref name="unitBundleTable"/>. The full qualified include is replaced by the name of the providing TDIL file, 
        /// so the TDIL Execution Engine can handle it. This method returns the replaced file content afterwards.
        /// </summary>
        /// <param name="fileContent">File content to process</param>
        /// <param name="unitBundleTable">Unit bundle table each include is added to</param>
        /// <returns>File content with converted file includes</returns>
        private string ConvertQualifiedIncludesToFilenames(string fileContent, UnitBundleTable unitBundleTable) {
            string processedFileContent = Regex.Replace(fileContent, "#include \"(?<unitName>[^\"]+)\"", match => {
                string unitName = match.Groups["unitName"].Value;
                string unitFileName = AddUnitReferenceToBundleTable(unitName, unitBundleTable);
                return string.Format("#include \"{0}\"", unitFileName);
            });

            return processedFileContent;
        }

        /// <summary>
        /// Adds the given <paramref name="unitReference"/>, the corresponding file name and its data to the given 
        /// <paramref name="unitBundleTable"/>.
        /// </summary>
        /// <param name="unitReference">Unit reference to process</param>
        /// <param name="unitBundleTable">Unit bundle table to update</param>
        /// <returns>The name of the TDIL file</returns>
        /// <exception cref="UnitBundlerException">If something went wrong</exception>
        private string AddUnitReferenceToBundleTable(string unitReference, UnitBundleTable unitBundleTable) {
            UnitName unitName = new UnitName(unitReference);
            string unitFilePath = iImportPathProvider.GetPath(unitName);
            
            try {
                string unitFileName = Path.GetFileName(unitFilePath);
                string fileContent = File.ReadAllText(unitFilePath); 
                byte[] unitFileContent = Encoding.UTF8.GetBytes(fileContent);
                unitBundleTable.AddEntry(unitFileName, unitFileContent);
                return unitFileName;
            } catch (Exception ex) {
                throw new UnitBundlerException(string.Format("Error on adding unit reference '{0}' to package table", unitReference), ex);
            }
        }
    }
}