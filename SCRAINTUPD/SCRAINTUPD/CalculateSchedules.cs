using System;
using System.Collections.Generic;
using System.Linq;
using Uheaa.Common;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;

namespace SCRAINTUPD
{
    public partial class ScraProcess
    {
        public List<ScraRecord> ScheduleProcessing(List<ScraRecord> recordsByBorrower)
        {
            recordsByBorrower = CalculateSchedules(recordsByBorrower);
            if (recordsByBorrower.Any(p => !p.CalcSchedules))
            {
                ErrorProcessing(recordsByBorrower, "MSCRA");
                return null;
            }

            return recordsByBorrower;
        }

        public List<ScraRecord> CalculateSchedules(List<ScraRecord> records)
        {
            int rowSkip = 0;
            if (records.All(p => p.TS0NUpdatedAt.HasValue))
            {
                var ownerGroupedRecords = records.GroupBy(p => p.LoanOwner);
                foreach(var group in ownerGroupedRecords)
                {
                    RI.FastPath(string.Format("TX3Z/ATS0N{0}", records.FirstOrDefault().BorrowerSSN));
                    if (RI.MessageCode == "01020")
                        return records; //Doesnt set CalcSchedules property since we didnt do anything

                    int recursiveRowSkip = 0;
                    var ownerRecords = group.Where(p => !p.TSX0TUpdatedAt.HasValue).ToList();
                    if (DetermineScreenAndProcess(ownerRecords, rowSkip, ref recursiveRowSkip))
                    {
                        //Updated when updating TSX0T now
                        //foreach (ScraRecord record in ownerRecords.Where(p => p.FoundLoanToDisClose))
                        //{
                        //    DA.SetScheduleCompleted(record, true);
                        //}

                        foreach (ScraRecord record in ownerRecords.Where(p => !p.FoundLoanToDisClose))
                        {
                            DA.SetProcessedTS0N(record, false);
                            record.SessionErrorMessage = "Unable to fully disclose to all loans.  Please review.";
                        }
                    }
                    else
                    {
                        foreach (ScraRecord record in ownerRecords)
                            DA.SetProcessedTS0N(record, false);
                    }
                }

                foreach (ScraRecord record in records.Where(p => p.FoundLoanToDisClose && p.SessionErrorMessage == null))
                {
                    record.CalcSchedules = true;
                    DA.SetProcessedTS0N(record, true);
                }
            }
            else
                records.ForEach(p => p.CalcSchedules = true);
            return records;
        }

        public bool DetermineScreenAndProcess(List<ScraRecord> records, int rowSkip, ref int recursiveRowSkip)
        {
            bool success = false;
            if (RI.ScreenCode == "TSX0P")
                success = TSX0PProcessing(records, rowSkip, ref recursiveRowSkip);
            else if (RI.ScreenCode == "TSX0Q")
                success = TSX0QProcessing(records, rowSkip, ref recursiveRowSkip);
            else if (RI.ScreenCode == "TSX0S")
                success = TSX0SProcessing(records, rowSkip, ref recursiveRowSkip);
            else if (RI.ScreenCode == "TSX0R")
                success = TSX0RProcessing(records, rowSkip, ref recursiveRowSkip);
            return success;
        }

        public bool TSX0PProcessing(List<ScraRecord> records, int rowSkip, ref int recursiveRowSkip)
        {
            for (int row = 8 + rowSkip; RI.MessageCode != "90007"; row++)
            {
                if (row > 20 || RI.CheckForText(row, 5, " "))
                {
                    row = 7;
                    RI.Hit(Key.F8);
                    continue;
                }

                if (CheckInvalidLoans(records))
                    return false;

                recursiveRowSkip = 0;
                RI.PutText(21, 12, RI.GetText(row, 4, 3), Key.Enter, true);
                if (RI.ScreenCode == "TSX0Q")
                {
                    if (!TSX0QProcessing(records, rowSkip, ref recursiveRowSkip))
                        return false;
                }
                else if (RI.ScreenCode == "TSX0S")
                {
                    if (!TSX0SProcessing(records, rowSkip, ref recursiveRowSkip))
                        return false;
                }
                else if (RI.ScreenCode == "TSX0R")
                {
                    if (!TSX0RProcessing(records, rowSkip, ref recursiveRowSkip))
                        return false;
                }
                else
                {
                    records.FirstOrDefault().SessionErrorMessage = RI.Message;
                    return false;
                }

                if (RI.ScreenCode != "TSX0P")
                {
                    RI.Hit(Key.F12);
                    RI.Hit(Key.F5);
                }
            }
            RI.Hit(Key.F5);
            return true;
        }

