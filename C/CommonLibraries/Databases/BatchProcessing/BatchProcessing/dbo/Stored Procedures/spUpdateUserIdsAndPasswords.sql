
/********************************************************
*Version    Date        Person                  Description
*=======    ==========  ============      ================
*1.0.0            07/16/2012  Jarom Ryan        This sp will update records in dbo.BLDB_DAT_UserIdAndPassword
            
********************************************************/

CREATE PROCEDURE [dbo].[spUpdateUserIdsAndPasswords]

        @UserId   INT,
		@UserName VARCHAR(128),
        @Password VARCHAR(128),
        @Notes VARCHAR(100),
		@LoginTypeId INT,
		@Requester VARCHAR(150)
AS
BEGIN
SET XACT_ABORT ON
BEGIN TRANSACTION

	EXEC AddHistoryRecord @UserName, @Password, @LoginTypeId,@Notes,@Requester, 'UPDATE';

	UPDATE
		dbo.[Login]
	SET
		EncrypedPassword = dbo.Encrypt(@Password), 
		LoginTypeId = @LoginTypeId,
		Notes = @Notes,
		Active = 1,
		LastUpdated = GETDATE()
	WHERE
		LoginId = @UserId

COMMIT
END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spUpdateUserIdsAndPasswords] TO [db_executor]
    AS [dbo];

