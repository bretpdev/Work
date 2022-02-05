CREATE PROCEDURE [compcb].[SetDeletedAt]
	@ProcessingQueueId INT
AS
	
	UPDATE
		compcb.ProcessingQueue
	SET
		DeletedAt = GETDATE(),
		DeletedBy = SUSER_SNAME()
	WHERE
		ProcessingQueueId = @ProcessingQueueId


RETURN 0
