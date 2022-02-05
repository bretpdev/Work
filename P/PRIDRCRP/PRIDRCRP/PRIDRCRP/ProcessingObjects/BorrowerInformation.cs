using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace PRIDRCRP
{
    public sealed class BorrowerInformation : FileInformation
    {
        public BorrowerRecord borrowerRecord { get; private set; } = new BorrowerRecord();

        private ProcessLogRun logRun;
        public override FileParser.FileSection Section { get; protected set; } = FileParser.FileSection.Section_I;

        public override List<Error> ExceptionLog { get; protected set; } = new List<Error>();

        private readonly List<string> ELPlans = new List<string>() { "CONSOLSTANDARD", "EXTENDEDFIXED", "EXTENDED-GRANDFATHERED", "CONSOL-STANDARD" };

        public BorrowerInformation(ProcessLogRun logRun)
        {
            this.logRun = logRun;
        }

        public override bool FoundInformation()
        {
            return borrowerRecord.Ssn != null && borrowerRecord.InterestRate.HasValue && borrowerRecord.FirstPayDue.HasValue && borrowerRecord.PaymentAmount.HasValue && borrowerRecord.RepayPlan != null;
        }

        public override void GetInformation(string line)
        {
            //Get informaton if it is available on the current line
            if (borrowerRecord.Ssn == null)
            {
                string ssnAndPage = ParseField(line, "ACCOUNT NO.", 56, new List<string> { "-" });
                if (ssnAndPage != null && ssnAndPage.Length != 10)
                {
                    logRun.AddNotification("Unable To Determine Ssn from Section I. Received: " + ssnAndPage, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                }
                else if (ssnAndPage != null)
                {
                    borrowerRecord.Ssn = ssnAndPage.Substring(0, ssnAndPage.Length - 1);
                    borrowerRecord.Page = ssnAndPage.Last().ToString();
                }
            }
            if (!borrowerRecord.InterestRate.HasValue)
            {
                borrowerRecord.InterestRate = ParseField(line, "INTEREST RATE", 13, new List<string> { "%" }) == null ? null : (decimal?)Convert.ToDecimal(ParseField(line, "INTEREST RATE", 13, new List<string> { "%" }));
            }
            if (!borrowerRecord.FirstPayDue.HasValue)
            {
                borrowerRecord.FirstPayDue = ParseField(line, "FIRST PAY DUE", 13) == null ? null : (DateTime?)Convert.ToDateTime(ParseField(line, "FIRST PAY DUE", 13));
            }
            if (!borrowerRecord.PaymentAmount.HasValue)
            {
                borrowerRecord.PaymentAmount = ParseField(line, "PAYMENT AMOUNT", 21, new List<string> { "$" }) == null ? null : (decimal?)Convert.ToDecimal(ParseField(line, "PAYMENT AMOUNT", 21, new List<string> { "$" }));
            }
            if (borrowerRecord.RepayPlan == null)
            {
                borrowerRecord.RepayPlan = ParseField(line, "REPAY PLAN", 25);
                if (borrowerRecord.RepayPlan != null && ELPlans.Contains(borrowerRecord.RepayPlan.Replace(" ", "")))
                {
                    ExceptionLog.Add(new Error(null, $" Borrower Ssn {(borrowerRecord.Ssn ?? "")}, {borrowerRecord.RepayPlan} as first repayment plan. EL Plan record needs manual review;".Trim()));
                }
            }
        }

        public override bool ValidateInformation(string file, DataAccess DA)
        {
            int intSsn = (borrowerRecord.Ssn ?? "0").ToInt();
            bool ssnInvalid = (borrowerRecord.Ssn == null) || (borrowerRecord.Ssn.Trim().Length != 9) || intSsn == 0;
            if (borrowerRecord == null || ssnInvalid || !borrowerRecord.FirstPayDue.HasValue || !borrowerRecord.InterestRate.HasValue || !borrowerRecord.PaymentAmount.HasValue || borrowerRecord.RepayPlan == null)
            {
                DA.ThrowDataValidationError("Unable to add First Repayment Plan Information.");
            }
            return true;
        }

        public override bool WriteToDatabase(DataAccess DA)
        {
            int? borrowerInformationId = DA.InsertToBorrowerInformation(borrowerRecord.Ssn, borrowerRecord.InterestRate.Value, borrowerRecord.FirstPayDue.Value, borrowerRecord.PaymentAmount.Value, borrowerRecord.RepayPlan, borrowerRecord.Page);

            //We're assigning a borrower information id to the errors that were previously created when parsign the borrower information
            if (borrowerInformationId.HasValue)
            {
                foreach (Error error in ExceptionLog)
                {
                    if (!error.BorrowerInformationId.HasValue)
                    {
                        error.BorrowerInformationId = borrowerInformationId;
                    }

                }
            }

            return borrowerInformationId.HasValue;
        }
    }
}
