using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common;

namespace EMAILBTCF
{
    public class Program
    {
        public static int Main(string[] args)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            if (DataAccessHelper.StandardArgsCheck(args, "EMAILBTCF", false))
            {
                int? filterCampaignId = null;
                var arg = args.SingleOrDefault(o => o.ToLower().StartsWith("filteremailcampaignid:"));
                if (arg != null)
                    filterCampaignId = arg.Split(':').Last().ToInt();
                new EmailBatchScript().Main(filterCampaignId);
                return 0;
            }
            return 1;
        }
    
    }
}
