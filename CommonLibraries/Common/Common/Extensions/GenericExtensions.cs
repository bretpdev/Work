
using System;
using System.Collections.Generic;
namespace Uheaa.Common
{
    public static class GenericExtensions
    {
        public static T[] MakeArray<T>(this T obj)
        {
            return new T[] { obj };
        }

        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (T element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
