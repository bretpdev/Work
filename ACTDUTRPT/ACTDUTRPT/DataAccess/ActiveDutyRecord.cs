using System;
using Uheaa.Common.DataAccess;

namespace ACTDUTRPT
{
    public class ActiveDutyRecord
    {
        [DbName("ActiveDutyReportingId")]
        public int ActiveDutyReportingId { get; set; }
        [DbName("AccountNumber")]
        public string AccountNumber { get; set; }
        [DbName("BorrSSN")]
        public string BorrowerSSN { get; set; }
        [DbName("EndrAccountNumber")]
        public string EndorserAccountNumber { get; set; }
        [DbName("EndrSSN")]
        public string EndorserSSN { get; set; }
        public string ProcessingSSN { get; set; }
        [DbName("TXCXBegin")]
        public DateTime? TXCXBeginDate { get; set; }
        [DbName("TXCXEnd")]
        public DateTime? TXCXEndDate { get; set; }
        [DbName("TXCXType")]
        public string TXCXType { get; set; }
        [DbName("ServiceComponent")]
        public string ServiceComponent { get; set; }
        [DbName("TXCXUpdated")]
        public DateTime? TXCXUpdatedAt { get; set; }
        public string SessionErrorMessage { get; set; }
    }
}