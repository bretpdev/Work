using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;

namespace SCRAINTUPD
{
    public partial class ScraProcess
    { 
        public List<EmergencyPeriod> EmergencyPeriods { get; set; }

        public List<ScraRecord> InterestProcessing(List<ScraRecord> recordsByBorrower)
        {
            recordsByBorrower = InterestUpdates(recordsByBorrower);
            if (recordsByBorrower.Any(p => !p.TS06UpdatedAt.HasValue))
            {
                ErrorProcessing(recordsByBorrower, "NSCRA");
                return null;
            }
            return recordsByBorrower;
        }

        public List<ScraRecord> InterestUpdates(List<ScraRecord> records)
        {
            foreach (ScraRecord record in records)
            {
                if (record.ScriptAction.IsIn("E", "U") && !record.TS06UpdatedAt.HasValue)
                {
                    record.TS06UpdatedSuccessfully = UpdateTS06(record);
                    if (record.TS06UpdatedSuccessfully)
                    {
                        record.TS06UpdatedAt = DateTime.Now;
                        DA.SetProcessedTS06(record, true);
                    }
                    else
                        DA.SetProcessedTS06(record, false);
                }
            }
            return records;
        }

        public bool UpdateTS06(ScraRecord record)
        {
            if (!FindLoan(record))
                return false;

            bool useLN72Rate = record.LN72RegRate > 6 && record.ScriptAction == "E";
            
            DateTime effectiveBegin = useLN72Rate ? record.LN72InterestBeginDate.Value : record.DODBeginDate.Value;
            
            if (effectiveBegin <= record.LoanAddDate.Value)
            {
                if (record.ScriptAction == "E")
                    return AddMRecord(record, record.LoanAddDate.Value.ToString("MMddyyyy"));
                else if (record.ScriptAction == "U")
                    return AddMCRecord(record, effectiveBegin);
            }
            else if (effectiveBegin > record.LoanAddDate.Value)
                return AddMRecord(record);

            return false;
        }

        public bool FindLoan(ScraRecord record)
        {
            RI.FastPath(string.Format("TX3Z/CTS06{0}", record.BorrowerSSN));
            if (RI.MessageCode == "01080")
            {
                record.SessionErrorMessage = RI.Message;
                return false;
            }
            else if (RI.ScreenCode == "TSX05") //we have more than one loan
            {
                for (int row = 8; RI.MessageCode != "90007"; row++)
                {
                    if (RI.CheckForText(row, 3, "  ") || row > 20)
                    {
                        row = 7;
                        RI.Hit(Key.F8);
                        continue;
                    }

                    if (RI.GetText(row, 47, 3).ToIntNullable() == record.LoanSequence)
                    {
                        RI.PutText(21, 18, RI.GetText(row, 3, 2), Key.Enter, true);
                        break;
                    }
                }
            }

            if (RI.ScreenCode != "TSX07")
            {
                record.SessionErrorMessage = RI.Message;
                return false; //no loans found
            }
            return true;
        }

