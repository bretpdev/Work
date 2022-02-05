using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.Scripts;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using System.Windows.Forms;

namespace TPERM
{
    public class TPERM : ScriptBase
    {
        public TPERM(ReflectionInterface ri)
            : base(ri, "TPERM", DataAccessHelper.Region.Pheaa)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
        }

        public override void Main()
        {
            Process();
            EndPL();
        }

        private void Process()
        {
            ApplicationData appData = GetNextQueue();

            while (appData != null)
            {
                using (BorrowerEntry be = new BorrowerEntry(appData))
                {
                    if (be.ShowDialog() != DialogResult.OK)
                        return;

                    appData = be.AppData;
                    if (appData.Result != ApplicationData.ApplicationResults.InProcess)
                    {
                        bool hold = false;
                        if (appData.Result == ApplicationData.ApplicationResults.NoSign)
                        {

                            if (!appData.HasNoLoans)
                                SendDenialLetter(appData);
                            else
                            {
                                if (!RI.ATD42AllLoans(appData.BorrowerInfo.Ssn, "G990L", "missing signature on TPERM form pls snd dnl lttr"))
                                {
                                    string message = string.Format("The following ARC was not added. ARC:G990L ACCT:{0} ERRORCODE:{1} COMMENT:missing signature on TPERM form pls snd dnl lttr.  Do you want to place this task on hold?",
                                         appData.BorrowerInfo.AccountNumber, RI.Message);
                                    ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                                    hold = Dialog.Error.YesNo(message);
                                }
                            }
                            ProcessHoldNoLoans(appData, hold);

                        }
                        else if (appData.Result == ApplicationData.ApplicationResults.WrongSign)
                        {
                            SendTNAC(appData.BorrowerInfo.AccountNumber, appData.HasNoLoans, appData.BorrowerInfo.Ssn);
                            if (!appData.HasNoLoans)
                                RI.Atd22AllLoans(appData.BorrowerInfo.AccountNumber, "G990L", "TPERM denied due to name discrepancy. Please send denial letter.", string.Empty, null, false);
                            else
                            {
                                if (!RI.ATD42AllLoans(appData.BorrowerInfo.Ssn, "G990L", "TPERM denied due to name discrepancy. Please send denial letter."))
                                {
                                    string message = string.Format("The following ARC was not added. ARC:G990L ACCT:{0} ERRORCODE:{1} COMMENT:missing signature on TPERM form pls snd dnl lttr.  Do you want to place this task on hold?",
                                         appData.BorrowerInfo.AccountNumber, RI.Message);
                                    ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                                    hold = Dialog.Error.YesNo(message);
                                }
                            }
                            ProcessHoldNoLoans(appData, hold);

                        }
                        else if (appData.Result == ApplicationData.ApplicationResults.Illegible)
                        {
                            AddComment(appData, appData.HasNoLoans);
                            PlaceTaskOnHold(appData.TaskInfo.TaskControlNumber);
                            if (!appData.HasNoLoans)
                                SendDenialLetter(appData);
                            else
                            {
                                if (!RI.ATD42AllLoans(appData.BorrowerInfo.Ssn, "G990L", "Brwr sbmted TPERM form is illegible/damaged pls snd dnl ltr"))
                                {
                                    string message = string.Format("The following ARC was not added. ARC:G990L ACCT:{0} ERRORCODE:{1} COMMENT:Brwr sbmted TPERM form is illegible/damaged pls snd dnl ltr. ",
                                         appData.BorrowerInfo.AccountNumber, RI.Message);
                                    ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                                }
                            }
                        }
                        else if (appData.Result == ApplicationData.ApplicationResults.NoMatch)
                        {
                            SendTNAC(appData.BorrowerInfo.AccountNumber, appData.HasNoLoans, appData.BorrowerInfo.Ssn);
                            SendDenialLetter(appData);
   
                            if (appData.HasNoLoans)
                            {
                                Dialog.Info.Ok("Borrower has no loans, task on hold for DLCO release.");
                                hold = true;
                            }
                            ProcessHoldNoLoans(appData, hold);
                        }
                        else if (appData.Result == ApplicationData.ApplicationResults.WrongForm)
                        {
                            AddComment(appData, appData.HasNoLoans);
                            if (!appData.HasNoLoans)
                                RI.Atd22AllLoans(appData.BorrowerInfo.AccountNumber, "RVW3P", string.Format("Per Seq {0}, review the TPERM or POA issue.", appData.TaskInfo.ActivitySeq), string.Empty, null, false);
                            else
                            {
                                if (!RI.ATD42AllLoans(appData.BorrowerInfo.Ssn, "RVW3P", string.Format("Per Seq {0}, review the TPERM or POA issue.", appData.TaskInfo.ActivitySeq)))
                                {
                                    string message = string.Format("The following ARC was not added. ARC:RVW3P ACCT:{0} ERRORCODE:{1} COMMENT:{2}  Do you want to place this task on hold?",
                                         appData.BorrowerInfo.AccountNumber, RI.Message, string.Format("Per Seq {0}, review the TPERM or POA issue.", appData.TaskInfo.ActivitySeq));
                                    ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                                    hold = Dialog.Error.YesNo(message);
                                }
                            }
                            ProcessHoldNoLoans(appData, hold);
                        }

                        appData = GetNextQueue();
                    }
                    else
                    {
                        bool result = ProcessApplication(appData);
                        if (!result)
                            return;
                        if (appData.AddMultiple)
                        {
                            appData.Clear();
                            continue;
                        }
                        else
                        {
                            CloseTask(appData);
                            appData = GetNextQueue();
                        }
                    }
                }
            }
        }

