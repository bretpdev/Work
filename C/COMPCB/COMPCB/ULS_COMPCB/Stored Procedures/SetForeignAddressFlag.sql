CREATE PROCEDURE [compcb].[SetForeignAddressFlag]
	@ProcessingQueueId INT
AS
	
	UPDATE
		compcb.ProcessingQueue
	SET
		IsForeignAddress = 1
	WHERE
		ProcessingQueueId = @ProcessingQueueId

RETURN 0
