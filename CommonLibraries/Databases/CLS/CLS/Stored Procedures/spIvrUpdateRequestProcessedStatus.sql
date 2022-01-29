-- =============================================
-- Author:		Jarom Ryan
-- Create date: 01/26/2012
-- Description:	Will update the IvrRequestTracking table one the record has been updated
-- =============================================
CREATE PROCEDURE [dbo].[spIvrUpdateRequestProcessedStatus]

	@RecNum		BigInt 
	
AS
BEGIN

	SET NOCOUNT ON;

	UPDATE	IvrRequestTracking 
	SET		ProcessedDate = GETDATE()
	WHERE	RecNum = @RecNum
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spIvrUpdateRequestProcessedStatus] TO [db_executor]
    AS [dbo];



