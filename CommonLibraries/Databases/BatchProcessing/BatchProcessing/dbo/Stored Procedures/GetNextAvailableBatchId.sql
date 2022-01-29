CREATE PROCEDURE [dbo].[GetNextAvailableBatchId]
	@SackerScriptId VARCHAR(10),
	@LoginType INT 
AS
BEGIN TRANSACTION
DECLARE @ScriptId INT = (SELECT DISTINCT ST.ScriptId FROM Scripts S INNER JOIN LoginScriptTracking ST ON ST.ScriptId = S.ScriptId WHERE SackerScriptId = @SackerScriptId)
DECLARE @UserName VARCHAR(128)  = NULL
 IF @ScriptId IS NOT NULL
	BEGIN
		
							SELECT @UserName =
								l.UserName
							FROM 
								[login] l
							INNER JOIN LoginType lt
								on lt.LoginTypeId = l.LoginTypeId
								and lt.LoginTypeId = @LoginType
							INNER JOIN Scripts s
								on s.SackerScriptId = @SackerScriptId
							INNER JOIN LoginScriptTracking st
								on st.LoginId = l.LoginId
								and st.ScriptId = s.ScriptId
							LEFT JOIN 
							(
								SELECT 
									count(context_info) AS [count],
									CAST(context_info AS VARCHAR(128)) AS [user]
								FROM 
									sys.dm_exec_sessions
								WHERE 
									context_info IS NOT NULL AND  context_info != 0x
								GROUP BY 
									cast(context_info as varchar(128))
							) c
								ON c.[user] = l.UserName
							WHERE
								(c.[count] IS NULL OR 
								c.[count] < lt.MaxInUse)
								and l.Active = 1
							ORDER BY
								L.LoginId 
							DESC
						
END
ELSE
	BEGIN
						SELECT @UserName =
								l.UserName
							FROM 
								[login] l
							INNER JOIN LoginType lt
								on lt.LoginTypeId = l.LoginTypeId
								and lt.LoginTypeId = @LoginType
							LEFT JOIN 
							(
								SELECT 
									COUNT(context_info) AS [count],
									CAST(context_info AS VARCHAR(128)) AS [user]
								FROM 
									sys.dm_exec_sessions
								WHERE 
									context_info IS NOT NULL AND  context_info != 0x
								GROUP BY 
									CAST(context_info AS VARCHAR(128))
							) c
								ON c.[user] = l.UserName
							WHERE
								(c.[count] is null or 
								c.[count] < lt.MaxInUse)
								and l.Active = 1
							ORDER BY
								L.LoginId 
							DESC
						
	END

	IF  @UserName is not null
	begin
		DECLARE @binary VARBINARY(128) = CAST(@UserName as VARBINARY(128))
		SET context_info @binary 
	end

	SELECT
		L.UserName,
		dbo.Decryptor(EncrypedPassword) as [Password]
	FROM
		[Login] L
	WHERE
		L.UserName = @UserName
COMMIT
		
RETURN 0

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetNextAvailableBatchId] TO [db_executor]
    AS [dbo];

