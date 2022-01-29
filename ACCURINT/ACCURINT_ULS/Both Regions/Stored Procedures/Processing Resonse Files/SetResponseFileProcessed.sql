CREATE PROCEDURE [accurint].[SetResponseFileProcessed]
	@ResponseFileId INT
AS
	
	UPDATE
		accurint.ResponseFileProcessingQueue
	SET
		ProcessedAt = GETDATE()
	WHERE
		ResponseFileId = @ResponseFileId

RETURN 0
