using DPLTRS.DataObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.Scripts;
using Uheaa.Common.WinForms;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;

namespace DPLTRS
{
    public class LetterHelper
    {
        public LetterHelper(ReflectionInterface ri, string scriptId)
        {
            ScriptId = scriptId;
            RI = ri;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
        }
        private ReflectionInterface RI;
        private string ScriptId;

        public SystemBorrowerDemographics PromptForBorrower()
        {
            using (var input = new InputBox<AccountIdentifierTextBox>("Please enter SSN or Account Number", ScriptId + " - Account Identifier"))
                if (input.ShowDialog() == DialogResult.OK)
                    return ResolveBorrower(input.InputControl.Text);
                else
                    return null;
        }
        public SystemBorrowerDemographics ResolveBorrower(string accountIdentifier)
        {
            string ssn = GetSsn(accountIdentifier);
            if (ssn == null)
            {
                Dialog.Warning.Ok("Couldn't find borrower with identifier " + accountIdentifier);
                return null;
            }

            return RI.GetDemographicsFromLP22(ssn);
        }

        private string GetSsn(string accountIdentifier)
        {
            if (accountIdentifier.Length == 10) //convert account number to ssn
            {
                RI.FastPath("LP22I;");
                RI.PutText(6, 33, accountIdentifier, ReflectionInterface.Key.Enter);
                if (RI.CheckForText(22,3, "47004"))
                    return null;
                accountIdentifier = RI.GetText(3, 23, 9);
            }
            return accountIdentifier;
        }

        public bool LC05Check(SystemBorrowerDemographics demos)
        {
            RI.FastPath("LC05I" + demos.Ssn);
            if (RI.CheckForText(22,3,"47004"))
            {
                Dialog.Info.Ok("Borrower does not have an LC05 record.");
                return false;
            }
            return true;
        }

        public bool DefaultAndEDCheck(SystemBorrowerDemographics demos)
        {
            bool inDefault = false;
            bool notAssignedToED = false;
            var settings = PageHelper.IterationSettings.Default();
            settings.MinRow = 7;
            settings.RowIncrementValue = 3;
            PageHelper.Iterate(RI, (row, s) =>
            {
                string sel = RI.GetText(row, 2, 2);
                RI.PutText(21, 13, sel, Key.Enter);
                if (RI.CheckForText(4, 10, "03")) //loan in default
                    inDefault = true;
                if (RI.CheckForText(19, 73, "MMDDCCYY"))
                    notAssignedToED = true;
                RI.Hit(Key.F12); //return to previous selection screen
            }, settings);

            if (!inDefault)
                Dialog.Info.Ok("Borrower is not in default.");
            else if (!notAssignedToED) //AKA Assigned to ED
                Dialog.Info.Ok("Borrower has been assigned to ED.");
            else
                return true;
            return false;
        }

        public void CreateLetter(SystemBorrowerDemographics demos, string letterId, IEnumerable<string> additionalHeaders = null, IEnumerable<string> additionalValues = null)
        {
            //Create LetterData
            string letterData = "";
            string keyLine = DocumentProcessing.ACSKeyLine(demos.Ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
            //Header: "BRWR_DF_SPE_ACC_ID,KeyLine,FirstName,LastName,Address1,Address2,City,State,Zip,Country"
            string[] letterDataArray = new string[] { demos.AccountNumber.Replace(" ",""), keyLine, demos.FirstName.Trim(), demos.LastName.Trim(),
                    demos.Address1.Trim(), demos.Address2.Trim(), demos.City.Trim(), demos.State.Trim(), demos.ZipCode.Trim(), demos.Country.IsNullOrEmpty() ? " " : demos.Country.Trim()};
            //Additional Headers(To Date): "DueDate"
            if (additionalValues != null)
            {
                string line = string.Join(",", letterDataArray.Union(additionalValues).ToArray());
                for (int num = 1; num < 9; num++)
                {
                    line = line.Replace(string.Format("removeValue{0}", num), "");
                }
                letterData = line;
            }
            else
            {
                letterData = string.Join(",", letterDataArray.ToArray());
            }
            EcorrProcessing.AddRecordToPrintProcessing(ScriptId, letterId, letterData, demos.AccountNumber.Replace(" ", ""), "MA2329");
        }

        public string GetUserFromLP40()
        {
            RI.FastPath("LP40I");
            if(!RI.CheckForText(1,77,"ANCE"))
            {
                RI.Hit(Key.Enter);
            }

            return RI.GetText(3, 14, 7);
        }

    }
}
