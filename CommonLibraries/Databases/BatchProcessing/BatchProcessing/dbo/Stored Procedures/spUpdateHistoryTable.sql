
/********************************************************
*Version    Date        Person                  Description
*=======    ==========  ============      ================
*1.0.0            07/17/2012  Jarom Ryan        Will insert data into history table before data is updated   
********************************************************/

CREATE PROCEDURE [dbo].[spUpdateHistoryTable]

      @UserId           AS VARCHAR(50),
      @Requestor  AS VARCHAR(50)
      
      
AS
BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
      DECLARE @Password VARCHAR(50)
      DECLARE @Notes VARCHAR(100)
      
      SELECT @Password = dbo.Decryptor(EncrypedPassword) , @Notes = Notes
      FROM dbo.[Login]
      WHERE UserName = @UserId
      
      SET NOCOUNT ON;

      INSERT INTO dbo.LoginHistory([UserName],EncryptedPassword,[Notes],[LastUpdated],[Requestor])
      VALUES(@UserId,dbo.Encrypt(@Password),@Notes,GETDATE(),@Requestor)

      SET NOCOUNT OFF;
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spUpdateHistoryTable] TO [db_executor]
    AS [dbo];

