CREATE PROCEDURE [duplrefs].[SetReferenceProcessedAt]
	@ReferenceQueueId INT

AS

	UPDATE
		duplrefs.ReferenceQueue
	SET
		ProcessedAt = GETDATE()
	WHERE
		ReferenceQueueId = @ReferenceQueueId

RETURN 0