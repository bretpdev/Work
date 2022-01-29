CREATE PROCEDURE [compcb].[SetProcessedAt]
	@ProcessingQueueId INT
AS

	UPDATE
		compcb.ProcessingQueue
	SET
		ProcessedAt = GETDATE()
	WHERE
		ProcessingQueueId = @ProcessingQueueId

RETURN 0
