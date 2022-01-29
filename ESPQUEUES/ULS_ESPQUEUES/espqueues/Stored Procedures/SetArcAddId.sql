CREATE PROCEDURE [espqueues].[SetArcAddId]
	@ProcessingQueueId INT,
	@ArcAddProcessingId BIGINT

AS

	UPDATE
		espqueues.ProcessingQueue
	SET
		ArcAddProcessingId = @ArcAddProcessingId
	WHERE
		ProcessingQueueId = @ProcessingQueueId

RETURN 0