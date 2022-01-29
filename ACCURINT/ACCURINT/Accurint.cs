using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;
using Uheaa.Common;
using Uheaa.Common.Scripts;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using System.Reflection;

namespace ACCURINT
{
    public class Accurint : BatchScript
    {
        [Flags]
        private enum Precondition
        {
            EncryptionSoftware,
            LoggedIn,
            FtpSoftware
        }

        private readonly string ArchiveFolder;
        private AccurintRequestFile AccurintRequestFile;
        private RequestFileCreator RequestFileCreator;
        private ResponseFileProcessor ResponseFileProcessor;
        private FileEncryption FileEncrypter { get; set; }
        public DataAccess DA { get; set; }
        public ProcessLogRun LogRun { get; set; }
        public RunInfo CurrentRun { get; set; }


        public Accurint(ReflectionInterface ri)
            : base(ri, "ACCURINT", "ERR_BU03", "EOJ_BU03", new List<string>() { }, DataAccessHelper.Region.Uheaa)
        {
            LogRun = new ProcessLogRun(ProcessLogData.ProcessLogId, ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, true);
            DA = new DataAccess(LogRun);

            ArchiveFolder = EnterpriseFileSystem.GetPath("Accurint Archive");
            AccurintRequestFile = new AccurintRequestFile();
            RequestFileCreator = new RequestFileCreator(ri, AccurintRequestFile, DA, LogRun, CurrentRun, ScriptId, UserId);
            FileEncrypter = new FileEncryption(DA);
            ResponseFileProcessor = new ResponseFileProcessor(FileEncrypter, DA, LogRun, CurrentRun, ScriptId, ArchiveFolder);
        }

