namespace INCIDENTRP
{
    public class ThirdPartyDataInvolved : IsEmptyBase
    {
        public string Name { get; set; }
        public string AccountNumber { get; set; }
        public string State { get; set; }
        public string DataRegion { get; set; }
        public bool NotifierKnowsPiiOwner { get; set; }
        public string NotifierRelationshipToPiiOwner { get; set; }
        public bool SocialSecurityNumbersWereReleased { get; set; }
        public bool LoanIdsOrNumbersWereReleased { get; set; }
        public bool LoanAmountsOrBalancesWereReleased { get; set; }
        public bool LoanPaymentHistoriesWereReleased { get; set; }
        public bool PayoffAmountsWereReleased { get; set; }
        public bool BankAccountNumbersWereReleased { get; set; }
        public bool DateOfBirthWasReleased { get; set; }
        public bool MedicalOrConditionalDisabilityWasReleased { get; set; }

        public bool IsComplete()
        {
            //The spec doesn't say anything is required.
            return true;
        }

        public void Save(DataAccess dataAccess, long ticketNumber)
        {
            if (IsEmpty())
                dataAccess.DeleteThirdPartyDataInvolved(ticketNumber);
            else
                dataAccess.SaveThirdPartyDataInvolved(this, ticketNumber);
        }
    }
}
