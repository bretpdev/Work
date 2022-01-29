CREATE PROCEDURE [duplrefs].[SetBorrowerProcessedAt]
	@BorrowerQueueId INT

AS

	UPDATE
		duplrefs.BorrowerQueue
	SET
		ProcessedAt = GETDATE()
	WHERE
		BorrowerQueueId = @BorrowerQueueId

RETURN 0
