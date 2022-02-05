using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.Scripts;
using Uheaa.Common.WinForms;

namespace REINSTREV
{
    public class ReinstatementReview : ScriptBase
    {
        public ReinstatementReview(ReflectionInterface ri) : base(ri, "REINSTREV") { }
        public override void Main()
        {
            string message = "If you wish to review the reinstatement eligibility of a specific borrower, enter the SSN or account number below and click OK.  Otherwise, just click OK to work tasks in the QREINSTA queue.";
            using (var input = new InputBox<AccountIdentifierTextBox>(message, "Enter Account Number or SSN"))
                if (input.ShowDialog() == DialogResult.OK)
                    ProcessSsn(input.InputControl.Text);
        }

        private void ProcessSsn(string ssn)
        {
            if (ssn.Length == 10) //convert account number to ssn
            {
                RI.FastPath("LP22I;");
                RI.PutText(6, 33, ssn, ReflectionInterface.Key.Enter);
                ssn = RI.GetText(3, 23, 9);
            }
            RI.FastPath("LP9ACQREINSTA");

            if (RI.AltMessageCode == "47423" || RI.AltMessageCode == "47420")
            {   //warn the user and end the script if all tasks are complete
                Dialog.Info.Ok("There are no more tasks in the QREINSTA queue.  Press OK to exit.", ScriptId);
                return;
            }

            //warn the user and end the script if the user has an unresolved task in another queue
            if (!RI.CheckForText(1, 9, "QREINSTA"))
            {
                string unresolvedQueue = RI.GetText(1, 9, 8);
                Dialog.Warning.Ok("You have an unresolved task in the " + unresolvedQueue + " queue.  You must complete the task before working the pdem review queue.");
                return;
            }

            if (!string.IsNullOrEmpty(ssn)) //no SSN selected
            {
                PageHelper.IteratePagesOnly(RI, (settings) =>
                {
                    string pageSsn = RI.GetText(17, 70, 9);
                    bool match = pageSsn == ssn;
                    settings.ContinueIterating = !match;
                });
                bool? response = Dialog.Info.YesNoCancel("Is this task for the correct borrower?  Click Yes to process the task.  Click No to pause the script so you can manually select the correct task (hit <Insert> to resume processing once the task is selected).  Click Cancel to quit.", ScriptId);
                if (response == null) //cancel
                    return;
                else if (response == false)
                    RI.PauseForInsert();
            }
            bool? processResult = ProcessTask();
            if (processResult == null) //cancel
                return;
            else if (processResult == true)
                if (Dialog.Info.YesNo("Processing for the task is complete and letters have been generated.  Do you wish to process another task?", ScriptId))
                    Main(); //round two (or higher)
        }

        private bool? ProcessTask()
        {
            Borrower b = new Borrower();
            if (!b.LoadFromLP9AC(RI))
                return false;

            //Access LC18
            bool? response = GetLC18Approval(b.Ssn);
            bool approved = response == true;
            if (response == null) //cancel
                return null;
            string comment = UpdateLC18(approved, b.Ssn);

            PrintScreens(b.Ssn);
            //add an activity record
            RI.AddCommentInLP50(b.Ssn, "LT", "04", "QRSTR", comment, ScriptId);
            CloseLP9ATask(b.Ssn);
            PrintLetters(b, approved);
            return true;
        }

        private bool? GetLC18Approval(string ssn)
        {
            RI.FastPath("LC18C" + ssn);
            if (!Dialog.Info.OkCancel("Click OK to pause the script so you can review the account and hit <Insert> when you are ready to resume processing or click Cancel to quit.", ScriptId))
                return null;
            RI.PauseForInsert();
            bool? response = Dialog.Info.YesNoCancel("Was the request approved?  Click Yes to update OneLINK and generate an approval letter.  Click No to update OneLINK and generate a denial letter.  Click Cancel to quit.", ScriptId);
            return response;
        }

        private string UpdateLC18(bool approved, string ssn)
        {
            //Update LC18
            if (!RI.CheckForText(1, 2, "LC18 C"))
                RI.FastPath("LC18C" + ssn);
            //update the reinstatement eligibility
            string code = "";
            string verb = "";
            if (approved)
            {
                code = "R";
                verb = "approved";
            }
            else if (RI.CheckForText(17, 67, "Y"))
            {
                verb = "denied";
                code = "N";
            }
            string comment;
            if (!string.IsNullOrEmpty(code))
            {
                RI.PutText(17, 67, code, true);
                RI.PutText(17, 69, DateTime.Now.ToString("MMddyyyy"));
                comment = "reviewed request for reinstatement from borrower, request " + verb + ", updated LC18 eligibility code to '" + code + "', completed task in QREINSTA queue, letters to collections for mailing to borrower and school";
            }
            else
                comment = "reviewed request for reinstatement from borrower, request denied, LC18 eligibility already correct, completed task in QREINSTA queue, letters to collections for mailing to borrower and school";

            RI.Hit(ReflectionInterface.Key.Enter); //post changes
            return comment;
        }

        private void PrintScreens(string ssn)
        {
            RI.Hit(ReflectionInterface.Key.F9); //print current screen

            RI.FastPath("LC41I" + ssn); //access LC41
            RI.PutText(7, 36, "X", ReflectionInterface.Key.Enter);
            while (RI.AltMessageCode != "46004" && RI.AltMessageCode != "47004" && !RI.CheckForText(21, 3, "47004"))
            {   //print each page
                RI.Hit(ReflectionInterface.Key.F9);
                RI.Hit(ReflectionInterface.Key.F8);
            }

            RI.FastPath("LC05I" + ssn); //print LC05 screens
            if (RI.AltMessageCode == "47004") //no records found
                return;
            //select the first loan
            RI.PutText(21, 13, "01", ReflectionInterface.Key.Enter);
            while (RI.AltMessageCode != "46004")
            {   //print each page
                RI.Hit(ReflectionInterface.Key.F9);
                RI.Hit(ReflectionInterface.Key.F8);
            }
        }

        private void CloseLP9ATask(string ssn)
        {
            //return to LP9A and complete the task
            RI.FastPath("LP9ACQREINSTA");
            //select the correct task
            while (!RI.CheckForText(17, 70, ssn))
                RI.Hit(ReflectionInterface.Key.F8);

            //complete the task
            RI.Hit(ReflectionInterface.Key.F6);
        }

        private void PrintLetters(Borrower b, bool approved)
        {
            string costCenter = "MA2329";
            string keyLine = DocumentProcessing.ACSKeyLine(b.Ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
            string dataLine = string.Join(",", new string[] {
                    "XXX-XX-" + b.Ssn.Substring(5), b.Demographics.LastName, b.Demographics.FirstName, b.Demographics.Address1, b.Demographics.Address2,
                    b.Demographics.City, b.Demographics.State, b.Demographics.ZipCode, b.Demographics.Country, keyLine, b.SchoolName, b.SchoolAddress1,
                    b.SchoolAddress2, "", b.SchoolCity, b.SchoolState, b.SchoolZip, b.SchoolCountry, b.Demographics.AccountNumber, costCenter});

            //print the letters
            EcorrProcessing.AddOneLinkRecordToPrintProcessing(ScriptId, approved ? "REINAPVB" : "REINDENB", dataLine, b.Demographics.AccountNumber, costCenter);
            EcorrProcessing.AddOneLinkRecordToPrintProcessing(ScriptId, approved ? "REINAPVS" : "REINDENS", dataLine, b.Demographics.AccountNumber, costCenter);
        }
    }
}
