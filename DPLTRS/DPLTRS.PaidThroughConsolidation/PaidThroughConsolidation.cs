using DPLTRS.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.Scripts;
using System.Windows.Forms;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using System.Reflection;
using static Uheaa.Common.Scripts.ReflectionInterface;
using System.Globalization;

namespace DPLTRS.PaidThroughConsolidation
{
    public class LoanManagementLetters_PaidThroughConsolidation : ScriptBase
    {
        LetterHelper lh;
        public new string ScriptId = "DPLTRS";
        public new ReflectionInterface RI { get; set; }
        public ProcessLogRun logRun { get; set; }
        public DPLTRS.DataAccess DA { get; set; }

        public LoanManagementLetters_PaidThroughConsolidation(ReflectionInterface ri)
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

                LoanData loanData = GetLC05Data(demos);
                if(loanData.LoanInfo.Count != 0)
                {
                    loanData = GetLG10Data(demos, loanData);
                }
                //If the user has BSYS access
                if(loanData.LoanInfo.Count == 0)
                {
                    MessageBox.Show("The Paid-in-Full Through Consolidation letter cannot be printed as the borrower has no loans on LC05 or none of the loans were paid through consolidation.", "No Defaulted Loans", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    try
                    { 
                    CreatePIFTCLetter(demos, loanData);
                    RI.AddCommentInLP50(demos.Ssn, "LT", "03", "DL002", "PIF Letter sent to borrower", ScriptId);
                    //CreatePIFTCLetterCoBorrower(demos, loanData);
                    Dialog.Info.Ok("The letter has been generated.", "Letter Generated");
                    }
                    catch (Exception e)
                    {
                        Dialog.Error.Ok("The letter HAS NOT been generated.", "ERROR");
                        throw e;
                    }   
            }
            }
        }

