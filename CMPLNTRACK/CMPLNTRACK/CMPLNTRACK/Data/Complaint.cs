using System;

namespace CMPLNTRACK
{
    public class Complaint
    {
        public int ComplaintId { get; set; }
        public string AccountNumber { get; set; }
        public string BorrowerName { get; set; }
        public int ComplaintTypeId { get; set; }
        public int ComplaintPartyId { get; set; }
        public int ComplaintGroupId { get; set; }
        public int? ResolutionComplaintHistoryId { get; set; }
        public string ComplaintDescription { get; set; }
        public DateTime ComplaintDate { get; set; }
        public string NeedHelpTicketNumber { get; set; }
        public int DaysToRespond { get; set; }
        public string ControlMailNumber { get; set; }
        public string AddedBy { get; set; }
        public string TypeName { get; set; }
        public string PartyName { get; set; }
        public string GroupName { get; set; }
    }
}