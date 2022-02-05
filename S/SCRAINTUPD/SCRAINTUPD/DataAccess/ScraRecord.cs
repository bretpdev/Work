using System;
using Uheaa.Common.DataAccess;

namespace SCRAINTUPD
{
    public class ScraRecord
    {
        [DbName("ScriptProcessingId")]
        public int ScriptProcessingId { get; set; }
        [DbName("BorrSSN")]
        public string BorrowerSSN { get; set; }
        [DbName("AccountNumber")]
        public string AccountNumber { get; set; }
        [DbName("DataComparisonId")]
        public int DataComparisonId { get; set; }
        [DbName("Loan")]
        public short LoanSequence { get; set; }
        [DbName("LN72Begin")]
        public DateTime? LN72InterestBeginDate { get; set; }
        [DbName("LN72End")]
        public DateTime? LN72InterestEndDate { get; set; }
        [DbName("LN72RegRate")]
        public decimal LN72RegRate { get; set; }
        [DbName("LN72SCRA")]
        public bool LN72SCRA { get; set; }
        [DbName("LN10LnAdd")]
        public DateTime? LoanAddDate { get; set; }
        [DbName("LN10Disb")]
        public DateTime? LoanDisbursementDate { get; set; }
        [DbName("LN10CurPri")]
        public decimal CurrentPrincipal { get; set; }
        [DbName("LN10Sta")]
        public string LN10Status { get; set; }
        [DbName("LN10Sub")]
        public string LN10Sub { get; set; }
        [DbName("LoanProgram")]
        public string LoanProgram { get; set; }
        [DbName("DW01Sta")]
        public string Dw01LoanStatus { get; set; }
        [DbName("DODBegin")]
        public DateTime? DODBeginDate { get; set; }
        [DbName("DODEnd")]
        public DateTime? DODEndDate { get; set; }
        [DbName("TXCXBegin")]
        public DateTime? TXCXBeginDate { get; set; }
        [DbName("TXCXEnd")]
        public DateTime? TXCXEndDate { get; set; }
        [DbName("TXCXType")]
        public string TXCXType { get; set; }
        [DbName("BenefitSourceId")]
        public short BenefitSourceId { get; set; }
        [DbName("ScriptAction")]
        public string ScriptAction { get; set; }
        [DbName("TS06Updated")]
        public DateTime? TS06UpdatedAt { get; set; }
        [DbName("TXCXUpdated")]
        public DateTime? TXCXUpdatedAt { get; set; }
        [DbName("TS0NUpdated")]
        public DateTime? TS0NUpdatedAt { get; set; }
        [DbName("TSX0TUpdated")]
        public DateTime? TSX0TUpdatedAt { get; set; }
        [DbName("AAPUpdated")]
        public long? ArcAddProcessingId { get; set; }
        [DbName("DataComparisonDate")]
        public DateTime? DataComparisonDate { get; set; }
        [DbName("ExemptSchedule")]
        public bool ExemptSchedule { get; set; }
        [DbName("Deconverted")]
        public bool Deconverted { get; set; }
        [DbName("ExemptLoanStatus")]
        public bool ExemptLoanStatus { get; set; }
        [DbName("ExemptForbType")]
        public bool ExemptForbType { get; set; }
        [DbName("ExemptLitigation")]
        public bool ExemptLitigation { get; set; }
        [DbName("ExemptDifferingSchedules")]
        public bool ExemptDifferingSchedules { get; set; }
        [DbName("ExemptFixedAlternateSchedule")]
        public bool ExemptFixedAlternateSchedule { get; set; }
        [DbName("ExemptInactiveSchedule")]
        public bool ExemptInactiveSchedule { get; set; }
        [DbName("BorrBalance")]
        public decimal BorrBalance { get; set; }
        [DbName("BalAtRepay")]
        public double BalAtRepay { get; set; }
        [DbName("RepayStart")]
        public string RepayStart { get; set; }
        //public string ArcComment { get; set; }
        public bool TS06UpdatedSuccessfully { get; set; } = true;
        public bool TS0NUpdatedSuccessfully { get; set; } = true;
        public bool TXCXUpdatedSuccessfully { get; set; } = true;
        public string SessionErrorMessage { get; set; }
        //public bool RepayStartDiffer { get; set; }
        public bool CalcSchedules { get; set; }
        public bool SpecialBypass { get; set; }
        public bool FoundLoanToDisClose { get; set; }
        public string LoanOwner { get; set; }

    }
}