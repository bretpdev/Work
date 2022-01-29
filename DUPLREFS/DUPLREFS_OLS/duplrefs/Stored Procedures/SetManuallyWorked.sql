CREATE PROCEDURE [duplrefs].[SetManuallyWorked]
	@ReferenceQueueId INT,
	@ManuallyWorked BIT

AS

	UPDATE
		duplrefs.ReferenceQueue
	SET
		ManuallyWorked = @ManuallyWorked
	WHERE
		ReferenceQueueId = @ReferenceQueueId

RETURN 0
