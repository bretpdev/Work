CREATE PROCEDURE [quecomplet].[UpdateProcessedAt]
	@QueueId INT,
	@HadError BIT
AS
	UPDATE
		ULS.quecomplet.Queues
	SET
		ProcessedAt = GETDATE(),
		HadError = @HadError
	WHERE 
		QueueId = @QueueId
RETURN 0