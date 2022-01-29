using System;
using System.Collections.Generic;
using System.Linq;
using Uheaa.Common;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;
namespace ACTDUTRPT
{
    public partial class ActiveDutyReportProcess
    {
        public const string DateFormat = "MMddyyyy";

        public void BenefitsProcessing(List<ActiveDutyRecord> recordsByBorrower)
        {
            foreach (ActiveDutyRecord record in recordsByBorrower.OrderBy(p => p.ActiveDutyReportingId))
            {
                if (!AddBenefits(record))
                {
                    ErrorProcessing(record, "NSCRA");
                }
            }
        }

        public bool AddBenefits(ActiveDutyRecord record)
        {
            if (!record.TXCXUpdatedAt.HasValue)
            {
                if (UpdateTXCX(record))
                {
                    record.TXCXUpdatedAt = DateTime.Now;
                    DA.SetProcessedTXCX(record, true);
                    return true;
                }
            }

            return false;
        }

        public bool UpdateTXCX(ActiveDutyRecord record)
        {
            RI.FastPath(string.Format("TX3Z/CTXCX{0}", record.ProcessingSSN));
            if (RI.ScreenCode != "TXXCW")
            {
                record.SessionErrorMessage = RI.Message;
                return false;
            }
            else
                return AddTXCX(record);
        }

        public bool AddTXCX(ActiveDutyRecord record)
        {
            RI.FastPath(string.Format("TX3Z/ATXCX{0}", record.ProcessingSSN));
            DateTime today = System.DateTime.Today.Date;

            if (record.TXCXBeginDate.Value.Date < defaultBeginDate && record.TXCXEndDate.Value.Date < defaultBeginDate)
            {
                record.SessionErrorMessage = "TXCX begin and end dates are both prior to 8/14/2008.  Please review.";
                return false;
            }

            if (record.TXCXBeginDate.Value.Date > today)
            {
                record.SessionErrorMessage = "TXCX begin cannot be in the future.  Please review.";
                return false;
            }

            DateTime beginDt = (record.TXCXBeginDate.Value.Date > defaultBeginDate.Date ? record.TXCXBeginDate.Value.Date : defaultBeginDate.Date);
            DateTime endDt = (record.TXCXEndDate.HasValue && record.TXCXEndDate.Value.Date != defaultEndDate.Date ? record.TXCXEndDate.Value.Date : oneYearFuture.Date);
            string beginDate = beginDt.ToString(DateFormat);
            string endDate = endDt.ToString(DateFormat);

            if (endDt.Date < beginDt.Date)
            {
                record.SessionErrorMessage = "TXCX end date cannot be before SCRA begin date (Defaults to 8/14/2008 for service prior to that date).  Please review Active duty reporting details.";
                return false;
            }

            if (beginDt.Date > today)
            {
                record.SessionErrorMessage = "TXCX begin date is in the future.  Please review.";
                return false;
            }

            if (RI.CheckForText(12, 7, " ")) //No records currently exist, insert
                return InsertTxcx(record, beginDate, endDate, System.DateTime.Today.ToString(DateFormat)) == "01004"; //Diagram 1, 13
            else //handles piecing rows together on the screen
                return PieceTogetherBenefits(record, beginDt, endDt);

        }

        private string InsertTxcx(ActiveDutyRecord record, string beginDate, string endDate, string notifyDate = null)
        {
            if (notifyDate.IsNullOrEmpty())
                notifyDate = defaultNotify;
            RI.FastPath(string.Format("TX3Z/ATXCX{0}", record.ProcessingSSN));
            RI.PutText(11, 7, beginDate, true);
            RI.PutText(11, 19, endDate, true);
            RI.PutText(11, 39, notifyDate, true);
            RI.PutText(11, 52, record.TXCXType, true);
            RI.PutText(11, 64, record.ServiceComponent, Key.Enter, true);
            if (RI.GetText(23, 3, 5) == "01004") //Message code in wrong place
                return RI.GetText(23, 3, 5);
            else
            {
                record.SessionErrorMessage = RI.Message;
                return RI.GetText(23, 3, 5);
            }
        }

