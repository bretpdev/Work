using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;

namespace DEMUPDTFED
{
    public class DemographicUpdate : FedBatchScript
    {
        private enum Outcome
        {
            Completed,
            Reassigned,
            Error
        }

        [Flags]
        private enum SkipType
        {
            None = 0,
            Address = 1,
            Phone = 2
        }

        private readonly DataAccess _dataAccess;
        //Use a class-level ErrorReport<T> object until the built-in one switches over to taking a type parameter.
        //private readonly ErrorReport<ErrorRecord> _err;
        //A custom EndOfJobReport object is needed that accounts for all of the queues in the database.
        //private EndOfJobReport _eoj;
        //A custom Recovery object is needed to allow multiple users to run the script concurrently.
        private CustomRecovery _recovery;
        private string _userId;

        public DemographicUpdate(ReflectionInterface ri)
            : base(ri, "DEMUPDTFED", "ERR_BU01", "EOJ_BU01", new string[] { })
        {
            _dataAccess = new DataAccess();
            //_err = new ErrorReport<ErrorRecord>("Demographic Update FED", "ERR_BU01");
        }

        public override void Main()
        {
            StartupMessage("This script updates demographics from queue tasks. Click OK to continue, or Cancel to quit.");

            List<string> queues = _dataAccess.GetQueues();

            if (DataAccessHelper.TestMode)
            {
                using (QueueChooser chooser = new QueueChooser(queues))
                {
                    chooser.ShowDialog();
                    queues = chooser.SelectedQueues;
                }
            }

            //Initialize class-level objects that need data from the system.
            _userId = UserId;
            Eoj = new EndOfJobReport( "Demographic Update FED", "EOJ_BU01", GetEojKeys(queues));
            _recovery = new CustomRecovery(ScriptId, _userId);

            //Recover if needed.
            if (_recovery.Exists) { WorkQueue(_recovery.Queue); }

            //Work the queues.
            foreach (string queue in queues)
            {
                WorkQueue(queue);
            }

            Err.Publish();
            Eoj.Publish();
            ProcessingComplete();
        }

        private List<int> GetEndorserLoans(string borrowerSsn, string endorserSsn)
        {
            RI.FastPath("TX3Z/ITX1JB" + borrowerSsn);
            RI.Hit(Key.F2);
            RI.Hit(Key.F4);
            List<int> loans = new List<int>();

            for (int row = 10; RI.MessageCode != "90007"; row++)
            {
                if (row > 21 || RI.GetText(row, 3, 1).IsNullOrEmpty())
                {
                    RI.Hit(Key.F8);
                    row = 9;
                    continue;
                }

                if (RI.CheckForText(row, 5, "E") && RI.CheckForText(row, 13, endorserSsn))
                    loans.Add(RI.GetText(row, 9, 3).ToInt());
            }

            return loans;
        }

        private bool AddArc(QueueTask task, string rejectReason, Demographics.Type type)
        {
            string arc = _dataAccess.GetArc(task.Queue, type, rejectReason);
            string comment = string.Format(_dataAccess.GetComment(rejectReason), task.Demographics.Source);
            comment += " " + task.Demographics.CommaDelimitedLine;
            string recipientId = (task.RecipientId == task.SSN ? "" : task.RecipientId);
            bool results;
            if (task.RecipientRelationship == QueueTask.Relationship.Borrower)
                results = Atd22ByBalance(task.SSN, arc, comment, task.SSN, ScriptId, false);
            else
            {
                List<int> loans = GetEndorserLoans(task.SSN, recipientId);
                if (loans.Count != 0)
                    results = Atd22ByLoan(task.SSN, arc, comment, recipientId, loans, ScriptId, false);
                else
                    results = Atd22ByBalance(task.SSN, arc, comment, recipientId, ScriptId, false);
            }
            if (results)
            {
                return true;
            }
            else
            {
                ReassignTask(task, RI.GetText(23, 2, 75).Trim(), false);
                return false;
            }
        }

        private bool CloseTask(QueueTask task)
        {
            RI.FastPath("TX3Z/ITX6X" + task.Queue.Insert(2, ";"));
            if (!RI.CheckForText(8, 75, "W")) { NotifyAndEnd("{0},{1},{2},{3}", task.AccountNumber, task.Queue, "Where's my queue task? I had one open on TX6X, but now it's gone.", task.Comment); }
            RI.PutText(21, 18, "1", Key.F2);
            RI.PutText(8, 19, "C"); //TASK STATUS
            RI.PutText(9, 19, "COMPL"); //ACTION RESPONSE
            RI.Hit(Key.Enter);
            if (RI.CheckForText(23, 2, "01644"))
            {
                RI.PutText(9, 19, "", true);
                RI.Hit(Key.Enter);
            }
            return RI.CheckForText(23, 2, "01005");
        }

