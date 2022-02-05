CREATE PROCEDURE [accurint].[SetResponseFilesProcessed]
	@RunId INT
AS
	UPDATE
		accurint.RunLogger
	SET
		ResponseFilesProcessedAt = GETDATE()
	WHERE
		RunId = @RunId

RETURN 0
