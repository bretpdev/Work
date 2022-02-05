using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACURINTC
{
    class RejectionHelper
    {
        public static Dictionary<DemographicsSource, List<RejectAction>> cache = new Dictionary<DemographicsSource, List<RejectAction>>();
        public List<RejectAction> Rejections { get; private set; }
        public RejectionHelper(DataAccess da, DemographicsSource demographicsSource)
        {
            if (!cache.ContainsKey(demographicsSource))
                cache[demographicsSource] = da.GetRejectActions(demographicsSource);
            this.Rejections = cache[demographicsSource];
        }
        public string GetRejectActionCode(RejectReason reason, DemographicType demographicType)
        {
            RejectAction action = Rejections.Where(p => p.RejectReasonId == reason).SingleOrDefault();
            switch (demographicType)
            {
                case DemographicType.Address:
                    return action.ActionCodeAddress;
                case DemographicType.Email:
                    return action.ActionCodeEmail;
                case DemographicType.Phone:
                    return action.ActionCodePhone;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
