using System;
using xcite.collections;

namespace Bumblebee.buddy.compiler {
    /// <summary>
    /// Provides context information for a compiling operation. The current active instance is accessible 
    /// via <see cref="Current"/> until it has been disposed.
    /// </summary>
    public class CompilingContext : IDisposable {

        /// <summary>
        /// Returns the last instantiated instance until it has been disposed.
        /// </summary>
        public static CompilingContext Current { get; private set; }

        private readonly LinearList<string> iAliasReferenceSet = new LinearList<string>();
        private readonly LinearList<string> iUnitReferenceSet = new LinearList<string>();

        /// <summary>
        /// Initializes a new instance. This instance is accessible via <see cref="Current"/> until 
        /// <see cref="Dispose"/> is called.
        /// </summary>
        public CompilingContext() {
            Current = this;
        }

        /// <inheritdoc />
        public void Dispose() {
            Current = null;
        }

        /// <summary>
        /// Adds the given <paramref name="aliasReference"/> to the compiling context. If the value is NULL 
        /// or empty, nothing happens. If the value is already registered, nothing will happen, too.
        /// </summary>
        /// <param name="aliasReference">Value to add</param>
        public void AddAliasReference(string aliasReference) {
            if (string.IsNullOrEmpty(aliasReference)) return;
            if (iAliasReferenceSet.Contains(aliasReference)) return;
            iAliasReferenceSet.Add(aliasReference);
        }

        /// <summary>
        /// Adds the given <paramref name="unitReference"/> to the compiling context. If the value is NULL 
        /// or empty, nothing happens. If the value is already registered, nothing will happen, too.
        /// </summary>
        /// <param name="unitReference">Value to add</param>
        public void AddUnitReference(string unitReference) {
            if (string.IsNullOrEmpty(unitReference)) return;
            if (iUnitReferenceSet.Contains(unitReference)) return;
            iUnitReferenceSet.Add(unitReference);
        }

        /// <summary>
        /// Returns the current set of referenced alias.
        /// </summary>
        public string[] AliasReferenceSet { get { return iAliasReferenceSet.ToArray(); } }

        /// <summary>
        /// Returns the current set of referenced units.
        /// </summary>
        public string[] UnitReferenceSet { get { return iUnitReferenceSet.ToArray(); } }
    }
}