        /// <summary>
        /// Checks whether the incoming address looks the same as any addresses in the borrower's system history.
        /// </summary>
        /// <returns>One of the string constants from the RejectReason class.</returns>
        private string CompareAddressHistory(QueueTask task)
        {
            RI.FastPath("TX3Z/ITX1J;" + task.RecipientId);
            if (RI.CheckForText(23, 2, "01019")) { NotifyAndEnd("{0},{1},{2},{3}", task.AccountNumber, task.Queue, RI.GetText(23, 2, 78), task.Comment); }
            int addressTypeColumn = (RI.CheckForText(1, 71, "TXX1R-01") ? 14 : 13);
            RI.Hit(Key.F6);
            RI.Hit(Key.F6);
            if (!RI.CheckForText(10, addressTypeColumn, "L")) { RI.PutText(10, addressTypeColumn, "L", Key.Enter); }
            RI.Hit(Key.F8);
            while (!RI.CheckForText(23, 2, "90007 NO MORE DATA TO DISPLAY"))
            {
                Demographics historyDemos = new Demographics();
                historyDemos.Street1 = RI.GetText(11, 10, 30).Trim('_');
                historyDemos.Street2 = RI.GetText(12, 10, 30).Trim('_');
                historyDemos.City = RI.GetText(14, 8, 20).Trim('_');
                historyDemos.State = RI.GetText(14, 32, 2).Trim('_');
                historyDemos.Zip = RI.GetText(14, 40, 5).Trim('_');
                if (historyDemos.AddressEquals(task.Demographics))
                {
                    if (RI.CheckForText(11, 55, "Y"))
                    {
                        return RejectReason.InHistoryValid;
                    }
                    else
                    {
                        string lastVerifiedString = RI.GetText(10, 32, 8).Replace(' ', '/');
                        DateTime lastVerifiedDate;
                        if (DateTime.TryParse(lastVerifiedString, out lastVerifiedDate) && (DateTime.Today - lastVerifiedDate).Days <= 365)
                        { return RejectReason.InHistoryWithin1Year; }
                        else
                        { return RejectReason.InHistoryMoreThan1Year; }
                    }
                }
                else
                {
                    RI.Hit(Key.F8);
                }
            }
            return RejectReason.NotInHistory;
        }

        /// <summary>
        /// Checks whether the incoming address looks the same as the borrower's current address in the system.
        /// </summary>
        /// <returns>The RejectReason.Match string constant if they appear the same, or null if not.</returns>
        private string CompareCurrentAddress(QueueTask task)
        {
            RI.FastPath("TX3Z/ITX1J;" + task.RecipientId);
            if (RI.CheckForText(23, 2, "01019")) { NotifyAndEnd("{0},{1},{2},{3}", task.AccountNumber, task.Queue, RI.GetText(23, 2, 78), task.Comment); }
            int addressTypeColumn = (RI.CheckForText(1, 71, "TXX1R-01") ? 14 : 13);
            if (!RI.CheckForText(10, addressTypeColumn, "L"))
            {
                RI.Hit(Key.F6);
                RI.Hit(Key.F6);
                RI.PutText(10, addressTypeColumn, "L", Key.Enter);
            }
            Demographics currentDemos = new Demographics();
            currentDemos.Street1 = RI.GetText(11, 10, 30).Trim('_');
            currentDemos.Street2 = RI.GetText(12, 10, 30).Trim('_');
            currentDemos.City = RI.GetText(14, 8, 20).Trim('_');
            currentDemos.State = RI.GetText(14, 32, 2).Trim('_');
            currentDemos.Zip = RI.GetText(14, 40, 5).Trim('_');
            if (currentDemos.AddressEquals(task.Demographics))
            {
                return RejectReason.Match;
            }
            else if (RI.CheckForText(11, 55, "N")) //ADDR VLD
            {
                return null;
            }
            else
            {
                string lastVerifiedString = RI.GetText(10, 32, 8).Replace(' ', '/');
                DateTime lastVerifiedDate;
                if (DateTime.TryParse(lastVerifiedString, out lastVerifiedDate) && (DateTime.Today - lastVerifiedDate).Days <= 15)
                { return RejectReason.NoMatchRecentUpdate; }
                else
                { return null; }
            }
        }

