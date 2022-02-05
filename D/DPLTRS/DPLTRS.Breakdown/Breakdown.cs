using DPLTRS.Breakdown.DataObjects;
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
using Uheaa.Common;

namespace DPLTRS.Breakdown
{
    public class Breakdown : ScriptBase
    {
        LetterHelper lh;
        public new string ScriptId = "DPLTRS";
        public new ReflectionInterface RI { get; set; }
        public ProcessLogRun logRun { get; set; }
        public DPLTRS.DataAccess DA { get; set; }
        public Breakdown(ReflectionInterface ri)
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
            if (demos != null && lh.LC05Check(demos))
            {
                if (!demos.IsValidAddress)
                {
                    Dialog.Error.Ok("Borrower does not have a valid address. Please correct the issue before sending a letter.");
                    return;
                }

                try
                {
                    LoanData loanData = GetLC05Data();
                    if(loanData.HasOpenLoans)
                    {
                        int? ret = CreateBreakdownLetter(demos, loanData);
                        RI.AddCommentInLP50(demos.Ssn, "LT", "03", "DBSNT", "Balance Breakdown Letter sent to borrower", "DPLTRS");
                        if(ret.HasValue && ret.Value > 0)
                        {
                            Dialog.Info.Ok("The letter has been generated.", "Letter Generated");
                        }
                        else
                        {
                            Dialog.Info.Ok("The letter has already been generated today, No new letter created.", "Letter Already Exists");
                        }
                    }
                    else
                    {
                        Dialog.Info.Ok("The borrower has no open loans.", "No Open Loans");
                    }
                }
                catch (Exception e)
                {
                    Dialog.Error.Ok("The letter HAS NOT been generated.", "ERROR");
                    throw e;
                }
            }
        }

