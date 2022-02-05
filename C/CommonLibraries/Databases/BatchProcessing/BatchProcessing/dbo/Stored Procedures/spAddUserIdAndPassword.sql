
/********************************************************

*Version    Date        Person                  Description
*=======    ==========  ============      ================
*1.0.0            07/16/2012  Jarom Ryan        Will insert records into the dbo.BLDB_DAT_UserIdAndPassword table
            
********************************************************/

CREATE PROCEDURE [dbo].[spAddUserIdAndPassword]
      -- Add the parameters for the stored procedure here
        @UserId VARCHAR(128),
        @Password VARCHAR(128),
        @Notes VARCHAR(100),
		@LoginTypeId INT,
		@Requester VARCHAR(150)
AS
BEGIN
SET XACT_ABORT ON
BEGIN TRANSACTION

	EXEC AddHistoryRecord @UserId, @Password, @LoginTypeId,@Notes,@Requester, 'ADD';

    INSERT INTO dbo.[Login](UserName, EncrypedPassword, Notes, LoginTypeId, Active)
    VALUES(@UserId,dbo.Encrypt(@Password),@notes, @LoginTypeId, 1)

	
			
COMMIT
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spAddUserIdAndPassword] TO [db_executor]
    AS [dbo];

