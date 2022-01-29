CREATE PROCEDURE [acurintc].[SetQueueProcessed]
	@ProcessQueueId int
AS
	
	UPDATE
		acurintc.ProcessQueue
	SET
		ProcessedAt = GETDATE()
	WHERE
		ProcessQueueId = @ProcessQueueId

RETURN 0