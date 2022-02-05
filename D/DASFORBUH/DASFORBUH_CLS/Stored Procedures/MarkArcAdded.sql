CREATE PROCEDURE [dasforbuh].[MarkArcAdded]
	@ProcessQueueId INT,
	@ArcAddProcessingId INT
AS

	UPDATE
		dasforbuh.ProcessQueue
	SET
		ArcAddProcessingId = @ArcAddProcessingId
	WHERE
		ProcessQueueId = @ProcessQueueId

RETURN 0
