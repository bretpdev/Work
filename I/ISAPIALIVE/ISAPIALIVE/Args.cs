using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace ISAPIALIVE
{
    public class Args : KvpArgs
    {
        [KvpRequiredArg("live", "dev")]
        public DataAccessHelper.Mode Mode { get; set; }
        [KvpRequiredArg("uheaa")]
        public DataAccessHelper.Region Region { get; set; }
        [KvpRequiredArg]
        public string AccountNumber { get; set; }

        public Args(string[] arguments) : base(arguments)
        {
        }
    }
}