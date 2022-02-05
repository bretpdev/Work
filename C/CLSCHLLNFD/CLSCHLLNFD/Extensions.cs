using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLSCHLLNFD
{
    public static class Extensions
    {
        public static bool IsIn(this string str, List<string> items)
        {
            return items.Contains(str);
        }

    }
}
