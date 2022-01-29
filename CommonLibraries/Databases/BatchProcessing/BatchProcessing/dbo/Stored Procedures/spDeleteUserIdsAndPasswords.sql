
/********************************************************
*Version    Date        Person                  Description
*=======    ==========  ============      ================
*1.0.0            07/16/2012  Jarom Ryan        Will delete Records from dbo.BLDB_DAT_UserIdAndPassword      
********************************************************/

CREATE PROCEDURE [dbo].[spDeleteUserIdsAndPasswords]
        @LoginId INT,
		@Requestor VARCHAR(150)
AS
BEGIN
SET XACT_ABORT ON
BEGIN TRANSACTION

	INSERT INTO LoginHistory(UserName, EncryptedPassword, LoginTypeId, Notes, Requestor, [Action])
		SELECT 
			L.UserName,
			L.EncrypedPassword,
			L.LoginTypeId,
			L.Notes,
			@Requestor,
			'DELETE'
		FROM
			[Login] L
		WHERE 
			L.LoginId = @LoginId

	DELETE FROM LoginScriptTracking WHERE LoginId = @LoginId
	DELETE FROM [Login] WHERE LoginId = @LoginId

COMMIT

END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spDeleteUserIdsAndPasswords] TO [db_executor]
    AS [dbo];

