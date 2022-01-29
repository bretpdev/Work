CREATE PROCEDURE [accurint].[SetResponseFilesDownloaded]
	@RunId INT,
	@ResponseFilesDownloadedAt DATETIME
AS
	UPDATE
		accurint.RunLogger
	SET
		ResponseFilesDownloadedAt = @ResponseFilesDownloadedAt
	WHERE
		RunId = @RunId
		
RETURN 0