        public bool TSX0QProcessing(List<ScraRecord> records, int rowSkip, ref int recursiveRowSkip)
        {
            for (int row = 8; RI.MessageCode != "90007"; row++)
            {
                if (row > 19 || RI.CheckForText(row, 19, " "))
                {
                    row = 7;
                    RI.Hit(Key.F8);
                    continue;
                }

                if (CheckInvalidLoans(records))
                    return false;
                
                //Skip owners that don't have any records
                //TODO - Resolve during testing that this completely resolves the issue
                if(records.Select(p => p.LoanOwner.Trim()).Contains(RI.GetText(row,30,8).Trim()))
                {
                    RI.PutText(21, 12, RI.GetText(row, 18, 3), Key.Enter, true);
                }
                //If there are no Loan owners select things the way it was done before
                //else if(records.Select(p => p.LoanOwner.Trim()).Where(p => !p.IsNullOrEmpty()).Count() > 0)
                //{
                //    RI.PutText(21, 12, RI.GetText(row, 18, 3), Key.Enter, true);
                //}
                else
                {
                    continue;
                }

                recursiveRowSkip = 0;
                if (RI.ScreenCode == "TSX0S")
                {
                    if (!TSX0SProcessing(records, rowSkip, ref recursiveRowSkip))
                        return false;
                }
                else if (RI.ScreenCode == "TSX0R")
                {
                    if (!TSX0RProcessing(records, rowSkip, ref recursiveRowSkip))
                        return false;
                }
                else
                {
                    records.FirstOrDefault().SessionErrorMessage = RI.Message;
                    return false;
                }

                if(RI.ScreenCode != "TSX0Q")
                {
                    RI.Hit(Key.F12);
                    RI.Hit(Key.F5);
                }
            }
            return true;
        }

        public bool TSX0SProcessing(List<ScraRecord> records, int rowSkip, ref int recursiveRowSkip)
        {
            for (int row = 11; RI.MessageCode != "90007"; row++)
            {
                if (row > 21 || RI.CheckForText(row, 3, " "))
                {
                    row = 10;
                    RI.Hit(Key.F8);
                    continue;
                }
                if (RI.GetText(row, 3, 1) == "X")
                    RI.PutText(row, 3, "_", Key.Enter);
            }

            RI.Hit(Key.F5);

            for (int row = (11 + recursiveRowSkip); RI.MessageCode != "90007"; row++)
            {
                if (row > 21)
                {
                    records.ForEach(p => p.SessionErrorMessage = "Script encountered more than 1 page on TSX0S and is not coded to handle this. Please process manually.");
                    return false;
                }
                if (RI.CheckForText(row, 3, " "))
                {
                    row = 10;
                    RI.Hit(Key.F8);
                    continue;
                }

                if (CheckInvalidLoans(records))
                    return false;

                RI.PutText(row, 3, "X", Key.Enter);
                if (!TSX0RProcessing(records, rowSkip, ref recursiveRowSkip))
                    return false;
                else //need to recalculate the row we're on because it could have changed
                {
                    //we use 10 as the base because we're going to increment row again
                    row = 10 + recursiveRowSkip;
                }

                if (RI.ScreenCode != "TSX0S")
                {
                    //recursiveRowSkip++;
                    RI.Hit(Key.F12);
                    RI.Hit(Key.F5);
                }

                if (RI.CheckForText(row, 3, "X"))
                    RI.PutText(row, 3, " ");
            }
            RI.Hit(Key.F5);
            return true;
        }