        private void ProcessHoldNoLoans(ApplicationData appData, bool hold)
        {
            if (!hold)
            {
                CloseTask(appData);
                AddComment(appData, false);
            }
            else
            {
                HoldNoLoans(appData);
            }
        }

        private void HoldNoLoans(ApplicationData appData)
        {
            ManualComments cmts = new ManualComments(string.Empty, 70);
            cmts.SetLabel("Please enter the reason the task is being placed on hold.");
            cmts.ShowDialog();
            string comment = cmts.Comment;
            RI.FastPath("TX3Z/ITD2A*");
            RI.PutText(4, 16, appData.BorrowerInfo.Ssn);
            RI.PutText(12, 65, appData.TaskInfo.ActivitySeq, ReflectionInterface.Key.Enter);
            RI.Hit(ReflectionInterface.Key.F4);
            RI.PutText(8, 5, string.Format("***task on hold {0:MM/dd/yyyy} {1}", DateTime.Now, comment), ReflectionInterface.Key.Enter);
            PlaceTaskOnHold(appData.TaskInfo.TaskControlNumber, comment);
        }

        private bool ProcessApplication(ApplicationData appData)
        {
            List<PossibleReferenceMatch> match = CheckForReference(appData.BorrowerInfo.Ssn, appData.ReferenceInfo.FirstName, appData.ReferenceInfo.LastName);
            if (match.Count != 0)
            {
                using (ExistingReferenceSelection sel = new ExistingReferenceSelection(match))
                {
                    sel.ShowDialog();
                    if (sel.SelectedReference != null)
                    {
                        appData.SelectedReference = sel.SelectedReference;
                        appData.ReferenceInfo.FirstName = sel.SelectedReference.FirstName;
                        appData.ReferenceInfo.LastName = sel.SelectedReference.LastName;
                        GetReferenceDemos(appData);
                    }
                }
            }

            using (AddModifyReferencesDemos refD = new AddModifyReferencesDemos(appData))
            {
                if (refD.ShowDialog() == DialogResult.OK)
                {
                    if (!AddOrUpdateRef(appData))
                    {
                        Dialog.Error.Ok("The reference was not added.  Please review and try again.");
                        return false;
                    }
                    AddComment(appData, false);
                }
                else return false;
            }

            return true;
        }

