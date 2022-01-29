CREATE PROCEDURE [print].[GetFileNamesForScript]
(
	@ScriptId VARCHAR(10)
)
AS
BEGIN
	SELECT
		SD.ScriptDataId,
		SD.SourceFile as [FileName],
		SD.ProcessAllFiles
		
	FROM
		[print].ScriptData SD
	WHERE
		SD.ScriptID = @ScriptId
END;