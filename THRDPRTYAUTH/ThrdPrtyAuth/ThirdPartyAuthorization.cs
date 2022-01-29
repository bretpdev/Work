using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Q;

namespace ThrdPrtyAuth
{
    public class ThirdPartyAuthorization : ScriptBase
    {
        private readonly string DATA_FILE;
        private readonly string USER_ID;

        public ThirdPartyAuthorization(ReflectionInterface ri)
            : base(ri, "3RDPRTAUTH")
        {
            DATA_FILE = DataAccessBase.PersonalDataDirectory + "ThirdPartyDat.txt";
            USER_ID = GetUserIDFromLP40();
        }

        public ThirdPartyAuthorization(ReflectionInterface ri, MDBorrower borrower, int runNumber)
            : base(ri, "3RDPRTAUTH", borrower, runNumber)
        {
            DATA_FILE = DataAccessBase.PersonalDataDirectory + "ThirdPartyDat.txt";
            USER_ID = GetUserIDFromLP40();
        }

        public override void Main()
        {
            //Get demographics either from the passed-in DUDE object or from the system.
            BorrowerDemographics demographics = null;
            if (base.CalledByMauiDUDE)
            {
                demographics = base.MauiDUDEBorrower.UserProvidedDemos;
            }
            else
            {
                //Bug the user for an SSN until they provide a valid one.
                InputBoxResults results = InputBox.ShowDialog("Please enter the SSN of the borrower for whom you wish to print a third-party authorization form.", "Third-Party Authorization Form");
                if (results.DialogRe != DialogResult.OK || results.UserProvidedText.Length == 0) { return; }
                string lp22Path = (results.UserProvidedText.Length > 9 ? "LP22I;;;;;;" : "LP22I");
                FastPath(lp22Path + results.UserProvidedText);
                while (!Check4Text(1, 65, "SON DEM"))
                {
                    results = InputBox.ShowDialog("The SSN entered is invalid. Re-enter the SSN below.", "Invalid SSN", results.UserProvidedText);
                    if (results.DialogRe != DialogResult.OK || results.UserProvidedText.Length == 0) { return; }
                    lp22Path = (results.UserProvidedText.Length > 9 ? "LP22I;;;;;;" : "LP22I");
                    FastPath(lp22Path + results.UserProvidedText);
                }

                //Check that the address is valid.
                if (!Check4Text(11, 57, "Y"))
                {
                    MessageBox.Show("Addres on file is invalid, cannot generate letter.");
                    EndDLLScript();
                }
                demographics = GetDemographicsFromLP22(results.UserProvidedText);
            }

            //Add a system activity record and print the letter.
            if (Common.HasOpenLoanOnOneLINK(base.RS, demographics.SSN))
            {
                AddCommentInLP50(demographics.SSN, "D3RDP", "FO", "03", "");
            }
            else
            {
                ATD22AllLoans(demographics.SSN, "D3RDP", "", false);
            }
            CreateLetter(demographics);
        }//Main()

        private void CreateLetter(BorrowerDemographics demographics)
        {
            if (demographics.State.Length == 0 || demographics.State.ToLower() == "fc") { demographics.State = "  "; }
            demographics.AccountNumber = demographics.AccountNumber.Replace(" ", "");
            List<string> values = new List<string>();
            values.Add(demographics.AccountNumber);
            values.Add(demographics.LName);
            values.Add(demographics.FName);
            values.Add(demographics.Addr1);
            values.Add(demographics.Addr2);
            values.Add(demographics.City);
            values.Add(demographics.State);
            values.Add(demographics.Zip);
            values.Add(demographics.Country);
            values.Add(ACSKeyLine(demographics.SSN, Common.ACSKeyLinePersonType.Borrower, Common.ACSKeyLineAddressType.Legal));
            values.Add(demographics.AccountNumber[0].ToString());
            values.Add(demographics.AccountNumber[1].ToString());
            values.Add(demographics.AccountNumber[2].ToString());
            values.Add(demographics.AccountNumber[3].ToString());
            values.Add(demographics.AccountNumber[4].ToString());
            values.Add(demographics.AccountNumber[5].ToString());
            values.Add(demographics.AccountNumber[6].ToString());
            values.Add(demographics.AccountNumber[7].ToString());
            values.Add(demographics.AccountNumber[8].ToString());
            values.Add(demographics.AccountNumber[9].ToString());
            using (StreamWriter fileWriter = new StreamWriter(DATA_FILE, false))
            {
                fileWriter.WriteCommaDelimitedLine("AN", "LastName", "FirstName", "Address1", "Address2", "City", "State", "ZIP", "Country", "KeyLine", "AN1", "AN2", "AN3", "AN4", "AN5", "AN6", "AN7", "AN8", "AN9", "AN10");
                fileWriter.WriteCommaDelimitedLine(values.ToArray());
                fileWriter.Close();
            }
            DocumentHandling.GiveMeItAll_LtrEmlFax_BarCd_StaCurDt(base.RI, "THRDPRTY", DocumentHandling.CentralizedPrintingSystemToAddComments.stacOneLINK, demographics.SSN, DATA_FILE, "AN", base.ScriptID, "State", DocumentHandling.CentralizedPrintingDeploymentMethod.dmLetter, "03", DocumentHandling.Barcode2DLetterRecipient.lrBorrower);
            MessageBox.Show("The letter has been created.");
            File.Delete(DATA_FILE);
        }//CreateLetter()
    }//class
}//namespace
