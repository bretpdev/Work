CREATE PROCEDURE [finalrev].[GetTaskCount]
AS
	SELECT
		COUNT(BorrowerRecordID)
	FROM
		finalrev.BorrowerRecord
	WHERE
		CAST(ProcessedAt AS DATE) = CAST(GETDATE() AS DATE)
		AND DeletedAt IS NULL
RETURN 0