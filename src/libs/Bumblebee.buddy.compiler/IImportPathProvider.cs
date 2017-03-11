using System;
using Bumblebee.buddy.compiler.packaging;

namespace Bumblebee.buddy.compiler {
    /// <summary>
    /// Provides methods to map import statements to the corresponding TDIL unit file.
    /// </summary>
    public interface IImportPathProvider {
        /// <summary>
        /// Returns the full path of the unit file that is associated with the given <paramref name="unitName"/>.
        /// </summary>
        /// <param name="unitName">Import statement to look up</param>
        /// <returns>Full path of the associated unit file</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="unitName"/> is NULL</exception>
        string GetPath(UnitName unitName);

        /// <summary>
        /// Returns the unit name that is associated with the given <paramref name="scenarioName"/>. Returns NULL 
        /// if <paramref name="scenarioName"/> is NULL or empty.
        /// </summary>
        /// <param name="scenarioName">Name of the scenario</param>
        /// <returns>Unit name or NULL</returns>
        UnitName GetUnitName(string scenarioName);
    }
}