        public bool UpdateEndingRate(ScraRecord record)
        {
            FindLoan(record);
            bool deletedRowPrior = false;
            DateTime beginSession, endSession;
            for (int row = 12; RI.MessageCode != "90007"; row++)
            {
                if (row > 21 || RI.CheckForText(row, 3, " "))
                {
                    row = 10; //start at 11 for 2nd page
                    RI.Hit(Key.F8);
                    continue;
                }

                beginSession = RI.GetText(row, 21, 10).ToDate();
                endSession = RI.GetText(row, 33, 10).ToDate();

                bool isActive = RI.CheckForText(row, 50, "A");
                string rdcCode = RI.GetText(row, 14, 2);
                string rdcType = RI.GetText(row, 18, 1);

                bool beginLessDodEnd = beginSession <= record.DODEndDate.Value;
                bool beginGreaterDodBegin = beginSession >= record.DODBeginDate.Value || beginSession > DateTime.Now;
                bool endAfterDodEnd = endSession > record.DODEndDate.Value;
                bool historyRecordBypass = endAfterDodEnd && endSession < DateTime.Now;
                bool dodCheck = (beginLessDodEnd && beginGreaterDodBegin) || beginGreaterDodBegin;
                bool eCase = record.ScriptAction == "E" && record.LN72RegRate < 6.00m;
                //bool emergencyRemoval = period != null && beginSession > record.DODEndDate;
                if (historyRecordBypass)
                    continue; //Skip rows where DOD provides shortened period on a past record

                if (!isActive)
                    return true;

                if ((isActive && rdcCode == "M" && rdcType == "_" && dodCheck) || (isActive && rdcCode.IsIn("M") && rdcType.IsIn("_", "C") && eCase))
                {
                    RI.PutText(22, 17, RI.GetText(row, 2, 2), Key.F11, true);
                    if (!RI.MessageCode.IsIn("01005", "06207"))
                    {
                        record.SessionErrorMessage = RI.Message;
                        return false;
                    }
                    if(!deletedRowPrior)
                        row--; //first time delete shifts screen by 2 rows
                    deletedRowPrior = true;
                    row--;
                }
                else if(isActive && rdcCode == "M" && rdcType == "_" && endAfterDodEnd)
                {
                    FindLoan(record);
                    RI.PutText(11, 6, "6.000", true);
                    RI.PutText(11, 14, "M", true); //RDC_CDE
                    RI.PutText(11, 21, beginSession.ToString("MMddyyyy"), true);
                    RI.PutText(11, 33, record.DODEndDate.Value.ToString("MMddyyyy"), Key.Enter, true);
                    if (RI.MessageCode == "04235") //end date override only
                        RI.Hit(Key.Enter);

                    if (!RI.MessageCode.IsIn("01005", "06207"))
                    {   
                        record.SessionErrorMessage = RI.Message;
                        return false; //send record and entire borrower to error processing
                    }
                    if (!deletedRowPrior)
                        row--; //first time delete shifts screen by 2 rows
                    deletedRowPrior = true;
                    row--;
                }
            }
            return true;
        }

        public bool AddMCRecord(ScraRecord record, DateTime effectiveBegin)
        {
            DateTime beginSession = DateTime.Now;
            DateTime endSession = DateTime.Now;
            int hitCount = 0;
            for(int row = 12; RI.MessageCode != "90007"; row++)
            {
                if(row > 21)
                {
                    RI.Hit(Key.F8);
                    row = 10;
                    hitCount++;
                    continue;
                }
                if (RI.GetText(row, 50, 1) == "I")
                    break;

                if (RI.CheckForText(row, 14, "M") && RI.CheckForText(row, 18, "C"))
                {
                    beginSession = RI.GetText(row, 21, 10).ToDate();
                    endSession = RI.GetText(row, 33, 10).ToDate();
                    break;
                }
            }

            if(hitCount > 0)
                RI.Hit(Key.F7, hitCount);

            RI.PutText(11, 6, "6.000", true);
            RI.PutText(11, 14, "M", true); //RDC_CDE
            RI.PutText(11, 18, "C", true); //RDC_TYP
            string dateBegin = (effectiveBegin > defaultBeginDate ? effectiveBegin : defaultBeginDate).ToString("MMddyyyy");
            string dateEnd = record.LoanAddDate.Value.AddDays(-1).ToString("MMddyyyy");
            RI.PutText(11, 21, dateBegin, true);//Bigger of DODBegin and 08/14/2008
            RI.PutText(11, 33, dateEnd, Key.Enter, true);
            if (RI.MessageCode.IsIn("01531")) //super specific issue where loan add date = 08/14/08
                return false;

            if (RI.MessageCode.IsIn("04235", "04236") && !(dateBegin == beginSession.ToString("MMddyyyy") && dateEnd == endSession.ToString("MMddyyyy")))
                RI.Hit(Key.Enter);
            else if (RI.MessageCode.IsIn("04235", "04236") && (dateBegin == beginSession.ToString("MMddyyyy") && dateEnd == endSession.ToString("MMddyyyy")))
                return AddMRecord(record, dateEnd);

            if (RI.MessageCode.IsIn("01005", "06207")) //worked or consol but worked
                return AddMRecord(record, dateEnd);
            else
            {
                record.SessionErrorMessage = RI.Message;
                return false;
            }
        }

