
/********************************************************
*Routine Name	: [dbo].[spSYSA_DeleteKey]
*Purpose		: Sets the key to inactive
*Revision History
*Version	Date		Person			Description
*=======	==========	============	================
*1.0.0		08/13/2012  Bret Pehrson
*1.0.1		
********************************************************/

CREATE PROCEDURE [dbo].[spSYSA_DeleteKey]
	-- Add the parameters for the stored procedure here
	  @Application varchar(100) = ''
	, @UserKey varchar(100) = ''
	, @SqlUserID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @CurrentDate AS DateTime = GetDate()

    -- Insert statements for procedure here
	UPDATE SYSA_LST_UserKeys
	SET RemovedBy = @SqlUserID,
	EndDate = @CurrentDate
	WHERE [Application] = @Application
	AND UserKey = @UserKey 

	SET NOCOUNT OFF;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spSYSA_DeleteKey] TO [db_executor]
    AS [dbo];

