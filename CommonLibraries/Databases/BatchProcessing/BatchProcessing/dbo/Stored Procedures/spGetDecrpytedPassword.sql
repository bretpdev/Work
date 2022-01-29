
/********************************************************
*Version    Date        Person                  Description
*=======    ==========  ============      ================
*1.0.0            07/172012   Jarom Ryan        Will decrpyt and return passwords from dbo.BLDB_DAT_UserIdAndPassword
      
********************************************************/

CREATE PROCEDURE [dbo].[spGetDecrpytedPassword]
      -- Add the parameters for the stored procedure here
      @UserId VARCHAR(50)
AS
BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
      SET NOCOUNT ON;

    -- Insert statements for procedure here
      SELECT 
			dbo.Decryptor(EncrypedPassword)
      FROM 
			dbo.[Login]
      WHERE 
			UserName = @UserId      
      

      SET NOCOUNT OFF;
END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGetDecrpytedPassword] TO [db_executor]
    AS [dbo];

