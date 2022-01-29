CREATE PROCEDURE [accurint].[GetRunInfo]
AS
	
	DECLARE @RunId INT = (SELECT MAX(RunId) FROM accurint.RunLogger WHERE DeletedAt IS NULL AND ResponseFilesProcessedAt IS NULL) --Get the last unfinished run

	SELECT
		RunId,
		RequestFileName,
		RequestFileCreatedAt,
		RequestFileUploadedAt,
		ResponseFilesDownloadedAt,
		ResponseFilesProcessedAt,
		RecordsSent,
		RecordsReceived,
		CreatedAt
	FROM
		accurint.RunLogger
	WHERE
		RunId = @RunId

RETURN 0
