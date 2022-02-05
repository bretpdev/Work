
/********************************************************
*Routine Name	: [dbo].[spSYSA_UpdateRole]
*Purpose		: Updates the name of the role
*Revision History
*Version	Date		Person			Description
*=======	==========	============	================
*1.0.0		08/06/2012  Bret Pehrson
*1.0.1		
********************************************************/

CREATE PROCEDURE [dbo].[spSYSA_UpdateRole]
	-- Add the parameters for the stored procedure here
	  @RoleName nvarchar(64)
	, @RoleID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE dbo.SYSA_LST_Role
	SET RoleName = @RoleName
	WHERE RoleID = @RoleID

	SET NOCOUNT OFF;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spSYSA_UpdateRole] TO [db_executor]
    AS [dbo];

