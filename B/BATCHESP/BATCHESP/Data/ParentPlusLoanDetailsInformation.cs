namespace BATCHESP
{
    /// <summary>
    /// Information pulled back from the DB concerning Parent Plus loans (PLUS, DLPLUS)
    /// </summary>
    public class ParentPlusLoanDetailsInformation
    {
        public int ParentPlusLoanDetailsId { get; set; }
       public string BorrowerSsn{get;set;}
		public string StudentSsn { get; set; }
		public int LoanSequence { get; set; }
		public bool DefermentRequested { get; set; }
		public bool PostEnrollmentDefermentEligible { get; set; }

    }
}
