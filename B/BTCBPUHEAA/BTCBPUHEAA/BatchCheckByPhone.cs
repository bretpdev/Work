using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using Uheaa.Common.DataAccess;
using System.Reflection;
using System.Threading.Tasks;

namespace BTCBPUHEAA
{
    public class BatchCheckByPhone
    {
        ProcessLogRun PLR { get; set; }
        DataAccess DA { get; set; }
        RecoveryLog Recovery { get; set; }
        NameHelper NH { get; set; } = new NameHelper();

        public BatchCheckByPhone(ProcessLogRun plr)
        {
            PLR = plr;
            DA = new DataAccess(PLR);
            Recovery = new RecoveryLog(string.Format("BTCBPUHEAARecovery_Uheaa{0}", DateTime.Now.ToShortDateString().Replace('/', '-')));
        }


        public bool ProcessCheckByPhone()
        {
            if (!string.IsNullOrEmpty(Recovery.RecoveryValue))  //Check for recovery.
                RecoveryHandling();

            //Get the outstanding payment records and write them to the files.
            IEnumerable<Payment> payments = DA.GetPendingPayments();
            if (!payments.Any())
                return false;

            Sanitize(payments);
            IEnumerable<int> recordNumbersAddedToFiles = CreateFiles(payments, PLR);

            //Copy the files to the archive and upload folders, and remove them from the temp folder.
            MoveFiles(NH.AutoPostFileName, NH.TelPayFileName, NH.TelPayFileNameArchive, NH.AutoPostFileNameArchive);


            Recovery.RecoveryValue = NH.FileDateTimeStamp;
            if (recordNumbersAddedToFiles.Count() > 0)
                DA.UpdateProcessedDate(recordNumbersAddedToFiles);

            Recovery.Delete();
            return true;
        }

