namespace INCIDENTRP
{
    public class BorrowerDataInvolved : IsEmptyBase
    {
        public string Name { get; set; }
        public string AccountNumber { get; set; }
        public string State { get; set; }
        public string DataRegion { get; set; }
        public bool BorrowerInformationIsVerified { get; set; }
        public bool NotifierKnowsPiiOwner { get; set; }
        public string NotifierRelationshipToPiiOwner { get; set; }
        public bool AddressWasReleased { get; set; }
        public bool BankAccountNumbersWereReleased { get; set; }
        public bool CreditReportOrScoreWasReleased { get; set; }
        public bool DateOfBirthWasReleased { get; set; }
        public bool EmployeeIdNumberWasReleased { get; set; }
        public bool EmployerIdNumberWasReleased { get; set; }
        public bool LoanAmountsOrBalancesWereReleased { get; set; }
        public bool LoanApplicationsWereReleased { get; set; }
        public bool LoanIdsOrNumbersWereReleased { get; set; }
        public bool LoanPaymentHistoriesWereReleased { get; set; }
        public bool MedicalOrConditionalDisabilityWasReleased { get; set; }
        public bool PayoffAmountsWereReleased { get; set; }
        public bool PhoneNumberWasReleased { get; set; }
        public bool PromissoryNotesWereReleased { get; set; }
        public bool SocialSecurityNumbersWereReleased { get; set; }

        public bool IsComplete()
        {
            //The spec doesn't say anything is required.
            return true;
        }

        public void Save(DataAccess dataAccess, long ticketNumber)
        {
            if (IsEmpty())
                dataAccess.DeleteBorrowerDataInvolved(ticketNumber);
            else
                dataAccess.SaveBorrowerDataInvolved(this, ticketNumber);
        }
    }
}
