using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Uheaa.Common;


namespace ACURINTFED
{
    public class Accurint : FedBatchScript
    {
        //Recovery value is phase, phase-specific data (where applicable).
        public const string RECOVERY_PHASE_CREATING = "Creating request file";
        public const string RECOVERY_PHASE_UPLOADED = "File sent to Accurint";
        public const string RECOVERY_PHASE_DOWNLOADED = "Response file saved to T drive";
        public const string RECOVERY_PHASE_PROCESSING = "Processing response file";

        //This script's ID gets used in a few places, so a const is warranted.
        public const string EOJ_SENT_TO_ACCURINT = "Total number of borrowers sent to Accurint";
        public const string EOJ_RETRIEVED_FROM_ACCURINT = "Total number of borrowers retrieved from Accurint";
        public const string EOJ_RA01_QUEUE_TASKS = "Total number of LX/01 queue tasks created";
        public const string EOJ_ERRORS = "Total number of errors received";
        private static readonly string[] EOJ_FIELDS = { EOJ_SENT_TO_ACCURINT, EOJ_RETRIEVED_FROM_ACCURINT, EOJ_RA01_QUEUE_TASKS, EOJ_ERRORS };

        private readonly string _archiveFolder;
        private readonly DataAccess DA;
        
        public ProcessLogRun logRun;

        public Accurint(ReflectionInterface ri)
            : base(ri, ACURINTFED.DataAccess.SCRIPTID, "ERR_Accurint", "EOJ_Accurint", EOJ_FIELDS)
        {
            _archiveFolder = EnterpriseFileSystem.GetPath("Accurint Archive");
            DA = new DataAccess();
            logRun = new ProcessLogRun( ACURINTFED.DataAccess.SCRIPTID, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.CornerStone, DataAccessHelper.CurrentMode, false, true, false);
        }