        private bool AddOrUpdateRef(ApplicationData appData)
        {
            if (appData.SelectedReference != null && appData.SelectedReference.IsAuthed)
                appData.Result = ApplicationData.ApplicationResults.AuthAlreadyExists;
            else
            {
                if (appData.AuthEndDate.HasValue)
                    appData.Result = ApplicationData.ApplicationResults.NewAuthEndDate;
                else
                    appData.Result = ApplicationData.ApplicationResults.NewAuth;
            }
            if (appData.SelectedReference != null)//Update the user did not select an existing reference
            {
                RI.FastPath("TX3ZCTX1JR" + appData.SelectedReference.ReferenceNumber);
                RI.Hit(ReflectionInterface.Key.F6);

            }
            else//we need to add a new reference
            {
                RI.FastPath("TX3ZATX1JR");
                RI.PutText(4, 6, appData.ReferenceInfo.LastName);
                RI.PutText(4, 34, appData.ReferenceInfo.FirstName);
                RI.PutText(4, 53, appData.ReferenceInfo.MiddleName);
                RI.PutText(7, 11, appData.BorrowerInfo.Ssn);
            }

            RI.PutText(8, 15, appData.ReferenceInfo.RelationshipCode, true);
            if (!appData.ReferenceInfo.NotAuthed && RI.CheckForText(8, 49, "N") || RI.CheckForText(8, 49, "_"))
                RI.PutText(8, 33, DateTime.Now.ToString("MMddyy"), true);
            RI.PutText(8, 49, appData.ReferenceInfo.NotAuthed ? "N" : "Y", true);
            if (appData.AuthEndDate.HasValue)
                RI.PutText(8, 71, appData.AuthEndDate.Value.ToString("MMddyy"));

            CommitChanges(appData);
            if (appData.ReferenceInfo.RefAddress != null)
            {
                RI.PutText(10, 32, DateTime.Now.ToString("MMddyy"), true);
                RI.PutText(11, 10, appData.ReferenceInfo.RefAddress.Street1, true);
                RI.PutText(12, 10, appData.ReferenceInfo.RefAddress.Street2, true);
                RI.PutText(13, 10, appData.ReferenceInfo.RefAddress.Street3, true);
                RI.PutText(12, 52, appData.ReferenceInfo.RefAddress.ForeignState, true);
                RI.PutText(12, 77, appData.ReferenceInfo.RefAddress.ForeignCode, true);
                RI.PutText(13, 52, "", true);
                RI.PutText(14, 8, appData.ReferenceInfo.RefAddress.City, true);
                RI.PutText(14, 32, appData.ReferenceInfo.RefAddress.State, true);
                RI.PutText(14, 40, appData.ReferenceInfo.RefAddress.Zip, true);
                RI.PutText(11, 55, appData.ReferenceInfo.RefAddress.IsValid ? "Y" : "N");
                RI.PutText(10, 67, "A");
            }

            CommitChanges(appData);

            AddUpdatePhone(appData.ReferenceInfo.HomePhone, "H");
            AddUpdatePhone(appData.ReferenceInfo.AltPhone, "A");
            AddUpdatePhone(appData.ReferenceInfo.WorkPhone, "W");
            AddUpdatePhone(appData.ReferenceInfo.MobilePhone, "M");

            if (RI.MessageCode == "01021")
                RI.Hit(ReflectionInterface.Key.Enter);

            if (RI.MessageCode == "01079")
                RI.Hit(ReflectionInterface.Key.Enter);

            return RI.MessageCode.IsIn("01004", "01100", "01097");

        }

        private void CommitChanges(ApplicationData appData)
        {
            if (appData.SelectedReference != null)
            {
                RI.Hit(ReflectionInterface.Key.Enter);
                RI.Hit(ReflectionInterface.Key.F6);
            }
        }

