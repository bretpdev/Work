using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Q;
using Key = Q.ReflectionInterface.Key;
using System.IO;

namespace GODISCCONS
{
    public class PrintGoDisclosureConsummated : ScriptBase
    {

        enum PersonType
        {
            Borrower,
            Endorser
        }

        private string DOCFOLDER;
        private Boolean INTESTMODE;

        /// <summary>
        /// Constructor For Borg Call
        /// </summary>
        /// <param name="ri"></param>
        public PrintGoDisclosureConsummated(ReflectionInterface ri) : base(ri, "GODISCCONS") 
        {
            if (ri.TestMode) 
            {
                DOCFOLDER = @"X:\PADD\LoanOrigination\Test\";
                INTESTMODE = true;
            }
            else
            { 
                DOCFOLDER = @"X:\PADD\LoanOrigination\";
                INTESTMODE = false;
            }
        }

        /// <summary>
        /// main functionality starting point
        /// </summary>
        public override void Main()
        {
            InputBoxResults iBResults = InputBox.ShowDialog("Enter the SSN of the borrower to print a GO loan consummation disclosure.", "GO Loan Disclosures - Consummation");
            bool userProvidedDataIsValid = false; //tracks when data provided by user is valid
            while (userProvidedDataIsValid == false)
            {
                if (iBResults.DialogRe != System.Windows.Forms.DialogResult.OK)
                {
                    EndDLLScript(); //end the script if the user didn't click OK
                }
                else if (iBResults.UserProvidedText.Length == 9 && iBResults.UserProvidedText.IsNumeric() == true)
                {
                    userProvidedDataIsValid = true;
                }
                else
                {
                    iBResults = InputBox.ShowDialog("Please provide the SSN.", "Please provide the SSN.");
                }
            }

            //continue processing once the data provided the user is valid
            Borrower brw = new Borrower(iBResults.UserProvidedText);

            PO1NChecksAndDataCollection(brw);
            VerifyHasEligibleLoans(brw);
            GetData(brw);
            PrintDocuments(brw);
            UpdatePO1N(brw);

            string message = "Processing complete.";
            string caption = "Processing Complete";
            MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
            EndDLLScript();
        }

        //does PO1N eligible loan checks
        private void PO1NChecksAndDataCollection(Borrower brw)
        {
            FastPath(string.Format("PO1NI{0}", brw.SSN));

            //warn the user and end the script if PO1N is not displayed (there are no eligible loans)
            if (Check4Text(1, 75, "POX1O")) { NoEligibleLoans(); }

            //review loans if selection screen is displayed
            if (Check4Text(1, 75, "POX1P"))
            {
                List<string> approvedLoans = new List<string>();
                while (!Check4Text(22, 2, "90007"))
                {
                    for (int i = 9; i < 21; i++)
                    {
                        if (Check4Text(i, 5, "GO"))
                        {
                            PutText(21, 16, GetText(i, 2, 2), Key.Enter);
                            if (Check4Text(6, 27, "APPROVED")) {
                                approvedLoans.Add(GetText(4, 46, 11)); 
                            }
                            Hit(Key.F12);
                        }
                    }
                    Hit(Key.F8);
                }

                //warn the user and end the script if there is more than one eligible loan
                if (approvedLoans.Count > 1)
                {
                    string message = "There are multiple eligible loans.  Correct this before restarting the script.";
                    string caption = "Multiple Open Loans";
                    MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    EndDLLScript();
                }

                //warn the user and end the script if there are no eligible loans
                if (approvedLoans.Count == 0) { NoEligibleLoans(); }

                //go to the POX1S detail screen if there is only one eligible loan
                FastPath("PO1NI");
                PutText(5, 41, brw.SSN);
                PutText(6, 41, approvedLoans[0].Replace(" ", ""));
                Hit(Key.Enter);
            }

            //review loans if detail screen is displayed
            if (Check4Text(1, 75, "POX1S"))
            {
                if (Check4Text(2, 46, "MD", "PA", "PT", "RN", "RX")) //check if the loan is a GO loan
                {
                    if (!Check4Text(6, 27, "APPROVED")) { NoEligibleLoans(); }
                }
                else
                {
                    NoEligibleLoans();
                }
            }

        }

