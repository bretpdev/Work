using System;

namespace PRIDRCRP
{
    public class BorrowerActivityResult
    {
        public int BorrowerActivityId { get; set; }
        public int BorrowerInformationId { get; set; }
        public DateTime ActivityDate { get; set; }
        public string ActivityDescription { get; set; }
    }
}