        private void AddUpdatePhone(ReferencePhone phone, string type)
        {
            if (phone == null)
                return;

            if (!RI.CheckForText(16, 14, type))
            {
                if (RI.MessageCode == "01004")
                    RI.Hit(ReflectionInterface.Key.F6, 3);
                RI.PutText(16, 14, type, ReflectionInterface.Key.Enter);

            }
            RI.PutText(16, 20, phone.Mbl, true);
            RI.PutText(16, 30, phone.Consent, true);
            RI.PutText(16, 45, DateTime.Now.ToString("MMddyy"), true);
            if (phone.Phone.IsPopulated())
            {
                RI.PutText(17, 14, phone.Phone.Substring(0, 3), true);
                RI.PutText(17, 23, phone.Phone.Substring(3, 3), true);
                RI.PutText(17, 31, phone.Phone.Substring(6), true);
                RI.PutText(17, 40, phone.PhoneExtension, true);
                RI.PutText(17, 54, phone.IsValid ? "Y" : "N", true);
                RI.PutText(18, 15, string.Empty, true);
                RI.PutText(18, 24, string.Empty, true);
                RI.PutText(18, 36, string.Empty, true);
                RI.PutText(18, 53, string.Empty, true);
            }
            if (phone.ForeignPhone.IsPopulated())
            {
                RI.PutText(18, 15, phone.ForeignPhone.Substring(0, 3), true);
                RI.PutText(18, 24, phone.ForeignPhone.Substring(2, 5), true);
                RI.PutText(18, 36, phone.ForeignPhone.Substring(6), true);
                RI.PutText(18, 53, phone.ForeignPhoneExtension, true);
                RI.PutText(17, 54, phone.IsValid ? "Y" : "N", true);
                RI.PutText(17, 14, string.Empty, true);
                RI.PutText(17, 23, string.Empty, true);
                RI.PutText(17, 31, string.Empty, true);
                RI.PutText(17, 40, string.Empty, true);
            }

            RI.PutText(17, 54, phone.IsValid ? "Y" : "N");
            RI.PutText(16, 78, "A");
            RI.PutText(19, 14, phone.SourceCode, ReflectionInterface.Key.Enter, true);
        }

        private void GetReferenceDemos(ApplicationData appData)
        {
            RI.FastPath("TX3ZITX1JR" + appData.SelectedReference.ReferenceNumber);
            appData.ReferenceInfo.RefAddress = new AddressDemographics()
            {
                Street1 = RI.GetText(11, 10, 33),
                Street2 = RI.GetTextRemoveUnderscores(12, 10, 33),
                Street3 = RI.GetTextRemoveUnderscores(13, 10, 33),
                City = RI.GetText(14, 8, 21),
                State = RI.GetText(14, 32, 2),
                Zip = RI.GetText(14, 40, 17),
                ForeignCode = RI.GetTextRemoveUnderscores(12, 77, 2),
                ForeignState = RI.GetTextRemoveUnderscores(12, 52, 16),
                IsValid = RI.CheckForText(11, 55, "Y")
            };

            RI.Hit(ReflectionInterface.Key.F6, 3);
            appData.ReferenceInfo.HomePhone = GetPhoneByType("H");
            appData.ReferenceInfo.AltPhone = GetPhoneByType("A");
            appData.ReferenceInfo.MobilePhone = GetPhoneByType("M");
            appData.ReferenceInfo.WorkPhone = GetPhoneByType("W");
        }

        private ReferencePhone GetPhoneByType(string type)
        {
            if (!RI.CheckForText(16, 14, type))
                RI.PutText(16, 14, type, ReflectionInterface.Key.Enter);

            if (RI.MessageCode == "01105")
                return null;
            return new ReferencePhone()
            {
                Mbl = RI.GetText(16, 20, 1),
                Consent = RI.GetText(16, 30, 1),
                Phone = RI.GetTextRemoveUnderscores(17, 14, 3) + RI.GetTextRemoveUnderscores(17, 23, 3) + RI.GetTextRemoveUnderscores(17, 31, 4),
                PhoneExtension = RI.GetTextRemoveUnderscores(17, 40, 5),
                SourceCode = RI.GetText(19, 14, 2),
                ForeignPhone = RI.GetTextRemoveUnderscores(18, 15, 3) + RI.GetTextRemoveUnderscores(18, 24, 5) + RI.GetTextRemoveUnderscores(18, 36, 10),
                ForeignPhoneExtension = RI.GetTextRemoveUnderscores(18, 53, 5),
                IsValid = RI.CheckForText(17, 54, "Y")
            };
        }

