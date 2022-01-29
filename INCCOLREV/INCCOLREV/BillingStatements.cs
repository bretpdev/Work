using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.Scripts;

namespace INCCOLREV
{
    public class BillingStatements : BatchScript
    {
        //Recovery record format: FileName,RowNumber
        private const string SAS_JOB = "ULWM33.LWM33";
        private string DataFile;
        LoginForm LoginData;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ri">Instance of Reflection Interface</param>
        public BillingStatements(ReflectionInterface ri)
            : base(ri, "INCCOLREV", "LOGS", null, null, Uheaa.Common.DataAccess.DataAccessHelper.Region.Uheaa)
        {
        }

        /// <summary>
        /// Script entry point
        /// </summary>
        public override void Main()
        {
#if Debug
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
#endif
            List<SprocValidationResult> sprocs = DatabaseAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly());
            if (sprocs.Count() > 0)
            {
                string message = "Before you can run the application, you need to be given acces to the following stored procedures in the database.\r\n\r\n";
                foreach (SprocValidationResult result in sprocs)
                {
                    message += result.Attribute.SprocName + "\r\n";
                }
                MessageBox.Show(message, "Database Access Needed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                EndDllScript();
            }

            StartupMessage("This is the 'Increase Collection Revenue - Billing Statement' script.");

            DataFile = string.Format("{0}{1}dat.txt", EnterpriseFileSystem.TempFolder, ScriptId);

            List<string> files = LoadFiles();

            //Store the Username and Password if there are multiple files
            LoginData = new LoginForm(UserId);
            if (LoginData.ShowDialog() == DialogResult.Cancel)
                EndDllScript();

            int recFile = 0;

            if (!Recovery.RecoveryValue.IsNullOrEmpty())
                recFile = int.Parse(Recovery.RecoveryValue.SplitAndRemoveQuotes(",")[0]);

            foreach (string file in files.Skip(recFile))
            {
                ProcessFile(file, recFile);
                recFile++;
            }

            //Delete the data file
            File.Delete(DataFile);

            //Delete the files after they have been processed
            foreach (string file in files)
            {
                File.Delete(file);
            }

            if (Err.HasErrors)
                ProcessingComplete(@"Processing Complete with errors. See error report in X:\PADD\LOGS");
            else
                ProcessingComplete();
        }

