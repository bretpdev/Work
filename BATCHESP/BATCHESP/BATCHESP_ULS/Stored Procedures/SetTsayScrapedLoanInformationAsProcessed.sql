CREATE PROCEDURE [batchesp].[SetTsayScrapedLoanInformationAsProcessed]
	@BorrowerSsn CHAR(9),
	@LoanSequence INT
AS
	UPDATE
		batchesp.TsayScrapedLoanInformation
	SET
		ProcessedAt = GETDATE(),
		ProcessedBy = SYSTEM_USER
	WHERE
		BorrowerSsn = @BorrowerSsn
		AND LoanSequence = @LoanSequence
		AND ProcessedAt IS NULL
RETURN 0
