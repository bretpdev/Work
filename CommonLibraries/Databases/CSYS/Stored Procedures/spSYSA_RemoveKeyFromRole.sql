
/********************************************************
*Routine Name	: [dbo].[spSYSA_AddKeyToRole]
*Purpose		: 
*Revision History
*Version	Date		Person			Description
*=======	==========	============	================
*1.0.0		07/31/2012  Bret Pehrson
*1.0.1		
********************************************************/

CREATE PROCEDURE [dbo].[spSYSA_RemoveKeyFromRole]
	-- Add the parameters for the stored procedure here
	  @RoleID int = 0
	, @UserKeyID int = '0'
	, @SqlUserID int = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @EndDate AS DateTime = GetDate()
	
    -- Insert statements for procedure here
	UPDATE SYSA_DAT_RoleKeyAssignment
	SET EndDate = @EndDate, RemovedBy = @SqlUserID
	WHERE RoleID = @RoleID
	AND UserKeyID = @UserKeyID
	AND EndDate IS NULL

	SET NOCOUNT OFF;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spSYSA_RemoveKeyFromRole] TO [db_executor]
    AS [dbo];

