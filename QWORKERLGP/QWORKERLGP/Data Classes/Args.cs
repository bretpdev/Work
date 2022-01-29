using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;

namespace QWORKERLGP
{
    class Args : KvpArgs
    {
        [KvpRequiredArg("dev", "live")]
        public string Mode { get; set; }
        [KvpValidArg]
        public bool ShowPrompts { get; set; } = true;
        [KvpValidArg]
        public int NumberOfThreads { get; set; } = 1;
        [KvpValidArg]
        public bool PauseForTest { get; set; } = false;

        public Args(string[] args)
            : base(args)
        { }
    }
}
