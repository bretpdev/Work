CREATE PROCEDURE [clschllnfd].[UpdateErrorLogArcAddId]
	@BorrowerSsn CHAR(9),
	@ArcAddProcessingId BIGINT
AS
	UPDATE
		clschllnfd.ErrorLogs
	SET
		ArcAddProcessingId = @ArcAddProcessingId
	WHERE
		BorrowerSsn = @BorrowerSsn
		AND CAST(AddedAt AS DATE) = CAST(GETDATE() AS DATE) --All ErrorLog records for the same bwr on same day get only one Arc on account
RETURN 0