        //apply additional edits to verify loan eligibility
        private void VerifyHasEligibleLoans(Borrower brw)
        {
            if (GetText(21, 23, 1) != string.Empty) { NoEligibleLoans(); }//REJ/SUSP CONDITIONS
            if (GetText(20, 21, 1) != string.Empty) { NoEligibleLoans(); }//INCOMPLETE ERRORS
            PutText(2, 78, "AP", Key.Enter);
            if (GetText(17, 35, 2) != "__") { NoEligibleLoans(); }//APPLICATION HOLD
            if (GetText(17, 65, 10) != "__________") { NoEligibleLoans(); }//APPLICATION HOLD DATE
            brw.TargetAppID = GetText(1, 19, 9);

            //access endorser page
            PutText(2, 78, "EL", Key.Enter);
            if (Check4Text(1,75,"POX1Y"))
            {
                brw.EndorserID = GetText(10,4,11).Replace("-","");
                if (brw.EndorserID == "___________") {brw.EndorserID = string.Empty;}
            }

            //go to PO6I and do additional eligibility checks
            FastPath("PO6II" + brw.SSN + ";" + brw.TargetAppID);
            string dateVerified;
            int row = 9;

            do
            {   
                if (Check4Text(row, 70, brw.TargetAppID))
                {
                    dateVerified = GetText(row, 10, 10);
                    if (!dateVerified.IsValidDate()) { NoEligibleLoans(); }
                    if (DateTime.Parse(dateVerified) < DateTime.Today.AddDays(-60)) { NoEligibleLoans(); }
                    if (Check4Text(row, 3, "N")) { NoEligibleLoans(); }

                    //there are better ways to code this but it follows the spec to avoid confusion with the spec
                    string numSgn;
                    string eligible = string.Empty;
                    numSgn = GetText(row, 7, 2);
                    if ((numSgn == "1") && (brw.EndorserID != string.Empty)) { eligible = "N"; }
                    if ((numSgn == "1") && (brw.EndorserID == string.Empty)) { eligible = "Y"; }
                    if ((numSgn == "2") && (brw.EndorserID == string.Empty)) { eligible = "N"; }
                    if ((numSgn == "2") && (brw.EndorserID != string.Empty)) { eligible = "Y"; }
                    if (numSgn == "__") 
                    { 
                        eligible = "N"; 
                    }
                    else if (double.Parse(numSgn) > 2) { eligible = "N"; }
                    if (eligible == "N")
                    {
                        MessageBox.Show("Invalid number of MPNs received.  Not eligible for final disclosure.", "Not Eligible", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        EndDLLScript();
                    }

                    break;
                }
                
                row++;
                if (row > 21) 
                {
                    Hit(Key.F8);                              
                    if (Check4Text(22, 2, "90007")) { NoEligibleLoans(); }
                    row = 9;
                }
            } while (!Check4Text(22,2,"90007"));


        }

        //gather information
        private void GetData(Borrower brw)
        {
            //get borrower's demos 
            brw.Demos = GetDemosFromCOMPASS(brw.SSN, PersonType.Borrower);

            //get endorser's demos
            if (brw.EndorserID != string.Empty)
            {
                brw.EndorserDemos = GetDemosFromCOMPASS(brw.EndorserID, PersonType.Endorser);
            }

            //gather information
            FastPath("PO1NI");
            PutText(5, 41, brw.SSN);
            PutText(6, 41, brw.TargetAppID);
            Hit(Key.Enter);

            PutText(2, 78, "GI", Key.Enter);
            brw.GuarAmount = double.Parse(GetText(9, 51, 10));
            brw.IntRate = double.Parse(GetText(12, 24, 6));

            PutText(2, 78, "SC", Key.Enter);
            brw.LoanTermBegin = GetText(11, 54, 10).Replace(" ", "/");
            brw.LoanTermEnd = GetText(11, 69, 10).Replace(" ", "/");
            brw.GradDate = GetText(13, 54, 10).Replace(" ", "/");
            brw.SchoolName = GetText(8, 31, 20);

            FastPath("TX3ZITXBL");
            PutText(7, 43, "A");
            PutText(9, 43, "G1");
            PutText(11, 43, DateTime.Today.Month + "01" + DateTime.Today.ToString("yy"));
            Hit( Key.Enter);
            brw.WsjRate = double.Parse(GetText(14, 43, 5));

            //calculate information for the letter
            brw.PerformCalculations();
        }

        //gets demographics for the borrower and/or endorser
        private SystemBorrowerDemographics GetDemosFromCOMPASS(string ssnOrEndorserID, PersonType type)
        {
            SystemBorrowerDemographics demos = GetDemographicsFromTX1J(ssnOrEndorserID);
            //check if the address is valid
            if (demos.AddrValidityIndicator == "N")
            {
                //if the address validity indicator is set to no then
                if (type == PersonType.Borrower)
                {
                    MessageBox.Show("Borrower address invalid.  A valid address must be located before this disclosure is created.", "Invalid Address", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Endorser address invalid.  A valid address must be located before this disclosure is created.", "Invalid Address", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                EndDLLScript();
            }
            return demos;
        }

        //print and image disclosures
        private void PrintDocuments(Borrower brw)
        {
            //borrower disclosure
            VbaStyleFileOpen(DataAccessBase.PersonalDataDirectory + "godiscdat.txt", 1, Common.MSOpenMode.Output);
            VbaStyleFileWriteLine(1, "SSN", "KeyLine", "FirstName", "LastName", "Address1", "Address2", "City", "State", "ZIP", "Country", "AccountNumber", "CancellationDate", "LoanAmount", "InterestRate", "TotalInterest", "TotalCost", "SchoolName", "APR", "LoanBeginDate", "AGD", "RepaymentBeginDate", "SecondLastRepaymentEndDate", "LastRepaymentEndDate", "PmtAmt1", "PmtAmt2", "PmtAmt3", "PmtAmt4", "WSJRate", "InSchoolInt", "InSchoolInt18", "TotalCost18", "NumPmts", "NumPmts18", "SecondLastRepaymentEndDate18", "LastRepaymentEndDate18");
            VbaStyleFileWriteLine(1, brw.SSN, brw.BorrowerKeyLine, brw.Demos.FName, brw.Demos.LName, brw.Demos.Addr1, brw.Demos.Addr2, brw.Demos.City, brw.Demos.State, brw.Demos.Zip, brw.Demos.Country, brw.Demos.AccountNumber, brw.CancellationDate, brw.GuarAmount.ToString("#,##0.00"), brw.IntRate.ToString("0.00"), brw.TotalInterestAtCurrentRate.ToString("#,##0.00"), brw.TotalCostAtCurrentRate.ToString("#,##0.00"), brw.SchoolName, brw.APR.ToString("0.00"), brw.LoanTermBegin, brw.GradDate, brw.RepaymentStart, brw.SecondToLastRepaymentDate, brw.LastRepaymentDate, brw.EstimatedPaymentAtCurrentRate.ToString("#,##0.00"), brw.EstimatedPaymentAtCurrentRate.ToString("#,##0.00"), brw.EstimatedPaymentAt18Percent.ToString("#,##0.00"), brw.EstimatedPaymentAt18Percent.ToString("#,##0.00"), brw.WsjRate.ToString("0.00"), brw.InSchoolInterestAtCurrentRate.ToString("#,##0.00"), brw.InSchoolInterestAt18Percent.ToString("#,##0.00"), brw.TotalCostAt18Percent.ToString("#,##0.00"), (brw.NumberOfPaymentsAtCurrentRate - 1).ToString(), (brw.NumberOfPaymentsAt18Percent - 1).ToString(), brw.SecondToLastRepaymentDateAt18Percent, brw.LastRepaymentDateAt18Percent);
            VbaStyleFileClose(1);
            
            GiveMeItAll_LtrEmlFax_BarCd_StaCurDt("GOCONDISC", DocumentHandling.CentralizedPrintingSystemToAddComments.stacCOMPASS, brw.SSN, DataAccess.PersonalDataDirectory + "godiscdat.txt", "AccountNumber", "GOCONDISC", DOCFOLDER, "State", DocumentHandling.CentralizedPrintingDeploymentMethod.dmLetter, "03", DocumentHandling.Barcode2DLetterRecipient.lrBorrower);

            DocumentHandling.AddBarcodeAndStaticCurrentDateForUserProcessing(RI, DataAccessBase.PersonalDataDirectory + "godiscdat.txt", "AccountNumber", "GOCONDISC", false, 0, true, DocumentHandling.Barcode2DLetterRecipient.lrBorrower);
            DocumentHandling.ImageDocs("SSN", "GOBCD", DOCFOLDER, "GOCONDISC", DataAccessBase.PersonalDataDirectory + "godiscdat.txt", INTESTMODE);
            
            File.Delete(DataAccessBase.PersonalDataDirectory + "godiscdat.txt");

            //endorser disclosre
            if (brw.EndorserID != string.Empty)
            {
                VbaStyleFileOpen(DataAccessBase.PersonalDataDirectory + "godisccodat.txt", 2, Common.MSOpenMode.Output);
                VbaStyleFileWriteLine(2, "SSN", "KeyLine", "FirstName", "LastName", "Address1", "Address2", "City", "State", "ZIP", "Country", "AccountNumber", "CancellationDate", "LoanAmount", "InterestRate", "TotalInterest", "TotalCost", "SchoolName", "APR", "LoanBeginDate", "AGD", "RepaymentBeginDate", "SecondLastRepaymentEndDate", "LastRepaymentEndDate", "PmtAmt1", "PmtAmt2", "PmtAmt3", "PmtAmt4", "WSJRate", "InSchoolInt", "InSchoolInt18", "TotalCost18", "NumPmts", "NumPmts18", "SecondLastRepaymentEndDate18", "LastRepaymentEndDate18");
                VbaStyleFileWriteLine(2, brw.EndorserID, brw.EndorserKeyLine, brw.EndorserDemos.FName, brw.EndorserDemos.LName, brw.EndorserDemos.Addr1, brw.EndorserDemos.Addr2, brw.EndorserDemos.City, brw.EndorserDemos.State, brw.EndorserDemos.Zip, brw.EndorserDemos.Country, brw.Demos.AccountNumber, brw.CancellationDate, brw.GuarAmount.ToString("#,##0.00"), brw.IntRate.ToString("0.00"), brw.TotalInterestAtCurrentRate.ToString("#,##0.00"), brw.TotalCostAtCurrentRate.ToString("#,##0.00"), brw.SchoolName, brw.APR.ToString("0.00"), brw.LoanTermBegin, brw.GradDate, brw.RepaymentStart, brw.SecondToLastRepaymentDate, brw.LastRepaymentDate, brw.EstimatedPaymentAtCurrentRate.ToString("#,##0.00"), brw.EstimatedPaymentAtCurrentRate.ToString("#,##0.00"), brw.EstimatedPaymentAt18Percent.ToString("#,##0.00"), brw.EstimatedPaymentAt18Percent.ToString("#,##0.00"), brw.WsjRate.ToString("0.00"), brw.InSchoolInterestAtCurrentRate.ToString("#,##0.00"), brw.InSchoolInterestAt18Percent.ToString("#,##0.00"), brw.TotalCostAt18Percent.ToString("#,##0.00"), (brw.NumberOfPaymentsAtCurrentRate - 1).ToString(), (brw.NumberOfPaymentsAt18Percent - 1).ToString(), brw.SecondToLastRepaymentDateAt18Percent, brw.LastRepaymentDateAt18Percent);
                VbaStyleFileClose(2);
                
                GiveMeItAll_LtrEmlFax_BarCd_StaCurDt("GOCNSDISCB", DocumentHandling.CentralizedPrintingSystemToAddComments.stacCOMPASS, brw.SSN, DataAccess.PersonalDataDirectory + "godisccodat.txt", "AccountNumber", "GOCNSDISCB", DOCFOLDER, "State", DocumentHandling.CentralizedPrintingDeploymentMethod.dmLetter, "03", DocumentHandling.Barcode2DLetterRecipient.lrBorrower);

                DocumentHandling.AddBarcodeAndStaticCurrentDateForUserProcessing(RI, DataAccessBase.PersonalDataDirectory + "godisccodat.txt", "AccountNumber", "GOCNSDISCB", false, 0, true, DocumentHandling.Barcode2DLetterRecipient.lrBorrower);
                DocumentHandling.ImageDocs("SSN", "GOCCD", DOCFOLDER, "GOCNSDISCB", DataAccessBase.PersonalDataDirectory + "godisccodat.txt", INTESTMODE);

                File.Delete(DataAccessBase.PersonalDataDirectory + "godisccodat.txt");
            }
        }

        //update PO1N
        private void UpdatePO1N(Borrower brw)
        {
            FastPath("PO1NC");
            PutText(5, 41, brw.SSN);
            PutText(6, 41, brw.TargetAppID);
            Hit(Key.Enter);

            PutText(2, 78, "AP", Key.Enter);
            PutText(17, 35, "02", Key.Enter);

            ATD37AllLoans(brw.SSN, "GOBCD", "borrower final disclosure sent", false);
            if (brw.EndorserID != string.Empty)
            {
                ATD37AllLoans(brw.SSN, "GOCCD", "co-signer final disclosure sent", false);
            }
        }

        //warn the user and end the script when there are no eligible loans
        private void NoEligibleLoans()
        {
            MessageBox.Show("No eligible loans.", "No Eligible Loans", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            EndDLLScript();
        }
    }
}
