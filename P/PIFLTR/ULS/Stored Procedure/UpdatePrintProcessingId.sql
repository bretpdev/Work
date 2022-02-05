CREATE PROCEDURE [pifltr].[UpdatePrintProcessingId]
	@ProcessQueueId INT,
	@PrintProcessingId INT
AS
	UPDATE
		ULS.pifltr.ProcessingQueue 
	SET
		PrintProcessingId = @PrintProcessingId
	WHERE
		ProcessQueueId = @ProcessQueueId
		AND PrintProcessingId IS NULL

RETURN 0


GO
