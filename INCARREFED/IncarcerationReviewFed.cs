using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace INCARREFED
{
    public class IncarcerationReviewFed : FedScript
    {
        public IncarcerationReviewFed(ReflectionInterface ri)
            : base(ri, "INCARREFED")
        {

        }

        public override void Main()
        {
            while (true)
            {
                FastPath("TX3Z/ITX6XJL;01");

                if (CheckForText(1, 72, "tXX6Y"))
                {
                    MessageBox.Show("There are no task to process.  The script will now end.");
                    EndDllScript();
                }

                if (CheckForText(8, 75, "W"))
                    MessageBox.Show("You already have a queue task open.  The script will continue to process this task.");

                string bwrSsn = GetText(8, 6, 9);
                PutText(21, 18, "01", ReflectionInterface.Key.Enter);

                FastPath("TX3Z/ITD2A");
                PutText(4, 16, bwrSsn);
                PutText(11, 65, "KJAIL", ReflectionInterface.Key.Enter);

                if (CheckForText(1, 72, "TDX2B"))
                {
                    MessageBox.Show("Unable to find a KJAIL ARC.  The script will now end.");
                    EndDllScript();
                }

                if (CheckForText(15, 2, "COMPL", "CANCL"))
                {
                    MessageBox.Show("It appears that the associated queue task may have already been worked.  The script will now end.");
                    EndDllScript();
                }

                List<string> commentText = (GetText(17, 2, 78) + GetText(18, 2, 78) + GetText(19, 2, 78) + GetText(20, 2, 78)).SplitAndRemoveQuotes(",");

                CommentData cData = new CommentData()
                {
                    IsBorrower = !commentText[0].Contains("ENDORSER/STUDENT"),
                    StudentAccountNumber = commentText[1],
                    BorrowerName = commentText[2],
                    FacilityName = commentText[3],
                    FacilityAddress = commentText[4],
                    FacilityCity = commentText[5],
                    FacilityState = commentText[6],
                    FacilityZip = commentText[7],
                    FacilityPhone = commentText[8],
                    InmateNumber = commentText[9],
                    ReleaseDate = commentText[10].IsNullOrEmpty() ? DateTime.Now : DateTime.Parse(commentText[10]),
                    Source = commentText[11],
                    OtherInfo = commentText[12].Replace("{INCARNOTFD}", ""),
                };

                FastPath("TX3Z/ITX1J");
                PutText(5, 16, "B");
                PutText(6, 16, bwrSsn, ReflectionInterface.Key.Enter, true);
                cData.BorrowerAccountNumber = GetText(3, 34, 12).Replace(" ", "");

                using (BorrowerReview bReview = new BorrowerReview(cData))
                {
                    if (bReview.ShowDialog() == DialogResult.Cancel)
                        EndDllScript();
                }

                FastPath(string.Format("TX3Z/ITS24{0}", cData.BorrowerAccountNumber));

                if (double.Parse(GetText(19, 42, 10).Replace(",", "")) <= 0.00)
                {
                    CloseTheTask();
                    if (MessageBox.Show("The account selected has no active balance. would you like to process another task?") == DialogResult.Yes)
                        continue;

                    EndDllScript();
                }

                UpdateTheSystem(cData);

                string comment = string.Format("{0},{1}, incarcerated at {2} until {3:MM-dd-yyyy} follow up date on {4:MM-dd-yyyy}", cData.IsBorrower ? "BORROWER" : "ENDORSER/STUDENT", cData.IsBorrower ? cData.BorrowerName : cData.StudentAccountNumber,
                    cData.FacilityName, cData.ReleaseDate, cData.FollowUpDate);

                bool updatedTd22 = Atd22AllLoans(cData.BorrowerAccountNumber, "KPRIS", comment, bwrSsn, ScriptId, false);

                if (!DataAccess.AddFutureDatedTask(cData))
                {
                    MessageBox.Show("The script was unable to add a record in the ArcAdd Database.  Please contact Systems Support to have the record added.");
                }
                if (!updatedTd22)
                {
                    MessageBox.Show("The script was unable to add a comment in TD22.  Please add the comment manually.");
                }

                CloseTheTask();

                if (MessageBox.Show("Would you like to process another task?", "New Task", MessageBoxButtons.YesNo) == DialogResult.No)
                    break;
            }//End while true

            MessageBox.Show("Processing Complete.");
            EndDllScript();
        }

        private void CloseTheTask()
        {
            FastPath("TX3Z/ITX6XJL;01");
            PutText(21, 18, "01", ReflectionInterface.Key.F2);
            PutText(8, 19, "C");
            PutText(9, 19, "COMPL", ReflectionInterface.Key.Enter);
        }

        private void UpdateTheSystem(CommentData cData)
        {
            FastPath("TX3Z/CTX1J");

            PutText(5, 16, cData.IsBorrower ? "B" : "E");
            PutText(6, 16, " ", true);
            PutText(6, 20, " ", true);
            PutText(6, 23, " ", true);
            PutText(6, 61, cData.IsBorrower ? cData.BorrowerAccountNumber : cData.StudentAccountNumber, ReflectionInterface.Key.Enter);

            Hit(ReflectionInterface.Key.F6);
            Hit(ReflectionInterface.Key.F6);

            PutText(11, 55, "N");
            PutText(10, 32, DateTime.Now.ToString("MMddyy"), ReflectionInterface.Key.Enter);

            if (!CheckForText(23, 2, "01096"))
            {
                MessageBox.Show("The address could not be updated on the account.");
                EndDllScript();
            }

            PutText(11, 10, cData.FacilityName, true);
            PutText(12, 10, string.Format("{0}  {1}", cData.FacilityAddress, cData.InmateNumber), true);
            PutText(14, 8, cData.FacilityCity, true);
            PutText(14, 32, cData.FacilityState, true);
            PutText(11, 55, "Y");

            if (cData.IsBorrower)
                PutText(8, 18, "44");

            PutText(10, 32, DateTime.Now.ToString("MMddyy"));

            PutText(14, 40, cData.FacilityZip, ReflectionInterface.Key.Enter, true);

            if (!CheckForText(23, 2, "01096"))
            {
                MessageBox.Show("The address could not be updated on the account.");
                EndDllScript();
            }

            Hit(ReflectionInterface.Key.F6);
            PutText(16, 14, "H", ReflectionInterface.Key.Enter);

            if (!CheckForText(23, 2, "01103"))
            {
                PutText(16, 45, DateTime.Now.ToString("MMddyy"));
                PutText(17, 54, "N");
                PutText(19, 14, "44", ReflectionInterface.Key.Enter);

                if (!CheckForText(23, 2, "01097"))
                {
                    MessageBox.Show("The phone number could not be updated on the account.");
                    EndDllScript();
                }
            }

            PutText(16, 20, "U");
            PutText(16, 30, "N");
            PutText(16, 45, DateTime.Now.ToString("MMddyy"));
            PutText(17, 14, cData.FacilityPhone.Replace("-", ""));
            PutText(17, 54, "Y");

            PutText(19, 14, "44", ReflectionInterface.Key.Enter);

            if (!CheckForText(23, 2, "01097") && !CheckForText(23,2,"01100"))
            {
                MessageBox.Show("The phone number could not be updated on the account.");
                EndDllScript();
            }
        }//End UpdateTheSystem
    }
}