        public override void Main()
        {
            //Warn the user and end the script if the Z drive is not available or GnuPG is not installed.
            if (!DriveInfo.GetDrives().Select(p => p.Name).Contains(@"Z:\"))
                NotifyAndEnd("The Z drive is not available so the script cannot be run.");
            if (!FileEncryption.EncryptionSoftwareIsInstalled)
                NotifyAndEnd("You must have the GnuPG software installed to run this script.");

            //Get credentials for the session and FTP, since we'll need to automatically log in at some point.
            Credentials sessionCredentials = new Credentials();
            this.RI.FastPath("PROF");
            sessionCredentials.UserName = this.RI.GetText(2, 49, 7);
            sessionCredentials.Password = DA.GetPasswordForBatchProcessing(sessionCredentials.UserName);
            Credentials ftpCredentials = DA.GetFtpCredentials();
#if !DEBUG
            if (string.IsNullOrEmpty(Recovery.RecoveryValue))
                RunAll(ftpCredentials, sessionCredentials, logRun);
            else
            {
#endif
                UserInput userInput = new UserInput();
                string recoveryPoint = Recovery.RecoveryValue.Split(',')[0];
                using (MainForm mainForm = new MainForm(userInput, recoveryPoint))
                {
                    while (mainForm.ShowDialog() == DialogResult.OK)
                    {
                        switch (userInput.Selected)
                        {
                            case UserInput.Selection.Continue:
                                switch (recoveryPoint)
                                {
                                    case RECOVERY_PHASE_CREATING:
                                        CreateRequestFile(sessionCredentials.UserName);
										goto case RECOVERY_PHASE_UPLOADED;
                                    case RECOVERY_PHASE_UPLOADED:
                                        SendRequestFile(ftpCredentials, sessionCredentials.UserName);
                                        goto case RECOVERY_PHASE_DOWNLOADED;
                                    case RECOVERY_PHASE_DOWNLOADED:
                                        GetResponseFile(ftpCredentials);
                                        goto case RECOVERY_PHASE_PROCESSING;
                                    case RECOVERY_PHASE_PROCESSING:
                                        ProcessResponseFile(sessionCredentials);
                                        break;
                                }//switch
                                break;
                            case UserInput.Selection.CreateRequestFile:
                                CreateRequestFile(sessionCredentials.UserName);
                                break;
                            case UserInput.Selection.SendRequestFile:
                                SendRequestFile(ftpCredentials, sessionCredentials.UserName);
                                break;
                            case UserInput.Selection.GetResponseFile:
                                GetResponseFile(ftpCredentials);
                                break;
                            case UserInput.Selection.ProcessResponseFile:
                                ProcessResponseFile(sessionCredentials);
                                ProcessingComplete();
                                break;
                        }//switch
                    }//while
                }//using
#if !DEBUG
			}
#endif
        }//Main()

        private void RunAll(Credentials ftpCredentials, Credentials sessionCredentials, ProcessLogRun logrun)
        {
            string reassignedTaskUserId = DA.GetUserIdForReassignedTasks();
            RequestFile requestFile = RequestFile.CreateFromQueueTasks(RI, Recovery, Eoj, sessionCredentials.UserName, reassignedTaskUserId, DA);
            if (requestFile != null)
            {
                requestFile.EncryptAndUpload(ftpCredentials);
                ResponseFile responseFile = ResponseFile.DownloadAll(RI, Recovery, ftpCredentials, logrun);
                if (responseFile == null)
                    NotifyAndEnd(string.Format("The Accurint response file was not successfully downloaded to {0}.", EnterpriseFileSystem.TempFolder));
                else
                    responseFile.Review(Err, Eoj, sessionCredentials);
            }//if
            if (Eoj.Counts[EOJ_ERRORS] > 0)
                NotifyOfErrors();

            ProcessingComplete();
        }//RunAll()

        private void CreateRequestFile(string userId)
        {
            string reassignedTaskUserId = DA.GetUserIdForReassignedTasks();
            //Check whether a file already exists.
            if (RequestFile.Exists())
            {
                string message = "An Accurint FED request file already exists. If you continue, new records will be added to the existing file. Proceed?";
                if (MessageBox.Show(message, ScriptId, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    string recoveryQueue = Recovery.RecoveryValue.Split(',')[1];
                    RequestFile.Recover(true, RI, Recovery, Eoj, recoveryQueue, userId, reassignedTaskUserId, DA);
                }
            }
            else
                RequestFile.CreateFromQueueTasks(RI, Recovery, Eoj, userId, reassignedTaskUserId, DA);
        }//CreateRequestFile()

        private void SendRequestFile(Credentials ftpCredentials, string userId)
        {
            string reassignedTaskUserId = DA.GetUserIdForReassignedTasks();
            RequestFile requestFile = RequestFile.Recover(false, RI, Recovery, Eoj, null, userId, reassignedTaskUserId, DA);
            if (!string.IsNullOrEmpty(Recovery.RecoveryValue) && Recovery.RecoveryValue.StartsWith(Accurint.RECOVERY_PHASE_UPLOADED)) { return; }
            requestFile.EncryptAndUpload(ftpCredentials);
        }//SendRequestFile()

        private void GetResponseFile(Credentials ftpCredentials)
        {
            if (!string.IsNullOrEmpty(Recovery.RecoveryValue) && Recovery.RecoveryValue.StartsWith(RECOVERY_PHASE_DOWNLOADED))
                return;

            ResponseFile responseFile = null;
            responseFile = ResponseFile.DownloadAll(RI, Recovery, ftpCredentials);
            if (responseFile == null)
                Dialog.Info.Ok(string.Format("The Accurint response file was not successfully downloaded to {0}.", EnterpriseFileSystem.TempFolder));
        }//GetResponseFile()

        private void ProcessResponseFile(Credentials sessionCreds)
        {

            ResponseFile responseFile = ResponseFile.GetDownloaded(RI, Recovery);
            if (responseFile == null)
                NotifyAndEnd(string.Format("No Accurint Response File in {0}.", EnterpriseFileSystem.TempFolder));
            else
                responseFile.Review(Err, Eoj, sessionCreds);

            if (Eoj.Counts[EOJ_ERRORS] > 0)
                NotifyOfErrors();
        }//ProcessResponseFile()

        private void NotifyOfErrors()
        {
            string errorReportDirectory = EnterpriseFileSystem.GetPath("ERR_Accurint");
            string to = DA.GetErrorNotificationRecipients();
            string from = "BatchScripts@utahsbr.edu";
            string subject = "Accurint Errors";
            string body = string.Format("Please work the errors in the {0} directory.", errorReportDirectory);
            string cc = Environment.UserName + "@utahsbr.edu";
            EmailHelper.SendMail(DataAccessHelper.TestMode, to, from, subject, body, cc, "", EmailHelper.EmailImportance.High, true);
        }//NotifyOfErrors()
    }//class
}//namespace
