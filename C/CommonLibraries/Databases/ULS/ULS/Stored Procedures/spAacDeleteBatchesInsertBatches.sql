-- =============================================
-- Author:		Jarom Ryan
-- Create date: 04/03/2013
-- Description:	Will insert rows into dbo.AacDeleteBatchesRecovery
-- =============================================
CREATE PROCEDURE [dbo].[spAacDeleteBatchesInsertBatches]
	
	@MajorBatch VARCHAR(10),
	@UserId VARCHAR(7)
AS
BEGIN

	SET NOCOUNT ON;

	INSERT INTO dbo.AacDeleteBatchesRecovery(MajorBatchToDelete,UserId)
	VALUES(@MajorBatch,@UserId)

END



GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spAacDeleteBatchesInsertBatches] TO [db_executor]
    AS [dbo];

