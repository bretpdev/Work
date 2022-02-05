using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptCoordinator
{
    [Serializable]
    public class DllInfo
    {
        public string Path { get; set; }
        public string MainClassName { get; set; }
        public bool UsesCommon { get; set; }
        public bool UsesQ { get; set; }
    }
}
