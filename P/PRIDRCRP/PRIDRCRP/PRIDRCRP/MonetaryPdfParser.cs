using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common;

namespace PRIDRCRP
{
    public class MonetaryPdfParser : PdfParser
    {
        public DataAccess DA { get; set; }
        public FileParser.FileSection Section { get; protected set; } = FileParser.FileSection.PDF;
        private ProcessLogRun logRun;

        public List<List<MonetaryHistoryRecord>> monetaryRecordsByLoan { get; private set; } = new List<List<MonetaryHistoryRecord>>();
        public List<LoanSequenceMappingRecord> loansToMap { get; private set; } = new List<LoanSequenceMappingRecord>();
        public MonetaryPdfParser(DataAccess da, ProcessLogRun logRun)
        {
            this.logRun = logRun;
            DA = da;
        }

        public void ProcessMonetaryHistory(string filename)
        {
            LoanSequencePdfParser loanSequenceData = new LoanSequencePdfParser(logRun);
            loansToMap = loanSequenceData.ProcessLoanRecords(filename);

            List<string> pages = ReadPdfFilePages(filename);
            foreach (string page in pages)
            {
                ReadLinesFromPages(page, loansToMap);
            }
        }

        public void ReadLinesFromPages(string page, List<LoanSequenceMappingRecord> loansToMap)
        {
            //If the page contains the term Monetary history, it will contian the information needed
            if (page.Contains("MONETARY HISTORY") && !page.Contains("BORROWER HISTORY"))
            {
                List<MonetaryHistoryRecord> recordsByLoan = new List<MonetaryHistoryRecord>();
                bool secondHeaderFound = false;
                bool endOfHistoryFound = false;
                bool addMonetaryRecordsToList = true;
                string ssn = ParseField(page, "BORROWER:", 10);
                string loanSequenceStr = ParseField(page, "LOAN NUM:", 5);
                int? loanNum = loanSequenceStr.ToIntNullable();
                if (!loanNum.HasValue || ssn.Length != 9)
                {
                    logRun.AddNotification("Unable to dertermine Ssn Received: " + ssn, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    throw new Exception("Unable to dertermine Ssn Received: " + ssn);
                }

                List<LoanSequenceMappingRecord> loanInfo = loansToMap.Where(p => p.Ssn == ssn && p.LoanNum == loanNum).ToList();

                loanInfo = DA.CalculateLoanSequence(loanInfo);

                foreach (var monetaryRecords in monetaryRecordsByLoan)
                {
                    if (monetaryRecords != null &&
                        monetaryRecords.Count > 0 &&
                        monetaryRecords.First().Ssn == ssn &&
                        monetaryRecords.First().LoanNum == loanNum)
                    {
                        recordsByLoan = monetaryRecords;
                        addMonetaryRecordsToList = false;
                    }
                }

                List<string> lines = page.Split(new string[] { Environment.NewLine, "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                for (int i = 0; i < lines.Count; i++)
                {
                    if (lines[i].Replace(" ", "").Contains("DATEDATECODECODEAMTPRINCIPALINTERESTFEESAFTERTRANAFTERTRANAFTERTRAN"))
                    {
                        secondHeaderFound = true;
                    }
                    else if (lines[i].Replace(" ", "").Contains("**ENDOFMONETARYHISTORY**"))
                    {
                        endOfHistoryFound = true;
                    }
                    else if (secondHeaderFound && !endOfHistoryFound)
                    {
                        recordsByLoan.Add(ParseLine(lines[i], ssn, loanNum.Value, loanInfo.First().LoanSequence));
                    }
                }

                if (addMonetaryRecordsToList)
                {
                    monetaryRecordsByLoan.Add(recordsByLoan);
                }
            }
        }

        private MonetaryHistoryRecord ParseLine(string line, string ssn, int loanNum, int? loanSequence)
        {
            MonetaryHistoryRecord record = new MonetaryHistoryRecord();
            record.Ssn = ssn;
            record.LoanNum = loanNum;
            record.LoanSequence = loanSequence;
            List<string> splitLine = line.Split(new char[0]).ToList();
            List<string> splitLineWithoutWhitespace = new List<string>();
            foreach (string str in splitLine)
            {
                if (!string.IsNullOrWhiteSpace(str))
                {
                    splitLineWithoutWhitespace.Add(str);
                }
            }
            if (splitLineWithoutWhitespace.Count == 10)
            {
                record.TransactionDate = Convert.ToDateTime(splitLineWithoutWhitespace[0]);
                record.PostDate = Convert.ToDateTime(splitLineWithoutWhitespace[1]);
                record.TransactionCode = splitLineWithoutWhitespace[2];
                record.CancelCode = "";
                record.TransactionAmount = ConvertPostNegativeToDecimal(splitLineWithoutWhitespace[3]);
                record.AppliedPrincipal = ConvertPostNegativeToDecimal(splitLineWithoutWhitespace[4]);
                record.AppliedInterest = ConvertPostNegativeToDecimal(splitLineWithoutWhitespace[5]);
                record.AppliedFees = ConvertPostNegativeToDecimal(splitLineWithoutWhitespace[6]);
                record.PrincipalBalanceAfterTransaction = ConvertPostNegativeToDecimal(splitLineWithoutWhitespace[7]);
                record.InterestBalanceAfterTransaction = ConvertPostNegativeToDecimal(splitLineWithoutWhitespace[8]);
                record.FeesBalanceAfterTransaction = ConvertPostNegativeToDecimal(splitLineWithoutWhitespace[9]);
            }
            else if (splitLineWithoutWhitespace.Count == 11)
            {
                record.TransactionDate = Convert.ToDateTime(splitLineWithoutWhitespace[0]);
                record.PostDate = Convert.ToDateTime(splitLineWithoutWhitespace[1]);
                record.TransactionCode = splitLineWithoutWhitespace[2];
                record.CancelCode = splitLineWithoutWhitespace[3];
                record.TransactionAmount = ConvertPostNegativeToDecimal(splitLineWithoutWhitespace[4]);
                record.AppliedPrincipal = ConvertPostNegativeToDecimal(splitLineWithoutWhitespace[5]);
                record.AppliedInterest = ConvertPostNegativeToDecimal(splitLineWithoutWhitespace[6]);
                record.AppliedFees = ConvertPostNegativeToDecimal(splitLineWithoutWhitespace[7]);
                record.PrincipalBalanceAfterTransaction = ConvertPostNegativeToDecimal(splitLineWithoutWhitespace[8]);
                record.InterestBalanceAfterTransaction = ConvertPostNegativeToDecimal(splitLineWithoutWhitespace[9]);
                record.FeesBalanceAfterTransaction = ConvertPostNegativeToDecimal(splitLineWithoutWhitespace[10]);
            }
            else
            {
                //Suppressing Error messages
                //logRun.AddNotification("Monetary History Row Does Not Contain Expected Column Count. Found: " + splitLineWithoutWhitespace.Count, NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }

            return record;
        }

        public bool ValidateInformation(string file, ManualReviewHelper review)
        {
            string errorLog = "";
            string ssn = null;
            foreach (List<MonetaryHistoryRecord> records in monetaryRecordsByLoan)
            {
                foreach (MonetaryHistoryRecord record in records)
                {
                    int intSsn = (record.Ssn ?? "0").ToInt();
                    bool ssnInvalid = (record.Ssn == null) || (record.Ssn.Trim().Length != 9) || intSsn == 0;
                    if (record == null || ssnInvalid || !record.LoanNum.HasValue || !record.TransactionDate.HasValue || !record.PostDate.HasValue || record.TransactionCode == null || !record.TransactionAmount.HasValue || !record.AppliedPrincipal.HasValue || !record.AppliedPrincipal.HasValue || !record.AppliedInterest.HasValue || !record.AppliedFees.HasValue || !record.PrincipalBalanceAfterTransaction.HasValue || !record.InterestBalanceAfterTransaction.HasValue || !record.FeesBalanceAfterTransaction.HasValue)
                    {
                        DA.ThrowDataValidationError("Bad Monetary History Records. File: " + file);
                    }
                    if(record != null && record.LoanSequence == null)
                    {
                        string loanNumStr = record.LoanNum.HasValue ? record.LoanNum.Value.ToString() : "";
                        ssn = record.Ssn;
                        if (!errorLog.Contains($" Unable to determine loan sequence for PDF record. SSN: {record.Ssn ?? ""}, Loan Num: { loanNumStr };"))
                        {
                            //Suppressing errors
                            //errorLog += $" Unable to determine loan sequence for PDF record. SSN: {record.Ssn ?? ""}, Loan Num: { loanNumStr };";
                        }
                    }
                }
            }
            if(errorLog != "")
            {
                //Supressing errors
                //review.FlagForManualReview(errorLog, ssn);
            }
            return true;
        }

        public bool WriteToDatabase(DataAccess DA)
        {
            bool success = true;
            foreach (List<MonetaryHistoryRecord> records in monetaryRecordsByLoan)
            {
                success = success && DA.InsertToMonetaryHistory(records);
            }
            return success;
        }
    }
}
