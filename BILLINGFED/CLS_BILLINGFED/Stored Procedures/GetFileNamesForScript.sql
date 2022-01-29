
CREATE PROCEDURE billing.[GetFileNamesForScript]
(
	@ScriptId VARCHAR(10)
)
AS
BEGIN
	SELECT
		SF.ScriptFileId,
		SF.SourceFile as [FileName],
		SF.ProcessAllFiles,
		SF.UsesBulkLoadId
	FROM
		billing.ScriptFiles SF
	WHERE
		SF.ScriptID = @ScriptId
		AND SF.Active = 1
END;