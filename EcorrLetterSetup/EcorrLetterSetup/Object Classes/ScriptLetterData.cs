using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcorrLetterSetup
{
    class ScriptLetterData
    {
        public int LetterHeaderMappingId { get; set; }
        public int HeaderId { get; set; }
        public string Header { get; set; }
        public int HeaderTypeId { get; set; }
        public string HeaderType { get; set; }
        public int Order { get; set; }
        public bool Active { get; set; }
    }
}
