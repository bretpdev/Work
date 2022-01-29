using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.Scripts;
using Uheaa.Common.DataAccess;
using System.Collections.Generic;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;

namespace ACURINTFED
{
    public class RequestFile
    {
        private const string RECOVERY_STEP_ADDED_TO_FILE = "Account added to request file";
        private const string RECOVERY_STEP_COMMENT_ADDED = "comment added";
        private const string RECOVERY_STEP_TASK_CLOSED = "Task closed";
        private const string BASE_FILE_NAME = "AccurintFEDRequestFile.txt";
        private readonly string ArchiveFolder;
        private readonly EndOfJobReport Eoj;
        private readonly string FileName;
        private readonly string ReassignedTaskUserId;
        private readonly RecoveryLog Recovery;
        private readonly string UserId;
        private ReflectionInterface RI { get; set; }
        private static string ScriptId { get; set; }

        private RequestFile(ReflectionInterface ri, RecoveryLog recoveryLog, EndOfJobReport eoj, string userId, string reassignedTaskUserId, string scriptId)
        {
            FileName = EnterpriseFileSystem.TempFolder + BASE_FILE_NAME;
            ArchiveFolder = EnterpriseFileSystem.GetPath("Accurint Archive");
            Eoj = eoj;
            ReassignedTaskUserId = reassignedTaskUserId;
            Recovery = recoveryLog;
            UserId = userId;
            RI = ri;
            ScriptId = scriptId;
        }//ctor

        public static bool Exists()
        {
            return File.Exists(Path.Combine(EnterpriseFileSystem.TempFolder, BASE_FILE_NAME));
        }//Exists()

        public static int RecordCount(bool testMode)
        {
            string fileName = EnterpriseFileSystem.TempFolder + BASE_FILE_NAME;
            return (File.Exists(fileName) ? File.ReadAllLines(fileName).Length : 0);
        }//RecordCount()

        #region Factory methods
        public static RequestFile CreateFromQueueTasks(ReflectionInterface ri, RecoveryLog recoveryLog, EndOfJobReport eoj, string userId, string reassignedTaskUserId, DataAccess da)
        {
            return CreateFromQueueTasks(ri, recoveryLog, eoj, null, userId, reassignedTaskUserId, da);
        }//CreateFromQueueTasks()

        public static RequestFile Recover(bool finishBuildingFromQueueTasks, ReflectionInterface ri, RecoveryLog recoveryLog, EndOfJobReport eoj, string recoveryQueue, string userId, string reassignedTaskUserId, DataAccess da)
        {
            if (finishBuildingFromQueueTasks)
                return CreateFromQueueTasks(ri, recoveryLog, eoj, recoveryQueue, userId, reassignedTaskUserId, da);
            else
                return new RequestFile(ri, recoveryLog, eoj, userId, reassignedTaskUserId, ScriptId);
        }//Recover()

        private static RequestFile CreateFromQueueTasks(ReflectionInterface ri, RecoveryLog recoveryLog, EndOfJobReport eoj, string recoveryQueue, string userId, string reassignedTaskUserId, DataAccess da)
        {
            RequestFile requestFile = new RequestFile(ri, recoveryLog, eoj, userId, reassignedTaskUserId, ScriptId);
            string[] queues = da.GetQueues();
            //Start with the recovery queue if there is one, or the first queue in the array otherwise.
            string startingQueue = (string.IsNullOrEmpty(recoveryQueue) ? queues[0] : recoveryQueue);
            foreach (string queue in queues.SkipWhile(p => p != startingQueue))
            {
                requestFile.WorkQueue(queue, userId, reassignedTaskUserId);
                recoveryLog.Delete();
            }
            if (File.Exists(string.Format("{0}{1}", EnterpriseFileSystem.TempFolder, BASE_FILE_NAME)))
                return requestFile;
            else
                return null;
        }//CreateFromQueueTasks()
        #endregion Factory methods