        public bool PieceTogetherBenefits(ActiveDutyRecord record, DateTime? beginDt, DateTime? endDt)
        {
            DateTime? beginSession = null, endSession = null;
            bool noMoreData = false;
            DateTime beginDate = beginDt.Value.Date;
            DateTime endDate = endDt.Value.Date;
            DateTime today = System.DateTime.Today.Date;
            List<string> returnCodes = new List<string>();
            for (int row = 12; RI.GetText(23, 3, 5) != "90007"; row++)
            {
                if (row > 21)
                {
                    row = 10;
                    RI.Hit(Key.F8);
                    continue;
                }

                if (RI.CheckForText(row, 7, " "))
                    noMoreData = true;

                if (!noMoreData && RI.GetText(row, 52, 7).Trim() == record.TXCXType) //figure out benefit type, and look at only rows from the screen that have the same type
                {
                    beginSession = RI.GetText(row, 7, 10).Replace(" ", "/").ToDateNullable();
                    endSession = RI.GetText(row, 19, 10).Replace(" ", "/").ToDateNullable();

                    if (beginSession.Value.Date > today && beginSession.Value.Date > endDate)
                    {
                        row--;
                        RI.FastPath(string.Format("TX3Z/DTXCX{0}", record.ProcessingSSN));
                        RI.PutText(row, 4, "X", Key.Enter);
                        RI.Hit(Key.Enter);
                        returnCodes.Add(RI.GetText(23, 3, 5));
                        RI.FastPath(string.Format("TX3Z/ATXCX{0}", record.ProcessingSSN));
                        continue;
                    }

                    if (beginDate <= today && beginDate <= endDate) //Some amount of the dod period should be recorded
                    {
                        if (beginDate == beginSession.Value.Date && endDate <= endSession.Value.Date) //Diagram 2, 20
                            return true;
                        else if (endDate > endSession.Value.Date) //create a row for latest period
                        {
                            //Diagram 3, 5, 7, 9, 11, 14, 15, 16, 18
                            if (beginDate <= endSession)
                            {
                                if (endDate == endSession.Value.Date.AddDays(1) && beginSession.Value.Date == endDate)
                                {
                                    endDate = beginSession.Value.Date.AddDays(-1);
                                    continue;
                                }
                                returnCodes.Add(InsertTxcx(record, endSession.Value.Date.AddDays(1).ToString(DateFormat), endDate.ToString(DateFormat)));
                                endDate = endSession.Value.Date;
                                continue;
                            }
                            else
                            {
                                returnCodes.Add(InsertTxcx(record, beginDate.ToString(DateFormat), endDate.ToString(DateFormat), System.DateTime.Today.ToString(DateFormat)));
                            }
                        }
                        else if (endDate == endSession.Value.Date)
                        {
                            //Diagram 12, 19
                            endDate = beginSession.Value.Date.AddDays(-1);
                            continue;
                        }
                        else if (endDate < endSession.Value.Date)
                        {
                            if(beginSession.Value.Date < today && endSession.Value.Date > today)
                            {
                                if (endDate < beginSession.Value.Date)
                                    continue;
                                DateTime greaterDate = today;
                                if (endDate > greaterDate)
                                    greaterDate = endDate;
                                returnCodes.Add(UpdateExisting(record, beginSession.Value.Date.ToString(DateFormat), greaterDate.ToString(DateFormat), today.ToString(DateFormat)));
                                row--;
                                if(endDate > beginSession.Value.Date)
                                    endDate = beginSession.Value.Date.AddDays(-1);
                                continue;
                            }
                            else if (endDate < beginSession.Value.Date) //Diagram 4, 7
                                continue;
                            else if (endDate == beginSession.Value.Date)
                            {
                                endDate = beginSession.Value.Date.AddDays(-1);
                                continue;
                            }
                            else //insert new row endDate to endSession and update existing to begin session to endDate -1
                            {
                                if (endDate > today)
                                {
                                    //Diagram 17
                                    returnCodes.Add(UpdateExisting(record, beginSession.Value.Date.ToString(DateFormat), endDate.ToString(DateFormat)));
                                    row--;
                                    endDate = beginSession.Value.Date;
                                    continue;
                                }
                                else if (beginDate < beginSession.Value.Date && beginDate != endDate)
                                {
                                    //Diagram 8
                                    endDate = beginSession.Value.Date.AddDays(-1);
                                    continue;
                                }
                                else
                                {
                                    //Diagram 10
                                    endDate = beginSession.Value.Date;
                                    continue;
                                }
                            }
                        }
                    }
                    else if (beginDate > today)
                    {
                        record.SessionErrorMessage = "Cannot apply DOD dates as the begin date is in the future.";
                        return false;
                    }
                }

                if (!beginSession.HasValue || (RI.GetText(row, 52, 7).Trim() != record.TXCXType && RI.GetText(row, 52, 7).Trim() != "")) //New benefit type with no existing rows
                {
                    if (noMoreData)
                    {
                        returnCodes.Add(InsertTxcx(record, beginDate.ToString(DateFormat), endDate.ToString(DateFormat)));
                        break;
                    }
                    else
                        continue;
                }

                if (beginDate < beginSession.Value.Date) //catch earliest row if it is before session
                {
                    //Diagram 6, 9, 16
                    if (endDate < beginSession.Value.Date)
                    {
                        returnCodes.Add(InsertTxcx(record, beginDate.ToString(DateFormat), endDate.ToString(DateFormat)));
                        endDate = beginDate;
                    }
                    else
                    {
                        returnCodes.Add(InsertTxcx(record, beginDate.ToString(DateFormat), beginSession.Value.Date.AddDays(-1).ToString(DateFormat)));
                        endDate = beginDate;
                    }
                }
                else
                    break;

                if (returnCodes.Any(p => p.IsIn("01004", "01005", "01006")))
                    break;
            }

            if (returnCodes.All(p => p.IsIn("01004", "01005", "01006")) || returnCodes.Count == 0)
                return true;
            else
            {
                record.SessionErrorMessage = RI.Message;
                return false;
            }
        }

        private string UpdateExisting(ActiveDutyRecord record, string beginDate, string endDate, string notifyDate = null)
        {
            string returnCode = "";
            if (notifyDate.IsNullOrEmpty())
                notifyDate = defaultNotify;
            RI.FastPath(string.Format("TX3Z/CTXCX{0}", record.ProcessingSSN));

            for (int row = 11; RI.GetText(23, 3, 5) != "90007"; row++)
            {
                if (row > 21)
                {
                    row = 10;
                    RI.Hit(Key.F8);
                    continue;
                }

                if (RI.GetText(row, 52, 7).Trim() == record.TXCXType && (RI.GetText(row, 7, 19).Replace(" ", "") == endDate || RI.GetText(row, 7, 10).Replace(" ", "") == beginDate))
                {
                    RI.PutText(row, 7, beginDate, true);
                    RI.PutText(row, 19, endDate, true);
                    RI.PutText(row, 39, notifyDate, true);
                    RI.PutText(row, 52, record.TXCXType, true);
                    RI.PutText(row, 64, record.ServiceComponent, Key.Enter, true);
                    returnCode = RI.GetText(23, 3, 5);
                    RI.FastPath(string.Format("TX3Z/ATXCX{0}", record.ProcessingSSN));
                    return returnCode;
                }
            }
            return "Record not found to update";
        }
    }
}