using System;

namespace PRETNFRNOT
{
    public class BorrowerData
    {
        public string RM_APL_PGM_PRC { get; set; }
        public DateTime RT_RUN_SRT_DTS_PRC { get; set; }
        public int RN_SEQ_LTR_CRT_PRC { get; set; }
        public int RN_SEQ_REC_PRC { get; set; }
        public string LetterId { get; set; }
        public string AccountNumber { get; set; }
        public string Ssn { get; set; }
        public bool OnEcorr { get; set; }
        public string ValidEcorrEmail { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public bool ValidAddress { get; set; }
        public string LoanSaleStatus { get; set; }
        public string RegionDeconversion { get; set; }
        public string SellingOwnerId { get; set; }
        public string TransferDate { get; set; }
        public string DelayCancelCode { get; set; }
        public string AchStatus { get; set; }
        public string AchSuspensionReason { get; set; }
        public string LastPaymentDate { get; set; }
        public string LastPaymentSource { get; set; }
        public string LastPaymentSubSource { get; set; }
        public string DueDay { get; set; }
        public bool IsCoborrower { get; set; }
        public string LetterAccountNumber { get; set; }
    }
}