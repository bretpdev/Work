CREATE PROCEDURE [batchesp].[GetUnprocessedTs2hPendingDisbursementsByBorrowerSsn]
	@BorrowerSsn CHAR(9)
AS
	SELECT
		Ts2hPendingDisbursementId,
		BorrowerSsn,
		LoanSequence,
		DisbSequence,
		DisbType,
		DisbursementDate
						
	FROM
		[batchesp].[Ts2hPendingDisbursements]
	WHERE
		ProcessedAt IS NULL
		AND BorrowerSsn = @BorrowerSsn

RETURN 0
