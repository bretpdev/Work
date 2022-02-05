
/********************************************************

Date        Person                  Description
==========  ============      ================
07/11/2012   Jarom Ryan       Will delete data in history table older than 3 months
            
********************************************************/

CREATE PROCEDURE [dbo].[spBLDBDeleteOldHistory]
      -- Add the parameters for the stored procedure here
AS
BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
      SET NOCOUNT ON;

    Delete 
    From dbo.BLDB_HST_UserIdAndPassword
    Where LastUpdated < DATEADD(month,-3,GETDATE())
            

      SET NOCOUNT OFF;
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spBLDBDeleteOldHistory] TO [db_executor]
    AS [dbo];

