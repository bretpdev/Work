CREATE PROCEDURE [espqueues].[SetProcessedAt]
	@ProcessingQueueId INT

AS

	UPDATE
		espqueues.ProcessingQueue
	SET
		ProcessedAt = GETDATE()
	WHERE
		ProcessingQueueId = @ProcessingQueueId

RETURN 0
