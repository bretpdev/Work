using System;

namespace IDRRPTFED
{
    public class RunHistory
    {
        public int? DateID { get; set; }
        public DateTime? RunDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string RunBy { get; set; }
    }
}