        /// <summary>
        /// Checks whether the incoming phone number matches any of the borrower's current phone numbers in the system.
        /// </summary>
        /// <returns>The RejectReason.Match string constant if a match is found, or null if not.</returns>
        private string CompareCurrentPhone(QueueTask task, string incomingPhoneNumber)
        {
            RI.FastPath("TX3Z/ITX1J;" + task.RecipientId);
            if (RI.CheckForText(23, 2, "01019")) { NotifyAndEnd("{0},{1},{2},{3}", task.AccountNumber, task.Queue, RI.GetText(23, 2, 78), task.Comment); }
            RI.Hit(Key.F6);
            RI.Hit(Key.F6);
            RI.Hit(Key.F6);
            RI.PutText(16, 14, "H", Key.Enter);
            string homePhone = RI.GetText(17, 14, 3) + RI.GetText(17, 23, 3) + RI.GetText(17, 31, 4);
            if (homePhone == incomingPhoneNumber) { return RejectReason.Match; }
            if (CheckForRecentUpdate())
                return RejectReason.NoMatchRecentUpdate;
            RI.PutText(16, 14, "A", Key.Enter);
            string alternatePhone = RI.GetText(17, 14, 3) + RI.GetText(17, 23, 3) + RI.GetText(17, 31, 4);
            if (alternatePhone == incomingPhoneNumber) { return RejectReason.Match; }
            if (CheckForRecentUpdate())
                return RejectReason.NoMatchRecentUpdate;

            return null;
        }

        private bool CheckForRecentUpdate()
        {
            if (RI.GetText(16, 45, 2) != "__")
            {
                DateTime verifiedDate = DateTime.Parse(RI.GetText(16, 45, 8).Replace(" ", "/"));
                if (RI.GetText(17, 54, 1) == "Y")
                {
                    return DateTime.Now.Subtract(verifiedDate).Days < 15;
                }
                else
                    return false;
            }
            else
                return false;
        }

        /// <summary>
        /// Checks whether the incoming phone number matches any phone numbers in the borrower's system history.
        /// </summary>
        /// <returns>One of the string constants from the RejectReason class.</returns>
        private string ComparePhoneHistory(QueueTask task, string incomingPhoneNumber)
        {
            RI.FastPath("TX3Z/ITX1J;" + task.RecipientId);
            if (RI.CheckForText(23, 2, "01019")) { NotifyAndEnd("{0},{1},{2},{3}", task.AccountNumber, task.Queue, RI.GetText(23, 2, 78), task.Comment); }
            //Bring up the correct phone type.
            RI.Hit(Key.F6);
            RI.Hit(Key.F6);
            RI.Hit(Key.F6);
            string phoneType = (incomingPhoneNumber == task.Demographics.HomePhone ? "H" : "A");
            if (!RI.CheckForText(16, 14, phoneType)) { RI.PutText(16, 14, phoneType, Key.Enter); }
            RI.Hit(Key.F8);
            while (!RI.CheckForText(23, 2, "90007 NO MORE DATA TO DISPLAY"))
            {
                string historicalPhoneNumber = RI.GetText(17, 14, 3) + RI.GetText(17, 23, 3) + RI.GetText(17, 31, 4);
                if (historicalPhoneNumber == incomingPhoneNumber)
                {
                    if (RI.CheckForText(17, 54, "Y"))
                    {
                        return RejectReason.InHistoryValid;
                    }
                    else
                    {
                        string lastVerifiedString = RI.GetText(16, 45, 8).Replace(' ', '/');
                        DateTime lastVerifiedDate;
                        if (DateTime.TryParse(lastVerifiedString, out lastVerifiedDate) && (DateTime.Today - lastVerifiedDate).Days <= 365)
                        { return RejectReason.InHistoryWithin1Year; }
                        else
                        { return RejectReason.InHistoryMoreThan1Year; }
                    }
                }
                else
                {
                    RI.Hit(Key.F8);
                }
            }
            return RejectReason.NotInHistory;
        }

        private bool CurrentDemographicsAreForeign(QueueTask task)
        {
            RI.FastPath("TX3Z/ITX1J;" + task.RecipientId);
            if (RI.CheckForText(23, 2, "01019")) { NotifyAndEnd("{0},{1},{2},{3}", task.AccountNumber, task.Queue, RI.GetText(23, 2, 78), task.Comment); }
            if (!RI.CheckForText(13, 52, "__")) { return true; } //FGN CNY
            if (!RI.CheckForText(12, 77, "__")) { return true; } //FGN CDE
            if (!RI.CheckForText(18, 15, "__")) { return true; } //FGN PHN: CNY
            if (!RI.CheckForText(18, 24, "__")) { return true; } //FGN PHN: CTY
            if (!RI.CheckForText(18, 36, "__")) { return true; } //FGN PHN: LCL
            return false;
        }

        private string GetEojKey(string queue, Outcome taskOutcome)
        {
            switch (taskOutcome)
            {
                case Outcome.Completed:
                    return string.Format("Total number of {0} queue tasks completed", queue);
                case Outcome.Reassigned:
                    return string.Format("Total number of {0} queue tasks reassigned", queue);
                case Outcome.Error:
                    return string.Format("Total number of {0} errors", queue);
                default:
                    Debug.Assert(false, string.Format("Unknown enum value \"{0}\"", taskOutcome));
                    return "";
            }
        }

        private List<string> GetEojKeys(IEnumerable<string> queues)
        {
            List<string> keys = new List<string>();
            foreach (Outcome outcome in new Outcome[] { Outcome.Completed, Outcome.Reassigned, Outcome.Error })
            {
                foreach (string queue in queues)
                {
                    keys.Add(GetEojKey(queue, outcome));
                }
            }
            return keys;
        }

