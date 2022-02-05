using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace PRIDRCRP
{
    public sealed class PaymentHistoryInformation : FileInformation
    {
        public override FileParser.FileSection Section { get; protected set; } = FileParser.FileSection.Section_III;
        public override List<Error> ExceptionLog { get; protected set; } = new List<Error>();

        public List<PaymentHistoryRecord> paymentRecords { get; private set; } = new List<PaymentHistoryRecord>();
        public PaymentHistoryRecord paymentTotals { get; private set; }
        private ProcessLogRun logRun;

        private bool foundHeaderRow = false;
        private bool foundTotalRow = false;
        private bool skipNextRow = false;
        private bool done = false;
        private int rowsToSkip = 1;
        private int row = 0;

        public PaymentHistoryInformation(ProcessLogRun logRun)
        {
            this.logRun = logRun;
        }

        public override bool FoundInformation()
        {
            return done;
        }

        public override void GetInformation(string line)
        {
            CheckParsingLocation(line);

            if(row > rowsToSkip && !done)
            {
                if(line.Replace(" ", "").Contains("BORROWERHISTORYANDACTIVITYREPORT"))
                {
                    skipNextRow = true;
                    return;
                }
                if(skipNextRow == true)
                {
                    skipNextRow = false;
                    return;
                }
                if(foundTotalRow)
                {
                    //Handle Total
                    paymentTotals = GetTotalsFromLine(line);
                    done = true;
                    return;
                }
                if (line.Trim() != "0" && line.Trim() != "")
                {
                    paymentRecords.Add(GetPaymentHistoryFromLine(line));
                }
            }
        }

        private PaymentHistoryRecord GetPaymentHistoryFromLine(string line)
        {
            //ToDecimal(null) = 0
            PaymentHistoryRecord paymentHistoryRecord = new PaymentHistoryRecord();
            paymentHistoryRecord.Description = StringParsingHelper.SafeSubStringTrimmed(line, 10, 18);
            paymentHistoryRecord.ActionDate = StringParsingHelper.SafeSubStringTrimmed(line, 28, 8).Trim() != "" ? (DateTime?)Convert.ToDateTime(StringParsingHelper.SafeSubStringTrimmed(line, 28, 8)) : null;
            paymentHistoryRecord.EffectiveDate = StringParsingHelper.SafeSubStringTrimmed(line, 38, 8).Trim() != "" ? (DateTime?)Convert.ToDateTime(StringParsingHelper.SafeSubStringTrimmed(line, 38, 8)) : null;
            paymentHistoryRecord.TotalPaid = StringParsingHelper.SafeSubStringTrimmed(line,46, 12).Trim() == "" ? null : (decimal?)Convert.ToDecimal(StringParsingHelper.SafeSubStringTrimmed(line, 46, 12));
            paymentHistoryRecord.InterestPaid = StringParsingHelper.SafeSubStringTrimmed(line, 58, 12).Trim() == "" ? null : (decimal?)Convert.ToDecimal(StringParsingHelper.SafeSubStringTrimmed(line, 58, 12));
            paymentHistoryRecord.PrincipalPaid = StringParsingHelper.SafeSubStringTrimmed(line, 70, 12).Trim() == "" ? null : (decimal?)Convert.ToDecimal(StringParsingHelper.SafeSubStringTrimmed(line, 70, 12));
            paymentHistoryRecord.LcNsfPaid = StringParsingHelper.SafeSubStringTrimmed(line,82, 12).Trim() == "" ? null : (decimal?)Convert.ToDecimal(StringParsingHelper.SafeSubStringTrimmed(line, 82, 12));
            paymentHistoryRecord.AccruedInterest = StringParsingHelper.SafeSubStringTrimmed(line, 94, 12).Trim() == "" ? null : (decimal?)Convert.ToDecimal(StringParsingHelper.SafeSubStringTrimmed(line, 94, 12));
            paymentHistoryRecord.LcNsfDue = StringParsingHelper.SafeSubStringTrimmed(line, 106, 12).Trim() == "" ? null : (decimal?)Convert.ToDecimal(StringParsingHelper.SafeSubStringTrimmed(line, 106, 12));
            paymentHistoryRecord.PrincipalBalance = StringParsingHelper.SafeSubStringTrimmed(line, 118, 12).Trim() == "" ? null : (decimal?)Convert.ToDecimal(StringParsingHelper.SafeSubStringTrimmed(line, 118, 12));
            return paymentHistoryRecord;
        }

        private PaymentHistoryRecord GetTotalsFromLine(string line)
        {
            PaymentHistoryRecord paymentHistoryRecord = new PaymentHistoryRecord();
            paymentHistoryRecord.TotalPaid = StringParsingHelper.SafeSubStringTrimmed(line,46, 12) == "" ? null : (decimal?)Convert.ToDecimal(StringParsingHelper.SafeSubStringTrimmed(line, 46, 12));
            paymentHistoryRecord.InterestPaid = StringParsingHelper.SafeSubStringTrimmed(line,58, 12) == "" ? null : (decimal?)Convert.ToDecimal(StringParsingHelper.SafeSubStringTrimmed(line, 58, 12));
            paymentHistoryRecord.PrincipalPaid = StringParsingHelper.SafeSubStringTrimmed(line,70, 12) == "" ? null : (decimal?)Convert.ToDecimal(StringParsingHelper.SafeSubStringTrimmed(line, 70, 12));
            paymentHistoryRecord.LcNsfPaid = StringParsingHelper.SafeSubStringTrimmed(line,82, 12) == "" ? null : (decimal?)Convert.ToDecimal(StringParsingHelper.SafeSubStringTrimmed(line, 82, 12));
            paymentHistoryRecord.AccruedInterest = StringParsingHelper.SafeSubStringTrimmed(line,94, 12) == "" ? null : (decimal?)Convert.ToDecimal(StringParsingHelper.SafeSubStringTrimmed(line, 94, 12));
            paymentHistoryRecord.LcNsfDue = StringParsingHelper.SafeSubStringTrimmed(line,106, 12) == "" ? null : (decimal?)Convert.ToDecimal(StringParsingHelper.SafeSubStringTrimmed(line, 106, 12));
            paymentHistoryRecord.PrincipalBalance = StringParsingHelper.SafeSubStringTrimmed(line,118, 12) == "" ? null : (decimal?)Convert.ToDecimal(StringParsingHelper.SafeSubStringTrimmed(line, 118, 12));
            return paymentHistoryRecord;
        }

        private void CheckParsingLocation(string line)
        {
            if(!done)
            {
                if (foundHeaderRow)
                {
                    row++;
                }

                if (line.Replace(" ", "").Contains("DESCRIPTIONACTDATEEFFDATETOTPAIDINTPAIDPRINPAIDLC/NSFPDACCRDINTLC/NSFDUEPRINBAL"))
                {
                    foundHeaderRow = true;
                }

                if (line.Contains("TOTALS"))
                {
                    foundTotalRow = true;
                }
            }
        }

        public override bool ValidateInformation(string file, DataAccess DA)
        {
            foreach(PaymentHistoryRecord record in paymentRecords)
            {
                if(record == null || record.Description == null)
                {
                    DA.ThrowDataValidationError("Unable to add Payment History Records. File: " + file);
                }
            }
            return true;
        }

        public override bool WriteToDatabase(DataAccess DA)
        {
            return DA.InsertToPaymentHistory(paymentRecords);
        }
    }
}
