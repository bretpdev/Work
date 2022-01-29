-- =============================================
-- Author:		Jarom Ryan
-- Create date: 12/20/2013
-- Description:	Deletes a campaign from BatchEmail
-- =============================================
CREATE PROCEDURE  [dbo].[DeleteBatchEmailCampaign]

@BatchEmailId int

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE FROM	
		[dbo].[BatchEmail]
	WHERE
		[BatchEmailId] = @BatchEmailId
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteBatchEmailCampaign] TO [db_executor]
    AS [dbo];



