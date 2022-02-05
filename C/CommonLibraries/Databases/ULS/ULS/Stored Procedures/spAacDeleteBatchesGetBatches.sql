-- =============================================
-- Author:		Jarom Ryan
-- Create date: 04/03/2013
-- Description:	Will return all Major batches that need to be delete for a given user id
-- =============================================
CREATE PROCEDURE [dbo].[spAacDeleteBatchesGetBatches]
	@UserId Varchar(7)
	
AS
BEGIN

SET NOCOUNT ON;
	
	SELECT 
		MajorBatchToDelete
	FROM
		dbo.AacDeleteBatchesRecovery
	WHERE 
		UserId = @UserId
    
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spAacDeleteBatchesGetBatches] TO [db_executor]
    AS [dbo];

