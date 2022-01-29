-- =============================================
-- Author:		Jarom Ryan
-- Create date: 05/14/2013
-- Description:	This will delete all records for a given userid
-- =============================================
CREATE PROCEDURE [dbo].[spDeleteAllBatchesForUserId]
	
	@UserId VARCHAR(10)
	
AS
BEGIN

	SET NOCOUNT ON;

    DELETE
    FROM 
		dbo.AacDeleteBatchesRecovery
	WHERE UserId = @UserId
    
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spDeleteAllBatchesForUserId] TO [db_executor]
    AS [dbo];

