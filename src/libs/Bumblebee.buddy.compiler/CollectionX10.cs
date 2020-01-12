using System.Collections;

namespace Bumblebee.buddy.compiler {
    /// <summary> Provides extension methods for <see cref="ICollection"/>. </summary>
    public static class CollectionX10 {
        /// <summary> Returns all items of the <paramref name="collection"/> as array. </summary>
        /// <typeparam name="TItem">Type of elements in the collection</typeparam>
        public static TItem[] ToArray<TItem>(this ICollection collection) {
            TItem[] resultSet = new TItem[collection.Count];
            collection.CopyTo(resultSet, 0);
            return resultSet;
        }
    }
}