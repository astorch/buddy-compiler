using System;

namespace Bumblebee.buddy.compiler.exceptions {
    public class UnitBundlerException : Exception {
        /// <inheritdoc />
        public UnitBundlerException(string message) : base(message) {
        }

        /// <inheritdoc />
        public UnitBundlerException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}