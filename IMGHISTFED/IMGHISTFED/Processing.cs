using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using Efs = Uheaa.Common.DataAccess.EnterpriseFileSystem;

namespace IMGHISTFED
{
    public partial class ActivityHistoryReport
    {
        public bool IsRunning = true;
        public static object processingLock = new object();

        /// <summary>
        /// Gets all the data for the files and gets their count to increment the progress bar.
        /// </summary>
        public void SetMaxValue()
        {
            if (Directory.Exists(ActForm.Folder) && Directory.GetFiles(ActForm.Folder).Count() > 0)
            {
                //Check to see if the files need to be deleted or continue processing
                string message = string.Format("There are files in the {0} folder from the last time this application was run. Would you like to "
                    + "delete the files and continue?\nYes:\tDelete Files and continue.\nNo:\tAppend the files and continue.\nCancel:\tCancel", ActForm.Folder);
                DialogResult result = MessageBox.Show(message, "Existing Files", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                if (result == DialogResult.Yes)
                {
                    while (Directory.Exists(ActForm.Folder))
                        Directory.Delete(ActForm.Folder, true);
                    Recovery.Delete();
                }
                else if (result == DialogResult.Cancel)
                {
                    ActForm.CloseForm(false);
                    ActForm.CheckProgress = false;
                }
                //If No, continue processing
            }

            //Create the directory
            if (!Directory.Exists(ActForm.Folder))
                Directory.CreateDirectory(ActForm.Folder);
            if(!Directory.Exists($"{ActForm.Folder}Uncommited"))
                Directory.CreateDirectory($"{ActForm.Folder}Uncommited");
            else
            {
                Directory.Delete($"{ActForm.Folder}Uncommited", true);
                Directory.CreateDirectory($"{ActForm.Folder}Uncommited");
            }

            files = new List<string>(); //Get a list of all files being read in

            //Get the SAS records into collections of objects.
            Demographics = GetDemographics();
            if (Demographics != null)
                Demographics = Demographics.OrderBy(p => p.BorrowerNumber);
            Activities = GetActivities();
            Deferments = GetDeferments();
            EndorserDemo = GetEndorserDemographics();
            Repayment = GetRepaymentInfo();
            if (EndorserDemo != null)
                EndorserDemo = EndorserDemo.OrderBy(p => p.BorrowerNumber);

            CheckFileTypes(); //Check to make sure all the files are ready to process

            //Get the highest number from each file and add one since they are 0 based
            Max = Demographics.Max(p => p.BorrowerNumber);
            if (EndorserDemo != null && EndorserDemo.Count() > 0)
                Max += EndorserDemo.Max(p => p.BorrowerNumber);
            if (Demographics.Min(p => p.BorrowerNumber) == 0)
                Max++;
            if (EndorserDemo != null && EndorserDemo.Count() > 0 && EndorserDemo.Min(p => p.BorrowerNumber) == 0)
                Max++;
            ActForm.SetProgressValues(Max);
        }

        /// <summary>
        /// Checks the files to make sure all conditions were met.
        /// </summary>
        private void CheckFileTypes()
        {
            if (Demographics == null || Activities == null || Deferments == null || EndorserDemo == null || Repayment == null)
            {
                List<string> message = new List<string>();
                message.Add(RError(R2));
                message.Add(RError(R3));
                message.Add(RError(R4));
                message.Add(RError(R6));
                message.Add(RError(R7));
                message = message.Distinct().ToList();
                string errorMessage = string.Format("The following errors were found:\r\n\r\n{0}\r\n\r\nPlease fix and try again", string.Join("\r\n", message));
                Dialog.Info.Ok(errorMessage, "File Error");
                ActForm.CheckProgress = false;
                ActForm.CloseForm(true);
            }
        }

        private string RError<T>(RFileParseResults<T> parseResults) where T : RFile
        {
            var r = "R" + parseResults.RNumber;
            var f = parseResults.ErrorType;
            if (f == FileErrorType.Missing)
                return string.Format("The {0} sas file is missing.", r);
            else if (f == FileErrorType.Empty)
                return string.Format("The {0} sas file is empty.", r);
            else if (f == FileErrorType.FileFormat)
                return parseResults.Error;
            return null;
        }

        /// <summary>
        /// Creates the reports and deletes the files.
        /// </summary>
        public void Process()
        {
            //Send it all to reports to be imaged.
            IndexFileGenerator indexFileGenerator = new IndexFileGenerator(ActForm.Folder);
            CreateReports(Demographics, Activities, Deferments, Repayment, indexFileGenerator);
            if (EndorserDemo != null && EndorserDemo.Count() > 0 && IsRunning)
                CreateEndorserReport(EndorserDemo, indexFileGenerator);
            if (IsRunning)
            {
                //Clean up and end.
                for (int reportNumber = 2; reportNumber <= 6; reportNumber++)
                {
                    string sasFile = string.Format("{0}{1}.R{2}", Efs.TempFolder, SAS_JOB, reportNumber);
                    if (File.Exists(sasFile))
                        FileHelper.DeleteFile(sasFile, ProcessLogData.ProcessLogId, ProcessLogData.ExecutingAssembly);
                }

                indexFileGenerator.ZipFiles();
                indexFileGenerator.DeleteDirectories();
                //Delete the temporary folder
                //if (ActForm.WillImage && !ActForm.DidCancel)
                //    ImagingGenerator.DeleteTempFolder(ActForm.Folder);
            }
        }

        public int UpdateCommitIndex(Dictionary<int,FileSet> pendingCommit, int commitIndex, int currentIndex, string pdfFile, string uncommitedPdfFile, string ssn, BorrowerIndexRecord record)
        {
            pendingCommit.Add(currentIndex, new FileSet(pdfFile, uncommitedPdfFile, ssn, record));
            for(int i = commitIndex+1; i <= currentIndex; i++)
            {
                if(pendingCommit.ContainsKey(i))
                {
                    commitIndex = i;
                }
                else
                {
                    return commitIndex;
                }
            }
            return commitIndex;
        }

        //THIS IS THE CRITICAL CODE, FAILURE IN HERE WILL RESULT IN POSSIBLE DUPLICATION
        public void AddCommitedFiles(Dictionary<int, FileSet> pendingCommit, int commitIndex, IndexFileGenerator indexGenerator)
        {
            var filesToCommit = pendingCommit.Where(p => p.Key <= commitIndex).OrderBy(p => p.Key).ToList();
            for(int i = 0; i < filesToCommit.Count; i++)
            {
                var f = filesToCommit[i];
                if (f.Value != null)
                {
                    //FS.Move(f.Value.UncommitedPdfFile, f.Value.PdfFile);
                    indexGenerator.MoveFile(f.Value.IndexRecord, f.Value.UncommitedPdfFile);
                    indexGenerator.WriteIndexLineForBorrower(f.Value.IndexRecord, Path.GetFileName(f.Value.PdfFile));

                    Recovery.RecoveryValue = string.Format("{0},0", f.Key);
                    pendingCommit.Remove(f.Key);
                }
            }
        }

        /// <summary>
        /// Creates the borrower letter in a PDF format to be imaged.
        /// </summary>
        /// <param name="demographics"></param>
        /// <param name="activities"></param>
        /// <param name="deferments"></param>
        private void CreateReports(IEnumerable<Demographic> demographics, IEnumerable<Activity> activities, IEnumerable<Deferment> deferments, IEnumerable<RepaymentInfo> repayments, IndexFileGenerator indexFileGenerator)
        {
            int recoveryCount = demographics.Min(p => p.BorrowerNumber);
            if (!Recovery.RecoveryValue.IsNullOrEmpty())
            {
                if (Recovery.RecoveryValue.SplitAndRemoveQuotes(",")[1] != "R2")
                    recoveryCount = int.Parse(Recovery.RecoveryValue.SplitAndRemoveQuotes(",")[0].ToString()) + 1;
                else
                    return;
                ActForm.Increase(recoveryCount);
            }

            //group in dictionaries for faster lookup
            Dictionary<int, IEnumerable<Demographic>> orderedDemographics = demographics.GroupBy(o => o.BorrowerNumber).ToDictionary(o => o.Key, o => o.AsEnumerable());
            Dictionary<string, IEnumerable<Activity>> orderedActivities = activities.GroupBy(o => o.SSN).ToDictionary(o => o.Key, o => o.AsEnumerable());
            Dictionary<string, IEnumerable<Deferment>> orderedDeferments = new Dictionary<string, IEnumerable<Deferment>>();
            Dictionary<string, IEnumerable<RepaymentInfo>> orderedRepayments = new Dictionary<string, IEnumerable<RepaymentInfo>>();
            if (deferments != null)
                orderedDeferments = deferments.GroupBy(o => o.SSN).ToDictionary(o => o.Key, o => o.AsEnumerable());
            if (repayments != null)
                orderedRepayments = repayments.GroupBy(o => o.BorrowerSsn).ToDictionary(o => o.Key, o => o.AsEnumerable());
            //Loop through each borrower, recovering if needed.
            int maxBorrowerNumber = demographics.Max(p => p.BorrowerNumber);
            //IndexFileGenerator indexFileGenerator = new IndexFileGenerator(ActForm.Folder);
            ITextEvents.InitializeImageScale();
            int commitIndex = recoveryCount - 1;
            Dictionary<int,FileSet> pendingCommit = new Dictionary<int, FileSet>();
            Queue<int> recordsToProcess = new Queue<int>();
            for (int borrowerNumber = recoveryCount; borrowerNumber <= maxBorrowerNumber; borrowerNumber++)
            {
                recordsToProcess.Enqueue(borrowerNumber);
            }

            Parallel.ForEach(recordsToProcess,
            index =>
            {
                if (!IsRunning) //Check to see if the current thread is running
                return;

                //Get the demographic history for this borrower.
                IEnumerable<Demographic> borrowerDemographics = orderedDemographics[index].ToArray();
                //Maybe I'm being pessimistic, but I can just see someone deleting a borrower from the middle of the file if the script has to go into recovery.
                //For cases like that, just skip to the next borrower.
                if (borrowerDemographics.Count() == 0)
                {
                    lock (processingLock)
                    {
                        pendingCommit.Add(index,null);
                    }
                    return;
                }

                Guid guid = Guid.NewGuid();
                string ssn = borrowerDemographics.FirstOrDefault()?.SSN;
                string name = borrowerDemographics.FirstOrDefault()?.Name;
                string fileName = string.Format("{0}{1}_{2}.pdf", ActForm.Folder, ssn, guid);
                string uncommitedFileName = string.Format("{0}Uncommited\\{1}_{2}.pdf", ActForm.Folder, ssn, guid);
                string imageFile = string.Format("{0}{1}_{2}.pdf", ActForm.ImagingFolder, ssn, guid);
                var letterOrientation = iTextSharp.text.PageSize.LETTER.Rotate();

                //Get the borrowers information from the database this can race and doesn't need to be locked
                BorrowerIndexRecord borrowerIndexRecord = indexFileGenerator.GetIndexDataForBorrower(ssn);

                using (Document doc = new Document(letterOrientation, 0, 0, 100, 50))
                {
                //Create Document class object and set its size to letter and give space left, right, top, bottom margin
                using (PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(uncommitedFileName, FileMode.Create)))
                    {
                        writer.SetPdfVersion(PdfWriter.PDF_VERSION_1_5);
                        writer.CompressionLevel = PdfStream.BEST_COMPRESSION;
                        writer.SetFullCompression();
                        writer.PageEvent = new ITextEvents();
                        doc.Open(); // open document to write

                    ITextTables tableGenerator = new ITextTables(Eoj);
                    //Create demographics table
                    var tableDemos = tableGenerator.GetDemosTable(ssn, name, borrowerDemographics);

                    //Create Activity History table
                    var tableActivity = tableGenerator.GetActivityTable(ssn, name, orderedActivities);

                    //Create Deferment/Forbearance History table
                    var tableDefermentForbearance = tableGenerator.GetDefermentForbearanceTable(ssn, name, orderedDeferments);

                    //Create Repayments table
                    var tableRepayments = tableGenerator.GetRepaymentTable(ssn, name, orderedRepayments);

                    //add tables to PDF
                    tableDemos.SpacingBefore = 15f; // Give some space after the text or it may overlap the table
                    tableActivity.SpacingBefore = 15f;
                        tableDefermentForbearance.SpacingBefore = 15f;
                        tableRepayments.SpacingBefore = 15f;

                        doc.Add(tableDemos);
                        doc.NewPage();
                        doc.Add(tableActivity);
                        doc.NewPage();
                        doc.Add(tableDefermentForbearance);
                        doc.NewPage();
                        doc.Add(tableRepayments);

                        doc.AddLanguage("english");

                    //close the document and writer
                    doc.Close();
                    }
                }

                lock (processingLock)
                {
                    try
                    {                          
                        //Set the second recovery value to 0 to show the R2 is still being processed
                        int newCommitIndex = UpdateCommitIndex(pendingCommit, commitIndex, index, fileName, uncommitedFileName, ssn, borrowerIndexRecord);
                        AddCommitedFiles(pendingCommit, newCommitIndex, indexFileGenerator);
                        commitIndex = newCommitIndex;
                        Recovery.RecoveryValue = string.Format("{0},0", newCommitIndex);

                        //Increment the EOJ counts for the collections that yielded some results.
                        Eoj.Counts[EOJ_TOTAL_FROM_R2].Increment();
                        Eoj.Counts[EOJ_PROCESSED_FROM_R2].Increment();
                        ActForm.Increase();
                    }
                    catch (Exception ex)
                    {
                        string message = string.Format(ex.Message, fileName);
                        ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical, ProcessLogData.ExecutingAssembly, ex);
                        Err.AddRecord(message, new { File = fileName });
                        throw ex; // We throw to not potentially unboundedly duplicate
                    }
                }
            });

