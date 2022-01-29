CREATE PROCEDURE [accurint].[SetArchivedResponseFileName]
	@ResponseFileId INT,
	@ArchivedFileName VARCHAR(260)
AS
	
	UPDATE
		accurint.ResponseFileProcessingQueue
	SET
		ArchivedFileName = @ArchivedFileName
	WHERE
		ResponseFileId = @ResponseFileId

RETURN 0
