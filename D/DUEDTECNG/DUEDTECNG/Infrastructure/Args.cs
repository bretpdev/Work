using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace DUEDTECNG
{
    class Args : KvpArgs
    {
        public Args(string[] arguments) : base(arguments) { }
        [KvpRequiredArg("live", "test")]
        public DataAccessHelper.Mode Mode { get; set; }
        [KvpRequiredArg]
        public List<byte> TargetDueDates { get; set; }
        [KvpRequiredArg]
        public byte NewDueDate { get; set; }
        [KvpValidArg]
        public List<string> AccountIdentifiers { get; set; } = new List<string>();
        [KvpValidArg]
        public bool SkipWorkAdd { get; set; }
        [KvpValidArg]
        public bool SkipWorkProcess { get; set; }
        [KvpValidArg]
        public int ThreadCount { get; set; } = 10;
        [KvpValidArg]
        public int? WorkAddLimit { get; set; }
    }
}
