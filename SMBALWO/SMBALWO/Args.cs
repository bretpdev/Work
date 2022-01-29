using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace SMBALWO
{
    class Args : KvpArgs
    {
        [KvpRequiredArg("dev", "live")]
        public string Mode { get; set; }
        [KvpValidArg]
        public bool ShowPrompts { get; set; } = true;
        [KvpValidArg]
        public bool LoadData { get; set; } = true;

        public Args(string[] arguments)
            : base(arguments)
        {
        }

        public void SetDataAccessValues()
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;

            if (Mode == "dev")
                DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            else
                DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Live;
        }
    }
}