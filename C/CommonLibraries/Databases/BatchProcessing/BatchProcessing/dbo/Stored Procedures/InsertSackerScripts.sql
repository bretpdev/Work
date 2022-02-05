CREATE PROCEDURE [dbo].[InsertSackerScripts]
	@SelectedSackerScripts [dbo].[SackerScripts] READONLY
AS
INSERT INTO Scripts(SackerScriptId)
(
	SELECT
		ss.SackerScript
	FROM
		@SelectedSackerScripts ss
	LEFT JOIN Scripts s 
		ON s.SackerScriptId = ss.SackerScript
	WHERE
		s.SackerScriptId is null
)
RETURN 0
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertSackerScripts] TO [db_executor]
    AS [dbo];

