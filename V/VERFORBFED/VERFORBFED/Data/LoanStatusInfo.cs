using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VERFORBFED
{
    public class LoanStatusInfo
    {
        public string WC_DW_LON_STA { get; set; }
		public string WX_OVR_DW_LON_STA { get; set; }
        public string SimpleName { get; set; }
        public LoanStatusInfo() { }
        public LoanStatusInfo(string WC_DW_LON_STA, string WX_OVR_DW_LON_STA, string simpleName)
        {
            this.WC_DW_LON_STA = WC_DW_LON_STA;
            this.WX_OVR_DW_LON_STA = WX_OVR_DW_LON_STA;
            this.SimpleName = simpleName;
        }

        public bool Matches(LoanStatusInfo other)
        {
            if (WC_DW_LON_STA != other.WC_DW_LON_STA && WC_DW_LON_STA != null && other.WC_DW_LON_STA != null)
                return false;
            if (WX_OVR_DW_LON_STA != other.WX_OVR_DW_LON_STA && WX_OVR_DW_LON_STA != null && other.WX_OVR_DW_LON_STA != null)
                return false;
            return true;
        }

        public const string REPAYMENT = "03";
        public const string DEFERMENT = "04";
        public const string FORBEARANCE = "05";
        public const string PAIDINFULL = "22";
    }
}
