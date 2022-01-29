using System;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Uheaa.Common.Scripts;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;
using Uheaa.Common.ProcessLogger;

namespace ACURINTR.DemographicsProcessors
{
    class DudePathEmail : ComparePathBase
    {
        public DudePathEmail(ReflectionInterface ri, string demographicsReviewQueue, string foreignDemographicsQueue, ProcessLogData logData, string userId, string scriptId)
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
            if (!DemographicsPassReview(queueData, task, pauseOnQueueClosingError))
                return;//The task is closed or reassigned in the DemographicsPassReview() function after adding an LP50 comment, so our work here is done.
            if (!UpdateOneLink(queueData, task, pauseOnQueueClosingError))
                return;
            string actionCode = task.GetRejectActionCode(RejectAction.RejectReasons.NONE, QueueTask.DemographicType.Email);
            string comment = string.Format("Accept demos from {0} {1}{2}{3}.", task.DemographicsSource, "{", task.Demographics.EmailAddress, "}");
            if (!AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment))
            {
                if (!task.IsClosed)
                    ReassignQueueTask(queueData, task, comment);
                return;
            }
            if (!task.IsClosed)
                CloseOrReassignQueueTask(queueData, task, pauseOnQueueClosingError, "Error closing task.");
            ProcessLocate(task);
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
            const string COMMENT_EMAIL_INVALID = "Added OneLINK comment for invalid e-mail";
            const string COMMENT_EMAIL_ON_FILE = "Added OneLINK comment for e-mail already on file";

            //Check the e-mail for error conditions.
            if (EmailIsInvalid(task.Demographics.EmailAddress))
            {
                string rejectReason = RejectAction.RejectReasons.RETURN_INVALID_ADDRESS;
                string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Email);
                string comment = string.Format("{0} {1}{2}{3}.", rejectReason, "{", task.Demographics.EmailAddress, "}");
                if (AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment))
                {
                    if (!task.IsClosed)
                        CloseOrReassignQueueTask(queueData, task, pauseOnQueueClosingError, rejectReason);
                }
                else
                {
                    if (!task.IsClosed)
                        ReassignQueueTask(queueData, task, rejectReason);
                }
                return false;
            }
            if (EmailMatchesOneLink(task.Demographics.Ssn, task.Demographics.EmailAddress, task.AdditionalInfo))
            {
                string rejectReason = RejectAction.RejectReasons.ADDRESS_SAME_AS_ON_FILE;
                string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Email);
                string comment = string.Format("{0} {1}{2}{3}.", rejectReason, "{", task.Demographics.EmailAddress, "}");
                if (AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment))
                {
                    if (!task.IsClosed)
                        CloseOrReassignQueueTask(queueData, task, pauseOnQueueClosingError, rejectReason);
                }
                else
                {
                    if (!task.IsClosed)
                        ReassignQueueTask(queueData, task, rejectReason);
                }
                return false;
            }

            return true;
        }

        /// <summary>
        /// Check to see if the email has invalid characters
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private bool EmailIsInvalid(string email)
        {
            const string VALID_FORMAT = @"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$";
            return (!Regex.IsMatch(email, VALID_FORMAT));
        }

        /// <summary>
        /// Check the session and compare the emails.
        /// </summary>
        /// <param name="ssnOrAccountNumber"></param>
        /// <param name="email"></param>
        /// <param name="validityIndicator"></param>
        /// <returns></returns>
        private bool EmailMatchesOneLink(string ssnOrAccountNumber, string email, string validityIndicator)
        {
            //Hit up LP22 to get the borrower's current demographics.
            if (ssnOrAccountNumber.Length == 9)
                RI.FastPath(string.Format("LP22I{0};;;;L", ssnOrAccountNumber));
            else
                RI.FastPath(string.Format("LP22I;;;L;;;{0}", ssnOrAccountNumber));
            return (RI.CheckForText(19, 9, email) && RI.CheckForText(18, 56, validityIndicator));
        }

        /// <summary>
        /// Update the onelink email.
        /// </summary>
        /// <param name="queueData"></param>
        /// <param name="task"></param>
        /// <param name="pauseOnQueueClosingError"></param>
        /// <returns></returns>
        protected override bool UpdateOneLink(QueueData queueData, QueueTask task, bool pauseOnQueueClosingError)
        {
            RI.FastPath(string.Format("LP22C{0}", task.Demographics.Ssn));
            RI.PutText(3, 9, task.OneLinkSourceCode);
            RI.PutText(19, 9, task.Demographics.EmailAddress, true);
            RI.PutText(18, 56, task.AdditionalInfo);
            RI.Hit(Key.F6);
            if (RI.CheckForText(22, 3, "49000"))
                return true;
            else
            {
                if (!task.IsClosed)
                    ReassignQueueTask(queueData, task, "OneLINK not successfully updated.");
                return false;
            }
        }
    }
}
