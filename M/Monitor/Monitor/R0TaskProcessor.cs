using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.Scripts;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DocumentProcessing;
using System.IO;

namespace Monitor
{
    class R0TaskProcessor
    {
        string utId;
        R0Task task;
        StandardArgs args;
        const string MONTR = "MONTR";
        public R0TaskProcessor(StandardArgs args, string utId, R0Task task)
        {
            this.utId = utId;
            this.task = task;
            this.args = args;
        }

        public TaskEoj ProcessTask()
        {
            var eoj = new TaskEoj();
            eoj.Task = task;
            eoj.R0CreateDate = task.DateRequested;
            if (PassesExemptionChecks(eoj))
            {
                var borrBalance = args.DA.GetBorrowerTotalBalance(task.Ssn);
                if (borrBalance > 30000)
                {
                    args.RI.FastPath("tx3z/CTS7C" + task.Ssn);
                    Action procTsx7d = new Action(() =>
                    {
                        args.RI.PutText(14, 48, "Y");
                        args.RI.Hit(ReflectionInterface.Key.Enter);
                    });
                    if (args.RI.ScreenCode == "TSX3S")
                    {
                        var settings = PageHelper.IterationSettings.Default();
                        settings.MinRow = 7;
                        PageHelper.Iterate(args.RI, (row, s) =>
                        {
                            var sel = args.RI.GetText(row, 3, 2);
                            if (sel.IsNumeric())
                            {
                                args.RI.PutText(22, 19, sel.PadLeft(2, '0'), ReflectionInterface.Key.Enter);
                                procTsx7d();
                                args.RI.Hit(ReflectionInterface.Key.F12); //return to list of loans
                            }
                        }, settings);
                    }
                    else
                        procTsx7d();
                }
                try
                {
                    if (eoj.CreateDate10 == null)
                        BaseRedisclosure(eoj);
                    else
                    {
                        if (eoj.CreateDate10 > (DateTime.Now.AddDays(-60)))
                        {
                            eoj.CancelReason = string.Format("Skipping borrower {0} for exemption reason: CreateDate10 within last 60 days.", task.AccountNumber);
                            args.PLR.AddNotification(eoj.CancelReason, NotificationType.EndOfJob, NotificationSeverityType.Informational);
                            eoj.EojType = EojReport.ExemptConditionSkipped;
                        }
                        else
                            ForceRedisclosure(eoj);
                    }
                }
                catch (EncounteredCancelCodeException ecce)
                {
                    eoj.EojType = EojReport.Cancelled;
                    eoj.CancelReason = ecce.Message;
                }
                catch (EncounteredForwardCodeException efce)
                {
                    eoj.EojType = EojReport.ForwardedSkipped;
                    eoj.CancelReason = efce.Message;
                }
            }
            return eoj;
        }

