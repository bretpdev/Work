using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace CLSCHLLNFD
{
    public class ClosedSchoolLoan
    {
        public ReflectionInterface RI { get; set; }
        public ProcessLogRun LogRun { get; set; }
        public DataAccess DA { get; set; }
        public const string ATCSK = "ATCSK"; 
        public const string DRDCR = "DRDCR";
        public const string ADCSH = "ADCSH";
        private const string HEADER_RECORD = "00";
        private const string DETAIL_RECORD = "01";
        private const string TRAILER_RECORD = "99";

        public ClosedSchoolLoan(ReflectionInterface ri, ProcessLogRun logRun)
        {
            RI = ri;
            LogRun = logRun;
        }

        public int Run(bool consoleWait)
        {
            List<string> files = Directory.GetFiles(EnterpriseFileSystem.FtpFolder, "502*").ToList();
            if (CheckMultipleFiles(files))
                return 1;
            string fileName = files.FirstOrDefault();

            DA = new DataAccess(LogRun);

            try
            {
                if (File.Exists(fileName))
                {
                    List<SchoolData> data = ReadInFile(fileName);
                    if (data != null)
                    {
                        WriteDataToDatabase(data);
                    }
#if !DEBUG
                    Repeater.TryRepeatedly(() => FS.Delete(fileName));
#endif
                    fileName = null;
                }
                else
                    LogRun.AddNotification($"No 502 file found to process today.", NotificationType.NoFile, NotificationSeverityType.Informational);
                ProcessAccounts();
                PrintLetters();
                AddFinalArc();
                Console.WriteLine("Processing Complete");

                if (consoleWait) //So BA has time to read terminal
                {
                    Console.WriteLine("Press Enter to end the code's execution.");
                    Console.ReadLine();
                }

                return 0; //Everything processed without errors
            }
            catch (Exception ex)
            {
                LogRun.AddNotification("There was an error that stopped the application before it finished processing", NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
            }

            if (DataAccessHelper.CurrentMode == DataAccessHelper.Mode.Dev)  //So BA has time to read terminal
            {
                Console.WriteLine("Press Enter to end the code's execution.");
                Console.ReadLine();
            }

            return 1;
        }

        /// <summary>
        /// Checks to make sure the file exists and that there is only 1 file to process
        /// </summary>
        private bool CheckMultipleFiles(List<string> files)
        {
            if (Directory.GetFiles(EnterpriseFileSystem.FtpFolder, "502*").Count() > 1)
            {
                string message = "More than one 502.txt file was found in the FTP folder. Ending script";
                Console.WriteLine(message);
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Reads in the 502.txt file
        /// </summary>
        private List<SchoolData> ReadInFile(string fileName)
        {
            List<SchoolData> closureDataFromFile = new List<SchoolData>();
            try
            {
                using (StreamR sr = new StreamR(fileName))
                {
                    string line;
                    int lineCount = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        lineCount++;
                        if (!line.SafeSubString(0, 2).IsIn(HEADER_RECORD, DETAIL_RECORD, TRAILER_RECORD)) // Log unexpected row, stop running script on file
                        {
                            LogRun.AddNotification($"The file {fileName} does not have the expected Record Type values on line {lineCount}. Expected values at the beginning of the line are \"{HEADER_RECORD}\", \"{DETAIL_RECORD}\", or \"{TRAILER_RECORD}\".", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                            return null;
                        }
                        if (line.SafeSubString(0, 2) == DETAIL_RECORD)
                        {
                            SchoolData sd = new SchoolData
                            {
                                CurrentGACode = line.SafeSubString(89, 3).Trim().ToDouble(),
                                StudentSsn = line.SafeSubString(92, 9),
                                PlusSsn = line.SafeSubString(179, 9).Trim(),
                                LoanType = line.SafeSubString(266, 2),
                                AwardId = line.SafeSubString(332, 21).Trim(),
                                SchoolCode = line.SafeSubString(2, 6).Trim(),
                                SchoolBranchCode = line.SafeSubString(8, 8).Trim(),
                                SchoolName = line.SafeSubString(16, 65).Trim()
                            };
                            closureDataFromFile.Add(sd);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                LogRun.AddNotification("Error reading in the 502.txt file.  File was empty or did not contain correctly formatted data.", NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                return null;
            }

            return closureDataFromFile;
        }

        /// <summary>
        /// Writes all the data from the excel file to a database table
        /// </summary>
        private void WriteDataToDatabase(List<SchoolData> data)
        {
            Console.WriteLine("Adding data found in 502.txt file to the clschllnfd.SchoolClosureData table");
            int recordsAdded = 0;

            foreach (SchoolData sData in data)
            {
                SchoolClosureData closure = LoadClosureData(sData);
                if (closure.DisbData != null)
                {
                    if (closure.DisbData.Count == 0)
                    {
                        LogRun.AddNotification($"No disbursement data found for AwardId:{sData.AwardId}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                        continue;
                    }
                    foreach (DisbursementData dData in closure.DisbData)
                    {
                        if (dData.DischargeAmount <= 0)
                        {
                            LogRun.AddNotification($"No discharge amount found for AwardId:{sData.AwardId}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                            continue;
                        }
                        if (DA.AddRecordToTable(closure, dData))
                            recordsAdded++;
                        else
                            LogRun.AddNotification($"The record for the AwardId:{sData.AwardId} in today's 502 file was not added to the database, most likely because it is a duplicate of a record already in the CLS.clschllnfd.SchoolClosureData table.", NotificationType.Other, NotificationSeverityType.Informational);
                    }
                }
                else
                    LogRun.AddNotification($"No disbursement data found for AwardId:{sData.AwardId}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }
            Console.WriteLine($"The number of records added to the CLS.clschllnfd.SchoolClosureData table: {recordsAdded}.");
        }

        /// <summary>
        /// Gets all the disbursement data for the loan sequence
        /// </summary>
        private SchoolClosureData LoadClosureData(SchoolData sData)
        {
            SchoolClosureData data = new SchoolClosureData();
            DetermineSsn(sData, data);
            data.LoanSeq = DA.GetLoanSequence(sData.AwardId, data.BorrowerSsn);
            data.DisbData = DA.GetDisbursements(data.BorrowerSsn, data.LoanSeq);
            data.SchoolCode = sData.SchoolBranchCode; //The full school code is passed in as the School Branch Code
            return data;
        }

        /// <summary>
        /// Determines the Borrower SSN according to the Plus SSN in the file.
        /// </summary>
        private static void DetermineSsn(SchoolData sData, SchoolClosureData data)
        {
            if (string.IsNullOrWhiteSpace(sData.PlusSsn))
            {
                data.BorrowerSsn = sData.StudentSsn.Trim();
                data.StudentSsn = "";
            }
            else
            {
                data.BorrowerSsn = sData.PlusSsn.Trim();
                data.StudentSsn = sData.StudentSsn.Trim();
            }
        }

        /// <summary>
        /// Load the next available account and call the Process method
        /// </summary>
        private void ProcessAccounts()
        {
            ProcessingData data = DA.GetProcessingData();
            string ssn = data?.BorrowerSsn;
            if (data == null)
                LogRun.AddNotification("No records found to process.", NotificationType.Other, NotificationSeverityType.Informational);

            while (data != null)
            {
                if (data.BorrowerSsn != ssn) //New borrower, so reset arcAddId and control ssn
                    ssn = data.BorrowerSsn;

                Console.WriteLine($"Processing discharge for Account:{data.AccountNumber}; LoanSeq:{data.LoanSeq}; DisbursementSeq:{data.DisbursementSeq}");
                if (DA.CheckLoanStatus(data).IsIn("01", "02", "03", "04", "05"))
                    Process(data);
                else
                    AddArc(data, ATCSK, ErrorMessage.LOAN_STATUS, false, true);
                data = null;
                data = DA.GetProcessingData();
            }
        }

        /// <summary>
        /// Determine if the Discharge Date is greater than the Payment Date, Add ARC if not.
        /// </summary>
        private void Process(ProcessingData data)
        {
            if (data.ProcessedAt == null)
            {
                DateTime? paymentDate = DA.GetPaymentDate(data.BorrowerSsn, data.LoanSeq);
                if (paymentDate.HasValue && paymentDate.Value.Date > data.DischargeDate.Date)
                {
                    Console.WriteLine($"The discharge could not be processed. Account: {data.AccountNumber}, Loan Seq: {data.LoanSeq} has payments.");
                    AddArc(data, ATCSK, ErrorMessage.UNREFUNDED_PAYMENTS, false, true);
                }
                else
                    AccessAts3q(data);
            }
        }

        /// <summary>
        /// Determine if the loan was selected or if on multiple loan selection screen.
        /// </summary>
        private void AccessAts3q(ProcessingData data)
        {
            RI.FastPath($"TX3ZATS3Q{data.BorrowerSsn};{data.DischargeDate.ToString("MMddyy")}");
            if (RI.ScreenCode == "TSX3O")
                WriteOff(data);
            else if (RI.ScreenCode == "TSX3S")
                MultipleLoans(data);
            else
                AddArc(data, ATCSK, ErrorMessage.ATS3Q_SESSION_MESSAGE, false, true, RI.Message);
        }

        /// <summary>
        /// Chooses which loan will be discharged. If loans is selected, call the WriteOff method.
        /// </summary>
        private void MultipleLoans(ProcessingData data)
        {
            while (RI.MessageCode != "90007")
            {
                for (int row = 7; row <= 21; row++)
                {
                    if (RI.GetText(row, 20, 4).ToIntNullable() == data.LoanSeq)
                    {
                        RI.PutText(22, 19, RI.GetText(row, 2, 3), ReflectionInterface.Key.Enter); //Was to 4, 2
                        if (RI.ScreenCode != "TSX3O")
                            AddArc(data, ATCSK, ErrorMessage.TX30_SESSION_MESSAGE, false, true, RI.Message);
                        else
                            WriteOff(data);
                        return;
                    }
                }
                RI.Hit(ReflectionInterface.Key.F8);
            }
            AddArc(data, ATCSK, ErrorMessage.LOAN_NOT_FOUND, false, true);
        }

        /// <summary>
        /// Adds borrower data to the Write-Off/Charge-Off screen
        /// </summary>
        private void WriteOff(ProcessingData data)
        {
            if (AlreadyWrittenOff(data))
                return;

            RI.PutText(8, 42, "X");
            if (data.StudentSsn.Trim().IsPopulated())
            {
                RI.PutText(9, 17, data.StudentSsn);
                RI.PutText(9, 35, "S");
            }
            RI.PutText(9, 45, "D");
            RI.PutText(12, 48, data.DischargeAmount.ToString());
            AddComment(data);
            RI.Hit(ReflectionInterface.Key.F11);
            if (RI.MessageCode == "02238")
            {
                DA.SetProcessedAt(data.SchoolClosureDataId);
                if (!GenerateComments(data))
                    Console.WriteLine($"Comments for write off attempt were not generated correctly for Account: {data.AccountNumber}, Loan Sequence: {data.LoanSeq}.");
            }
            else
                AddArc(data, ATCSK, ErrorMessage.WRITE_OFF, false, true, RI.Message);
        }

        private bool GenerateComments(ProcessingData data)
        {
            bool success = true;
            List<ErrorData> errorRecords = DA.GetErrorRecords(data.BorrowerSsn); // Gets error records that were added on the run date
            List<int> priorProcessedLoans = DA.GetPriorProcessedDisbursements(data) ?? new List<int>();

            if (IsReadyForEndingArc(data.BorrowerSsn, errorRecords, data.AddedAt) && errorRecords?.Count > 0)
                success &= LeaveEndingErrorArc(data, errorRecords, priorProcessedLoans.Count > 0);

            if (priorProcessedLoans.Count() > 0 && IsReadyForEndingArc(data.BorrowerSsn, errorRecords, data.AddedAt))
                success &= LeavePriorProcessedArc(data, priorProcessedLoans);

            return success;
        }

        private bool LeavePriorProcessedArc(ProcessingData data, List<int> loanSeqs)
        {
            string loans = FormatLoanSequences(loanSeqs);

            ArcAddResults result = null;
            ArcData arcData = new ArcData(DataAccessHelper.Region.CornerStone)
            {
                Arc = "ATCSK",
                ArcTypeSelected = ArcData.ArcType.Atd22ByLoan,
                AccountNumber = data.AccountNumber,
                Comment = $"{loans} A write off was already present on the loan for the given disbursement. No additional write off submitted.",
                LoanSequences = loanSeqs,
                IsEndorser = false,
                IsReference = false,
                RecipientId = data.BorrowerSsn,
                ScriptId = Program.ScriptId
            };
            result = arcData.AddArc();

            if (result.ArcAdded)
                return true;
            else
            {
                LogRun.AddNotification($"Error encountered when leaving the ATCSK ARC for already written off disbursements for Account: {data.AccountNumber}, Loan Sequences: {loans}.", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                return false;
            }

        }

        /// <summary>
        /// Formats the given list of loan sequences.
        /// </summary>
        private static string FormatLoanSequences(List<int> loanSeqs)
        {
            if (loanSeqs != null && loanSeqs.Count() > 0)
                return $"For Loan Seq: {string.Join(", ", loanSeqs.OrderBy(l => l).ToList())}:";
            return "";
        }

        /// <summary>
        /// Scrapes the Session ATS3Q screen to see if a write off has already been processed for the given loan sequence.
        /// </summary>
        private bool AlreadyWrittenOff(ProcessingData data)
        {
            if (RI.GetText(12, 32, 10) == ".00" || RI.GetText(12, 32, 10) == "0.00")
            {
                string message = $"Write off for Account: {data.AccountNumber}, Loan Sequence: {data.LoanSeq}, Disbursement Sequence: {data.DisbursementSeq} was unable to be processed as there was no principal balance on the loan per ATS3Q screen.";
                LogRun.AddNotification(message, NotificationType.HandledException, NotificationSeverityType.Informational);
                DA.SetProcessedAt(data.SchoolClosureDataId);
                DA.UpdateWasProcessedPrior(data.SchoolClosureDataId);
                if (!GenerateComments(data))
                    Console.WriteLine($"Comments for an already written off disbursement were not generated correctly for Account: {data.AccountNumber}, Loan Sequence: {data.LoanSeq}.");
                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds the comment for the write-off. If the comment is longer than 77 characters, it will write the remaining comment on the 2nd line.
        /// </summary>
        private void AddComment(ProcessingData data)
        {
            string comment = $"School Discharge; Discharge Date:{data.DischargeDate.ToShortDateString()}; Discharge Amount:${data.DischargeAmount}; Loan Seq:{data.LoanSeq}";
            RI.PutText(18, 2, comment.SafeSubString(0, 77));
            if (comment.Length > 77)
                RI.PutText(19, 2, comment.SafeSubString(77, comment.Length - 77));
        }

        /// <summary>
        /// In the event that the last worked loan seq on an account results in
        /// a successful processing of the write off, and in the event that other
        /// loan seqs encountered an error, this method will be called to drop that
        /// error ARC with all the errors collected into one message on one ARC.
        /// </summary>
        private bool LeaveEndingErrorArc(ProcessingData data, List<ErrorData> errorData, bool hasPriorProcessedDisbursements)
        {
            if ((errorData == null || errorData.Count == 0) && hasPriorProcessedDisbursements == false)
            {
                LogRun.AddNotification($"Ending error ARC not added for Account: {data.AccountNumber}, Loan Sequence: {data.LoanSeq}", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                return false;
            }

            string comment = FormatComment(errorData); // Compiles all errors into one message
            Console.WriteLine($"Adding ARC: ATCSK for Account:{data.AccountNumber}; Comment:{comment}");
            List<int> loanSeqs = errorData.Select(p => p.LoanSeq).Distinct().ToList();
            string loans = FormatLoanSequences(loanSeqs);

            ArcAddResults result = null;
            ArcData arcData = new ArcData(DataAccessHelper.Region.CornerStone)
            {
                Arc = "ATCSK",
                ArcTypeSelected = ArcData.ArcType.Atd22ByLoan,
                AccountNumber = data.AccountNumber,
                Comment = comment, //ArcAdd comment field is a VARCHAR(300)
                LoanSequences = loanSeqs,
                IsEndorser = false,
                IsReference = false,
                RecipientId = data.BorrowerSsn,
                ScriptId = Program.ScriptId
            };
            result = arcData.AddArc();

            if (result.ArcAdded)
                return UpdateAllErrorArcIds(result, data, loanSeqs);
            else
            {
                LogRun.AddNotification($"Error encountered when passing ATCSK ARC info to ArcAddProcessing for Account: {data.AccountNumber}, Loan Sequences: {loans}.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return false;
            }
        }

        private bool UpdateAllErrorArcIds(ArcAddResults result, ProcessingData data, List<int> loanSeqs)
        {
            if (result.ArcAdded)
            {
                foreach (int seq in loanSeqs)
                {
                    DA.UpdateErrorArcId(data, seq, result.ArcAddProcessingId);
                }
                DA.UpdateErrorLogArcId(data, result.ArcAddProcessingId);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Leaves an ARC just warning the BU that a write off was already done. 
        /// No need to track this ARC in the processing table, as we will treat this
        /// as a non-error.
        /// </summary>
        private bool LeaveAlreadyWrittenOffWarning(ProcessingData data)
        {
            ArcAddResults result = null;
            ArcData arcData = new ArcData(DataAccessHelper.Region.CornerStone)
            {
                Arc = "ATCSK",
                ArcTypeSelected = ArcData.ArcType.Atd22ByLoan,
                AccountNumber = data.AccountNumber,
                Comment = $"Loan Seq: {data.LoanSeq} was already written off. No new write off done on ATS3Q.", //ArcAdd comment field is a VARCHAR(300)
                LoanSequences = new List<int> { data.LoanSeq },
                IsEndorser = false,
                IsReference = false,
                RecipientId = data.BorrowerSsn,
                ScriptId = Program.ScriptId
            };
            result = arcData.AddArc();

            return result.ArcAdded;
        }

        /// <summary>
        /// Adds the ARC
        /// </summary>
        private void AddArc(ProcessingData data, string arc, string comment, bool isFinalARC = false, bool isError = false, string sessionMessage = "")
        {
            if (isError)
            {
                DA.AddErrorRecord(data, arc, comment, sessionMessage);
                if (!GenerateComments(data))
                    Console.WriteLine($"Comments were not generated correctly for Account: {data.AccountNumber}, Loan Sequence: {data.LoanSeq}.");
                return;
            }

            Console.WriteLine($"Adding ARC:{arc} for Account:{data.AccountNumber}; Comment:{comment}");
            ArcAddResults result = null;
            ArcData arcData = new ArcData(DataAccessHelper.Region.CornerStone)
            {
                Arc = arc,
                ArcTypeSelected = ArcData.ArcType.Atd22ByLoan,
                AccountNumber = data.AccountNumber,
                Comment = comment,
                LoanSequences = data.LoanSeqs.Count > 0 ? data.LoanSeqs : new List<int>() { data.LoanSeq },
                IsEndorser = false,
                IsReference = false,
                RecipientId = data.BorrowerSsn,
                ScriptId = Program.ScriptId
            };
            result = arcData.AddArc();

            if (!isFinalARC)
            {
                if (result != null && result.ArcAdded && result.ArcAddProcessingId > 0) // Update any ProcessingData record with the ARC that has been added
                    DA.UpdateArcId(data, result.ArcAddProcessingId);
                else
                {
                    string message = $"Error adding ARC {arc} for Account {data.AccountNumber}.";
                    Console.WriteLine(message);
                    LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning, result.Ex);
                }
            }
            else if ((result != null && result.ArcAdded ? result.ArcAddProcessingId : 0) > 0 && isFinalARC)
            {
                DA.UpdateFinalArcId(data, result.ArcAddProcessingId);
            }
        }

        private bool IsReadyForEndingArc(string borrowerSsn, List<ErrorData> errorRecords, DateTime addedAt)
        {
            List<int> unprocessedLoans = DA.GetAllUnprocssedLoansForBorrower(borrowerSsn, addedAt);

            if (errorRecords.Count == 0 && unprocessedLoans.Count > 0) // If no loans had errors but still there are more loans to work
                return false;
            else if (unprocessedLoans.Count == 0) // If no loans left to work for borrower
                return true;

            List<int> unworkedLoans = unprocessedLoans.Where(p => !p.IsIn(errorRecords.Select(o => o.LoanSeq).ToArray())).ToList();

            if (unworkedLoans.Count > 0) // If all loans were either worked or hit error
                return false;
            return true;
        }

        /// <summary>
        /// Takes all the comments for the different errors for the different loans and
        /// disbursement sequences and combines them into one.
        /// </summary>
        public string FormatComment(List<ErrorData> errorData)
        {
            List<string> comments = errorData.Select(p => p.ErrorMessage + p.SessionMessage).Distinct().ToList();
            string collectiveComment = "";

            foreach (var error in comments)
            {
                string loanSequences = "For Loan Seq ";
                foreach (var ed in errorData)
                {
                    if (ed.ErrorMessage + ed.SessionMessage == error)
                        loanSequences += $"{ed.LoanSeq}, ";
                }
                loanSequences = loanSequences.Remove(loanSequences.Length - 2) + ": ";
                collectiveComment += loanSequences + error + " ";
            }

            return collectiveComment.Remove(collectiveComment.Length - 1).SafeSubString(0, 300);  // ArcAdd only allows 300 chars
        }

        private void PrintLetters()
        {
            List<PrintingData> population = DA.GetPrintingAccounts();
            //Convert loan level records into 1 record per borrower with list of loans and list of closureIds
            var borrowerLoans = population.GroupBy(x => x.BorrowerSsn)
                .Select(y => new BorrowerPrintRecord()
                {
                    BorrowerSsn = y.Key,
                    AccountNumber = y.Select(z => z.AccountNumber).First(),
                    AddedAt = y.Select(z => z.AddedAt).First(),
                    AllLoans = DA.GetAllLoans(y.Key),
                    DischargedLoans = y.Select(z => z.LoanSeq).Distinct().ToList(),
                    SchoolCodes = y.Select(z => z.SchoolCode).Distinct().ToList(),
                    ClosedSchoolIds = y.Select(z => z.SchoolClosureDataId).Distinct().ToList()
                });

            //Check to see if a document needs to be printed
            foreach (var record in borrowerLoans)
                PrintingProcess(record);
        }

        private void AddFinalArc()
        {
            var population = DA.GetFinalArcData();
            foreach (string bor in population.Select(p => p.BorrowerSsn).Distinct().ToList())
            {
                List<int> loanSeqs = population.Where(p => p.BorrowerSsn == bor).Select(l => l.LoanSeq).Distinct().OrderBy(b => b).ToList();
                string accountNumber = population.Where(p => p.BorrowerSsn == bor).Select(a => a.AccountNumber).FirstOrDefault();
                DateTime addedAt = population.Where(p => p.BorrowerSsn == bor).Select(a => a.AddedAt).FirstOrDefault();
                string comment = $"Closed School Discharge approval letter sent to borrower.  FSA approved Closed School Discharge for loan sequences {string.Join(", ", loanSeqs)}. Discharge process completed on {DateTime.Now.ToString("MM/dd/yyyy")}.";
                ProcessingData pData = new ProcessingData() { BorrowerSsn = bor, AccountNumber = accountNumber, LoanSeqs = loanSeqs, AddedAt = addedAt };
                AddArc(pData, ADCSH, comment, true);
            }
        }

        /// <summary>
        /// Checks to see if all the records have been processed. If all are processed, at record to PrintProcessing
        /// </summary>
        private void PrintingProcess(BorrowerPrintRecord printData)
        {
            try
            {
                Dictionary<string, string> schools = new Dictionary<string, string>();
                string tempSchool;
                foreach (string schoolCode in printData.SchoolCodes)
                {
                    tempSchool = DA.GetSchoolName(schoolCode);
                    if (tempSchool == null)
                    {
                        string errorMessage = $"Unable to identify school name. School Code: {schoolCode}. Account number:{printData.AccountNumber}.";
                        LogRun.AddNotification(errorMessage, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    }
                    if (!schools.ContainsKey(schoolCode))
                        schools.Add(schoolCode, tempSchool);
                }

                bool wasSuccessfullyPrinted = true;
                foreach (string school in schools.Keys)
                {
                    List<DischargeRecords> discharges = DA.GetDischargeTotal(printData, school);
                    if (discharges.Count > 0)
                    {
                        Console.WriteLine($"All disbursements have been processed for Account:{printData.AccountNumber}, adding record into [print].PrintProcess to send letter.");

                        string letterData = BuildLetterData(printData, discharges.Sum(p => p.DischargeAmount), schools[school]);
                        int? printProcessingId = DA.InsertPrintProcessing(letterData, printData.AccountNumber);
                        if (printProcessingId.HasValue)
                        {
                            foreach (DischargeRecords record in discharges)
                                DA.UpdatePrintProcessingId(record.SchoolClosureDataId, printProcessingId.Value);
                        }
                        else
                            wasSuccessfullyPrinted = false;
                    }
                }
                if (wasSuccessfullyPrinted)
                {
                    ProcessingData pData = new ProcessingData() { BorrowerSsn = printData.BorrowerSsn, AccountNumber = printData.AccountNumber, LoanSeq = printData.DischargedLoans.First(), LoanSeqs = printData.DischargedLoans, AddedAt = printData.AddedAt };
                    string loans = string.Join(", ", printData.DischargedLoans);
                    AddArc(pData, DRDCR, $"Closed School Loan Discharge Credit Review for Loan Sequence(s) {loans}");
                }
                else
                {
                    string message = $"There was an error adding the [print].PrintProcessing record to the database for borrower:{printData.AccountNumber};";
                    LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                }

            }
            catch (Exception ex)
            {
                string message = $"There was an error adding the [print].PrintProcessing record to the database for borrower:{printData.AccountNumber};";
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
            }
        }

        private string BuildLetterData(BorrowerPrintRecord data, double totalDischarge, string school)
        {
            SystemBorrowerDemographics demos = RI.GetDemographicsFromTx1j(data.BorrowerSsn);
            data.AccountNumber = demos.AccountNumber;
            string keyLine = DocumentProcessing.ACSKeyLine(data.BorrowerSsn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
            string letterData = $"{keyLine},{DateTime.Now.ToShortDateString()},{demos.AccountNumber},{demos.FirstName} {demos.LastName},{demos.Address1},{demos.Address2},{demos.City},{demos.State},"
                                + $"{demos.ZipCode},{demos.Country},\"{totalDischarge}\",\"{school.Trim()}\"";
            foreach (int loan in data.AllLoans)
            {
                LoanDetail detail = DA.GetLoanDetail(data.BorrowerSsn, loan);
                letterData += $",{detail.LoanProgram},{detail.FirstDisbursementDate.ToShortDateString()},\"{detail.CurrentBalance.ToString("$#,##0.00")}\"";
            }
            for (int i = 0; i < 30 - data.AllLoans.Count; i++)
                letterData += $",,,";
            letterData += ",MA4481";
            return letterData;
        }
    }
}