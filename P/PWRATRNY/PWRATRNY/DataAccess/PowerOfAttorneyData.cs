using Uheaa.Common.Scripts;

namespace PWRATRNY
{
    public class PowerOfAttorneyData
	{
		public string ExpirationDate { get; set; }
		public SystemBorrowerDemographics BorrowerDemos { get; set; }
		public ReferenceData RefData { get; set; }
		public bool SendDenialOrConfirmationLetter { get; set; }
		public bool AddReference { get; set; }
		public bool AddMPOAAActionCode { get; set; }
		public bool AddMPOADActionCode { get; set; }
		public bool RequestApproved { get; set; }
		public bool UserModified { get; set; } = false;
		public bool UserModifiedAddress { get; set; } = false;
		public bool UserModifiedHomePhoneNumber { get; set; } = false;
		public bool UserModifiedReferenceName { get; set; } = false;
		public bool UserModifiedRelationship { get; set; } = false;
		public bool UserModifiedAltPhone { get; set; } = false;
		public bool UserModifiedForeignPhone { get; set; } = false;
		public bool UserModifiedEmail { get; set; } = false;
		public bool PrintApprovalLetter { get; set; } = false;
		public bool PrintDenialLetter { get; set; } = false;

		public PowerOfAttorneyData(UserPOAEntry userEntry, SystemBorrowerDemographics demos)
		{
			ExpirationDate = userEntry.ExpirationDate;
			BorrowerDemos = demos;
			RefData = new ReferenceData(userEntry.FirstName, userEntry.LastName);
			SendDenialOrConfirmationLetter = BorrowerDemos.IsValidAddress;
		}
	}
}