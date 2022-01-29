CREATE PROCEDURE [fp].[GetRecordCount]
	@ScriptId varchar(10)
AS
	SELECT
		COUNT(FileProcessingId)
	FROM
		[fp].FileProcessing
	WHERE
		ProcessedAt IS NULL
		AND Active = 1
		AND ScriptFileId = (SELECT ScriptFileId FROM [fp].ScriptFiles WHERE ScriptID = @ScriptId)

RETURN 0
GO
GRANT EXECUTE
    ON OBJECT::[fp].[GetRecordCount] TO [db_executor]
    AS [UHEAA\Developers];

