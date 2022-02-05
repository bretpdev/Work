CREATE PROCEDURE [olpayrevr].[SetProcessed]
	@ProcessingQueueId INT,
	@HadError BIT,
	@ErrorDescription VARCHAR(500) = ''

AS
	
	UPDATE
		olpayrevr.ReversalsProcessingQueue
	SET
		ProcessedAt = GETDATE(),
		HadError = @HadError,
		ErrorDescription = @ErrorDescription
	WHERE
		ProcessingQueueId = @ProcessingQueueId

RETURN 0