        public bool TSX0RProcessing(List<ScraRecord> records, int rowSkip, ref int recursiveRowSkip)
        {
            //List<ScraRecord> loansToDisclose = new List<ScraRecord>();
            string scheduleType = "";
            List<bool> results = new List<bool>();
            for (int row = 10; RI.MessageCode != "90007"; row++)
            {
                if (row > 23 || RI.CheckForText(row, 3, " "))
                {
                    row = 9;
                    RI.Hit(Key.F8);
                    continue;
                }

                if (CheckInvalidLoans(records))
                    return false;

                if (RI.GetText(row, 3, 1) == "X")
                {
                    scheduleType = GetScheduleType(row);
                    DateTime disbDate = RI.GetText(row, 5, 10).ToDate();
                    string sessionLoanProgram = RI.GetText(5, 15, 8).Replace(" ", "");
                    string sessionPrincipal = RI.GetText(row, 16, 10);
                    if (MultipleRepaymentStartDates(records, scheduleType))
                        return false;

                    results.Add(AddLoansToDisclosureList(records, disbDate, sessionLoanProgram, sessionPrincipal));
                }
            }

            if (!records.Any(p => p.FoundLoanToDisClose) || results.All(p => !p))
            {
                recursiveRowSkip++;
                RI.Hit(Key.F12);
                //RI.FastPath(string.Format("TX3Z/ATS0N{0}", records.FirstOrDefault().BorrowerSSN));
                return DetermineScreenAndProcess(records, rowSkip, ref recursiveRowSkip);
            }

            RI.Hit(Key.Enter);
            if (RI.ScreenCode == "TSX0T")
            {
                if (!TSX0TProcessing(records.Where(p => p.FoundLoanToDisClose && !p.TSX0TUpdatedAt.HasValue).ToList(), scheduleType))
                    return false;
            }
            else
            {
                records.FirstOrDefault().SessionErrorMessage = RI.Message;
                return false;
            }

            if (RI.ScreenCode != "TSX0R")
            {
                recursiveRowSkip++;
                RI.Hit(Key.F12);
                RI.Hit(Key.F5);
            }

            return true;
        }

        public static bool AddLoansToDisclosureList(List<ScraRecord> records, DateTime disbDate, string sessionLoanProgram, string sessionPrincipal)
        {
            bool foundLoans = false;
            List<ScraRecord> loans = new List<ScraRecord>();
            foreach (ScraRecord record in records)
            {
                string LoanProgramAlt = "";

                if (record.LoanProgram.IsIn("DLUNST", "DLSTFD"))
                    LoanProgramAlt = "S/DLUNS";
                else if (record.LoanProgram.IsIn("DLUCNS", "DLSCNS"))
                    LoanProgramAlt = "S/DLUCN";
                else if (record.LoanProgram.IsIn("UNSTFD", "STFFRD"))
                    LoanProgramAlt = "S/UNSTF";
                else if (record.LoanProgram.IsIn("SUBSPC", "UNSPC"))
                    LoanProgramAlt = "S/UNSPC";
                else if (record.LoanProgram.IsIn("SUBCNS", "UNCNS"))
                    LoanProgramAlt = "S/UNCNS";

                if(record.LoanDisbursementDate == disbDate && (record.LoanProgram == sessionLoanProgram || LoanProgramAlt == sessionLoanProgram) && record.CurrentPrincipal == sessionPrincipal.ToDecimal())
                {
                    record.FoundLoanToDisClose = true;
                    foundLoans = true;
                }
            }

            //if (records.Any((p => p.LoanDisbursementDate == disbDate && (p.LoanProgram == sessionLoanProgram || LoanProgramAlt == sessionLoanProgram) && p.CurrentPrincipal == sessionPrincipal.ToDecimal())))
            //{
            //    records.Where(p => p.LoanDisbursementDate == disbDate && (p.LoanProgram == sessionLoanProgram || LoanProgramAlt == sessionLoanProgram) && p.CurrentPrincipal == sessionPrincipal.ToDecimal()).ToList().ForEach(p => p.FoundLoanToDisClose = true);
            //    foundLoans = true;
            //}
            //else if (records.Any((p => p.LoanDisbursementDate == disbDate && p.CurrentPrincipal.ToString() == sessionPrincipal)))
            //{
            //    records.Where(p => p.LoanDisbursementDate == disbDate && p.CurrentPrincipal.ToString() == sessionPrincipal).ToList().ForEach(p => p.FoundLoanToDisClose = true);
            //    foundLoans = true;
            //}

            return foundLoans;
        }

