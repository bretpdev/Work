using Uheaa.Common.Scripts;
using Uheaa.Common;
using System;
using System.Collections.Generic;

namespace ACURINTC
{
    public class PendingDemos : SystemBorrowerDemographics
    {
        public PendingDemos()
        {
            StatusInfo = new TaskStatusInfo();
        }
        public int ProcessQueueId { get; set; }
        public string Queue { get; set; }
        public string SubQueue { get; set; }
        public string TaskControlNumber { get; set; }
        public DemographicsSource DemographicsSourceId { get; set; }
        public SystemSource SystemSourceId { get; set; }
        public string OriginalAddressText { get; set; }
        public string OriginalHomePhone { get; set; }
        public string OriginalAltPhone { get; set; }
        public string OriginalWorkPhone { get; set; }
        public string OriginalMobilePhone { get; set; }
        public string AdditionalInfo { get; set; }
        public DateTime? PendingVerificationDate { get; set; }
        public DateTime? CurrentVerificationDate { get; set; }
        public DateTime? HomePhoneVerificationDate { get; set; }
        public DateTime? AltPhoneVerificationDate { get; set; }
        public DateTime? WorkPhoneVerificationDate { get; set; }
        public DateTime? MobilePhoneVerificationDate { get; set; }
        public bool OriginalAddressIsValid { get; set; }
        public bool OriginalHomePhoneIsValid { get; set; }
        public bool OriginalAltPhoneIsValid { get; set; }
        public bool OriginalWorkPhoneIsValid { get; set; }
        public bool OriginalMobilePhoneIsValid { get; set; }
        public string Address3 { get; set; }

        public bool HasAddress
        {
            get
            {
                if (Address1.IsPopulated() || Address2.IsPopulated() || City.IsPopulated()
                   || State.IsPopulated() || Country.IsPopulated() || ZipCode.IsPopulated())
                    return true;
                return false;
            }
        }

        public bool HasAltPhone
        {
            get
            {
                return !string.IsNullOrWhiteSpace(AlternatePhone);
            }
        }

        public bool HasEmail
        {
            get
            {
                return !string.IsNullOrWhiteSpace(EmailAddress);
            }
        }

        public bool HasHomePhone
        {
            get
            {
                return !string.IsNullOrWhiteSpace(PrimaryPhone);
            }
        }

        public string GenerateBracketedAddressString()
        {
            string combinedAddress = (Address1 + " " + Address2 + " " + Address3).Trim();
            string foreignCountry = (Country + " " + ForeignState).Trim();
            List<string> addressPieces = new List<string>() { combinedAddress, City, State, ZipCode };
            if (!string.IsNullOrWhiteSpace(foreignCountry))
                addressPieces.Add(foreignCountry);
            return "{" + string.Join(", ", addressPieces) + "}";
        }

        public TaskStatusInfo StatusInfo { get; set; }
    }
}