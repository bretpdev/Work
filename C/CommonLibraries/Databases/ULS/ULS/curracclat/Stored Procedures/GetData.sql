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

GRANT EXECUTE ON [curracclat].[GetData] TO db_executor
GO
GRANT EXECUTE
    ON OBJECT::[curracclat].[GetData] TO [db_executor]
    AS [dbo];

