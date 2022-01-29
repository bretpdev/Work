CREATE PROCEDURE [fp].[GetFileNamesForScript]
	@ScriptId varchar(10)
AS
	SELECT
		ScriptFileId,
		SourceFile as [FileName],
		ProcessAllFiles
	FROM
		[fp].ScriptFiles
	WHERE
		ScriptID = @ScriptId

RETURN 0
GO
GRANT EXECUTE
    ON OBJECT::[fp].[GetFileNamesForScript] TO [db_executor]
    AS [dbo];

