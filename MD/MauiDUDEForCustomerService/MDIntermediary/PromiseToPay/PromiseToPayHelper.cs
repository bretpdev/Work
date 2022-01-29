using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace MDIntermediary.PromiseToPay
{
    public class PromiseToPayHelper
    {
        private DataAccessHelper.Region region { get; set; }
        private IBorrower borrower { get; set; }
        public PromiseToPayHelper(IBorrower borrower, DataAccessHelper.Region region)
        {
            this.borrower = borrower;
            this.region = region;
        }

        public string ShowPromiseToPayForm()
        {
            using (PromiseToPay ptp = new PromiseToPay())
            {
                var result = ptp.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    //There is a warning if you call to string directly on the property because of how forms handles multi thread property access
                    int daysToExclude = ptp.DaysToExclude;
                    //AddBRPTPArc(ptp.DaysToExclude);
                    return daysToExclude.ToString();
                }
                return "";
            }
        }

        public void AddBRPTPArc(int days)
        {
            ArcData arc = new ArcData(region)
            {
                AccountNumber = borrower.AccountNumber.Replace(" ", ""),
                Arc = "BRPTP",
                Comment = days.ToString(),
                ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                ScriptId = "MD",
            };
            ArcAddResults result = arc.AddArc();
            if (!result.ArcAdded)
            {
                Dialog.Error.Ok("Promise to pay arc(BRPTP) NOT added.");
            }
            else
            {
                Dialog.Info.Ok("Promise to pay arc(BRPTP) added");
            }
        }
    }
}
