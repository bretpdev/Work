using Uheaa.Common;

namespace QUECOMPLET
{
    public class Args : KvpArgs
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