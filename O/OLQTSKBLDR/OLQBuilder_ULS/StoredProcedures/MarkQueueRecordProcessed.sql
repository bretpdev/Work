CREATE PROCEDURE [olqtskbldr].[MarkQueueRecordProcessed]
	@QueueId INT
AS
	UPDATE
		[olqtskbldr].[Queues]
	SET 
		ProcessedAt = GETDATE()
	WHERE
		QueueId = @QueueId
RETURN 0