        /// <summary>
        /// Checks the demographics screen to see if the borrower's address and/or phone changed from invalid to valid today.
        /// </summary>
        /// <returns>One or more SkipType flags.</returns>
        private SkipType LocateSkipType(string ssn)
        {
            SkipType skips = SkipType.None;
            RI.FastPath("TX3Z/ITX1JB;" + ssn);
            int f6Hits = 0;
            DateTime currentAddressDate;
            if (RI.CheckForText(11, 55, "Y") && DateTime.TryParse(RI.GetText(10, 32, 8).Replace(' ', '/'), out currentAddressDate) && currentAddressDate == DateTime.Today)
            {
                for (; f6Hits < 2; f6Hits++) { RI.Hit(Key.F6); }
                RI.Hit(Key.F8);
                if (RI.CheckForText(11, 55, "N")) { skips |= SkipType.Address; }
            }
            DateTime currentPhoneDate;
            if (RI.CheckForText(17, 54, "Y") && DateTime.TryParse(RI.GetText(16, 45, 8).Replace(' ', '/'), out currentPhoneDate) && currentPhoneDate == DateTime.Today)
            {
                for (; f6Hits < 3; f6Hits++) { RI.Hit(Key.F6); }
                RI.Hit(Key.F8);
                if (RI.CheckForText(17, 54, "N")) { skips |= SkipType.Phone; }
            }
            return skips;
        }

        private void ProcessLocate(QueueTask task)
        {
            SkipType skips = LocateSkipType(task.SSN);
            if (skips != SkipType.None)
            {
                string skipString = "";
                switch (skips)
                {
                    case SkipType.Address | SkipType.Phone:
                        skipString = "B";
                        break;
                    case SkipType.Address:
                        skipString = "A";
                        break;
                    case SkipType.Phone:
                        skipString = "P";
                        break;
                }
                Locate locate = _dataAccess.GetLocate(task.Demographics.Source);
                string arc = _dataAccess.GetArc(task.Queue, Demographics.Type.Address, RejectReason.Locate);
                string comment = string.Format(_dataAccess.GetComment(RejectReason.Locate), skipString, locate.Type, locate.Description);
                Atd22ByBalance(task.SSN, arc, comment, "", ScriptId, false);
            }
        }

        private QueueTask ReadQueueTask(string queue)
        {
            //This method must be called from ITX6X, so check the screen ID.
            Debug.Assert(RI.CheckForText(1, 74, "TXX71"), "Must be on ITX6X to read a queue task.");

            //Select the first queue task.
            string ssn = RI.GetText(8, 6, 9);
            string comment = (RI.GetText(9, 2, 77) + RI.GetText(10, 2, 77)).Trim('_').Trim();
            RI.PutText(21, 18, "1", Key.Enter);

            //If recipientID and relationship are auto filled, clear them out.
            if (RI.CheckForText(23, 2, "01507"))
            {
                RI.PutText(10, 41, "", true);
                RI.PutText(10, 45, "", true);
                RI.PutText(10, 48, "", true);
                RI.PutText(12, 41, "", Key.Enter, true);
            }

            //Check for concurrency problems (i.e., someone else is working the same queue and selected the first task before we did).
            while (RI.CheckForText(23, 2, "01029 RECORD UPDATED OR DELETED BY ANOTHER USER"))
            {
                RI.Hit(Key.F5);
                ssn = RI.GetText(8, 6, 9);
                comment = (RI.GetText(9, 2, 77) + RI.GetText(10, 2, 77)).Trim('_').Trim();
                RI.PutText(21, 18, "1", Key.Enter);
            }
            //Update recovery as soon as a task is open.
            if (_recovery.LastAction == CustomRecovery.Action.None)
            {
                _recovery.Queue = queue;
                _recovery.LastAction = CustomRecovery.Action.OpenedTask;
            }
            //Make sure we got into the task details. (I've seen this fail for accounts that have no active loans.)
            if (RI.CheckForText(1, 72, "TDX0M"))
            {
                //Finish getting the rest of the details.
                string recipientId = RI.GetText(5, 16, 11).Replace(" ", "");
                string relationshipIndicator = RI.GetText(6, 16, 1);
                string accountNumber = (recipientId.StartsWith("P") ? recipientId : GetDemographicsFromTx1j(recipientId).AccountNumber.Replace(" ", ""));
                return new QueueTask(queue, ssn, accountNumber, recipientId, relationshipIndicator, comment);
            }
            else
            {
                //Get the account number because we need it, but leave the recipient ID blank as a flag to the calling method.
                string accountNumber = GetDemographicsFromTx1j(ssn).AccountNumber.Replace(" ", "");
                return new QueueTask(queue, ssn, accountNumber, "", "", comment);
            }
        }

