using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Uheaa.Common.ProcessLogger;

namespace PRIDRCRP
{
    public class LoanSequencePdfParser : PdfParser
    {
        public FileParser.FileSection Section { get; protected set; } = FileParser.FileSection.PDF;
        private ProcessLogRun logRun;

        public List<LoanSequenceMappingRecord> LoanRecords { get; private set; } = new List<LoanSequenceMappingRecord>();

        public LoanSequencePdfParser(ProcessLogRun logRun)
        {
            this.logRun = logRun;
        }

        public List<LoanSequenceMappingRecord> ProcessLoanRecords(string filename)
        {
            List<string> pages = ReadPdfFilePages(filename);
            foreach(string page in pages)
            {
                ReadLinesFromPages(page);
            }
            return LoanRecords;
        }

        public void ReadLinesFromPages(string page)
        {
            if(page.Contains("LOAN INFORMATION:"))
            {
                List<LoanSequenceMappingRecord> recordsByLoan = new List<LoanSequenceMappingRecord>();
                bool secondHeaderFound = false;
                bool endOfHistoryFound = false;
                string ssn = ParseField(page, "SSN", 14);
                if(ssn.Length != 9)
                {
                    logRun.AddNotification("Unable to dertermine Ssn Received: " + ssn, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    throw new Exception("Unable to dertermine Ssn Received: " + ssn);
                }
                
                List<string> lines = page.Split(new string[] { Environment.NewLine, "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                for(int i = 0; i < lines.Count; i++)
                {
                    if(lines[i].Replace(" ", "").Contains("DISBURSEMENTDISBURSEMENTRATEDATE"))
                    {
                        secondHeaderFound = true;
                    }
                    else if(lines[i].Replace(" ", "").Contains("PAYMENTAMOUNT"))
                    {
                        endOfHistoryFound = true;
                    }
                    else if(secondHeaderFound && !endOfHistoryFound)
                    {
                        recordsByLoan.Add(ParseLine(lines[i], ssn));
                    }
                }
                LoanRecords.AddRange(recordsByLoan);
            }
        }

        private LoanSequenceMappingRecord ParseLine(string line, string ssn)
        {
            LoanSequenceMappingRecord record = new LoanSequenceMappingRecord();
            record.Ssn = ssn;
            List<string> splitLine = line.Split(new char[0]).ToList();
            List<string> splitLineWithoutWhitespace = new List<string>();
            foreach(string str in splitLine)
            {
                if(!string.IsNullOrWhiteSpace(str))
                {
                    splitLineWithoutWhitespace.Add(str);
                }
            }
            if(splitLineWithoutWhitespace.Count == 5)
            {
                record.DisbursementDate = Convert.ToDateTime(splitLineWithoutWhitespace[0]);
                record.DisbursementAmount = ConvertPostNegativeToDecimal(splitLineWithoutWhitespace[1]);
                record.InterestRate = ConvertPostNegativeToDecimal(splitLineWithoutWhitespace[2]);
                record.GuaranteeDate = Convert.ToDateTime(splitLineWithoutWhitespace[3]);
                record.LoanNum = int.Parse(splitLineWithoutWhitespace[4]);
            }
            else
            {
                logRun.AddNotification("Loan Sequence Mapping Row Does Not Contain Expected Column Count. Found: " + splitLineWithoutWhitespace.Count, NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }

            return record;
        }
    }
}
