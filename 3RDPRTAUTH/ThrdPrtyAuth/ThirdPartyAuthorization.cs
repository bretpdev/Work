using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;
using ThrdPrtyAuth.Infrastructure;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Q;

namespace ThrdPrtyAuth
{
    public class ThirdPartyAuthorization : Uheaa.Common.Scripts.ScriptBase
    {
        public ProcessLogRun logRun { get; set; }
        public DataAccess DA { get; set; }

        private string DATA_FILE { get; set; }

        private MDBorrower Bor { get; set; } // This Should be only Q stuff
        private Uheaa.Common.Scripts.SystemBorrowerDemographics sysBorrDemos { get; set; }
        private bool calledByMauiDUDE{ get; set; }
        public List<CoBorrowers> coBorrowers { get; set; }

        public ThirdPartyAuthorization(Uheaa.Common.Scripts.ReflectionInterface RI)
        : base(RI, "3RDPRTAUTH")
        {
            calledByMauiDUDE = false;
            DATA_FILE = Uheaa.Common.DataAccess.EnterpriseFileSystem.TempFolder  + "ThirdPartyDat.txt";
        }

        public ThirdPartyAuthorization(Q.ReflectionInterface RI, MDBorrower borrower, int runNumber)
        : this(new Uheaa.Common.Scripts.ReflectionInterface(RI.ReflectionSession))
        {
            Bor = borrower;
            calledByMauiDUDE = true;
            DATA_FILE = Uheaa.Common.DataAccess.EnterpriseFileSystem.TempFolder + "ThirdPartyDat.txt";
        }

