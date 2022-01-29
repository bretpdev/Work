using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using DPLTRS.Rehabilitation.DataObjects;
using Microsoft.VisualBasic;
using Uheaa.Common.DocumentProcessing;
using static Uheaa.Common.Scripts.ReflectionInterface;
using DPLTRS.DataObjects;

namespace DPLTRS.Rehabilitation
{
    public class LoanManagementLetters_Rehabilitation : ScriptBase
    {
        LetterHelper lh;
        public new string ScriptId = "DPLTRS";
        public new ReflectionInterface RI { get; set; }
        public ProcessLogRun logRun { get; set; }
        public DPLTRS.DataAccess DA { get; set; }

        public LoanManagementLetters_Rehabilitation(ReflectionInterface ri)
            : base()
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
            if (demos != null)
            {
                if (!demos.IsValidAddress)
                {
                    Dialog.Error.Ok("Borrower does not have a valid address. Please correct the issue before sending a letter.");
                    return;
                }

                LoanData loanData = GetLC10Data(demos);
                if(lh.LC05Check(demos))
                {
                    try
                    { 
                        loanData = GetLC05Data(demos, loanData);
                        loanData.RHPay = Financial.Pmt(loanData.ARate / 100 / 12, 111, -1 * loanData.ABal, 2);
                        loanData = TotalLoanDataRows(loanData);
                        CreateSLCT2Letter(demos, loanData);
                        RI.AddCommentInLP50(demos.Ssn, "LT", "03", "DLRS2", "Qualify For Loan Rehabilitation letter sent to borrower", "DPLTRS");
                        Dialog.Info.Ok("The letter has been generated.", "Letter Generated");
                    }
                    catch (Exception ex)
                    {
                        Dialog.Error.Ok("The letter HAS NOT been generated.", "Error");
                        throw ex;
                    }
                }
            }
        }

        public string GetFormattedDueDay(string dueDay)
        {
            switch (dueDay)
            {
                case "01":
                    return "1st";
                case "07":
                    return "7th";
                case "15":
                    return "15th";
                case "22":
                    return "22nd";
                default:
                    return null;
            }
        }

        public LoanData TotalLoanDataRows(LoanData loanData)
        {
            loanData.loanDataTotals.Pri = 0;
            loanData.loanDataTotals.Int = 0;
            loanData.loanDataTotals.Col = 0;
            loanData.loanDataTotals.Fee = 0;
            loanData.loanDataTotals.Tot = 0;
            foreach (LoanDataRow ldr in loanData.loanDataRows)
            {
                loanData.loanDataTotals.Pri += ldr.Pri;
                loanData.loanDataTotals.Int += ldr.Int;
                loanData.loanDataTotals.Col += ldr.Col;
                loanData.loanDataTotals.Fee += ldr.Fee;
                loanData.loanDataTotals.Tot += ldr.Tot;
            }
            return loanData;
        }

        public LoanData GetLC10Data(SystemBorrowerDemographics demos)
        {
            LoanData loanData = new LoanData();
            RI.FastPath("LC10I" + demos.Ssn);
            loanData.CollCostProj = double.Parse(RI.GetText(17, 36, 10)); //Collections Fees Projected
            loanData.ABal = double.Parse(RI.GetText(18, 36, 10)); //Outstading Balance Due
            return loanData;
        }

