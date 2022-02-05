CREATE PROCEDURE [espqueues].[SetProcessingStepId]
	@ProcessingStepId INT,
	@ProcessingQueueId INT

AS
	
	UPDATE
		espqueues.ProcessingQueue
	SET
		ProcessingStepId = @ProcessingStepId
	WHERE
		ProcessingQueueId = @ProcessingQueueId
	
RETURN 0