        private void ReassignTask(QueueTask task, string reason, bool acceptedDemographics)
        {
            if (_recovery.LastAction != CustomRecovery.Action.ReassignedTask)
            {
                RI.FastPath("TX3Z/CTX6J;");
                RI.PutText(7, 42, task.Queue);
                RI.PutText(13, 42, _userId);
                RI.Hit(Key.Enter);
                RI.PutText(8, 15, _dataAccess.GetLoanServicingManagerId());
                RI.Hit(Key.Enter);
                if (!RI.CheckForText(23, 2, "01005 RECORD SUCCESSFULLY CHANGED")) { NotifyAndEnd("{0},{1},{2},{3}", task.AccountNumber, task.Queue, "could not reassign task", task.Comment); }
                _recovery.LastAction = CustomRecovery.Action.ReassignedTask;
            }

            //Add a general comment for why the task was reassigned.
            RI.FastPath("TX3Z/ATC00" + task.SSN);
            RI.PutText(19, 38, "4", Key.Enter);
            RI.PutText(18, 2, string.Format("task assigned to manager, Recipient Id: {0}, {1}, {2}", task.RecipientId, reason, task.Comment), Key.Enter);
            if (RI.CheckForText(23, 2, "01004 RECORD SUCCESSFULLY ADDED"))
            {
                Eoj.Counts[GetEojKey(task.Queue, Outcome.Reassigned)].Increment();
            }
            else
            {
                Err.AddRecord("task assigned to manager, " + reason, new ErrorRecord(task, acceptedDemographics));
                Eoj.Counts[GetEojKey(task.Queue, Outcome.Error)].Increment();
            }
        }

        /// <summary>
        /// Reviews the incoming address for formatting problems, and checks whether is has been previously blocked in the system.
        /// </summary>
        /// <returns>One of the string constants from the RejectReason class, or null if no problems are found.</returns>
        private string ReviewAddress(QueueTask task)
        {
            if (string.IsNullOrEmpty(task.Demographics.Street1)) { return RejectReason.InvalidAddress; }
            if (string.IsNullOrEmpty(task.Demographics.City) || string.IsNullOrEmpty(task.Demographics.State) || string.IsNullOrEmpty(task.Demographics.Zip)) { return RejectReason.IncompleteAddress; }
            if (System.Text.RegularExpressions.Regex.IsMatch(task.Demographics.City, "[^a-zA-Z ]") || System.Text.RegularExpressions.Regex.IsMatch(task.Demographics.State, "[^a-zA-Z]")) { return RejectReason.IncompleteAddress; }
            if (task.Demographics.State.Length < 2) { return RejectReason.IncompleteAddress; }
            if (!string.IsNullOrEmpty(task.Demographics.ForeignCountry)) { return RejectReason.IncomingForeignDemos; }
            if (task.Demographics.State.ToUpper() == "FC") { return RejectReason.IncomingForeignDemos; }
            string[] illegalAddresses = new string[] { "GENERAL DELIVERY", "TEMP AWAY", "TEMPORARILY AWAY" };
            if (illegalAddresses.Contains(task.Demographics.Street1) || illegalAddresses.Contains(task.Demographics.Street2.ToUpper())) { return RejectReason.InvalidAddress; }
            if (task.Demographics.Zip.Length < 5) { return RejectReason.IncompleteAddress; }
            if (System.Text.RegularExpressions.Regex.IsMatch(task.Demographics.Zip, @"[^\d]")) { return RejectReason.InvalidAddress; }
            if (SkipExistsForAddress(task)) { return RejectReason.BlockedAddress; }
            return null;
        }

        /// <summary>
        /// Reviews the incoming phone number for formatting problems, and checks whether is has been previously blocked in the system.
        /// </summary>
        /// <returns>One of the string constants from the RejectReason class, or null if no problems are found.</returns>
        private string ReviewPhone(QueueTask task, string incomingPhoneNumber)
        {
            if (string.IsNullOrEmpty(incomingPhoneNumber) || incomingPhoneNumber.Length < 10) { return RejectReason.IncompletePhone; }
            if (incomingPhoneNumber.EndsWith("5551212")) { return RejectReason.InvalidPhone; }
            if (System.Text.RegularExpressions.Regex.IsMatch(incomingPhoneNumber, @"[^\d]")) { return RejectReason.InvalidPhone; }
            if (incomingPhoneNumber.Length > 10) { return RejectReason.IncomingForeignDemos; }
            if (incomingPhoneNumber.StartsWith("011")) { return RejectReason.IncomingForeignDemos; }
            if (SkipExistsForPhone(task, incomingPhoneNumber)) { return RejectReason.BlockedPhone; }
            return null;
        }

