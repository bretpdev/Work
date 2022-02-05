using System;
using Uheaa.Common.DataAccess;

namespace LNDERLETTR
{
    public class LetterData
    {
        public int LettersId { get; set; }
        [DbName("BF_SSN")]
        public string Ssn { get; set; }
        [DbName("WF_ORG_LDR")]
        public string LenderId { get; set; }
        [DbName("II_LDR_VLD_ADR")]
        public string ValidLenderAddress { get; set; }
        [DbName("LDR_STR_ADR_1")]
        public string Address {  get; set; }
        public bool InLenderList { get; set; }
        public DateTime? LetterCreatedAt { get; set; }
        public int? ArcAddProcessingId { get; set; }
        public int Population { get; set; }
        public DateTime? QueueClosedAt { get; set; }
        public LenderData LenderInfo { get; set; }
    }
}