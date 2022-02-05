using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACURINTC
{
    class RejectReasonHelper
    {
        public List<RejectReasonInfo> Sources = new List<RejectReasonInfo>();
        public RejectReasonHelper(DataAccess da)
        {
            this.Sources = da.GetRejectReasons();
        }
        public string GetRejectReasonText(RejectReason reason)
        {
            return this.Sources.SingleOrDefault(o => o.RejectReasonId == reason).RejectReason;
        }
    }
}