        /// <summary>
        /// Verify that all necessary network drives are available.  Process log and end the script of they are not accessable.
        /// </summary>
        public bool ValidateDrives()
        {
            IEnumerable<string> availableDrives = DriveInfo.GetDrives().Select(p => p.Name);
            string message = string.Empty;
            if (!availableDrives.Contains(@"O:\") || !availableDrives.Contains(@"X:\"))
                message += "The M and/or X drive is not available. ";
            if (!Directory.Exists(NH.UheaaArchiveFolder))
                message += $"The \"{NH.UheaaArchiveFolder}\" folder doesn't exist.";
            if (!Directory.Exists(NH.UheaaUploadFolder))
                message = $"The \"{NH.UheaaUploadFolder}\" folder doesn't exist.";
            if (message != string.Empty)
            {
                PLR.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                return false;
            }
            return true;
        }

        private string BuildAutoPostHeader(int batchNumber)
        {
            string region = "UT";

            StringBuilder headerBuilder = new StringBuilder();
            headerBuilder.Append("AHEADER");
            headerBuilder.Append(" ");
            headerBuilder.AppendFormat("{0:0000}", batchNumber);
            headerBuilder.Append(NH.FileDateTimeStamp);
            headerBuilder.Append("2");
            headerBuilder.Append("10");
            headerBuilder.Append("10");

            headerBuilder.Append("".PadRight(8, ' '));
            headerBuilder.Append(DateTime.Now.ToString("MMddyyyy"));

            headerBuilder.Append("TELPAY");

            headerBuilder.Append("Y");

            headerBuilder.Append("".PadRight(142, ' '));
            headerBuilder.Append(region);
            return headerBuilder.ToString();
        }

        private string BuildAutoPostTrailer(int batchNumber, int recordCount, decimal paymentTotal)
        {
            string region = "UT";
            StringBuilder headerBuilder = new StringBuilder();
            headerBuilder.Append("TRAILER");
            headerBuilder.Append(" ");
            headerBuilder.AppendFormat("{0:0000}", batchNumber);
            headerBuilder.Append(NH.FileDateTimeStamp);
            headerBuilder.AppendFormat("{0:0000000}", recordCount);
            headerBuilder.Append(paymentTotal.ToString().Replace(".", "").PadLeft(11, '0'));
            headerBuilder.Append("".PadRight(152, ' '));
            headerBuilder.Append(region);
            return headerBuilder.ToString();
        }

        private string BuildTelPayTrailer(int recordCount, decimal paymentTotal, decimal routingHash)
        {
            string newRoutingHash = "";
            if (routingHash.ToString().Length > 10)
                newRoutingHash = routingHash.ToString().Substring(routingHash.ToString().Length - 10, 10); //take the right 10 characters of the hash
            else if (routingHash.ToString().Length < 10)
                newRoutingHash = routingHash.ToString().PadLeft(10, '0');
            else
                newRoutingHash = routingHash.ToString();
            StringBuilder headerBuilder = new StringBuilder();
            headerBuilder.Append("TRL");
            headerBuilder.AppendFormat("{0:000000}", recordCount);
            headerBuilder.Append(paymentTotal.ToString().Replace(".", "").PadLeft(12, '0'));
            headerBuilder.Append(newRoutingHash);
            headerBuilder.Append("".PadRight(50, ' '));
            return headerBuilder.ToString();
        }
        private List<int> CreateFiles(IEnumerable<Payment> pendingPayments, ProcessLogRun PLR)
        {
            int batchNumber = GetNextBatchNumber();
            File.WriteAllText(EnterpriseFileSystem.TempFolder + NH.AutoPostFileName, BuildAutoPostHeader(batchNumber) + Environment.NewLine);
            List<int> recordNumbersAddedToFiles = AddRecordToFile(pendingPayments, batchNumber);
            return recordNumbersAddedToFiles;
        }

        private List<int> AddRecordToFile(IEnumerable<Payment> pendingPayments, int batchNumber)
        {
            bool has1GoodRecord = false;
            string autoPostFile = EnterpriseFileSystem.TempFolder + NH.AutoPostFileName;
            string autoPostFileArchive = EnterpriseFileSystem.TempFolder + NH.AutoPostFileNameArchive;
            string telPayFile = EnterpriseFileSystem.TempFolder + NH.TelPayFileName;
            string telPayFileArchive = EnterpriseFileSystem.TempFolder + NH.TelPayFileNameArchive;
            string payGovPayFile = EnterpriseFileSystem.TempFolder + NH.PayGovFileName;
            string payGovFileArchive = EnterpriseFileSystem.TempFolder + NH.PayGovFileNameArchive;
            List<int> recordNumbersAddedToFiles = new List<int>();
            int recordNumber = 1;
            decimal paymentTotal = 0; //Use a long because the file allows 11 digits, while an int barely gets you 10 digits.
            decimal routingHash = 0;
            foreach (Payment payment in pendingPayments.OrderBy(x => x.AccountNumber))
            {
                try
                {
                    Console.WriteLine("Region: {1}; Processing Account: {0}", payment.AccountNumber, DataAccessHelper.CurrentRegion);
                    string telPayLine = payment.GetTelpayRecord();
                    string autoPostLine = payment.GetAutoPostRecord(batchNumber, recordNumber);

                    string telPayLineArchive = payment.GetTelpayRecordArchive();
                    File.AppendAllText(autoPostFile, autoPostLine + Environment.NewLine);
                    File.AppendAllText(telPayFile, telPayLine + Environment.NewLine);
                    File.AppendAllText(telPayFileArchive, telPayLineArchive + Environment.NewLine);
                    File.AppendAllText(autoPostFileArchive, autoPostLine + Environment.NewLine);
                    recordNumbersAddedToFiles.Add(payment.RecNo);
                    paymentTotal += payment.PaymentAmount;
                    routingHash += payment.RoutingNumber.Substring(0, 8).ToDecimal();
                    recordNumber++;
                    has1GoodRecord = true;
                    //Add file name to database
                    DA.SetFileName(payment, NH.TelPayFileName, NH.UheaaArchiveFolder);
                }
                catch (FormatException ex)
                {
                    string violatedFile = (ex.StackTrace.Contains("GetTelPayRecord") ? "TelPay" : "Uheaa Auto Post");

                    //compile and add comment
                    string comment = string.Format("CBP/IVR payment was not processed due to the extracted {0} being blank or not matching the file layout for {1}.  {2}, {3}, {4}, {5}, {6}, {7}, {8}",
                    ex.Message, violatedFile, payment.RecNo, payment.EffectiveDate, payment.AccountType, payment.RoutingNumber.Substring(payment.RoutingNumber.Length - 3), payment.BankAccountNumber.Substring(payment.BankAccountNumber.Length - 3), payment.PaymentAmount, payment.AccountHolderName);

                    ArcData arc = new ArcData(DataAccessHelper.CurrentRegion)
                    {
                        AccountNumber = payment.AccountNumber,
                        Comment = comment,
                        ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                        Arc = "OPSER",
                        RecipientId = "",
                        ScriptId = "BTCBPUHEAA"
                    };

                    var result = arc.AddArc();
                    if (!result.ArcAdded)
                    {
                        string message = string.Format("Unable to add ARC: OPSER; Comment: {0}; for Account: {1}", comment, payment.AccountNumber);
                        PLR.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    }

                    payment.RoutingNumber = "XXXXXXXXX";
                    payment.BankAccountNumber = "XXXXXXXXXXXXXXXXX";
                    comment = string.Format("CBP/IVR payment was not processed due to the extracted {0} being blank or not matching the file layout for {1}.  {2}, {3}, {4}, {5}, {6}, {7}, {8}. Exception message: {0}",
                        ex.Message, violatedFile, payment.RecNo, payment.EffectiveDate, payment.AccountType, payment.RoutingNumber, payment.BankAccountNumber, payment.PaymentAmount, payment.AccountHolderName);
                    PLR.AddNotification(comment, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    DA.UpdateProcessedDate(new int[] { payment.RecNo });
                }
            }

            if (has1GoodRecord)
            {
                File.AppendAllText(autoPostFile, BuildAutoPostTrailer(batchNumber, recordNumber - 1, paymentTotal) + Environment.NewLine);
                File.AppendAllText(telPayFile, BuildTelPayTrailer(recordNumber - 1, paymentTotal, routingHash) + Environment.NewLine);
            }
            else
            {
                File.Delete(autoPostFile);
            }
            return recordNumbersAddedToFiles;
        }

        public int GetNextBatchNumber()
        {
            string[] archivedFiles = Directory.GetFiles(NH.UheaaArchiveFolder, string.Format("{0}{1:yyyyMMdd}*", NH.AUTO_POST_FILE_PREFIX_ARCHIVE, DateTime.Now));

            if (archivedFiles.Length == 0)
                return 1;//No files were sent earlier today, so this is the first batch.
            else
            {
                //Find the most recent file and get the batch number out of it.
                string mostRecentFile = archivedFiles[0];
                for (int i = 1; i < archivedFiles.Length; i++)
                {
                    if (new FileInfo(archivedFiles[i]).LastWriteTime > new FileInfo(mostRecentFile).LastWriteTime)
                        mostRecentFile = archivedFiles[i];
                }
                string headerLine = File.ReadAllLines(mostRecentFile)[0];
                string lastBatchString = headerLine.Substring(8, 4);
                int lastBatchNumber = int.Parse(lastBatchString);
                return lastBatchNumber + 1;
            }
        }

        private void MoveFiles(params string[] baseFileNames)
        {
            try
            {
                foreach (string baseName in baseFileNames)
                {
                    string source = EnterpriseFileSystem.TempFolder + baseName;
                    string destination = baseName.Contains("ARCHIVE") ? NH.UheaaArchiveFolder : NH.UheaaUploadFolder;
                    File.Copy(source, destination + baseName);
                    File.Delete(source);
                }
            }
            catch (Exception)
            {
                string message = "Could not save the Check-by-Phone files to the archive and/or upload folder.";
                PLR.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }

        }

        private void RecoveryHandling()
        {
            //Delete the files from archive/upload and temp that have the recovery value's date/time stamp.
            string autoPostBaseName = string.Format("{0}{1}.txt", NH.AUTO_POST_FILE_PREFIX, Recovery.RecoveryValue);
            string telPayBaseName = string.Format("{0}{1}.txt", NH.TEL_FILE_PREFIX, Recovery.RecoveryValue);
            string payGovBaseName = string.Format("{0}{1}.txt", NH.PAY_GOV_FILE_PREFIX, Recovery.RecoveryValue);
            File.Delete(EnterpriseFileSystem.TempFolder + autoPostBaseName);
            File.Delete(EnterpriseFileSystem.TempFolder + telPayBaseName);
            File.Delete(EnterpriseFileSystem.TempFolder + payGovBaseName);
            File.Delete(NH.UheaaArchiveFolder + autoPostBaseName);
            File.Delete(NH.UheaaArchiveFolder + telPayBaseName);
            File.Delete(NH.UheaaUploadFolder + autoPostBaseName);
            File.Delete(NH.UheaaUploadFolder + telPayBaseName);
            Recovery.Delete();
        }

        private void Sanitize(IEnumerable<Payment> payments)
        {
            var info = typeof(Payment).GetProperties();
            foreach (Payment rawData in payments)
            {
                foreach (PropertyInfo property in info)
                {
                    if (property.GetValue(rawData, null) != null && property.PropertyType == typeof(string))
                    {
                        property.SetValue(rawData, property.GetValue(rawData, null).ToString().Trim(), null);
                    }
                }
            }
        }
    }
}