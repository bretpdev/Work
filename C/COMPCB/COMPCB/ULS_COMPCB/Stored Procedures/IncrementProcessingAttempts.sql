CREATE PROCEDURE [compcb].[IncrementProcessingAttempts]
	@ProcessingQueueId INT
AS

	UPDATE
		compcb.ProcessingQueue
	SET
		ProcessingAttempts = ProcessingAttempts + 1
	WHERE
		ProcessingQueueId = @ProcessingQueueId

RETURN 0
