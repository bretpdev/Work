using System.Collections.Generic;
using System.IO;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace BTCHLTRS
{
    public class BatchLetters : BatchScript
    {
        private bool ChangePrinterSettings { get; set; }
        public BatchLetters(ReflectionInterface ri)
            : base(ri, "BTCHLTRS", "ERR_BU35", "EOJ_BU35", new List<string>() { string.Empty }, DataAccessHelper.Region.Uheaa)
        {
            ChangePrinterSettings = true;
        }

        public override void Main()
        {
            if (DataAccessHelper.TestMode)
            {
                if (!Dialog.Info.YesNo("Do you want to have the script change your printer settings automatically?"))
                    ChangePrinterSettings = false;
            }

            StartupMessage("This Is BatchLetters (UHEAA).  Please set the printer to Simplex and click OK to continue, or Cancel to quit.");
            PrintLetters();
            ProcessingComplete();
        }

        /// <summary>
        /// When in recovery will finish processing the printing.
        /// </summary>
        private void FinishRecovery()
        {
            List<string> recoveryValues = Recovery.RecoveryValue.SplitAndRemoveQuotes(",");
            DatabaseData item = DataAccess.GetRecoveryFileData(recoveryValues[2].ToInt());
            string file = recoveryValues[1];
            if (recoveryValues[0] == "ARC")
                AddArc(file, item);
            else
            {
                if (ChangePrinterSettings)
                {
                    PrinterInfo pi = new PrinterInfo(item.IsDuplex);
                    pi.ChangePrinterSettings();
                }
                DoPrinting(file, item, !item.Arc.IsNullOrEmpty());
            }

            File.Delete(file);
        }

        /// <summary>
        /// Generates ecorr letters and sends non ecorr letters to the printer.
        /// </summary>
        /// <param name="file">File to process.</param>
        /// <param name="item">DatabaseData object for letter campaign.</param>
        private void DoPrinting(string file, DatabaseData item, bool addArc)
        {
            DocumentProcessing.CostCenterPrinting(item.LetterId, file, item.StateFieldCodeName, ScriptId, item.AccountNumberFieldName, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.CostCenterOptions.AddBarcode, item.CostcenterFieldCodeName);

            if (!addArc)
                File.Delete(file);
            Recovery.Delete();
        }

        private bool Processletter(DatabaseData item)
        {
            List<string> files = CheckFile(item);

            foreach (string file in files)
                if (!ProcessFile(item, file))
                    continue;

            return true;
        }

        private bool ProcessFile(DatabaseData item, string file)
        {
            if (IsSasFileEmpty(file))//If the file is empty then delete the file and continue to the next file.
            {
                File.Delete(file);
                return false;
            }

<<<<<<< HEAD
            if (file.Contains("ULWS11.LWS11"))//This SAS file does not have a header so the code needs to add one.
=======
			//Removed as part of request 4438 as the sas does provide a header row now.
            /*if (file.Contains("ULWS11.LWS11"))//This SAS file does not have a header so the code needs to add one.
>>>>>>> 828da4f89f093b26bf5ef8094867481eeee9ff93
            {
                string fileWithHeader = AddHeaders(file);
                DoPrinting(fileWithHeader, item, !item.Arc.IsNullOrEmpty());
                File.Delete(fileWithHeader);
            }
<<<<<<< HEAD
            else
                DoPrinting(file, item, !item.Arc.IsNullOrEmpty());
=======
            else*/
            DoPrinting(file, item, !item.Arc.IsNullOrEmpty());
>>>>>>> 828da4f89f093b26bf5ef8094867481eeee9ff93

            if (!item.Arc.IsNullOrEmpty())
                AddArc(file, item);

            File.Delete(file);
            return true;
        }

        private List<string> CheckFile(DatabaseData item)
        {
            List<string> files = new List<string>();

            if (item.ProcessAllFiles)
                files = Directory.GetFiles(EnterpriseFileSystem.FtpFolder, item.SasFilePattern).ToList();
            else
            {
                string file = FileSystemHelper.DeleteOldFilesReturnMostCurrent(EnterpriseFileSystem.FtpFolder, item.SasFilePattern);
                if (!file.IsNullOrEmpty())
                    files.Add(file);
            }

            if (!item.OkIfMissing && files.Count == 0)
                ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, "The following file was missing: " + item.SasFilePattern, NotificationType.NoFile, NotificationSeverityType.Informational);
            return files;
        }

        /// <summary>
        /// Gets all the letter records and sends them to cost center printing.
        /// </summary>
        private void PrintLetters()
        {
            if (!Recovery.RecoveryValue.IsNullOrEmpty())
                FinishRecovery();
            PrinterInfo pi = new PrinterInfo(false);

            if (ChangePrinterSettings)
                pi.ChangePrinterSettings();

            List<DatabaseData> data = DataAccess.GetLettersFromDb();
            foreach (DatabaseData item in data.Where(p => !p.IsDuplex))
                Processletter(item);

            if (ChangePrinterSettings)
            {
                pi.Duplex = 2;
                pi.ChangePrinterSettings();
            }
            else
                Dialog.Info.Ok("Please change the printer to Duplex");

            foreach (DatabaseData item in data.Where(p => p.IsDuplex))
                Processletter(item);
        }

<<<<<<< HEAD
        private string AddHeaders(string file)
=======
		//Removed as part of 4438 as it is no longer needed.
        /*private string AddHeaders(string file)
>>>>>>> 828da4f89f093b26bf5ef8094867481eeee9ff93
        {
            string newFile = Path.Combine(EnterpriseFileSystem.TempFolder, ("HeaderFile_" + Path.GetFileName(file)));
            using (StreamReader sr = new StreamReader(file))
            {
                using (StreamWriter sw = new StreamWriter(newFile))
                {
                    sw.WriteLine("SSN,Arc,AN,FirstName,MI,LastName,Address1,Address2,Address3,City,State,Zip,Country,Foreign State,KeyLine,Arc Date,Payment,State_Ind,COST_CENTER_CODE");
                    while (!sr.EndOfStream)
                        sw.WriteLine(sr.ReadLine());
                }
            }

            return newFile;
<<<<<<< HEAD
        }
=======
        }*/
>>>>>>> 828da4f89f093b26bf5ef8094867481eeee9ff93

        /// <summary>
        /// Adds a comment to TD22
        /// </summary>
        /// <param name="file">File to process.</param>
        /// <param name="item">Object that has the arc and comment to use.</param>
        private void AddArc(string file, DatabaseData item)
        {
            using (StreamReader sr = new StreamReader(file))
            {
                //Read out the header
                sr.ReadLine();

                int recoverycount = 1;
                if (!Recovery.RecoveryValue.IsNullOrEmpty() && Recovery.RecoveryValue.Contains("ARC"))
                {
                    List<string> recoveryValue = Recovery.RecoveryValue.SplitAndRemoveQuotes(",");
                    while (recoverycount != recoveryValue[3].ToInt())
                    {
                        sr.ReadLine();
                        recoverycount++;
                    }
                }

                for (; !sr.EndOfStream; recoverycount++)
                {
                    //A standard for all sas files that will leave a comment will need the Account Number to be the first field
                    string accountNumber = sr.ReadLine().SplitAndRemoveQuotes(",")[0];
                    if (RI.Atd22ByBalance(accountNumber, item.Arc, item.Comment, string.Empty, ScriptId, false, false))
                        ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, string.Format("Unable to add ARC: {0}, Comment: {1} to the following account: {2}", item.Arc, item.Comment, accountNumber), NotificationType.ErrorReport, NotificationSeverityType.Warning);

                    Recovery.RecoveryValue = string.Format("ARC,{0},{1},{2}", file, item.BatchLettersId, recoverycount);
                }
            }
        }

        /// <summary>
        /// Checks to see if a file is empty.  There are some files that generate a header record when they are empty
        /// </summary>
        /// <param name="file">File to Check</param>
        /// <returns>True if the file has more than 2 lines.</returns
        private bool IsSasFileEmpty(string file)
        {
<<<<<<< HEAD
            if (file.Contains("ULWS11.LWS11"))
=======
            //Removed in 4438 as S11 now contains a header and can be handled like the rest of the files.
			/*if (file.Contains("ULWS11.LWS11"))
>>>>>>> 828da4f89f093b26bf5ef8094867481eeee9ff93
            {
                FileInfo fi = new FileInfo(file);
                if (fi.Length == 0)
                    return true;

                return false;
<<<<<<< HEAD
            }
=======
            }*/
>>>>>>> 828da4f89f093b26bf5ef8094867481eeee9ff93
            using (StreamReader sr = new StreamReader(file))
            {
                sr.ReadLine();
                return sr.EndOfStream;  //if we reached end of stream, file is empty.  otherwise, it has more than one line and isn't empty.
            }
        }
    }
}
