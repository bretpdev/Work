using Uheaa.Common.DataAccess;
using Uheaa.Common;

namespace IDRRPTFED
{
    public class Arguments : KvpArgs
    {
        [KvpRequiredArg("dev", "live")]
        public string Mode { get; set; }
        [KvpValidArg]
        public bool Legacy { get; set; }
        [KvpValidArg]
        public static bool ShowPrompts { get; set; }
        [KvpValidArg("byapp", "byfile", "bydate")]
        public static string RunType { get; set; }
        [KvpValidArg]
        public static string File { get; set; }
        public Arguments(string[] args)
            : base(args)
        {
        }

        public void SetDataAccessValues(string[] args)
        {
            if (Mode == "dev")
                DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            else
                DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Live;
        }
    }
}