        public void EncryptAndUpload(Credentials ftpCredentials)
        {
            //Warn the user if the request file is missing.
            if (!File.Exists(FileName))
                throw new Exception(string.Format("The {0} file is missing.", FileName));

            //Archive a copy of the file, using a unique file name.
            string uniqueFileName = string.Format("AccurintFEDRequestFile_{0:MMddyyyyhhmmss}.txt", DateTime.Now);
            File.Copy(FileName, ArchiveFolder + uniqueFileName, true);

            //Encrypt the file.
            string encryptedFile = EnterpriseFileSystem.TempFolder + uniqueFileName + ".pgp";
            if (!FileEncryption.EncryptFile(FileName, encryptedFile)) { throw new Exception("Encrypted file is not created"); }

            //Upload the encrypted file, unless we're in test mode.
            FtpHandler ftp = new FtpHandler(ftpCredentials);
            if (DataAccessHelper.CurrentMode != DataAccessHelper.Mode.Live || ftp.UploadFile(encryptedFile))
            {
                if (DataAccessHelper.CurrentMode != DataAccessHelper.Mode.Live)
                    MessageBox.Show(string.Format("Save the encrypted file: {0}.  \n\nPress ok to delete it.", encryptedFile));

                File.Delete(FileName);
                File.Delete(encryptedFile);

                Recovery.RecoveryValue = Accurint.RECOVERY_PHASE_UPLOADED;
            }
            else
                throw new Exception("FTP process failed");
        }//EncryptAndUpload()

        private List<int> GetEndorserLoans(string borrowerSsn, string endorserSsn)
        {
            RI.FastPath("TX3Z/ITX1JB" + borrowerSsn);
            RI.Hit(Key.F2);
            RI.Hit(Key.F4);
            List<int> loans = new List<int>();

            for (int row = 10; RI.MessageCode != "90007"; row++)
            {
                if (row > 21 || RI.GetText(row, 3, 1).IsNullOrEmpty())
                {
                    RI.Hit(Key.F8);
                    row = 9;
                    continue;
                }

                if (RI.CheckForText(row, 5, "E") && RI.CheckForText(row, 13, endorserSsn))
                    loans.Add(RI.GetText(row, 9, 3).ToInt());
            }

            return loans;
        }

        private void AddToRequestFile(SystemBorrowerDemographics demos)
        {
            using (StreamWriter fileWriter = new StreamWriter(FileName, true))
            {
                fileWriter.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8}", demos.Ssn, demos.LastName, demos.FirstName, demos.DateOfBirth, demos.Address1, demos.Address2, demos.City, demos.State, demos.ZipCode.Replace("-", ""));
            }
        }//AddToRequestFile()

        private bool CompleteTask(string ssn, string queue, string endorserSsn = null)
        {
            if (string.IsNullOrEmpty(Recovery.RecoveryValue) || Recovery.RecoveryValue.Split(',').Length < 3 || Recovery.RecoveryValue.Split(',')[2] != RECOVERY_STEP_COMMENT_ADDED)
            {
                if (queue.Contains("IO") || queue.Contains("IM"))
                {
                    List<int> loans = GetEndorserLoans(ssn, endorserSsn);

                    if (!RI.Atd22ByLoan(ssn, "KUESS", "Account sent to Accurint", endorserSsn, loans, ScriptId, false))
                        return false;
                }
                else
                    if (!RI.Atd22ByBalance(ssn, "KUBSS", "Account sent to Accurint", "", ScriptId, false, false)) { return false; }

                Recovery.RecoveryValue = string.Format("{0},{1},{2}", Accurint.RECOVERY_PHASE_CREATING.ToString(), queue, RECOVERY_STEP_COMMENT_ADDED);
            }
            if (!string.IsNullOrEmpty(Recovery.RecoveryValue) && Recovery.RecoveryValue.Split(',').Length >= 3 && Recovery.RecoveryValue.Split(',')[2] == RECOVERY_STEP_TASK_CLOSED)
                return true;
            else
            {
                string message = RI.CloseCompassQueue(queue, "C", "NOCTC");

                bool success = message.Contains("01005");
                if (success) 
                Recovery.RecoveryValue = string.Format("{0},{1},{2}", Accurint.RECOVERY_PHASE_CREATING, queue, RECOVERY_STEP_TASK_CLOSED);
                return success;
            }
        }//CompleteTask()