        private List<PossibleReferenceMatch> CheckForReference(string borSsn, string referenceFirstName, string referenceLastName)
        {
            List<PossibleReferenceMatch> possibleReferenceLocation = new List<PossibleReferenceMatch>();
            RI.FastPath("TX3ZITX1JB" + borSsn);
            RI.Hit(ReflectionInterface.Key.F2);
            RI.Hit(ReflectionInterface.Key.F4);
            for (int row = 10; RI.MessageCode != "90007"; row++)
            {
                if (row > 21)
                {
                    row = 9;
                    RI.Hit(ReflectionInterface.Key.F8);
                    continue;
                }

                //We only want to check references that are active.
                if (!RI.CheckForText(row, 78, "A"))
                    continue;

                string selection = RI.GetText(row, 2, 2);
                string page = RI.GetText(2, 71, 2);
                RI.PutText(22, 12, selection, ReflectionInterface.Key.Enter, true);

                possibleReferenceLocation.Add(new PossibleReferenceMatch() { FirstName = RI.GetText(4, 34, 13), LastName = RI.GetText(4, 6, 23), ReferenceNumber = RI.GetText(3, 12, 11).Replace(" ", ""), IsAuthed = RI.CheckForText(8, 49, "Y") });

                RI.Hit(ReflectionInterface.Key.F12);
            }
            return possibleReferenceLocation;
        }

        private void PlaceTaskOnHold(string taskControlNum, string comment = "")
        {
            AccessQueue(string.Format("{0}*", taskControlNum.Substring(0, 9)));
            if (RI.CheckForText(1, 74, "TXX71")) //Multiple queues
            {
                for (int row = 8; RI.MessageCode != "90007"; row += 3)
                {
                    if (RI.CheckForText(row, 4, " ") || row > 18)
                    {
                        row = 5;
                        RI.Hit(ReflectionInterface.Key.F8);
                        continue;
                    }

                    RI.PutText(21, 18, RI.GetText(row, 3, 3), ReflectionInterface.Key.F2, true);
                    RI.PutText(8, 19, "H", ReflectionInterface.Key.Enter);
                    RI.Hit(ReflectionInterface.Key.F12);
                }
            }
            else
            {
                RI.PutText(21, 18, "01", ReflectionInterface.Key.F2);
                RI.PutText(8, 19, "H", ReflectionInterface.Key.Enter);
            }
            RI.FastPath("TX3Z/CTX6J");
            RI.PutText(7, 42, "SB");
            RI.PutText(8, 42, "01");
            RI.PutText(10, 42, "BF10Q");
            RI.PutText(9, 42, (taskControlNum.Substring(0, 9) + "*"));
            RI.PutText(9, 76, "A", ReflectionInterface.Key.Enter);
            if (RI.ScreenCode == "TXX6O")
            {
                RI.PutText(21, 2, string.Format("{0}", comment), ReflectionInterface.Key.Enter, true);
                RI.Hit(ReflectionInterface.Key.F12);
            }
            else
            {

                for (int row = 9; RI.MessageCode != "90007"; row += 2)
                {
                    if (RI.CheckForText(row, 3, " ") || row > 20)
                    {
                        row = 5;
                        RI.Hit(ReflectionInterface.Key.F8);
                        continue;
                    }

                    RI.PutText(21, 18, RI.GetText(row, 2, 3), ReflectionInterface.Key.Enter, true);
                    RI.PutText(21, 2, string.Format("{0}", comment), ReflectionInterface.Key.Enter);
                    RI.Hit(ReflectionInterface.Key.F12);
                }
            }
        }

        private void SelectCoMakerLoans(ApplicationData appData)
        {
            for (int row = 11; RI.MessageCode != "90007"; row++)
            {
                if (row > 22 || RI.CheckForText(row, 2, " "))
                {
                    row = 10;
                    RI.Hit(ReflectionInterface.Key.F8);
                    continue;
                }

                if (RI.GetText(22, 59, 9) == appData.SelectedCoMaker.Ssn)
                    RI.PutText(row, 2, "X");
            }

            RI.Hit(ReflectionInterface.Key.F11);
        }

