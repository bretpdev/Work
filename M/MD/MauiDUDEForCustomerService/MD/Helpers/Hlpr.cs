using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uheaa.Common.Scripts;

namespace MD
{
    public static class Hlpr
    {
        public static LoginHelper Login { get { return LoginHelper.Instance; } }
        public static ReflectionHelper RH { get { return ReflectionHelper.Instance; } }
        public static ReflectionInterface RI { get { return RH.CurrentSession; } }
        public static UIHelper UI { get { return UIHelper.Instance; } }

        public static void Instantiate()
        {
            RH.Instantiate();
        }
    }
}
