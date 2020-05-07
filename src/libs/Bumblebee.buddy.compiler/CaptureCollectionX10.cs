using System.Collections;
using System.Text.RegularExpressions;

namespace Bumblebee.buddy.compiler {
    /// <summary> Provides extension methods for <see cref="ICollection"/>. </summary>
    public static class CaptureCollectionX10 {
        
        /// <summary>
        /// Returns all item of the <paramref name="collection"/> as string array.
        /// The string is taken from the collection item value.
        /// </summary>
        public static string[] ToArray(this CaptureCollection collection) {
            string[] resultSet = new string[collection.Count];
            for (int i = -1; ++i != collection.Count;) {
                resultSet[i] = collection[i].Value;
            }

            return resultSet;
        }
        
    }
}