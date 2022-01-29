using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace PRIDRCRP
{
    public sealed class DisbursementInformation : FileInformation
    {
        public List<DisbursementRecord> disbursementRecords { get; private set; } = new List<DisbursementRecord>();
        public override List<Error> ExceptionLog { get; protected set; } = new List<Error>();

        public DisbursementRecord totals { get; private set; }

        public override FileParser.FileSection Section { get; protected set; } = FileParser.FileSection.Section_I;
        private ProcessLogRun logRun;

        private bool headerRow1Found = false;
        private bool headerRow2Found = false;
        private bool emptyRowAfterHeaderFound = false;
        private bool totalRowFound = false;
        private bool done = false;

        public DisbursementInformation(ProcessLogRun logRun)
        {
            this.logRun = logRun;
        }

        public override bool FoundInformation()
        {
            return done;
        }

        public override void GetInformation(string line)
        {
            //update the location of the file reader in the set if disbursement information
            bool skip = CheckRelativeDisbursementLocation(line);
            //start reading rows after finding all three rows preceding data rows
            if(!done && emptyRowAfterHeaderFound && !skip)
            {
                if(totalRowFound)
                {
                    totals = GetTotals(line);
                    done = true;
                }
                else
                {
                    disbursementRecords.Add(GetDisbRecordFromRow(line));
                }
            }
        }

        /// <summary>
        /// Gets a disbursement record from a row of disbursement information
        /// Parsing needs to be done using substring because the data and header is not on the same line
        /// </summary>
        private DisbursementRecord GetDisbRecordFromRow(string line)
        {
            DisbursementRecord disbursementRecord = new DisbursementRecord();
            disbursementRecord.DisbursementDate = Convert.ToDateTime(StringParsingHelper.SafeSubStringTrimmed(line,2, 8)); //Date is at location 2
            disbursementRecord.InterestRate = Convert.ToDecimal(StringParsingHelper.SafeSubStringTrimmed(line,11, 6).Replace("%", "")); //Interest Rate is at 11, need to remove %
            disbursementRecord.LoanType = StringParsingHelper.SafeSubStringTrimmed(line,18, 9); //Loan Type is at 18
            disbursementRecord.DisbursementNumber = StringParsingHelper.SafeSubStringTrimmed(line,27, 2);//Disbursement number is at 27
            disbursementRecord.LoanId = StringParsingHelper.SafeSubStringTrimmed(line,30, 11);
            disbursementRecord.DisbursementAmount = Convert.ToDecimal(StringParsingHelper.SafeSubStringTrimmed(line,41, 13).Replace("$", ""));
            disbursementRecord.CapitalizedInterest = Convert.ToDecimal(StringParsingHelper.SafeSubStringTrimmed(line,54, 13).Replace("$", ""));
            disbursementRecord.RefundCancel = Convert.ToDecimal(StringParsingHelper.SafeSubStringTrimmed(line, 67, 13).Replace("$", ""));
            disbursementRecord.BorrowerPaidPrincipal = Convert.ToDecimal(StringParsingHelper.SafeSubStringTrimmed(line, 80, 12).Replace("$", ""));
            disbursementRecord.PrincipalOutstanding = Convert.ToDecimal(StringParsingHelper.SafeSubStringTrimmed(line, 92, 13).Replace("$", ""));
            disbursementRecord.InterestPaid = Convert.ToDecimal(StringParsingHelper.SafeSubStringTrimmed(line, 105, 13).Replace("$", ""));
            return disbursementRecord;
        }

        private DisbursementRecord GetTotals(string line)
        {
            DisbursementRecord disbursementRecord = new DisbursementRecord();
            disbursementRecord.DisbursementAmount = Convert.ToDecimal(StringParsingHelper.SafeSubStringTrimmed(line, 41, 13).Replace("$", ""));
            disbursementRecord.CapitalizedInterest = Convert.ToDecimal(StringParsingHelper.SafeSubStringTrimmed(line, 54, 13).Replace("$", ""));
            disbursementRecord.RefundCancel = Convert.ToDecimal(StringParsingHelper.SafeSubStringTrimmed(line, 67, 13).Replace("$", ""));
            disbursementRecord.BorrowerPaidPrincipal = Convert.ToDecimal(StringParsingHelper.SafeSubStringTrimmed(line, 80, 12).Replace("$", ""));
            disbursementRecord.PrincipalOutstanding = Convert.ToDecimal(StringParsingHelper.SafeSubStringTrimmed(line, 92, 13).Replace("$", ""));
            disbursementRecord.InterestPaid = Convert.ToDecimal(StringParsingHelper.SafeSubStringTrimmed(line, 105, 13).Replace("$", ""));
            return disbursementRecord;
        }

        /// <summary>
        /// Checks the relavtive position in the file
        /// </summary>
        /// <returns> true if the processing method should skip adding the record for the line</returns>
        private bool CheckRelativeDisbursementLocation(string line)
        {
            if(!done)
            {
                if (!headerRow1Found)
                {
                    if (line.Replace(" ", "") == "DISBINTLOANDISBLOANDISBAMTCAPITALIZEDREFUND/BORRPAIDPRINBALPAIDLC/NSF0")
                    {
                        headerRow1Found = true;
                    }
                }
                else if (headerRow1Found && !headerRow2Found)
                {
                    if (line.Replace(" ", "") == "DATERATETYPE#I.D.INTERESTCANCELPRINCIPALOUTSTANDINGINTERESTPAID0")
                    {
                        headerRow2Found = true;
                    }
                }
                else if (headerRow2Found && !emptyRowAfterHeaderFound)
                {
                    if (line.Trim() == "0")
                    {
                        emptyRowAfterHeaderFound = true;
                        return true;
                    }
                }
                else if (emptyRowAfterHeaderFound && !totalRowFound)
                {
                    if(line.Replace(" ", "").Contains("BORROWERHISTORYANDACTIVITYREPORT") || line.Replace(" ", "").Contains("PAGE#"))
                    {
                        return true;
                    }
                    if (line.Contains("TOTAL"))
                    {
                        totalRowFound = true;
                    }
                }
            }
            return false;
        }

        public override bool ValidateInformation(string file, DataAccess DA)
        {
            foreach(DisbursementRecord record in disbursementRecords)
            {
                if(record == null || !record.DisbursementDate.HasValue || !record.InterestRate.HasValue || record.LoanType == null || record.DisbursementNumber == null || record.LoanId == null || !record.DisbursementAmount.HasValue || !record.CapitalizedInterest.HasValue || !record.RefundCancel.HasValue || !record.BorrowerPaidPrincipal.HasValue || !record.PrincipalOutstanding.HasValue || !record.InterestPaid.HasValue)
                {
                    DA.ThrowDataValidationError("Bad Disbursement Records. File: " + file);
                }
            }
            return true;

        }

        public override bool WriteToDatabase(DataAccess DA)
        {
            return DA.InsertToDisbursements(disbursementRecords);
        }

    }
}