        /// <summary>
        /// Gathers the files needed to process
        /// </summary>
        /// <returns>List of strings containing the fileName for each file to process</returns>
        private List<string> LoadFiles()
        {
            List<string> files = new List<string>();
            //Get R2 files
            string[] r2 = Directory.GetFiles(EnterpriseFileSystem.FtpFolder, SAS_JOB + "R2*");
            int r2Count = 0;
            if (r2.Count() > 0)
            {
                foreach (string s in r2)
                {
                    if (new FileInfo(s).Length > 0)
                    {
                        files.Add(s);
                        r2Count++;
                    }
                }
            }
            if (r2Count == 0)
            {
                MessageBox.Show("The R2 file is missing", "Missing File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                EndDllScript();
            }

            //Get R3 files
            string[] r3 = Directory.GetFiles(EnterpriseFileSystem.FtpFolder, SAS_JOB + "R3*");
            int r3Count = 0;
            if (r3.Count() > 0)
            {
                foreach (string s in r3)
                {
                    if (new FileInfo(s).Length > 0)
                    {
                        files.Add(s);
                        r3Count++;
                    }
                }
            }
            if (r3Count == 0)
            {
                MessageBox.Show("The R3 file is missing", "Missing File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                EndDllScript();
            }

            return files;
        }

        private void ProcessFile(string file, int fileToProcess)
        {
            //Add the letter text from database
            List<BorrowerData> borData = AddTextToFile(file);

            DocumentProcessing.CostCenterPrinting("BLNGSTMNT", DataFile, "STATE_INDICATOR", ScriptId, "DF_SPE_ACC_ID",
                DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.CostCenterOptions.AddBarcode, "COST_CENTER_CODE");

            if (CheckForText(16, 2, "LOGON"))
                Login(LoginData.UserName, LoginData.Password);

            AddComments(borData, fileToProcess);
        }

        /// <summary>
        /// Reads in the sas file and merges due diligent data from database
        /// </summary>
        /// <param name="file">The SAS file to pull data from</param>
        /// <returns>The new data file to process</returns>
        private List<BorrowerData> AddTextToFile(string file)
        {
            string header = "";

            List<BorrowerData> borData = new List<BorrowerData>();
            using (StreamReader sr = new StreamReader(file))
            {
                header = sr.ReadLine();
                header += "," + DataAccess.DUE_DILIGENCE_1 + "," + DataAccess.DUE_DILIGENCE_2 + "," + DataAccess.DUE_DILIGENCE_3;
                while (!sr.EndOfStream)
                {
                    List<string> line = sr.ReadLine().SplitAndRemoveQuotes(",");
                    if (line.Count() > 0)
                    {
                        BorrowerData data = LoadBorrower(line);
                        borData.Add(data);
                    }
                }
            }

            //Add the data to a new data file to merge with the doc
            AddDataToFile(header, borData);

            return borData;
        }

        /// <summary>
        /// Adds one row from the sas to a BorrowerData object
        /// </summary>
        /// <param name="line">a string array of items from the sas file</param>
        /// <returns>BorrowerData object of values from the sas</returns>
        private BorrowerData LoadBorrower(List<string> line)
        {
            BorrowerData data = new BorrowerData();
            data.SSN = line[0];
            data.Today = line[1];
            data.AccountNumber = line[2];
            data.FirstName = line[3];
            data.LastName = line[4];
            data.Address1 = line[5];
            data.Address2 = line[6];
            data.City = line[7];
            data.State = line[8];
            data.Zip = line[9];
            data.Country = line[10];
            data.ACSKEY = line[11];
            data.DateLastPay = line[12];
            data.AmountLastPay = line[13];
            data.Balance = line[14];
            data.AmountDue = line[15];
            data.NextPayDue = line[16];
            data.StateIndicator = line[17];
            data.CostCenterCode = line[18];
            if (!data.DateLastPay.IsNullOrEmpty() && (DateTime.Parse(data.DateLastPay) < DateTime.Now.Date.AddDays(-30)))
            {
                data.DueDiligenceText1 = DataAccess.DueDiligenceDefaultedText1.Replace("<<BALANCE>>", double.Parse(data.Balance).ToString("$###.##0.00"));
                data.DueDiligenceText2 = DataAccess.DueDiligenceDefaultedText2;
                data.DueDiligenceText3 = DataAccess.DueDiligenceDefaultedText3;
            }
            else
            {
                data.DueDiligenceText1 = DataAccess.DueDiligenceRepaymentText1;
                data.DueDiligenceText2 = DataAccess.DueDiligenceRepaymentText2;
                data.DueDiligenceText3 = DataAccess.DueDiligenceRepaymentText3;
            }
            return data;
        }

        /// <summary>
        /// Writes all of the BorrowerData to a new file to be merged with the letter
        /// </summary>
        /// <param name="header">The Header from the original file</param>
        /// <param name="borData">List of BorrowerData objects</param>
        /// <returns>The FileName of the new data file</returns>
        private void AddDataToFile(string header, List<BorrowerData> borData)
        {
            using (StreamWriter sw = new StreamWriter(DataFile))
            {
                sw.WriteLine(header);
                foreach (BorrowerData data in borData)
                {
                    string line = string.Format("{0},\"{1}\",{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},\"{19}\",\"{20}\",\"{21}\"",
                        data.SSN, data.Today, data.AccountNumber, data.FirstName, data.LastName, data.Address1,
                        data.Address2, data.City, data.State, data.Zip, data.Country, data.ACSKEY, data.DateLastPay,
                        data.AmountLastPay, data.Balance, data.AmountDue, data.NextPayDue, data.StateIndicator, data.CostCenterCode,
                        data.DueDiligenceText1, data.DueDiligenceText2, data.DueDiligenceText3);
                    sw.WriteLine(line);
                }
            }
        }

        /// <summary>
        /// Adds comments to LP50. Adds 
        /// </summary>
        /// <param name="borData">List of BorrowerData objects</param>
        /// <param name="fileToProcess">The file number being processed</param>
        private void AddComments(List<BorrowerData> borData, int fileToProcess)
        {
            int recStep = 0;
            if (!Recovery.RecoveryValue.IsNullOrEmpty() && !Recovery.RecoveryValue.SplitAndRemoveQuotes(",")[1].IsNullOrEmpty())
                recStep = int.Parse(Recovery.RecoveryValue.SplitAndRemoveQuotes(",")[1]);

            string comment = "THE MONTHLY BILLING STATEMENT AND DUE DILIGENCE HAS BEEN SENT FOR DEFAULTED ACCOUNT.";

            foreach (BorrowerData data in borData.Skip(recStep))
            {
                if (AddCommentInLP50(data.SSN, "DLBBS", "LT", "03", comment, ScriptId))
                    Recovery.RecoveryValue = string.Format("{0},{1}", fileToProcess, recStep);
                else
                    Err.AddRecord("Error adding comment", data);

                //Increment the record processed
                recStep++;
            }
        }
    }//class
}//namespace
