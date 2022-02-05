using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace ACS
{
    class Args : KvpArgs
    {
        [KvpRequiredArg("live", "test", "dev")]
        public DataAccessHelper.Mode Mode { get; set; }

        [KvpValidArg]
        public DateTime? RunDate { get; set; }

        public Args(string[] arguments) : base(arguments)
        {
        }
    }
}
