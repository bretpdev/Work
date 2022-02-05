CREATE PROCEDURE [qworkerlgp].[UpdateProcessedAt]
	@QueueId INT,
	@HadError BIT
AS
	UPDATE
		qworkerlgp.Queues
	SET
		ProcessedAt = GETDATE(),
		HadError = @HadError
	WHERE
		QueueId = @QueueId
RETURN 0
