using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Uheaa.Common
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Returns only the items in the given collection that are of type T
        /// </summary>
        public static IEnumerable<T> Filter<T>(this IEnumerable collection)
        {
            return collection.Cast<object>().Where(o => o is T).Cast<T>();
        }

        public static int? IndexOf<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
        {
            int index = 0;
            foreach (T item in collection)
            {
                if (predicate(item))
                    return index;
                index++;
            }
            return null;
        }

        public static IEnumerable<T> Append<T>(this IEnumerable<T> collection, T extraElement)
        {
            return collection.Concat(new T[] { extraElement });
        }

        /// <summary>
        /// Given a valid getChildren argument, returns the fully traversed list of Ts and T's children, recursively.
        /// </summary>
        public static IEnumerable<T> Recurse<T>(this IEnumerable<T> collection, Func<T, IEnumerable<T>> getChildren)
        {
            List<T> list = new List<T>();
            foreach (T t in collection)
            {
                list.Add(t);
                list.AddRange(getChildren(t).Recurse(getChildren));
            }
            return list;
        }
    }
}
