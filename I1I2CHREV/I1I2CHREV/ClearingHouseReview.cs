using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace I1I2CHREV
{
    public class ClearingHouseReview : ScriptBase
    {
        public ClearingHouseReview(ReflectionInterface ri)
            : base(ri, "I1I2CHREV")
        { }

        public override void Main()
        {
            ValidateRegion(DataAccessHelper.Region.Uheaa);

            using (var intro = new IntroForm())
                if (intro.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                    EndDllScript();

            string queue = null;
            do
            {
                queue = "I2";
                if (!CheckQueueForTasks(queue))
                {
                    queue = "I1";
                    if (!CheckQueueForTasks(queue))
                        ProcessingComplete("There are no tasks left to work in either the I1 or I2 queues.");
                }
                //select task by placing 01 on screen and hitting Enter
                RI.PutText(21, 18, "01", ReflectionInterface.Key.Enter);

                LocationStatus status = LocationStatus.Cancel;
                Demographics demographics = Demographics.FromTS24(RI);
                //get the student information if the borrower only has PLUS loans
                status = PlusLoanBorrowerDemo(status, demographics);

                switch (status)
                {
                    case LocationStatus.Cancel:
                        EndDllScript();
                        break;
                    case LocationStatus.Locate: //update borrower demographics if the user clicked Locate
                        Locate(queue, demographics);
                        break;
                    case LocationStatus.NotFound: //send a letter to the school or add a queue task if the borrower was not found on clearinghouse
                        CreateKLSLTSchoolLtr(demographics, queue);
                        break;
                    case LocationStatus.NoLocate: //add and activity record if the borrower was found on clearinghouse but not located
                        NoLocate(queue, demographics);
                        break;
                }
            } while (CompleteTask(queue));
            EndDllScript();

        }

        /// <summary>
        /// Checks to see if all the loans are plus and if there is a tilp loan.
        /// </summary>
        /// <param name="status"></param>
        /// <param name="demographics"></param>
        /// <returns>LocationStatus enum</returns>
        private LocationStatus PlusLoanBorrowerDemo(LocationStatus status, Demographics demographics)
        {
            if (demographics.SchoolInfo.AllLoansArePlus)
            {
                Dialog.Info.Ok("The borrower only has PLUS loans so the script will generate letters to the schools which the student(s) attended.", "PLUS Loans Only");
                //demographics already loaded the initial students
                if (demographics.StudentInfo.Students.Any())
                    status = LocationStatus.Locate;
            }
            else
            {
                if (demographics.SchoolInfo.AtLeastOneLoanIsTilp)
                    Dialog.Info.Ok("The borrower has TILP loans.  Please review Clearinghouse and if the information on Clearinghouse is the same information on COMPASS then please call the College of Education.", "TILP Loans Found");
                else
                    Dialog.Info.Ok("Please review Clearinghouse.", "FFELP Loans Only");

                RI.FastPath("TX3Z/CTX1JB;" + demographics.Ssn);

                using (var mainForm = new MainForm(demographics))
                    status = mainForm.ShowDialog();
            }
            return status;
        }

        /// <summary>
        /// Update LP22 with updated demos
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="demographics"></param>
        private void Locate(string queue, Demographics demographics)
        {
            if (!demographics.SchoolInfo.SkipOneLink && !demographics.SchoolInfo.AtLeastOneLoanIsTilp)
            {
                //update LP22 if they are in OneLink
                if (RI.BorrowerExistsInOnelink(demographics.Ssn))
                    UpdateLP22(demographics);
            }
            UpdateTX1J(demographics);

            if (!Atd22AllLoans(demographics.Ssn, "KS2CH", "", demographics.Ssn, ScriptId, false))
            {
                string message = string.Format("Error adding KS2CH ARC to Borrower: {0}", demographics.Ssn);
                ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
            }

            RI.FastPath("TX3ZITS24" + demographics.Ssn);
            if (!CheckForText(13, 73, "VALID"))
                CreateKLSLTSchoolLtr(demographics, queue);
        }

        /// <summary>
        /// Updates LP22 if there are no tilp loans
        /// </summary>
        /// <param name="demographics"></param>
        private void UpdateLP22(Demographics demographics)
        {
            RI.FastPath("LP22C" + demographics.Ssn);
            RI.PutText(3, 9, "M");
            RI.PutText(10, 9, demographics.Address1, true);
            RI.PutText(11, 9, demographics.Address2, true);
            RI.PutText(12, 9, demographics.City, true);
            RI.PutText(12, 60, demographics.Zip, true);
            if (!string.IsNullOrEmpty(demographics.Country))
            {
                RI.PutText(11, 55, demographics.Country, true);
                RI.PutText(12, 52, "FC");
            }
            else
            {
                RI.PutText(11, 55, "", true);
                RI.PutText(12, 52, demographics.State);
            }
            RI.PutText(10, 57, "Y", ReflectionInterface.Key.Enter);
            if (!RI.CheckForText(22, 3, "46012"))
            {
                Dialog.Warning.Ok("Fix the problem and then press <Insert> to continue.", "Fix Problem");
                RI.PauseForInsert();
            }
            RI.Hit(ReflectionInterface.Key.F6);

            if (!AddCommentInLP50(demographics.Ssn, "AM", "96", "KGNRL", "successful ch review", ScriptId))
            {
                string message = string.Format("Error leave KGNRL Action Code for borrower: {0}", demographics.Ssn);
                ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
            }
        }

        /// <summary>
        /// Updates TX1J
        /// </summary>
        /// <param name="demographics"></param>
        private void UpdateTX1J(Demographics demographics)
        {
            RI.FastPath("TX3Z/CTX1JB;" + demographics.Ssn);
            RI.Hit(ReflectionInterface.Key.F6);
            RI.Hit(ReflectionInterface.Key.F6);

            RI.PutText(8, 18, "0104"); //source & 3rd party
            RI.PutText(10, 32, DateTime.Now.ToString("MMddyy")); //last ver date
            RI.PutText(11, 10, demographics.Address1, true);
            RI.PutText(11, 55, "Y"); //validity indicator
            RI.PutText(12, 10, demographics.Address2, true);
            RI.PutText(14, 8, demographics.City, true);
            if (string.IsNullOrEmpty(demographics.Country))
            {
                RI.PutText(13, 52, "", true);
                RI.PutText(12, 77, "", true);
                RI.PutText(14, 32, demographics.State, true);
            }
            else
            {
                RI.PutText(14, 32, "", true);
                RI.PutText(12, 52, "", true);
                RI.PutText(13, 52, demographics.Country, true);
            }
            RI.PutText(14, 40, demographics.Zip, true);

            RI.Hit(ReflectionInterface.Key.Enter); //commit changes
        }

        /// <summary>
        /// Processing for when there is no locate
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="demographics"></param>
        private void NoLocate(string queue, Demographics demographics)
        {
            Atd22AllLoans(demographics.Ssn, "KU2CH", "", "", ScriptId, false);
            RI.FastPath("TX3ZITS24" + demographics.Ssn);
            //if (!CheckForText(13, 73, "VALID"))
            CreateKLSLTSchoolLtr(demographics, queue);
        }

        /// <summary>
        /// Close the task by setting it to complete
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        private bool CompleteTask(string queue)
        {
            RI.FastPath("TX3Z/ITX6X" + queue);
            RI.PutText(21, 18, "01", ReflectionInterface.Key.F2);
            RI.PutText(8, 19, "CCOMPL", ReflectionInterface.Key.Enter);
            return Dialog.Info.YesNo("Would you like to work another task?", "Task Complete");
        }

        private void CreateKLSLTSchoolLtr(Demographics demos, string queue)
        {
            RI.FastPath("TX3ZITD3G" + demos.Ssn);
            string skipBeginDate = RI.GetText(20, 5, 8);
            //if blank skip begin date then assume current date
            if (string.IsNullOrEmpty(skipBeginDate))
                skipBeginDate = DateTime.Now.ToString("MMddyy");
            else
                skipBeginDate = skipBeginDate.Replace("/", "");
            RI.FastPath("TX3Z/ITD2A" + demos.Ssn);
            //enter ARC to search for
            RI.PutText(11, 65, "KLSLT");
            RI.PutText(21, 16, skipBeginDate);
            RI.PutText(21, 30, DateTime.Now.ToString("MMddyy"), ReflectionInterface.Key.Enter);
            //get the schools if the ARC is found
            if (RI.MessageCode != "01019")
                return;
            else if (!demos.SchoolInfo.AllLoansAreStopPursuit && demos.SchoolInfo.Schools.Any())
                ProcessPlusOnlyLetters(demos, queue);
            else if (!demos.SchoolInfo.Schools.Any())
            {
                using (var form = new ManualForm())
                {
                    form.ShowDialog();
                    if (form.LetterWasSent)
                        Atd22AllLoans(demos.Ssn, "KLSLT", "manual letter sent last attended school on NSLDS", demos.Ssn, ScriptId, false);
                    else
                        Atd22AllLoans(demos.Ssn, "KU2CH", "manual letter not sent to last attended school on NSLDS", demos.Ssn, ScriptId, false);
                }
            }
        }

        /// <summary>
        /// Process letters for plus only loans
        /// </summary>
        /// <param name="demos"></param>
        /// <param name="queue"></param>
        private void ProcessPlusOnlyLetters(Demographics demos, string queue)
        {
            List<School> letterSchools = new List<School>();
            //process letters for each plus only borrower student
            if (demos.SchoolInfo.AllLoansArePlus)
                foreach (Student student in demos.StudentInfo.Students)
                    letterSchools.AddRange(GenerateSchoolLetters(demos, student, student.SSN, "PL", queue));
            else //process letters for borrowers
                letterSchools.AddRange(GenerateSchoolLetters(demos, null, demos.Ssn, "", queue));
            //add a comment if there is one or warn the user if there are no letters
            if (letterSchools.Any(p => p != null))
            {
                string comment = "letter(s) mailed to school(s): " + string.Join(", ", letterSchools.Select(s => s.SchoolCode).ToArray());
                Atd22AllLoans(demos.Ssn, "KLSLT", comment, demos.Ssn, ScriptId, false);
            }
            else
                Dialog.Warning.Ok("No schools were found on LG29 or no schools were found to which letters have not already been sent so no school letters will be generated.", "No Schools on LG29");
        }

        /// <summary>
        /// Generates a letter for the school
        /// </summary>
        /// <returns></returns>
        public IEnumerable<School> GenerateSchoolLetters(Demographics demos, Student student, string ssn, string pl, string queue)
        {
            RI.FastPath("LG29I" + ssn);
            if (!CheckForText(1, 51, "STUDENT ENROLLMENT STATUS MENU")) //check to be sure they are on OneLINK
            {
                if (RI.CheckForText(20, 8, "SEL"))
                {
                    while (!RI.CheckForText(22, 3, "46004"))
                    {
                        GetSchoolDemos(demos);
                    }
                }
                else if (RI.CheckForText(1, 74, "DIS"))
                {
                    string schoolCode = RI.GetText(10, 71, 8);
                    if (!demos.SchoolInfo.Schools.Any(s => s.SchoolCode == schoolCode))
                        demos.SchoolInfo.Schools.Add(new School() { SchoolCode = schoolCode });
                }
            }
            string export = EnterpriseFileSystem.GetPath("I12_Tall_Team");
            if (student == null)
                student = new Student() { SSN = "XXXXXXXXX" };
            //get info for schools that haven't received letters if there are any
            foreach (var school in demos.SchoolInfo.Schools.Where(p => p.SchoolCode.IsPopulated()))
            {
                CreateFileAndSave(demos, student, ssn, pl, queue, export, school);
                yield return school;
            }
        }

        /// <summary>
        /// Gets the demos for each school found
        /// </summary>
        /// <param name="demos"></param>
        private void GetSchoolDemos(Demographics demos)
        {
            for (int row = 9; row <= 18; row++)
            {
                if (string.IsNullOrEmpty(RI.GetText(row, 6, 1)))
                {
                    continue;
                }
                string schoolCode = RI.GetText(row, 14, 8);
                if (!demos.SchoolInfo.Schools.Any(s => s.SchoolCode == schoolCode))
                {
                    School school = new School() { SchoolCode = schoolCode };
                    demos.SchoolInfo.Schools.Add(school); //add amy missing schools
                }
            }
            RI.Hit(ReflectionInterface.Key.F8);
        }

        /// <summary>
        /// Creates the file that will be saved and used for printing
        /// </summary>
        private void CreateFileAndSave(Demographics demos, Student student, string ssn, string pl, string queue, string export, School school)
        {
            string comment = school.SchoolCode;
            var schoolDemos = school.GetDemographics(RI);
            string dataFilePath = string.Format("{0}I1I2CHREV_school_letter_data{1}.txt", EnterpriseFileSystem.TempFolder, Guid.NewGuid().ToString());
            using (var stream = new StreamWriter(dataFilePath))
            {
                string[] headers = new string[] {"SSN", "Queue", "FirstName", "MI", "LastName", "Address1", "Address2", "City", "State", "ZIP", "Country", "Phone", "AltPhone", "Email", "School", 
                    "SchName", "SchAdd", "SchAdd2", "SchAdd3", "SchCity", "SchST", "SchZip", "sSSN", "sName", "sAddress1", "sAddress2", "sAddress3", "sCitySTZIP", "sCountry", "sPhone"};
                stream.WriteLine(string.Join(",", headers));
                stream.WriteLine(string.Format("\"{0}\", \"{1}\", \"{2}\", \"{3}\", \"{4}\", \"{5}\", \"{6}\", \"{7}\", \"{8}\", \"{9}\", \"{10}\", \"{11}\", \"{12}\", \"{13}\", \"{14}\", \"{15}\", \"{16}\", \"{17}\", \"{18}\", \"{19}\", \"{20}\", \"{21}\", \"{22}\", \"{23}\", \"{24}\", \"{25}\", \"{26}\", \"{27}\", \"{28}\", \"{29}\"", "XXXXX" + ssn.Substring(5), queue, demos.Name, "", "", demos.Address1.ToUpper(), demos.Address2.ToUpper(), demos.City.ToUpper(),
                    (demos.State ?? "").ToUpper(), demos.Zip, (demos.Country ?? "").ToUpper(), demos.Phone, demos.AltPhone, demos.Email, school.SchoolCode, schoolDemos.Name, schoolDemos.Address.Trim(), schoolDemos.Address2.Trim(),
                    (schoolDemos.Address3 ?? "").Trim(), schoolDemos.City, schoolDemos.State, schoolDemos.Zip, "XXXXX" + student.SSN.Substring(5), student.Name ?? "", student.Address1 ?? "", student.Address2 ?? "", "",
                    student.CityStateZip ?? "", student.Country ?? "", student.Phone ?? ""));
            }
            string append = DateTime.Now.ToString("yyyyMMdd_hhmmss");
            //print cover letter
            DocumentProcessing.SaveDocs(pl.IsPopulated() ? "SCHLTRPL" : "SCHLTRCVR", dataFilePath, export + (pl.IsPopulated() ? "SCHLTRPL_" : "SCHLTRCVR_") + append + ".docx", true);
            //print detail
            DocumentProcessing.SaveDocs(pl.IsPopulated() ? "SCHLSTPL" : "SCHLTRLST", dataFilePath, export + (pl.IsPopulated() ? "SCHLSTPL_" : "SCHLTRLST_") + append + ".docx", true);

            Repeater.TryRepeatedly(() => File.Delete(dataFilePath));
        }

        /// <summary>
        /// Return true if queue exists
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        private bool CheckQueueForTasks(string queue)
        {
            FastPath("TX3Z/ITX6X" + queue);
            return RI.ScreenCode != "TXX6Y"; //being on TXX6Y means there are no tasks to work
        }

        /// <summary>
        /// Ends the application
        /// </summary>
        /// <param name="message"></param>
        private void ProcessingComplete(string message)
        {
            MessageBox.Show(message);
            EndDllScript();
        }
    }
}