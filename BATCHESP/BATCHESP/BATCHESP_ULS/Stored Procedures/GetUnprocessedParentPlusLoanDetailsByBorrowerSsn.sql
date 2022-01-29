CREATE PROCEDURE [batchesp].[GetUnprocessedParentPlusLoanDetailsByBorrowerSsn]
	@BorrowerSsn CHAR(9)
AS

	SELECT
		PPLD.ParentPlusLoanDetailsId,
		PPLD.BorrowerSsn,
		PPLD.StudentSsn,
		PPLD.LoanSequence,
		PPLD.DefermentRequested,
		PPLD.PostEnrollmentDefermentEligible
	FROM
		batchesp.ParentPlusLoanDetails PPLD
	WHERE
		PPLD.ProcessedAt IS NULL
		AND BorrowerSSN = @BorrowerSSN

RETURN 0
