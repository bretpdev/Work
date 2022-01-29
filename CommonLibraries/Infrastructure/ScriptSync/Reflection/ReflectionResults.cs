using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Uheaa
{
    [Serializable]
    public class ReflectionResults
    {
        public string MainClassName { get; internal set; }
        public bool UsesCommon { get; internal set; }
        public bool UsesQ { get; internal set; }
        public Assembly Assembly { get; internal set; }
    }
}
