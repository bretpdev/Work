using System;

namespace BATCHESP
{
    /// <summary>
    /// Deferment and forbearance info pulled back from the DB.
    /// Loaded into tables with intial AddNewWorkToTables sproc call.
    /// </summary>
    public class TsayDefermentForbearance 
    {
        public int TsayDefermentForbearanceId { get; set; }
        public string BorrowerSsn { get; set; }
        public int LoanSequence { get; set; }
        public string Type { get; set; }
        public string DeferSchool { get; set; }
        public DateTime? BeginDate { get; set; } // From LN50
        public DateTime? EndDate { get; set; } // From LN50
        public DateTime? CertificationDate { get; set; }
        public DateTime? RequestedBeginDate { get; set; } // From DF10
        public DateTime? RequestedEndDate { get; set; } // From DF10
    }
}
