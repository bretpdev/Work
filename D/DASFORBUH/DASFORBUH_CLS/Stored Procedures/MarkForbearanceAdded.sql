CREATE PROCEDURE [dasforbuh].[MarkForbearanceAdded]
	@ProcessQueueId INT
AS

	UPDATE
		dasforbuh.ProcessQueue
	SET
		ForbearanceAddedOn = GETDATE()
	WHERE
		ProcessQueueId = @ProcessQueueId

RETURN 0
