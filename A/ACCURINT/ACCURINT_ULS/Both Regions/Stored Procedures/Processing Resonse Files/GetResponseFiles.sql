CREATE PROCEDURE [accurint].[GetResponseFiles]
	@RunId INT
AS
	
	SELECT
		ResponseFileId,
		RunId,
		ResponseFileName,
		ArchivedFileName,
		ProcessedAt
	FROM
		accurint.ResponseFileProcessingQueue RFPQ
	WHERE
		RFPQ.DeletedAt IS NULL
		AND RFPQ.RunId = @RunId

RETURN 0