        public override void Main()
        {
            //Get demographics either from the passed-in DUDE object or from the system.
            ThrdPrtyAuth.Infrastructure.BorrowerDemographics demographics = null;
            logRun = new ProcessLogRun("3RDPRTAUTH", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            DA = new DataAccess(logRun);

            if (calledByMauiDUDE)
            {
                demographics.AccountNumber = Bor.UserProvidedDemos.AccountNumber;
                demographics.LName = Bor.UserProvidedDemos.LName;
                demographics.FName = Bor.UserProvidedDemos.FName;
                demographics.Addr1 = Bor.UserProvidedDemos.Addr1;
                demographics.Addr2 = Bor.UserProvidedDemos.Addr2;
                demographics.City = Bor.UserProvidedDemos.City;
                demographics.State = Bor.UserProvidedDemos.State;
                demographics.Zip = Bor.UserProvidedDemos.Zip;
                demographics.Country = Bor.UserProvidedDemos.Country;
                demographics.KeyLine = DocumentProcessing.ACSKeyLine(Bor.UserProvidedDemos.SSN, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
            }
            else
            {
                //Bug the user for an SSN until they provide a valid one.
                InputBoxResults results = InputBox.ShowDialog("Please enter the SSN of the borrower for whom you wish to print a third-party authorization form.", "Third-Party Authorization Form");
                if (results.DialogRe != DialogResult.OK || results.UserProvidedText.Length == 0) { return; }
                string lp22Path = (results.UserProvidedText.Length > 9 ? "LP22I;;;;;;" : "LP22I");
                RI.FastPath(lp22Path + results.UserProvidedText);
                while (!RI.CheckForText(1, 65, "SON DEM"))
                {
                    results = InputBox.ShowDialog("The SSN entered is invalid. Re-enter the SSN below.", "Invalid SSN", results.UserProvidedText);
                    if (results.DialogRe != DialogResult.OK || results.UserProvidedText.Length == 0) { return; }
                    lp22Path = (results.UserProvidedText.Length > 9 ? "LP22I;;;;;;" : "LP22I");
                    RI.FastPath(lp22Path + results.UserProvidedText);
                }

                //Check that the address is valid.
                if (!RI.CheckForText(10, 57, "Y"))
                {
                    MessageBox.Show("Address on file is invalid, cannot generate letter.");
                    return;
                }
                sysBorrDemos = RI.GetDemographicsFromLP22(results.UserProvidedText);
                demographics = translate(sysBorrDemos);
                string noSpaces = demographics.AccountNumber;
                demographics.AccountNumber = noSpaces.Replace(" ", "");

            }

            int returnValue = 0;

            if (RI.HasOpenLoanOnOneLINK(demographics.SSN))
                returnValue = CreateLetter(demographics);
            else
            {
                MessageBox.Show($"Account {demographics.AccountNumber} has no open loans.", "Notice", MessageBoxButtons.OK);
                return;
            }

            if (returnValue > 0)
                return;

            AddArcs(demographics);
            
        }//Main()



        private void AddArcs(Infrastructure.BorrowerDemographics demographics)
        {
            ArcData arc;
            List<string> prgs = new List<string>();
            List<int> seqs = new List<int>();
            //Add a system activity record and print the letter.
            if (RI.HasOpenLoanOnOneLINK(demographics.SSN))
            {
                arc = new ArcData(DataAccessHelper.Region.Uheaa)
                {
                    Arc = "D3RDP",
                    Comment = "",
                    ScriptId = ScriptId,
                    ActivityType = "FO",
                    ActivityContact = "03",
                    AccountNumber = demographics.AccountNumber,
                    ArcTypeSelected = ArcData.ArcType.OneLINK,
                    LoanSequences = seqs,
                    LoanPrograms = prgs
                };
            }
            else
            {
                arc = new ArcData(DataAccessHelper.Region.Uheaa)
                {
                    Arc = "D3RDP",
                    Comment = "",
                    ScriptId = ScriptId,
                    AccountNumber = demographics.AccountNumber,
                    ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                    LoanSequences = seqs,
                    LoanPrograms = prgs
                };
            }

            ArcAddResults result = arc.AddArc();
            if(!result.ArcAdded)
                logRun.AddNotification(string.Format("An error occurred while adding an activity comment for the following borrowers account: Account Number: {0} Borrowers Name: {1} {2} Error: {3} (3A)", demographics.AccountNumber, demographics.FirstName, demographics.LastName, result.Ex.Message), NotificationType.ErrorReport, NotificationSeverityType.Critical);
        }

        private int CreateLetter(Infrastructure.BorrowerDemographics demographics)
        {
            if (demographics.State.Length == 0 || demographics.State.ToLower() == "fc") { demographics.State = "  "; }
            demographics.AccountNumber = demographics.AccountNumber.Replace(" ", "");

            string lineData = $"\"{demographics.AccountNumber}\",\"{demographics.LName}\",\"{demographics.FName}\",\"{demographics.Addr1}\",\"{demographics.Addr2}\",\"{demographics.City}\",\"{demographics.State}\",\"{demographics.Zip}\",\"{demographics.Country}\",\"{demographics.KeyLine}\",";
            lineData += $"\"{demographics.AccountNumber[0].ToString()}\",\"{demographics.AccountNumber[1].ToString()}\",\"{demographics.AccountNumber[2].ToString()}\",\"{demographics.AccountNumber[3].ToString()}\",\"{demographics.AccountNumber[4].ToString()}\",";
            lineData += $"\"{demographics.AccountNumber[5].ToString()}\",\"{demographics.AccountNumber[6].ToString()}\",\"{demographics.AccountNumber[7].ToString()}\",\"{demographics.AccountNumber[8].ToString()}\",\"{demographics.AccountNumber[9].ToString()}\"";

            int? retVal = 0;

            try
            {
                retVal = DA.AddOneLink(this.ScriptId, "THRDPRTY", lineData, demographics.AccountNumber, "MA2324");
                //retVal = EcorrProcessing.AddRecordToPrintProcessing(this.ScriptId, "THRDPRTY", lineData, demographics.AccountNumber, "MA2324");
            }
            catch(Exception e)
            {
                retVal = null;
                logRun.AddNotification($"Account {demographics.AccountNumber} generates error {e.Message}.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }

            if (retVal == null)
            {
                MessageBox.Show($"Account {demographics.AccountNumber} generated and error, consult process log.", "Error", MessageBoxButtons.OK);
                return 1;
            }
            else
            {
                MessageBox.Show("The THRDPRTY letter will be generated by UHECORPRT.", "Letter Scheduled", MessageBoxButtons.OK);
                return 0;
            }
        }//CreateLetter()

        ThrdPrtyAuth.Infrastructure.BorrowerDemographics translate(Uheaa.Common.Scripts.SystemBorrowerDemographics entity)
        {
            return new Infrastructure.BorrowerDemographics
            {
                AccountNumber = entity.AccountNumber,
                LName = entity.LastName,
                FName = entity.FirstName,
                Addr1 = entity.Address1,
                Addr2 = entity.Address2,
                City = entity.City,
                State = entity.State,
                Zip = entity.ZipCode,
                Country = entity.Country,
                KeyLine = DocumentProcessing.ACSKeyLine(entity.Ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal)
            };
        }


        }//class
    }//namespace
