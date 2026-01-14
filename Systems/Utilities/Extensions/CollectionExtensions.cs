using System;
using System.Collections.Generic;

namespace Administrator.Utilities.Extensions
{
    /// <summary> Helpful methods for working with collections. </summary>
    public static class CollectionExtensions
    {
        /// <summary> Split an array into a jagged array using the given separator as a delineator. </summary>
        /// <typeparam name="T"> The kind of array. </typeparam>
        /// <param name="source"> The source array. </param>
        /// <param name="separator"> The item used as a delineator. Is not preserved. </param>
        /// <returns> The resulting jagged array. </returns>
        public static IEnumerable<IEnumerable<T>> SplitArray<T>(this IEnumerable<T> source, T separator)
        {
            List<T> current = new List<T>();
            foreach (T? item in source)
            {
                // Use Object.Equals for reliable comparison of generic types.
                if (Object.Equals(item, separator))
                {
                    yield return current;
                    current = new List<T>();
                }
                else
                {
                    current.Add(item);
                }
            }
            yield return current; // Return the last segment.
        }
    }
}
