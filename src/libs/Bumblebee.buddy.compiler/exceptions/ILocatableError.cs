using System.Runtime.InteropServices;

namespace Bumblebee.buddy.compiler.exceptions {
    /// <summary>
    /// Indicates that the exception publishes information for locating the error.
    /// </summary>
    public interface ILocatableError : _Exception {
        /// <summary>
        /// Returns the line index the error is located.
        /// </summary>
        uint LineIndex { get; }

        /// <summary>
        /// Returns the column index the error is located.
        /// </summary>
        uint ColumnIndex { get; }
    }
}