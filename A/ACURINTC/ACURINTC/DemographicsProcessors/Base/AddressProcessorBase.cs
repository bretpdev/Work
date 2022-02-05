using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace ACURINTC
{
    class AddressProcessorBase : ProcessorBase
    {
        protected readonly AddressHelper AH;
        public AddressProcessorBase(General g, QueueInfo data, PendingDemos task, bool skipTaskClose) : base(g, data, task, skipTaskClose)
        {
            this.AH = new AddressHelper(g.RI as ReflectionInterface, task, g.DA);
        }

        protected bool DemographicsPassReview()
        {
            if (task.OriginalAddressIsValid)
            {
                var validAddressResult = GetRejectResult(RejectReason.ValidAddress, DemographicType.Address);
                if (!COCH.AddComment(data, task, systemCode, validAddressResult.ActionCode, validAddressResult.Comment))
                    CTH.CreateTask(task, data.DemographicsReviewQueue, "");
                return false;
            }
            if (task.Ssn.Substring(0, 1) == "P")
            {
                CTH.ReassignTask(data, task, "Task is for a reference");
                return false;
            }
            RejectionResults r = null;
            //Check the address for problem conditions.
            if (!string.IsNullOrWhiteSpace(task.Country) || !string.IsNullOrWhiteSpace(task.ForeignState) || task.State.ToUpper() == "FC")
                r = GetRejectResult(RejectReason.ForeignDemographics, DemographicType.Address);
            else if (task.Address1.IsNullOrEmpty())
                r = GetRejectResult(RejectReason.NoStreet, DemographicType.Address);
            else if (task.Address1.ToUpper() == "GENERAL DELIVERY" || (task.Address2.IsPopulated() && task.Address2.ToUpper() == "GENERAL DELIVERY") || task.Address1.Length > 30 || (task.Address3 ?? "").Length > 30 || (task.City ?? "").Length > 20)
                r = GetRejectResult(RejectReason.InvalidAddressReturn, DemographicType.Address);
            else if (task.City.IsNullOrEmpty() || task.State.IsNullOrEmpty() || task.ZipCode.IsNullOrEmpty() || System.Text.RegularExpressions.Regex.IsMatch(task.City, @"[^A-Za-z\s]") || task.State.Length == 1)
                r = GetRejectResult(RejectReason.IncompleteAddress, DemographicType.Address);
            else if (task.ZipCode.Length < 5 || !task.ZipCode.IsNumeric())
                r = GetRejectResult(RejectReason.IncompleteAddress, DemographicType.Address);
            if (r != null)
            {
                if (CTH.CreateTask(task, data.ForeignReviewQueue, ""))
                    COCH.AddComment(data, task, systemCode, r.ActionCode, r.Comment);
                else
                    G.LogRun.AddNotification(string.Format("AccountNumber: {0} QueueName: {1} CapturedDemographics: {2} ErrorReason: {3}", task.AccountNumber, data.DemographicsReviewQueue, task.OriginalAddressText, "Could not create user queue task."), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                CTH.ReassignTask(data, task, r.Comment);
                return false;
            }
            if (CTH.BorrowerHasForeignAddress(task.Ssn))
            {
                CTH.CreateTask(task, data.ForeignReviewQueue, "");
                return false;
            }

            if (task.OriginalAddressText.Contains(task.Address1) && task.OriginalAddressText.Contains(task.Address2) && task.OriginalAddressText.Contains(task.City) && task.OriginalAddressText.Contains(task.State) && task.OriginalAddressText.Contains(task.ZipCode))
            {
                r = GetRejectResult(RejectReason.SameAddress, DemographicType.Address);
                if (!COCH.AddComment(data, task, systemCode, r.ActionCode, r.Comment))
                    CTH.CreateTask(task, data.DemographicsReviewQueue, "");
                return false;
            }
            if (AH.CompassCompareAddressHistory())
            {
                r = GetRejectResult(RejectReason.InvalidAddressHistory, DemographicType.Address);
                if (!COCH.AddComment(data, task, systemCode, r.ActionCode, r.Comment))
                    CTH.CreateTask(task, data.DemographicsReviewQueue, "");
                return false;
            }
            return true;
        }

        protected bool OneLinkDemographicsPassReview()
        {
            if (OTH.AddressMatches(task))
            {
                var r = GetRejectResult(RejectReason.SameAddress, DemographicType.Address);
                if (!OTH.AddComment(data, task, systemCode, r.ActionCode, r.Comment))
                    CTH.CreateTask(task, data.DemographicsReviewQueue, "");
                return false;
            }
            if (OTH.AddressIsValid(task))
            {
                var r = GetRejectResult(RejectReason.ValidAddress, DemographicType.Address);
                if (!OTH.AddComment(data, task, systemCode, r.ActionCode, r.Comment))
                    CTH.CreateTask(task, data.DemographicsReviewQueue, "");
                return false;
            }
            if (!string.IsNullOrWhiteSpace(task.Address3))
            {
                CTH.CreateTask(task, data.DemographicsReviewQueue, "");
                OTH.ReassignTask(data, task, "Unable to update OneLink Address.  PDEM RECEIVED: " + task.GenerateBracketedAddressString());
                return false;
            }
            return true;
        }
    }
}
