using System;
using System.Collections.Generic;
using System.Linq;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using Uheaa.Common;
using static Uheaa.Common.Scripts.ReflectionInterface;

namespace BATCHESP
{
    public class DefermentForbearanceHelper
    {
        private ProcessLogRun PLR;
        private TaskHelper TH;
        private NonSelectionHelper NSH;
        private ReflectionInterface RI;
        private readonly string[] SchoolDefers = { "D15", "D18", "D45" };

        public DefermentForbearanceHelper(ProcessLogRun plr, ReflectionInterface ri, TaskHelper th, NonSelectionHelper nsh)
        {
            this.PLR = plr;
            this.TH = th;
            this.RI = ri;
            this.NSH = nsh;
        }

        /// <summary>
        /// Process a change to an existing deferment.
        /// </summary>
        public bool DefermentForbearanceChange(EspEnrollment esp, TsayDefermentForbearance df, List<Ts26LoanInformation> ts26s, List<Ts01Enrollment> ts01s, List<ParentPlusLoanDetailsInformation> pplus, DateTime? newBeginDate, DateTime? newEndDate)
        {
            RI.FastPath("tx3z/CTS0H" + esp.BorrowerSsn + ";;" + df.Type);
            if (RI.ScreenCode == "TSX0G")
            {
                bool found = false;
                var settings = PageHelper.IterationSettings.Default();
                settings.MinRow = 7;
                PageHelper.Iterate(RI, (row, s) =>
                {
                    DateTime? begin = RI.GetText(row, 24, 8).ToDateNullable();
                    DateTime? end = RI.GetText(row, 33, 8).ToDateNullable();
                    if (begin == df.BeginDate && end == df.EndDate && !RI.CheckForText(row, 58, "DENIED"))
                    {
                        string sel = RI.GetText(row, 2, 2);
                        RI.PutText(22, 13, sel, Key.Enter);
                        s.ContinueIterating = false;
                        found = true;
                    }
                }, settings);
                if (!found)
                {
                    RI.Hit(Key.F5);
                    settings = PageHelper.IterationSettings.Default();
                    settings.MinRow = 7;
                    PageHelper.Iterate(RI, (row, s) =>
                    {
                        DateTime? begin = RI.GetText(row, 24, 8).ToDateNullable();
                        if ((begin == df.BeginDate || begin == df.RequestedBeginDate) && !RI.CheckForText(row, 58, "DENIED")) // Check if date matches original request date (in case it has changed since then)
                        {
                            string sel = RI.GetText(row, 2, 2);
                            RI.PutText(22, 13, sel, Key.Enter);
                            s.ContinueIterating = false;
                            found = true;
                        }
                    }, settings);
                    if (!found)
                    {
                        return DefermentForbearanceAdd(esp, df.Type, df, ts26s, ts01s, pplus, newBeginDate ?? esp.EnrollmentBeginDate, newEndDate);
                    }
                }
            }

            bool result = DefermentForbearanceProcess(esp, df.Type, df, ts26s, ts01s, pplus, newBeginDate, newEndDate, null, DfMode.Change);

            return result;
        }

        /// <summary>
        /// Add a new deferment.
        /// </summary>
        public bool DefermentForbearanceAdd(EspEnrollment esp, string dfType, TsayDefermentForbearance df, List<Ts26LoanInformation> ts26s, List<Ts01Enrollment> ts01s, List<ParentPlusLoanDetailsInformation> pplus, DateTime? newBeginDate, DateTime? newEndDate)
        {
            RI.FastPath("tx3z/ATS0H" + esp.BorrowerSsn);
            string enrollmentStatus = null;
            if (dfType == null)
            {
                if (ts26s.SingleOrDefault().LoanProgramType.IsIn("PLUS", "DLPLUS"))
                {
                    if (esp.BorrowerSsn != esp.StudentSsn)
                        dfType = "D45";
                    else if (esp.Esp_Status.IsIn(StatusCode.OnLeave, StatusCode.FullTime, StatusCode.Enrolled))
                        dfType = "D15";
                    else if (esp.Esp_Status.IsIn(StatusCode.HalfTime, StatusCode.ThreeQuartersTime))
                        dfType = "D18";
                }
                else
                {
                    if (esp.Esp_Status.IsIn(StatusCode.OnLeave, StatusCode.FullTime, StatusCode.Enrolled))
                        dfType = "D15";
                    else if (esp.Esp_Status.IsIn(StatusCode.HalfTime, StatusCode.ThreeQuartersTime))
                        dfType = "D18";
                }
            }
            if (dfType == "D15")
                enrollmentStatus = "F";
            if (dfType == "D18")
                enrollmentStatus = "H";
            RI.PutText(9, 33, dfType.First().ToString());
            RI.PutText(11, 33, DateTime.Now.ToString("MMddyy"));
            RI.Hit(Key.Enter);
            string typeDigitsOnly = new string(dfType.Where(o => char.IsDigit(o)).ToArray());
            if (RI.ScreenCode == "TSX7E")
                RI.PutText(21, 13, typeDigitsOnly, true);
            else
                RI.PutText(22, 13, typeDigitsOnly, true);
            RI.Hit(Key.Enter);
            bool result = DefermentForbearanceProcess(esp, dfType, df, ts26s, ts01s, pplus, newBeginDate, newEndDate, enrollmentStatus, DfMode.Add);

            return result;
        }

