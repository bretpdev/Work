CREATE PROCEDURE [achrirdf].[MarkQueueAsProcessed]
	@ProcessQueueId int
AS
	
	UPDATE 
		achrirdf.ProcessQueue
	SET
		ProcessedAt = getdate(), 
		ProcessedBy = SYSTEM_USER
	WHERE
		ProcessQueueId = @ProcessQueueId

RETURN 0
