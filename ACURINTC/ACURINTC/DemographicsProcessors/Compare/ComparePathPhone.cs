using System;
using System.Collections.Generic;
using System.Reflection;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;

namespace ACURINTC
{
    class ComparePathPhone : PhoneProcessorBase
    {
        public ComparePathPhone(General g, QueueInfo data, PendingDemos task, bool skipTaskClose) :
            base(g, data, task, skipTaskClose)
        { }
        public void Process()
        {
            if (!DemographicsPassReview())
            {
                CTH.CloseOrReassignTask(data, task, "Error closing task.");
                return;
            }

            bool closeCompass = false;
            bool closeOnelink = false;
            if (UpdateCompass())
            {
                string actionCode = RH.GetRejectActionCode(RejectReason.None, DemographicType.Phone);
                string comment = string.Format("Accept demos from {0} {1}{2}{3}.", demographicsSource, "{", phoneNumber, "}") + ". " + string.Format("{0} phone number {1}{2}{3} updated as {4} phone.", demographicsSource, "{", phoneNumber, "}", targetPhoneType.ToString().ToLower());
                if (!CTH.AddComment(data, task, systemCode, actionCode, comment))
                    CTH.ReassignTask(data, task, comment);
                else
                    closeCompass = true;
            }
            if (borrowerExistsOnOneLink)
            {
                if (OneLinkDemographicsPassReview() && UpdateOneLink())
                {
                    string actionCode = RH.GetRejectActionCode(RejectReason.None, DemographicType.Phone);
                    string olPhoneType = targetOneLinkPhoneType.ToString().ToLower();
                    if (targetOneLinkPhoneType == PhoneType.Work)
                        olPhoneType = "other";
                    string comment = string.Format("Accept demos from {0} {1}{2}{3}.", demographicsSource, "{", phoneNumber, "}") + ". " + string.Format("{0} phone number {1}{2}{3} updated as {4} phone.", demographicsSource, "{", phoneNumber, "}", olPhoneType);
                    if (!OTH.AddComment(data, task, systemCode, actionCode, comment))
                        OTH.ReassignTask(data, task, comment);
                    else
                        closeOnelink = true;

                    OTH.ProcessLocate(task, systemCode);
                }
            }
            if (closeCompass || closeOnelink)
                CTH.CloseOrReassignTask(data, task, "Error closing task.");
        }

        /// <summary>
        /// update the phone number in compass to match the new one.
        /// </summary>
        protected bool UpdateCompass()
        {
            RI.FastPath(string.Format("TX3Z/CTX1JB;{0}", task.Ssn));
            RI.Hit(Key.F6, 3); //tab to phone info
            PH.UpdatePhoneNumber(systemCode);
            if (RI.CheckForText(23, 2, "01097", "01100"))
                return true;
            else if (RI.CheckForText(23, 2, "06536"))
            {
                //add the phone number to the home phone field instead
                RI.PutText(16, 14, "H", Key.Enter);
                PH.UpdatePhoneNumber(systemCode);
                return true;
            }
            else
            {
                if (!CTH.CreateTask(task, data.DemographicsReviewQueue, ""))
                    G.LogRun.AddNotification(string.Format("AccountNumber: {0} QueueName: {1} CapturedDemographics: {2} ErrorReason: {3}", task.AccountNumber, data.DemographicsReviewQueue, task.OriginalHomePhone, "COMPASS not successfully updated."), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return false;
            }
        }

        /// <summary>
        /// update the phone information in one link.
        /// </summary>
        protected bool UpdateOneLink()
        {
            bool success = false;
            RI.FastPath(string.Format("LP22C{0}", task.Ssn));
            RI.PutText(3, 9, systemCode.OneLinkSourceCode);

            if (!RI.CheckForText(13, 38, "Y"))
            {
                RI.PutText(13, 12, phoneNumber);
                RI.PutText(13, 38, "Y");
                RI.PutText(13, 68, "N");
            }
            else if (!RI.CheckForText(14, 38, "Y"))
            {
                RI.PutText(14, 12, phoneNumber);
                RI.PutText(14, 38, "Y");
                RI.PutText(14, 68, "N");
            }
            else
            {
                RI.PutText(15, 12, phoneNumber);
                RI.PutText(15, 38, "Y");
                RI.PutText(15, 68, "N");
            }

            RI.Hit(Key.F6);
            switch (RI.GetText(22, 3, 5))
            {
                case "49000":
                    //Set the recovery step, which will determine what comment to add below.
                    success = true;
                    break;
                case "UPDAT":
                    if (RI.CheckForText(22, 3, "UPDATE ERROR: ALTPH") || RI.CheckForText(22, 3, "UPDATE ERRORS: ALTPH"))
                        success = true;
                    else
                    {
                        success = false;
                        G.LogRun.AddNotification(string.Format("Error updating the phone number '{0}' for borrower '{1}' with source '{2}'.  Error Message: {3}", phoneNumber, task.AccountNumber, systemCode.OneLinkSourceCode, RI.Message), NotificationType.ErrorReport, NotificationSeverityType.Informational);
                    }
                    break;
                case "40163":
                case "48081":
                    //Set the recovery step, which will determine what comment to add below.
                    success = false;
                    break;
                default:
                    OTH.ReassignTask(data, task, "OneLINK not successfully updated.");
                    return false;
            }

            if (success)
            {
                return true;
            }
            else
            {
                var rejectReason = RejectReason.BlockedPhone;
                var rejectReasonText = G.RRH.GetRejectReasonText(rejectReason);
                string actionCode = RH.GetRejectActionCode(rejectReason, DemographicType.Phone);
                string comment = string.Format("{0} returned phone number blocked by parameter card.", demographicsSource);
                if (OTH.AddComment(data, task, systemCode, actionCode, comment))
                    OTH.CloseOrReassignTask(data, task, comment);
                else
                    OTH.ReassignTask(data, task, rejectReasonText);
                return false;
            }
        }
    }
}
