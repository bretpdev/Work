CREATE PROCEDURE [curracclat].[GetData]
AS
	SELECT
		ProcessDataId,
		Ssn
	FROM
		[curracclat].ProcessData
	WHERE
		DeletedAt IS NULL
		AND ProcessedAt IS NULL