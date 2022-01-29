using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Q;
using Key = Q.ReflectionInterface.Key;

namespace BTCHCBP
{
    public class BatchCheckByPhone : FedBatchScriptBase
    {
        //Recovery value is the date/time stamp used in the file names.

        private const string AUTO_POST_FILE_PREFIX = "AUTOPOST";
        private const string AUTO_POST_FILE_PREFIX_ARCHIVE = "AUTOPOSTARCHIVE";

        private const string PAY_GOV_FILE_PREFIX = "PAYGOV";
        private const string PAY_GOV_FILE_PREFIX_ARCHIVE = "PAYGOVARCHIVE";

        private const string EOJ_FROM_DATABASE = "Records read from database";
        private const string EOJ_TO_FILE = "Records added to batch files";
        private const string EOJ_TO_QUE = "Records with errors added to Queue";
        private const string EOJ_ERRORS = "Records with errors";
        private static readonly string[] EOJ_FIELDS = { EOJ_FROM_DATABASE, EOJ_TO_FILE, EOJ_TO_QUE, EOJ_ERRORS };

        private readonly string _archiveFolder;
        private readonly string _autoPostFileName;
        private readonly string _autoPostFileNameArchive;
        private readonly DataAccess _dataAccess;
        private readonly ErrorReport<Payment> _err;
        private readonly string _fileDateTimeStamp;
        private readonly string _payGovFileName;
        private readonly string _payGovFileNameArchive;
        private readonly string _uploadFolder;

        public BatchCheckByPhone(ReflectionInterface ri)
            : base(ri, "BTCHCBP", "ERR_BU35", "EOJ_BU03", EOJ_FIELDS)
        {
            _archiveFolder = Efs.GetPath("Check by Phone Archive");
            _fileDateTimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssff");
            _autoPostFileName = string.Format("{0}{1}.txt", AUTO_POST_FILE_PREFIX, _fileDateTimeStamp);
            _autoPostFileNameArchive = string.Format("{0}{1}.txt", AUTO_POST_FILE_PREFIX_ARCHIVE, _fileDateTimeStamp);
            _dataAccess = new DataAccess(ri.TestMode);
            _err = new ErrorReport<Payment>(ri.TestMode, "Batch Check By Phone - FED", "ERR_BU35", Region.CornerStone);
            _payGovFileName = string.Format("{0}{1}.txt", PAY_GOV_FILE_PREFIX, _fileDateTimeStamp);
            _payGovFileNameArchive = string.Format("{0}{1}.txt", PAY_GOV_FILE_PREFIX_ARCHIVE, _fileDateTimeStamp);
            _uploadFolder = Efs.GetPath("Check by Phone Upload");
        }

