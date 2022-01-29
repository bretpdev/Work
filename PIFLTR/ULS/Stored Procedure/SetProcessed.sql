CREATE PROCEDURE [pifltr].[SetProcessed]
@ProcessQueueId INT

AS

	UPDATE
		PQ
	SET
		PQ.ProcessedAt = GETDATE()
	FROM
		ULS.pifltr.ProcessingQueue PQ
	WHERE
		PQ.ProcessQueueId = @ProcessQueueId
	
GO