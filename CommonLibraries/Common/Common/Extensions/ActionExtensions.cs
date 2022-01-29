using System;

namespace Uheaa.Common
{
    public static class ActionExtensions
    {
        public static void SafeExecute(this Action a)
        {
            if (a != null) a();
        }
    }
}
