using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace DTX7LFED
{
    public class Args : KvpArgs
    {
        [KvpRequiredArg("dev", "live")]
        public string Mode { get; set; }
        [KvpRequiredArg("uheaa", "cornerstone")]
        public string Region { get; set; }
        [KvpValidArg]
        public bool ShowPrompts { get; set; } = true;
        [KvpValidArg]
        public int NumberOfThreads { get; set; } = 1;
        [KvpValidArg]
        public int? MaxExpiredRecordsToProcess { get; set; }
        [KvpValidArg]
        public int? MaxDueDilRecordsToProcess { get; set; }
        [KvpValidArg]
        public List<string> LetterIds { get; set; } = new List<string>();

        public Args(string[] args)
            : base(args)
        {
        }

        public void SetDataAccessValues()
        {
            if (Region == "cornerstone")//THE ARGS VALIDATOR WILL ENSURE THAT ONLY THE CORRECT REGIONS WILL BE ACCEPTED
                DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            else
                DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;

            if (Mode == "dev")
                DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            else
                DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Live;
        }
    }
}
