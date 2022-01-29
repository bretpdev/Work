using System;
using System.Collections.Generic;
using System.Reflection;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;

namespace ACURINTC
{
    class PdemPathAddress : AddressProcessorBase
    {
        public PdemPathAddress(General g, QueueInfo data, PendingDemos task, bool skipTaskClose) : base(g, data, task, skipTaskClose)
        { }
        public void Process()
        {
            //Start at the appropriate point given the recovery value.
            string actionCode = RH.GetRejectActionCode(RejectReason.None, DemographicType.Address);
            if (!DemographicsPassReview())
            {
                CTH.CloseOrReassignTask(data, task, "Error closing task.");
                return;
            }

            bool closeCompass = false;
            bool closeOnelink = false;
            if (UpdateCompass())
            {
                string comment = string.Format("Accept demos from {0} {1}.", demographicsSource, task.GenerateBracketedAddressString());
                if (!CTH.AddComment(data, task, systemCode, actionCode, comment))
                    CTH.CreateTask(task, data.DemographicsReviewQueue, "");
                else
                    closeCompass = true;
            }
            if (borrowerExistsOnOneLink)
                if (OneLinkDemographicsPassReview() && UpdateOneLink())
                {
                    string comment = string.Format("Accept demos from {0} {1}.", demographicsSource, task.GenerateBracketedAddressString());
                    if (!OTH.AddComment(data, task, systemCode, actionCode, comment))
                        OTH.CreateTask(task, data.DemographicsReviewQueue, "");
                    else
                    {
                        OTH.ProcessLocate(task, systemCode);
                        closeOnelink = true;
                    }
                }
            if (closeCompass || closeOnelink)
                CTH.CloseOrReassignTask(data, task, "Error closing task.");


        }

        /// <summary>
        /// Update one link address from out QueueTask data.
        /// </summary>
        protected bool UpdateOneLink()
        {
            //Check that we don't already have a queue task to update this borrower's address.
            RI.FastPath(string.Format("LP50I{0};;;;;K0ADD", task.Ssn));
            if (!RI.CheckForText(22, 3, "47004"))
                return false;
            //Check that the address isn't too long to fit in the system.
            if (task.Address1.Length > 35 || (task.Address2.IsPopulated() && task.Address2.Length > 35))
                return false;

            //Update the borrower's address.
            RI.FastPath(string.Format("LP22C{0}", task.Ssn));

            RI.PutText(3, 9, systemCode.OneLinkSourceCode);
            RI.PutText(10, 9, task.Address1.Replace("-", " "), true);
            if (task.Address2.IsNullOrEmpty())
                RI.PutText(11, 9, "", true);
            else
                RI.PutText(11, 9, task.Address2.Replace("-", " "), true);
            RI.PutText(12, 9, task.City, true);
            RI.PutText(12, 52, task.State);
            RI.PutText(12, 60, task.ZipCode, true);
            RI.PutText(10, 57, "Y");
            RI.Hit(Key.F6);
            return RI.CheckForText(22, 3, "49000");
        }

        protected bool UpdateCompass()
        {
            RI.FastPath("tx3z/ctx1j;" + task.Ssn);
            if (RI.MessageCode == "01019")
                throw new Exception("Unable to locate borrower " + task.AccountNumber + " on CTX1J: " + RI.Message);
            RI.Hit(Key.F6);
            RI.Hit(Key.F6);
            if (RI.CheckForText(1, 71, "TXX1R-01"))
                RI.PutText(8, 18, systemCode.CompassSourceCode);

            RI.PutText(10, 32, (task.PendingVerificationDate ?? DateTime.Now).ToString("MMddyy"));
            RI.PutText(11, 55, "Y");
            //Enter the address and blank out any extra text.
            RI.PutText(11, 10, task.Address1, true);
            RI.PutText(12, 10, (task.Address2 ?? ""), true);
            RI.PutText(13, 10, (task.Address3 ?? ""), true);
            RI.PutText(14, 8, task.City, true);
            RI.PutText(14, 32, task.State);
            RI.PutText(14, 40, task.ZipCode, true);
            RI.Hit(Key.Enter);
            if (RI.CheckForText(23, 2, "01096", "01097"))
                return true;
            else
            {
                if (!CTH.CreateTask(task, data.DemographicsReviewQueue, ""))
                    G.LogRun.AddNotification(string.Format("AccountNumber: {0} QueueName: {1} CapturedDemographics: {2} ErrorReason: {3}", task.AccountNumber, data.DemographicsReviewQueue, task.OriginalAddressText, "COMPASS not successfully updated."), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return false;
            }
        }
    }
}