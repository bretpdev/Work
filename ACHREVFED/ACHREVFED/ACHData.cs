using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACHREVFED
{
    public class ACHData
    {
        public int ACHReviewId { get; set; }
        public string AccountNumber { get; set; }
        public string Ssn { get; set; }
        public string Name { get; set; }
        public int DaysDelq { get; set; } = 0;
        public string HomeConsent { get; set; }
        public string HomePhone { get; set; }
        public string AltConsent { get; set; }
        public string AltPhone { get; set; }
        public string WorkConsent { get; set; }
        public string WorkPhone { get; set; }
        public string DX_STR_ADR_1 { get; set; }
        public string DX_STR_ADR_2 { get; set; }
        public string DX_STR_ADR_3 { get; set; }
        public string DM_CT { get; set; }
        public string DC_DOM_ST { get; set; }
        public string DF_ZIP_CDE { get; set; }
        public string LF_LON_CUR_OWN { get; set; }
        public decimal AmountDue { get; set; } = 0;
        public decimal TotalBalance { get; set; } = 0;
        public bool IsCoBorrower { get; set; } = false;
        public string BorrowerSsn { get; set; } = null;

        public override string ToString()
        {
            return string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}", Ssn.PadRight(14, ' '), Name.PadRight(55, ' '), DaysDelq.ToString().PadRight(89, ' '), HomeConsent,
                HomePhone.PadRight(15, ' '), AltConsent, AltPhone.PadRight(15, ' '), WorkConsent, WorkPhone.PadRight(68, ' '), DX_STR_ADR_1.PadRight(30, ' '), DX_STR_ADR_2.PadRight(30, ' '), DX_STR_ADR_3.PadRight(30, ' '),
                DM_CT.PadRight(20, ' '), DC_DOM_ST.PadRight(2, ' '), DF_ZIP_CDE.PadRight(42, ' '), LF_LON_CUR_OWN.PadRight(8, ' '), AmountDue.ToString().PadRight(7, ' ').Replace(".", ""), TotalBalance.ToString().PadRight(10).Replace(".", ""));
        }
    }
}