        public LoanData GetLC05Data(SystemBorrowerDemographics demos, LoanData loanData)
        {
            RI.PutText(21,13,"01");
            RI.Hit(ReflectionInterface.Key.Enter);
            int loanCounter = 1;
            while(!RI.CheckForText(22,3,"46004"))
            {
                if(RI.CheckForText(4, 10, "03") && RI.CheckForText(19,73, "MMDDCCYY"))
                {
                    loanData.DueDay = RI.GetText(10,75,2);
                    loanData.ARate += double.Parse(RI.GetText(6, 8, 6)) * (double.Parse(RI.GetText(20, 32, 12)) / loanData.ABal);
                    LoanDataRow ldr = new LoanDataRow();
                    ldr.Clm = RI.GetText(21, 11, 4);
                    ldr.Pri = double.Parse(RI.GetText(9, 34, 10)) - double.Parse(RI.GetText(10, 34, 10));
                    ldr.Int = double.Parse(RI.GetText(11, 34, 10)) - double.Parse(RI.GetText(12, 34, 10));
                    ldr.Col = double.Parse(RI.GetText(17, 34, 10)) - double.Parse(RI.GetText(18, 34, 10)) + double.Parse(RI.GetText(19, 34, 10));
                    ldr.Fee = double.Parse(RI.GetText(15, 34, 10)) - double.Parse(RI.GetText(16, 34, 10));
                    ldr.Tot = double.Parse(RI.GetText(20, 32, 10)); //Use Tot to recreate balance by loan
                    ldr.RateByLoan = double.Parse(RI.GetText(6, 8, 6));
                    ldr.ColCostProjByLoan = double.Parse(RI.GetText(19, 34, 10));
                    //Get Unique Id from LC05
                    RI.Hit(Key.F10);
                    RI.Hit(Key.F10);
                    ldr.UniqueId = RI.GetText(3,13,19);

                    loanData.loanDataRows.Add(ldr);
                }
                loanCounter++;
                if(loanCounter == 33)
                {
                    Dialog.Info.Ok("The letter only accomodates 32 loans and the borrower has more than 32 loans.  Wait for the system to generate the letter or generate the letter manually.", "Too Many Loans");
                    return null;
                }
                RI.Hit(ReflectionInterface.Key.F8);
            }
            return loanData;
        }

