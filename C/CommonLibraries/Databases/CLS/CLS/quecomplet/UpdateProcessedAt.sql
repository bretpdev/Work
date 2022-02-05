CREATE PROCEDURE [quecomplet].[UpdateProcessedAt]
	@QueueId INT,
	@HadError BIT
AS
	UPDATE
		CLS.quecomplet.Queues
	SET
		ProcessedAt = GETDATE(),
		HadError = @HadError
	WHERE 
		QueueId = @QueueId
RETURN 0
