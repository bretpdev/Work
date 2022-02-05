using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace INTNOTFORB
{
    public class InterestNoticesBorrowersInForbearance : BatchScript
    {
        private static readonly string[] EojFields = { "" };
        private List<FileDetails> Files { get; set; }

        public InterestNoticesBorrowersInForbearance(ReflectionInterface ri)
            : base(ri, "INTNOTFORB", "ERR_BU35", "EOJ_BU35", EojFields, DataAccessHelper.Region.Uheaa)
        {
            Files = new List<FileDetails>();
            InitializeFileDetails();
        }

        public override void Main()
        {
            StartupMessage("This script sends interest notices to borrowers, co-makers, and endorsers of accounts hat have been in forbearance for six months or more. Click OK to continue, or Cancel to quit.");

            try
            {
                //If a recovery file exists finish recovery then process the rest of the file if any exist.
                if (!Recovery.RecoveryValue.IsNullOrEmpty())
                    DoRecovery();

                foreach (FileDetails possibleFile in Files)
                {
                    possibleFile.NewestFile = (FileSystemHelper.DeleteOldFilesReturnMostCurrent(EnterpriseFileSystem.FtpFolder, possibleFile.FilePattern));
                }

                Files = Files.Where(p => !p.NewestFile.IsNullOrEmpty()).ToList();
                if (Files.Count == 0)
                {
                    ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, "All of the ULWO84.LWO84 files were missing", NotificationType.NoFile, NotificationSeverityType.Critical);
                    NotifyAndEnd("No Files Found");
                }

                bool allFilesAreEmpty = true;
                foreach (FileDetails file in Files)
                {
                    //We do not want to process empty files
                    if (new FileInfo(file.NewestFile).Length != 0)
                    {
                        allFilesAreEmpty = false;
                        PrintLetters(file);
                        AddComment(file);
                    }

                    File.Delete(file.NewestFile);
                }
                string completeMessage = allFilesAreEmpty ? "There was no data to process." : "Processing Complete";
                ProcessingComplete(completeMessage);
            }
            catch (EndDLLException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("An error has occurred.  Please contact a programmer and reference ProcessLogId {0}", ProcessLogData.ProcessLogId));
                ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, "Unhandled Exception", NotificationType.ErrorReport, NotificationSeverityType.Critical, System.Reflection.Assembly.GetExecutingAssembly(), ex);
                EndDllScript();
            }
        }

        /// <summary>
        /// Adds all the required files to the List<FileDetails> Files object
        /// </summary>
        private void InitializeFileDetails()
        {
            //Setting the activity comment fr all 7 reports
            Dictionary<int, string> activityComments = new Dictionary<int, string>();
            activityComments.Add(2, "Interest Notification Sent to borrower in forbearance");
            activityComments.Add(3, "Interest Notification Generated for borrower but not sent due to invalid address");
            activityComments.Add(4, "Interest Notification Sent to Comaker");
            activityComments.Add(5, "Interest Notification Generated for comaker but not sent due to invalid address");
            activityComments.Add(6, "Interest Notification Sent to Endorser");
            activityComments.Add(7, "Interest Notification Generated for endorser but not sent due to invalid address");

            for (int reportNumber = 2; reportNumber < 8; reportNumber++)
            {
                string filePattern = "ULWO84.LWO84R" + reportNumber + "*";
                //All SAS files that need a letter to be mailed out end in an even report number.
                bool needsLetters = reportNumber.IsEven();

                Files.Add(new FileDetails(filePattern, activityComments[reportNumber], needsLetters));
            }
        }

        /// <summary>
        /// Checks to see if the file needs to have letters printed and calls cost center printing
        /// </summary>
        /// <param name="file">FileDetails object that has all of the information needed to print the letter</param>
        private void PrintLetters(FileDetails file)
        {
            if (!file.NeedsLetter)
                return;

            SetRecoveryValue(1, "PrintingStarted", file.NewestFile);
            DocumentProcessing.CostCenterPrinting("INTERESTNO", file.NewestFile, "STATE", ScriptId, "ACCOUNT", DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.CostCenterOptions.AddBarcode, "COST_CENTER_CODE");
            SetRecoveryValue(1, "PrintingComplete", file.NewestFile);
        }

        /// <summary>
        /// Adds an activity comment to the borrowers account
        /// </summary>
        /// <param name="file">FileDetails object that has all of the information needed add the activity comment</param>
        private void AddComment(FileDetails file)
        {
            int lineCount = 1;
            using (StreamReader sr = new StreamReader(file.NewestFile))
            {
                //Read the header
                sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    if (Recovery.RecoveryValue.Contains("AddingArc"))
                    {
                        //When in recovery read in lines until we get to the last borrower processed
                        int recoveryCount = Recovery.RecoveryValue.SplitAndRemoveQuotes(",")[0].ToInt();
                        while (lineCount != recoveryCount)
                        {
                            sr.ReadLine();
                            lineCount++;
                        }
                    }
                    string line = sr.ReadLine();
                    List<string> fileData = line.SplitAndRemoveQuotes(",");
                    List<int> loans = new List<int>();
                    //Fields 11 - 35 contains the loan sequences for the given record.
                    for (int field = 11; field < 35; field++)
                    {
                        if (fileData[field].IsNumeric())
                            loans.Add(int.Parse(fileData[field]));
                    }

                    //fileData[0] is the ssn filedata[1] is the recipient id.
                    if (!RI.Atd22ByLoan(fileData[0], "FOR06", file.ActiviyComment, fileData[1], loans, ScriptId, false))
                    {
                        Err.AddRecord("There was an error adding an activity comment to the following account. ",
                            new
                            {
                                Ssn = fileData[0],
                                RecipientId = fileData[1],
                                ARC = "FOR06",
                                Comment = file.ActiviyComment,
                                Loans = loans.ToArray().ToString(),
                                ErrorMessage = GetText(23, 2, 77)
                            });
                    }
                    lineCount++;
                    SetRecoveryValue(lineCount, "AddingArc", file.NewestFile);
                }

                SetRecoveryValue(1, "ArcsAdded", file.NewestFile);
            }
        }

        private void SetRecoveryValue(int count, string phase, string file)
        {
            Recovery.RecoveryValue = string.Format("{0}, {1}, {2}", count, phase, file);
        }

        private void DoRecovery()
        {
            string recoveryPhase = Recovery.RecoveryValue.SplitAndRemoveQuotes(",")[1];

            if (recoveryPhase == "ArcsAdded")
            {
                if (File.Exists(Recovery.RecoveryValue.SplitAndRemoveQuotes(",")[2]))
                    File.Delete(Recovery.RecoveryValue.SplitAndRemoveQuotes(",")[2]);
                Recovery.Delete();
                return;
            }
            string fileP = Path.GetFileName(Recovery.RecoveryValue.SplitAndRemoveQuotes(",")[2]);
            FileDetails file = Files.Where(p => p.FilePattern.SafeSubString(12, 2) == fileP.SafeSubString(12, 2)).SingleOrDefault();

            //string recoveryFile = FileSystemHelper.DeleteOldFilesReturnMostCurrent(EnterpriseFileSystem.FtpFolder, file.FilePattern);
            file.NewestFile = Recovery.RecoveryValue.SplitAndRemoveQuotes(",")[2];

            if (Directory.GetFiles(Path.GetDirectoryName(file.NewestFile), fileP).Count() == 0)
            {
                ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, "The script is in recovery but cannot find the recovery file: " + file.FilePattern, NotificationType.NoFile, NotificationSeverityType.Critical);
                NotifyAndEnd("The script is in recovery but cannot find the recovery file: " + file.NewestFile);
            }

            if (Recovery.RecoveryValue.Contains("PrintingStarted"))
                PrintLetters(file);
            if (Recovery.RecoveryValue.Contains("PrintingComplete") || Recovery.RecoveryValue.Contains("AddingArc"))
                AddComment(file);

            //Clean up recovery so that the script can process the normal files
            File.Delete(file.NewestFile);
            Recovery.Delete();
        }
    }
}
