CREATE PROCEDURE [curracclat].[SetProcessedAt]
	@ProcessDataId INT
AS
	UPDATE
		[curracclat].ProcessData
	SET
		ProcessedAt = GETDATE()
	WHERE
		ProcessDataId = @ProcessDataId

GRANT EXECUTE ON [curracclat].[SetProcessedAt] TO db_executor
GO
GRANT EXECUTE
    ON OBJECT::[curracclat].[SetProcessedAt] TO [db_executor]
    AS [dbo];