        private int? CreateBreakdownLetter(SystemBorrowerDemographics demos, LoanData loanData)
        {
            demos.AccountNumber = demos.AccountNumber.Replace(" ", "");
            string letterId = "BRKDWN";
            string keyLine = DocumentProcessing.ACSKeyLine(demos.Ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
            //Fileheader: "AccountNumber,KeyLine,SSN,LastName,FirstName,Address1,Address2,City,State,Country,ZIP,TotalAcc,TotalCol,Balance,TotalDef,PrincCur,PrincCol,IntAcc,IntCol,IntCur,LegalAcc,LegalCol,LegalCur,OtherAcc,OtherCol,OtherCur,CCAcc,CCCol,CCCur,CCProj"
            string letterData = demos.AccountNumber.Trim() + "," + keyLine + "," + demos.Ssn.Trim() + "," + demos.LastName.Trim() + "," + demos.FirstName.Trim() + "," + demos.Address1.Trim() + "," + demos.Address2.Trim() + "," + demos.City.Trim() + "," + demos.State.Trim() + "," + demos.Country.Trim() + "," + demos.ZipCode.Trim() + "," + string.Format("{0:0.00}", loanData.TotalAcc) + "," + string.Format("{0:0.00}", loanData.TotalCol) + "," + string.Format("{0:0.00}", loanData.Balance) + "," + string.Format("{0:0.00}", loanData.TotalDef) + "," + string.Format("{0:0.00}", loanData.PrincCur) + "," + string.Format("{0:0.00}", loanData.PrincCol) + "," + string.Format("{0:0.00}", loanData.IntAcc) + "," + string.Format("{0:0.00}", loanData.IntCol) + "," + string.Format("{0:0.00}", loanData.IntCur) + "," + string.Format("{0:0.00}", loanData.LegalAcc) + "," + string.Format("{0:0.00}", loanData.LegalCol) + "," + string.Format("{0:0.00}", loanData.LegalCur) + "," + string.Format("{0:0.00}", loanData.OtherAcc) + "," + string.Format("{0:0.00}", loanData.OtherCol) + "," + string.Format("{0:0.00}", loanData.OtherCur) + "," + string.Format("{0:0.00}", loanData.CCAcc) + "," + string.Format("{0:0.00}", loanData.CCCol) + "," + string.Format("{0:0.00}", loanData.CCCur) + "," + string.Format("{0:0.00}", loanData.CCProj);
            int? ret = EcorrProcessing.AddRecordToPrintProcessing(ScriptId, letterId, letterData, demos.AccountNumber, "MA2329");
            //Add Coborrower Letters
            CreatePayOffLetterCoborrower(demos, loanData);
            return ret;
        }

        private void CreatePayOffLetterCoborrower(SystemBorrowerDemographics demos, LoanData loanData)
        {
            List<DPLTRS.DataObjects.CoBorrowerRecord> records = DA.GetCoBorrowersForBorrower(demos.Ssn);
            if (records != null && records.Count > 0)
            {
                var recordsBySsn = records.GroupBy(r => r.CoBorrowerSSN);
                foreach (var coborrower in recordsBySsn)
                {
                    LoanData coborrowerLoanData = CalculateCoborrowerBalance(demos, loanData, coborrower);
                    SendBreakdownLetterCoborrower(demos, coborrowerLoanData, coborrower.FirstOrDefault());
                }
            }
        }

        private void SendBreakdownLetterCoborrower(SystemBorrowerDemographics demos, LoanData coborrowerLoanData, CoBorrowerRecord coBorrowerRecord)
        {
            string letterId = "BRKDWN";
            string keyLine = DocumentProcessing.ACSKeyLine(demos.Ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
            //Fileheader: "AccountNumber,KeyLine,SSN,LastName,FirstName,Address1,Address2,City,State,Country,ZIP,TotalAcc,TotalCol,Balance,TotalDef,PrincCur,PrincCol,IntAcc,IntCol,IntCur,LegalAcc,LegalCol,LegalCur,OtherAcc,OtherCol,OtherCur,CCAcc,CCCol,CCCur,CCProj"
            string letterData = demos.AccountNumber.Replace(" ", "") + "," + keyLine + "," + demos.Ssn.Trim() + "," + coBorrowerRecord.LastName.Trim() + "," + coBorrowerRecord.FirstName.Trim() + "," + coBorrowerRecord.Address1.Trim() + "," + coBorrowerRecord.Address2.Trim() + "," + coBorrowerRecord.City.Trim() + "," + coBorrowerRecord.State.Trim() + "," + coBorrowerRecord.ForeignCountry.Trim() + "," + coBorrowerRecord.Zip.Trim() + "," + string.Format("{0:0.00}", coborrowerLoanData.TotalAcc) + "," + string.Format("{0:0.00}", coborrowerLoanData.TotalCol) + "," + string.Format("{0:0.00}", coborrowerLoanData.Balance) + "," + string.Format("{0:0.00}", coborrowerLoanData.TotalDef) + "," + string.Format("{0:0.00}", coborrowerLoanData.PrincCur) + "," + string.Format("{0:0.00}", coborrowerLoanData.PrincCol) + "," + string.Format("{0:0.00}", coborrowerLoanData.IntAcc) + "," + string.Format("{0:0.00}", coborrowerLoanData.IntCol) + "," + string.Format("{0:0.00}", coborrowerLoanData.IntCur) + "," + string.Format("{0:0.00}", coborrowerLoanData.LegalAcc) + "," + string.Format("{0:0.00}", coborrowerLoanData.LegalCol) + "," + string.Format("{0:0.00}", coborrowerLoanData.LegalCur) + "," + string.Format("{0:0.00}", coborrowerLoanData.OtherAcc) + "," + string.Format("{0:0.00}", coborrowerLoanData.OtherCol) + "," + string.Format("{0:0.00}", coborrowerLoanData.OtherCur) + "," + string.Format("{0:0.00}", coborrowerLoanData.CCAcc) + "," + string.Format("{0:0.00}", coborrowerLoanData.CCCol) + "," + string.Format("{0:0.00}", coborrowerLoanData.CCCur) + "," + string.Format("{0:0.00}", coborrowerLoanData.CCProj);
            EcorrProcessing.AddCoBwrRecordToPrintProcessing(ScriptId, letterId, letterData, coBorrowerRecord.AccountNumber.Replace(" ", ""), "MA2329", demos.Ssn);
        }

        private LoanData CalculateCoborrowerBalance(SystemBorrowerDemographics demos, LoanData borrowerLoanData, IGrouping<string, CoBorrowerRecord> coborrower)
        {
            LoanData loanData = new LoanData();
            //Get the loan detail rows that the coborrower is on
            foreach (var uniqueId in coborrower)
            {
                foreach (var r in borrowerLoanData.loanDataRows)
                {
                    if (r.UniqueId == uniqueId.UniqueId)
                    {
                        loanData.Balance += r.Balance;
                        loanData.LegalAcc += r.LegalAcc;
                        loanData.OtherAcc += r.OtherAcc;
                        loanData.CCAcc += r.CCAcc;
                        loanData.CCProj += r.CCProj;
                        loanData.PrincCol += r.PrincCol;
                        loanData.IntCol += r.IntCol;
                        loanData.LegalCol += r.LegalCol;
                        loanData.OtherCol += r.OtherCol;
                        loanData.CCCol += r.CCCol;
                        loanData.TotalDef += r.TotalDef;
                        loanData.IntAcc += r.IntAcc;
                    }
                }
            }
            loanData.PrincCur = loanData.TotalDef - loanData.PrincCol;
            loanData.IntCur = loanData.IntAcc - loanData.IntCol;
            loanData.LegalCur = loanData.LegalAcc - loanData.LegalCol;
            loanData.OtherCur = loanData.OtherAcc - loanData.OtherCol;
            loanData.CCCur = loanData.CCAcc - loanData.CCCol;
            loanData.TotalAcc = loanData.TotalDef + loanData.IntAcc + loanData.LegalAcc + loanData.OtherAcc + loanData.CCAcc + loanData.CCProj;
            loanData.TotalCol = loanData.PrincCol + loanData.IntCol + loanData.LegalCol + loanData.OtherCol + loanData.CCCol;
            return loanData;
        }

        public LoanData GetLC05DataRow(LoanData loanData)
        {
            LoanDataRow ld = new LoanDataRow();
            ld.Balance = double.Parse(RI.GetText(20, 32, 12));
            ld.LegalAcc = double.Parse(RI.GetText(13, 34, 10));
            ld.OtherAcc = double.Parse(RI.GetText(15, 34, 10));
            ld.CCAcc = double.Parse(RI.GetText(17, 34, 10));
            ld.CCProj = double.Parse(RI.GetText(19, 34, 10));
            ld.PrincCol = double.Parse(RI.GetText(10, 34, 10));
            ld.IntCol = double.Parse(RI.GetText(12, 34, 10));
            ld.IntAcc = double.Parse(RI.GetText(11, 34, 10));
            ld.LegalCol = double.Parse(RI.GetText(14, 34, 10));
            ld.OtherCol = double.Parse(RI.GetText(16, 34, 10));
            ld.CCCol = double.Parse(RI.GetText(18, 34, 10));
            ld.TotalDef = double.Parse(RI.GetText(9, 34, 10));
            RI.Hit(ReflectionInterface.Key.F10);
            RI.Hit(ReflectionInterface.Key.F10);
            ld.UniqueId = RI.GetText(3, 13, 19);
            loanData.loanDataRows.Add(ld);

            loanData.Balance += ld.Balance;
            loanData.LegalAcc += ld.LegalAcc;
            loanData.OtherAcc += ld.OtherAcc;
            loanData.CCAcc += ld.CCAcc;
            loanData.CCProj += ld.CCProj;
            loanData.PrincCol += ld.PrincCol;
            loanData.IntCol += ld.IntCol;
            loanData.LegalCol += ld.LegalCol;
            loanData.OtherCol += ld.OtherCol;
            loanData.CCCol += ld.CCCol;
            loanData.TotalDef += ld.TotalDef;
            loanData.IntAcc += ld.IntAcc;
            loanData.HasOpenLoans = true;
            return loanData;
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
                    loanData = GetLC05DataRow(loanData);
                }

                //Look at next loan
                RI.Hit(ReflectionInterface.Key.F8);
            }
            loanData.PrincCur = loanData.TotalDef - loanData.PrincCol;
            loanData.IntCur = loanData.IntAcc - loanData.IntCol;
            loanData.LegalCur = loanData.LegalAcc - loanData.LegalCol;
            loanData.OtherCur = loanData.OtherAcc - loanData.OtherCol;
            loanData.CCCur = loanData.CCAcc - loanData.CCCol;
            loanData.TotalAcc = loanData.TotalDef + loanData.IntAcc + loanData.LegalAcc + loanData.OtherAcc + loanData.CCAcc + loanData.CCProj;
            loanData.TotalCol = loanData.PrincCol + loanData.IntCol + loanData.LegalCol + loanData.OtherCol + loanData.CCCol;
            return loanData;
        }
    }
}