        public bool AddMRecord(ScraRecord record, string pEndDate = null)
        {
            FindLoan(record);

            string beginDate = "";
            if (pEndDate.IsPopulated() && record.LoanAddDate.Value > record.DODBeginDate.Value)
                beginDate = (record.LoanAddDate.Value > defaultBeginDate ? record.LoanAddDate.Value : defaultBeginDate).ToString("MMddyyyy");
            if (pEndDate.IsPopulated() && record.LoanAddDate.Value <= record.DODBeginDate.Value)
                beginDate = (record.DODBeginDate.Value > defaultBeginDate ? record.DODBeginDate.Value : defaultBeginDate).ToString("MMddyyyy");
            else if (pEndDate.IsNullOrEmpty())
                beginDate = (record.DODBeginDate.Value > defaultBeginDate ? record.DODBeginDate.Value : defaultBeginDate).ToString("MMddyyyy");

            string endDate = (record.DODEndDate.HasValue && record.DODEndDate.Value != defaultEndDate ? record.DODEndDate.Value : oneYearFuture).ToString("MMddyyyy");

            RI.PutText(11, 6, "6.000", true);
            RI.PutText(11, 14, "M", true);//RDC_CDE
            RI.PutText(11, 21, beginDate, true);
            RI.PutText(11, 33, endDate, Key.Enter, true);

            if (RI.MessageCode.IsIn("04235", "06337"))
            {
                if (UpdateEndingRate(record))
                    return PieceTogetherMRecords(record);
            }

            if (RI.MessageCode.IsIn("01005", "06207"))
                return true; //success.  This will allow the script to go to the next step
            else
            {
                record.SessionErrorMessage = RI.Message;
                return false; //send record and entire borrower to error processing
            }
        }

        private bool PieceTogetherMRecords(ScraRecord record)
        {
            DateTime beginSession, endSession;
            int timesToHitF8 = 0;
            FindLoan(record);
            for (int row = 12; RI.MessageCode != "90007"; row++)
            {
                for (int count = timesToHitF8; count > 0 && (row == 11 || RI.MessageCode.IsIn("01005","06207")); count--)
                {
                    RI.Hit(Key.F8);
                    if (RI.MessageCode == "90007")
                        break;
                }

                if (row > 21 || RI.CheckForText(row, 3, " "))
                {
                    row = 10; //2nd page starts 1 row higher than first
                    timesToHitF8++;
                    continue;
                }
                

                beginSession = RI.GetText(row, 21, 10).ToDate();
                endSession = RI.GetText(row, 33, 10).ToDate();

                bool isActive = RI.CheckForText(row, 50, "A");
                double originalIntRate = RI.GetText(row, 75, 5).ToDouble();

                string endDate = (record.DODEndDate.HasValue && record.DODEndDate.Value != defaultEndDate ? record.DODEndDate.Value : oneYearFuture).ToString("MMddyyyy");

                DateTime compareBegin = (record.DODBeginDate.HasValue && record.DODBeginDate.Value > defaultBeginDate ? record.DODBeginDate.Value : defaultBeginDate);
                DateTime validStart = beginSession > compareBegin ? beginSession : compareBegin;
                if (!isActive)
                    return true;

                bool sessionBeginLessDodEnd = beginSession < record.DODEndDate.Value;
                bool windowBeginDate = (beginSession >= compareBegin || compareBegin < endSession);
                if (isActive && sessionBeginLessDodEnd && windowBeginDate)
                {
                    if (originalIntRate <= 6.00) //skip these rows
                        continue;
                    else
                    {
                        if (endSession < endDate.ToDate())
                            endDate = endSession.ToString("MMddyyyy");
                        FindLoan(record);
                        RI.PutText(11, 6, "6.000", true);
                        RI.PutText(11, 14, "M", true);//RDC_CDE
                        RI.PutText(11, 21, validStart.ToString("MMddyyyy"), true);
                        RI.PutText(11, 33, endDate, Key.Enter, true);

                        if (RI.MessageCode == "04235") //special rates > 6% need to overwrite
                            RI.Hit(Key.Enter);

                        if (RI.MessageCode.IsIn("01005", "06207"))
                        {
                            row--;
                            continue; //success.  This will allow the script to go to the next step
                        }
                        else
                        {
                            record.SessionErrorMessage = RI.Message;
                            return false; //send record and entire borrower to error processing
                        }
                    }
                }
            }
            return true;
        }
    }
}
