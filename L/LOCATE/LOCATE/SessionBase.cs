using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace LOCATE
{
    public class SessionBase
    {
        public enum SkipType
        {
            A, //Address
            B, //Both Address and Phone
            None,
            P //Phone, Having the enum be a one letter characters for ease of use later on the the script.
        }

        private ReflectionInterface RI { get; set; }
        private string AccountIdentifer { get; set; }
        private bool IsAddressValid { get; set; }
        private bool IsPhoneValid { get; set; }
        public ProcessLogData LogData { get; set; }

        public SessionBase(ReflectionInterface ri, string accountIdentifer, ProcessLogData logData)
        {
            RI = ri;
            AccountIdentifer = accountIdentifer;
            LogData = logData;
        }

        protected bool HasOpenLoansOnCompass()
        {
            RI.FastPath("TX3Z/ITS26*");
            RI.PutText(8, 40, AccountIdentifer, ReflectionInterface.Key.Enter, true);
            if (RI.ScreenCode == "TSX28")//Selection Screen
            {
                for (int row = 8; RI.MessageCode != "90007"; row++)
                {
                    if (RI.CheckForText(row, 3, " ") || row > 20)
                    {
                        RI.Hit(ReflectionInterface.Key.F8);
                        row = 7;
                        continue;
                    }
                    if (HasBalance(row, 60, 9))
                        return true;
                }
            }
            else
            {
                return HasBalance(11, 12, 10);
            }

            return false;
        }

        protected bool HasOpenLoansOnOneLink()
        {
            RI.FastPath("LC05I*");
            RI.PutText(6, 18, AccountIdentifer, ReflectionInterface.Key.Enter, true);
            if (RI.CheckForText(1, 62, "DEFAULT/CLAIM RECAP"))
                return HasBalance(4, 71, 9);
            return false;
        }

        private bool HasBalance(int row, int col, int length)
        {
            decimal? value = RI.GetText(row, col, length).ToDecimalNullable();

            if (value.HasValue && value.Value > 0)
                return true;

            return false;
        }

        protected SkipType IsOneLinkLocate()
        {
            RI.FastPath("LP22I*");
            if (AccountIdentifer.Length == 9)
                RI.PutText(4, 33, AccountIdentifer, ReflectionInterface.Key.Enter, true);
            else
            {
                RI.PutText(4, 33, " ", true);
                RI.PutText(6, 33, AccountIdentifer, ReflectionInterface.Key.Enter, true);
            }

            if (!RI.CheckForText(1, 62, "PERSON DEMOGRAPHICS"))
                return SkipType.None;

            if (RI.CheckForText(10, 57, "Y") && RI.CheckForText(10, 72, DateTime.Now.ToString("MMddyyyy")) || RI.CheckForText(13, 38, "Y") && RI.CheckForText(13, 44, DateTime.Now.ToString("MMddyyyy")))
            {
                bool validAddress = RI.GetText(10, 57, 1) == "Y";
                DateTime? addressDate = RI.GetText(10, 72, 8).ToDateNullable();
                bool validPhone = RI.GetText(13, 38, 1) == "Y";
                DateTime? phoneDate = RI.GetText(13, 44, 8).ToDateNullable();

                if (!IsAddressValid)
                    IsAddressValid = validAddress;

                if (!IsPhoneValid)
                    IsPhoneValid = validPhone;

                SkipType hisType = CheckLp2jHistory();

                if (((validAddress && addressDate.HasValue && addressDate.Value.Date == DateTime.Now.Date) && (hisType == SkipType.B || hisType == SkipType.A)) &&
                    ((validPhone && phoneDate.HasValue && phoneDate.Value.Date == DateTime.Now.Date) && (hisType == SkipType.B || hisType == SkipType.P)))
                    return SkipType.B;
                else if (((validAddress && addressDate.HasValue && addressDate.Value.Date == DateTime.Now.Date) && (hisType == SkipType.B || hisType == SkipType.A)))
                    return SkipType.A;
                else if (((validPhone && phoneDate.HasValue && phoneDate.Value.Date == DateTime.Now.Date) && (hisType == SkipType.B || hisType == SkipType.P)))
                    return SkipType.P;
                else
                    return SkipType.None;
            }
            else
                return SkipType.None;

        }

        /// <summary>
        /// This method assumes that the current screen is LPP22
        /// </summary>
        private SkipType CheckLp2jHistory()
        {
            if (RI.CheckForText(23, 20, "SET2"))
                RI.Hit(ReflectionInterface.Key.F2);

            RI.Hit(ReflectionInterface.Key.F5);
            RI.PutText(7, 51, "X", ReflectionInterface.Key.Enter, true);

            string previousPhoneStatus = string.Empty;
            string previousAddressStatus = string.Empty;
            for (int row = 7; !RI.CheckForText(22, 3, "46004") && RI.CheckForText(row, 34, DateTime.Now.ToString("MMddyyyy")); row++)
            {
                if (previousAddressStatus.IsPopulated() && previousPhoneStatus.IsPopulated())
                    break;
                if (row > 20 || RI.CheckForText(7, 3, " "))
                {
                    RI.Hit(ReflectionInterface.Key.F8);
                    row = 6;
                    continue;
                }

                RI.PutText(21, 13, RI.GetText(row, 2, 2), ReflectionInterface.Key.Enter, true);

                if (!RI.CheckForText(5, 80, " ") && previousAddressStatus.IsNullOrEmpty())
                    previousAddressStatus = RI.GetText(5, 80, 1);

                if (!RI.CheckForText(8, 38, " ") && previousPhoneStatus.IsNullOrEmpty())
                    previousPhoneStatus = RI.GetText(8, 38, 1);

                RI.Hit(ReflectionInterface.Key.F12);
            }

            RI.Hit(ReflectionInterface.Key.F12);//GO back to LP22

            if (previousPhoneStatus.IsPopulated() && previousAddressStatus.IsPopulated())
                return SkipType.B;
            else if (previousAddressStatus.IsPopulated())
                return SkipType.A;
            else if (previousPhoneStatus.IsPopulated())
                return SkipType.P;
            else
                return SkipType.None;
        }

        protected SkipType IsCompassLocate()
        {
            bool isAddressLocate = false;
            bool isPhoneLocate = false;

            RI.FastPath("TX3Z/CTX1JB*");
            if (AccountIdentifer.Length == 9)
                RI.PutText(6, 16, AccountIdentifer, ReflectionInterface.Key.Enter);
            else
            {
                RI.PutText(6, 16, " ");
                RI.PutText(6, 61, AccountIdentifer, ReflectionInterface.Key.Enter);
            }

            if (!RI.CheckForText(1, 71, "TXX1R"))
                return SkipType.None;

            if (RI.CheckForText(11, 55, "Y") && RI.GetText(10, 32, 8).ToDate().Date == DateTime.Now.Date)
            {
                RI.Hit(ReflectionInterface.Key.F6, 2);
                RI.Hit(ReflectionInterface.Key.F8);
                if (RI.CheckForText(11, 55, "N"))
                {
                    isAddressLocate = true;
                    RI.Hit(ReflectionInterface.Key.F7);
                }
            }
            else
                RI.Hit(ReflectionInterface.Key.F6, 2);

            if (RI.CheckForText(17, 54, "Y") && RI.GetText(16, 45, 8).ToDate().Date == DateTime.Now.Date)
            {
                RI.Hit(ReflectionInterface.Key.F6);
                RI.Hit(ReflectionInterface.Key.F8);
                if (RI.CheckForText(17, 54, "N"))
                    isPhoneLocate = true;
            }

            if (!IsAddressValid)
                IsAddressValid = RI.CheckForText(11, 55, "Y");

            if (!IsPhoneValid)
                IsPhoneValid = RI.CheckForText(17, 54, "Y");

            if (isAddressLocate && isPhoneLocate)
                return SkipType.B;
            else if (isAddressLocate)
                return SkipType.A;
            else if (isPhoneLocate)
                return SkipType.P;
            else
                return SkipType.None;
        }

        protected void ProcessCompassLocate(LocateTypes type, SkipType skipType, List<string> queues)
        {
            AddCompassComment(type, skipType);
            if (IsAddressValid && IsPhoneValid)
            {
                if (AccountIdentifer.Length != 9)
                    AccountIdentifer = RI.GetDemographicsFromTx1j(AccountIdentifer).Ssn;

                RI.FastPath("TX3Z/ITX6T" + AccountIdentifer);
                if (RI.CheckForText(1, 74, "TXX6V"))
                {
                    int row = 7;
                    while (RI.MessageCode != "01020")
                    {
                        string queueId = RI.GetText(row, 8, 2);
                        if (queueId.IsNullOrEmpty())
                        {
                            string message = string.Format("There was not a queue found that matches the queues in the database so there was nothing to work for borrower: {0}.", AccountIdentifer);
                            Dialog.Info.Ok(message);
                            ProcessLogger.AddNotification(LogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                            return;   
                        }
                        if (queues.Contains(queueId))
                            CloseCompassQueue(row, queueId);
                        else
                        {
                            if (row == 22)
                            {
                                RI.Hit(ReflectionInterface.Key.F8);
                                row = 7;
                                if (RI.MessageCode == "90007")
                                    break;
                            }
                            else
                                row += 5;

                            continue;
                        }

                        RI.FastPath("TX3Z/ITX6T" + AccountIdentifer);
                    }
                }
            }

            RI.FastPath("TX3Z/CTX1J;" + AccountIdentifer);//UNDONE not sure if this is needed.
        }

        private void CloseCompassQueue(int row, string queueId)
        {
            //Select the task to put it in working status.
            RI.PutText(21, 18, RI.GetText(row, 2, 2), ReflectionInterface.Key.Enter);
            //Go back and get into the task's status update screen.
            RI.FastPath("TX3Z/ITX6T" + AccountIdentifer);
            RI.PutText(21, 18, RI.GetText(row, 2, 2), ReflectionInterface.Key.F2);
            //Complete the task.
            RI.PutText(8, 19, "C");
            RI.PutText(9, 19, "COMPL", ReflectionInterface.Key.Enter);

            if (RI.MessageCode != "01005")
                RI.Atd22AllLoans(AccountIdentifer, "LSERR", string.Format("Error canceling queue task {0}", queueId), string.Empty, Program.ScriptId, false);
        }

        private void AddCompassComment(LocateTypes type, SkipType skipType)
        {
            string arc = "SFNDM";
            string comment = string.Format("Prev Skip Type: {0} Locate Type: {1}", skipType.ToString(), type.ToString());
            bool success = RI.Atd22AllLoans(AccountIdentifer, arc, comment, string.Empty, Program.ScriptId, false);
            if (!success && RI.MessageCode == "50108")
                success = RI.Atd37FirstLoan(AccountIdentifer, arc, comment, Program.ScriptId, "");
            if (!success)
                RI.Atd22AllLoans(AccountIdentifer, "LSERR", "Error adding SFNDM.", string.Empty, Program.ScriptId, false);
        }

        protected void ProcessOneLinkLocate(LocateTypes type, SkipType skipType, List<string> queues)
        {
            if (!Dialog.Def.YesNo(string.Format("The borrower was located by {0}.  Is this correct?", type.ToString()), "Confirm Selection"))
                return;

            if (AccountIdentifer.Length != 9)
                AccountIdentifer = RI.GetDemographicsFromLP22(AccountIdentifer).Ssn;

            string comment = string.Format("Address Valid: {0} Phone Valid: {1} Prev Skip Type: {2} Locate Type: {3}", skipType.IsIn(SkipType.B, SkipType.A) ? "Y" : "N", skipType.IsIn(SkipType.B, SkipType.P) ? "Y" : "N", skipType.ToString(), type.ToString());
            if (!RI.AddCommentInLP50(AccountIdentifer, "AM", "36", "SFNDM", comment, Program.ScriptId))
                RI.AddQueueTaskInLP9O(AccountIdentifer, "LOCATERR", null, "Error adding comment.");

            if (IsPhoneValid && IsAddressValid)
            {
                RI.FastPath("LP8YCSKP;;;" + AccountIdentifer);
                if (RI.CheckForText(1, 64, "QUEUE TASK DETAIL"))
                {
                    int row = 7;
                    do
                    {
                        if (RI.CheckForText(row, 33, "C", "X"))
                        {
                            //There are no more active tasks. Don't waste time looking at the completed and closed ones.
                            break;
                        }
                        if (RI.CheckForText(row, 33, "A") && queues.Contains(RI.GetText(row, 65, 8)))
                        {
                            //Cancel the task.
                            string queueId = RI.GetText(row, 65, 8);
                            RI.PutText(row, 33, "X", ReflectionInterface.Key.F6);
                            if (!RI.CheckForText(22, 3, "49000 DATA SUCCESSFULLY UPDATED"))
                            {
                                //Add a queue task.
                                comment = string.Format("Error canceling {0} queue task.", queueId);
                                RI.AddQueueTaskInLP9O(AccountIdentifer, "LOCATERR", null, comment);
                                //Return to LP8YC and look for more tasks to close, starting after the row that just failed..
                                RI.FastPath("LP8YCSKP;;;" + AccountIdentifer);
                                row += 1;
                            }
                        }
                        else
                        {
                            row += 1;
                            if (RI.CheckForText(row, 33, " "))
                            {
                                //If we've gone through the whole page and failed to close any tasks, stop the madness.
                                break;
                            }
                        }
                    } while (!RI.CheckForText(22, 3, "46004"));
                }
            }

            RI.FastPath("LP22I" + AccountIdentifer);
        }
    }
}
