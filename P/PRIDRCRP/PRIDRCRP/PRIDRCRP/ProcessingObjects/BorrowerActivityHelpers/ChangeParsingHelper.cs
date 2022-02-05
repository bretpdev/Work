using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;

namespace PRIDRCRP
{
    public static class ChangeParsingHelper
    {
        public static List<Error> CheckPmtAmt(RepaymentPlanChangesRecord record, string flatActivityDescription, BorrowerActivityResult result)
        {
            List<Error> errorLog = new List<Error>();

            if (flatActivityDescription.Contains("PAYMENTAMOUNTCHANGEDFROM$"))
            {
                string secondPart = flatActivityDescription.Substring(flatActivityDescription.IndexOf("PAYMENTAMOUNTCHANGEDFROM$"));
                string startingPaymentAmt = StringParsingHelper.ReadNumericFromHeaderToString(secondPart, "TO", new char[] { '.', ',', ' ', '$' }).Replace("$","").TrimEnd(new char[] { '.'});
                decimal? val = startingPaymentAmt.ToDecimalNullable();
                if (val.HasValue && !record.PmtAmt.HasValue)
                {
                    record.PmtAmt = val;
                }
                else if (val.HasValue && record.PmtAmt.HasValue)
                {
                    errorLog.Add(new Error(result.BorrowerInformationId, $"Borrower Information Id: {result.BorrowerInformationId.ToString()} Borrower Activity Id: {result.BorrowerActivityId.ToString()} Payment amount already exists for repayment plan change. Existing amount: {record.PmtAmt.Value.ToString()} Additional principal: {val.ToString()};".Trim()));
                }
            }
            if (flatActivityDescription.Contains("PAYMENTAMOUNTCHANGEDTO"))
            {
                string startingPaymentAmt = StringParsingHelper.ReadNumericFromHeaderToString(flatActivityDescription, "PAYMENTAMOUNTCHANGEDTO", new char[] { '.', ',', ' ', '$' }).Replace("$", "").TrimEnd(new char[] { '.' });
                decimal? val = startingPaymentAmt.ToDecimalNullable();
                if (val.HasValue && !record.PmtAmt.HasValue)
                {
                    record.PmtAmt = val;
                }
                else if (val.HasValue && record.PmtAmt.HasValue)
                {
                    errorLog.Add(new Error(result.BorrowerInformationId, $"Borrower Information Id: {result.BorrowerInformationId.ToString()} Borrower Activity Id: {result.BorrowerActivityId.ToString()} Payment amount already exists for repayment plan change. Existing amount: {record.PmtAmt.Value.ToString()} Additional principal: {val.ToString()};".Trim()));
                }
            }
            if (flatActivityDescription.Contains("BORROWERSPAYMENTSTEMPORARILYCHANGEDTO$"))
            {
                string startingPaymentAmt = StringParsingHelper.ReadNumericFromHeaderToString(flatActivityDescription, "BORROWERSPAYMENTSTEMPORARILYCHANGEDTO$", new char[] { '.', ',', ' ', '$' }).Replace("$", "").TrimEnd(new char[] { '.' });
                if (startingPaymentAmt.Length > 0 && startingPaymentAmt[startingPaymentAmt.Length - 1] == '.')
                {
                    startingPaymentAmt = startingPaymentAmt.Substring(0, startingPaymentAmt.Length - 1);
                }
                decimal? val = startingPaymentAmt.ToDecimalNullable();
                if (val.HasValue && !record.PmtAmt.HasValue)
                {
                    record.PmtAmt = val;
                }
                else if (val.HasValue && record.PmtAmt.HasValue)
                {
                    errorLog.Add(new Error(result.BorrowerInformationId, $"Borrower Information Id: {result.BorrowerInformationId.ToString()} Borrower Activity Id: {result.BorrowerActivityId.ToString()} Payment amount already exists for repayment plan change. Existing amount: {record.PmtAmt.Value.ToString()} Additional principal:  {val.ToString()};".Trim()));
                }
            }
            if (flatActivityDescription.Contains("INTERESTRATEINEFFECT"))
            {
                string startingPaymentAmt = StringParsingHelper.ReadNumericFromHeaderToString(flatActivityDescription, "NEWPAYMENTAMOUNTIS$", new char[] { '.', ',', ' ', '$' }).Replace("$", "").TrimEnd(new char[] { '.' });
                if (startingPaymentAmt.Length > 0 && startingPaymentAmt[startingPaymentAmt.Length - 1] == '.')
                {
                    startingPaymentAmt = startingPaymentAmt.Substring(0, startingPaymentAmt.Length - 1);
                }
                decimal? val = startingPaymentAmt.ToDecimalNullable();
                if (val.HasValue && !record.PmtAmt.HasValue)
                {
                    record.PmtAmt = val;
                }
                else if (val.HasValue && record.PmtAmt.HasValue)
                {
                    errorLog.Add(new Error(result.BorrowerInformationId, $"Borrower Information Id: {result.BorrowerInformationId.ToString()} Borrower Activity Id: {result.BorrowerActivityId.ToString()} Payment amount already exists for repayment plan change. Existing amount: {record.PmtAmt.Value.ToString()} Additional principal: {val.ToString()};".Trim()));
                }
            }
            return errorLog;
        }

