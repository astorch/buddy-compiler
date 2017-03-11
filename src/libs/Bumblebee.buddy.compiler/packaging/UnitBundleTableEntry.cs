namespace Bumblebee.buddy.compiler.packaging {
    /// <summary>
    /// Describes an entry of a <see cref="UnitBundleTable"/>.
    /// </summary>
    public class UnitBundleTableEntry {
        /// <summary>
        /// Creates a new instance with the given arguments.
        /// </summary>
        /// <param name="name">Name of the entry</param>
        /// <param name="data">Data of the entry</param>
        public UnitBundleTableEntry(string name, byte[] data) {
            Name = name;
            Data = data;
        }

        /// <summary>
        /// Returns the name of the entry.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Returns the data of the entry.
        /// </summary>
        public byte[] Data { get; private set; }
    }
}