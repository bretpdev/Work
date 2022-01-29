
/********************************************************
*Routine Name	: [dbo].[spGENR_InactivateUser]
*Purpose		: Change users status to inactive
*Used by		: Systems Support
*Version	Date		Person			Description
*=======	==========	============	================
*1.0.0		
*1.0.1		
********************************************************/

CREATE PROCEDURE [dbo].[spGENR_InactivateUser]
	-- Add the parameters for the stored procedure here
	  @SqlUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE SYSA_DAT_Users
	SET [Status] = 'Inactive'
	WHERE SqlUserId = @SqlUserId

	SET NOCOUNT OFF;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGENR_InactivateUser] TO [db_executor]
    AS [dbo];

