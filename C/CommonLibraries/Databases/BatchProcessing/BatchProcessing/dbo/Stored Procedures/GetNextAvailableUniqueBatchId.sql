
CREATE PROCEDURE [dbo].[GetNextAvailableUniqueBatchId]
	@SackerScriptId VARCHAR(10),
	@LoginType INT,
	@InactiveIDs InactiveIDs READONLY
AS
BEGIN TRANSACTION

DECLARE
	@UserName VARCHAR(128) = NULL,
	@Password VARCHAR(128) = NULL,
	@LoginId INT

	SELECT
		@LoginId = COALESCE(SL.LoginId, L.LoginId),
		@UserName = COALESCE(SL.UserName, L.UserName),
		@Password = dbo.Decryptor(COALESCE(SL.EncrypedPassword, L.EncrypedPassword))
	FROM
		[Login] L
		INNER JOIN LoginType LT
			ON LT.LoginTypeId = L.LoginTypeId
		LEFT JOIN
		( -- Script Logins (SL)
			SELECT
				L.UserName,
				L.EncrypedPassword,
				L.LoginId
			FROM
				[Login] L
				INNER JOIN LoginType LT
					ON LT.LoginTypeId = @LoginType
				INNER JOIN Scripts S
					ON S.SackerScriptId = @SackerScriptId
				INNER JOIN LoginScriptTracking ST
					ON ST.LoginId = L.LoginId
					AND ST.ScriptId = S.ScriptId
				LEFT JOIN InvalidLoginTracking ILT
					ON ILT.LoginId = L.LoginId
					AND ILT.InActivated = 0--WAS NOT INACTIVATED JUST COULD NOT LOGIN
					AND DATEDIFF(MINUTE, ILT.AddedAt, GETDATE()) < 30
			WHERE
				ILT.LoginId IS NULL
		) SL
			ON SL.LoginId = L.LoginId
		LEFT JOIN 
		(
			SELECT
				COUNT(CONTEXT_INFO) AS [Count],
				CAST(CONTEXT_INFO AS VARCHAR(128)) AS [User]
			FROM
				sys.dm_exec_sessions
			WHERE
				CONTEXT_INFO IS NOT NULL AND CONTEXT_INFO != 0x
			GROUP BY
				CAST(CONTEXT_INFO AS VARCHAR(128))
		) C
			ON C.[User] = L.UserName
		LEFT JOIN @InactiveIDs IID ON IID.UserName = L.UserName
		LEFT JOIN InvalidLoginTracking ILT
			ON ILT.LoginId = L.LoginId
			AND ILT.InActivated = 0--WAS NOT INACTIVATED JUST COULD NOT LOGIN
			AND DATEDIFF(MINUTE, ILT.AddedAt, GETDATE()) < 30
				
	WHERE
		(
			C.[Count] IS NULL
			OR C.[Count] < LT.MaxInUse
		)
		AND Active = 1
		AND IID.UserName IS NULL
		AND LT.LoginTypeId = @LoginType
		AND ILT.LoginId IS NULL
	ORDER BY
		L.LoginId 
	DESC

	IF @UserName IS NOT NULL
	BEGIN
		DECLARE @Binary VARBINARY(128) = CAST(@UserName AS VARBINARY(128))
		SET CONTEXT_INFO @Binary 
	END

	SELECT
		@UserName [Username],
		@Password [Password],
		@LoginId [LoginId]
COMMIT
		
RETURN 0
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetNextAvailableUniqueBatchId] TO [db_executor]
    AS [dbo];

