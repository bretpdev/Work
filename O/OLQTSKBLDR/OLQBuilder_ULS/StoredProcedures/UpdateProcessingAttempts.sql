CREATE PROCEDURE [olqtskbldr].[UpdateProcessingAttempts]
	@QueueId INT
AS
	UPDATE
		[olqtskbldr].[Queues]
	SET 
		ProcessingAttempts = ProcessingAttempts + 1
	WHERE
		QueueId = @QueueId
RETURN 0
