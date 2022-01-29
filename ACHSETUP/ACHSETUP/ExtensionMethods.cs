using System.Collections.Generic;
using System.Linq;

namespace ACHSETUP
{
    static class ExtensionMethods
    {
        public static string FormatForComments(this IEnumerable<int> loanSequences)
        {
            return string.Join(", ", loanSequences.Select(p => p.ToString()).ToArray());
        }
    }
}