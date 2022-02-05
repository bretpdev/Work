using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;


namespace PEPSFED
{
    public class Args : KvpArgs
    {
        [KvpRequiredArg("dev", "live")]
        public string Mode { get; set; }
        [KvpValidArg]
        public bool ShowPrompts { get; set; } = true;
        [KvpValidArg]
        public bool SkipFileLoad { get; set; } = false;
        [KvpValidArg]
        public bool SkipProcessing { get; set; } = false;
        [KvpValidArg]
        public string LoadFrom { get; set; }


        public Args(string[] arguments)
            : base(arguments)
        {
            LoadFrom = LoadFrom?.Trim('"');
            if (string.IsNullOrWhiteSpace(LoadFrom))
                LoadFrom = null;
            else if (!LoadFrom.EndsWith("\\"))
                LoadFrom += "\\";
        }

        public void SetDataAccessValues()
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            if (Mode == "dev")
                DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            else
                DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Live;
        }
    }
}
