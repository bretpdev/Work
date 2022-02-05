using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.Scripts;
using Uheaa.Common.ProcessLogger;
using System.Reflection;
using System.Threading;

namespace I1I2SCHLTR
{
    public class I1I2SchoolLetter
    {
        public const string ERR_ErrorClosingQueue = "There was an error closing the queue task";
        public const string ERR_ErrorAddingComments = "There was an error adding comments";
        public const string ERR_ErrorClosingComment = "There was an error closing activity comment";
        public const string affliliatedLenderCostCenter = "ma2324";
        public const string otherLenderCostCenter = "ma2327";

        string DataFile = EnterpriseFileSystem.TempFolder + "DataFile.txt";
        string CoverLetter = EnterpriseFileSystem.TempFolder + "CoverLetter.txt";
        string StateMail = EnterpriseFileSystem.TempFolder + "StateMailData.txt";

        ReflectionInterface RI { get; set; }
        ProcessLogRun LogRun { get; set; }
        DataAccess DA { get; set; }

        public I1I2SchoolLetter(ReflectionInterface ri, ProcessLogRun logRun)
        {
            this.RI = ri;
            this.LogRun = logRun;
            this.DA = new DataAccess(logRun);
        }

        public void Process()
        {
            //StartupMessage("This script creates letters, completes tasks, and adds comments for borrowers for whom UHEAA is requesting updated demographic information from schools.  Click OK to continue or Cancel to Quit.");

            DataFiles files = new DataFiles(LogRun, DA);

            if (files.Prints != null && files.QueueTasks != null && files.Comments != null)
            {
                if (files.QueueTasks.Count > 0)
                    CloseQueueTask(files.QueueTasks);
                if (files.Comments.Count > 0)
                    AddComments(files.Comments);
                if (files.Prints.Count > 0)
                    Print(files.Prints, files.Schools);
            }
            DeleteFiles();
            //ProcessingComplete();
        }