        public override void Main()
        {
            StartupMessage("This script sends Check-by-Phone records to AES for batch processing. Click OK to continue, or Cancel to quit.");

            //Check that the M and Z drives are available.
            IEnumerable<string> availableDrives = DriveInfo.GetDrives().Select(p => p.Name);
            if (!availableDrives.Contains(@"M:\") || !availableDrives.Contains(@"Z:\")) { NotifyAndEnd("The M and/or Z drive is not available."); }

            //While we're at it, check that the archive and upload folders are available.
            if (!Directory.Exists(_archiveFolder)) { NotifyAndEnd("The \"{0}\" folder doesn't exist.", _archiveFolder); }
            if (!Directory.Exists(_uploadFolder)) { NotifyAndEnd("The \"{0}\" folder doesn't exist.", _uploadFolder); }

            //Check for recovery. It will only exist if we've created the files but not updated the processed date in the database,
            //in which case the simplest thing to do is just delete the files and start over.
            if (!string.IsNullOrEmpty(Recovery.RecoveryValue)) { Nigerian(); }

            //Get the outstanding payment records and write them to the files.
            IEnumerable<Payment> payments = _dataAccess.GetPendingPayments();
            IEnumerable<int> recordNumbersAddedToFiles = CreateFiles(payments);

            //Copy the files to the archive and upload folders, and remove them from the temp folder.
            if (!RI.TestMode)
            {
                MoveFiles(_autoPostFileName, _payGovFileName);
                MoveArchiveFiles(_payGovFileNameArchive, _autoPostFileNameArchive);
            }

            //Now is the time to update recovery.
            Recovery.RecoveryValue = _fileDateTimeStamp;

            //Mark the database records as processed.
            _dataAccess.UpdateProcessedDate(recordNumbersAddedToFiles);

            _err.Publish();
            ProcessingComplete();
        }//Main()

        private string BuildAutoPostHeader(int batchNumber)
        {
            StringBuilder headerBuilder = new StringBuilder();
            headerBuilder.Append("AHEADER");
            headerBuilder.Append(" ");
            headerBuilder.AppendFormat("{0:0000}", batchNumber);
            headerBuilder.Append(_fileDateTimeStamp);
            headerBuilder.Append("2");
            headerBuilder.Append("10");
            headerBuilder.Append("10");
            headerBuilder.Append("".PadRight(16, ' '));
            headerBuilder.Append("TELPAY");
            headerBuilder.Append("N");
            headerBuilder.Append("".PadRight(142, ' '));
            headerBuilder.Append("KU");
            return headerBuilder.ToString();
        }//BuildAutoPostHeader()

        private string BuildAutoPostTrailer(int batchNumber, int recordCount, long paymentTotal)
        {
            StringBuilder headerBuilder = new StringBuilder();
            headerBuilder.Append("TRAILER");
            headerBuilder.Append(" ");
            headerBuilder.AppendFormat("{0:0000}", batchNumber);
            headerBuilder.Append(_fileDateTimeStamp);
            headerBuilder.AppendFormat("{0:0000000}", recordCount);
            headerBuilder.AppendFormat("{0:00000000000}", paymentTotal);
            headerBuilder.Append("".PadRight(152, ' '));
            headerBuilder.Append("KU");
            return headerBuilder.ToString();
        }//BuildAutoPostTrailer()

        private List<int> CreateFiles(IEnumerable<Payment> pendingPayments)
        {
            string autoPostFile = Efs.TempFolder + _autoPostFileName;
            string autoPostFileArchive = Efs.TempFolder + _autoPostFileNameArchive;
            string payGovFile = Efs.TempFolder + _payGovFileName;
            string payGovFileArchive = Efs.TempFolder + _payGovFileNameArchive;
            int batchNumber = GetNextBatchNumber();
            File.WriteAllText(autoPostFile, BuildAutoPostHeader(batchNumber) + Environment.NewLine);
            List<int> recordNumbersAddedToFiles = new List<int>();
            int recordNumber = 1;
            long paymentTotal = 0; //Use a long because the file allows 11 digits, while an int barely gets you 10 digits.
            bool has1GoodRecord = false;
            foreach (Payment payment in pendingPayments)
            {
                Eoj.Counts[EOJ_FROM_DATABASE].Increment();
                try
                {
                    string payGovLine = payment.GetPayGovRecord();
                    string autoPostLine = payment.GetAutoPostRecord(batchNumber, recordNumber);

                    string payGovLineArchive = payment.GetPayGovRecordArchive();
                    File.AppendAllText(autoPostFile, autoPostLine + Environment.NewLine);
                    File.AppendAllText(payGovFile, payGovLine + Environment.NewLine);
                    File.AppendAllText(payGovFileArchive, payGovLineArchive + Environment.NewLine);
                    File.AppendAllText(autoPostFileArchive, autoPostLine + Environment.NewLine);
                    recordNumbersAddedToFiles.Add(payment.RecNo);
                    paymentTotal += payment.PaymentAmount;
                    recordNumber++;
                    Eoj.Counts[EOJ_TO_FILE].Increment();
                    has1GoodRecord = true;
                }
                catch (FormatException ex)
                {
                    string violatedFile = (ex.StackTrace.Contains("GetPayGovRecord") ? "pay.gov" : "Compass Auto Post");

                    //compile and add comment
                    string comment = string.Format("CBP/IVR payment was not processed due to the extracted {0} being blank or not matching the file layout for {1}.  {2}, {3}, {4}, {5}, {6}, {7}, {8}",
                                                    ex.Message,
                                                    violatedFile,
                                                    payment.RecNo,
                                                    payment.EffectiveDate,
                                                    payment.AccountType,
                                                    payment.RoutingNumber.Substring(payment.RoutingNumber.Length - 3),
                                                    payment.BankAccountNumber.Substring(payment.BankAccountNumber.Length - 3),
                                                    payment.PaymentAmount,
                                                    payment.AccountHolderName);

                    if (ATD22AllLoans(payment.CornerStoneAccountNumber, "OPSER", comment, "BTCHCBP", false) != Common.CompassCommentScreenResults.CommentAddedSuccessfully)
                    {
                        string message = string.Format("The following record was not processed with this data batch due to the extracted data being blank or not matching the file layout for {0}.", violatedFile);
                        payment.RoutingNumber = "XXXXXXXXX";
                        payment.BankAccountNumber = "XXXXXXXXXXXXXXXXX";
                        _err.AddRecord(message, payment);
                        Eoj.Counts[EOJ_ERRORS].Increment();
                    }
                    else
                    {
                        Eoj.Counts[EOJ_TO_QUE].Increment();
                    }
                }

            }//foreach
            if (has1GoodRecord)
            {
                File.AppendAllText(autoPostFile, BuildAutoPostTrailer(batchNumber, recordNumber - 1, paymentTotal) + Environment.NewLine);
            }
            else
            {
                File.Delete(autoPostFile);
            }
            return recordNumbersAddedToFiles;
        }//CreateFiles()

        public int GetNextBatchNumber()
        {
            string[] archivedFiles = Directory.GetFiles(_archiveFolder, string.Format("{0}{1:yyyyMMdd}*", AUTO_POST_FILE_PREFIX, DateTime.Now));
            if (archivedFiles.Length == 0)
            {
                //No files were sent earlier today, so this is the first batch.
                return 1;
            }
            else
            {
                //Find the most recent file and get the batch number out of it.
                string mostRecentFile = archivedFiles[0];
                for (int i = 1; i < archivedFiles.Length; i++)
                {
                    if (new FileInfo(archivedFiles[i]).LastWriteTime > new FileInfo(mostRecentFile).LastWriteTime) { mostRecentFile = archivedFiles[i]; }
                }
                string headerLine = File.ReadAllLines(mostRecentFile)[0];
                string lastBatchString = headerLine.Substring(8, 4);
                int lastBatchNumber = int.Parse(lastBatchString);
                return lastBatchNumber + 1;
            }
        }//GetBatchNumber()

        private void MoveFiles(params string[] baseFileNames)
        {
            foreach (string baseName in baseFileNames)
            {
                Debug.Assert(File.Exists(Efs.TempFolder + baseName), string.Format("The {0} file was not found in {1}.", baseName, Efs.TempFolder));
            }
            if (!RI.TestMode)
            {
                try
                {
                    foreach (string baseName in baseFileNames)
                    {
                        string source = Efs.TempFolder + baseName;
                        File.Copy(source, _uploadFolder + baseName);
                        File.Delete(source);
                    }
                }
                catch (Exception)
                {
                    NotifyAndEnd("Could not save the Check-by-Phone files to the archive and/or upload folder.");
                }
            }//if
        }//MoveFiles()

        private void MoveArchiveFiles(params string[] baseFileNames)
        {
            foreach (string baseName in baseFileNames)
            {
                Debug.Assert(File.Exists(Efs.TempFolder + baseName), string.Format("The {0} file was not found in {1}.", baseName, Efs.TempFolder));
            }
            if (!RI.TestMode)
            {
                try
                {
                    foreach (string baseName in baseFileNames)
                    {
                        string source = Efs.TempFolder + baseName;
                        File.Copy(source, _archiveFolder + baseName);
                        File.Delete(source);
                    }
                }
                catch (Exception)
                {
                    NotifyAndEnd("Could not save the Check-by-Phone files to the archive and/or upload folder.");
                }
            }//if
        }

        //Don't anybody get any funny ideas about bigotry or whatnot regarding this method's name.
        //It just reflects its role as the opposite of a Finnish subroutine
        //(i.e., rather than being nordic, oceanic, and cold, it's southern, land-locked, and hot).
        private void Nigerian()
        {
            //Delete the files from archive/upload and temp that have the recovery value's date/time stamp.
            string autoPostBaseName = string.Format("{0}{1}.txt", AUTO_POST_FILE_PREFIX, Recovery.RecoveryValue);
            string payGovBaseName = string.Format("{0}{1}.txt", PAY_GOV_FILE_PREFIX, Recovery.RecoveryValue);
            File.Delete(Efs.TempFolder + autoPostBaseName);
            File.Delete(Efs.TempFolder + payGovBaseName);
            File.Delete(_archiveFolder + autoPostBaseName);
            File.Delete(_archiveFolder + payGovBaseName);
            File.Delete(_uploadFolder + autoPostBaseName);
            File.Delete(_uploadFolder + payGovBaseName);
            //Re-initialize the EOJ counts.
            Eoj.Counts[EOJ_FROM_DATABASE] = new Count(0);
            Eoj.Counts[EOJ_TO_FILE] = new Count(0);
            Eoj.Counts[EOJ_ERRORS] = new Count(0);
            //Note that the ErrorReport class doesn't provide a way to start over
            //(heck, even the above code for the EOJ counts is a hack to get around EOJ recovery).
            //We can expect the same records to end up in the error report again, which is manually worked,
            //so whoever's doing the DCRs will just need to recognize the duplicates. Live and let live, right?
            //Oll klear? Get out of recovery.
            Recovery.Delete();
        }//Nigerian()
    }//class
}//namespace
