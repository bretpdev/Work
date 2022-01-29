using Uheaa.Common;

namespace TCPAPNS
{
    public class Args : KvpArgs
    {
        [KvpRequiredArg("dev", "live")]
        public string Mode { get; set; }
        [KvpValidArg]
        public bool Onelink { get; set; } = false;
        [KvpValidArg]
        public bool Fileload { get; set; } = false;

        public Args(string[] args)
            :base(args)
        { }
    }
}