        private string SanitizeAddress(string address)
        {
            string sanitized = address.ToUpper();
            sanitized = System.Text.RegularExpressions.Regex.Replace(sanitized, @"[^A-Z0-9 \\/]", "", RegexOptions.Compiled);
            sanitized = sanitized.Replace("STREET", "ST");
            sanitized = sanitized.Replace("AVENUE", "AVE");
            sanitized = sanitized.Replace("ROAD", "RD");
            sanitized = sanitized.Replace("LANE", "LN");
            sanitized = sanitized.Replace("DRIVE", "DR");
            sanitized = sanitized.Replace("HIGHWAY", "HWY");
            sanitized = sanitized.Replace("P O BOX", "PO BOX");
            sanitized = sanitized.Replace("P O BX", "PO BOX");
            sanitized = sanitized.Replace("FLOOR", "FL");
            return sanitized;
        }

        private bool SkipExistsForAddress(QueueTask task)
        {
            RI.FastPath("TX3Z/ITD2A" + task.SSN);
            RI.PutText(11, 65, "K0ADD", Key.Enter);
            if (RI.CheckForText(23, 2, "01019 ENTERED KEY NOT FOUND")) { return false; }
            if (RI.CheckForText(1, 72, "TDX2C")) { RI.PutText(5, 14, "X", Key.Enter); } //Selection screen; select all.
            while (!RI.CheckForText(23, 2, "90007 NO MORE DATA TO DISPLAY"))
            {
                string recipientId = RI.GetText(13, 51, 9);
                if (recipientId == task.SSN || recipientId == task.RecipientId)
                {
                    //Get the address from the comment text and compare it to the one from our queue task.
                    string skipAddress = (RI.GetText(17, 2, 79) + RI.GetText(18, 2, 79)).Trim();
                    string[] skipFields = skipAddress.Split(',');
                    if (skipFields.Length >= 6)
                    {
                        Demographics skipDemos = new Demographics();
                        skipDemos.Street1 = skipFields[0];
                        skipDemos.Street2 = skipFields[1];
                        skipDemos.City = skipFields[2];
                        skipDemos.State = skipFields[3];
                        skipDemos.Zip = skipFields[4].SafeSubString(0, 5);
                        skipDemos.ForeignCountry = skipFields[5];
                        if (skipDemos.AddressEquals(task.Demographics)) { return true; }
                    }
                }
                RI.Hit(Key.F8);
            }
            return false;
        }

        private bool SkipExistsForPhone(QueueTask task, string incomingPhoneNumber)
        {
            RI.FastPath("TX3Z/ITD2A" + task.SSN);
            RI.PutText(11, 65, "K0PHN", Key.Enter);
            if (RI.CheckForText(23, 2, "01019 ENTERED KEY NOT FOUND")) { return false; }
            if (RI.CheckForText(1, 72, "TDX2C")) { RI.PutText(5, 14, "X", Key.Enter); } //Selection screen; select all.
            while (!RI.CheckForText(23, 2, "90007 NO MORE DATA TO DISPLAY"))
            {
                string recipientId = RI.GetText(13, 51, 9);
                if (recipientId == task.SSN || recipientId == task.RecipientId)
                {
                    //Get the phone numbers from the comment text and compare them to the one from our queue task.
                    string skipComment = (RI.GetText(17, 2, 79) + RI.GetText(18, 2, 79)).Trim();
                    IEnumerable<string> skipPhones = skipComment.Split(',').Select(p => System.Text.RegularExpressions.Regex.Replace(p, @"[^\d]", ""));
                    if (skipPhones.Contains(incomingPhoneNumber)) { return true; }
                }
                RI.Hit(Key.F8);
            }
            return false;
        }

        private bool UpdateAddress(QueueTask task)
        {
            RI.FastPath("TX3Z/CTX1J;" + task.RecipientId);
            bool isBorrowerScreen = RI.CheckForText(1, 71, "TXX1R-01");
            if (RI.CheckForText(23, 2, "01019")) { NotifyAndEnd("{0},{1},{2},{3}", task.AccountNumber, task.Queue, RI.GetText(23, 2, 78), task.Comment); }
            int addressTypeColumn = (isBorrowerScreen ? 14 : 13);
            RI.Hit(Key.F6);
            RI.Hit(Key.F6);
            if (!RI.CheckForText(10, addressTypeColumn, "L")) { RI.PutText(10, addressTypeColumn, "L", Key.Enter); }
            if (isBorrowerScreen) RI.PutText(8, 18, _dataAccess.GetDemographicsSourceCode(task.Demographics.Source));
            if (isBorrowerScreen) RI.PutText(9, 18, "", true); //3RD PARTY
            RI.PutText(10, 32, DateTime.Now.ToString("MMddyy")); //ADDR LAST VER
            RI.PutText(11, 55, "Y"); //ADDR VALID
            RI.PutText(11, 10, task.Demographics.Street1, true);
            RI.PutText(12, 10, task.Demographics.Street2, true);
            RI.PutText(14, 8, System.Text.RegularExpressions.Regex.Replace(task.Demographics.City, "[^a-zA-Z ]", "", RegexOptions.Compiled), true);
            RI.PutText(14, 32, task.Demographics.State);
            RI.PutText(14, 40, task.Demographics.Zip, true);
            RI.Hit(Key.Enter);
            return RI.CheckForText(23, 2, "01096", "01097");
        }

