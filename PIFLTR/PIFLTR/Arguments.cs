using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace PIFLTR
{
    public class Arguments : KvpArgs
    {
        [KvpRequiredArg("dev", "live")]
        public string Mode { get; set; }
        [KvpValidArg]
        public bool ShowPrompts { get; set; } 

        public Arguments(string[] args)
            : base(args)
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