using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Uheaa.Common.Scripts;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;
using Uheaa.Common;
using System.Reflection;
using Uheaa.Common.ProcessLogger;

namespace ACURINTR.DemographicsProcessors
{
    class DudePathAddress : ComparePathBase
    {
        public DudePathAddress(ReflectionInterface ri, string demographicsReviewQueue, string foreignDemographicsQueue, ProcessLogData logData, string userId, string scriptId)
            : base(ri, demographicsReviewQueue, foreignDemographicsQueue, logData, userId, scriptId)
        {
        }

        /// <summary>
        /// Overrides ComparePathBase.Process
        /// </summary>
        /// <param name="task"></param>
        /// <param name="queueData"></param>
        /// <param name="pauseOnQueueClosingError"></param>
        public override void Process(QueueTask task, QueueData queueData, bool pauseOnQueueClosingError)
        {
            bool borrowerExistsOnOnelink = BorrowerExistsOnOnelink(task.Demographics.Ssn);
            string actionCode = task.GetRejectActionCode(RejectAction.RejectReasons.NONE, QueueTask.DemographicType.Address);

            if (!DemographicsPassReview(queueData, task, pauseOnQueueClosingError, borrowerExistsOnOnelink))
                return;
            bool result = borrowerExistsOnOnelink ?
                UpdateOneLink(queueData, task, pauseOnQueueClosingError) :
                UpdateCompass(queueData, task);
            if (!result)
            {
                ReassignQueueTask(queueData, task, "OneLINK not successfully updated.");
                return;
            }
            string comment = string.Format("Accept demos from {0} {1}.", task.DemographicsSource, General.FormatAddressForComments(task.Demographics));
            result = borrowerExistsOnOnelink ?
                AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) :
                AddCompassComment(queueData, task, actionCode, comment);
            if (!result)
            {
                ReassignQueueTask(queueData, task, comment);
                return;
            }

            CloseOrReassignQueueTask(queueData, task, pauseOnQueueClosingError, "Error closing task.");
            ProcessLocate(task);
        }

        /// <summary>
        /// Compares a Demographic to the session.  returns true if they match.
        /// </summary>
        /// <param name="taskDemographics"></param>
        /// <param name="validityIndicator"></param>
        /// <returns>true if the demos match</returns>
        private bool AddressMatchesOneLink(AccurintRDemographics taskDemographics, string validityIndicator)
        {
            //Make sure we at least got an SSN or account number.
            if (taskDemographics.AccountNumber.IsNullOrEmpty() && taskDemographics.Ssn.IsNullOrEmpty())
            {
                throw new Exception("SSN and account number are both missing.");
            }

            //Hit up LP22 to get the borrower's legal address.
            if (taskDemographics.Ssn.IsPopulated() && taskDemographics.Ssn.Length == 9)
                RI.FastPath(string.Format("LP22I{0};;;;L", taskDemographics.Ssn));
            else
                RI.FastPath(string.Format("LP22I;;;L;;;{0}", taskDemographics.AccountNumber));

            //Compare the address in question to the borrower's legal address.
            if (!RI.CheckForText(12, 60, taskDemographics.ZipCode.SafeSubString(0, 5)))
                return false;
            if (!RI.CheckForText(12, 52, taskDemographics.State))
                return false;
            List<string> addressNumberGroups = taskDemographics.Address1.GetNumericGroups();
            List<string> onScreenNumberGroups = (RI.GetText(10, 9, 35) + RI.GetText(11, 9, 35)).GetNumericGroups();
            foreach (string addressNumber in addressNumberGroups)
            {
                if (!onScreenNumberGroups.Contains(addressNumber))
                    return false;
            }
            foreach (string onScreenNumber in onScreenNumberGroups)
            {
                if (!addressNumberGroups.Contains(onScreenNumber))
                    return false;
            }
            if (!RI.CheckForText(10, 57, validityIndicator))
                return false;

            return true;
        }

