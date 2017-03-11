using System;
using Bumblebee.buddy.compiler.exceptions;

namespace Bumblebee.buddy.compiler.writers {
    /// <summary>
    /// Implements a writer for creating TDIL unit sections.
    /// </summary>
    public class TdilSectionWriter : AbstractTdilWriter<TdilSectionWriter> {
        private readonly string iSectionName;
        private readonly string[] iSectionParameterNames;

        /// <summary>Initializes a new instance.</summary>
        /// <param name="sectionName">Name of the unit section</param>
        public TdilSectionWriter(string sectionName) : this(sectionName, new string[0]) {
            // Currently nothing to do here
        }

        /// <summary>Initializes a new instance.</summary>
        /// <param name="sectionName">Name of the unit section</param>
        /// <param name="sectionParameterNames">Name of unit section parameters</param>
        public TdilSectionWriter(string sectionName, string[] sectionParameterNames) {
            if (string.IsNullOrEmpty(sectionName)) throw new ArgumentNullException("sectionName", "Must not be NULL or empty!");
            iSectionName = sectionName;
            iSectionParameterNames = sectionParameterNames;
        }

        /// <summary>
        /// Tells the instance to write a header.
        /// </summary>
        protected override void OnWriteHeader() {
            StringBuilder.Append(iSectionName);

            // Build argument list, if there are some
            string argumentSet = null;
            if (iSectionParameterNames != null && iSectionParameterNames.Length != 0) {
                string paramDeclarationLine = string.Join(", ", iSectionParameterNames);
                argumentSet = string.Format("({0})", paramDeclarationLine);
            }
            
            // Compose line tail
            string lineTail = string.Format(":{0}", argumentSet);

            // Append
            StringBuilder.AppendLine(lineTail);
        }

        /// <summary>
        /// Tells the instance to write a footer.
        /// </summary>
        protected override void OnWriteFooter() {
            StringBuilder.AppendLine("End");
        }

        /// <summary>
        /// Tells the instance to create a finalize exception with specific information. The client must 
        /// not throw it, just create.
        /// </summary>
        /// <returns>Newly created exception instance</returns>
        protected override Exception CreateFinalizedException() {
            return new TdilSectionAlreadyFinalizedException(string.Format("The current unit  section '{0}' has already been finalized!", iSectionName));
        }
    }
}