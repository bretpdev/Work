using DPLTRS.Current.DataObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using DPLTRS.DataObjects;

namespace DPLTRS.Current
{
    public class Current : ScriptBase
    {
        public enum LC05Sta
        {
            NoArrangement,
            Current,
            Delinquent,
            None,
            NoOpenLoans
        }

        LetterHelper lh;
        public new string ScriptId = "DPLTRS";
        public new ReflectionInterface RI { get; set; }
        public ProcessLogRun logRun { get; set; }
        public DPLTRS.DataAccess DA { get; set; }
        public Current(ReflectionInterface ri)
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

                LoanData loanData = LC05(demos);
                if(loanData.status == LC05Sta.NoOpenLoans)
                {
                    Dialog.Info.Ok("The Current letter cannot be printed as the borrower has no loans on LC05.");
                }
                else if(loanData.status == LC05Sta.NoArrangement)
                {
                    Dialog.Info.Ok("The Current letter cannot be printed as the borrower has not made payment arrangements.");
                }
                else if(BorrowerCurrent(demos.Ssn))
                {
                    Dialog.Info.Ok("This borrower is not current.  A Current letter will not be generated.");
                }
                else if(!CheckLC41(demos.Ssn, 3, 90) && !CheckLC41(demos.Ssn, 1, 35))
                {
                    Dialog.Info.Ok("The Current letter cannot be printed as the borrower has not made a payment in the past 35 days or 3 payments in the past 90 days.");
                }
                else
                {
                    try
                    {
                        CreateCurrentLetter(demos, loanData);
                        RI.AddCommentInLP50(demos.Ssn, "LT", "03", "DL001", "sent current letter to borrower", "DPLTRS");
                        Dialog.Info.Ok("The letter has been generated.", "Letter Generated");
                    }
                    catch(Exception e)
                    {
                        Dialog.Error.Ok("The letter HAS NOT been generated.", "ERROR");
                        throw e;
                    }
                }
                
            }
        }

        public void CreateCurrentLetter(SystemBorrowerDemographics demos, LoanData loanData)
        {
            demos.AccountNumber = demos.AccountNumber.Replace(" ", "");
            string letterId = "CURRENT";
            string keyLine = DocumentProcessing.ACSKeyLine(demos.Ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
            //Fileheader: "AccountNumber,KeyLine,SSN,LastName,FirstName,Address1,Address2,City,State,Country,ZIP,BilledAmt,DueDay"
            string letterData = demos.AccountNumber.Trim() + "," + keyLine + "," + demos.Ssn.Trim() + "," + demos.LastName.Trim() + "," + demos.FirstName.Trim() + "," + demos.Address1.Trim() + "," + demos.Address2.Trim() + "," + demos.City.Trim() + "," + demos.State.Trim() + "," + demos.Country.Trim() + "," + demos.ZipCode.Trim() + "," + "\"" + loanData.billedAmt.Trim() + "\""+ "," + "\"" + loanData.dueDay + "\"";
            EcorrProcessing.AddRecordToPrintProcessing(ScriptId, letterId, letterData, demos.AccountNumber, "MA2329");
            //Add Coborrower Letters
            CreateCurrentLetterCoborrower(demos, loanData);
        }

        private void CreateCurrentLetterCoborrower(SystemBorrowerDemographics demos, LoanData loanData)
        {
            List<DPLTRS.DataObjects.CoBorrowerRecord> records = DA.GetCoBorrowersForBorrower(demos.Ssn);
            if (records != null && records.Count > 0)
            {
                var recordsBySsn = records.GroupBy(r => r.CoBorrowerSSN);
                foreach (var coborrower in recordsBySsn)
                {
                    LoanData coborrowerLoanData = CalculateCoborrowerLoanData(demos, loanData, coborrower);
                    SendCurrentLetterCoborrower(demos, coborrowerLoanData, coborrower.FirstOrDefault());
                }
            }
        }

        private void SendCurrentLetterCoborrower(SystemBorrowerDemographics demos, LoanData coborrowerLoanData, DPLTRS.DataObjects.CoBorrowerRecord coborrower)
        {
            demos.AccountNumber = demos.AccountNumber.Replace(" ", "");
            string letterId = "CURRENT";
            string keyLine = DocumentProcessing.ACSKeyLine(demos.Ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
            //Fileheader: "AccountNumber,KeyLine,SSN,LastName,FirstName,Address1,Address2,City,State,Country,ZIP,BilledAmt,DueDay"
            string letterData = demos.AccountNumber.Trim() + "," + keyLine + "," + demos.Ssn.Trim() + "," + coborrower.LastName.Trim() + "," + coborrower.FirstName.Trim() + "," + coborrower.Address1.Trim() + "," + coborrower.Address2.Trim() + "," + coborrower.City.Trim() + "," + coborrower.State.Trim() + "," + coborrower.ForeignCountry.Trim() + "," + demos.ZipCode.Trim() + "," + "\"" + coborrowerLoanData.billedAmt.Trim() + "\"" + "," + "\"" + coborrowerLoanData.dueDay + "\"";
            EcorrProcessing.AddCoBwrRecordToPrintProcessing(ScriptId, letterId, letterData, coborrower.AccountNumber.Replace(" ", ""), "MA2329", demos.Ssn);
        }

        private LoanData CalculateCoborrowerLoanData(SystemBorrowerDemographics demos, LoanData borrowerLoanData, IGrouping<string, CoBorrowerRecord> coborrower)
        {
            LoanData ld = new LoanData();
            //Get the loan detail rows that the coborrower is on
            foreach (var uniqueId in coborrower)
            {
                foreach (var r in borrowerLoanData.loanDataRows)
                {
                    if(r.uniqueId == uniqueId.UniqueId)
                    {
                        LoanDataRow ldr = new LoanDataRow();
                        ldr.uniqueId = uniqueId.UniqueId;
                        ldr.billedAmountByLoan = r.billedAmountByLoan;
                        ld.loanDataRows.Add(ldr);
                    }
                }
            }

            double billedAmt = 0;
            foreach(var ldr in ld.loanDataRows)
            {
                billedAmt += ldr.billedAmountByLoan;
            }
            ld.billedAmt = string.Format("{0:0.00}", billedAmt);
            ld.dueDay = borrowerLoanData.dueDay;
            ld.dueDate = borrowerLoanData.dueDate;

            return ld;
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

        public bool BorrowerCurrent(string Ssn)
        {
            bool current = false;
            int eligPmt = 0;
            List<DateTime> pmtDate = new List<DateTime>();
            RI.FastPath("LC41I" + Ssn + ";X");
            if(RI.CheckForText(21,3,"SEL"))
            {
                int row = 7;
                while(DateTime.ParseExact(RI.GetText(row,5,8), "MMddyyyy", CultureInfo.InvariantCulture) >= DateTime.Today.AddDays(-90) && RI.GetText(22,3,5) != "46004")
                {
                    if((RI.CheckForText(row, 34, "BR") || RI.CheckForText(row, 34, "EP")) && RI.CheckForText(row, 39, " "))
                    {
                        eligPmt++;
                        pmtDate.Add(DateTime.ParseExact(RI.GetText(row, 5, 8), "MMddyyyy", CultureInfo.InvariantCulture));
                        if(eligPmt == 3)
                        {
                            break;
                        }
                    }
                    row++;
                    if(RI.CheckForText(row, 34, "  "))
                    {
                        row = 7;
                        RI.Hit(ReflectionInterface.Key.F8);
                    }
                }
            }
            if(eligPmt >= 3)
            {
                for(int i = 0; i < pmtDate.Count; i++)
                {
                    if(DateTime.Today.AddDays(-35) <= pmtDate[i])
                    {
                        current = true;
                        return current;
                    }
                }
            }
            return current;
        }

        public bool CheckLC41(string ssn, int cnt, int days)
        {
            int count = 0;
            RI.FastPath("LC41I" + ssn + "X");
            while(!RI.CheckForText(22,3, "46004 NO MORE DATA TO DISPLAY"))
            {
                for(int i = 7; i < 18; i++)
                {
                    if(RI.CheckForText(i, 34, "BR") || RI.CheckForText(i,34,"EP"))
                    {
                        DateTime dt = DateTime.ParseExact(RI.GetText(i, 5, 8), "MMddyyyy", CultureInfo.InvariantCulture);
                        if (DateTime.Today.AddDays(-1 * days) <= dt)
                        {
                            count++;
                        }
                        if(count >= cnt)
                        {
                            return true;
                        }
                    }
                }
                RI.Hit(ReflectionInterface.Key.F8);
            }
            return false;
        }

        public LoanData LC05(SystemBorrowerDemographics demos)
        {
            LoanData loanData = new LoanData();
            loanData.billedAmt = RI.GetText(3, 16, 10);
            //Access LC05 target
            RI.PutText(21, 13, "01");
            RI.Hit(ReflectionInterface.Key.Enter);
            while(!RI.CheckForText(22,3,"46004"))
            {
                //If there is an open loan
                if(RI.GetText(4,10,2) == "03" && RI.GetText(19,73,8) == "MMDDCCYY")
                {
                    //If the date is MM then no arrangements have been made
                    if (RI.GetText(16,73,2) == "MM")
                    {
                        if ((loanData.status == LC05Sta.None || loanData.status == LC05Sta.NoOpenLoans) && loanData.status != LC05Sta.Delinquent)
                        {
                            loanData.status = LC05Sta.NoArrangement; //Final status
                        }
                    }
                    //if the datte is >60 in the past the borrower is Current
                    else if(DateTime.ParseExact(RI.GetText(16,73,8), "MMddyyyy", CultureInfo.InvariantCulture) > DateTime.Today.AddDays(-60))
                    {
                        if(loanData.status != LC05Sta.Delinquent && loanData.status != LC05Sta.NoArrangement)
                        {
                            loanData.status = LC05Sta.Current;
                        }
                    }
                    else if(loanData.status != LC05Sta.NoArrangement)
                    {
                        loanData.status = LC05Sta.Delinquent; //Final status
                    }

                    //Get the date
                    if (!RI.CheckForText(10, 73, "MM"))
                    {
                        loanData.dueDate = RI.GetText(10, 73, 8);
                        loanData.dueDay = GetFormattedDueDay(RI.GetText(10, 75, 2));
                    }
                    //Get unique id and billedAmtByLoan
                    LoanDataRow ldr = new LoanDataRow();
                    ldr.billedAmountByLoan = double.Parse(RI.GetText(11, 71, 10));
                    RI.Hit(ReflectionInterface.Key.F10);
                    RI.Hit(ReflectionInterface.Key.F10);
                    ldr.uniqueId = RI.GetText(3, 13, 19);
                    loanData.loanDataRows.Add(ldr);
                }
                //if an open loan is not found
                else
                {
                    if(loanData.status == LC05Sta.None && loanData.status != LC05Sta.Delinquent && loanData.status != LC05Sta.NoArrangement)
                    {
                        loanData.status = LC05Sta.NoOpenLoans;
                    }
                }
                //Look at next loan
                RI.Hit(ReflectionInterface.Key.F8);
            }
            return loanData;
        }
    }
}
