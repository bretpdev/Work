using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace AUCONEMAIL
{
    public class AddUpdateContactEmailFed : FedBatchScript
    {
        private const string EojNumberOfBorrowers = "Number of borrowers in the file";
        private const string EojNumberOfEmailsUpdated = "Number of email address updated";
        private const string EojNumberOfErrors = "Number of errors";
        private const string FilePattern = "UNWS73.NWS73R";

        public AddUpdateContactEmailFed(ReflectionInterface ri)
            : base(ri, "AUCONEMAIL", "ERR_BU35", "EOJ_BU35", new List<string>() { EojNumberOfBorrowers, EojNumberOfEmailsUpdated, EojNumberOfErrors })
        {
        }

        public override void Main()
        {
            string file = CheckRecovery();

            //if no recovery file is found allow the user to choose a file.
            if (file.IsNullOrEmpty())
                file = ChooseFile();

            ProcessFile(file);
            FileHelper.DeleteFile(file, ProcessLogData.ProcessLogId, ProcessLogData.ExecutingAssembly);
            ProcessingComplete();
        }

        /// <summary>
        /// Processes the file selected by the user, or the recovery file.
        /// </summary>
        /// <param name="file">File to process.</param>
        private void ProcessFile(string file)
        {
            int recoveryCount = 0;
            int lineCount = 0;
            int.TryParse(Recovery.RecoveryValue, out recoveryCount);
            using (StreamReader sr = new StreamReader(file))
            {
                //Read in header.
                sr.ReadLine();
                //Catch up recovery if needed.
                while (lineCount != recoveryCount)
                {
                    sr.ReadLine();
                    lineCount++;
                }

                while (!sr.EndOfStream)
                {
                    List<string> line = sr.ReadLine().SplitAndRemoveQuotes(",");
                    UpdateEmail(line);
                    Recovery.RecoveryValue = lineCount++.ToString();
                    Eoj.Counts[EojNumberOfBorrowers].Increment();
                }
            }
        }

        /// <summary>
        /// Access TX1J email screen and enters email address.
        /// </summary>
        /// <param name="line">FIle line.</param>
        private void UpdateEmail(List<string> line)
        {
            FastPath("TX3Z/CTX1JB*");
            //Clear out the SSN field
            PutText(6, 16, "", true);
            PutText(6, 20, "", true);
            PutText(6, 23, "", true);
            PutText(6, 61, line[0], ReflectionInterface.Key.Enter);//Account Number

            //Access the email screen
            Hit(ReflectionInterface.Key.F2);
            Hit(ReflectionInterface.Key.F10);

            if (!CheckForText(10, 14, "C"))
                PutText(10, 14, "C", ReflectionInterface.Key.Enter);//Contact Email Type.

            PutText(9, 20, line[2]);//Source Code
      
            string formatedValidityDate = line[3].Substring(0, 2) + line[3].Substring(3, 2) + line[3].Substring(8, 2);
            PutText(11, 17, formatedValidityDate);//Email Address Last Verified Date.
            PutText(12, 14, "Y");//Since the SAS is only pulling valid emails we can put Y for every email.

            if (!EnterEmailAddress(line[1]))
            {
                string errorMessage = string.Format("Unable to up the email address for the following borrower.  Account Number: {0} Email: {1} SourceCode: {2} ValidityDate:{3} Error: {4}",
                    line[0], line[1], line[2], line[3], RI.Message);
                ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, errorMessage, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                Eoj.Counts[EojNumberOfErrors].Increment();
            }
            else
                Eoj.Counts[EojNumberOfEmailsUpdated].Increment();

        }

        /// <summary>
        /// Enters the borrowers email address on the email screen.
        /// </summary>
        /// <param name="email">Borrowers Email Address.</param>
        /// <returns>True if the email was updated.</returns>
        private bool EnterEmailAddress(string email)
        {
            if (email.Length < 59)
                RI.PutText(14, 10, email, true);
            else
            {
                int row = 14;
                for (int i = 0; i > email.Length; i += 59)
                {
                    if (i > 59)
                        RI.PutText(row, 10, email.Substring(i, 59), true);
                    else
                        RI.PutText(row, 10, email.Substring(i, (((row - 14) * 59) - email.Length)));//Take the starting row and minus that from the current row to get the number of 59 character sets we have used and get the length of the original email address

                    row++;
                }
            }

            RI.Hit(ReflectionInterface.Key.Enter);

            if (!RI.MessageCode.IsIn("01003", "01005"))
                return false;

            return true;

        }

        /// <summary>
        /// Checks to see if the user is in recovery and if a file exists on the users T drive.
        /// </summary>
        /// <returns>Recovery file, Returns null if no file is found.</returns>
        private string CheckRecovery()
        {
            string[] recoveryFile = Directory.GetFiles(EnterpriseFileSystem.TempFolder, FilePattern + "*");
            if (recoveryFile.Count() != 0)
            {
                if (Recovery.RecoveryValue.IsNullOrEmpty())
                {
                    MessageBox.Show("The script has found a file on your T drive, but no recovery log exists.  Please review the file to determine if it has already been processed.");
                    EndDllScript();
                }
            }
            else if (!Recovery.RecoveryValue.IsNullOrEmpty())
            {
                if (recoveryFile.Count() == 0)
                {
                    MessageBox.Show("The script is in recovery, but no recovery file exists.  Please review and try again.");
                    EndDllScript();
                }
            }
            else if (recoveryFile.Count() > 1)
            {
                MessageBox.Show("The script is in recovery, but multiple files exist on your T drive.  Please review and try again.");
                EndDllScript();
            }

            return recoveryFile.FirstOrDefault();
        }

        /// <summary>
        /// Allows the user to choose a file to process and moves the file to the T drive.
        /// </summary>
        /// <returns>Full path of the file.</returns>
        private string ChooseFile()
        {
            using (OpenFileDialog fileChooser = new OpenFileDialog())
            {
                fileChooser.InitialDirectory = EnterpriseFileSystem.FtpFolder;

                bool validFile = false;
                do
                {
                    if (fileChooser.ShowDialog() == DialogResult.Cancel)
                        EndDllScript();

                    if (!fileChooser.SafeFileName.Contains(FilePattern))
                        MessageBox.Show(string.Format("The filename must contain {0}.  Please try again.", FilePattern), "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        validFile = true;
                }
                while (!validFile);

                string path = Path.Combine(EnterpriseFileSystem.TempFolder, fileChooser.SafeFileName);

                File.Move(fileChooser.FileName, path);

                return path;
            }
        }
    }
}
