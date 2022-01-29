using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace PAYOFF
{
    public class PayoffProcessor : ScriptBase
    {
        public ProcessLogRun LogRun { get; set; }
        public Borrower Borrower { get; set; }
        public List<Loan> LoanOptions { get; set; }
        public string LetterId 
        {
            get { return "PAYOFFUH"; } //This is technically not the same conceptually as the script id, so not duplication
        }

        public PayoffProcessor(ReflectionInterface ri) : base(ri, "PAYOFF", DataAccessHelper.Region.Uheaa)
        {
            LogRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            Borrower = new Borrower();
            Borrower.LoanPayoffRecords = new List<LoanPayoffData>();
        }

        public PayoffProcessor(ReflectionInterface ri, Borrower borrower) : base(ri, "PAYOFF", DataAccessHelper.Region.Uheaa)
        {
            LogRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            Borrower = borrower;
            //Borrower = new Borrower();
            //Borrower.LoanPayoffRecords = new List<LoanPayoffData>();
        }

        public override void Main()
        {
            if(!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), true))
            {
                return;
            }

            var payoffInfo = new PayoffInformation(Borrower, RI, LogRun);
            if(payoffInfo.ShowDialog() == DialogResult.OK)
            {
                LoanOptions = payoffInfo.LoanOptions;
                CreatePrintProcessingRecord();
                AddArcs();

                ProcessCoborrowers(payoffInfo.DA);

                Dialog.Info.Ok("Process Complete", "Process Complete");
            }
            LogRun.LogEnd();
        }

        private void ProcessCoborrowers(DataAccess DA)
        {
            List<CoborrowerLoan> coborrowerLoans = DA.GetCoborrowerLoanInformation(Borrower.Demos.AccountNumber);
            var groupedLoans = coborrowerLoans.GroupBy(p => p.CoborrowerSSN);
            //foreach coborrower
            foreach(var loans in groupedLoans)
            {
                var selectedLoans = Borrower.LoanPayoffRecords.Select(p => p.SequenceNumber);
                var coborrowerSelectedLoans = new List<int>();
                foreach (var loan in loans)
                {
                    if(selectedLoans.Contains(loan.LoanSequence))
                    {
                        coborrowerSelectedLoans.Add(loan.LoanSequence);
                    }
                }

                //if the coborrower is on any of the selected loans add a coborrower letter
                if(coborrowerSelectedLoans.Count > 0)
                {
                    var coborrowerDemos = DA.GetDemos(loans.Key);
                    List<LoanPayoffData> coborrowerLoanPayoffData = new List<LoanPayoffData>();
                    foreach(int loan in coborrowerSelectedLoans)
                    {
                        var results = Borrower.LoanPayoffRecords.Where(p => p.SequenceNumber == loan);
                        if(results.Any())
                        {
                            coborrowerLoanPayoffData.AddRange(results);
                        }
                    }
                    CreateCoborrowerPrintProcessingRecord(coborrowerDemos, coborrowerLoanPayoffData);
                }
            }
        }

        /// <summary>
        /// Creates data strings to be inserted into the print processing tables.
        /// </summary>
        private void CreateCoborrowerPrintProcessingRecord(SystemBorrowerDemographics coborrowerDemos, List<LoanPayoffData> coborrowerLoans)
        {
            string keyline = DocumentProcessing.ACSKeyLine(Borrower.Demos.Ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
            string letterData = $"{keyline},{coborrowerDemos.AccountNumber},\"{coborrowerDemos.FirstName} {coborrowerDemos.LastName}\",\"{coborrowerDemos.Address1}\",\"{coborrowerDemos.Address2}\""
                + $",\"{coborrowerDemos.City}\",\"{coborrowerDemos.State}\",{coborrowerDemos.ZipCode},\"{coborrowerDemos.Country}\",{Borrower.PayoffDate}";
            for (int i = 0; i <= 29; i++)
            {
                if (coborrowerLoans.Count > i)
                {
                    var matchingLoanOption = LoanOptions.Where(p => p.LoanSequence == Borrower.LoanPayoffRecords[i].SequenceNumber).FirstOrDefault();
                    var fullLoanProgram = matchingLoanOption != null ? matchingLoanOption.LoanProgram : Borrower.LoanPayoffRecords[i].LoanProgram;
                    letterData += $",{fullLoanProgram},{coborrowerLoans[i].DateDisbursed},\"{coborrowerLoans[i].DailyInterest:C}\",\"{coborrowerLoans[i].CurrentPrincipal:C}\""
                            + $",\"{coborrowerLoans[i].PayoffInterest:C}\",\"{coborrowerLoans[i].PayoffAmount:C}\"";
                }
                else
                    letterData += $",,,,,,";
            }
            EcorrProcessing.AddCoBwrRecordToPrintProcessing(ScriptId, LetterId, letterData, coborrowerDemos.AccountNumber, "MA2329", Borrower.Demos.Ssn, DataAccessHelper.Region.Uheaa);
        }

        /// <summary>
        /// Creates data strings to be inserted into the print processing tables.
        /// </summary>
        private void CreatePrintProcessingRecord()
        {
            string keyline = DocumentProcessing.ACSKeyLine(Borrower.Demos.Ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
            string letterData = $"{keyline},{Borrower.Demos.AccountNumber},\"{Borrower.Demos.FirstName} {Borrower.Demos.LastName}\",\"{Borrower.Demos.Address1}\",\"{Borrower.Demos.Address2}\""
                + $",\"{Borrower.Demos.City}\",\"{Borrower.Demos.State}\",{Borrower.Demos.ZipCode},\"{Borrower.Demos.Country}\",{Borrower.PayoffDate}";
            for (int i = 0; i <= 29; i++)
            {
                if (Borrower.LoanPayoffRecords.Count > i)
                {
                    var matchingLoanOption = LoanOptions.Where(p => p.LoanSequence == Borrower.LoanPayoffRecords[i].SequenceNumber).FirstOrDefault();
                    var fullLoanProgram = matchingLoanOption != null ? matchingLoanOption.LoanProgram : Borrower.LoanPayoffRecords[i].LoanProgram;
                    letterData += $",{fullLoanProgram},{Borrower.LoanPayoffRecords[i].DateDisbursed},\"{Borrower.LoanPayoffRecords[i].DailyInterest:C}\",\"{Borrower.LoanPayoffRecords[i].CurrentPrincipal:C}\""
                        + $",\"{Borrower.LoanPayoffRecords[i].PayoffInterest:C}\",\"{Borrower.LoanPayoffRecords[i].PayoffAmount:C}\"";
                }
                else
                    letterData += $",,,,,,";
            }
            EcorrProcessing.AddRecordToPrintProcessing(ScriptId, LetterId, letterData, Borrower.Demos.AccountNumber, "MA2329", DataAccessHelper.Region.Uheaa);
        }

        /// <summary>
        /// Adds a PAYOF arc to the borrower account.
        /// </summary>
        private void AddArcs()
        {
            List<int> loanSeqs = Borrower.LoanPayoffRecords.Select(p => p.SequenceNumber).ToList();
            decimal payoffAmount = Borrower.LoanPayoffRecords.Sum(p => p.PayoffAmount);
            string comment = $"Payoff letter generated for Loan Seqs: {string.Join(",", loanSeqs)}; Payoff Date: {Borrower.PayoffDate}; Payoff Amount: {payoffAmount:C}";
            ArcData arc = new ArcData(DataAccessHelper.Region.Uheaa)
            {
                AccountNumber = Borrower.Demos.AccountNumber,
                Arc = "PAYOF", //TODO CONFIRM THE ARC
                ArcTypeSelected = ArcData.ArcType.Atd22ByLoan,
                Comment = comment,
                LoanSequences = loanSeqs,
                ScriptId = ScriptId
            };

            ArcAddResults result = arc.AddArc();
            if (!result.ArcAdded)
            {
                string message = $"Error adding PAYOF arc for Account {Borrower.Demos.AccountNumber}, Loan Sequences: {string.Join(",", loanSeqs)}, Payoff Date: {Borrower.PayoffDate}, Payoff Amount: {payoffAmount}";
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning, result.Ex);
            }
        }
    }
}
