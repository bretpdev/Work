using ENRLINFO.Forms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace ENRLINFO
{
    public class OnelinkProcessingHelper
    {
        public enum ExistingLG29Result
        {
            GetAdditonalInfo,
            Failure,
            Success
        }

        public ReflectionInterface ri { get; set; }
        public ProcessLogRun logRun { get; set; }
        public OnelinkProcessingHelper(ReflectionInterface ri, ProcessLogRun logRun)
        {
            this.ri = ri;
            this.logRun = logRun;
        }

        public LP90Data ProcessLP90(SystemBorrowerDemographics demos, EnrollmentData data)
        {
            List<QueueTask> queueTasks = new List<QueueTask>();

            bool oneLP90 = false;
            ri.FastPath($"LP9OI{demos.Ssn}");
            //Multiple queue tasks
            if (ri.CheckForText(1, 63, "ACTIVITY SELECTION"))
            {
                //for i from 0-12
                for(int row = 0; row < 13; row++)
                {
                    if(ri.GetText(row + 8, 6, 2).Trim() == "")
                    {
                        row = 0;
                        ri.Hit(ReflectionInterface.Key.F8); //Go to next page
                        if(ri.CheckForText(22,3,"46004")) //No more data
                        {
                            break;
                        }
                    }

                    //Check for queue
                    if(ri.CheckForText(row + 8, 11, "EVSNTFLU"))
                    {
                        //if the queue is found
                        //get the create time
                        QueueTask queueTask = new QueueTask();
                        queueTask.CreatedAt = ri.GetText(row + 8, 72, 4);
                        //get the school
                        ri.PutText(21, 12, ri.GetText(row + 8, 6, 2), ReflectionInterface.Key.Enter);
                        queueTask.School = ri.GetText(9, 25, 8);
                        if(queueTask.School.ToIntNullable() == null) //unable to parse the school
                        {
                            queueTask.School = "0";
                        }
                        if(queueTask.School == data.SchoolCode)
                        {
                            queueTask.MatchesSchoolCode = true;
                        }
                        else
                        {
                            queueTask.MatchesSchoolCode = false;
                        }
                        queueTasks.Add(queueTask);
                        ri.Hit(ReflectionInterface.Key.F12);
                    }
                }
                oneLP90 = false; //There are multiple records on LP90
            }
            else if (ri.CheckForText(22, 3, "49201") || ri.CheckForText(22, 3, "47004"))
            {
                //49201 NO TASK PRESNET FOR KEY DATA ENTERED
                logRun.AddNotification($"No EVSNTFLU task found for borrower. Account Number {demos.AccountNumber}", NotificationType.ErrorReport, NotificationSeverityType.Warning);
            }
            else
            {
                //only one queue task
                oneLP90 = true;
            }

            //Order the queue tasks by time
            queueTasks = queueTasks.OrderBy(p => p.CreatedAt.ToInt()).ToList();
            //check that no record with the school code matching the one provided has a duplicate by time
            bool hasDuplicates = queueTasks.Where(p => p.School == data.SchoolCode).DistinctBy(p => p.CreatedAt).Count() < queueTasks.Where(p => p.School == data.SchoolCode).Count();
            return new LP90Data() { QueueTasks = queueTasks, OneQueue = oneLP90, DuplicateTimes = hasDuplicates};
        }

        public bool CancelQueuesLP8Y(SystemBorrowerDemographics demos, LP90Data data)
        {
            ri.FastPath($"LP8YCSCR;EVSNTFLU;;{demos.Ssn}");

            //review each task
            if(data.QueueTasks.Count == 1 || data.OneQueue)
            {
                //only one queue task was found on LP90
                if(ri.GetText(1,75,3) == "DET")
                {
                    int row = 7;
                    while(ri.GetText(22,3,5) != "46004")
                    {
                        //Cancel available tasks
                        if(ri.CheckForText(row, 33,"A"))
                        {
                            ri.PutText(row, 33, "X");
                        }
                        //Make tasks available that are being worked and refresh the screen
                        if(ri.CheckForText(row,33,"W"))
                        {
                            ri.PutText(row, 33, "A");
                            ri.Hit(ReflectionInterface.Key.F6);
                            ri.FastPath($"LP8YCSCR;EVSNTFLU;;{demos.Ssn}");
                            row = 6;
                        }
                        //increment the row counter
                        row++;
                        //go to the next page if the line is blank
                        if(ri.GetText(row,33,1).Trim() == "")
                        {
                            ri.Hit(ReflectionInterface.Key.F8);
                            row = 7;
                        }
                    }
                    //post the changes and stop looking if no more tasks are found
                    ri.Hit(ReflectionInterface.Key.F6);
                }
            }
            if(data.QueueTasks.Count > 1 && !data.DuplicateTimes)
            {
                //if multiple tasks are found in LP90
                if(ri.CheckForText(1,75,"DET"))
                {
                    int row = 6;
                    int page = 0;
                    for(int i = 1; i < data.QueueTasks.Count; i++)
                    {
                        int iOffset = (i % (((page + 1) * 14) + 1));
                        if (data.QueueTasks[i].MatchesSchoolCode)
                        {
                            //Check if we need to move to a new page
                            int newPage = (i - 1) / 14;
                            if (newPage > page)
                            {
                                //page the amount of times for the records skipped
                                for (int j = 0; j < newPage - page; j++)
                                {
                                    //go to the next page if the line is blank
                                    if (ri.GetText(row + iOffset, 33, 1).Trim() == "")
                                    {
                                        ri.Hit(ReflectionInterface.Key.F8);
                                        page++;
                                        //post the changes and stop looking if no more tasks are found
                                        if (ri.CheckForText(22, 3, "46004"))
                                        {
                                            logRun.AddNotification($"Error, Account number {demos.AccountNumber}, received screen code {ri.GetText(22, 3, 30)}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                                            Dialog.Error.Ok($"Error! Contact Systems Support. {ri.GetText(22, 3, 30)}");
                                            return false;
                                        }
                                        row = 6;
                                    }
                                }
                            }
                            //cancel available tasks
                            if(ri.CheckForText(row + iOffset, 33, "A"))
                            {
                                ri.PutText(row + iOffset, 33, "X");
                            }
                        }
                    }
                    ri.Hit(ReflectionInterface.Key.Enter);
                    ri.Hit(ReflectionInterface.Key.F6);
                }
            }
            else if(data.QueueTasks.Count == 0)
            {
                //no queue tasks are found
                logRun.AddNotification($"No queue tasks found.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }
            return true;
        }

        public void HandleLG29(SystemBorrowerDemographics demos, EnrollmentData data, AdditionalEnrollmentData additionalData, string scriptId)
        {
            ri.FastPath($"LG29C{demos.Ssn};{data.SchoolCode}");
            //review/update enrollment infomration if it already exists for borrower/school
            if (!ri.CheckForText(5, 22, "SSN"))
            {
                //select the row if the target screen is not displayed
                if (ri.CheckForText(20, 8, "SEL"))
                {
                    int row = 9;
                    //find the row with the greatest cert date
                    DateTime greatestCertDate = DateTime.MinValue;
                    while (!ri.CheckForText(22, 3, "46004"))
                    {
                        DateTime currentRecord = ri.GetText(row, 51, 8).ToDateFormat().ToDate();
                        if (currentRecord > greatestCertDate)
                        {
                            greatestCertDate = currentRecord;
                        }
                        row++;
                        if (ri.GetText(row, 6, 1).Trim() == "")
                        {
                            ri.Hit(ReflectionInterface.Key.F8);
                            row = 9;
                        }
                    }
                    //go back to page one
                    while (!ri.CheckForText(22, 3, "46003"))
                    {
                        ri.Hit(ReflectionInterface.Key.F7);
                    }
                    //select the row
                    row = 9;
                    while (!ri.CheckForText(row, 51, greatestCertDate.ToString("MMddyyyy")))
                    {
                        row++;
                        if (ri.GetText(row, 6, 1).Trim() == "")
                        {
                            ri.Hit(ReflectionInterface.Key.F8);
                            row = 9;
                        }
                    }
                    bool matchingEnrollmentStatus = ri.GetText(row, 25, 1) == additionalData.EnrollmentStatus;
                    bool matchingStatusDate = ri.GetText(row, 29, 8) == additionalData.StatusEffectiveDate.ToString("MMddyyyy");
                    bool matchingAGD = ri.GetText(row, 40, 8) == additionalData.AGD.ToString("MMddyyyy");
                    if (matchingEnrollmentStatus && matchingStatusDate && matchingAGD)
                    {
                        //Add arc for no update
                        string comment = $"{data.SchoolCode} - {data.SourceText} no update";
                        string warning = "The data already exists on OneLINK.";
                        AddComment(demos, comment, scriptId, warning);
                    }
                    else
                    {
                        string sel = ri.GetText(row, 5, 2);
                        ri.PutText(20, 19, sel, ReflectionInterface.Key.Enter);
                        //ri.Hit(ReflectionInterface.Key.Enter);
                        //add an activity record and go to the next task if the info cannot be updated
                        if (!ri.CheckForText(21, 3, "46011"))
                        {
                            string comment = $"{data.SchoolCode} - {data.SourceText} data cannot be updated due to status of loans";
                            AddComment(demos, comment, scriptId);
                        }
                        //updateLG29
                        else
                        {
                            UpdateLG29(demos, data, additionalData, scriptId);
                        }

                    }
                }
            }
            //add enrollment information if none exists for borrower/school
            else
            {
                ri.PutText(1, 7, "A", ReflectionInterface.Key.Enter);
                if(!ri.CheckForText(21,3,"44000"))
                {
                    string comment = $"{data.SchoolCode} - {data.SourceText} data cannot be updated due to status of loans";
                    AddComment(demos, comment, scriptId);
                }
                else
                {
                    UpdateLG29(demos, data, additionalData, scriptId);
                }
            }
        }


        public void HandleExistingLG29(SystemBorrowerDemographics demos, EnrollmentData data, DateTime guarenteeDate, string scriptId)
        {
            ri.FastPath($"LG29C{demos.Ssn};{data.SchoolCode}");
            //review/update enrollment information if it already exists for borrower/school
            if(!ri.CheckForText(5,22,"SSN"))
            {
                //instruct the user to review the information
                Dialog.Warning.Ok("Compare the EVR to the information on OneLINK to determine the missing information. Hit Insert when you are ready to continue.", "Review for Missing Information");
                //pause the script for the user to review info
                ri.PauseForInsert();
                var result = Dialog.Warning.YesNo($"The oldest Guarentee date is {guarenteeDate.ToString("MM/dd/yyyy")} would you like to add information to OneLINK after this date?", "Add Information?");
                if(result == true)
                {
                    var additionalInfo = GetAdditionalInfo();
                    while (additionalInfo != null)
                    {//var additionalInfo = GetAdditionalInfo();

                        ri.FastPath($"LG29C{demos.Ssn};{data.SchoolCode}");
                        //select the first record
                        if (ri.CheckForText(20, 8, "SEL"))
                        {
                            ri.PutText(20,19,"01",ReflectionInterface.Key.Enter);
                        }
                        //add an activity record and go to the next task if the info cannot be updated
                        if (!ri.CheckForText(21, 3, "46011"))
                        {
                            string comment = $"{data.SchoolCode} - {data.SourceText} data cannot be updated due to loan status";
                            AddComment(demos, comment, scriptId);
                        }
                        else
                        {
                            data.Source = "H";
                            UpdateLG29(demos, data, additionalInfo, scriptId);
                        }
                        additionalInfo = GetAdditionalInfo();
                    }
                }
                else
                {
                    //no update
                    string comment = $"{data.SchoolCode} - {data.SourceText} no update";
                    string warning = "The data already exists on OneLINK.";
                    AddComment(demos, comment, scriptId, warning);
                }
            }
            //get the last guarentee date from LG02 and prompt the user to enter info since that date if no info for the borrower/school exists
            else
            {
                DateTime? parsedGuarenteeDate = null;
                ri.FastPath($"LG02I;{demos.Ssn}");
                if(ri.CheckForText(21,3,"SEL"))
                {
                    //access the last page
                    string lastPage = ri.GetText(2, 79, 2).Replace(" ", "");
                    lastPage = lastPage.Length == 1 ? "0" + lastPage : lastPage; //handle adding a 0 beforehand so that it doesn't try to access the wrong page
                    ri.PutText(2, 73, lastPage, ReflectionInterface.Key.Enter);
                    //find the last row on the page
                    int row = 20;
                    while(ri.GetText(row,55,1).Trim() == "")
                    {
                        row--;
                    }
                    //select the record
                    ri.PutText(21, 13, ri.GetText(row, 2, 2), ReflectionInterface.Key.Enter);
                }
                //get the guar date
                if(ri.CheckForText(1,56,"CON")) //CL
                {
                    parsedGuarenteeDate = ri.GetText(5, 10, 8).ToDate();
                }
                else if(ri.CheckForText(1,60,"PLUS LOA")) //PL pre-common/common
                {
                    parsedGuarenteeDate = ri.GetText(4, 10, 8).ToDate();
                }
                else if(ri.CheckForText(1,60,"PLUS MAS")) //PL MPN
                {
                    parsedGuarenteeDate = ri.GetText(5, 10, 8).ToDate();
                }
                else if(ri.CheckForText(1,61,"SLS")) //SL
                {
                    parsedGuarenteeDate = ri.GetText(4, 10, 8).ToDate();
                }
                else //SF,SU,SF/SU MPN
                {
                    parsedGuarenteeDate = ri.GetText(5, 10, 8).ToDate();
                }

                ri.FastPath($"LG29A{demos.Ssn};{data.SchoolCode}");
                //prompt the user for information and add the information
                Dialog.Warning.Ok($"Click OK and enter all enrollment infomration available since {parsedGuarenteeDate.Value.ToString("MM/dd/yyyy")}.", "Enter Enrollment Information");
                //prompt the user for information and add the information
                int counter = 0;
                var additionalInfo = GetAdditionalInfo();
                while (additionalInfo != null)
                {
                    counter++;
                    //add a new school record the first time
                    if(counter == 1)
                    {
                        //add an activity record and got to the next task if the info cannot be updated
                        if(!ri.CheckForText(21,3,"46011") && !ri.CheckForText(21,3,"44000"))
                        {
                            string comment = $"{data.SchoolCode} - {data.SourceText} data cannot be updated due to status of loans";
                            AddComment(demos, comment, scriptId);
                        }
                        else
                        {
                            data.Source = "H";
                            UpdateLG29(demos, data, additionalInfo, scriptId);
                        }
                    }
                    else if(counter == 2)
                    {
                        //access LG29
                        ri.FastPath($"LG29C{demos.Ssn};{data.SchoolCode}");
                        if (!ri.CheckForText(21, 3, "46011") && !ri.CheckForText(21, 3, "44000"))
                        {
                            string comment = $"{data.SchoolCode} - {data.SourceText} data cannot be updated due to status of loans";
                            AddComment(demos, comment, scriptId);
                        }
                        else
                        {
                            data.Source = "H";
                            UpdateLG29(demos, data, additionalInfo, scriptId);
                        }
                    }
                    else
                    {
                        //access LG29
                        ri.FastPath($"LG29C{demos.Ssn};{data.SchoolCode}");
                        //select the first record
                        ri.PutText(20,19,"01",ReflectionInterface.Key.Enter);
                        //Update LG29
                        if (!ri.CheckForText(21, 3, "46011") && !ri.CheckForText(21, 3, "44000"))
                        {
                            string comment = $"{data.SchoolCode} - {data.SourceText} data cannot be updated due to status of loans";
                            AddComment(demos, comment, scriptId);
                        }
                        else
                        {
                            data.Source = "H";
                            UpdateLG29(demos, data, additionalInfo, scriptId);
                        }
                    }
                    additionalInfo = GetAdditionalInfo();
                }
            }
        }

        public void UpdateLG29(SystemBorrowerDemographics demos, EnrollmentData data, AdditionalEnrollmentData additionalData, string scriptId)
        {
            ri.PutText(7, 41, data.Source.ToString()); 
            ri.PutText(8, 20, additionalData.EnrollmentStatus);
            ri.PutText(9, 35, additionalData.StatusEffectiveDate.ToString("MMddyyyy"));
            ri.PutText(10, 35, additionalData.AGD.ToString("MMddyyyy"));
            ri.PutText(10, 71, data.SchoolCode);
            ri.PutText(11, 35, additionalData.SchoolCertificationDate.ToString("MMddyyyy"));
            ri.Hit(ReflectionInterface.Key.Enter);

            string code = ri.GetText(21, 3, 5);
            if(code.IsIn("49000","48003"))
            {
                //updated
                string comment = $"{data.SchoolCode} - {data.SourceText} Updated";
                string warning = "Data updated on OneLINK";
                AddComment(demos, comment, scriptId, warning);                
            }
            else if(code.IsIn("10000", "10001"))
            {
                //call to verify school agd
                string queueComment = "call school to verify agd";
                string queue = "CALLSCHL";
                string comment = $"{data.SchoolCode} - {data.SourceText} data cannot be updated due to status of loans";
                string warning = "Sent to queue for follow up with school.";
                AddComment(demos, comment, scriptId, warning, queue, queueComment);                
            }
            else if(code.IsIn("10002"))
            {
                //add task to HISTREVR queue
                string queueComment = "please request a full enrollment history";
                string queue = "HISTREVR";
                string comment = $"{data.SchoolCode} - {data.SourceText} no update, enrollment history being requested";
                string warning = "No update on OneLINK. Enrollment history being requested.";
                AddComment(demos, comment, scriptId, warning, queue, queueComment);                
            }
            else if(code.IsIn("10003"))
            {
                //call to verify the accuracy of LOA information
                string queueComment = "call school to verify accuracy of LOA information";
                string queue = "LOACALL1";
                string comment = $"{data.SchoolCode} - {data.SourceText} sent to queue for review";
                string warning = "Sent to queue for follow up with school.";
                AddComment(demos, comment, scriptId, warning, queue, queueComment);
            }
            else if(code.IsIn("10004", "10007", "10009", "10011", "10017"))
            {
                //source is acutally H, retry
                data.Source = "H";
                UpdateLG29(demos, data, additionalData, scriptId);
            }
            else if(code.IsIn("10005"))
            {
                //call school to verify L status effective date
                string queueComment = "call school to verify accuracy of L status effective date";
                string queue = "LOACALL1";
                string comment = $"{data.SchoolCode} - {data.SourceText} sent to queue for review";
                string warning = "Sent to queue for follow up with school.";
                AddComment(demos, comment, scriptId, warning, queue, queueComment);
            }
            else if(code.IsIn("10006"))
            {
                //call school to verify accuracy of L status
                string queueComment = "call school to verify accuracy of L status";
                string queue = "LOACALL1";
                string comment = $"{data.SchoolCode} - {data.SourceText} sent to queue for review";
                string warning = "Sent to queue for follow up with school.";
                AddComment(demos, comment, scriptId, warning, queue, queueComment);
            }
            else if(code.IsIn("10008"))
            {
                //call school to verify W or L status and effective date
                string queueComment = "call school to verify W or L status and effective date";
                string queue = "CALLSCHL";
                string comment = $"{data.SchoolCode} - {data.SourceText} sent to queue for review";
                string warning = "Sent to queue for follow up with school.";
                AddComment(demos, comment, scriptId, warning, queue, queueComment);
            }
            else if(code.IsIn("10010","10013"))
            {
                //updateNN
                string comment = $"{data.SchoolCode} - {data.SourceText} no update";
                string warning = "Update not necessary on OneLINK.";
                AddComment(demos, comment, scriptId, warning);
            }
            else if(code.IsIn("10012"))
            {
                //call school to verify accuracy of G status
                string queueComment = "call the school to verify accuracy of G status";
                string queue = "CALLSCHL";
                string comment = $"{data.SchoolCode} - {data.SourceText} sent to queue for review";
                string warning = "Sent to queue for follow up with school.";
                AddComment(demos, comment, scriptId, warning, queue, queueComment);
            }
            else if(code.IsIn("10014"))
            {
                //call school to verify X status
                string queueComment = "call school to verify X status";
                string queue = "NVRENRLC";
                string comment = $"{data.SchoolCode} - {data.SourceText} sent to queue for review";
                string warning = "Sent to queue for follow up with school.";
                AddComment(demos, comment, scriptId, warning, queue, queueComment);
            }
            else if(code.IsIn("10015"))
            {
                //call school to verify correct status and date
                string queueComment = "call school to verify correct status and date";
                string queue = "NVRENRLC";
                string comment = $"{data.SchoolCode} - {data.SourceText} sent to queue for review";
                string warning = "Sent to queue for follow up with school.";
                AddComment(demos, comment, scriptId, warning, queue, queueComment);
            }
            else if(code.IsIn("10016","14000"))
            {
                string queueComment = "follow up to verify student's death";
                string queue = "EVRDEATH";
                string comment = $"{data.SchoolCode} - {data.SourceText} sent to queue for review";
                string warning = "Sent to queue for review.";
                AddComment(demos, comment, scriptId, warning, queue, queueComment);
            }
            else if(code.IsIn("10052"))
            {
                //noupdate
                string comment = $"{data.SchoolCode} - {data.SourceText} no update";
                string warning = "The data already exists on OneLINK.";
                AddComment(demos, comment, scriptId, warning);
            }
            else
            {
                logRun.AddNotification($"Unknown error code encountered. No action taken. Error Code: {code} ", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                Dialog.Error.Ok("Unknown error code encountered. No action taken.");
            }
        }

        public void AddComment(SystemBorrowerDemographics demos, string comment, string scriptId, string warning = null, string queue = null, string queueComment = null)
        {
            if(queue != null)
            {
                bool queueAdded = ri.AddQueueTaskInLP9O(demos.Ssn, queue, null, queueComment);
                if (queueAdded == false)
                {
                    logRun.AddNotification($"Failed to add queue({queue}) in LP9O Account Number: {demos.AccountNumber}, Queue Comment: {queueComment ?? ""}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    Dialog.Error.Ok($"Failed to add arc. Please make sure the arc is added manually. Notification text {warning}");
                    return;
                }
            }
            bool result = ri.AddCommentInLP50(demos.Ssn, "MS", "08", "GEVRR", comment, scriptId);
            if(result == false)
            {
                logRun.AddNotification($"Failed to add arc in LP50 Account Number: {demos.AccountNumber}, Activity Type: MS, Activity Contact: 08 Action Code: GEVRR, Comment: {comment}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                Dialog.Error.Ok($"Failed to add arc. Please make sure the arc is added manually. Notification text {warning}");
                return;
            }
            if(warning != null)
            {
                Dialog.Warning.Ok(warning);
            }
        }

        public DateTime? HandleLG02(SystemBorrowerDemographics demos)
        {
            DateTime minDate = DateTime.Today;
            ri.FastPath($"LG02I{demos.Ssn}");
            if(ri.CheckForText(22,3,"47004 NO RECORDS FOUND FOR ENTERED SEARCH CRITERIA"))
            {
                ri.FastPath($"LG02I;{demos.Ssn}");
                if(ri.CheckForText(22,3,"47004 NO RECORDS FOUND FOR ENTERED SEARCH CRITERIA"))
                {
                    Dialog.Error.Ok("No loans were found on LG02");
                    logRun.AddNotification($"No loans were found on LG02 for account number {demos.AccountNumber}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    return null;
                }
            }
            if(ri.CheckForText(1,58,"LOAN APPLICATION SELECT"))
            {
                //selection screen
                for(int row = 0; row < 11; row++)
                {
                    if (ri.GetText(row + 10, 55, 2).Trim() != "")
                    {
                        DateTime? sessionDate = ri.GetText(row + 10, 55, 8).ToDateNullable();
                        if (sessionDate.HasValue && sessionDate.Value < minDate)
                        {
                            minDate = sessionDate.Value;
                        }
                    }
                    if(row == 10)
                    {
                        ri.Hit(ReflectionInterface.Key.F8);
                        if(ri.CheckForText(22,3,"46004"))
                        {
                            break;
                        }
                        row = -1; //row++ will happen right after
                    }
                }
            }
            else
            {
                //target screen
                DateTime? sessionDate = ri.GetText(5, 10, 8).ToDateNullable();
                if (sessionDate.HasValue && sessionDate.Value < minDate)
                {
                    minDate = sessionDate.Value;
                }
            }
            return minDate;
        }

        public static AdditionalEnrollmentData GetAdditionalInfo()
        {
            AdditionalEnrollmentInformation additionalInfo = new AdditionalEnrollmentInformation();
            var result = additionalInfo.ShowDialog();

            if (result == DialogResult.OK)
            {
                return additionalInfo.Data;
            }
            else
            {
                return null;
            }
        }
    }
}
