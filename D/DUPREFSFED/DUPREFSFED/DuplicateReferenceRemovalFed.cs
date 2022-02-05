using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace DUPREFSFED
{
    public class DuplicateReferenceRemovalFed : FedBatchScript
    {
        private const string NumberOfReferencesInFile = "# of References in File";
        private const string NumberOfErrors = "# ofErrors";
        private const string NumberOfReferencesProcessed = "# of References Processed";
        private string FileToProcess { get { return "DUPLREFS FED.TXT"; } }

        public DuplicateReferenceRemovalFed(ReflectionInterface ri)
            : base(ri, "DUPREFSFED", "ERR_BU35", "EOJ_BU35", new List<string>() { NumberOfReferencesInFile, NumberOfReferencesProcessed, NumberOfErrors })
        { }

        public override void Main()
        {
            List<string> files = Directory.GetFiles(EnterpriseFileSystem.FtpFolder, FileToProcess).ToList();

            CheckNumberOfFiles(files);

            //If we passed CheckNumberOfFiles we know that there is only 1 file in FTP
            string file = files[0];

            CheckFileLength(file);
            CheckFileFormat(file);
            ProcessFile(file);

            //We are done delete the file
            File.Delete(file);
            ProcessingComplete();
        }

        /// <summary>
        /// Checks the Number of files.  If there are no file or more than one the user will get an error message and the script will end.
        /// </summary>
        /// <param name="files">List of all possible files in FTP.</param>
        private void CheckNumberOfFiles(List<string> files)
        {
            string message = string.Empty;
            if (files.Count == 0)
            {
                message = string.Format("The {0} file was not found in {1}", FileToProcess, EnterpriseFileSystem.FtpFolder);
                ShowMessageAndLogProcessEndScript(message, NotificationType.NoFile, NotificationSeverityType.Informational);
            }
            else if (files.Count > 1)
            {
                message = string.Format("Multiple {0} files were found in {1}", FileToProcess, EnterpriseFileSystem.FtpFolder);
                ShowMessageAndLogProcessEndScript(message, NotificationType.ErrorReport, NotificationSeverityType.Informational);
            }
        }

        /// <summary>
        /// Reads in in the file and processes each line.
        /// </summary>
        /// <param name="file">File to process.</param>
        private void ProcessFile(string file)
        {
            using (StreamReader sr = new StreamReader(file))
            {
                //Read the header line
                sr.ReadLine();

                //Get the recovery counter line.  if no recovery then start at 1.
                int recoveryCounter = Recovery.RecoveryValue.IsNullOrEmpty() ? 1 : Recovery.RecoveryValue.ToInt();

                for (int count = 1; !sr.EndOfStream; count++)
                {
                    //if in recovery read though until we get to the reference we ended on.
                    if (recoveryCounter > count)
                    {
                        sr.ReadLine();
                        continue;
                    }

                    Recovery.RecoveryValue = count.ToString();

                    if (!ProcessReference(sr.ReadLine().SplitAndRemoveQuotes(",")[1]))
                        Eoj.Counts[NumberOfErrors].Increment();
                    else
                        Eoj.Counts[NumberOfReferencesProcessed].Increment();

                    Eoj.Counts[NumberOfReferencesInFile].Increment();
                }
            }
        }

        /// <summary>
        /// Accesses CTX1J and inactivates the reference.
        /// </summary>
        /// <param name="referenceId">ReferenceId to inactivate.</param>
        /// <returns>True if the reference was inactivated.</returns>
        private bool ProcessReference(string referenceId)
        {
            FastPath("TX3Z/CTX1JR;" + referenceId);

            //Move to the next section on TX1J
            Hit(ReflectionInterface.Key.F6);

            //Update status to H (History)
            PutText(7, 66, "H");
            //Update History Reason to X (Duplicate)
            PutText(9, 59, "X", ReflectionInterface.Key.Enter);

            if (RI.MessageCode == "01094")
                return true;
            else
            {
                Err.AddRecord("", new { ReferenceId = referenceId, ErrorMessage = RI.Message });
                ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, string.Format("Unable to update Reference Id: {0} Error: {1}", referenceId, RI.Message), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return false;
            }
        }

        /// <summary>
        /// Checks to make sure the file is in the correct format.
        /// </summary>
        /// <param name="file">File to check.</param>
        private void CheckFileFormat(string file)
        {
            using (StreamReader sr = new StreamReader(file))
            {
                if (sr.ReadLine().SplitAndRemoveQuotes(",").Count != 6)
                {
                    string message = string.Format("The file {0} in {1} not in the correct format.", Path.GetFileName(file), Path.GetDirectoryName(file));
                    ShowMessageAndLogProcessEndScript(message, NotificationType.ErrorReport, NotificationSeverityType.Informational);
                }
            }
        }

        /// <summary>
        /// Checks to see if the file is empty.  If the file is empty the user will get a prompt and the script will end.
        /// </summary>
        /// <param name="file">File to check.</param>
        private void CheckFileLength(string file)
        {
            FileInfo fi = new FileInfo(file);
            if (fi.Length == 0)
            {
                string message = string.Format("The file {0} in {1} is empty.", Path.GetFileName(file), Path.GetDirectoryName(file));
                ShowMessageAndLogProcessEndScript(message, NotificationType.EmptyFile, NotificationSeverityType.Informational);
                File.Delete(file);
                EndDllScript();
            }
        }

        /// <summary>
        /// Shows a message to the User and ends the script.
        /// </summary>
        /// <param name="message">Message to display.</param>
        /// <param name="type">ProcessLog Notification Type</param>
        /// <param name="sevType">ProcessLog Notification Severity Type</param>
        private void ShowMessageAndLogProcessEndScript(string message, NotificationType type, NotificationSeverityType sevType)
        {
            Dialog.Info.Ok(message);
            ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, message, type, sevType);
            if (message.Contains("empty"))
                return;
            EndDllScript();
        }
    }
}
