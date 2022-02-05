CREATE PROCEDURE [dbo].[GetRelatedScriptsForLoginId]
	@LoginId int = null
AS
	SELECT
		S.SackerScriptId
	FROM
		LoginScriptTracking LST
	INNER JOIN Scripts S
		ON S.ScriptId = LST.ScriptId
	WHERE
		LST.LoginId = @LoginId
	
RETURN 0

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRelatedScriptsForLoginId] TO [db_executor]
    AS [dbo];

