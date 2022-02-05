CREATE PROCEDURE [dbo].[InsertLoginScriptTracking]
	@SackerScriptId VARCHAR(10),
	@MaxLogins INT,
	@Relateduserids [dbo].[RelatedUserIds] READONLY
AS

IF EXISTS(SELECT SackerScriptId FROM Scripts WHERE SackerScriptId = @SackerScriptId)
BEGIN
	UPDATE
		Scripts
	SET
		MaxLogins = @MaxLogins
END
ELSE
BEGIN
	INSERT INTO Scripts(SackerScriptId, MaxLogins)
	VALUES(@SackerScriptId, @MaxLogins)
END


DELETE 
	LST
FROM 
	LoginScriptTracking LST 
	INNER JOIN Scripts S 
		ON S.SackerScriptId = @SackerScriptId


INSERT INTO LoginScriptTracking(LoginId, ScriptId)
(
	SELECT
		L.LoginId,
		S.ScriptId
	FROM
		@Relateduserids U
	INNER JOIN [Login] L 
		ON L.UserName = U.UserId
	INNER JOIN Scripts S 
		ON S.SackerScriptId = @SackerScriptId
)
RETURN 0

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertLoginScriptTracking] TO [db_executor]
    AS [dbo];

