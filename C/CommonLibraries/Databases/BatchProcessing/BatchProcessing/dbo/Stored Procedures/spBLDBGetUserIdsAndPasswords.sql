
/********************************************************

Date        Person                  Description
==========  ============      ================
07/11/2012  Jarom Ryan        will gather userid's and decrpyt passwords from DB

********************************************************/

CREATE PROCEDURE [dbo].[spBLDBGetUserIdsAndPasswords]

AS
BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
      SET NOCOUNT ON;

      SELECT
      UserName,
	  dbo.Decryptor(EncrypedPassword) as DecryptedPassword,
      Notes
      
      FROM  dbo.[Login]
            

      SET NOCOUNT OFF;
END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spBLDBGetUserIdsAndPasswords] TO [db_executor]
    AS [dbo];

