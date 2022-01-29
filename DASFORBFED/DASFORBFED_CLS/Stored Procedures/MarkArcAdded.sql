CREATE PROCEDURE [dasforbfed].[MarkArcAdded]
	@ProcessQueueId INT,
	@ArcAddProcessingId INT
AS

	UPDATE
		dasforbfed.ProcessQueue
	SET
		ArcAddProcessingId = @ArcAddProcessingId
	WHERE
		ProcessQueueId = @ProcessQueueId

RETURN 0
