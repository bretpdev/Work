using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace RTRNMAILOL
{
    public class Args : KvpArgs
    {
        [KvpRequiredArg("live", "dev")]
        public DataAccessHelper.Mode Mode { get; set; }
        [KvpValidArg]
        public bool PauseBetweenRecords { get; set; }


        public Args(string[] arguments) : base(arguments)
        {
        }
    }
}