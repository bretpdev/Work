CREATE PROCEDURE [duplrefs].[SetReferenceLp2fProcessedAt]
	@ReferenceQueueId INT

AS

	UPDATE
		duplrefs.ReferenceQueue
	SET
		Lp2fProcessedAt = GETDATE()
	WHERE
		ReferenceQueueId = @ReferenceQueueId

RETURN 0