        public bool MultipleRepaymentStartDates(List<ScraRecord> records, string scheduleType)
        {
            if (scheduleType.IsIn("PG", "PL") && records.DistinctBy(p => p.RepayStart).Count() != 1)
            {
                records.ForEach(p => p.SessionErrorMessage = "Pre HERA Borrower has multiple repayment start dates.  Please redisclose manually.");
                return true;
            }
            return false;
        }

        public string GetScheduleType(int row)
        {
            string scheduleType = RI.GetText(row, 48, 3);
            if (scheduleType.IsNullOrEmpty() || scheduleType == "___")
                scheduleType = "L";
            return scheduleType;
        }

        public bool CheckInvalidLoans(List<ScraRecord> records)
        {
            if (RI.MessageCode == "03459") //invalid loans
            {
                records.FirstOrDefault().SessionErrorMessage = RI.Message;
                return true;
            }
            return false;
        }

        public bool TSX0TProcessing(List<ScraRecord> records, string scheduleType)
        {
            string loanLevelMaxTerm = SetLoanTerm(records, scheduleType);
            return ApplySchedule(records, loanLevelMaxTerm, scheduleType);
        }

        public string SetLoanTerm(List<ScraRecord> records, string scheduleType)
        {
            string loanLevelMaxTerm = "";
            if (records.Any())
            {
                if (scheduleType.IsIn("PG", "PL"))
                {
                    if (records.FirstOrDefault().BalAtRepay < 10000.00)
                        loanLevelMaxTerm = "144";
                    else if (records.FirstOrDefault().BalAtRepay < 20000.00)
                        loanLevelMaxTerm = "180";
                    else if (records.FirstOrDefault().BalAtRepay < 40000.00)
                        loanLevelMaxTerm = "240";
                    else if (records.FirstOrDefault().BalAtRepay < 60000.00)
                        loanLevelMaxTerm = "300";
                    else if (records.FirstOrDefault().BalAtRepay >= 60000.00)
                        loanLevelMaxTerm = "360";
                }
                else if (scheduleType.IsIn("EL", "EG"))
                    loanLevelMaxTerm = RI.GetText(21, 26, 3); //extended term always
                else
                    loanLevelMaxTerm = RI.GetText(20, 26, 3); //S2 S5 and all other plans do normal term
            }
            return loanLevelMaxTerm;
        }

        public bool ApplySchedule(List<ScraRecord> records, string loanLevelMaxTerm, string scheduleType)
        {
            if (loanLevelMaxTerm.IsNullOrEmpty())
                return false;

            RI.PutText(9, 23, loanLevelMaxTerm, true);
            RI.PutText(8, 14, scheduleType, Key.Enter, true);
            if (RI.MessageCode == "02077") //first pay due falls within deferment or forbearance
                return true; //keep doing other loans
            else if (RI.MessageCode.IsIn("06701", "02556", "02900", "02553")) //insufficient terms, term remaining less than required, invalid schedule type, installment exceeds max grad
            {
                RI.Hit(Key.F12);
                RI.Hit(Key.Enter);
                if (scheduleType == "EG")
                    scheduleType = "EL";
                else if (scheduleType == "PG")
                    scheduleType = "PL";
                else
                    scheduleType = "L";

                RI.PutText(8, 14, scheduleType, Key.Enter, true);
            }

            if (RI.MessageCode.IsIn("01840", "02074", "06579", "02073"))
            {
                RI.Hit(Key.F4);
                RI.Hit(Key.F4);
                if (RI.MessageCode != "01832")
                {
                    records.ForEach(p => p.SessionErrorMessage = string.Format("SCRA RPS Error for borrower {0}. Session Message: {1}", records.FirstOrDefault().AccountNumber, RI.Message));
                    return false;
                }
            }
            else
            {
                records.ForEach(p => p.SessionErrorMessage = string.Format("SCRA RPS Error for borrower {0}. Session Message: {1}", records.FirstOrDefault().AccountNumber, RI.Message));
                return false;
            }

            foreach (ScraRecord record in records.Where(p => p.FoundLoanToDisClose))
            {
                record.TSX0TUpdatedAt = DateTime.Now; //Set this so that later runs know that it is updated.
                DA.SetScheduleCompleted(record, true);
            }
            return true;
        }
    }
}
