using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace ACURINTC
{
    class PhoneProcessorBase : ProcessorBase
    {
        protected readonly string phoneNumber;
        protected readonly string mblIndicator;
        protected readonly string consentIndicator;
        protected readonly PhoneType targetPhoneType;
        protected PhoneType targetOneLinkPhoneType;
        protected readonly PhoneHelper PH;
        protected readonly DateTime? phoneVerificationDate;
        public PhoneProcessorBase(General g, QueueInfo data, PendingDemos task, bool skipTaskClose) : base(g, data, task, skipTaskClose)
        {
            if (!task.OriginalHomePhoneIsValid)
                targetPhoneType = PhoneType.Home;
            else if (!task.OriginalAltPhoneIsValid)
                targetPhoneType = PhoneType.Alternate;
            else if (!task.OriginalWorkPhoneIsValid)
                targetPhoneType = PhoneType.Work;
            else
                targetPhoneType = PhoneType.None;
            this.phoneNumber = task.PrimaryPhone;
            this.PH = new PhoneHelper(g.RI, task, g.DA, phoneNumber, targetPhoneType);
            this.phoneVerificationDate = task.HomePhoneVerificationDate;
            this.mblIndicator = task.PrimaryMblIndicator;
            this.consentIndicator = task.PrimaryConsentIndicator;
        }

        protected bool DemographicsPassReview()
        {
            if (targetPhoneType == PhoneType.None)
            {
                //all phones are valid
                var r = GetRejectResult(RejectReason.ValidPhones, DemographicType.Phone, phoneNumber);
                if (!COCH.AddComment(data, task, systemCode, r.ActionCode, r.Comment))
                    CTH.CreateTask(task, data.DemographicsReviewQueue, "");
                return false;
            }
            DateTime yearAgo = DateTime.Now.AddYears(-1).Date;
            bool validHomeMatch = phoneNumber == task.OriginalHomePhone && task.OriginalHomePhoneIsValid;
            bool homeYearInvalid = phoneNumber == task.OriginalHomePhone && !task.OriginalHomePhoneIsValid && task.HomePhoneVerificationDate > yearAgo;
            bool validWorkMatch = phoneNumber == task.OriginalWorkPhone && task.OriginalWorkPhoneIsValid;
            bool workYearInvalid = phoneNumber == task.OriginalWorkPhone && !task.OriginalWorkPhoneIsValid && task.WorkPhoneVerificationDate > yearAgo;
            bool validAltMatch = phoneNumber == task.OriginalAltPhone && task.OriginalAltPhoneIsValid;
            bool altYearInvalid = phoneNumber == task.OriginalAltPhone && !task.OriginalAltPhoneIsValid && task.AltPhoneVerificationDate > yearAgo;
            bool validMobileMatch = phoneNumber == task.OriginalMobilePhone && task.OriginalMobilePhoneIsValid;
            bool mobileYearInvalid = phoneNumber == task.OriginalMobilePhone && !task.OriginalMobilePhoneIsValid && task.MobilePhoneVerificationDate > yearAgo;
            if (validHomeMatch || validWorkMatch || validAltMatch || validMobileMatch || homeYearInvalid || workYearInvalid || altYearInvalid || mobileYearInvalid)
            {
                var r = GetRejectResult(RejectReason.SamePhone, DemographicType.Phone, phoneNumber);
                if (!COCH.AddComment(data, task, systemCode, r.ActionCode, r.Comment))
                    CTH.CreateTask(task, data.DemographicsReviewQueue, "");
                return false;
            }
            if (task.Ssn.Substring(0, 1) == "P")
            {
                CTH.ReassignTask(data, task, "Task is for a reference");
                return false;
            }
            //Check the phone number for error conditions.
            if (phoneNumber.StartsWith("011"))
            {
                var r = GetRejectResult(RejectReason.ForeignDemographics, DemographicType.Phone, phoneNumber);
                if (CTH.CreateTask(task, data.ForeignReviewQueue, ""))
                {
                    if (!CTH.AddComment(data, task, systemCode, r.ActionCode, r.Comment))
                        CTH.CreateTask(task, data.DemographicsReviewQueue, "");
                }
                else
                    G.LogRun.AddNotification(string.Format("AccountNumber: {0} QueueName: {1} CapturedDemographics: {2.} ErrorReason: {3}", task.AccountNumber, data.DemographicsReviewQueue, task.OriginalHomePhone, "Could not create user queue task."), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return false;
            }

            if (phoneNumber.Length > 10)
            {
                var r = GetRejectResult(RejectReason.PhoneTooLong, DemographicType.Phone, phoneNumber);
                if (CTH.CreateTask(task, data.ForeignReviewQueue, ""))
                {
                    if (!CTH.AddComment(data, task, systemCode, r.ActionCode, r.Comment))
                        CTH.CreateTask(task, data.DemographicsReviewQueue, "");
                }
                else
                    G.LogRun.AddNotification(string.Format("AccountNumber: {0} QueueName: {1} CapturedDemographics: {2.} ErrorReason: {3}", task.AccountNumber, data.DemographicsReviewQueue, task.OriginalHomePhone, "Could not create user queue task."), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return false;
            }


            if (phoneNumber.Length < 10)
            {
                var r = GetRejectResult(RejectReason.IncompletePhone, DemographicType.Phone, phoneNumber);
                if (!COCH.AddComment(data, task, systemCode, r.ActionCode, r.Comment))
                    CTH.CreateTask(task, data.DemographicsReviewQueue, "");
                return false;
            }

            char firstExchangeChar = phoneNumber.Skip(3).FirstOrDefault();
            if (!phoneNumber.IsNumeric() || phoneNumber == "8015551212" || firstExchangeChar == '1' || firstExchangeChar == '0')
            {
                var r = GetRejectResult(RejectReason.InvalidPhoneReturn, DemographicType.Phone, phoneNumber);
                if (!COCH.AddComment(data, task, systemCode, r.ActionCode, r.Comment))
                    CTH.CreateTask(task, data.DemographicsReviewQueue, "");
                return false;
            }

            if (CTH.BorrowerHasForeignNumber(task.Ssn))
            {
                CTH.ReassignTask(data, task, "Current demographics foreign.");
                return false;
            }

            if (PH.CompassCompareHistoryPhone() || (borrowerExistsOnOneLink && PH.PhoneNumberIsInvalidInOneLinkWithinPastYear()))
            {
                var r = GetRejectResult(RejectReason.InvalidPhoneHistory, DemographicType.Phone, phoneNumber);
                if (!COCH.AddComment(data, task, systemCode, r.ActionCode, r.Comment))
                    CTH.CreateTask(task, data.DemographicsReviewQueue, "");
                return false;
            }
            return true;
        }
        protected bool OneLinkDemographicsPassReview()
        {
            RI.FastPath(string.Format("LP22C{0}", task.Ssn));
            bool homeValid = RI.CheckForText(13, 38, "Y");
            bool altValid = RI.CheckForText(14, 38, "Y");
            bool otherValid = RI.CheckForText(15, 38, "Y");
            if (!homeValid)
                targetOneLinkPhoneType = PhoneType.Home;
            else if (!altValid)
                targetOneLinkPhoneType = PhoneType.Alternate;
            else if (!otherValid)
                targetOneLinkPhoneType = PhoneType.Work;
            if (homeValid && altValid && otherValid)
            {
                //all phones are valid
                var r = GetRejectResult(RejectReason.ValidPhones, DemographicType.Phone, phoneNumber);
                if (!OTH.AddComment(data, task, systemCode, r.ActionCode, r.Comment))
                    CTH.CreateTask(task, data.DemographicsReviewQueue, "");
                return false;
            }
            bool sameHome = RI.CheckForText(13, 12, phoneNumber) && RI.CheckForText(13, 38,"Y");
            bool sameAlt = RI.CheckForText(14, 12, phoneNumber) && RI.CheckForText(14, 38, "Y");
            bool sameOther = RI.CheckForText(15, 12, phoneNumber) && RI.CheckForText(15, 38, "Y");
            if (sameHome || sameAlt || sameOther)
            {
                var r = GetRejectResult(RejectReason.SamePhone, DemographicType.Phone, phoneNumber);
                if (!OTH.AddComment(data, task, systemCode, r.ActionCode, r.Comment))
                    CTH.CreateTask(task, data.DemographicsReviewQueue, "");
                return false;
            }
            return true;
        }
    }
}
