using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;

namespace DirectoryCompressor
{
    public class Args : KvpArgs
    {
        [KvpValidArg("cmd", "gui")]
        public string Mode { get; set; }
        [KvpValidArg]
        public string Location { get; set; }
        [KvpValidArg]
        public string Destination { get; set; }

        public Args(string[] args)
            : base(args)
        {
            if (string.IsNullOrEmpty(Mode))
                Mode = "gui";
        }

    }
}
