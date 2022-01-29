using System;
using System.Collections.Generic;
using System.Linq;

namespace BATCHESP
{
    public static class IEnumerableExtensions
    {
        public static bool AllAndAny<T>(this IEnumerable<T> list, Func<T, bool> predicate)
        {
            return list.Any() && list.All(predicate);
        }
    }
}
