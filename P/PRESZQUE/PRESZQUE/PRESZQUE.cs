using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using System.Windows.Forms;
using Uheaa.Common.ProcessLogger;

namespace PRESZQUE
{
    public class PRESZQUE : ScriptBase
    {
        public PRESZQUE(ReflectionInterface ri) : base(ri, "PRESZQUE", DataAccessHelper.Region.Uheaa) { }
        public override void Main()
        {
            while (WorkNextItemInQueue()) ;

        }

        HashSet<string> processed = new HashSet<string>();
        private bool WorkNextItemInQueue()
        {
            LoanInfo info = null;
            RI.FastPath("TX3Z/ITX6XSZ;01");
            PageHelper.Iterate(RI, (row, s) =>
            {
                var sel = RI.GetText(row, 2, 3).ToIntNullable();
                if (!sel.HasValue)
                    return; //not a real row
                string ssn = RI.GetText(row, 6, 9);
                string seq = RI.GetText(row, 15, 4);
                if (processed.Contains(ssn + seq))
                    return; //go to next result
                processed.Add(ssn + seq);
                info = new LoanInfo();
                info.Ssn = ssn;
                info.Seq = seq;
                info.RequestedDate = RI.GetText(row, 47, 10).ToDate();
                s.ContinueIterating = false;
            });
            if (info != null)
            {
                GetLoanType(info);
                CalculateEffectivePriorToConversion(info);
                CalculateInterestRatePriorToConversionChanged(info);
                CalculateOutOfSchoolDateChanged(info);
                info.CloseReason = GetCloseReason(info);
                if (info.CloseReason != null)
                    CloseTask(info);

                return true;
            }
            return false;
        }

        private void GetLoanType(LoanInfo info)
        {
            RI.FastPath("TX3Z/ITS26" + info.Ssn);
            if (RI.ScreenCode == "TSX28")
            {
                PageHelper.Iterate(RI, (row, s) =>
                {
                    if (RI.CheckForText(row, 14, info.Seq))
                    {
                        RI.PutText(21, 12, GetText(row, 2, 2), ReflectionInterface.Key.Enter);
                        s.ContinueIterating = false;
                    }
                });
            }

            if (RI.ScreenCode == "TSX29")
            {
                info.AccountNumber = RI.GetText(4, 40, 12).Replace(" ", "");
                var dateNotified = RI.GetText(21, 42, 8).ToDateNullable();
                string sepDate = RI.GetText(18, 17, 8);
                info.LoanType = RI.GetText(6, 66, 6).Trim();
                info.IsFixedRate = RI.CheckForText(11, 69, "FIXED RATE");
                RI.Hit(ReflectionInterface.Key.F6);
                RI.Hit(ReflectionInterface.Key.Enter);
                string lenderCode = RI.GetText(5, 59, 6);
                PageHelper.Iterate(RI, (row, s) =>
                {
                    if (RI.CheckForText(row, 33, "0290A") || RI.CheckForText(row, 33, "0291A"))
                        info.EffectiveDate = RI.GetText(row, 12, 8).ToDate();
                });
                info.IsRehabbedLoan = lenderCode == "828476" && info.EffectiveDate != DateTime.Parse("05/16/13");
                if (dateNotified == info.RequestedDate)
                    if (sepDate.ToDate() < info.EffectiveDate)
                        info.OutOfSchoolDateChanged = true;
            }
        }

        private void CalculateEffectivePriorToConversion(LoanInfo info)
        {
            RI.FastPath("tx3z/ITSAY" + info.Ssn);
            if (RI.MessageCode == "01020")
                return; //no deferments or forbearances, continue
            if (RI.ScreenCode == "TSXAZ")
                PageHelper.Iterate(RI, (row, s) =>
                {
                    if (RI.CheckForText(row, 14, info.Seq.Substring(1))) //this is a 3-char loan seq field
                    {
                        string sel = RI.GetText(row, 2, 2);
                        RI.PutText(22, 17, sel, ReflectionInterface.Key.Enter);
                        s.ContinueIterating = false;

                    }
                });

            PageHelper.Iterate(RI, (innerRow, settings) =>
            {
                var appliedDate = RI.GetText(innerRow, 72, 8).ToDateNullable();
                if (appliedDate == info.EffectiveDate)
                {
                    if (RI.GetText(innerRow, 30, 8).ToDate() < info.EffectiveDate || RI.GetText(innerRow, 40, 8).ToDate() < info.EffectiveDate)
                        info.AddedEffectivePriorToConversion = true;
                }
                else if (appliedDate == info.RequestedDate)
                {
                    var typ = RI.GetText(innerRow, 25, 1);
                    if (typ == "D")
                        info.DefermentAdded = true;
                    if (typ == "F")
                        info.ForbearanceAdded = true;
                }
            });
        }