        private bool UpdatePhone(QueueTask task, string incomingPhoneNumber)
        {
            RI.FastPath("TX3Z/CTX1J;" + task.AccountNumber);
            if (RI.CheckForText(23, 2, "01019")) { NotifyAndEnd("{0},{1},{2},{3}", task.AccountNumber, task.Queue, RI.GetText(23, 2, 78), task.Comment); }
            RI.Hit(Key.F6);
            RI.Hit(Key.F6);
            RI.Hit(Key.F6);
            RI.PutText(16, 14, "H", Key.Enter);
            bool homePhoneIsValid = RI.CheckForText(17, 54, "Y");
            RI.PutText(16, 14, "A", Key.Enter);
            bool alternatePhoneIsValid = RI.CheckForText(17, 54, "Y");
            string phoneType;
            if (incomingPhoneNumber == task.Demographics.HomePhone)
            { phoneType = (homePhoneIsValid && !alternatePhoneIsValid ? "A" : "H"); }
            else
            { phoneType = (alternatePhoneIsValid && !homePhoneIsValid ? "H" : "A"); }
            RI.PutText(16, 14, phoneType, Key.Enter);
            RI.PutText(19, 14, _dataAccess.GetDemographicsSourceCode(task.Demographics.Source));
            RI.PutText(16, 20, "U"); //MBL
            RI.PutText(16, 30, "N"); //CONSENT
            RI.PutText(16, 45, DateTime.Now.ToString("MMddyy")); //PHN LAST VER
            RI.PutText(17, 54, "Y"); //PHN VLD
            RI.PutText(17, 14, incomingPhoneNumber);
            RI.PutText(17, 40, "", true); //EXT
            if (RI.CheckForText(17, 60, "NO PHN")) { RI.PutText(17, 67, "", true); }
            RI.Hit(Key.Enter);
            return RI.CheckForText(23, 2, "01096", "01097", "01100");
        }

        private bool WorkAddress(QueueTask task)
        {
            string[] reviewProblems = new string[] { RejectReason.InvalidAddress, RejectReason.BlockedAddress, RejectReason.IncompleteAddress, RejectReason.IncomingForeignDemos };
            string rejectReason = ReviewAddress(task);
            if (reviewProblems.Contains(rejectReason)) { return AddArc(task, rejectReason, Demographics.Type.Address); }

            if (CurrentDemographicsAreForeign(task))
            {
                ReassignTask(task, "current foreign", false);
                return false;
            }

            rejectReason = CompareCurrentAddress(task);
            if (new string[] { RejectReason.Match, RejectReason.NoMatchRecentUpdate }.Contains(rejectReason)) { return AddArc(task, rejectReason, Demographics.Type.Address); }

            rejectReason = CompareAddressHistory(task);
            if (rejectReason == RejectReason.InHistoryWithin1Year) { return AddArc(task, rejectReason, Demographics.Type.Address); }

            if (task.Demographics.Street1 != null)
                task.Demographics.Street1 = SanitizeAddress(task.Demographics.Street1);

            if (task.Demographics.Street2 != null)
                task.Demographics.Street2 = SanitizeAddress(task.Demographics.Street2);

            if (task.Demographics.Street1.Length > 29 || task.Demographics.Street2.Length > 29)
            {
                ReassignTask(task, "address too long", false);
                return false;
            }
            if (task.Demographics.City != null && task.Demographics.City.Length > 20)
            {
                ReassignTask(task, "city too long", false);
                return false;
            }

            if (UpdateAddress(task))
            {
                if (task.RecipientRelationship == QueueTask.Relationship.Borrower)
                {
                    ProcessLocate(task);
                    _recovery.LastAction = CustomRecovery.Action.ProcessedLocate;
                }

                bool didAdd = AddArc(task, rejectReason, Demographics.Type.Address);
                _recovery.LastAction = CustomRecovery.Action.UpdatedAddress;
                return didAdd;
            }
            else
            {
                ReassignTask(task, "error updating address", false);
                return false;
            }
        }

