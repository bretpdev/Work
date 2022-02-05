CREATE PROCEDURE [dasforbfed].[MarkForbearanceAdded]
	@ProcessQueueId INT
AS

	UPDATE
		dasforbfed.ProcessQueue
	SET
		ForbearanceAddedOn = GETDATE()
	WHERE
		ProcessQueueId = @ProcessQueueId

RETURN 0