        private void CalculateInterestRatePriorToConversionChanged(LoanInfo info)
        {
            RI.FastPath("tx3z/ITS06" + info.Ssn);
            PageHelper.Iterate(RI, (row, s) =>
            {
                if (RI.CheckForText(row, 47, info.Seq.Substring(1)))
                {
                    RI.PutText(21, 18, RI.GetText(row, 3, 2), ReflectionInterface.Key.Enter);
                    s.ContinueIterating = false;
                }
            });

            PageHelper.Iterate(RI, (row, s) =>
            {
                if (RI.CheckForText(row, 14, "P") && RI.CheckForText(row, 46, "A"))
                {
                    info.InterestRatePriorToConversionChanged = true;
                    s.ContinueIterating = false;
                }
            });
        }

        private void CalculateOutOfSchoolDateChanged(LoanInfo info)
        {
            RI.FastPath("tx3z/ITS26" + info.Ssn);
            if (RI.ScreenCode == "TSX28")
                PageHelper.Iterate(RI, (row, s) =>
                {
                    if (RI.CheckForText(row, 14, info.Seq))
                    {
                        string sel = RI.GetText(row, 2, 2);
                        RI.PutText(21, 12, sel, ReflectionInterface.Key.Enter);
                        s.ContinueIterating = false;
                    }
                });
            var sepDate = RI.GetText(18, 17, 8).ToDateNullable();
            var notifDate = RI.GetText(21, 42, 8).ToDateNullable();
            if (notifDate == info.RequestedDate && sepDate < info.EffectiveDate)
                info.OutOfSchoolDateChanged = true;
        }

        private string GetCloseReason(LoanInfo info)
        {
            if (info.IsRehabbedLoan)
                return "Loan is in Rehab"; //rehabbed loans need no adjustments
            if (info.LoanType == "TILP" || info.LoanType == "COMPLT")
                return "Loan Type is TILP or COMPLT"; //no adjustment for these types
            if (info.InterestRatePriorToConversionChanged)
                return null;
            if (info.IsUnsubsidized && info.IsFixedRate)
            {
                if (info.DefermentAdded || info.ForbearanceAdded)
                    return "Unsubsidized, fixed rate deferment or forbearance added.";
                else if (info.OutOfSchoolDateChanged)
                    return "Out of school date changed.";
            }
            else if (info.IsUnsubsidized && !info.IsFixedRate)
            {
                if (info.DefermentAdded)
                    return null;
                else if (info.ForbearanceAdded)
                    return "Unsubsidized, variable rate forbearance added.";
                else if (info.OutOfSchoolDateChanged)
                    return null;
            }
            else if (info.IsSubsidized)
            {
                if (info.DefermentAdded)
                    return null;
                else if (info.ForbearanceAdded)
                    return "Subsidized forbearance added.";
                else if (info.OutOfSchoolDateChanged)
                    return null;
            }
            return null;
        }

        private void CloseTask(LoanInfo info)
        {
            RI.FastPath("tx3z/ITX6T" + info.Ssn);
            var settings = PageHelper.IterationSettings.Default();
            settings.MinRow = 7;
            PageHelper.Iterate(RI, (row, s) =>
            {
                if (RI.CheckForText(row, 40, info.Ssn + info.Seq) && RI.CheckForText(row, 8, "SZ"))
                {
                    string sel = RI.GetText(row, 2, 2);
                    RI.PutText(21, 18, sel, ReflectionInterface.Key.Enter);
                    s.ContinueIterating = false;
                }
            }, settings);

            RI.FastPath("tx3z/ITX6T" + info.Ssn);
            settings = PageHelper.IterationSettings.Default();
            settings.MinRow = 7;
            PageHelper.Iterate(RI, (row, s) =>
            {
                if (RI.CheckForText(row, 40, info.Ssn + info.Seq) && RI.CheckForText(row, 8, "SZ"))
                {
                    string sel = RI.GetText(row, 2, 2);
                    if (RI.CheckForText(row + 1, 76, "W"))
                    {
                        RI.PutText(21, 18, sel);
                        RI.Hit(ReflectionInterface.Key.F2);
                        RI.PutText(8, 19, "C");
                        RI.Hit(ReflectionInterface.Key.Enter);
                        s.ContinueIterating = false;
                        LeaveComment(info);
                    }
                }
            }, settings);
        }

        private void LeaveComment(LoanInfo info)
        {
            var data = new ArcData(DataAccessHelper.CurrentRegion);
            data.AccountNumber = info.AccountNumber;
            data.Arc = "ACCNT";
            data.Comment = "Closed SZ Queue task: " + info.CloseReason;
            data.LoanSequences = new List<int>(new int[] { int.Parse(info.Seq) });
            data.ArcTypeSelected = ArcData.ArcType.Atd22ByLoan;
            data.ScriptId = "PRESZQUE";
            data.RecipientId = info.Ssn;
            var result = data.AddArc();
            if (result.Errors.Any())
                PL.AddNotification(PL.LogData.ProcessLogId, string.Join(Environment.NewLine, result.Errors), NotificationType.ErrorReport, NotificationSeverityType.Critical);
        }
    }
}
