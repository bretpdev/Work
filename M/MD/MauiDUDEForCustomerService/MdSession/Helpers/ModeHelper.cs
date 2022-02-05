using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MdSession
{
    public static class ModeHelper
    {
        private static bool testMode = true;
        public static bool TestMode { get { return testMode; } set { testMode = value; } }
        public static bool LiveMode { get { return !TestMode; } set { TestMode = !value; } }
    }
}