        /// <summary>
        /// compare our data with the session and leave notes if new info is found.
        /// </summary>
        /// <param name="queueData"></param>
        /// <param name="task"></param>
        /// <param name="pauseOnQueueClosingError"></param>
        /// <param name="borrowerExistsOnOnelink"></param>
        /// <returns></returns>
        protected override bool DemographicsPassReview(QueueData queueData, QueueTask task, bool pauseOnQueueClosingError, bool borrowerExistsOnOnelink = true)
        {
            //Recovery constants:
            const string COMMENT_MISSING_STREET = "Added OneLINK comment for missing street";
            const string QUEUE_MISSING_ADDRESS = "Created queue task to review missing address";
            const string COMMENT_MISSING_ADDRESS = "Added OneLINK comment for missing address";
            const string QUEUE_SHORT_STATE = "Created queue task to review one-letter state code";
            const string COMMENT_SHORT_STATE = "Added OneLINK comment for one-letter state code";
            const string QUEUE_FOREIGN_ADDRESS = "Created queue task to review foreign address";
            const string COMMENT_FOREIGN_ADDRESS = "Added OneLINK comment for foreign address";
            const string COMMENT_ADDRESS_ON_FILE = "Added OneLINK comment for address already on file";

            if (task.Demographics.FirstName.IsPopulated())
            {
                string comment = string.Format("Post Office has possible alternate name for borrower {0} {1}.", task.Demographics.FirstName, task.Demographics.LastName);
                AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, "K4AKA", comment);
            }