        private bool WorkPhone(QueueTask task, string incomingPhoneNumber)
        {
            string[] reviewProblems = new string[] { RejectReason.IncompletePhone, RejectReason.InvalidPhone, RejectReason.BlockedPhone, RejectReason.IncomingForeignDemos };
            string rejectReason = ReviewPhone(task, incomingPhoneNumber);
            if (reviewProblems.Contains(rejectReason)) { return AddArc(task, rejectReason, Demographics.Type.Phone); }

            if (CurrentDemographicsAreForeign(task))
            {
                ReassignTask(task, "current foreign", false);
                return false;
            }

            rejectReason = CompareCurrentPhone(task, incomingPhoneNumber);
            if (new string[] { RejectReason.Match, RejectReason.NoMatchRecentUpdate }.Contains(rejectReason)) { return AddArc(task, rejectReason, Demographics.Type.Phone); }

            rejectReason = ComparePhoneHistory(task, incomingPhoneNumber);
            if (rejectReason == RejectReason.InHistoryWithin1Year) { return AddArc(task, rejectReason, Demographics.Type.Phone); }

            if (UpdatePhone(task, incomingPhoneNumber))
            {
                if (task.RecipientRelationship == QueueTask.Relationship.Borrower)
                {
                    ProcessLocate(task);
                    _recovery.LastAction = CustomRecovery.Action.ProcessedLocate;
                }

                AddArc(task, rejectReason, Demographics.Type.Phone);
                _recovery.LastAction = (incomingPhoneNumber == task.Demographics.HomePhone ? CustomRecovery.Action.UpdatedHomePhone : CustomRecovery.Action.UpdatedOtherPhone);
                return true;
            }
            else
            {
                ReassignTask(task, "error updating phone", false);
                return false;
            }
        }

        private void WorkQueue(string queue)
        {
            //Make sure we can get to the queue and we doesn't already have a task open.
            string queuePath = string.Format("TX3Z/ITX6X{0}", queue.Insert(2, ";"));
            RI.FastPath(queuePath);
            if (RI.CheckForText(23, 2, "01020 NO RECORDS FOUND FOR ENTERED SEARCH CRITERIA"))
            { return; }
            else if (!RI.CheckForText(23, 2, "     "))
            { NotifyAndEnd("error accessing queue, {0}, {1}", queue, RI.GetText(23, 2, 78)); }
            else if (_recovery.LastAction == CustomRecovery.Action.None && RI.CheckForText(8, 75, "W"))
            { NotifyAndEnd("a queue task is already open, {0}", queue); }

            //Work all tasks in the queue.
            for (; !RI.CheckForText(23, 2, "01020 NO RECORDS FOUND FOR ENTERED SEARCH CRITERIA"); RI.FastPath(queuePath))
            {
                //Get the details from the next available queue task.
                QueueTask task = ReadQueueTask(queue);

                if (task.Demographics == null || string.IsNullOrEmpty(task.RecipientId) || _recovery.LastAction == CustomRecovery.Action.ReassignedTask)
                {
                    //Reassign the task if we can't get the details.
                    string errorMessage = (task.Demographics == null ? "parse error" : RI.GetText(23, 2, 77));
                    ReassignTask(task, errorMessage, false);
                    _recovery.Clear();
                    continue;
                }

                List<string> sourceCodes = _dataAccess.GetSourceCodes();
                bool foundSourceCode = false;
                foreach (string item in sourceCodes)
                {
                    if (item.ToUpper() == task.Demographics.Source.ToUpper())
                    {
                        //Once we find the source code we can break out of the loop.
                        foundSourceCode = true;
                        break;
                    }
                }

                if (!foundSourceCode)
                {
                    ReassignTask(task, "Invalid Source", false);
                    _recovery.Clear();
                    continue;
                }

                //Update all demographics found in the task comments.
                bool taskIsStillOpen = true;
                if (task.Demographics.HasAddress && _recovery.LastAction.CompareTo(CustomRecovery.Action.UpdatedAddress) < 0)
                {
                    taskIsStillOpen = WorkAddress(task);
                }
                if (taskIsStillOpen && task.Demographics.HasHomePhone && _recovery.LastAction.CompareTo(CustomRecovery.Action.UpdatedHomePhone) < 0)
                {
                    taskIsStillOpen = WorkPhone(task, task.Demographics.HomePhone);
                }
                if (taskIsStillOpen && task.Demographics.HasOtherPhone && _recovery.LastAction.CompareTo(CustomRecovery.Action.UpdatedOtherPhone) < 0)
                {
                    taskIsStillOpen = WorkPhone(task, task.Demographics.OtherPhone);
                }

                if (taskIsStillOpen)
                {
                    if (CloseTask(task))
                    {
                        Eoj.Counts[GetEojKey(task.Queue, Outcome.Completed)].Increment();
                    }
                    else
                    {
                        ReassignTask(task, "error closing queue task", true);
                    }
                }
                _recovery.Clear();
            }
        }

        private class ErrorRecord
        {
            public string AccountNumber { get; set; }
            public string RecipientId { get; set; }
            public string Queue { get; set; }
            public string Demographics { get; set; }
            public string AcceptedOrRejected { get; set; }

            public ErrorRecord(QueueTask task, bool acceptedDemographics)
            {
                AccountNumber = task.AccountNumber;
                RecipientId = task.RecipientId;
                Queue = task.Queue;
                Demographics = task.Comment;
                AcceptedOrRejected = (acceptedDemographics ? "accepted" : "rejected");
            }
        }
    }
}