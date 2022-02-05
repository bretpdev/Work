CREATE PROCEDURE [duplrefs].[SetReferenceArcAddId]
	@ReferenceQueueId INT,
	@ArcAddProcessingId BIGINT

AS

	UPDATE
		duplrefs.ReferenceQueue
	SET
		ArcAddProcessingId = @ArcAddProcessingId
	WHERE
		ReferenceQueueId = @ReferenceQueueId

RETURN 0