            //Check the address for problem conditions.
            if (task.Demographics.Address1.IsNullOrEmpty())
            {
                string rejectReason = RejectAction.RejectReasons.RETURN_ADDRESS_WITHOUT_STREET;
                string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Address);
                string comment = string.Format("{0} {1}.", rejectReason, General.FormatAddressForComments(task.Demographics));
                if (borrowerExistsOnOnelink ? AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) : AddCompassComment(queueData, task, actionCode, comment))
                {
                    CloseOrReassignQueueTask(queueData, task, pauseOnQueueClosingError, rejectReason);
                }
                else
                    ReassignQueueTask(queueData, task, rejectReason);
                return false;
            }
            if (task.Demographics.City.IsNullOrEmpty() || task.Demographics.State.IsNullOrEmpty() || task.Demographics.ZipCode.IsNullOrEmpty() || System.Text.RegularExpressions.Regex.IsMatch(task.Demographics.City, @"[^A-Za-z\s]"))
            {
                string rejectReason = RejectAction.RejectReasons.RETURN_INCOMPLETE_ADDRESS;
                if (CreateQueueTask(queueData, task, DemographicsReviewQueue, ""))
                {
                    string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Address);
                    string comment = string.Format("{0} {1}.", rejectReason, General.FormatAddressForComments(task.Demographics));
                    if (borrowerExistsOnOnelink ? AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) : AddCompassComment(queueData, task, actionCode, comment))
                    {
                        CloseOrReassignQueueTask(queueData, task, pauseOnQueueClosingError, rejectReason);
                    }
                    else
                        ReassignQueueTask(queueData, task, rejectReason);
                }
                else
                    ReassignQueueTask(queueData, task, rejectReason);
                return false;
            }
            if (task.Demographics.State.Length == 1)
            {
                string rejectReason = RejectAction.RejectReasons.RETURN_INCOMPLETE_ADDRESS;
                if (CreateQueueTask(queueData, task, DemographicsReviewQueue, ""))
                {
                    string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Address);
                    string comment = string.Format("{0} .", rejectReason, General.FormatAddressForComments(task.Demographics));
                    if (borrowerExistsOnOnelink ? AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) : AddCompassComment(queueData, task, actionCode, comment))
                    {
                        CloseOrReassignQueueTask(queueData, task, pauseOnQueueClosingError, rejectReason);
                    }
                    else
                        ReassignQueueTask(queueData, task, rejectReason);
                }
                else
                    ReassignQueueTask(queueData, task, rejectReason);
                return false;
            }
            if (task.Demographics.State.ToUpper() == "FC" || task.Demographics.Country.IsPopulated())
            {
                string rejectReason = RejectAction.RejectReasons.RETURN_FOREIGN_DEMOGRAPHICS;
                if (CreateQueueTask(queueData, task, ForeignDemographicsQueue, ""))
                {
                    string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Address);
                    string comment = string.Format("{0} {1}.", rejectReason, General.FormatAddressForComments(task.Demographics));
                    if (borrowerExistsOnOnelink ? AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) : AddCompassComment(queueData, task, actionCode, comment))
                    {
                        CloseOrReassignQueueTask(queueData, task, pauseOnQueueClosingError, comment);
                    }
                    else
                        ReassignQueueTask(queueData, task, rejectReason);
                }
                else
                    ReassignQueueTask(queueData, task, rejectReason);
                return false;
            }
            if (task.Demographics.ZipCode.Length < 5)
            {
                ReassignQueueTask(queueData, task, RejectAction.RejectReasons.RETURN_INCOMPLETE_ADDRESS);
                return false;
            }
            if (!task.Demographics.ZipCode.IsNumeric())
            {
                ReassignQueueTask(queueData, task, RejectAction.RejectReasons.RETURN_INVALID_ADDRESS);
                return false;
            }

            //Compare the address to the borrower's current address.
            if (AddressIsForeign(task.Demographics, borrowerExistsOnOnelink))
            {
                ReassignQueueTask(queueData, task, "Foreign demographics provided.");
                return false;
            }
            if (borrowerExistsOnOnelink ? AddressMatchesOneLink(task.Demographics, task.AdditionalInfo) : AddressMatchesCompass(task.Demographics))
            {
                string rejectReason = RejectAction.RejectReasons.ADDRESS_SAME_AS_ON_FILE;
                string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Address);
                string comment = string.Format("{0} {1}.", rejectReason, General.FormatAddressForComments(task.Demographics));
                if (borrowerExistsOnOnelink ? AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) : AddCompassComment(queueData, task, actionCode, comment))
                {
                    CloseOrReassignQueueTask(queueData, task, pauseOnQueueClosingError, rejectReason);
                }
                else
                    ReassignQueueTask(queueData, task, rejectReason);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Update onelink address.
        /// </summary>
        /// <param name="queueData"></param>
        /// <param name="task"></param>
        /// <param name="pauseOnQueueClosingError"></param>
        /// <returns></returns>
        protected override bool UpdateOneLink(QueueData queueData, QueueTask task, bool pauseOnQueueClosingError)
        {
            //Check that we don't already have a queue task to update this borrower's address.
            RI.FastPath(string.Format("LP50I{0};;;;;K0ADD", task.Demographics.Ssn));
            if (!RI.CheckForText(22, 3, "47004"))
                return false;

            //Check that the address isn't too long to fit in the system.
            if (task.Demographics.Address1.Length > 35 || (task.Demographics.Address2.IsPopulated() && task.Demographics.Address2.Length > 35))
                return false;

            //Update the borrower's address.
            RI.FastPath(string.Format("LP22C{0}", task.Demographics.Ssn));
            RI.PutText(3, 9, task.OneLinkSourceCode);
            RI.PutText(10, 9, task.Demographics.Address1.Replace("-", " "), true);
            if (task.Demographics.Address2.IsNullOrEmpty())
                task.Demographics.Address2 = "";
            RI.PutText(11, 9, task.Demographics.Address2.Replace("-", " "), true);
            RI.PutText(12, 9, task.Demographics.City, true);
            RI.PutText(12, 52, task.Demographics.State);
            RI.PutText(12, 60, task.Demographics.ZipCode, true);
            RI.PutText(10, 57, task.AdditionalInfo);
            RI.Hit(Key.F6);
            return RI.CheckForText(22, 3, "49000");
        }
    }
}
