using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;


namespace PYOFFLTRFD
{
    public class PayOffLetterFed : ScriptBase
    {
        PayoffInformation Info { get; set; }
        public BorrowerData BorrowerData { get; set; }
        private ProcessLogRun LogRun { get; set; }
        public string LetterId
        {
            get
            {
                return "PAYOFFFED";
            }
        }

        /// <summary>
        /// Use this when launching from Maui DUDE
        /// </summary>
        public PayOffLetterFed(ReflectionInterface ri, BorrowerData data)
            : this(ri)
        {
            BorrowerData = data;
        }

        /// <summary>
        /// Use this when launching from a session
        /// </summary>
        public PayOffLetterFed(ReflectionInterface ri)
            : base(ri, "PYOFFLTRFD", DataAccessHelper.Region.CornerStone)
        {
            BorrowerData = new BorrowerData();
            BorrowerData.PData = new List<PayoffData>();
        }

        public override void Main()
        {
            LogRun = RI.LogRun ?? new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);

            string msg = DatabaseAccessHelper.GenerateSprocAccessAlert(Assembly.GetExecutingAssembly());
            if (msg != null)
            {
                Dialog.Error.Ok(msg, "Stored Procedure Error");
                return;
            }

            Info = new PayoffInformation(BorrowerData, RI, LogRun);
            if (Info.ShowDialog() == DialogResult.OK)
            {
                CreatePrintProcessingRecord();
                AddArcs();

                Dialog.Info.Ok("Process Complete", "Process Complete");
            }
            LogRun.LogEnd();
        }

        /// <summary>
        /// Creates data strings to be inserted into the print processing tables.
        /// </summary>
        private void CreatePrintProcessingRecord()
        {
            string keyline = DocumentProcessing.ACSKeyLine(BorrowerData.Demos.Ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
            string letterData = $"{keyline},{BorrowerData.Demos.AccountNumber},{BorrowerData.Demos.FirstName},{BorrowerData.Demos.LastName},{BorrowerData.Demos.Address1},{BorrowerData.Demos.Address2}"
                + $",{BorrowerData.Demos.City},{BorrowerData.Demos.State},{BorrowerData.Demos.ZipCode},{BorrowerData.Demos.ForeignState},{BorrowerData.Demos.Country},{BorrowerData.PayoffDate}";
            for (int i = 0; i <= 29; i++)
            {
                if (BorrowerData.PData.Count > i)
                    letterData += $",{BorrowerData.PData[i].LoanProgram},{BorrowerData.PData[i].DateDisbursed},\"{BorrowerData.PData[i].DailyInterest:C}\",\"{BorrowerData.PData[i].CurrentPrincipal:C}\""
                        + $",\"{BorrowerData.PData[i].PayoffInterest:C}\",\"{BorrowerData.PData[i].PayoffAmount:C}\"";
                else
                    letterData += $",,,,,,";
            }
            EcorrProcessing.AddRecordToPrintProcessing(ScriptId, LetterId, letterData, BorrowerData.Demos.AccountNumber, "MA4481", DataAccessHelper.Region.CornerStone);
        }

        /// <summary>
        /// Adds a PAYOF arc to the borrower account.
        /// </summary>
        private void AddArcs()
        {
            List<int> loanSeqs = BorrowerData.PData.Select(p => p.SequenceNumber).ToList();
            decimal payoffAmount = BorrowerData.PData.Sum(p => p.PayoffAmount);
            string comment = $"Payoff letter generated for Loan Seqs: {string.Join(",", loanSeqs)}; Payoff Date: {BorrowerData.PayoffDate}; Payoff Amount: {payoffAmount:C}";
            ArcData arc = new ArcData(DataAccessHelper.Region.CornerStone)
            {
                AccountNumber = BorrowerData.Demos.AccountNumber,
                Arc = "PAYOF",
                ArcTypeSelected = ArcData.ArcType.Atd22ByLoan,
                Comment = comment,
                LoanSequences = loanSeqs,
                ScriptId = "PYOFFLTRFD"
            };

            ArcAddResults result = arc.AddArc();
            if (!result.ArcAdded)
            {
                string message = $"Error adding PAYOF arc for Account {BorrowerData.Demos.AccountNumber}, Loan Sequences: {string.Join(",", loanSeqs)}, Payoff Date: {BorrowerData.PayoffDate}, Payoff Amount: {payoffAmount}";
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning, result.Ex);
            }
        }
    }
}