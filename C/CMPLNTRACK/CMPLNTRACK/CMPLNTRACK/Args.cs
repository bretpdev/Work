using Uheaa.Common;

namespace CMPLNTRACK
{
    public class Args : KvpArgs
    {
        [KvpValidArg("live", "dev")]
        public string Mode { get; set; }
        [KvpValidArg]
        public string AccountNumber { get; set; }
        [KvpValidArg("uheaa", "")]
        public string Region { get; set; }
        public Args(string[] arguments) : base(arguments) { }
    }
}