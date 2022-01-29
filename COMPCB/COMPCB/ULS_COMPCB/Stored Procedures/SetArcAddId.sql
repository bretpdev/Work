CREATE PROCEDURE [compcb].[SetArcAddId]
	@ProcessingQueueId INT,
	@ArcAddProcessingId BIGINT
AS

	UPDATE
		compcb.ProcessingQueue
	SET
		ArcAddProcessingId = @ArcAddProcessingId
	WHERE
		ProcessingQueueId = @ProcessingQueueId

RETURN 0
