using System;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace BATCHESP
{
    public class Args : KvpArgs
    {
        [KvpRequiredArg("live", "dev")]
        public DataAccessHelper.Mode Mode { get; set; }
        [KvpValidArg]
        public bool SkipWorkAdd { get; set; }
        [KvpValidArg]
        public bool SkipTaskClose { get; set; }
        [KvpValidArg]
        public string AccountNumber { get; set; }
        [KvpValidArg]
        public string AccountIdentifiers { get; set; }
        [KvpValidArg]
        public string SubQueues { get; set; }
        [KvpValidArg]
        public bool SkipTaskAssign { get; set; }
        [KvpValidArg]
        public int NumberOfThreads { get; set; }
        public Args(string[] arguments) : base(arguments)
        {
            if (AccountIdentifiers == null && AccountNumber != null)
                AccountIdentifiers = AccountNumber;
        }
    }
}
