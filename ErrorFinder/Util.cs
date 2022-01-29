using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ErrorFinder
{
    public static class Util
    {
        public static string TimeStamp()
        {
            return DateTime.Now.ToString("MM-dd-yy_hh.mm.ss");
        }
    }
}