            try
            {
                //Need to add the rest of the files if the process did not crash
                AddCommitedFiles(pendingCommit, maxBorrowerNumber, indexFileGenerator);
            }
            catch (Exception ex)
            {
                string message = string.Format(ex.Message, "Error commiting the last of the files");
                ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical, ProcessLogData.ExecutingAssembly, ex);
                Err.AddRecord(message, new object());
                throw ex; // We throw to not potentially unboundedly duplicate
            }

            Recovery.RecoveryValue = "0,R2";
        }

        /// <summary>
        /// Creates the endorser letter in a PDF format to be imaged.
        /// </summary>
        private void CreateEndorserReport(IEnumerable<EndorserDemographic> endorserDemo, IndexFileGenerator indexFileGenerator)
        {
            int recoveryCount = endorserDemo.Min(p => p.BorrowerNumber);
            if (!Recovery.RecoveryValue.IsNullOrEmpty() && Recovery.RecoveryValue.SplitAndRemoveQuotes(",")[1] == "R2")
            {
                if (int.Parse(Recovery.RecoveryValue[0].ToString()) > recoveryCount)
                    recoveryCount = int.Parse(Recovery.RecoveryValue[0].ToString()) + 1;
                if (ActForm.CurrentCount == 0)
                    ActForm.Increase(Max - (endorserDemo.Max(b => b.BorrowerNumber) + 1) + recoveryCount + 1);
            }

            int maxBorrowerNumber = endorserDemo.Max(p => p.BorrowerNumber);
            //ImagingGenerator img = new ImagingGenerator(ScriptId, UserId, ActForm.WillImage);
            //IndexFileGenerator indexFileGenerator = new IndexFileGenerator(ActForm.Folder);
            ITextEvents.InitializeImageScale();
            for (int borrowerNumber = recoveryCount; borrowerNumber <= maxBorrowerNumber; borrowerNumber++)
            {
                if (!IsRunning) //Check to see if the current thread is running
                    return;

                Guid guid = Guid.NewGuid();
                var demo = endorserDemo.Where(p => p.BorrowerNumber == borrowerNumber).First();
                string ssn = demo?.BorrowerSSN;
                string name = demo?.BorrowerName;
                string endorserSsn = demo?.SSN;
                string endorserName = demo?.Name;
                string fileName = string.Format("{0}Uncommited\\{1}_{2}.pdf", ActForm.Folder, ssn, guid);//string.Format("{0}{1}_{2}.pdf", ActForm.Folder, endorserSsn, guid);
                //string uncommitedFileName = string.Format("{0}Uncommited\\{1}_{2}.pdf", ActForm.Folder, ssn, guid);
                var letterOrientation = iTextSharp.text.PageSize.LETTER.Rotate();
                using (Document doc = new Document(letterOrientation, 0, 0, 100, 50))
                {
                    //Create Document class object and set its size to letter and give space left, right, top, bottom margin
                    using (PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(fileName, FileMode.Create)))
                    {
                        writer.SetPdfVersion(PdfWriter.PDF_VERSION_1_5);
                        writer.CompressionLevel = PdfStream.BEST_COMPRESSION;
                        writer.SetFullCompression();
                        writer.PageEvent = new ITextEvents();
                        doc.Open(); // open document to write

                        var font10 = FontFactory.GetFont("ARIAL", 8);
                        var font10b = FontFactory.GetFont("ARIAL", 8);
                        font10b.SetStyle("bold");

                        ITextTables tableGenerator = new ITextTables(Eoj);
                        //Create demographics table
                        var tableDemos = tableGenerator.GetEndorserDemosTable(ssn, name, endorserSsn, endorserName, endorserDemo.Where(p => p.BorrowerNumber == borrowerNumber));

                        //Create Activity History table
                        Activity newAct = new Activity() { Name = name, SSN = ssn };
                        List<Activity> act = new List<Activity>();
                        act.Add(newAct);
                        PdfPTable tableActivity = tableGenerator.GetEndorserActivityTable(ssn, name, endorserSsn, endorserName, act.AsEnumerable());

                        //Create Deferment/Forbearance History table
                        Deferment newDef = new Deferment() { SSN = ssn, Name = name };
                        List<Deferment> def = new List<Deferment>();
                        def.Add(newDef);
                        PdfPTable tableDefermentForbearance = tableGenerator.GetEndorserDefermentForbearanceTable(ssn, name, endorserSsn, endorserName, def.AsEnumerable());

                        //add tables to PDF
                        tableDemos.SpacingBefore = 15f; // Give some space after the text or it may overlap the table
                        tableActivity.SpacingBefore = 15f;
                        tableDefermentForbearance.SpacingBefore = 15f;

                        doc.Add(tableDemos);
                        doc.NewPage();
                        doc.Add(tableActivity);
                        doc.NewPage();
                        doc.Add(tableDefermentForbearance);

                        doc.AddLanguage("english");

                        //close the document and writer
                        doc.Close();
                    }
                }

                try
                {
                    var indexRecord = indexFileGenerator.GetIndexDataForEndorser(ssn, endorserSsn);
                    indexFileGenerator.MoveFile(indexRecord, fileName);
                    indexFileGenerator.WriteIndexLineForBorrower(indexRecord, fileName);

                    Eoj.Counts[EOJ_PROCESSED_FROM_R6].Increment();
                    Eoj.Counts[EOJ_TOTAL_FROM_R6].Increment();

                    //Make sure the recovery has R2 to know that the R2 has been processed.
                    Recovery.RecoveryValue = string.Format("{0},R2", borrowerNumber.ToString());
                    ActForm.Increase();
                }
                catch(Exception ex)
                {
                    string message = string.Format(ex.Message, fileName);
                    ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical, ProcessLogData.ExecutingAssembly, ex);
                    Err.AddRecord(message, new { File = fileName });
                }
            }

            Recovery.RecoveryValue = "";
        }

        /// <summary>
        /// Opens the R3 file and reads in all the data
        /// </summary>
        /// <returns>List of Activity data</returns>
        private List<Activity> GetActivities()
        {
            R3 = ParseRFile<Activity>(3);
            if (R3.ErrorType == FileErrorType.Ok)
                return R3.ParsedResults;
            return null;

        }

        /// <summary>
        /// Opens the R4 files and reads in all the data
        /// </summary>
        /// <returns>List of Deferment data</returns>
        private List<Deferment> GetDeferments()
        {
            R4 = ParseRFile<Deferment>(4);
            if (R4.ErrorType == FileErrorType.Ok)
                return R4.ParsedResults;
            return null;
        }

        /// <summary>
        /// Opens the R2 file and reads in all the data
        /// </summary>
        /// <returns>List of Demographic data</returns>
        private List<Demographic> GetDemographics()
        {
            R2 = ParseRFile<Demographic>(2);
            if (R2.ErrorType == FileErrorType.Ok)
                return R2.ParsedResults;
            return null;
        }

        /// <summary>
        /// Opens the R6 files and reads in all the data
        /// </summary>
        /// <returns>List of EndorserDemopgrahic data</returns>
        private List<EndorserDemographic> GetEndorserDemographics()
        {
            R6 = ParseRFile<EndorserDemographic>(6);
            if (R6.ErrorType == FileErrorType.Ok)
                return R6.ParsedResults;
            return null;
        }

        /// <summary>
        /// Opens and parses the R7 file.
        /// </summary>
        private List<RepaymentInfo> GetRepaymentInfo()
        {
            R7 = ParseRFile<RepaymentInfo>(7);
            if (R7.ErrorType == FileErrorType.Ok)
                return R7.ParsedResults;
            return null;

        }

        class RFileParseResults<T> where T : RFile
        {
            public List<T> ParsedResults { get; set; }
            public FileErrorType ErrorType { get; set; }
            public string Error { get; set; }
            public int RNumber { get; set; }
        }

        /// <summary>
        /// Opens an R* file and reads in all the data
        /// </summary>
        /// <returns>List of parsed data T</returns>
        private RFileParseResults<T> ParseRFile<T>(int rNumber) where T : RFile, new()
        {
            var results = new RFileParseResults<T>();
            results.RNumber = rNumber;
            string file = string.Format("{0}{1}.R" + rNumber, Efs.TempFolder, SAS_JOB);
            if (!File.Exists(file))
            {
                string message = "File Missing: " + file;
                ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, message, NotificationType.NoFile, NotificationSeverityType.Critical);
                Err.AddRecord(ERR_ErrorInProcessing, new { File = message });
                results.ErrorType = FileErrorType.Missing;
                return results;
            }
            List<T> parsedList = new List<T>();
            using (StreamReader sasReader = new StreamReader(file))
            {
                //Get the header row out of the way.
                sasReader.ReadLine();
                //Parse the rest of the file.
                while (!sasReader.EndOfStream)
                {
                    string sasLine = sasReader.ReadLine();
                    T line = new T();
                    var lineParseResults = line.Parse(sasLine);
                    if (!lineParseResults.Successful)
                    {
                        results.ErrorType = FileErrorType.FileFormat;
                        results.Error = lineParseResults.Error;
                        ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, lineParseResults.Error, NotificationType.FileFormatProblem, NotificationSeverityType.Critical);
                        return results;
                    }
                    parsedList.Add(line);
                }
            }
            files.Add(file);
            results.ParsedResults = parsedList;
            results.ErrorType = FileErrorType.Ok;
            return results;
        }

        /// <summary>
        /// Public EOJ and ERR reports, add Process Log notifications, end Process Log, create log file and delete recovery
        /// </summary>
        public void EndScript(bool errorFound, string message = "Process Complete")
        {
            if (Err != null)
                Err.Publish();
            if (Eoj != null)
            {
                if (ProcessLogData != null && ProcessLogData.ProcessLogId > 0)
                    Eoj.PublishProcessLogger(ProcessLogData);

                Eoj.Publish();
            }
            if (ProcessLogData != null && ProcessLogData.ProcessLogId > 0)
                ProcessLogger.LogEnd(ProcessLogData.ProcessLogId);

            Recovery.Delete();
            if (!errorFound)
            {
                File.Create(string.Format("{0}MBS{1}_{2}_{3}.TXT", EnterpriseFileSystem.LogsFolder, ScriptId, UserId, DateTime.Now.ToString("MMddyyyy_hhmmss")));

                if (!CalledByJams && !message.IsNullOrEmpty())
                {
                    MessageBox.Show(message, ScriptId, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                if (ShouldDelete)
                {
                    if (files != null && files.Count > 0)
                    {
                        //Delete the files
                        foreach (string item in files)
                        {
#if !DEBUG
                    FileHelper.DeleteFile(item, ProcessLogData.ProcessLogId, ProcessLogData.ExecutingAssembly);
#endif
                        }
                    }
                }
                ActForm.StopThread();
            }
        }

        /// <summary>
        /// Ends the ProcessLogger and aborts the thread.
        /// </summary>
        public void CancelScript()
        {
            ProcessLogger.LogEnd(ProcessLogData.ProcessLogId);
            MessageBox.Show("Process Canceled");
            ActForm.StopThread();
        }

    }
}