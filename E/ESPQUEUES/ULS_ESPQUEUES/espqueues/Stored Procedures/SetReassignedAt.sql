CREATE PROCEDURE [espqueues].[SetReassignedAt]
	@ProcessingQueueId INT

AS

	UPDATE
		espqueues.ProcessingQueue
	SET
		ReassignedAt = GETDATE()
	WHERE
		ProcessingQueueId = @ProcessingQueueId

RETURN 0