CREATE PROCEDURE [enrqtskllr].[SetArcAddedAt]
	@ProcessingQueueId INT

AS

	UPDATE
		enrqtskllr.ProcessingQueue
	SET
		ArcAddedAt = GETDATE()
	WHERE
		ProcessingQueueId = @ProcessingQueueId

RETURN 0 