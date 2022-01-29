namespace INCIDENTRP
{
    public class AgencyEmployeeHrDataInvolved : IsEmptyBase
    {
        public string Name { get; set; }
        public string State { get; set; }
        public bool NotifierKnowsEmployee { get; set; }
        public string NotifierRelationshipToEmployee { get; set; }
        public bool DateOfBirthWasReleased { get; set; }
        public bool EmployeeIdNumberWasReleased { get; set; }
        public bool HomeAddressWasReleased { get; set; }
        public bool HealthInformationWasReleased { get; set; }
        public bool PerformanceInformationWasReleased { get; set; }
        public bool PersonnelFilesWereReleased { get; set; }
        public bool UnauthorizedReferenceWasReleased { get; set; }

        public bool IsComplete()
        {
            //The spec doesn't say anything is required.
            return true;
        }

        public void Save(DataAccess dataAccess, long ticketNumber)
        {
            if (IsEmpty())
                dataAccess.DeleteAgencyEmployeeHrDataInvolved(ticketNumber);
            else
                dataAccess.SaveAgencyEmployeeHrDataInvolved(this, ticketNumber);
        }
    }
}
