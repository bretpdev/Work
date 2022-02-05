using DPLTRS.Payoff.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using DPLTRS.DataObjects;
using System.Globalization;
using System.Windows.Forms;
using Uheaa.Common;

namespace DPLTRS.Payoff
{
    public class Payoff : ScriptBase
    {
        LetterHelper lh;
        public new string ScriptId = "DPLTRS";
        public new ReflectionInterface RI { get; set; }
        public ProcessLogRun logRun { get; set; }
        public DPLTRS.DataAccess DA { get; set; }
        public Payoff(ReflectionInterface ri)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            DataAccessHelper.CurrentMode = DataAccessHelper.TestMode ? DataAccessHelper.Mode.Dev : DataAccessHelper.Mode.Live;
            RI = ri;
            lh = new LetterHelper(ri, ScriptId);
            logRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.CurrentMode);
            DA = new DataAccess(logRun);
        }

        public override void Main()
        {
            SystemBorrowerDemographics demos = lh.PromptForBorrower();
            if(demos != null && lh.LC05Check(demos))
            {
                if (!demos.IsValidAddress)
                {
                    Dialog.Error.Ok("Borrower does not have a valid address. Please correct the issue before sending a letter.");
                    return;
                }

                PayoffDateForm payOffDateForm = new PayoffDateForm();
                if(payOffDateForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        DateTime payOffDate = AdjustPayoffDate(payOffDateForm.PayoffDate);
                        payOffDateForm.Dispose();
                        //Need to reconstruct Balance
                        LoanData loanData = GetLC05Data();
                        CreatePayoffLetter(demos, loanData, payOffDate);
                        RI.AddCommentInLP50(demos.Ssn, "LT", "03", "DPOSL", "sent payoff letter to borrower; payoff date = " + payOffDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture), "DPLTRS");
                        Dialog.Info.Ok("The letter has been generated.", "Letter Generated");
                    }
                    catch(Exception ex)
                    {
                        Dialog.Error.Ok("The letter HAS NOT been generated.", "Error");
                        throw ex;
                    }
                }
                else
                {
                    return;
                }
            }
        }

        public DateTime AdjustPayoffDate(DateTime payOffDate)
        {
            if(payOffDate.DayOfWeek == DayOfWeek.Saturday)
            {
                payOffDate.AddDays(2);
            }
            else if(payOffDate.DayOfWeek == DayOfWeek.Sunday)
            {
                payOffDate.AddDays(1);
            }
            return payOffDate;
        }

        public LoanData GetLC05Data()
        {
            LoanData loanData = new LoanData();
            RI.PutText(21, 13, "01");
            RI.Hit(ReflectionInterface.Key.Enter);
            while (!RI.CheckForText(22, 3, "46004"))
            {
                //If there is an open loan
                if (RI.GetText(4, 10, 2) == "03" && RI.GetText(19, 73, 8) == "MMDDCCYY")
                {
                    LoanDataRow ld = new LoanDataRow();
                    ld.BalanceByLoan = double.Parse(RI.GetText(20, 32, 12));
                    loanData.Balance += ld.BalanceByLoan;
                    RI.Hit(ReflectionInterface.Key.F10);
                    RI.Hit(ReflectionInterface.Key.F10);
                    ld.UniqueId = RI.GetText(3, 13, 19);
                    loanData.loanDataRows.Add(ld);
                }

                //Look at next loan
                RI.Hit(ReflectionInterface.Key.F8);
            }
            return loanData;
        }

        public void CreatePayoffLetter(SystemBorrowerDemographics demos, LoanData loanData, DateTime payOffDate)
        {
            demos.AccountNumber = demos.AccountNumber.Replace(" ", "");
            string letterId = "PAYOFF";
            string keyLine = DocumentProcessing.ACSKeyLine(demos.Ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
            //Fileheader: "AccountNumber,KeyLine,SSN,LastName,FirstName,Address1,Address2,City,State,Country,ZIP,Balance,PayoffDate"
            string letterData = demos.AccountNumber + "," + keyLine + "," + demos.Ssn.Trim() + "," + demos.LastName.Trim() + "," + demos.FirstName.Trim() + "," + demos.Address1.Trim() + "," + demos.Address2.Trim() + "," + demos.City.Trim() + "," + demos.State.Trim() + "," + demos.Country.Trim() + "," + demos.ZipCode.Trim() + "," + string.Format("{0:0.00}", loanData.Balance) + "," + "\"" + payOffDate.ToLongDateString() + "\"";
            EcorrProcessing.AddRecordToPrintProcessing(ScriptId, letterId, letterData, demos.AccountNumber, "MA2329");
            //Add Coborrower Letters
            CreatePayOffLetterCoborrower(demos, loanData, payOffDate);
        }

        private void CreatePayOffLetterCoborrower(SystemBorrowerDemographics demos, LoanData loanData, DateTime payOffDate)
        {
            List<DPLTRS.DataObjects.CoBorrowerRecord> records = DA.GetCoBorrowersForBorrower(demos.Ssn);
            if (records != null && records.Count > 0)
            {
                var recordsBySsn = records.GroupBy(r => r.CoBorrowerSSN);
                foreach (var coborrower in recordsBySsn)
                {
                    double balance = CalculateCoborrowerBalance(demos, loanData, coborrower);
                    SendPayoffLetterCoborrower(demos, balance, coborrower.FirstOrDefault(), payOffDate);
                }
            }
        }

        private void SendPayoffLetterCoborrower(SystemBorrowerDemographics demos, double balance, CoBorrowerRecord coBorrowerRecord, DateTime payOffDate)
        {
            string letterId = "PAYOFF";
            string keyLine = DocumentProcessing.ACSKeyLine(demos.Ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
            //Fileheader: "AccountNumber,KeyLine,SSN,LastName,FirstName,Address1,Address2,City,State,Country,ZIP,Balance,PayoffDate"
            string letterData = demos.AccountNumber.Trim() + "," + keyLine + "," + demos.Ssn.Trim() + "," + coBorrowerRecord.LastName.Trim() + "," + coBorrowerRecord.FirstName.Trim() + "," + coBorrowerRecord.Address1.Trim() + "," + coBorrowerRecord.Address2.Trim() + "," + coBorrowerRecord.City.Trim() + "," + coBorrowerRecord.State.Trim() + "," + coBorrowerRecord.ForeignCountry.Trim() + "," + coBorrowerRecord.Zip.Trim() + "," + string.Format("{0:0.00}", balance) + "," + "\"" + payOffDate.ToLongDateString() + "\"";
            EcorrProcessing.AddCoBwrRecordToPrintProcessing(ScriptId, letterId, letterData, coBorrowerRecord.AccountNumber.Replace(" ", ""), "MA2329", demos.Ssn);
        }

        private double CalculateCoborrowerBalance(SystemBorrowerDemographics demos, LoanData borrowerLoanData, IGrouping<string, DPLTRS.DataObjects.CoBorrowerRecord> coborrower)
        {
            double balance = 0;
            //Get the loan detail rows that the coborrower is on
            foreach (var uniqueId in coborrower)
            {
                foreach (var r in borrowerLoanData.loanDataRows)
                {
                    if (r.UniqueId == uniqueId.UniqueId)
                    {
                        balance += r.BalanceByLoan;
                    }
                }
            }
            return balance;
        }
    }
}