        public void CreatePIFTCLetter(SystemBorrowerDemographics demos, LoanData loanData)
        {
            demos.AccountNumber = demos.AccountNumber.Replace(" ", "");
            string letterId = "PIFTC";
            string keyLine = DocumentProcessing.ACSKeyLine(demos.Ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
            //Fileheader: "AccountNumber,KeyLine,SSN,LastName,FirstName,Address1,Address2,City,State,Country,ZIP,LoanType,UniqueID,DisbDate" Last 3 repeated 20 times
            string letterData = demos.AccountNumber + "," + keyLine + "," + demos.FirstName.Trim() + "," + demos.LastName.Trim() + "," + "\"" + demos.Address1.Trim() + "\"" + "," + "\"" + demos.Address2.Trim() + "\"" + "," + "\"" + demos.City.Trim() + "\"" + "," + "\"" + demos.State.Trim() + "\"" + "," + demos.ZipCode.Trim() + "," + (demos.Country.IsNullOrEmpty() ? " " : "\"" + demos.Country.Trim() + "\"");
            for (int i = 0; i < 20; i++)
            {
                if (i < loanData.LoanInfo.Count)
                {
                    letterData += "," + loanData.LoanInfo[i].LoanType + "," + loanData.LoanInfo[i].UniqueId + "," + "\"" + loanData.LoanInfo[i].DisbDate.Value.ToString("MM/dd/yyyy") + "\"";
                }
                else
                {
                    letterData += ",,,";
                }
            }

            EcorrProcessing.AddRecordToPrintProcessing(ScriptId, letterId, letterData, demos.AccountNumber, "MA2329");
        }

        public void CreatePIFTCLetterCoBorrower(SystemBorrowerDemographics demos, LoanData loanData)
        {
            List<CoBorrowerRecord> records = DA.GetCoBorrowersForBorrower(demos.Ssn);
            if(records != null && records.Count > 0)
            {
                var recordsBySsn = records.GroupBy(r => r.CoBorrowerSSN);
                foreach (var coBorrower in recordsBySsn)
                {
                    double coBorrowerBalance = 0;
                    List<LoanDataStrings> lds = new List<LoanDataStrings>();
                    foreach (var uniqueId in coBorrower)
                    {
                        foreach (var loanDetailRow in loanData.LoanInfo)
                        {
                            if (loanDetailRow.LoanType == uniqueId.UniqueId)
                            {
                                if(loanDetailRow.Balance.HasValue)
                                {
                                    coBorrowerBalance += loanDetailRow.Balance.Value;
                                }
                                lds.Add(new LoanDataStrings() { LoanType = loanDetailRow.LoanType, UniqueId = loanDetailRow.UniqueId, DisbDate = loanDetailRow.DisbDate, Balance = loanDetailRow.Balance });
                            }
                        }
                    }
                    LoanData cbLoanData = new LoanData(loanData.Balance, lds);
                    CreatePIFTCLetterCoBorrowerHelper(demos, loanData, coBorrower.FirstOrDefault());
                }
            }
        }

        public void CreatePIFTCLetterCoBorrowerHelper(SystemBorrowerDemographics demos, LoanData loanData, CoBorrowerRecord coBorrowerDemos)
        {
            string letterId = "PIFTC";
            string keyLine = DocumentProcessing.ACSKeyLine(demos.Ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
            //Fileheader: "AccountNumber,KeyLine,SSN,LastName,FirstName,Address1,Address2,City,State,Country,ZIP,LoanType,UniqueID,DisbDate" Last 3 repeated 20 times
            string letterData = demos.AccountNumber.Replace(" ", "") + "," + keyLine + "," + coBorrowerDemos.FirstName.Trim() + "," + coBorrowerDemos.LastName.Trim() + "," + coBorrowerDemos.Address1.Trim() + "," + coBorrowerDemos.Address2.Trim() + "," + coBorrowerDemos.City.Trim() + "," + coBorrowerDemos.State.Trim() + "," + coBorrowerDemos.Zip.Trim() + "," + (coBorrowerDemos.ForeignCountry.IsNullOrEmpty() ? " " : coBorrowerDemos.ForeignCountry.Trim());
            for (int i = 0; i < 20; i++)
            {
                if (i < loanData.LoanInfo.Count)
                {
                    letterData += "," + loanData.LoanInfo[i].LoanType + "," + loanData.LoanInfo[i].UniqueId + "," +  "\"" + loanData.LoanInfo[i].DisbDate.Value.ToString("MM/dd/yyyy") + "\"";
                }
                else
                {
                    letterData += ",,,";
                }
            }

            EcorrProcessing.AddCoBwrRecordToPrintProcessing(ScriptId, letterId, letterData, coBorrowerDemos.AccountNumber.Replace(" ", ""), "MA2329", demos.Ssn);
        }

        /// <summary>
        /// For use with the info from GetLC05Data
        /// </summary>
        public LoanData GetLG10Data(SystemBorrowerDemographics demos, LoanData loanData)
        {
            RI.FastPath("LG10I" + demos.Ssn);
            int row = 7;
            if (RI.CheckForText(1, 53, "LOAN BWR STATUS RECAP SELECT"))
            {
                while (!RI.CheckForText(20, 3, "46004"))
                {
                    RI.PutText(19, 15, RI.GetText(row, 5, 2));
                    RI.Hit(Key.Enter);
                    loanData = GetLG10Info(demos, loanData);
                    RI.Hit(Key.F12);
                    row = row + 1;
                    if (RI.GetText(row, 6, 1) == "")
                    {
                        RI.Hit(Key.F8);
                        row = 7;
                    }
                }
            }
            if (RI.CheckForText(1, 52, "LOAN BWR STATUS RECAP DISPLAY"))
            {
                loanData = GetLG10Info(demos, loanData);
            }

            return loanData;
        }

        public LoanData GetLC05Data(SystemBorrowerDemographics demos)
        {
            List<LoanDataStrings> loanInfo = new List<LoanDataStrings>(); //VB uses a 2 dimensional string array without any detail on what it refers to
            double balance = 0;
            RI.FastPath("LC05I" + demos.Ssn);
            if (RI.CheckForText(1, 62, "DEFAULT/CLAIM RECAP"))
            {
                double tempBalance;
                bool success = double.TryParse(RI.GetText(4, 69, 12), out tempBalance);
                if (!success)
                {
                    Dialog.Info.Ok("Unable to determine Balance from LC05.");
                    return null;
                }
                balance = tempBalance;
                RI.PutText(21, 13, "01");
                RI.Hit(Key.Enter);

                while (!RI.CheckForText(22, 3, "46004"))
                {
                    if (RI.CheckForText(4, 26, "11") || RI.CheckForText(4, 26, "12"))
                    {
                        var loanInfoEntry = new LoanDataStrings();
                        loanInfoEntry.LoanType = RI.GetText(3, 13, 2);
                        RI.Hit(Key.F10);
                        RI.Hit(Key.F10);
                        loanInfoEntry.UniqueId = RI.GetText(3, 13, 19);
                        RI.Hit(Key.F10);
                        double tempLoanBalance;
                        string val = RI.GetText(20, 33, 10);
                        bool successBalance = double.TryParse(RI.GetText(20, 34, 10), out tempLoanBalance);
                        if (!successBalance)
                        {
                            Dialog.Info.Ok("Unable to determine Balance from LC05 on UniqueId: " + loanInfoEntry.UniqueId);
                            return null;
                        }
                        loanInfoEntry.Balance = tempLoanBalance;
                        loanInfo.Add(loanInfoEntry);
                    }
                    RI.Hit(Key.F8);
                }
            }
            return new LoanData(balance, loanInfo);
        }

        private LoanData GetLG10Info(SystemBorrowerDemographics demos, LoanData loanData)
        {
            int row = 11;
            while (!RI.CheckForText(22, 3, "46004"))
            {
                for (int i = 0; i < loanData.LoanInfo.Count; i++)
                {
                    if (RI.CheckForText(row, 4, loanData.LoanInfo[i].UniqueId))
                    {
                        if (!RI.CheckForText(row, 26, "MM"))
                        {
                            string val = RI.GetText(row, 26, 8);
                            loanData.LoanInfo[i].DisbDate = DateTime.ParseExact(RI.GetText(row, 26, 8), "MMddyyyy", CultureInfo.InvariantCulture);
                        }
                    }
                }
                row++;

                if (!RI.CheckForText(row, 2, "_"))
                {
                    RI.Hit(Key.F8);
                    row = 11;
                }
            }

            return loanData;
        }
    }
}