        private bool ReassignTask(string queue, string assignedToUserId, string reassignToUserId)
        {
            //Break the full queue name into queue and subqueue so they can be entered on separate lines.
            string[] queueComponents = queue.Split(';');

            //Go into the open queue task.
            RI.FastPath("TX3Z/CTX6J");
            RI.PutText(7, 42, queueComponents[0]);
            RI.PutText(8, 42, queueComponents[1]);
            RI.PutText(13, 42, assignedToUserId);
            RI.PutText(9, 42, "", true);
            RI.Hit(ReflectionInterface.Key.Enter);

            //Reassign it.
            try { RI.PutText(8, 15, reassignToUserId); }
            catch (Exception) { return false; } //Current user can't write to the ASSIGNED TO field.
            RI.Hit(ReflectionInterface.Key.Enter);
            return RI.CheckForText(23, 2, "01005");
        }//ReassignTask()

        private bool SelectNextTask(string queue)
        {
            RI.FastPath("TX3Z/ITX6X" + queue);
            if (RI.CheckForText(23, 2, "01020")) { return false; }
            if (RI.CheckForText(1, 72, "TXX6Y"))
            {
                MessageBox.Show(string.Format("Unable to access queue {0}. ", queue));
                throw new EndDLLException();
            }

            string taskStatus = "U";
            if (!string.IsNullOrEmpty(Recovery.RecoveryValue))
            {
                string[] recoveryFields = Recovery.RecoveryValue.Split(',');
                if (recoveryFields.Length >= 3 && recoveryFields[2] != RECOVERY_STEP_TASK_CLOSED)
                    taskStatus = "W";
            }
            while (!RI.CheckForText(23, 2, "90007 NO MORE DATA TO DISPLAY"))
            {
                for (int row = 8; !RI.CheckForText(row, 4, " "); row += 3)
                {
                    if (RI.CheckForText(row, 75, taskStatus))
                    {
                        RI.PutText(21, 18, RI.GetText(row, 4, 1), ReflectionInterface.Key.Enter);
                        return true;
                    }
                }//for
                RI.Hit(ReflectionInterface.Key.F8);
            }//while
            return false;
        }//SelectNextTask()

        private void WorkQueue(string queue, string userId, string reassignedTaskUserId)
        {
            while (SelectNextTask(queue))
            {
                string endorserSsn = string.Empty;
                string ssn = RI.GetText(4, 16, 11).Replace(" ", "");
                if (string.IsNullOrEmpty(Recovery.RecoveryValue) || (Recovery.RecoveryValue.Split(',').Length >= 3 && Recovery.RecoveryValue.Split(',')[2] == RECOVERY_STEP_TASK_CLOSED))
                {
                    if (queue.Contains("IM") || queue.Contains("IO"))
                        endorserSsn = RI.GetText(5, 16, 3) + RI.GetText(5, 20, 2) + RI.GetText(5, 23, 4);

                    SystemBorrowerDemographics demos = RI.GetDemographicsFromTx1j(endorserSsn.IsNullOrEmpty() ? ssn : endorserSsn);
                    AddToRequestFile(demos);
                    Eoj.Counts[Accurint.EOJ_SENT_TO_ACCURINT].Increment();
                    Recovery.RecoveryValue = string.Format("{0},{1},{2}", Accurint.RECOVERY_PHASE_CREATING, queue, RECOVERY_STEP_ADDED_TO_FILE);
                }

#if DEBUG
                //if (MessageBox.Show("Would you like to close the current task?", "New Task", MessageBoxButtons.YesNo) == DialogResult.No)
                //    return;
#endif

                if (!CompleteTask(ssn, queue, endorserSsn) && !ReassignTask(queue, userId, reassignedTaskUserId))
                    throw new Exception("Error Assigning Task to Manager");
#if DEBUG
                //if (MessageBox.Show("Would you like to process another task?", "New Task", MessageBoxButtons.YesNo) == DialogResult.No)
                //    return;
#endif
            }//while
        }//WorkQueue()
    }//class
}//namespace
