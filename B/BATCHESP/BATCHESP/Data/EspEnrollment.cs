using System;
using System.Collections.Generic;

namespace BATCHESP
{
    public class EspEnrollment
    {
        public int EspEnrollmentId { get; set; }
		public string BorrowerSsn { get; set; }
        public string AccountNumber { get; set; }
		public string Queue { get; set; }
        public string SubQueue { get; set; }
        public string TaskControlNumber { get; set; }
        public string Arc { get; set; }
        public DateTime? ArcRequestDate { get; set; }
        public string Message1 { get; set; }
        public string SupplementalMessage { get; set; }
        public string StudentSsn { get; set; }
        public string StudentSsn2 { get; set; }
        public string SchoolCode { get; set; }
        public string Esp_Status { get; set; }
        public DateTime? Esp_SeparationDate { get; set; }
		public DateTime? Esp_CertificationDate { get; set; }
        public DateTime? EnrollmentBeginDate { get; set; }
        public string SourceCode { get; set; }
        public bool RequiresReview { get; set; }

        public override string ToString()
        {
            List<string> items = new List<string>();
            foreach (var prop in this.GetType().GetProperties())
                items.Add(prop.Name + ":" + prop.GetValue(this));
            return string.Join(";", items);
        }
    }
}