        public static void CheckPaymentTable(RepaymentPlanChangesRecord record, BorrowerActivityResult result, string flatActivityDescription)
        {
            if (flatActivityDescription.Contains("#PMTSPMTAMTBEGINNING"))
            {
                RawPaymentChange payChange = ReadPaymentTable(result.ActivityDescription);
                if (payChange.PaymentAmount != "" && !record.PmtAmt.HasValue)
                {
                    record.PmtAmt = Convert.ToDecimal(payChange.PaymentAmount);
                }
                if (payChange.EffectiveDate != "" && !record.EffectiveDate.HasValue)
                {
                    record.EffectiveDate = Convert.ToDateTime(payChange.EffectiveDate);
                }
            }
        }

        public static List<Error> CheckFirstPayDue(RepaymentPlanChangesRecord record, string flatActivityDescription, BorrowerActivityResult result, bool ignoreInterestRateChange = false)
        {
            List<Error> errorLog = new List<Error>();

            if (flatActivityDescription.Contains("INTERESTRATEINEFFECT") && !ignoreInterestRateChange)
            {
                string newFirstPayDueDate = StringParsingHelper.ReadNumericFromHeaderToString(flatActivityDescription, "EFFECTIVE", new char[] { '/', ' ' });
                DateTime? val = newFirstPayDueDate.ToDateNullable();
                if (val.HasValue && !record.EffectiveDate.HasValue)
                {
                    record.EffectiveDate = val;
                }
                else if (val.HasValue && record.EffectiveDate.HasValue)
                {
                    errorLog.Add(new Error(result.BorrowerInformationId, $"Borrower Information Id: {result.BorrowerInformationId.ToString()} Borrower Activity Id: {result.BorrowerActivityId.ToString()} first pay due date exists for repayment plan change. Existing amount: {record.EffectiveDate.Value.ToString()} Additional principal: {val.ToString()};".Trim()));
                }
            }
            else if (flatActivityDescription.Contains("BORROWERSPAYMENTSTEMPORARILYCHANGEDTO"))
            {
                string newFirstPayDueDate = StringParsingHelper.ReadNumericFromHeaderToString(flatActivityDescription, "BEGINNING", new char[] { '/', ' ' });
                DateTime? val = newFirstPayDueDate.ToDateNullable();
                if (val.HasValue && !record.EffectiveDate.HasValue)
                {
                    record.EffectiveDate = val;
                }
                else if (val.HasValue && record.EffectiveDate.HasValue)
                {
                    errorLog.Add(new Error(result.BorrowerInformationId, $"Borrower Information Id: {result.BorrowerInformationId.ToString()} Borrower Activity Id: {result.BorrowerActivityId.ToString()} first pay due date exists for repayment plan change. Existing amount: {record.EffectiveDate.Value.ToString()} Additional principal: {val.ToString()};".Trim()));
                }
            }
            return errorLog;
        }

        public static RawPaymentChange ReadPaymentTable(string body)
        {
            RawPaymentChange payChange = new RawPaymentChange();
            bool skipOne = false;
            bool read = false;
            List<string> lines = body.Split('|').ToList();
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].Replace(" ", "").Contains("#PMTSPMTAMTBEGINNING"))
                {
                    if (lines.Count > i + 1 && lines[i + 1].Replace(" ", "").Contains("--"))
                    {
                        skipOne = true;
                    }
                    else
                    {
                        skipOne = false;
                        read = true;
                    }
                    continue;
                }
                if (skipOne)
                {
                    skipOne = false;
                    read = true;
                    continue;
                }
                if (read)
                {
                    List<string> splitLine = lines[i].Split(new char[] { }).Where(s => s.Trim() != "").ToList();
                    read = false;
                    if (splitLine.Count < 3)
                    {
                        payChange.EffectiveDate = "";
                        payChange.PaymentAmount = "";
                    }
                    else
                    {
                        payChange.EffectiveDate = splitLine[2].Trim();
                        payChange.PaymentAmount = splitLine[1].Trim();
                    }
                    return payChange;
                }
            }
            payChange.EffectiveDate = "";
            payChange.PaymentAmount = "";
            return payChange;
        }
    }
}
