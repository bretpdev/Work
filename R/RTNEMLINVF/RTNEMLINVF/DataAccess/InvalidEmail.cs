using System;

namespace RTNEMLINVF
{
    public class InvalidEmail
    {
        public int InvalidEmailId { get; set; }
        public string Ssn { get; set; }
        public string EmailType { get; set; }
        public string EmailAddress { get; set; }
        public DateTime? InvalidatedAt { get; set; }
        public long ArcAddProcessingId { get; set; }
    }
}