        /// <summary>
        /// Processes Exemption Info, populates the EOJ object with results and CreateDate10.  Returns true if the borrower passed, false if not.
        /// </summary>
        private bool PassesExemptionChecks(TaskEoj current)
        {
            var exemptionInfo = args.DA.GetRedisclosureExemptions(task.Ssn, task.DateRequested);
            if (exemptionInfo == null)
            {
                current.EojType = EojReport.Cancelled;
                current.CancelReason = "Borrower not found on system.";
                return false;
            }
            current.CreateDate10 = exemptionInfo.CreateDate10;
            List<string> reasons = new List<string>();
            #region CancelReasons
            if (exemptionInfo.AllExemptScheduleTypes)
                reasons.Add("All Exempt Schedule Types");
            if (exemptionInfo.RedisclosedAfterR0Date)
                reasons.Add("Redisclosed after R0 Date");
            if (exemptionInfo.HasAllLoansDeconverted)
                reasons.Add("All Loans Deconverted");
            if (exemptionInfo.HasAllLoansPaidInFull)
                reasons.Add("All Loans Paid In Full");
            if (reasons.Any())
            {
                current.EojType = EojReport.Cancelled;
                current.CancelReason = string.Join(". ", reasons);
                return false;
            }
            #endregion
            #region Skip Without Arc
            if (exemptionInfo.IsInExemptForbType)
                reasons.Add("In Exempt Forb Type");
            if (exemptionInfo.HasAllLoansInExemptStatus)
                reasons.Add("All Loans Exempt");
            if (exemptionInfo.HasUndisclosedSetupArc)
                reasons.Add("Has Undisclosed Setup ARC");
            if (exemptionInfo.HasUndisclosedRepayOptionsArc)
                reasons.Add("Has Unclosed Repay Options ARC");
            if (!args.ValidReasons.Any(o => o.Reason == task.MonitorReason))
                reasons.Add("No Valid Monitor Reasons");
            if (reasons.Any())
            {
                current.EojType = EojReport.ExemptConditionSkipped;
                current.CancelReason = string.Join(". ", reasons);
                return false;
            }
            #endregion
            #region Arc Skips
            if (exemptionInfo.HasAllLoansInLitigation)
            {
                current.CancelReason = "Monitor unable to redisclose, borrower is in litigation";
                AddArc("MONTR", current.CancelReason);
                current.EojType = EojReport.ForwardedSkipped;
                return false;
            }
            else if (exemptionInfo.IsInExemptScheduleType && exemptionInfo.IsInNonExemptScheduleType)
            {
                current.CancelReason = "Monitor unable to redisclose, borrower is on mixed exempt and non exempt schedule types";
                AddArc("MONTR", current.CancelReason);
                current.EojType = EojReport.ForwardedSkipped;
                return false;
            }
            else if (exemptionInfo.IsInFixedAlternativeScheduleType)
            {
                current.CancelReason = "Monitor unable to redisclose, borrower is on alternative fixed schedule";
                AddArc("MONTR", current.CancelReason);
                current.EojType = EojReport.ForwardedSkipped;
                return false;
            }
            #endregion  
            return true;
        }

        private MonthlyInfo GetMonthlyPaymentInfo()
        {
            var info = new MonthlyInfo();
            args.RI.FastPath("tx3z/ITS26" + task.Ssn);
            if (args.RI.ScreenCode == "TSX29")
            {
                info.OldMonthlyPayment = args.RI.GetText(19, 43, 9).ToDecimal();
                info.BeginningBalance = args.RI.GetText(11, 36, 10).ToDecimal();
                info.MultipleRepaymentStartDates = false;
            }
            else
            {
                DateTime? last = null;
                args.RI.FastPath("tx3z/ITS26" + task.Ssn);
                PageHelper.Iterate(args.RI, (r, s) =>
                {
                    string sel = args.RI.GetText(r, 2, 2);
                    if (sel.IsNumeric()) //valid row
                    {
                        args.RI.PutText(21, 12, sel.PadLeft(2, '0'), ReflectionInterface.Key.Enter);
                        info.OldMonthlyPayment += args.RI.GetText(19, 43, 9).ToDecimal();
                        info.BeginningBalance += args.RI.GetText(11, 36, 10).ToDecimal();
                        DateTime? date = args.RI.GetText(17, 44, 8).ToDateNullable() ?? args.RI.GetText(6, 18, 8).ToDateNullable();
                        if (last == null)
                            last = date;
                        if (date != last)
                            info.MultipleRepaymentStartDates = true;
                        args.RI.Hit(ReflectionInterface.Key.F12); //go back to list
                        return;
                    }
                });
            }
            return info;
        }

