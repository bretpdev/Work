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

namespace WLCLETALGN
{
    public partial class AlignWelcomeLetters
    {
        /// <summary>
        /// Reads the selected file into a List<BorrowerData> and gets the count
        /// </summary>
        /// <param name="fileName">The selected file to read in</param>
        public void GetBorrowerCount(string fileName)
        {
            Borrowers = new List<BorrowerData>();
            using (StreamReader sr = new StreamReader(fileName))
            {
                string header = sr.ReadLine(); //Get rid of the header
                while (!sr.EndOfStream)
                {
                    BorrowerData data = GetData(sr.ReadLine());
                    if (data == null)
                    {
                        ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, "Invalid File", NotificationType.FileFormatProblem, NotificationSeverityType.Warning);
                        MessageBox.Show(GetMessage(), "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                        Borrowers.Add(data);
                    if (Rec.RecoveryValue.IsNullOrEmpty())
                        EndOfJob.Counts[EOJ_Total].Increment();
                }
            }
        }

        /// <summary>
        /// Gets the error message for an invalid file format
        /// </summary>
        /// <returns>The error message with the fields the application is looking for</returns>
        private string GetMessage()
        {
            string message = "This file is not in the correct format for this application. Make sure your file has the following fields and try again.\r\n\r\n" +
                "AccountNumber\r\nFirstName\r\nLastName\r\nStreet1\r\nStreet2\r\nCity\r\nState\r\nZip\r\nForeignCountry\r\nForeignState\r\nACSKeyline";
            return message;
        }

        /// <summary>
        /// Split the current line from the file and assign the data to a BorrowerData object
        /// </summary>
        /// <param name="line">Comma Delimited line from the sas file</param>
        /// <returns>BorrowerData object</returns>
        private BorrowerData GetData(string line)
        {
            BorrowerData data = new BorrowerData();
            List<string> splitLine = line.SplitAndRemoveQuotes(",");
            if (splitLine.Count != 11) { return null; } //If the data has more or less values, return null
            data.AccountNumber = splitLine[0];
            data.Firstname = splitLine[1];
            data.Lastname = splitLine[2];
            data.Address1 = splitLine[3];
            data.Address2 = splitLine[4];
            data.City = splitLine[5];
            data.State = splitLine[6];
            data.Zip = splitLine[7];
            data.Country = splitLine[8];
            data.KeyLine = splitLine[10];
            return data;
        }

        /// <summary>
        /// Uses CostCenterPrinting to merge the new data file with the ALIGNWLCME.doc.
        /// </summary>
        public void Print()
        {
            if (Rec.RecoveryValue.SplitAndRemoveQuotes(",")[0] != "Printed")
            {
                string dataFile = CreateDataFile();
                DocumentProcessing.CostCenterPrinting("ALIGNWLCME", dataFile, "State", ScriptId, "AccountNumber", DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.CostCenterOptions.AddBarcode, "CostCenter");
                FileHelper.DeleteFile(dataFile, ProcessLogData.ProcessLogId, ProcessLogData.ExecutingAssembly);
                RecoveryValues[0] = "Printed";
                UpdateRecovery();
            }
            Processing.FinishedPrinting(); //Abort the thread
        }

        /// <summary>
        /// Create a data file to merge with the letter
        /// </summary>
        private string CreateDataFile()
        {
            string file = EnterpriseFileSystem.TempFolder + "PrintData_" + UserId + ".txt";
            using (StreamWriter sw = new StreamWriter(file))
            {
                sw.WriteLine("KeyLine, AccountNumber, FirstName, LastName, Address1, Address2, City, State, Zip, Country, CostCenter");
                foreach (BorrowerData data in Borrowers)
                {
                    sw.WriteLine(string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, MA2324", data.KeyLine, data.AccountNumber, data.Firstname, data.Lastname,
                        data.Address1, data.Address2, data.City, data.State, data.Zip, data.Country));
                }
            }
            return file;
        }

        /// <summary>
        /// Adds an ALIGN arc in TD22 for each borrower in the file.
        /// </summary>
        public void AddComments()
        {
            int counter = Rec.RecoveryValue.SplitAndRemoveQuotes(",")[2].IsNullOrEmpty() ? 0 : int.Parse(Rec.RecoveryValue.SplitAndRemoveQuotes(",")[2]);
            if (counter > 0) Processing.IncrementProgress(counter); //Set the progress bar value to the recovery value
            foreach (BorrowerData borr in Borrowers.Skip(counter))
            {
                if (IsCommenting)
                {
                    if (Atd22AllLoans(borr.AccountNumber, "ALIGN", "Welcome letter sent to Borrower", "", ScriptId, false))
                        EndOfJob.Counts[EOJ_TotalArcProcessed].Increment();
                    else
                    {
                        EndOfJob.Counts[ERR_AddingArc].Increment();
                        ErrReport.AddRecord(ERR_AddingArc, borr);
                    }
                    Processing.IncrementProgress();
                    counter++;
                    RecoveryValues[1] = "Commenting";
                    RecoveryValues[2] = counter.ToString();
                    UpdateRecovery();
                }
                else
                    break;
            }
            if (!Processing.DidCancel && counter == Borrowers.Count)
            {
                RecoveryValues[1] = "Commented";
                RecoveryValues[2] = "0";
                UpdateRecovery();
                Processing.FinishedComments(); //Abort the thread
            }
        }

        /// <summary>
        /// Checks to see if in recovery and loads the recovery data. Creates a lock and updates the recovery log with the current recovery information.
        /// </summary>
        public void UpdateRecovery()
        {
            if (Processing.ShouldRecover)
            {
                List<string> splitRec = Rec.RecoveryValue.SplitAndRemoveQuotes(",");
                RecoveryValues[0] = splitRec[0];
                RecoveryValues[1] = splitRec[1];
                RecoveryValues[2] = splitRec[2];
                RecoveryValues[3] = splitRec[3];
                Processing.ShouldRecover = false;
            }
            lock (Lock) //Set a lock on the recovery object so it only updates once per thread
                Rec.RecoveryValue = string.Format("{0},{1},{2},{3}", RecoveryValues[0], RecoveryValues[1], RecoveryValues[2], RecoveryValues[3]);
        }

        /// <summary>
        /// Ends the ProcessLogger and calls processingcomplete
        /// </summary>
        public void EndProcessLog()
        {
            if (EndOfJob != null)
            {
                if (ProcessLogData != null && ProcessLogData.ProcessLogId > 0)
                    EndOfJob.PublishProcessLogger(ProcessLogData);
                EndOfJob.Publish();
            }
            ErrReport.Publish();
            Rec.Delete();
            FileHelper.DeleteFile(FileName, ProcessLogData.ProcessLogId, ProcessLogData.ExecutingAssembly);
            ProcessLogger.LogEnd(ProcessLogData.ProcessLogId);

            File.Create(string.Format("{0}MBS{1}_{2}_{3}.TXT", EnterpriseFileSystem.LogsFolder, ScriptId, UserId, DateTime.Now.ToString("MMddyyyy_hhmmss")));
            Recovery.Delete();

            if (!CalledByJams)
            {
                MessageBox.Show("Process Complete", ScriptId, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}