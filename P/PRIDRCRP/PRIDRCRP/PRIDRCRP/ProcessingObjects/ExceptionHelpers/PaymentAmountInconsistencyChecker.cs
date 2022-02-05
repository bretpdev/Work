using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;

namespace PRIDRCRP
{
    public class PaymentAmountInconsistencyChecker
    {
        private decimal? lastPaymentAmount;
        private DataAccess DA;

        public PaymentAmountInconsistencyChecker(DataAccess DA)
        {
            this.DA = DA;   
        }

        public List<Error> CheckForPaymentAmount(string flatActivityDescription, string activityDescription, BorrowerActivityResult result, string lastPlanType)
        {
            List<Error> errorLog = new List<Error>();

            if (flatActivityDescription.Contains("PAYMENTAMOUNTCHANGEDFROM$"))
            {
                string startingPaymentAmt = StringParsingHelper.ReadNumericFromHeaderToString(flatActivityDescription, "PAYMENTAMOUNTCHANGEDFROM$", new char[] { '.', ',', ' ' });
                //Check if the last and starting payment amount match
                decimal? val = startingPaymentAmt.ToDecimalNullable();
                if (val.HasValue)
                {
                    if (lastPaymentAmount.HasValue && val != lastPaymentAmount.Value && result.ActivityDate >= DA.GetBorrowerInformation(result.BorrowerInformationId).FirstOrDefault().FirstPayDue)
                    {
                        errorLog.Add(new Error(result.BorrowerInformationId, $"Borrower Information Id: {result.BorrowerInformationId}  Borrower Activity Id: {result.BorrowerActivityId} Borrower payment amount went from {lastPaymentAmount.Value.ToString()} to {val.ToString()} without being logged;"));
                    }
                    //Set new payment amount 
                    string newPaymentAmt = StringParsingHelper.ReadNumericFromHeaderToString(flatActivityDescription, "PAYMENTAMOUNTCHANGEDFROM$" + val.Value.ToString("0.00") + "TO$", new char[] { '.', ',', ' ' });
                    if(newPaymentAmt.Length > 0 && newPaymentAmt[newPaymentAmt.Length - 1] == '.')
                    {
                        newPaymentAmt = newPaymentAmt.Substring(0, newPaymentAmt.Length - 1);
                    }
                    val = newPaymentAmt.ToDecimalNullable();
                    if (val.HasValue)
                    {
                        lastPaymentAmount = val;
                    }
                    else
                    {
                        errorLog.Add(new Error(result.BorrowerInformationId, $"Borrower Information Id: {result.BorrowerInformationId} Borrower Activity Id: {result.BorrowerActivityId} New borrower payment amount unable to be parsed using last known value: " + (lastPaymentAmount.HasValue ? lastPaymentAmount.Value.ToString() : "NULL") + ";"));
                    }
                }
            }
            if (flatActivityDescription.Contains("INTERESTRATEINEFFECT"))
            {
                string startingPaymentAmt = StringParsingHelper.ReadNumericFromHeaderToString(flatActivityDescription, "NEWPAYMENTAMOUNTIS$", new char[] { '.', ',', ' ' });
                if (startingPaymentAmt.Length > 0 && startingPaymentAmt[startingPaymentAmt.Length - 1] == '.')
                {
                    startingPaymentAmt = startingPaymentAmt.Substring(0, startingPaymentAmt.Length - 1);
                }
                decimal? val = startingPaymentAmt.ToDecimalNullable();
                if (val.HasValue)
                {
                    lastPaymentAmount = val;
                }
            }
            if(flatActivityDescription.Contains("PAYMENTAMOUNTCHANGEDTO"))
            {
                string startingPaymentAmt = StringParsingHelper.ReadNumericFromHeaderToString(flatActivityDescription, "PAYMENTAMOUNTCHANGEDTO", new char[] { '.', ',', ' ' });
                if (startingPaymentAmt.Length > 0 && startingPaymentAmt[startingPaymentAmt.Length - 1] == '.')
                {
                    startingPaymentAmt = startingPaymentAmt.Substring(0, startingPaymentAmt.Length - 1);
                }
                decimal? val = startingPaymentAmt.ToDecimalNullable();
                if (val.HasValue)
                {
                    lastPaymentAmount = val;
                }
            }
            if (flatActivityDescription.Contains("BORROWERSPAYMENTSTEMPORARILYCHANGEDTO$"))
            {
                string startingPaymentAmt = StringParsingHelper.ReadNumericFromHeaderToString(flatActivityDescription, "BORROWERSPAYMENTSTEMPORARILYCHANGEDTO$", new char[] { '.', ',', ' ' });
                if (startingPaymentAmt.Length > 0 && startingPaymentAmt[startingPaymentAmt.Length - 1] == '.')
                {
                    startingPaymentAmt = startingPaymentAmt.Substring(0, startingPaymentAmt.Length - 1);
                }
                decimal? val = startingPaymentAmt.ToDecimalNullable();
                if (val.HasValue)
                {
                    lastPaymentAmount = val;
                }
            }
            if (flatActivityDescription.Contains("#PMTSPMTAMTBEGINNING"))
            {
                var paymentChange = ChangeParsingHelper.ReadPaymentTable(activityDescription);
                if (paymentChange.PaymentAmount != "")
                {
                    decimal? val = paymentChange.PaymentAmount.ToDecimalNullable();
                    if (val.HasValue)
                    {
                        lastPaymentAmount = val;
                    }
                }
            }

            string lastPlanTypeString = lastPlanType == null ? "" : lastPlanType;
            if (DA.GetDateInDefFor(result.BorrowerInformationId, result.ActivityDate) ||
                lastPlanTypeString.Replace(" ", "") == "FORCEDICR" ||
                lastPlanTypeString.Replace(" ", "") == "INCOMEBASED" ||
                lastPlanTypeString.Replace(" ", "") == "INCOMECONTINGENT" ||
                lastPlanTypeString.Replace(" ", "") == "STANDARD")
            {
                return new List<Error>();
            }
            else
            {
                return errorLog;
            }
        }
    }
}