        private void BaseRedisclosure(TaskEoj eoj)
        {
            var calculation = CalculateAts0n(false);
            if (!calculation.ReturnResult)
            {
                eoj.EojType = calculation.EojType;
                eoj.CancelReason = calculation.Reason;
            }
            else
            {
                lock (args.MS) //lock the settings, we'll be incrementing and checking values during this bit
                {
                    if (calculation.NewMonthlyPayment >= calculation.OldMonthlyPaymentInfo.OldMonthlyPayment + args.MS.MaxIncrease)
                    {
                        if (args.MS.TotalPreNoteCounter < args.MS.MaxPreNote)
                        {
                            foreach (string arc in new string[] { "OVRPS", "OVRLR" })
                            {
                                string comment = "Monitor Redisclosure is above threshold.  Increasing from {0} to {1} for loans.";
                                comment = string.Format(comment, calculation.OldMonthlyPaymentInfo.OldMonthlyPayment, calculation.NewMonthlyPayment);
                                AddArc(arc, comment);
                            }
                            SendLetters(calculation);
                            args.MS.PrenoteAdded(task.Ssn);
                            eoj.EojType = EojReport.PaymentsTooHighPrenotifications;
                            eoj.CancelReason = "Max Prenote";
                        }
                        else
                        {
                            eoj.EojType = EojReport.MaxLimitSkipped;
                            eoj.CancelReason = "Max Prenote Skipped";
                        }
                        eoj.OldMonthlyPayment = calculation.OldMonthlyPaymentInfo.OldMonthlyPayment;
                        eoj.NewMonthlyPayment = calculation.NewMonthlyPayment;
                    }
                    else
                    {
                        calculation = CalculateAts0n(true);
                        if (!calculation.ReturnResult)
                        {
                            eoj.EojType = calculation.EojType;
                            eoj.CancelReason = calculation.Reason;
                        }
                        else
                        {
                            args.MS.PrenoteAdded(task.Ssn);
                            args.PLR.AddNotification(
                                string.Format("Account: {0}, Old Payment: {1}, New Payment: {2}", task.AccountNumber, calculation.OldMonthlyPaymentInfo.OldMonthlyPayment, calculation.NewMonthlyPayment),
                                NotificationType.EndOfJob, NotificationSeverityType.Informational);
                            eoj.EojType = EojReport.Redisclosed;
                            eoj.OldMonthlyPayment = calculation.OldMonthlyPaymentInfo.OldMonthlyPayment;
                            eoj.NewMonthlyPayment = calculation.NewMonthlyPayment;
                            eoj.ForceDisclosure = false;
                        }
                    }
                }
            }
        }

        private void ForceRedisclosure(TaskEoj eoj)
        {
            lock (args.MS)
                if (args.MS.TotalForceCounter >= args.MS.MaxForce)
                {
                    eoj.EojType = EojReport.MaxLimitSkipped;
                    eoj.CancelReason = "Max Forced";
                    return;
                }
            var calculation = CalculateAts0n(true);
            if (!calculation.ReturnResult)
            {
                eoj.EojType = EojReport.Cancelled;
                eoj.CancelReason = calculation.Reason;
                return;
            }
            args.MS.ForceAdded(task.Ssn);
            args.PLR.AddNotification(
                string.Format("Account: {0}, Old Payment: {1}, New Payment: {2}", task.AccountNumber, calculation.OldMonthlyPaymentInfo.OldMonthlyPayment, calculation.NewMonthlyPayment),
                NotificationType.EndOfJob, NotificationSeverityType.Informational);
            eoj.EojType = EojReport.Redisclosed;
            eoj.OldMonthlyPayment = calculation.OldMonthlyPaymentInfo.OldMonthlyPayment;
            eoj.NewMonthlyPayment = calculation.NewMonthlyPayment;
            eoj.ForceDisclosure = true;
        }


