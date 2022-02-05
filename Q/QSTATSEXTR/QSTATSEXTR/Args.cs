using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace QSTATSEXTR
{
    class Args : KvpArgs
    {
        [KvpRequiredArg]
        public DataAccessHelper.Mode Mode { get; set; }
        [KvpValidArg]
        public string RunDate { get; set; } 
        public Args(string[] arguments) : base(arguments) { }
    }
}
