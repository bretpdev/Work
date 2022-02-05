
/********************************************************

*Version    Date        Person                  Description
*=======    ==========  ============      ================
*1.0.0            07/17/2012  Jarom Ryan        Gets the BANKO user id
            
********************************************************/

CREATE PROCEDURE [dbo].[spBANKOGetUserId]

AS
BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
      SET NOCOUNT ON;

    -- Insert statements for procedure here
      SELECT 
              UserName
      From dbo.[Login]
      Where UserName = 'b1020760'

      SET NOCOUNT OFF;
END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spBANKOGetUserId] TO [db_executor]
    AS [dbo];