        public override void Main()
        {
            //Warn the user and end the script if the X and T drives are not available.
            IEnumerable<string> driveNames = DriveInfo.GetDrives().Select(p => p.Name);
            foreach (string requiredDrive in new string[] { EnterpriseFileSystem.TempFolder.Substring(0, 3), ArchiveFolder.Substring(0, 3) })
                if (!driveNames.Contains(requiredDrive))
                    NotifyAndEnd("The {0} drive is not available so the script cannot be run.", requiredDrive);

            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), true))
                return;

            CurrentRun = DA.GetRunInfo();
            if (!RunEstablished())
                return;

            UserInput userInput = new UserInput();
            using (MainForm mainForm = new MainForm(userInput))
            {
                while (mainForm.ShowDialog() == DialogResult.OK)
                {
                    switch (userInput.Selected)
                    {
                        case UserInput.Selection.RunAll:
                            RunAll(userInput);
                            break;
                        case UserInput.Selection.CreateRequestFile:
                            CreateRequestFile(userInput);
                            break;
                        case UserInput.Selection.SendRequestFile:
                            SendRequestFile();
                            break;
                        case UserInput.Selection.GetResponseFile:
                            GetResponseFile();
                            break;
                        case UserInput.Selection.ProcessResponseFile:
                            ProcessResponseFile();
                            break;
                        case UserInput.Selection.SendInputFile:
                            SendInputFile();
                            break;
                    }
                }
            }
            RI.CloseSession();
            LogRun.LogEnd();
        }

        private bool CheckPreconditions(params Precondition[] requiredPreconditions)
        {
            List<string> fails = new List<string>();

            //Is GnuPG installed?
            if (requiredPreconditions.Contains(Precondition.EncryptionSoftware) && !FileEncrypter.EncryptionSoftwareIsInstalled)
            {
                fails.Add("You must have the GnuPG software installed to run this process.");
            }

            //Is the user logged in?
            if (requiredPreconditions.Contains(Precondition.LoggedIn) && !UserId.StartsWith("UT"))
            {
                fails.Add("You must be logged in to the AES sytems to run this process.");
            }

            //Does machine have CoreFTP software?
            if (requiredPreconditions.Contains(Precondition.FtpSoftware) && !FtpHandler.CoreFtpInstalledCheck())
            {
                fails.Add($"Unable to find required Core FTP software on your system. It should be installed at {FtpHandler.COREFTP_EXECUTABLE}");
            }

            if (fails.Count == 0)
            {
                return true;
            }
            else
            {
                MessageBox.Show(string.Join(Environment.NewLine, fails.ToArray()), ScriptId, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
        }

        private void RunAll(UserInput input)
        {
            if (!CheckPreconditions(Precondition.EncryptionSoftware, Precondition.LoggedIn, Precondition.FtpSoftware))
                return;

            if (RequestFileCreator.CreateRequestFileFromQueueTasks(input.OnlyProcessFirstSelection) && EncryptAndUploadRequestFile() && DownloadResponseFiles() && ResponseFileProcessor.ReviewResponseFiles())
            {
                MessageBox.Show("Processing complete.", ScriptId, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                NotifyOfErrors(RI.ProcessLogData.ProcessLogId);
            }
        }

        /// <summary>
        /// Establishes a new run if there is no ongoing run. Else, asks user
        /// whether to recover or start a new run. Returns false if
        /// user selects neither and cancels the run.
        /// </summary>
        /// <returns></returns>
        private bool RunEstablished()
        {
            if (CurrentRun == null)
                CurrentRun = DA.CreateRun();
            else
            {
                bool? result = Dialog.Warning.YesNoCancel($"There is already a run in progress from {CurrentRun.CreatedAt}. Do you wish to continue that run?", "Recover from Past Run?");
                if (result == null)
                {
                    LogRun.AddNotification($"User {RI.UserId} cancelled run {CurrentRun.RunId}", NotificationType.Other, NotificationSeverityType.Informational);
                    return false;
                }

                if (result == false)
                {
                    DA.DeleteRun(CurrentRun.RunId); // Delete old run
                    LogRun.AddNotification($"User {RI.UserId} deleted run {CurrentRun.RunId}", NotificationType.Other, NotificationSeverityType.Informational);
                    CurrentRun = DA.CreateRun(); // Create new run
                }
            }
            InitizializeCurrentRun();
            return true;
        }

        private void InitizializeCurrentRun()
        {
            RequestFileCreator = new RequestFileCreator(RI, AccurintRequestFile, DA, LogRun, CurrentRun, ScriptId, UserId);
            ResponseFileProcessor = new ResponseFileProcessor(FileEncrypter, DA, LogRun, CurrentRun, ScriptId, ArchiveFolder);
        }

        private void CreateRequestFile(UserInput input)
        {
            if (!CheckPreconditions(Precondition.EncryptionSoftware, Precondition.LoggedIn)) { return; }

            if (RequestFileCreator.CreateRequestFileFromQueueTasks(input.OnlyProcessFirstSelection))
                MessageBox.Show("Processing complete.", ScriptId, MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                NotifyOfErrors(RI.ProcessLogData.ProcessLogId);
        }

        private void SendRequestFile()
        {
            if (!CheckPreconditions(Precondition.FtpSoftware)) { return; }

            if (EncryptAndUploadRequestFile())
            {
                MessageBox.Show("Processing complete. Request file has been uploaded.", ScriptId, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                NotifyOfErrors(RI.ProcessLogData.ProcessLogId);
            }
        }

        private void GetResponseFile()
        {
            if (!CheckPreconditions(Precondition.FtpSoftware)) { return; }

            if (DownloadResponseFiles())
            {
                MessageBox.Show("Processing complete. Response file downloaded.", ScriptId, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                NotifyOfErrors(RI.ProcessLogData.ProcessLogId);
            }
        }

        private void ProcessResponseFile()
        {
            if (!CheckPreconditions(Precondition.EncryptionSoftware, Precondition.LoggedIn)) { return; }

            if (ResponseFileProcessor.ReviewResponseFiles())
            {
                MessageBox.Show("Processing complete. Response file processed.", ScriptId, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                NotifyOfErrors(RI.ProcessLogData.ProcessLogId);
            }
        }

        private void NotifyOfErrors(int processLogId)
        {
            string message = $"Please see Process Log Id {processLogId} to view and address the error(s) that occurred.";
            Dialog.Info.Ok(message);
        }

        private bool EncryptAndUploadRequestFile()
        {
            //Warn the user if the request file is missing or empty.
            if (!AccurintRequestFile.Exists)
            {
                string message = string.Format("The {0} file is missing.", AccurintRequestFile.FileName);
                MessageBox.Show(message, ScriptId, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (AccurintRequestFile.IsEmpty)
            {
                string message = string.Format("The {0} file is empty.  Do you want to run the remaining selected processes (if any)?", AccurintRequestFile.FileName);
                return (MessageBox.Show(message, ScriptId, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes);
            }

            //Archive a copy of the file, using a unique file name.
            string uniqueFileName = string.Format("AccurintRequestFile_{0:MMddyyyyhhmmss}.txt", DateTime.Now);
            AccurintRequestFile.ArchiveCopy(uniqueFileName);

            //Encrypt the file.
            string encryptedFile = EnterpriseFileSystem.TempFolder + uniqueFileName + ".pgp";
            if (!FileEncrypter.EncryptFile(AccurintRequestFile.FileName, encryptedFile))
            {
                MessageBox.Show("The file was not successfully encrypted.", ScriptId, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (DataAccessHelper.TestMode)
            {
                AccurintRequestFile.Delete();
                File.Delete(encryptedFile);
                return DA.SetRequestUploaded(CurrentRun.RunId, uniqueFileName);
            }

            //Upload the encrypted file.
            FtpHandler ftp = new FtpHandler();
            if (ftp.UploadFile(encryptedFile))
            {
                AccurintRequestFile.Delete();
                File.Delete(encryptedFile);
                return DA.SetRequestUploaded(CurrentRun.RunId, uniqueFileName);
            }
            else
            {
                MessageBox.Show("The Accurint request file was not successfully FTPd.", ScriptId, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
        }

        private bool DownloadResponseFiles()
        {
            const string RESPONSE_FILE_PATTERN = "LN_Output_AccurintRequest*";

            //Warn the user if there is already a response file.
            if (Directory.GetFiles(EnterpriseFileSystem.TempFolder, RESPONSE_FILE_PATTERN).Length > 0)
            {
                string message = $"There is already a LN_Output_Accurint response file in {EnterpriseFileSystem.TempFolder}.";
                message += "  If the file has already been processed, delete the file and click <Get Response File> again.";
                message += "  If the file has not been processed, click <Process Response File> to process the file and then click <Get Response File> again.";
                MessageBox.Show(message, ScriptId, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            FtpHandler ftp = new FtpHandler();
            string[] outputFiles = ftp.DownloadFiles(RESPONSE_FILE_PATTERN, EnterpriseFileSystem.TempFolder);
            if (outputFiles.Length > 0)
            {
                DA.SetResponseFilesDownloaded(CurrentRun.RunId, DateTime.Now);
                return true;
            }
            else
            {
                string message = string.Format("The Accurint response file was not successfully downloaded to {0}.", EnterpriseFileSystem.TempFolder);
                MessageBox.Show(message, ScriptId, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
        }

        private void SendInputFile()
        {
            if (!CheckPreconditions(Precondition.EncryptionSoftware, Precondition.LoggedIn)) { return; }
            string workingFile = "";
            string archiveFile = "";

            if (!CurrentRun.RequestFileCreatedAt.HasValue)
            {
                try
                {
                    //Check that the Special Accurint Requests file exists.
                    string originalFile = EnterpriseFileSystem.FtpFolder + "Special Accurint Requests";
                    if (!File.Exists(originalFile))
                    {
                        string message = "The file is not ready to send to LexisNexis at this time.";
                        MessageBox.Show(message, ScriptId, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    //Copy the Skip Population file to both a temporary directory and X:\Archive\Accurint\.
                    AccurintRequestFile.FileName = workingFile = EnterpriseFileSystem.TempFolder + "SASAccurintRequestFile.txt";
                    File.Copy(originalFile, workingFile);
                    archiveFile = $"{ArchiveFolder}SASAccurintRequestFile.{DateTime.Now:MMddyyyyhhmmss}.txt";
                    File.Copy(originalFile, archiveFile);
                    List<OneLinkDemosRecord> oneLinkDemosRecords = DA.GetUnprocessedOLRecords(CurrentRun.RunId);

                    //Add a OneLINK comment for each record in the file, counting the records as we go.
                    int recordCount = 0;

                    using (StreamReader requestReader = new StreamReader(workingFile))
                    {
                        //Skip the header row.
                        requestReader.ReadLine();

                        SpecialRequestErrorFile errorFile = new SpecialRequestErrorFile();
                        while (!requestReader.EndOfStream)
                        {
                            recordCount++;
                            string ssn = requestReader.ReadLine().SplitAndRemoveQuotes(",")[0];
                            SystemBorrowerDemographics dems = RI.GetDemographicsFromLP22(ssn);
                            dems.AccountNumber.Replace(" ", "");
                            List<OneLinkDemosRecord> oneLinkDemoRecordMatches = oneLinkDemosRecords.Where(p => p.Ssn == ssn).ToList();
                            OneLinkDemosRecord demoRecord = oneLinkDemoRecordMatches.Count() == 0 ? null : oneLinkDemoRecordMatches.First();

                            if (demoRecord == null)
                            {
                                demoRecord = DA.AddSpecialRequestDemoRecord(dems.AccountNumber, null, null, CurrentRun.RunId);
                                if (demoRecord == null)
                                {
                                    LogRun.AddNotification($"Error adding record to accurint.DemsProcessingQueue_OL for account {dems.AccountNumber}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                                    Dialog.Warning.Ok($"Error adding record to DB and request file for account {dems.AccountNumber}. Skipping account.");
                                    continue;
                                }
                            }

                            if (!demoRecord.AddedToRequestFileAt.HasValue)
                            {
                                bool result = RequestFileCreator.AddDemosToRequestFile(dems, demoRecord.DemosId, true);
                                if (!result)
                                {
                                    LogRun.AddNotification($"Error adding record to Accurint request file. Record: {demoRecord}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                                    Dialog.Warning.Ok($"Error adding record to request file for account {dems.AccountNumber}");
                                    continue;
                                }
                            }

                            if (!demoRecord.RequestCommentAdded.HasValue)
                            {
                                if (!AddCommentInLP50(ssn, "AM", "10", "KUBSS", "", ScriptId))
                                    errorFile.AddRecord(ssn, "", "", "", "", RI.GetText(22, 3, 78));
                                else
                                    DA.SetOneLinkRequestCommentAdded(demoRecord.DemosId);
                            }
                        }
                    }

                    File.Delete(originalFile);
                    DA.SetRequestFileInfo(CurrentRun.RunId, recordCount);

                    //Record the number of records found in the file.
                    string reportFolder = EnterpriseFileSystem.GetPath("EOJ_Accurint");
                    string reportFile = string.Format(@"{0}Accurint Sent\Sent to Accurint Total.{1:yyyy-MM-dd-hhmm}", reportFolder, DateTime.Now);
                    File.WriteAllText(reportFile, recordCount.ToString());
                }
                catch (Exception ex)
                {
                    string errorMessage = $"Error encountered while trying to send the special request file. Please see ProcessLogId {LogRun.ProcessLogId} for more details. File NOT sent.";
                    LogRun.AddNotification(errorMessage, NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                    Dialog.Error.Ok(errorMessage);
                    return;
                }

                //Encrypt the request file.
                string encryptedFile = string.Format("{0}SASAccurintRequestFile.{1:MMddyyyy}.txt.pgp", EnterpriseFileSystem.TempFolder, DateTime.Now);
                if (!FileEncrypter.EncryptFile(workingFile, encryptedFile))
                {
                    string message = "The file was not successfully encrypted.";
                    MessageBox.Show(message, ScriptId, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                FtpHandler ftp = new FtpHandler();
                if (ftp.UploadFile(encryptedFile))
                {
                    File.Delete(workingFile);
                    File.Delete(encryptedFile);
                    if (!string.IsNullOrEmpty(archiveFile))
                        DA.SetRequestUploaded(CurrentRun.RunId, archiveFile);
                    else
                    {
                        // Get last archive file. Used for recovery purposes if encryption failed but file was already read
                        string archiveFileName = new DriveInfo(ArchiveFolder).RootDirectory.GetFiles("SASAccurintRequestFile.*").OrderByDescending(p => p.CreationTime).Select(fn => fn.FullName)?.First();
                        DA.SetRequestUploaded(CurrentRun.RunId, archiveFileName);
                    }
                    string message = "The file has been sent to Accurint.";
                    MessageBox.Show(message, ScriptId, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    string message = "The Accurint request file was not successfully FTPd.";
                    MessageBox.Show(message, ScriptId, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

        }
    }
}
