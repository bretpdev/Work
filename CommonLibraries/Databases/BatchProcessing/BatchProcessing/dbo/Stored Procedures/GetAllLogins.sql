CREATE PROCEDURE [dbo].[GetAllLogins]
	@ScriptId VARCHAR(10)

AS
	SELECT 
		UserName,
		CASE	
			WHEN LST.LoginId IS NULL THEN 0
			ELSE 1
		END AS IsRelated
	FROM
		[Login] L
		LEFT JOIN Scripts S
			ON S.SackerScriptId = @ScriptId
		LEFT JOIN LoginScriptTracking LST
			ON LST.ScriptId = S.ScriptId
			AND LST.LoginId = L.LoginId
RETURN 0

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllLogins] TO [db_executor]
    AS [dbo];

