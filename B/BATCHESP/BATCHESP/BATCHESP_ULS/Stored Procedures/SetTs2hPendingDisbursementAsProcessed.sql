CREATE PROCEDURE [batchesp].[SetTs2hPendingDisbursementAsProcessed]
	@Ts2hPendingDisbursementId INT
AS
	UPDATE
		batchesp.Ts2hPendingDisbursements
	SET
		ProcessedAt = GETDATE(),
		ProcessedBy = SYSTEM_USER
	WHERE
		Ts2hPendingDisbursementId = @Ts2hPendingDisbursementId
	
RETURN 0
