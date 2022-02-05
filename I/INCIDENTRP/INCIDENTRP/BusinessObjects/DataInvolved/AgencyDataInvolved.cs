namespace INCIDENTRP
{
    public class AgencyDataInvolved : IsEmptyBase
    {
        public bool AccountingOrAdministrativeRecordsWereReleased { get; set; }
        public bool ClosedSchoolRecordsWereReleased { get; set; }
        public bool ConfidentialCaseFilesWereReleased { get; set; }
        public bool ContractInformationWasReleased { get; set; }
        public bool OperationsReportsWereReleased { get; set; }
        public bool ProposalAndLoanPurchaseRequestsWereReleased { get; set; }
        public bool UespParticipantRecordsWereReleased { get; set; }
        public bool OtherInformationWasReleased { get; set; }
        public string OtherInformation { get; set; }

        public bool IsComplete()
        {
            //The spec doesn't say anything is required.
            return true;
        }

        public void Save(DataAccess dataAccess, long ticketNumber)
        {
            if (IsEmpty())
                dataAccess.DeleteAgencyDataInvolved(ticketNumber);
            else
                dataAccess.SaveAgencyDataInvolved(this, ticketNumber);
        }
    }
}
