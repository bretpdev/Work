using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace MDIntermediary
{
    public class CallsHelper
    {
        public bool TestMode { get; set; }
        public bool IncomingCall { get; set; }
        public CallsHelper(bool testMode, bool incomingCall)
        {
            this.TestMode = testMode;
            this.IncomingCall = incomingCall;
        }

        private List<CallReason> callReasonsInternal = null;
        private List<CallReason> CallReasons
        {
            get
            {
                if (callReasonsInternal == null)
                {
                    if (TestMode)
                        DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
                    else
                        DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Live;

                    DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;

                    callReasonsInternal = CallReason.GetCallReasons()
                        .Where(o => o.Uheaa)
                        .Where(o => (o.Inbound && IncomingCall) || (o.Outbound && !IncomingCall))
                        .ToList();
                }
                return callReasonsInternal;
            }
        }

        public List<string> GetCategories()
        {
            var categories = CallReasons.Select(o => o.Category).Distinct().OrderBy(o => o).ToList();
            categories.Insert(0, "");
            return categories;
        }

        public List<CallReason> GetReasons(string category)
        {
            var reasons = CallReasons.Where(o => o.Category == category).OrderBy(o => o.ReasonText).ToList();
            if (reasons.Any())
                reasons.Insert(0, CallReason.NoReason);
            return reasons;
        }
    }
}
