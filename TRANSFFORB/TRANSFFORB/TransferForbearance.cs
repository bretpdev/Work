using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace TRANSFFORB
{
    public class TransferForbearance : BatchScript
    {
        private const string EojTotalFromSas = "Total number of records in the SAS file";
        private const string EojNumberOfErrors = "There was an error transferring the forbearance";
        private const string EojProcessed = "Number of records successfully processed";
        private static readonly string[] EojFields = { EojTotalFromSas, EojProcessed, EojNumberOfErrors };

        private const string CommentAdded = "CommentAdded";
        private const string ForbearanceAdded = "ForbearanceAdded";

        public TransferForbearance(ReflectionInterface ri)
            : base(ri, "TRANSFFORB", "ERR_BU01", "EOJ_BU01", EojFields, DataAccessHelper.Region.Uheaa)
        {
        }

        public override void Main()
        {
            StartupMessage("This script adds a transfer forbearance. Press OK to continue and Cancel to stop the script");

            //Set up the recovery fields
            int recoveryCounter = string.IsNullOrEmpty(Recovery.RecoveryValue) ? 0 : int.Parse(Recovery.RecoveryValue.SplitAndRemoveQuotes(",")[0]);

            string file = OpenFile();

            List<BorrowerData> fileData = GetBorrowerData(file);

            foreach (BorrowerData bor in fileData.Skip(recoveryCounter))
            {
                if (string.IsNullOrEmpty(Recovery.RecoveryValue) || Recovery.RecoveryValue.Contains(CommentAdded))
                {
                    if (!AddTheForbearance(bor, recoveryCounter))
                    {
                        recoveryCounter++;
                        Recovery.RecoveryValue = string.Format("{0},{1}", recoveryCounter, CommentAdded);
                        continue;
                    }
                }
                if (Recovery.RecoveryValue.Contains(ForbearanceAdded))
                {
                    if (!Atd22ByLoan(bor.AccountNumber, "PRFOR", "Transfer Forbearance successfully added", "", bor.LoanSeq, ScriptId, false))
                    {
                        Err.AddRecord("Unable to add a comment in TD22.  Forbearance has been added.", bor);
                        Eoj.Counts[EojNumberOfErrors]++;
                        recoveryCounter++;
                        Recovery.RecoveryValue = string.Format("{0},{1}", recoveryCounter, CommentAdded);
                        continue;
                    }

                    recoveryCounter++;
                    Recovery.RecoveryValue = string.Format("{0},{1}", recoveryCounter, CommentAdded);

                    Eoj.Counts[EojProcessed]++;
                }
            }

            File.Delete(file);

            if (Err.HasErrors)
                ProcessingComplete("Processing Complete\r\n\r\nThere were errors in processing. Please review the error report");
            else
                ProcessingComplete();
        }

        /// <summary>
        /// Verifies the correct document was selected and gets the file name
        /// </summary>
        /// <returns>File Name of the document</returns>
        private string OpenFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            while (true)
            {
                dialog.InitialDirectory = !string.IsNullOrEmpty(Recovery.RecoveryValue) ? EnterpriseFileSystem.TempFolder : EnterpriseFileSystem.FtpFolder;
                if (dialog.ShowDialog() == DialogResult.Cancel)
                    EndDllScript();
                if (dialog.FileName.ToUpper().Contains("TRANSFER FORBEARANCE UHEAA"))
                    break;
                else
                    MessageBox.Show("You must choose the a file name Transfer Forbearance UHEAA.csv", "Invalid File Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            string fileName = EnterpriseFileSystem.TempFolder + dialog.FileName.Substring(dialog.FileName.LastIndexOf("\\") + 1, dialog.FileName.Length - dialog.FileName.LastIndexOf("\\") - 1);

            if (string.IsNullOrEmpty(Recovery.RecoveryValue))
            {
                //Check for existence of file
                if (File.Exists(fileName))
                {
                    if (MessageBox.Show("There is already a file named Transfer Forbearance Uheaa.csv in the T: drive. Do you want to delete it?", "File already exists",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        File.Delete(fileName);
                    else
                        EndDllScript();
                }
                File.Move(dialog.FileName, fileName);
                if (!File.Exists(fileName))
                {
                    MessageBox.Show("There was an error moving the file from " + dialog.FileName + " to the T: drive.", "Error moving file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    EndDllScript();
                }
            }
            return fileName;
        }

        /// <summary>
        /// Loads the file into a BorrowerData List
        /// </summary>
        /// <param name="fileName">The location of the file</param>
        /// <returns>BorrowerData List</returns>
        private List<BorrowerData> GetBorrowerData(string fileName)
        {
            List<BorrowerData> data = new List<BorrowerData>();

            using (StreamReader sr = new StreamReader(fileName))
            {
                string header = sr.ReadLine();
                string borAcct = string.Empty;
                BorrowerData bor = new BorrowerData();

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (string.IsNullOrEmpty(line)) continue;
                    List<string> splitLine = line.SplitAndRemoveQuotes(",");

                    if (splitLine[0] == borAcct)
                    {
                        bor.LoanSeq.Add(int.Parse(splitLine[1]));
                        continue;
                    }
                    else if (splitLine[0] != borAcct && !string.IsNullOrEmpty(bor.AccountNumber))
                    {
                        data.Add(bor);
                        Eoj.Counts[EojTotalFromSas]++;
                    }

                    bor = new BorrowerData();
                    bor.LoanSeq = new List<int>();
                    bor.AccountNumber = splitLine[0];
                    borAcct = splitLine[0];
                    bor.LoanSeq.Add(int.Parse(splitLine[1]));
                    bor.DeliqDate = (DateTime.Parse(splitLine[2])).ToString("MMddyy");
                    bor.LoanDate = (DateTime.Parse(splitLine[3])).ToString("MMddyy");
                }
                data.Add(bor);
                Eoj.Counts[EojTotalFromSas]++;
            }

            return data;
        }

        /// <summary>
        /// Adds a transfer forbearance to borrowers account for each loan sequence number
        /// </summary>
        /// <param name="bor">BorrowerData object</param>
        /// <param name="recoveryCounter">Recovery counter</param>
        /// <returns></returns>
        private bool AddTheForbearance(BorrowerData bor, int recoveryCounter)
        {
            FastPath("TX3Z/ATS0H");
            PutText(7, 33, bor.AccountNumber, true);
            PutText(9, 33, "F");
            PutText(11, 33, DateTime.Now.ToString("MMddyy"), ReflectionInterface.Key.Enter, true);

            if (RI.ScreenCode != "TSX7E")
            {
                Err.AddRecord(string.Format("Unable to access the forbearance menu selection screen {0}", GetText(23, 2, 75)), bor);
                Eoj.Counts[EojNumberOfErrors]++;
                return false;
            }

            //Put in a forbearance type 17
            PutText(21, 13, "17", ReflectionInterface.Key.Enter, true);

            if (RI.MessageCode == "50108")
            {
                Err.AddRecord(string.Format("Unable to access the forbearance menu selection screen {0}", GetText(23, 2, 75)), bor);
                Eoj.Counts[EojNumberOfErrors]++;
                return false;
            }

            if (RI.ScreenCode == "TSXA5")
            {
                while (RI.MessageCode != "90007")
                {
                    for (int row = 12; row < 22; row++)
                    {
                        int? sequence = GetText(row, 13, 3).ToIntNullable();
                        if (!sequence.HasValue)  break; //reached the end of loans on page, stop searching page
                        if (!bor.LoanSeq.Contains(sequence.Value)) continue; //loan not in file. skip

                        string loanType = GetText(row, 17, 5);
                        if (loanType != "UNCNS" && loanType != "UNSPC")
                            if (CheckForText(row, 2, "_")) //make sure we can select the loan
                                PutText(row, 2, "X");
                    }
                    Hit(ReflectionInterface.Key.F8); //next page
                }
            }

            Hit(ReflectionInterface.Key.Enter);

            if (RI.ScreenCode != "TSX32")
            {
                Err.AddRecord(string.Format("Unable to add the forbearance {0}", GetText(23, 2, 75)), bor);
                Eoj.Counts[EojNumberOfErrors]++;
                return false;
            }

            PutText(7, 18, bor.DeliqDate);
            PutText(8, 18, bor.LoanDate);
            PutText(9, 18, DateTime.Now.ToString("MMddyy"));
            if (CheckForText(14, 50, "ELIGIBLITY"))
            {
                PutText(14, 73, "Y");
            }
            PutText(20, 17, "", true);
            Hit(ReflectionInterface.Key.F6);

            if (RI.MessageCode != "01004")
            {
                Err.AddRecord(string.Format("Unable to add the forbearance {0}", GetText(23, 2, 75)), bor);
                Eoj.Counts[EojNumberOfErrors]++;
                return false;
            }

            Recovery.RecoveryValue = string.Format("{0},{1}", recoveryCounter, ForbearanceAdded);
            return true;
        }
    }//class
}//namespace