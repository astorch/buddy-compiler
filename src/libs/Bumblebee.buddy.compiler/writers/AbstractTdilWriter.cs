using System;
using System.IO;
using System.Text;
using Bumblebee.buddy.compiler.exceptions;

namespace Bumblebee.buddy.compiler.writers {
    /// <summary>
    /// Provides an abstract writer implementation.
    /// </summary>
    public abstract class AbstractTdilWriter<TWriter> : IDisposable where TWriter : AbstractTdilWriter<TWriter> {
        private string iFinalStringContent;
        
        /// <summary>Initializes a new instance.</summary>
        protected AbstractTdilWriter() {
            StringBuilder = new StringBuilder();
        }

        /// <summary>
        /// Is raised when <see cref="Dispose"/> is called.
        /// </summary>
        public event EventHandler Disposing;

        /// <summary>
        /// Returns TRUE if the unit already has been finalized.
        /// </summary>
        public bool IsFinalized { get; private set; }

        /// <summary>
        /// Returns the underlying string builder.
        /// </summary>
        protected StringBuilder StringBuilder { get; private set; }

        /// <summary>
        /// Appends the given <paramref name="line"/> to the current unit followed by the default line terminator.
        /// </summary>
        /// <param name="line">Line to append</param>
        /// <returns>This to support fluent signatures</returns>
        /// <exception cref="TdilUnitAlreadyFinalizedException">If the unit has already been finalized</exception>
        public TWriter AppendLine(string line) {
            EnsureNotFinalized();
            EnsureHeaderWritten();
            StringBuilder.AppendLine(line);
            return (TWriter) this;
        }

        /// <summary>
        /// Appends an empty line to the current unit followed by the default line terminator.
        /// </summary>
        /// <returns>This to support fluent signatures</returns>
        public TWriter AppendLine() {
            return AppendLine(string.Empty);
        }

        /// <summary>
        /// Appends a formatted line to the current unit followed by the default line terminator.
        /// </summary>
        /// <param name="format">A composite format string</param>
        /// <param name="args">An object array that contains zero or more objects to format</param>
        /// <returns>This to support fluent signatures</returns>
        public TWriter AppendLine(string format, params object[] args) {
            string line = string.Format(format, args);
            return AppendLine(line);
        }

        /// <summary>
        /// Convertes the value of this instance to a string. By calling this method, the unit is going to be finalized.
        /// </summary>
        /// <returns>String representation of the value of this instance</returns>
        public string WriteToString() {
            EnsureHeaderWritten();
            if (IsFinalized) return iFinalStringContent;
            OnWriteFooter();
            IsFinalized = true;
            iFinalStringContent = StringBuilder.ToString();
            return iFinalStringContent;
        }

        /// <summary>
        /// Writes the value of this instance into the given <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">Stream to write into</param>
        public void WriteToStream(Stream stream) {
            using (BufferedStream bufferedStream = new BufferedStream(stream)) {
                using (StreamWriter streamWriter = new StreamWriter(bufferedStream)) {
                    string fileContent = WriteToString();
                    streamWriter.Write(fileContent);
                }
            }
        }

        /// <summary>
        /// Writes the value of this instance into a file described by the given <paramref name="fileInfo"/>.
        /// </summary>
        /// <param name="fileInfo">File info of the target file</param>
        public void WriteToFile(FileInfo fileInfo) {
            if (fileInfo == null) throw new ArgumentNullException("fileInfo");
            using (Stream fileStream = fileInfo.OpenWrite()) {
                WriteToStream(fileStream);
            }
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose() {
            OnDispose();
            if (Disposing == null) return;
            Disposing(this, EventArgs.Empty);
        }

        /// <summary>
        /// Is invoked when <see cref="Dispose"/> is called. The default implementation is empty. 
        /// Clients may override.
        /// </summary>
        protected virtual void OnDispose() {
            // Clients may override
        }

        /// <summary>
        /// Throws an <see cref="TdilUnitAlreadyFinalizedException"/> if the unit has already been finalized.
        /// </summary>
        protected void EnsureNotFinalized() {
            if (!IsFinalized) return;
            throw CreateFinalizedException();
        }

        /// <summary>
        /// Ensures that <see cref="OnWriteHeader"/> has been already been called.
        /// </summary>
        protected void EnsureHeaderWritten() {
            if (StringBuilder.Length != 0) return;
            OnWriteHeader();
        }

        /// <summary>
        /// Tells the instance to create a finalize exception with specific information. The client must 
        /// not throw it, just create.
        /// </summary>
        /// <returns>Newly created exception instance</returns>
        protected abstract Exception CreateFinalizedException();

        /// <summary>
        /// Tells the instance to write a header.
        /// </summary>
        protected abstract void OnWriteHeader();

        /// <summary>
        /// Tells the instance to write a footer.
        /// </summary>
        protected abstract void OnWriteFooter();
    }
}