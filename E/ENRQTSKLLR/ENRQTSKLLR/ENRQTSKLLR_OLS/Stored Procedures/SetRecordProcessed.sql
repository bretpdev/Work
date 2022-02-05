CREATE PROCEDURE [enrqtskllr].[SetRecordProcessed]
	@ProcessingQueueId INT

AS

	UPDATE
		enrqtskllr.ProcessingQueue
	SET
		ProcessedAt = GETDATE()
	WHERE
		ProcessingQueueId = @ProcessingQueueId

RETURN 0