        /// <summary>
        /// Go to ITX6X and close the queue task
        /// </summary>
        /// <param name="data"></param>
        private void CloseQueueTask(List<QueueTaskData> queueTasks)
        {
            //int counter = Recovery.RecoveryValue.IsNullOrEmpty() ? 0 : Recovery.RecoveryValue.ToInt();

            foreach (QueueTaskData borData in queueTasks)
            {
                var success = DA.CloseQueueTask(borData.Queue, borData.SubQueue,borData.SSN);
                if(success)
                {
                    success = DA.MarkQueueTaskDataProcessed(borData.QueueTaskDataId);
                    if (!success)
                    {
                        LogRun.AddNotification($"Error marking record as processed. QueueTaskDataId: {borData.QueueTaskDataId.ToString()}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    }
                }
                else
                {
                    LogRun.AddNotification($"Error adding borrower to queuecomplet. QueueTaskDataId: {borData.QueueTaskDataId.ToString()}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                }
            }
            //foreach (QueueTaskData borData in queueTasks)
            //{
            //    RI.FastPath(string.Format("TX3ZITX6X{0};{1};{2}*", borData.Queue, borData.SubQueue, borData.SSN));
            //    if (!RI.CheckForText(23, 2, "01020"))
            //    {
            //        RI.PutText(21, 18, "01", ReflectionInterface.Key.Enter);
            //        RI.Hit(ReflectionInterface.Key.Home);
            //        RI.PutText(1, 4, "ITX6X", ReflectionInterface.Key.EndKey);
            //        RI.Hit(ReflectionInterface.Key.Enter);
            //        RI.Hit(ReflectionInterface.Key.Enter);
            //        RI.PutText(21, 18, "01", ReflectionInterface.Key.F2);
            //        RI.PutText(8, 19, "C");
            //        RI.PutText(9, 19, "COMPL", ReflectionInterface.Key.Enter);
            //        if (RI.CheckForText(23, 2, "01005"))
            //        {
            //            bool success = DA.MarkQueueTaskDataProcessed(borData.QueueTaskDataId);
            //            if(!success)
            //            {
            //                LogRun.AddNotification($"Error marking record as processed. QueueTaskDataId: {borData.QueueTaskDataId.ToString()}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            //            }
            //        }
            //        else
            //        {
            //            LogRun.AddNotification(ERR_ErrorClosingQueue + $" Screen Code Not 01005, Received Code {RI.GetText(23,2,5)}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            //        }
            //    }
            //    else
            //    {
            //        LogRun.AddNotification(ERR_ErrorClosingQueue + " Screen Code 01020 no records found", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            //    }
            //}
        }

        /// <summary>
        /// Adds an Arc to borrower accounts
        /// </summary>
        /// <param name="data">List of BorrowerData</param>
        private void AddComments(List<BorrowerData> commentData)
        {
            //int counter = Recovery.RecoveryValue.IsNullOrEmpty() ? 0 : Recovery.RecoveryValue.ToInt();

            foreach (BorrowerData borData in commentData)
            {
                //Get list of valid and invalid schools for each borrower
                string validList = string.Join(",", borData.Schools.Where(s => s.SchoolStatus.ToLower() != "c").Select(p => p.School).ToArray());
                string invalidList = string.Join(",", borData.Schools.Where(s => s.SchoolStatus.ToLower() == "c").Select(p => p.School).ToArray());
                string validComment = string.Format("letter(s) mailed to school(s): {0} to request verification of borrower's demographic information", validList);
                string invalidComment = string.Format("letter(s) mailed to school(s): {0} to request verification of borrower's demographic information", invalidList);
                //Make sure each arc gets the correct valid or invalid comment
                bool commentAdded = false;
                if (!borData.CommentProcessedAt.HasValue && !validList.IsNullOrEmpty())
                {
                    commentAdded = AddComment(borData.SSN, validComment);
                }
                else if(!commentAdded && !borData.CommentProcessedAt.HasValue && !invalidList.IsNullOrEmpty())
                {
                    commentAdded = AddComment(borData.SSN, invalidComment);
                }

                //Mark the record as comment processed if a comment was added
                if (!borData.CommentProcessedAt.HasValue && commentAdded)
                {
                    bool success = DA.MarkCommentProcessed(borData.CommentDataId);
                    if(!success)
                    {
                        LogRun.AddNotification($"Error marking record as comment added. CommentDataId: {borData.CommentDataId.ToString()}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    }
                }
                else if(!borData.CommentProcessedAt.HasValue)
                {
                    LogRun.AddNotification(ERR_ErrorAddingComments, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                }

                if (!borData.TaskProcessedAt.HasValue)
                {
                    CloseTask(borData); //This closes the activity comment, different from the CloseQueueTask
                }
            }
        }

        /// <summary>
        /// Add the comment to ADT22 for all loans
        /// </summary>
        /// <param name="ssn">The SSN of the current borrower</param>
        /// <param name="comment">The comment to add to the KLSLT arc</param>
        /// <returns>True if added successfully, false if not</returns>
        private bool AddComment(string ssn, string comment)
        {
            string accountNumber = DA.GetAccountNumberFromSsn(ssn);
            ArcData data = new ArcData(DataAccessHelper.CurrentRegion)
            {
                AccountNumber = accountNumber,
                Arc = "KLSLT",
                Comment = comment,
                ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                ScriptId = Program.ScriptId
            };

            var results = data.AddArc();
            if(!results.ArcAdded)
            {
                LogRun.AddNotification($"Failed to add arc for account: {accountNumber} Arc: KLSLT Comment: {comment}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }
            return results.ArcAdded;
            //return RI.Atd22AllLoans(ssn, "KLSLT", comment, "", Program.ScriptId, false);
        }

        /// <summary>
        /// Closes the Activity Comment for the KLSLT Arc
        /// </summary>
        /// <param name="bor">List of BorrowerData</param>
        private void CloseTask(BorrowerData bor)
        {
            string code = bor.Schools.Select(s => s.SchoolStatus.ToLower() != "c").Any() ? "PRNTD" : "INVAD";

            RI.FastPath("TX3ZCTD2A");
            RI.PutText(4, 16, bor.SSN);
            RI.PutText(11, 65, "KLSLT");
            RI.PutText(21, 16, bor.CommentProcessedAt.HasValue ? bor.CommentProcessedAt.Value.ToString("MMddyy") : DateTime.Now.Date.ToString("MMddyy"));
            RI.PutText(21, 30, DateTime.Now.Date.ToString("MMddyy"));
            RI.PutText(7, 60, "X", ReflectionInterface.Key.Enter);

            if (!RI.CheckForText(1, 72, "01019"))
            {
                if (RI.CheckForText(1, 72, "TDX2D"))
                {
                    RI.PutText(15, 2, code, ReflectionInterface.Key.Enter);
                    bool success = DA.MarkCommentTaskProcessed(bor.CommentDataId);
                    if (!success)
                    {
                        LogRun.AddNotification($"Error marking record as task processed. CommentDataId: {bor.CommentDataId.ToString()}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    }
                }
                else if (RI.CheckForText(1, 72, "TDX2C"))
                {
                    RI.PutText(5, 14, "X", ReflectionInterface.Key.Enter);
                    //Look for the empty record
                    while (!RI.GetText(15, 2, 4).Contains("_"))
                    {
                        if (RI.CheckForText(23, 2, "90007"))
                            continue;
                        RI.Hit(ReflectionInterface.Key.F8);
                    }
                    if (RI.CheckForText(15, 2, "____"))
                    {
                        RI.PutText(15, 2, code, ReflectionInterface.Key.Enter);
                        bool success = DA.MarkCommentTaskProcessed(bor.CommentDataId);
                        if (!success)
                        {
                            LogRun.AddNotification($"Error marking record as task processed. CommentDataId: {bor.CommentDataId.ToString()}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                        }
                    }
                    if (!RI.CheckForText(23, 2, "01005"))
                    {
                        LogRun.AddNotification(ERR_ErrorClosingComment + $" Received Screen Code: {RI.GetText(23,2,5)}", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                    }
                }
            }
        }

        /// <summary>
        /// Add the school data for each borrower loan
        /// </summary>
        /// <param name="data"></param>
        private SchoolData AddSchoolData(PrintData data)
        {
            SchoolData sData = new SchoolData();
            if (data.SchoolStatus.ToLower() != "c")
            {
                List<SchoolData> lenderLevelSchoolData = DA.GetSchoolDemographics(data.School, data.SSN);
                if(lenderLevelSchoolData == null || lenderLevelSchoolData.Count == 0)
                {
                    LogRun.AddNotification($"Failed to get results for school demographics for School: {data.School}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    return null; //there were no results for the school
                }

                bool hasAffiliatedLender = false;
                foreach (var sd in lenderLevelSchoolData)
                {
                    if(sd.Lender != null)
                    {
                        hasAffiliatedLender = hasAffiliatedLender || DA.IsAffiliatedLender(sd.Lender);
                    }
                }

                var result = lenderLevelSchoolData.First();
                result.CostCenterCode = hasAffiliatedLender ? affliliatedLenderCostCenter : otherLenderCostCenter;
                sData = result;
            }
            return sData;
        }

        private void Print(List<PrintData> print, List<SchoolData> schools)
        {
            foreach (IGrouping<int, PrintData> runDate in print.GroupBy(p => p.RunDateId))
            {
                List<IGrouping<int, PrintData>> runDateList = new List<IGrouping<int, PrintData>>();
                runDateList.Add(runDate);
                List<PrintData> pData = runDateList.SelectMany(p => p).ToList();

                //We do the grouping to make sure the files are processed one at a time
                //Multiple R2 SAS files can be queued at the same time so we don't want to bundle them all together
                var schoolLists = GetSchoolLists(pData, schools);
                if(schoolLists == null)
                {
                    return;
                }
                foreach (Dictionary<string, List<SchoolData>> costCenter in schoolLists)
                {
                    int low = 1;
                    int high = 3;
                    int pagesPerDoc = 2;
                    string currentCostCenter = "";
                    for (int i = 0; i < 10; i++)
                    {
                        List<List<SchoolData>> printData = costCenter.Where(p => p.Value.Count >= low && p.Value.Count <= high).Select(r => r.Value).ToList();
                        if (printData != null && printData.Count > 0)
                        {
                            currentCostCenter = printData.Select(p => p.Select(r => r.CostCenterCode).FirstOrDefault()).FirstOrDefault();
                            PrintStateMailCoverLetter(pagesPerDoc, "", currentCostCenter, printData.Count);
                            for (int j = 0; j < printData.Count; j++)
                            {
                                PrintCoverLetter(printData[j][0]);
                                PrintDocument(printData[j]);
                            }
                        }
                        low += 3;
                        high += 3;
                        pagesPerDoc++;
                    }
                }
                

                //because all of the R2 data goes to schools we can just mark the R2 records as processed after the letter is sent to the school
                foreach (PrintData data in pData)
                {
                    bool success = DA.MarkPrintDataProcessed(data.PrintDataId);
                    if (!success)
                    {
                        LogRun.AddNotification($"Error marking record as processed. PrintDataId: {data.PrintDataId.ToString()}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    }
                }
            }
        }

        /// <summary>
        /// Creates two lists of dictionarys for each cost center, each containing a SchoolData object for each school
        /// </summary>
        /// <param name="R2">List of PrintData objects</param>
        /// <param name="schools"></param>
        /// <returns>List<Dictionary<string, List<SchoolData>>> seperated by cost center and school</returns>
        private List<Dictionary<string, List<SchoolData>>> GetSchoolLists(List<PrintData> print, List<SchoolData> schools)
        {
            Dictionary<string, List<SchoolData>> affiliatedCostCenterDictionary = new Dictionary<string, List<SchoolData>>();
            Dictionary<string, List<SchoolData>> otherCostCenterDictionary = new Dictionary<string, List<SchoolData>>();
            SchoolData sData = new SchoolData();
            foreach (SchoolData school in schools)
            {
                List<SchoolData> dataList = new List<SchoolData>();
                foreach (PrintData data in print.Where(s => s.School == school.School && s.SchoolStatus.ToLower() != "c"))
                {
                    if (school.SchoolName.IsNullOrEmpty())
                    {
                        sData = AddSchoolData(data);
                        if(sData == null)
                        {
                            return null;
                        }
                    }
                    sData.Borrowers.Add(data);
                    dataList.Add(sData);
                }
                if (dataList.Any(p => p.CostCenterCode == affliliatedLenderCostCenter))
                    affiliatedCostCenterDictionary.Add(school.School, dataList);
                else
                    otherCostCenterDictionary.Add(school.School, dataList);
            }
            //Add the dictionaries to a queue so you know the first one in is the first one out
            List<Dictionary<string, List<SchoolData>>> sortedFiles = new List<Dictionary<string, List<SchoolData>>>();
            sortedFiles.Add(affiliatedCostCenterDictionary);
            sortedFiles.Add(otherCostCenterDictionary);
            return sortedFiles;
        }

        private void PrintCoverLetter(SchoolData school)
        {
            using (StreamWriter sw = new StreamWriter(CoverLetter))
            {
                sw.WriteLine("SchName, SchAdd, SchAdd2, SchAdd3, SchCity, SchST, SchZip");
                sw.WriteLine(string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}", school.SchoolName, school.SchoolAddress,
                    school.SchoolAddress2, school.SchoolAddress3, school.SchoolCity, school.SchoolState, school.SchoolZip));
            }
            DocumentProcessing.PrintDocs(EnterpriseFileSystem.GetPath("SCHLTRCVR"), "SCHLTRCVR", CoverLetter);
        }

        private void PrintDocument(List<SchoolData> school)
        {
            using (StreamWriter sw = new StreamWriter(DataFile))
            {
                sw.WriteLine("SSN, FirstName, MI, LastName, Address1, Address2, City, State, ZIP, Country, Phone, AltPhone, Email, School, SchName");
                foreach (SchoolData bor in school)
                {
                    foreach (PrintData borData in bor.Borrowers)
                    {
                        sw.WriteLine(string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}",
                            string.Format("XXX-XX-{0}", borData.SSN.SafeSubString(5, 4)), borData.FirstName, borData.MiddleInitial, borData.LastName,
                            borData.Address1, borData.Address2, borData.City, borData.State, borData.Zip, borData.Country, borData.Phone,
                            borData.AlternatePhone, borData.Email, bor.School, bor.SchoolName));
                    }
                }
            }
            DocumentProcessing.PrintDocs(EnterpriseFileSystem.GetPath("SCHLTRLST"), "SCHLTRLST", DataFile);
        }

        /// <summary>
        /// Print the State Mail Cover Sheet
        /// </summary>
        /// <param name="pagesPerDoc">Number of pages per document</param>
        /// <param name="buMessage">The Business Unit message</param>
        /// <param name="costCenter">Cost Center</param>
        /// <param name="standard">Number of schools in current print job</param>
        private void PrintStateMailCoverLetter(int pagesPerDoc, string buMessage, string costCenter, int standard)
        {
            using (StreamWriter sw = new StreamWriter(StateMail))
            {
                sw.WriteLine("BU, Description, NumPages, Cost, Standard, Foreign, CoverComment");
                sw.WriteLine(string.Format("{0}, School Letter - Skip Trace Assistance Requested, {1}, {2}, {3}, 0, Deliver mail to business unit for processing"
                    , buMessage, pagesPerDoc, costCenter.ToUpper(), standard));
            }
            DocumentProcessing.PrintDocs(EnterpriseFileSystem.GetPath("ScriptedCoverLetter"), "Scripted State Mail Cover Sheet", StateMail);
        }

        /// <summary>
        /// Delete the files after processing complete
        /// </summary>
        /// <param name="files"></param>
        private void DeleteFiles()
        {
            if (File.Exists(DataFile))
                FileHelper.DeleteFile(DataFile, LogRun.ProcessLogId, LogRun.Assembly);
            if (File.Exists(CoverLetter))
                FileHelper.DeleteFile(CoverLetter, LogRun.ProcessLogId, LogRun.Assembly);
            if (File.Exists(StateMail))
                FileHelper.DeleteFile(StateMail, LogRun.ProcessLogId, LogRun.Assembly);
        }
    }
}