        private void SendDenialLetter(ApplicationData appData)
        {
            if (DataAccessHelper.TestMode)
            {
                Dialog.Info.Ok("Letter writer does not work in the test region.  Tha app will skip this part.");
                return;
            }
            RI.FastPath(string.Format("TX3Z/ITXBI{0}FS06OTPRMD", appData.BorrowerInfo.Ssn));
            if (appData.SelectedCoMaker != null)
                SelectCoMakerLoans(appData);
            else
                RI.PutText(7, 2, "X", ReflectionInterface.Key.F11);

            if (RI.CheckForText(1, 2, "FS06OTPRMD"))
            {
                List<int> num = RI.GetText(20, 15, 50).SplitAndRemoveQuotes(" ").Select(p => int.Parse(p)).ToList();
                int newIn = 0;
                if (appData.Result == ApplicationData.ApplicationResults.NoSign)
                    newIn = 4;
                else if (appData.Result == ApplicationData.ApplicationResults.WrongSign)
                    newIn = 4;
                else if (appData.Result == ApplicationData.ApplicationResults.NoMatch)
                    newIn = 9;
                else if (appData.Result == ApplicationData.ApplicationResults.Illegible)
                    newIn = 6;
                else if (appData.Result == ApplicationData.ApplicationResults.NoAuthAddedRef)
                    newIn = 5;

                List<string> nums = new List<string>();

                foreach (int selNum in num)
                {
                    if (selNum < newIn && !nums.Where(p => p.Contains(string.Format("+{0}", newIn))).Any())
                        nums.Add(selNum.ToString());
                    else
                    {
                        nums.Add(string.Format("+{0}", newIn));
                        nums.Add(selNum.ToString());
                    }

                }

                nums.Add("%e");
                nums = nums.Distinct().ToList();

                RI.PutText(20, 15, string.Join(" ", nums), ReflectionInterface.Key.Enter, true);
                RI.Hit(ReflectionInterface.Key.Enter, 2);
                RI.PutText(6, 26, "1");
                RI.PutText(11, 26, "1", ReflectionInterface.Key.Enter);
            }
        }

        private void SendTNAC(string accountNumber, bool noLoans, string ssn)
        {
            RI.FastPath(string.Format("TX3Z/ITD2A{0}", accountNumber));
            RI.PutText(21, 16, DateTime.Now.AddMonths(-1).ToString("MMddyy"));
            RI.PutText(21, 30, DateTime.Now.ToString("MMddyy"));
            RI.PutText(11, 65, "G727M", ReflectionInterface.Key.Enter);
            if (RI.ScreenCode == "TDX2C" || RI.ScreenCode == "TDX2D")//Borrower has TNAC in last 30 days
                return;
            if (!noLoans)
                RI.Atd22AllLoans(accountNumber, "G727M", string.Empty, string.Empty, null, false);
            else
                RI.ATD42AllLoans(ssn, "G727M", string.Empty);
        }

        private void CloseTask(ApplicationData appData)
        {
            RI.FastPath("TX3Z/ITX6XSB;01;;BF10Q");
            bool selectedTask = false;
            for (int row = 8; RI.MessageCode != "90007"; row += 3)
            {
                if (RI.CheckForText(row, 4, " ") || row > 20)
                {
                    row = 5;
                    RI.Hit(ReflectionInterface.Key.F8);
                    continue;
                }
                if (RI.CheckForText(row, 75, "W"))
                {
                    RI.PutText(21, 18, RI.GetText(row, 4, 1), ReflectionInterface.Key.F2);
                    selectedTask = true;
                    break;
                }
            }

            if (!selectedTask)
                return;

            RI.PutText(8, 19, "C");
            RI.PutText(9, 19, "COMPL", ReflectionInterface.Key.Enter);
            CloseDupTasks(appData);
        }

