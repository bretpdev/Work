using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace I1I2FEDREV
{
    public class I1I2ClearingHouseNSLDSReviewFed : FedScript
    {
        public I1I2ClearingHouseNSLDSReviewFed(ReflectionInterface ri)
            : base(ri, "I1I2FEDREV")
        {
        }

        public override void Main()
        {
            if (!Dialog.Info.OkCancel("This is the I1I2 Clearinghouse NSLDS Review Script.  You will need to be logged into NSLDS for this script."))
                EndDllScript();
            do
                AccessQueues("I2", "01");
            while (Dialog.Info.YesNo("Do you want to process another task?", "New Task"));

        }

        /// <summary>
        /// Selects a queue task to be processed.
        /// </summary>
        /// <param name="queue">Queue we are processing.</param>
        /// <param name="subQueue">Sub Queue we are processing.</param>
        private void ProcessQueue(string queue, string subQueue)
        {
            PutText(21, 18, "01", ReflectionInterface.Key.Enter);
            BorrowerInfo borrower = BorrowerInfo.Populate(RI);

            using (BorrowerDemo demo = new BorrowerDemo(borrower))
            {
                DialogResult result = demo.ShowDialog();
                if (result == DialogResult.Cancel)
                    EndDllScript();
                else if (result == DialogResult.Yes)
                {
                    UpdateDemos(borrower);
                    Atd22AllLoans(borrower.Ssn, "KSLDS", string.Empty, string.Empty, ScriptId, false);
                    if (borrower.HasValidPhone)
                        CloseTask(queue, subQueue);
                    else
                    {
                        if (!CheckTd2a(borrower.Ssn))
                            CloseTask(queue, subQueue);
                        else
                            SchoolProcessing(borrower.Ssn, queue, subQueue);
                    }
                }
                else//Assuming that the the user selected NO
                {
                    Atd22AllLoans(borrower.Ssn, "KULDS", string.Empty, string.Empty, ScriptId, false);
                    if (!CheckTd2a(borrower.Ssn))
                        CloseTask(queue, subQueue);
                    else
                        SchoolProcessing(borrower.Ssn, queue, subQueue);
                }
            }
        }

        /// <summary>
        /// Accesses TS26 and gets all of the current schools for each loan.  The user will then send an email to each school, 
        /// and it will add a comment indicating if an email was sent to each school.
        /// </summary>
        /// <param name="ssn">Borrower's SSN</param>
        /// <param name="queue">The Queue we are processing.</param>
        /// <param name="subqueue">The Sub Queue we are processing.</param>
        private void SchoolProcessing(string ssn, string queue, string subqueue)
        {
            List<SchoolInfo> schools = GetAllSchoolData(ssn);

            List<SchoolInfo> nonConsolSchool = schools.Where(p => p.SchoolCode != "07777800" && p.SchoolCode != "08888700").ToList();

            if (schools.Count == 0)
            {
                CloseTask(queue, subqueue);
                return;
            }

            if (schools.Where(p => p.SchoolCode == "07777800" || p.SchoolCode == "08888700").Count() > 0)
            {
                string message = "Review NSLDS to determine the school the borrower attended last, and send an email to the last school attended";
                Dialog.Info.Ok(message);
                bool sentEmail = Dialog.Info.YesNo("Was an email sent to the school gathered from NSLDS?");

                if (sentEmail)
                {
                    string comment = "email(s) sent to school(s): {Insert School gathered from NSLDS}  to request verification of borrower's demographic information";
                    Atd22AllLoans(ssn, "KEMLD", comment, "", ScriptId, true);
                }
                else
                {
                    string comment = "email(s) not sent to school(s): {{Insert School gathered from NSLDS}  Unable to find schools email address.";
                    Atd22AllLoans(ssn, "KNOEM", comment, "", ScriptId, true);
                }

            }

            if (nonConsolSchool.Count > 0)
            {
                Dialog.Info.Ok(string.Format("Send an email to the following schools: \n {0} \n Press OK once all emails have been sent.",
                    string.Join("\n", schools.Select(e => " - " + e.SchoolName).ToArray())));

                foreach (SchoolInfo school in schools)
                    school.EmailSent = Dialog.Info.YesNo(string.Format("Was an email sent to {0}?", school.SchoolName));

                List<SchoolInfo> emailSent = schools.Where(p => p.EmailSent).ToList();
                List<SchoolInfo> emailNotSent = schools.Where(p => !p.EmailSent).ToList();

                if (emailSent.Count != 0)
                {
                    string comment = string.Format("email(s) sent to school(s): {0}  to request verification of borrower's demographic information",
                        string.Join("\n", emailSent.Select(e => e.SchoolCode + " ").ToArray()));

                    Atd22AllLoans(ssn, "KEMLD", comment, "", ScriptId, false);
                }

                if (emailNotSent.Count != 0)
                {
                    string comment = string.Format("email(s) not sent to school(s): {0}  Unable to find schools email address.",
                        string.Join("\n", emailNotSent.Select(e => e.SchoolCode + " ").ToArray()));

                    Atd22AllLoans(ssn, "KNOEM", comment, "", ScriptId, false);
                }
            }

            CloseTask(queue, subqueue);
        }

        /// <summary>
        /// Accesses ITX0Y and gets the school name from the school code.
        /// </summary>
        /// <param name="schoolCode">School code to look up.</param>
        /// <returns>SchoolInfo Object with SchoolCode and SchoolName</returns>
        private SchoolInfo GetSchoolInfo(string schoolCode)
        {
            FastPath("TX3Z/ITX0Y" + schoolCode);
            string name = string.Empty;

            //Not sure why but the Spec indicates that the position changes based upon the screen...
            if (RI.ScreenCode == "TXX04")
                name = GetText(6, 14, 40);
            else
                name = GetText(6, 19, 50);

            return new SchoolInfo()
            {
                SchoolCode = schoolCode,
                SchoolName = name
            };

        }

        /// <summary>
        /// Accesses TS26 reviews all loans gathers the current school information for each loan.
        /// </summary>
        /// <param name="ssn">Borrower's SSN.</param>
        /// <returns>List with all School information for each loan the borrower has.  
        /// The list will not be duplicated, so if 3 loans have the same school only 1 record will be in the list.</returns>
        private List<SchoolInfo> GetAllSchoolData(string ssn)
        {
            List<SchoolInfo> schools = new List<SchoolInfo>();
            FastPath("TX3Z/ITS26" + ssn);
            if (RI.ScreenCode == "TSX29")
                schools.Add(GetSchoolInfo(GetText(13, 18, 8)));
            else
            {
                //loop though all of the loans on TS26
                for (int row = 8; RI.MessageCode != "90007"; row++)
                {
                    if (row > 21 || CheckForText(row, 3, " "))
                    {
                        row = 7;
                        Hit(ReflectionInterface.Key.F8);
                        continue;
                    }

                    int page = GetText(2, 68, 4).ToInt();
                    PutText(21, 12, GetText(row, 2, 3), ReflectionInterface.Key.Enter, true);

                    string schoolCode = GetText(13, 18, 8);

                    if (!schools.Any(p => p.SchoolCode == schoolCode))
                        schools.Add(GetSchoolInfo(schoolCode));

                    FastPath("TX3Z/ITS26" + ssn);
                    while (page != GetText(2, 74, 3).ToInt())
                        Hit(ReflectionInterface.Key.F8);
                }
            }

            return schools;
        }

        /// <summary>
        /// Closes a given task.
        /// </summary>
        /// <param name="queue">Queue to close.</param>
        /// <param name="subQueue">SubQueue to close.</param>
        private void CloseTask(string queue, string subQueue)
        {
            FastPath("TX3Z/ITX6X" + queue + subQueue);
            PutText(21, 18, "01", ReflectionInterface.Key.F2);
            PutText(8, 19, "C");
            PutText(9, 19, "COMPL", ReflectionInterface.Key.Enter);

        }

        /// <summary>
        /// Checks TD2a to see if the borrower has a KEMLD arc within the past 30 days.
        /// </summary>
        /// <param name="ssn">Borrower's Ssn</param>
        /// <returns>True if they have a KEMLD ARC within the past 30 days.  False if they do not.</returns>
        private bool CheckTd2a(string ssn)
        {
            FastPath("TX3Z/ITD2A");
            PutText(4, 16, ssn);
            PutText(11, 65, "KEMLD");
            PutText(21, 16, DateTime.Now.AddDays(-30).ToString("MMddyy"));
            PutText(21, 30, DateTime.Now.ToString("MMddyy"), ReflectionInterface.Key.Enter);
            return RI.MessageCode == "01019";
        }

        /// <summary>
        /// Accesses CTX1J and updates the borrowers demographics.
        /// </summary>
        /// <param name="borrower">Object with Borrower Demographics.</param>
        private void UpdateDemos(BorrowerInfo borrower)
        {
            FastPath("TX3Z/CTX1JB" + borrower.Ssn);
            Hit(ReflectionInterface.Key.F6);
            Hit(ReflectionInterface.Key.F6);
            PutText(8, 18, "01");
            PutText(9, 18, "04");
            PutText(10, 32, DateTime.Now.ToString("MMddyy"));
            PutText(11, 55, "Y");
            PutText(11, 10, borrower.Street1, true);
            PutText(12, 10, borrower.Street2, true);

            if (borrower.State.IsNullOrEmpty())
            {
                PutText(14, 32, "", true);
                PutText(13, 52, borrower.Country, true);
            }
            else
            {
                PutText(14, 32, borrower.State, true);
                PutText(13, 52, "", true);
                PutText(12, 52, "", true);
                PutText(12, 77, "", true);
            }

            PutText(14, 8, borrower.City, true);
            PutText(14, 40, borrower.Zip, true);

            Hit(ReflectionInterface.Key.Enter);
        }

        /// <summary>
        /// Accesses A given queue and sub queue (This is a Recursive method)
        /// </summary>
        /// <param name="queue">Queue to Access</param>
        /// <param name="subqueue">SubQueue to Access</param>
        /// <param name="final">Indicator for the code to know when to stop Recursing.</param>
        private void AccessQueues(string queue, string subqueue, bool final = false)
        {
            FastPath(string.Format("TX3Z/ITX6X{0};{1}", queue, subqueue));
            if (RI.MessageCode == "01020")
            {
                if (final)
                {
                    Dialog.Info.Ok("There are no queues to process.");
                    EndDllScript();
                }

                AccessQueues("I1", "SC", true);
            }
            else
                ProcessQueue(queue, subqueue);
        }
    }
}
