using System;
using Bumblebee.buddy.compiler.exceptions;

namespace Bumblebee.buddy.compiler.writers {
    /// <summary>
    /// Implements a writer for creating TDIL units.
    /// </summary>
    public class TdilUnitWriter : AbstractTdilWriter<TdilUnitWriter> {
        private readonly string iUnitName;
        private readonly string[] iUnitParamNames;

        /// <summary>
        /// Creates a new instance. The given <paramref name="unitName"/> is written to the unit header. It must not be NULL or empty. 
        /// Additional unit parameter names are not declared.
        /// </summary>
        /// <param name="unitName">Name of the unit - must noch be NULL or empty</param>
        /// <exception cref="ArgumentNullException">If the given <paramref name="unitName"/> is NULL or empty</exception>
        public TdilUnitWriter(string unitName) : this(unitName, new string[0]) {
            // Currently nothing to do here
        }

        /// <summary>
        /// Creates a new instance. The given <paramref name="unitName"/> is written to the unit header. It must not be NULL or empty. 
        /// Additionally, if there are <paramref name="unitParamNames"/> given, they will be declared in the unit header, too.
        /// </summary>
        /// <param name="unitName">Name of the unit - must not be NULL or empty</param>
        /// <param name="unitParamNames">Collection of unit parameter names</param>
        /// <exception cref="ArgumentNullException">If the given <paramref name="unitName"/> is NULL or empty</exception>
        public TdilUnitWriter(string unitName, string[] unitParamNames) {
            if (string.IsNullOrEmpty(unitName)) throw new ArgumentNullException("unitName", "Must not be NULL or empty!");
            iUnitName = unitName;
            iUnitParamNames = unitParamNames;
        }

        /// <summary>
        /// Creates a new instance of <see cref="TdilSectionWriter"/>. The content of this writer is being appended to the 
        /// unit writer when it's being disposed.
        /// </summary>
        /// <param name="sectionName">Name of the section</param>
        /// <returns>Instance of <see cref="TdilSectionWriter"/></returns>
        public TdilSectionWriter CreateSection(string sectionName) {
            return CreateSection(sectionName, new string[0]);
        }

        /// <summary>
        /// Creates a new instance of <see cref="TdilSectionWriter"/>. The content of this writer is being appended to the 
        /// unit writer when it's being disposed.
        /// </summary>
        /// <param name="sectionName">Name of the section</param>
        /// <param name="sectionParameterNames">Name of section parameters</param>
        /// <returns>Instance of <see cref="TdilSectionWriter"/></returns>
        public TdilSectionWriter CreateSection(string sectionName, string[] sectionParameterNames) {
            EnsureHeaderWritten();

            TdilSectionWriter sectionWriter = new TdilSectionWriter(sectionName, sectionParameterNames);
            sectionWriter.Disposing += OnSectionWriterDisposing;
            return sectionWriter;
        }

        /// <summary>
        /// Is invoked when an instance of <see cref="TdilSectionWriter"/> is disposed that has being created by calling 
        /// <see cref="CreateSection(string)"/>.
        /// </summary>
        /// <param name="sender">Event dispatcher</param>
        /// <param name="eventArgs">Event arguments</param>
        private void OnSectionWriterDisposing(object sender, EventArgs eventArgs) {
            TdilSectionWriter sectionWriter = (TdilSectionWriter) sender;
            sectionWriter.Disposing -= OnSectionWriterDisposing;

            string sectionText = sectionWriter.WriteToString();
            AppendLine(sectionText);
        }

        /// <summary>
        /// Tells the instance to write a header.
        /// </summary>
        protected override void OnWriteHeader() {
            StringBuilder.Append(string.Format("Unit {0}", iUnitName));

            // Add the unit global parameter names if there are some
            if (iUnitParamNames != null && iUnitParamNames.Length != 0) {
                string paramDeclarationLine = string.Join(", ", iUnitParamNames);
                StringBuilder.AppendFormat(" ({0})", paramDeclarationLine);
            }

            // Add line terminator
            StringBuilder.AppendLine();
            
            // Add empty line
            StringBuilder.AppendLine();
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
            return new TdilUnitAlreadyFinalizedException(string.Format("The current unit '{0}' has already been finalized!", iUnitName));
        }
    }
}