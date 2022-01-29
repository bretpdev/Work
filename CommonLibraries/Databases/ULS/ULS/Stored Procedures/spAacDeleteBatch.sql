-- =============================================
-- Author:		Jarom Ryan
-- Create date: 04/03/2013
-- Description:	This sp will delete a given record from dbo.AacDeleteBatchesRecovery
-- =============================================
CREATE PROCEDURE [dbo].[spAacDeleteBatch]
	
	@BatchNumber VARCHAR(10)
	
AS
BEGIN

	SET NOCOUNT ON;

    DELETE
    FROM	
		dbo.AacDeleteBatchesRecovery
	WHERE
		MajorBatchToDelete = @BatchNumber
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spAacDeleteBatch] TO [db_executor]
    AS [dbo];