        public void CreateSLCT2Letter(SystemBorrowerDemographics demos, LoanData loanData)
        {
            demos.AccountNumber = demos.AccountNumber.Replace(" ", "");
            //Fileheader: "AccountNumber,KeyLine,FirstName,LastName,Address1,Address2,City,State,Zip,Country,rehab_bal_due,rehab_pay_amt,DueDay,ProjCollCosts"
            //Fileheader LoanDetail: Clm" & i & ",Pri" & i & ",Int" & i & ",Col" & i & ",Fee" & i & ",Tot" & i
            //Fileheader LoanDetailTotal: ",ClmTot,PriTot,IntTot,ColTot,FeeTot,TotTot"
            string letterId = "SLCT2";
            string formattedDueDay = GetFormattedDueDay(loanData.DueDay);
            string keyLine = DocumentProcessing.ACSKeyLine(demos.Ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
            string letterData = demos.AccountNumber.Trim() + "," + keyLine + "," + demos.FirstName.Trim() + "," + demos.LastName.Trim() + "," + demos.Address1.Trim() + "," + demos.Address2.Trim() + "," + demos.City.Trim() + "," + demos.State.Trim() + "," + demos.ZipCode.Trim() + "," + demos.Country.Trim();
            letterData += "," + string.Format("{0:0.00}", loanData.ABal) + "," + string.Format("{0:0.00}", loanData.RHPay) + "," + formattedDueDay + "," + string.Format("{0:0.00}", loanData.CollCostProj);
            //Create letterData columns for loan detail page
            for (int i = 0; i < 32; i++)
            {
                if(i < loanData.loanDataRows.Count)
                {
                    letterData += "," + loanData.loanDataRows[i].Clm + "," + loanData.loanDataRows[i].Pri + "," + loanData.loanDataRows[i].Int + "," + loanData.loanDataRows[i].Col + "," + loanData.loanDataRows[i].Fee + "," + loanData.loanDataRows[i].Tot;
                }
                else
                {
                    letterData += ",,,,,,";
                }
            }
            //Add calculated totals
            letterData += "," + loanData.loanDataTotals.Clm + "," + loanData.loanDataTotals.Pri + "," + loanData.loanDataTotals.Int + "," + loanData.loanDataTotals.Col + "," + loanData.loanDataTotals.Fee + "," + loanData.loanDataTotals.Tot;
            //Add record to print processing
            EcorrProcessing.AddRecordToPrintProcessing(ScriptId, letterId, letterData, demos.AccountNumber, "MA2329");
            //CreateCoborrowerLetters
            CreateCoBorrowerLetters(demos, loanData);
        }

        public void CreateCoBorrowerLetters(SystemBorrowerDemographics demos, LoanData loanData)
        {
            List<DPLTRS.DataObjects.CoBorrowerRecord> records = DA.GetCoBorrowersForBorrower(demos.Ssn);
            if (records != null && records.Count > 0)
            {
                var recordsBySsn = records.GroupBy(r => r.CoBorrowerSSN);
                foreach (var coBorrower in recordsBySsn)
                {
                    LoanData cbLoanData = CalculateCoborrowerLoanData(demos, coBorrower, loanData);
                    CreateSLCT2LetterCoBorrower(demos, coBorrower, cbLoanData);
                }
            }

        }

        public LoanData CalculateCoborrowerLoanData(SystemBorrowerDemographics demos, IGrouping<string, CoBorrowerRecord> coborrower, LoanData borrowerLoanData)
        {
            LoanData ld = new LoanData();
            //Get the loan detail rows that the coborrower is on
            foreach (var uniqueId in coborrower)
            {
                foreach (var r in borrowerLoanData.loanDataRows)
                {
                    if (uniqueId.UniqueId == r.UniqueId)
                    {
                        ld.loanDataRows.Add(r);
                    }
                }
            }
            ld = TotalLoanDataRows(ld);
            //ABal is the sum of the tot column
            if(ld.loanDataTotals.Tot.HasValue)
            {
                ld.ABal = ld.loanDataTotals.Tot.Value;
            }
            //Calculate Rate and Projection
            foreach (var r in ld.loanDataRows)
            {
                if (r.RateByLoan.HasValue && r.Tot.HasValue)
                {
                    ld.ARate += r.RateByLoan.Value * (r.Tot.Value / ld.ABal);
                }
                if(r.ColCostProjByLoan.HasValue)
                {
                    ld.CollCostProj += r.ColCostProjByLoan.Value;
                }
            }
            ld.RHPay = Financial.Pmt(ld.ARate / 100 / 12, 111, -1 * ld.ABal, 2);
            ld.DueDay = borrowerLoanData.DueDay;
            return ld;
        }

        public void CreateSLCT2LetterCoBorrower(SystemBorrowerDemographics demos, IGrouping<string, CoBorrowerRecord> coborrower, LoanData coborrowerLoanData)
        {
            CoBorrowerRecord cb = coborrower.FirstOrDefault();
            //Fileheader: "AccountNumber,KeyLine,FirstName,LastName,Address1,Address2,City,State,Zip,Country,rehab_bal_due,rehab_pay_amt,DueDay,ProjCollCosts"
            //Fileheader LoanDetail: Clm" & i & ",Pri" & i & ",Int" & i & ",Col" & i & ",Fee" & i & ",Tot" & i
            //Fileheader LoanDetailTotal: ",ClmTot,PriTot,IntTot,ColTot,FeeTot,TotTot"
            string letterId = "SLCT2";
            string formattedDueDay = GetFormattedDueDay(coborrowerLoanData.DueDay);
            string keyLine = DocumentProcessing.ACSKeyLine(demos.Ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
            string letterData = demos.AccountNumber + "," + keyLine + "," + cb.FirstName.Trim() + "," + cb.LastName.Trim() + "," + cb.Address1.Trim() + "," + cb.Address2.Trim() + "," + demos.City.Trim() + "," + cb.State.Trim() + "," + cb.Zip.Trim() + "," + cb.ForeignCountry.Trim();
            letterData += "," + string.Format("{0:0.00}", coborrowerLoanData.ABal) + "," + string.Format("{0:0.00}", coborrowerLoanData.RHPay) + "," + formattedDueDay + "," + string.Format("{0:0.00}", coborrowerLoanData.CollCostProj);
            //Create letterData columns for loan detail page
            for (int i = 0; i < 32; i++)
            {
                if (i < coborrowerLoanData.loanDataRows.Count)
                {
                    letterData += "," + coborrowerLoanData.loanDataRows[i].Clm + "," + coborrowerLoanData.loanDataRows[i].Pri + "," + coborrowerLoanData.loanDataRows[i].Int + "," + coborrowerLoanData.loanDataRows[i].Col + "," + coborrowerLoanData.loanDataRows[i].Fee + "," + coborrowerLoanData.loanDataRows[i].Tot;
                }
                else
                {
                    letterData += ",,,,,,";
                }
            }
            //Add calculated totals
            letterData += "," + coborrowerLoanData.loanDataTotals.Clm + "," + coborrowerLoanData.loanDataTotals.Pri + "," + coborrowerLoanData.loanDataTotals.Int + "," + coborrowerLoanData.loanDataTotals.Col + "," + coborrowerLoanData.loanDataTotals.Fee + "," + coborrowerLoanData.loanDataTotals.Tot;
            //Add record to print processing
            EcorrProcessing.AddCoBwrRecordToPrintProcessing(ScriptId, letterId, letterData, cb.AccountNumber.Replace(" ", ""), "MA2329", demos.Ssn);
        }

    }
}