        private enum DfMode { Add, Change }

        /// <summary>
        /// Internal use only, this is functionality used by DefermentForbearance Add/Change
        /// </summary>
        private bool DefermentForbearanceProcess(EspEnrollment esp, string dfType, TsayDefermentForbearance df, List<Ts26LoanInformation> ts26s, List<Ts01Enrollment> ts01s, List<ParentPlusLoanDetailsInformation> pplus, DateTime? newBeginDate, DateTime? newEndDate, string enrollmentStatus, DfMode mode, string schoolCode = null)
        {
            bool modeIsChangeAndSepDateIsFuture = mode == DfMode.Change && esp.Esp_SeparationDate > DateTime.Now.Date;
            bool updatingPplus = (esp.BorrowerSsn != esp.StudentSsn && pplus != null && pplus.Any()) ? true : false;
            bool anyBefore2008 = ts26s.Any(ts => ts.DisbursementDate < DateTime.Parse("07/01/2008"));
            bool isPostEnrollDefElig = pplus.Where(p => p.LoanSequence == ts26s.Select(o => o.LoanSequence).FirstOrDefault()).Select(q => q.PostEnrollmentDefermentEligible).SingleOrDefault();
            if (enrollmentStatus == null)
            {
                if (dfType == "D15")
                    enrollmentStatus = "F";
                if (dfType == "D18")
                    enrollmentStatus = "H";
            }

            if (RI.ScreenCode == "TSXA5")
            {
                var settings = PageHelper.IterationSettings.Default();
                settings.MaxRow = 21;
                PageHelper.Iterate(RI, (row, s) =>
                {
                    var seq = RI.GetText(row, 13, 3).ToIntNullable();
                    if (ts26s.Any(o => o.LoanSequence == seq) && RI.CheckForText(row, 2, "_"))
                        RI.PutText(row, 2, "X");
                }, settings);
                RI.Hit(Key.Enter);
            }
            string screenCode = RI.ScreenCode;
            if (screenCode == "TSX4A")
            {
                if (newBeginDate.HasValue)
                    RI.PutText(6, 18, newBeginDate.Value.ToString("MMddyy"));
                if (newEndDate.HasValue)
                    RI.PutText(7, 18, newEndDate.Value.ToString("MMddyy"));
                if (!string.IsNullOrWhiteSpace(RI.GetText(8, 18, 1)) && !modeIsChangeAndSepDateIsFuture)
                    RI.PutText(8, 18, esp.ArcRequestDate.Value.ToString("MMddyy"));
                RI.PutText(9, 18, esp.Esp_CertificationDate.Value.ToString("MMddyy"));

                if (RI.GetText(11, 22, 1) == "_")
                {
                    if (enrollmentStatus != null)
                        RI.PutText(11, 22, enrollmentStatus);
                }

                if (mode == DfMode.Add)
                {
                    RI.PutText(12, 15, schoolCode ?? esp.SchoolCode);

                    if (updatingPplus && dfType == "D45")
                        RI.PutText(10, 17, esp.StudentSsn); //STUDENT SSN field
                }
                if (dfType == "D45") //Different field locations for a D45 PPlus deferment
                {
                    RI.PutText(11, 31, "Y"); // ENROLLED AT LEAST HALF TIME field 
                    RI.PutText(14, 36, "Y"); // FORBEARANCE TO CLEAR DELINQUENCY field
                    RI.PutText(15, 36, "Y"); // CAPITALIZE INTEREST field

                    if (ts26s.Any(o => o.LoanProgramType.IsIn("PLUSGB", "DLPLGB")))
                    {
                        if (RI.CheckForText(13, 36, "Y", "N", "_"))
                            RI.PutText(13, 36, !anyBefore2008 ? "Y" : "N", true); // POST ENROLL DEFER REQUESTED field
                    }
                    else
                    {
                        if (RI.CheckForText(13, 36, "Y", "N", "_"))
                            RI.PutText(13, 36, isPostEnrollDefElig ? "Y" : "N", true); // POST ENROLL DEFER REQUESTED field
                    }
                    RI.PutText(16, 73, "Y"); // OFFICIAL VERIFICATION field
                }
                else
                {
                    RI.PutText(13, 36, "Y"); // FORBEARANCE TO CLEAR DELINQUENCY field
                    if (RI.CheckForText(14, 36, "Y", "N", "_"))
                        RI.PutText(14, 36, "Y"); // CAPITALIZE INTEREST field
                    if (RI.GetText(16, 74, 1).Trim().Length > 0)
                        RI.PutText(16, 74, "Y"); // OFFICIAL VERIFICATION field
                }

                if (ts26s.Any(o => o.LoanProgramType.IsIn("PLUSGB", "DLPLGB")))
                {
                    if (RI.CheckForText(18, 36, "Y", "N", "_"))
                        RI.PutText(18, 36, !anyBefore2008 ? "Y" : "N", true); // POST ENROLL DEFER REQUESTED field
                }
                else if ((dfType == "D15" || dfType == "D18") && RI.CheckForText(18, 36, "Y", "N", "_")) // Sometimes this field is not present in Session (when not last defer of this type on loan)
                    RI.PutText(18, 36, isPostEnrollDefElig ? "Y" : "N", true); // POST ENROLL DEFER REQUESTED field

                RI.PutText(16, 36, "Y"); // BORROWER REQUESTED field
                bool before1981 = ts26s.Any(ts => ts.DisbursementDate < DateTime.Parse("11/01/1981"));
                if (RI.CheckForText(14, 74, "Y", "_"))
                    RI.PutText(14, 74, before1981 ? "Y" : " ", true);
                RI.Hit(Key.Enter);

                if (RI.MessageCode.IsIn("02354", "02515"))
                {
                    RI.PutText(8, 18, "", true);
                    RI.PutText(8, 21, "", true);
                    RI.PutText(8, 24, "", true);
                    RI.Hit(Key.Enter);
                }
            }
            if (screenCode == "TSX4D")
            {
                if (newBeginDate.HasValue)
                    RI.PutText(6, 17, newBeginDate.Value.ToString("MMddyy")); //BEGIN DATE field
                if (newEndDate.HasValue)
                    RI.PutText(7, 17, newEndDate.Value.ToString("MMddyy")); //END DATE field
                RI.PutText(9, 18, esp.Esp_CertificationDate.Value.ToString("MMddyy")); // DATE CERTIFIED field
                if (mode == DfMode.Add)
                {
                    RI.PutText(10, 15, schoolCode ?? esp.SchoolCode); // SCHOOL CODE field
                }
                if (newEndDate.HasValue)
                    RI.PutText(12, 24, newEndDate.Value.AddDays(1).ToString("MMddyy")); // NEXT TERM BEGIN DATE field
                if (RI.CheckForText(14, 36, "Y", "N", "_"))
                    RI.PutText(14, 36, "Y"); // CAPITALIZE INTEREST field
                RI.PutText(16, 36, "Y"); // SIGNATURE OF BORROWER field
                RI.Hit(Key.Enter);

                if (RI.MessageCode == "02354")
                {
                    RI.PutText(8, 18, "", true);
                    RI.PutText(8, 21, "", true);
                    RI.PutText(8, 24, "", true);
                    RI.Hit(Key.Enter);
                }
            }
            if (screenCode == "TSX4B")
            {
                if (newBeginDate.HasValue)
                    RI.PutText(6, 18, newBeginDate.Value.ToString("MMddyy"));
                if (newEndDate.HasValue)
                    RI.PutText(7, 18, newEndDate.Value.ToString("MMddyy"));
                if (!string.IsNullOrWhiteSpace(RI.GetText(8, 18, 1)) && !modeIsChangeAndSepDateIsFuture)
                    RI.PutText(8, 18, esp.ArcRequestDate.Value.ToString("MMddyy"));
                RI.PutText(9, 18, esp.Esp_CertificationDate.Value.ToString("MMddyy"));

                if (RI.GetText(11, 22, 1) == "_")
                {
                    if (enrollmentStatus != null)
                        RI.PutText(11, 22, enrollmentStatus);
                }

                if (mode == DfMode.Add)
                {
                    RI.PutText(12, 16, schoolCode ?? esp.SchoolCode);

                    if (updatingPplus && dfType == "D45")
                        RI.PutText(10, 17, esp.StudentSsn); //STUDENT SSN field
                }

                if ((updatingPplus || dfType == "D18" || dfType == "D15"))
                {
                    if (RI.CheckForText(18, 36, "Y", "N", "_")) // Session has different fields depending on if deferments are stacked 
                    {
                        if (ts26s.Any(o => o.LoanProgramType.IsIn("DLPLUS", "PLUS"))) // Session different for plus loans
                            RI.PutText(18, 36, isPostEnrollDefElig ? "Y" : "N", true); // POST ENROLL DEFER REQUESTED field
                        else if (ts26s.Any(o => o.LoanProgramType.IsIn("DLPLGB", "PLUSGB")))
                            RI.PutText(18, 36, !anyBefore2008 ? "Y" : "N", true); // POST ENROLL DEFER REQUESTED field
                    }
                }

                RI.PutText(13, 36, "Y"); // NEW LOAN REQUIREMENTS MET field
                RI.PutText(14, 36, "Y"); // FORBEARANCE TO CLEAR DELINQ field
                if (RI.CheckForText(15, 36, "Y", "N", "_"))
                    RI.PutText(15, 36, "Y"); // CAPITALIZE INT field
                RI.PutText(16, 36, "Y"); // BORROWER REQUESTED field
                RI.PutText(16, 65, "Y"); // OFFICIAL VERIFICATION field
                RI.Hit(Key.Enter);

            }
            if (screenCode == "TSX32")
            {
                if (newBeginDate.HasValue)
                    RI.PutText(7, 18, newBeginDate.Value.ToString("MMddyy"));
                if (newEndDate.HasValue)
                    RI.PutText(8, 18, newEndDate.Value.ToString("MMddyy"));
                RI.PutText(9, 18, esp.Esp_CertificationDate.Value.ToString("MMddyy"));
                if (RI.CheckForText(17, 34, "Y", "N", "_"))
                    RI.PutText(17, 34, "Y");
                RI.Hit(Key.Enter);
            }
            if (RI.ScreenCode == screenCode) //screen should have changed but didn't
            {
                var msg = "Unable to add or change defer.  " + RI.Message;
                TH.SupervisorCloseTask(esp, ts26s, df, pplus, msg);
                PLR.AddNotification(esp.AccountNumber + ": " + msg, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                return false;
            }
            else
            {
                return DefermentForbearanceNonSelectionCheck(esp, dfType, df, ts26s, ts01s, pplus, isPostEnrollDefElig, newBeginDate ?? df.BeginDate.Value, newEndDate ?? df.EndDate.Value);
            }
        }

        readonly string[] DDB = new string[] { "F - PERM DIS", "F - DEATH", "F - BANKRUPTCY" }; //don't touch Death (F13), Disability (F14), or Bankruptcy (F10) entries
        /// <summary>
        /// Evaluate Non-selection reasons with the database and adjust D/F accordingly
        /// </summary>
        public bool DefermentForbearanceNonSelectionCheck(EspEnrollment esp, string dfType, TsayDefermentForbearance df, List<Ts26LoanInformation> ts26s, List<Ts01Enrollment> ts01s, List<ParentPlusLoanDetailsInformation> pplus, bool isPostEnrollDefElig, DateTime newBeginDate, DateTime newEndDate)
        {
            List<string> nonSelectionReasons = new List<string>();
            Action addReason = new Action(() =>
            {
                string reason = RI.GetText(21, 2, 80).Trim();
                if (!string.IsNullOrWhiteSpace(reason) && !nonSelectionReasons.Contains(reason))
                    nonSelectionReasons.Add(reason);
            });
            if (RI.ScreenCode == "TSX30")
                IterateTsx30(ts26s, addReason);
            else
                addReason();

            if (nonSelectionReasons.Any())
            {
                foreach (var reason in nonSelectionReasons)
                {
                    if (NSH.ReasonExists(reason, "A"))
                    {
                        TH.CloseTask(esp, ts26s, df, pplus, "Not Eligible");
                        return false;
                    }
                    else if (NSH.ReasonExists(reason, "B") || NSH.ReasonExists(reason, "D"))
                    {
                        bool cannotContinue = false;
                        bool needToCheckTsx31Again = false;
                        DateTime? additionalBegin = null;
                        DateTime? additionalEnd = null;

                        Action procTsx31 = () =>
                        {
                            DateHelper dh = new DateHelper(newBeginDate, newEndDate);
                            int foundToDelete = 0;
                            int? ourItem = null;
                            needToCheckTsx31Again = false; //Setting value again here in case this Action is called multiple times
                            Tsx31Data endingD46ToDelete = null;
                            IterateTsx31(tsx31 =>
                            {
                                if (ourItem == null && tsx31.Begin == newBeginDate && tsx31.End == newEndDate && tsx31.Cert == esp.Esp_CertificationDate)
                                {
                                    ourItem = tsx31.CurrentItem;
                                    return;  //this is our row, don't touch it
                                }
                                else if (ourItem == null) // When the first row isn't our row, we find it so we know what row not to touch
                                {
                                    var settings = PageHelper.IterationSettings.Default();
                                    IterateTsx31(dfRecord =>
                                    {
                                        if (ourItem != null)
                                        {
                                            settings.ContinueIterating = false;
                                            return;
                                        }
                                        else if (dfRecord.Begin == newBeginDate && dfRecord.End == newEndDate && dfRecord.Cert == esp.Esp_CertificationDate)
                                        {
                                            ourItem = dfRecord.CurrentItem;
                                            settings.ContinueIterating = false;
                                            return;
                                        }
                                    }, settings);
                                }
                                if (tsx31.CurrentItem == ourItem) // Don't touch the our d/f that we are adding/changing
                                    return;
                                if (tsx31.DfType == "D - POST ENRLL" && isPostEnrollDefElig && dh.EndingDefNeedsRemoval(tsx31.Begin, tsx31.End) && endingD46ToDelete == null) // Only remove if they are elig for system to auto-generate a new one
                                {
                                    if (dfType == "D45" || dfType == "D15" || dfType == "D18")
                                    {
                                        endingD46ToDelete = new Tsx31Data() // Record the D46 to be deleted
                                        {
                                            End = tsx31.End,
                                            Begin = tsx31.Begin,
                                            Cert = tsx31.Cert,
                                            IsDdb = tsx31.IsDdb,
                                            DfType = tsx31.DfType,
                                            Row = tsx31.Row,
                                        };
                                        foundToDelete++;
                                        return;
                                    }
                                }
                                if (dh.Intersects(tsx31.Begin, tsx31.End) && endingD46ToDelete == null)
                                {
                                    if (tsx31.IsDdb)
                                    {
                                        if (dh.SurroundsCurrentRange(tsx31.Begin, tsx31.End))
                                        {
                                            TH.SupervisorCloseTask(esp, ts26s, df, pplus, "Existing DDB conflicts.");
                                            cannotContinue = true;
                                            tsx31.Settings.ContinueIterating = false;
                                        }
                                        else
                                        {
                                            if (dh.IsWithinCurrentRange(tsx31.Begin, tsx31.End))
                                            {
                                                additionalBegin = tsx31.End.AddDays(1);
                                                additionalEnd = dh.ImmutableEnd;
                                            }
                                            var dh2 = new DateHelper(tsx31.Begin, tsx31.End);
                                            var newDates = dh2.GetNewStartAndEnd(newBeginDate, newEndDate);
                                            while (RI.MessageCode != "90007")
                                                RI.Hit(Key.F7);
                                            RI.Hit(Key.F5);
                                            IterateTsx31(child =>
                                            {
                                                if (child.CurrentItem == ourItem)
                                                {
                                                    RI.PutText(child.Row, 21, newDates.Item1.ToString("MMddyy"));
                                                    RI.PutText(child.Row, 30, newDates.Item2.ToString("MMddyy"));
                                                    RI.Hit(Key.Enter);
                                                    child.Settings.ContinueIterating = false;
                                                    tsx31.Settings.ContinueIterating = false;
                                                }
                                            });

                                        }
                                    }
                                    else
                                    {
                                        if (dh.IsWithinCurrentRange(tsx31.Begin, tsx31.End))
                                            foundToDelete++;
                                        else
                                        {
                                            var newDates = dh.GetNewStartAndEnd(tsx31.Begin, tsx31.End);
                                            RI.PutText(tsx31.Row, 21, newDates.Item1.ToString("MMddyy"));
                                            RI.PutText(tsx31.Row, 30, newDates.Item2.ToString("MMddyy"));
                                            RI.Hit(Key.Enter);
                                        }
                                    }
                                }
                            });
                            if (ourItem == null) // Dates were changed by Compass, couldn't find our defer 
                            {
                                cannotContinue = true;
                                PLR.AddNotification($"Unable to process non-selection reason while applying a forb for esp task {esp.TaskControlNumber}. Loan this occurred for: {ts26s.SingleOrDefault().LoanSequence.ToString()}.", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                            }
                            if (cannotContinue)
                                return;
                            while (foundToDelete > 0)
                            {
                                bool itemRemoved = false;
                                needToCheckTsx31Again = false;
                                RI.PutText(1, 4, "D", Key.Enter);
                                ourItem = null;
                                IterateTsx31(tsx31 =>
                                {
                                    if (ourItem == null && tsx31.Begin == newBeginDate && tsx31.End == newEndDate && tsx31.Cert == esp.Esp_CertificationDate)
                                    {
                                        ourItem = tsx31.CurrentItem;
                                        return;  //this is our row, don't touch it
                                    }
                                    else if (ourItem == null)
                                    {
                                        var settings = PageHelper.IterationSettings.Default();
                                        IterateTsx31(dfRecord =>
                                        {
                                            if (ourItem != null)
                                            {
                                                settings.ContinueIterating = false;
                                                return;
                                            }
                                            else if (dfRecord.Begin == newBeginDate && dfRecord.End == newEndDate && dfRecord.Cert == esp.Esp_CertificationDate)
                                            {
                                                ourItem = dfRecord.CurrentItem;
                                                settings.ContinueIterating = false;
                                                return;
                                            }
                                        }, settings);
                                    }
                                    if (endingD46ToDelete != null && isPostEnrollDefElig && tsx31.DfType == "D - POST ENRLL" && RI.CheckForText(tsx31.Row, 3, "_"))
                                    {
                                        itemRemoved = RemoveDeferment(esp, ts26s, df, tsx31, pplus, ref foundToDelete, endingD46ToDelete);
                                        endingD46ToDelete = null; // Set to null now that it has been removed
                                        if (!itemRemoved)
                                        {
                                            cannotContinue = true;
                                            tsx31.Settings.ContinueIterating = false;
                                        }
                                        else
                                            needToCheckTsx31Again = true;
                                    }
                                    else if (!tsx31.IsDdb && dh.IsWithinCurrentRange(tsx31.Begin, tsx31.End) && RI.CheckForText(tsx31.Row, 3, "_"))
                                    {
                                        itemRemoved = RemoveDeferment(esp, ts26s, df, tsx31, pplus, ref foundToDelete);
                                        if (!itemRemoved)
                                        {
                                            cannotContinue = true;
                                            tsx31.Settings.ContinueIterating = false;
                                        }
                                    }
                                });
                            }
                        };
                        if (RI.ScreenCode == "TSX30")
                            IterateTsx30(ts26s, procTsx31);
                        else
                            procTsx31();
                        if (cannotContinue)
                        {
                            TH.SupervisorCloseTask(esp, ts26s, df, pplus, $"Unable to process def/forb for deferment on account {esp.AccountNumber}.");
                            return false;
                        }
                        while (needToCheckTsx31Again)
                            procTsx31();

                        bool result = DefermentForbearanceNonSelectionCheck(esp, dfType, df, ts26s, ts01s, pplus, isPostEnrollDefElig, newBeginDate, newEndDate);
                        if (additionalBegin.HasValue && additionalEnd.HasValue)
                            if ((additionalEnd.Value - additionalBegin.Value).TotalDays >= 1)
                                result &= DefermentForbearanceAdd(esp, dfType, df, ts26s, ts01s, pplus, additionalBegin.Value, additionalEnd.Value);
                        return result;
                    }
                    else if (NSH.ReasonExists(reason, "C"))
                    {
                        if (ts26s.AllAndAny(o => o.LoanStatus == "VERIFIED DISABILITY"))
                        {
                            TH.CloseTask(esp, ts26s, df, pplus, "Unable to adjust due to Verified Disability, borrower is being discharged");
                            return false;
                        }
                        else
                        {
                            TH.SupervisorCloseTask(esp, ts26s, df, pplus, "Non Selection reason is Disability but loan not in status");
                            return false;
                        }
                    }
                    else if (NSH.ReasonExists(reason, "E"))
                    {
                        TH.CloseTask(esp, ts26s, df, pplus, "Borr waived in school deferments");
                        return false;
                    }
                    else if (NSH.ReasonExists(reason, "F"))
                    {
                        if (ts26s.AllAndAny(o => o.LoanStatus == "VERIFIED DEATH"))
                        {
                            TH.CloseTask(esp, ts26s, df, pplus, "Unable to adjust due to Verified Death, borrower is being discharged");
                            return false;
                        }
                        else
                        {
                            TH.SupervisorCloseTask(esp, ts26s, df, pplus, "Non Selection reason is Death but loan not in status");
                            return false;
                        }
                    }
                    else
                    {
                        TH.SupervisorCloseTask(esp, ts26s, df, pplus, "Sending to Sup to review non selection reason: " + nonSelectionReasons.FirstOrDefault());
                        return false;
                    }
                }
            }
            else
            {
                while (!RI.ScreenCode.IsIn("TSX4A", "TSX4B", "TSX32", "TSX4D")) // Added TSX4D bc D16's F12 back to that screen code
                {
                    RI.Hit(Key.F12);
                }
                RI.Hit(Key.F6);
                if (!RI.MessageCode.IsIn("01004", "01005"))
                {
                    TH.SupervisorCloseTask(esp, ts26s, df, pplus, "Sending to Sup to review defer forb application for esp");
                    return false;
                }
                else if (newEndDate < DateTime.Now.Date)
                {
                    if (ts26s.AllAndAny(o => o.LoanStatus == "IN REPAYMENT"))
                    {
                        if (df.EndDate > esp.ArcRequestDate && dfType != "F02" && dfType.IsIn(SchoolDefers))
                        {
                            return DefermentForbearanceAdd(esp, "F02", df, ts26s, ts01s, pplus, newEndDate.AddDays(1), df.EndDate);
                        }
                        else
                        {
                            RI.FastPath("tx3z/ITSAY" + esp.BorrowerSsn);
                            if (RI.ScreenCode == "TSXAZ")
                            {
                                throw new Exception("enter into the loan sequence");
                            }
                            bool? result = false;
                            PageHelper.Iterate(RI, (row, settings) =>
                            {
                                if (RI.CheckForText(row, 25, "D15", "D16", "D18") && RI.CheckForText(row, 72, DateTime.Now.ToShortDateString()))
                                {
                                    var beginDate = RI.GetText(row, 30, 10).ToDateNullable();
                                    var endDate = RI.GetText(row, 40, 10).ToDateNullable();
                                    var aboveBeginDate = RI.GetText(row - 1, 30, 10).ToDateNullable();
                                    var aboveEndDate = RI.GetText(row - 1, 40, 10).ToDateNullable();
                                    if (aboveBeginDate == (endDate ?? DateTime.MinValue).AddDays(1))
                                    {
                                        var list = new List<TsayDefermentForbearance>();
                                        list.Add(df);
                                        UpdateEnrollment(esp, ts26s, ts01s, list, pplus);
                                    }
                                    else
                                    {
                                        if (dfType != "F02" && dfType.IsIn(SchoolDefers))
                                            result = DefermentForbearanceAdd(esp, "F02", df, ts26s, ts01s, pplus, ts26s.Min(t => t.RepaymentStartDate), esp.ArcRequestDate);
                                    }
                                    settings.ContinueIterating = false;
                                }
                            });
                            if (result.HasValue)
                                return result.Value;
                        }
                    }
                    else if (ts26s.AllAndAny(o => o.LoanStatus.IsIn("DEFERMENT", "FORBEARANCE")) && dfType != "F02")
                    {
                        var notifyDate = esp.ArcRequestDate;
                        if (df == null)
                        {
                            TH.SupervisorCloseTask(esp, ts26s, df, pplus, "Review for late school notif forb");
                            return false;
                        }
                        else if (df.EndDate < notifyDate && dfType.IsIn(SchoolDefers))
                        {
                            return DefermentForbearanceAdd(esp, "F02", df, ts26s, ts01s, pplus, newEndDate.AddDays(1), df.EndDate);
                        }
                        else if (dfType.IsIn(SchoolDefers))
                        {
                            return DefermentForbearanceAdd(esp, "F02", df, ts26s, ts01s, pplus, newEndDate.AddDays(1), notifyDate);
                        }
                    }
                }
            }
            return true;
        }

        private bool RemoveDeferment(EspEnrollment esp, List<Ts26LoanInformation> ts26s, TsayDefermentForbearance df, Tsx31Data tsx31, List<ParentPlusLoanDetailsInformation> pplus, ref int foundToDelete, Tsx31Data endingD46ToDelete = null)
        {
            if (endingD46ToDelete != null && (tsx31.DfType != endingD46ToDelete.DfType
                || tsx31.Begin != endingD46ToDelete.Begin || tsx31.End != endingD46ToDelete.End || tsx31.Cert != endingD46ToDelete.Cert))
            {
                TH.SupervisorCloseTask(esp, ts26s, df, pplus, "D46 failed to be removed.");
                foundToDelete = 0;
                return false; // Since the deferments don't match, we will not remove it. Nor will we remove any other defs.  Task will be assigned to a supervisor
            }
            RI.PutText(tsx31.Row, 3, "X", Key.Enter);
            RI.PutText(1, 4, "C", Key.Enter);

            if (RI.MessageCode == "01893") // Compass message = MODIFICATION OF TRAN_ID AND/OR KEY IS NOT ALLOWED FOR HOTKEY-ONLY TRAN
            {
                tsx31.Settings.ContinueIterating = false;
                return false;
            }

            foundToDelete--;
            tsx31.Settings.ContinueIterating = false;
            return true;
        }

        private void IterateTsx31(Action<Tsx31Data> process, PageHelper.IterationSettings parentIteratorSettings = null)
        {
            var settings = parentIteratorSettings ?? PageHelper.IterationSettings.Default();
            settings.MinRow = 12;
            settings.MaxRow = 18;
            int currentItem = 0;
            PageHelper.Iterate(RI, (row, s) =>
            {
                string dfType = RI.GetText(row, 6, 14).Trim();
                bool isDdb = dfType.IsIn(DDB);
                DateTime begin = RI.GetText(row, 21, 8).ToDate();
                DateTime end = RI.GetText(row, 30, 8).ToDate();
                DateTime? cert = RI.GetText(row, 72, 8).ToDateNullable();
                currentItem++;
                process(new Tsx31Data() { Begin = begin, End = end, Cert = cert, IsDdb = isDdb, DfType = dfType, Row = row, Settings = s, CurrentItem = currentItem });
            }, settings);
        }

        private delegate void ProcessTsx31Row(DateTime begin, DateTime end, DateTime cert, bool isDdb, int row, PageHelper.IterationSettings s);


        private void IterateTsx30(List<Ts26LoanInformation> ts26s, Action process)
        {
            PageHelper.Iterate(RI, (row, settings) =>
            {
                int? sel = RI.GetText(row, 2, 2).ToIntNullable();
                int? seq = RI.GetText(row, 30, 3).ToIntNullable();
                if (sel.HasValue && seq.HasValue && ts26s.Any(o => o.LoanSequence == seq))
                {
                    RI.PutText(21, 14, sel.Value.ToString(), Key.Enter, true);
                    process();
                    RI.Hit(Key.F12);
                }
                if (!sel.HasValue)
                    settings.ContinueIterating = false;
            });
            RI.Hit(Key.Enter);
        }


        /// <summary>
        /// Update the system to match the given enrollment data.
        /// </summary>
        public bool UpdateEnrollment(EspEnrollment esp, List<Ts26LoanInformation> ts26s, List<Ts01Enrollment> ts01s, List<TsayDefermentForbearance> dfs, List<ParentPlusLoanDetailsInformation> pplus)
        {
            RI.FastPath("tx3z/CTS01" + esp.BorrowerSsn);
            RI.Hit(Key.F10);

            if (!ts26s.Any(p => p.LoanProgramType.IsIn("DLUNST", "DLSTFD", "UNSTFD", "STFFRD")))
            {
                TH.SupervisorCloseTask(esp, ts26s, dfs, pplus, "Unable to update TS01, please review enrollment");
                return false;
            }

            if (RI.CheckForText(3, 67, "MORE:   +"))
            {
                TH.SupervisorCloseTask(esp, ts26s, dfs, pplus, "Multiple pages on TSX03.");
                return false;
            }
            RI.PutText(4, 19, esp.Esp_SeparationDate.Value.ToString("MMddyy"));
            RI.PutText(4, 49, esp.Esp_Status);
            RI.PutText(4, 73, esp.SourceCode);
            RI.PutText(5, 19, esp.ArcRequestDate.Value.ToString("MMddyy"));
            RI.PutText(5, 43, esp.SchoolCode);
            if (esp.Esp_CertificationDate.Value.Date <= DateTime.Today)
                RI.PutText(6, 19, esp.Esp_CertificationDate.Value.ToString("MMddyy"));
            else
                RI.PutText(6, 19, esp.ArcRequestDate.Value.ToString("MMddyy"));
            RI.PutText(6, 43, esp.EnrollmentBeginDate.Value.ToString("MMddyy"));
            foreach (var row in Enumerable.Range(9, 14))
            {
                if (RI.CheckForText(row, 3, "_"))
                {
                    var loanProgram = RI.GetText(row, 33, 6).Trim();
                    var disbursementDate = RI.GetText(row, 40, 8).ToDateNullable();

                    /* Don't update the following loans:
                        Plus loans (DLPLGB, PLUSGB, DLPLUS, PLUS)
                        Consol loans(CNSLDN, SPCNSL, SUBCNS, SUBSPC, UNCNS, UNSPC, DLPCNS, DLUSPL, DLUCNS, DLSSPL, DLSCNS), 
                        Specialty loans (TILP, SLS, TEACH, COMPLT)
                       In other words, only update: DLUNST, DLSTFD, UNSTFD, STFFRD
                     */
                    if (loanProgram.IsIn("DLUNST", "DLSTFD", "UNSTFD", "STFFRD"))
                    {
                        if (ts26s.Any(o => o.LoanProgramType == loanProgram && o.DisbursementDate == disbursementDate))
                            RI.PutText(row, 3, "X");
                    }
                }
                else
                    break;
            }
            RI.Hit(Key.F6);

            while (RI.MessageCode == "03001")
            {
                RI.HitChar(' ');
                RI.Hit(Key.F6);
            }
            if (RI.MessageCode == "03861")
                RI.Hit(Key.F6);
            if (RI.MessageCode == "90002" || RI.MessageCode == "03001")
            {
                PLR.AddNotification(RI.Message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return false;
            }
            RI.Hit(Key.F6);
            if (RI.MessageCode.IsIn("01714", "01713"))
                RI.Hit(Key.F6);
            if (RI.MessageCode == "01712")
            {
                TH.SupervisorCloseTask(esp, ts26s, dfs, pplus, string.Format("{0}, {1}, {2}, {3}", RI.GetText(1, 4, 1), RI.GetText(1, 5, 4), RI.GetText(1, 72, 5), RI.GetText(23, 2, 78)));
                return false;
            }
            if (RI.MessageCode == "90036")
                RI.Hit(Key.F6, 2);
            if (!RI.MessageCode.IsIn("01460", "01067", "90036"))
            {
                if (RI.MessageCode == "01279")
                    TH.SupervisorCloseTask(esp, ts26s, dfs, pplus, "Possible Verified DDB, Please Review");
                else
                    TH.SupervisorCloseTask(esp, ts26s, dfs, pplus, "Unexpected Outcome: " + RI.Message);
                return false;
            }
            if (ts26s.Any(o => o.LoanStatus.IsIn("VERIFIED DEATH", "VERIFIED DISABILITY"))) // Sometimes Session doesn't catch DDB
            {
                TH.SupervisorCloseTask(esp, ts26s, dfs, pplus, "Possible Verified DDB, Please Review");
                return false;
            }
            DateTime? newRepayBegin = null;
            foreach (var row in Enumerable.Range(9, 14))
            {
                var loanProgram = RI.GetText(row, 33, 6).Trim();
                var disbursementDate = RI.GetText(row, 40, 8).ToDateNullable();
                if (RI.CheckForText(row, 3, "X"))
                    if (ts26s.Any(o => o.LoanProgramType == loanProgram && o.DisbursementDate == disbursementDate))
                    {
                        var repayBegin = RI.GetText(row, 15, 8).ToDateNullable();
                        if (newRepayBegin == null)
                            newRepayBegin = repayBegin;
                        if (repayBegin < newRepayBegin)
                            newRepayBegin = repayBegin;
                    }
            }
            if (newRepayBegin < DateTime.Today)
                if (ts26s.AllAndAny(o => o.RepaymentStartDate < DateTime.Today))
                {
                    if (newRepayBegin < ts26s.Min(ts26 => ts26.RepaymentStartDate))
                        return DefermentForbearanceAdd(esp, "F02", null, ts26s, ts01s, pplus, newRepayBegin, ts26s.Min(o => o.RepaymentStartDate)?.AddDays(-1));
                }
                else
                {
                    if (newRepayBegin < esp.ArcRequestDate)
                        return DefermentForbearanceAdd(esp, "F02", null, ts26s, ts01s, pplus, newRepayBegin, esp.ArcRequestDate);
                }
            return true;
        }
    }
}
