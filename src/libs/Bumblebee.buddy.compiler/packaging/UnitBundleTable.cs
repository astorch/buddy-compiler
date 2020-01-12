using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Bumblebee.buddy.compiler.packaging {
    /// <summary>
    /// Describes the content of a bundle that has been defined by the <see cref="UnitBundler"/>.
    /// </summary>
    public class UnitBundleTable : IEnumerable<UnitBundleTableEntry> {
        private readonly List<UnitBundleTableEntry> iEntryTable = new List<UnitBundleTableEntry>(100);

        /// <summary>
        /// Adds a new entry to the table with the given values.
        /// </summary>
        /// <param name="name">Name of the entry</param>
        /// <param name="data">Data of the entry</param>
        /// <exception cref="ArgumentNullException">If any parameter is NULL</exception>
        public void AddEntry(string name, byte[] data) {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name), "Must not be NULL or empty");
            if (data == null) throw new ArgumentNullException(nameof(data));

            iEntryTable.Add(new UnitBundleTableEntry(name, data));
        }

        /// <summary>
        /// Adds the given <paramref name="content"/> as new entry to the table with the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">Name of the entry</param>
        /// <param name="content">Content of the entry</param>
        /// <exception cref="ArgumentNullException">If any parameter is NULL</exception>
        public void AddEntry(string name, string content) {
            byte[] data = Encoding.UTF8.GetBytes(content);
            AddEntry(name, data);
        }

        /// <inheritdoc />
        public IEnumerator<UnitBundleTableEntry> GetEnumerator() {
            return iEntryTable.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}