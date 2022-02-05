using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace MDIntermediary
{
    public class CallReason
    {
        public int ReasonId { get; set; }
        public string Category { get; set; }
        public string ReasonText { get; set; }
        public bool Cornerstone { get; set; }
        public bool Uheaa { get; set; }
        public bool Inbound { get; set; }
        public bool Outbound { get; set; }

        [UsesSproc(DataAccessHelper.Database.MauiDude, "[calls].[GetAllEnabledReasons]")]
        public static List<CallReason> GetCallReasons()
        {
            return DataAccessHelper.ExecuteList<CallReason>("[calls].[GetAllEnabledReasons]", DataAccessHelper.Database.MauiDude);
        }

        static CallReason()
        {
            NoReason = new CallReason() { ReasonText = "" };
        }
        public static CallReason NoReason { get; private set; }
    }
}
