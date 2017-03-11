using System;
using Bumblebee.buddy.compiler.exceptions;

namespace Bumblebee.buddy.compiler.writers {
    /// <summary>
    /// Implements a writer for creating TDIL files.
    /// </summary>
    public class TdilFileWriter : AbstractTdilWriter<TdilFileWriter> {
        private readonly string iTdilVersion;

        /// <summary>
        /// Creates a new instance. The given <paramref name="tdilVersion"/> is written to the file header.
        /// </summary>
        /// <param name="tdilVersion">TDIL Version - Must not be NULL or empty</param>
        public TdilFileWriter(string tdilVersion) {
            if (string.IsNullOrEmpty(tdilVersion)) throw new ArgumentNullException("tdilVersion", "Must not be null or empty!");
            iTdilVersion = tdilVersion;
        }

        /// <summary>
        /// Creates a new instance of <see cref="TdilUnitWriter"/>. The content of this writer is being appended to the 
        /// file writer when it's being disposed.
        /// </summary>
        /// <param name="unitName">Name of the unit</param>
        /// <returns>Instance of <see cref="TdilUnitWriter"/></returns>
        public TdilUnitWriter CreateUnit(string unitName) {
            return CreateUnit(unitName, new string[0]);
        }

        /// <summary>
        /// Creates a new instance of <see cref="TdilUnitWriter"/>. The content of this writer is being appended to the 
        /// file writer when it's being disposed.
        /// </summary>
        /// <param name="unitName">Name of the unit</param>
        /// <param name="unitParamNames">Name of unit parameters</param>
        /// <returns>Instance of <see cref="TdilUnitWriter"/></returns>
        /// <exception cref="ArgumentNullException">If <paramref name="unitName"/> is NULL or empty</exception>
        public TdilUnitWriter CreateUnit(string unitName, string[] unitParamNames) {
            if (string.IsNullOrEmpty(unitName)) throw new ArgumentNullException("unitName", "Must not be NULL or empty!");
            
            if (unitParamNames == null)
                unitParamNames = new string[0];

            EnsureHeaderWritten();
            AppendLine();

            TdilUnitWriter unitWriter = new TdilUnitWriter(unitName, unitParamNames);
            unitWriter.Disposing += OnUnitWriterDisposing;
            return unitWriter;
        }

        /// <summary>
        /// Adds an import statement for the given <paramref name="importReference"/> to the current unit.
        /// If <paramref name="importReference"/> is NULL or empty, nothing will happen.
        /// </summary>
        /// <param name="importReference">Import reference to add</param>
        /// <returns>This to support fluent signatures</returns>
        public TdilFileWriter AddImport(string importReference) {
            if (string.IsNullOrEmpty(importReference)) return this;
            return AddImport(new[] {importReference});
        }

        /// <summary>
        /// Adds an import statement for each of the given <paramref name="importReferences"/> to the current unit. 
        /// If <paramref name="importReferences"/> is NULL, nothing happens.
        /// </summary>
        /// <param name="importReferences">Set of import references to add</param>
        /// <returns>This to support fluent signatures</returns>
        public TdilFileWriter AddImport(string[] importReferences) {
            if (importReferences == null) return this;
            if (importReferences.Length == 0) return this;

            EnsureHeaderWritten();
            AppendLine();

            for (int i = -1; ++i != importReferences.Length;) {
                string importReference = importReferences[i];
                string importStatement = string.Format("#include \"{0}\"", importReference);
                AppendLine(importStatement);
            }

            return this;
        }

        /// <summary>
        /// Adds an alias statement for the given <paramref name="aliasReference"/> to the current unit.
        /// If <paramref name="aliasReference"/> is NULL or empty, nothing will happen.
        /// </summary>
        /// <param name="aliasReference">Alias reference to add</param>
        /// <returns>This to support fluent signatures</returns>
        public TdilFileWriter AddAlias(string aliasReference) {
            if (string.IsNullOrEmpty(aliasReference)) return this;
            return AddAlias(new[] {aliasReference});
        }

        /// <summary>
        /// Adds an alias statement for each of the given <paramref name="aliasReferences"/> to the current unit. 
        /// If <paramref name="aliasReferences"/> is NULL, nothing happens.
        /// </summary>
        /// <param name="aliasReferences">Set of alias references to add</param>
        /// <returns>This to support fluent signatures</returns>
        public TdilFileWriter AddAlias(string[] aliasReferences) {
            if (aliasReferences == null) return this;
            if (aliasReferences.Length == 0) return this;

            EnsureHeaderWritten();
            AppendLine();

            for (int i = -1; ++i != aliasReferences.Length;) {
                string aliasReference = aliasReferences[i];
                string aliasStatement = string.Format("#alias \"{0}\"", aliasReference);
                AppendLine(aliasStatement);
            }

            return this;
        }

        /// <summary>
        /// Is invoked when an instance of <see cref="TdilUnitWriter"/> is disposed that has being created by calling 
        /// <see cref="CreateUnit(string)"/>.
        /// </summary>
        /// <param name="sender">Event dispatcher</param>
        /// <param name="eventArgs">Event arguments</param>
        private void OnUnitWriterDisposing(object sender, EventArgs eventArgs) {
            TdilUnitWriter unitWriter = (TdilUnitWriter) sender;
            unitWriter.Disposing -= OnUnitWriterDisposing;

            string unitText = unitWriter.WriteToString();
            AppendLine(unitText);
        }

        /// <summary>
        /// Tells the instance to write a header.
        /// </summary>
        protected override void OnWriteHeader() {
            StringBuilder
                .AppendLine(string.Format("// Compiler generated file"))
                .AppendLine(string.Format("// Buddy Compiler version {0}", iTdilVersion))
                .AppendLine(string.Format("// Generated on {0}", DateTime.Now))
                ;
        }

        /// <summary>
        /// Tells the instance to write a footer.
        /// </summary>
        protected override void OnWriteFooter() {
            // Currently nothing to do here
        }

        /// <summary>
        /// Tells the instance to create a finalize exception with specific information. The client must 
        /// not throw it, just create.
        /// </summary>
        /// <returns>Newly created exception instance</returns>
        protected override Exception CreateFinalizedException() {
            return new TdilFileAlreadyFinalizedException("The current tdil file has already been finalized!");
        }
    }
}