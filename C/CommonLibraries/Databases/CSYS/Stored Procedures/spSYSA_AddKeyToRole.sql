
/********************************************************
*Routine Name	: [dbo].[spSYSA_AddKeyToRole]
*Purpose		: 
*Revision History
*Version	Date		Person			Description
*=======	==========	============	================
*1.0.0		07/31/2012  Bret Pehrson
*1.0.1		
********************************************************/

CREATE PROCEDURE [dbo].[spSYSA_AddKeyToRole]
	-- Add the parameters for the stored procedure here
	  @RoleID int = 0
	, @UserKeyID int = 0
	, @SqlUserID int = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @StartDate AS DateTime = GetDate()
	
    -- Insert statements for procedure here
	INSERT INTO SYSA_DAT_RoleKeyAssignment(RoleID, UserKeyID, StartDate, AddedBy)
	VALUES(@RoleID, @UserKeyID, @StartDate, @SqlUserID)

	SET NOCOUNT OFF;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spSYSA_AddKeyToRole] TO [db_executor]
    AS [dbo];