        private void CloseDupTasks(ApplicationData appData)
        {
            RI.FastPath("TX3Z/ITX6X");
            RI.PutText(6, 37, "SB", true);
            RI.PutText(8, 37, "01", true);
            RI.PutText(10, 37, (appData.BorrowerInfo.Ssn + "*"), true);
            RI.PutText(12, 37, "BF10Q", ReflectionInterface.Key.Enter, true);
            if (RI.CheckForText(1, 74, "TXX71"))
            {
                appData.DupTasks = new List<TaskData>();
                for (int row = 8; RI.MessageCode != "90007"; row += 3)
                {
                    if (row > 20 || RI.CheckForText(row, 4, " "))
                    {
                        row = 5;
                        RI.Hit(ReflectionInterface.Key.F8);
                        continue;
                    }
                    appData.DupTasks.Add(new TaskData(RI.GetText(row, 6, 19)));
                }

                foreach (TaskData task in appData.DupTasks)
                {
                    RI.FastPath("TX3Z/ITD2A*");
                    RI.PutText(4, 16, appData.BorrowerInfo.Ssn);
                    RI.PutText(12, 65, task.ActivitySeq, ReflectionInterface.Key.Enter);
                    RI.Hit(ReflectionInterface.Key.F4);
                    RI.PutText(8, 5, string.Format("Dupe task see seq {0}. No chngs made", appData.TaskInfo.ActivitySeq), ReflectionInterface.Key.Enter);
                    RI.FastPath(string.Format("TX3Z/ITX6XSB;01;{0};BF10Q", task.TaskControlNumber));
                    RI.PutText(21, 18, "01", ReflectionInterface.Key.Enter);
                    RI.FastPath(string.Format("TX3Z/ITX6XSB;01;{0};BF10Q", task.TaskControlNumber));
                    RI.PutText(21, 18, "01", ReflectionInterface.Key.F2);
                    RI.PutText(8, 19, "C");
                    RI.PutText(9, 19, "COMPL", ReflectionInterface.Key.Enter);
                }
            }

        }

        private void AddComment(ApplicationData appData, bool noLoans)
        {
            RI.FastPath("TX3Z/ITD2A*");
            RI.PutText(4, 16, appData.BorrowerInfo.Ssn);
            RI.PutText(12, 65, appData.TaskInfo.ActivitySeq, ReflectionInterface.Key.Enter);
            RI.Hit(ReflectionInterface.Key.F4);
            RI.PutText(8, 5, appData.GetComment(noLoans), ReflectionInterface.Key.Enter);
        }

        private ApplicationData GetNextQueue()
        {
            AccessQueue();

            if (!RI.CheckForText(1, 74, "TXX71"))
                return null;

            if (!RI.GetText(8, 75, 1).IsIn("W", "A"))
            {
                Dialog.Error.Ok("You do not have any tasks assigned.");
                return null;
            }
            ApplicationData bData = new ApplicationData();
            bData.TaskInfo = new TaskData(RI.GetText(8, 6, 20)); //Task Control Number
            string tempSSN = RI.GetText(8, 6, 9);
            RI.PutText(21, 18, "01", ReflectionInterface.Key.Enter, true);
            if (RI.MessageCode == "01848")
            {
                Dialog.Error.Ok(RI.Message);
                return null;
            }
            string bwrSsn = RI.GetText(4, 16, 11).Replace(" ", "");
            if (RI.MessageCode == "50108")//Borrower has no loans
            {
                bData.HasNoLoans = true;
                bwrSsn = tempSSN;
            }
            bData.Populate(RI, bwrSsn);
            RI.FastPath("TX3Z/ITD2A*");
            RI.PutText(4, 16, bwrSsn);
            RI.PutText(12, 65, bData.TaskInfo.ActivitySeq, ReflectionInterface.Key.Enter);
            if (RI.ScreenCode != "TDX2D")
            {
                Dialog.Error.Ok(string.Format("Unable to locate ARC in TD2A for Task Control Number {0}.  Please review and try again.", bData.TaskInfo.TaskControlNumber));
                return null;
            }

            if (RI.GetText(13, 2, 5) != "BF10Q")
            {
                Dialog.Error.Ok("The task found is not a BF10Q task.  Please review and try again.");
                return null;
            }

            bData.TaskInfo.CorrDocNum = RI.GetText(15, 64, 18);
            return bData;
        }

        private void AccessQueue(string ssn = "")
        {
            RI.FastPath("TX3ZITX6X");
            RI.PutText(6, 37, "SB", true);
            RI.PutText(8, 37, "01", true);
            RI.PutText(10, 37, ssn, true);
            RI.PutText(12, 37, "BF10Q", ReflectionInterface.Key.Enter, true);
        }

        private void EndPL()
        {
            ProcessLogger.LogEnd(ProcessLogData.ProcessLogId);
        }
    }
}
