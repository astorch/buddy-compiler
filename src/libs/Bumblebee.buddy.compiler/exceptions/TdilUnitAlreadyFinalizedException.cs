﻿using System;

namespace Bumblebee.buddy.compiler.exceptions {
    /// <summary>
    /// Defines an exception that indicates that the program wants to extend an already finalized TDIL unit.
    /// </summary>
    public class TdilUnitAlreadyFinalizedException : Exception {
        /// <summary>
        /// Creates a new instance with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="message">Exception message</param>
        public TdilUnitAlreadyFinalizedException(string message) : base(message) {
            // Currently nothing to do here
        }

        /// <summary>
        /// Creates a new instance with the given <paramref name="message"/> and predecessing <paramref name="innerException"/>.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Predecessing exception</param>
        public TdilUnitAlreadyFinalizedException(string message, Exception innerException) : base(message, innerException) {
            // Currently nothing to do here
        }
    }
}