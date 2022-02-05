CREATE PROCEDURE [accurint].[CreateNewRun]
AS
	
	DECLARE @RunId INT;

	INSERT INTO [accurint].[RunLogger] DEFAULT VALUES
	SELECT @RunId = SCOPE_IDENTITY()

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