        private bool AddArc(string arc, string comment)
        {
            ArcData ad = new ArcData(DataAccessHelper.Region.CornerStone);
            ad.AccountNumber = task.AccountNumber;
            ad.Arc = arc;
            ad.Comment = comment;
            ad.ArcTypeSelected = ArcData.ArcType.Atd22AllLoans;
            ad.ScriptId = "MONITOR";
            var results = ad.AddArc();
            if (!results.ArcAdded)
            {
                args.PLR.AddNotification("Error adding " + arc + " arc: " + string.Join(Environment.NewLine, results.Errors), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return false;
            }
            return true;
        }

        private int CalculateTsx0sGroups()
        {
            int count = 0;
            PageHelper.Iterate(args.RI, row =>
            {
                if (args.RI.CheckForText(row, 3, "_"))
                    count++;
            });
            return count;
        }

        private string GetTerm(decimal balance)
        {
            string term = "";
            if (balance < 10000)
                term = "144";
            else if (balance < 20000)
                term = "180";
            else if (balance < 40000)
                term = "240";
            else if (balance < 60000)
                term = "300";
            else
                term = "360";
            return term;
        }

        private class Ats0nCalculation
        {
            public MonthlyInfo OldMonthlyPaymentInfo { get; set; }
            public decimal NewMonthlyPayment { get; set; }
            public Dictionary<string, decimal> CoMakerNewMonthlyPayments { get; set; } = new Dictionary<string, decimal>();
            public bool ReturnResult { get; set; }
            public string Reason { get; set; }
            public EojReport EojType { get; set; }
        }

        private Ats0nCalculation CalculateAts0n(bool rediscloseWhileGathering)
        {
            List<decimal> payments = new List<decimal>();
            var calculation = new Ats0nCalculation();
            calculation.OldMonthlyPaymentInfo = GetMonthlyPaymentInfo();
            var crawler = new Ats0nCrawler(args.RI, task);
            Ats0nCrawler.Tsx0tResult schedResult = null;
            while ((schedResult = crawler.MoveToNextTsx0t()).SchedType != null)
            {
                if (schedResult.SchedType == "PL" || schedResult.SchedType == "PG")
                {
                    if (calculation.OldMonthlyPaymentInfo.MultipleRepaymentStartDates)
                    {
                        calculation.Reason = "Pre HERA Borr has mult repay start.  Please redisclose manually";
                        AddArc(MONTR, calculation.Reason);
                        calculation.ReturnResult = false;
                        calculation.EojType = EojReport.ForwardedSkipped;
                        return calculation;
                    }
                    if (args.RI.CheckForText(9, 23, "_"))
                        args.RI.PutText(9, 23, GetTerm(calculation.OldMonthlyPaymentInfo.BeginningBalance));
                }
                else
                {
                    int maxTermRemaining = args.RI.GetText(20, 26, 3).ToIntNullable() ?? 0;
                    int extendedTermRemaining = args.RI.GetText(21, 26, 3).ToIntNullable() ?? 0;
                    var highest = Math.Max(maxTermRemaining, extendedTermRemaining);
                    if (args.RI.CheckForText(9, 23, "_"))
                    {
                        if (schedResult.SchedType.IsIn("EL", "EG", "S2", "S5"))
                            args.RI.PutText(9, 23, highest.ToString());
                        else
                            args.RI.PutText(9, 23, maxTermRemaining.ToString());
                    }
                }
                if (args.RI.CheckForText(8, 14, "_"))
                    args.RI.PutText(8, 14, schedResult.SchedType, ReflectionInterface.Key.Enter);
                if (args.RI.ScreenCode != "TSX0T")
                {
                    AddArc("MONTR", "Monitor Error " + args.RI.Message);
                    throw new EncounteredForwardCodeException(args.RI.ScreenCode + ":" + args.RI.Message);
                }
                if (args.RI.MessageCode == "02077")
                {
                    //don't include this loan in the calculations
                    continue;
                }
                if (args.RI.MessageCode.IsIn("06701", "02556", "02900"))
                {
                    args.RI.Hit(ReflectionInterface.Key.F12);
                    args.RI.Hit(ReflectionInterface.Key.Enter);
                    args.RI.PutText(8, 14, "L");
                    args.RI.Hit(ReflectionInterface.Key.Enter);
                }
                if (args.RI.MessageCode.IsIn("01840", "02074", "06579", "02073"))
                {
                    string comakerSsn = args.RI.GetText(7, 12, 9);

                    args.RI.Hit(ReflectionInterface.Key.F10);
                    decimal installmentAmount = args.RI.GetText(9, 49, 12).ToDecimal();
                    payments.Add(installmentAmount);
                    calculation.NewMonthlyPayment += installmentAmount;
                    if (!string.IsNullOrWhiteSpace(comakerSsn))
                    {
                        if (!calculation.CoMakerNewMonthlyPayments.ContainsKey(comakerSsn))
                            calculation.CoMakerNewMonthlyPayments.Add(comakerSsn, 0);
                        calculation.CoMakerNewMonthlyPayments[comakerSsn] += installmentAmount;
                    }
                    if (rediscloseWhileGathering)
                    {
                        args.RI.Hit(ReflectionInterface.Key.F12);
                        args.RI.Hit(ReflectionInterface.Key.F4);
                        args.RI.Hit(ReflectionInterface.Key.F4);
                        if (args.RI.MessageCode != "01832") //repayment schedule successfully added
                        {
                            calculation.Reason = "Monitor Error: " + args.RI.Message + " - " + args.RI.GetText(1, 4, 5) + " " + args.RI.ScreenCode;
                            AddArc(MONTR, calculation.Reason);
                            calculation.ReturnResult = false;
                            calculation.EojType = EojReport.Cancelled;
                            return calculation;
                        }
                    }
                }
                else
                {
                    calculation.Reason = "Monitor Error: " + args.RI.Message + " - " + args.RI.GetText(1, 4, 5) + " " + args.RI.ScreenCode;
                    AddArc(MONTR, calculation.Reason);
                    calculation.ReturnResult = false;
                    calculation.EojType = EojReport.ForwardedSkipped;
                    return calculation;
                }
            }
            if (schedResult.HasError)
            {
                calculation.Reason = "Monitor Error: " + schedResult.ErrorMessage;
                AddArc(MONTR, calculation.Reason);
                calculation.ReturnResult = false;
                calculation.EojType = EojReport.ForwardedSkipped;
                return calculation;
            }
            calculation.ReturnResult = true;
            return calculation;
        }

        private void SendLetters(Ats0nCalculation calculation)
        {
#if !DEBUG
            var borrowerDemos = args.RI.GetDemographicsFromTx1j(task.AccountNumber);
            if (string.IsNullOrWhiteSpace(borrowerDemos.State.Trim('_')))
                borrowerDemos.State = borrowerDemos.ForeignState ?? "";
            string letterId = "PMTMONFED";
            var costCenter = args.DA.GetCostCenterForLetter(letterId);
            Dictionary<SystemBorrowerDemographics, decimal> lettersToGenerate = new Dictionary<SystemBorrowerDemographics, decimal>();
            lettersToGenerate.Add(borrowerDemos, calculation.NewMonthlyPayment);
            foreach (var comakerSsn in calculation.CoMakerNewMonthlyPayments.Keys)
            {
                var coDemos = args.RI.GetDemographicsFromTx1j(comakerSsn);
                lettersToGenerate.Add(coDemos, calculation.CoMakerNewMonthlyPayments[comakerSsn]);
            }
            foreach (var letter in lettersToGenerate)
            {
                var demos = letter.Key;
                var amount = letter.Value;
                string keyline = DocumentProcessing.ACSKeyLine(task.Ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
                var lineValues = new object[] { task.AccountNumber, keyline, '"' + amount.ToMoney() + '"', DateTime.Now.AddDays(60).ToString("MM/dd/yyyy"), demos.State, demos.FirstName, demos.LastName, '"' + demos.Address1 + '"', '"' + demos.Address2 + '"', demos.City, demos.ZipCode, demos.Country };
                string line = CsvHelper.SimpleEncode(lineValues);
                int? result = null;
                if (demos.AccountNumber == borrowerDemos.AccountNumber)
                    result = EcorrProcessing.AddRecordToPrintProcessing(Program.ScriptId, letterId, line, demos.AccountNumber, costCenter, DataAccessHelper.Region.CornerStone);
                else
                    result = EcorrProcessing.AddCoBwrRecordToPrintProcessing(Program.ScriptId, letterId, line, demos.AccountNumber, costCenter, borrowerDemos.Ssn, DataAccessHelper.Region.CornerStone);
                if (result.HasValue)
                {
                    string info = $"Data sent to Ecorr Centralized Printing and Imaging: Acct {demos.AccountNumber}; Letter {letterId}";
                    args.PLR.AddNotification(info, NotificationType.EndOfJob, NotificationSeverityType.Informational);
                }
                else
                {
                    args.PLR.AddNotification($"Unable to process letter {letterId} for account {demos.AccountNumber}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                }
            }
#endif
        }
    }
}
