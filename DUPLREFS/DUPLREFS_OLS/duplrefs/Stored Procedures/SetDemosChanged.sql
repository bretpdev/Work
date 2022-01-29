CREATE PROCEDURE [duplrefs].[SetDemosChanged]
	@ReferenceQueueId INT,
	@DemosChanged BIT

AS

	UPDATE
		duplrefs.ReferenceQueue
	SET
		DemosChanged = @DemosChanged
	WHERE
		ReferenceQueueId = @ReferenceQueueId

RETURN 0
