
/********************************************************
*Routine Name	: [dbo].[spSYSA_IsRoleNameInactive]
*Purpose		: Checks to see if a Role Name is inactive
*Revision History
*Version	Date		Person			Description
*=======	==========	============	================
*1.0.0		08/06/2012  Bret Pehrson
*1.0.1		
********************************************************/

CREATE PROCEDURE [dbo].[spSYSA_IsRoleNameInactive]
	-- Add the parameters for the stored procedure here
	  @RoleName nvarchar(64)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT RoleID, RoleName, AddedBy, StartDate, RemovedBy, EndDate FROM SYSA_LST_Role
	WHERE RoleName = @RoleName
	AND EndDate IS NOT NULL
	ORDER BY RoleID DESC

	SET NOCOUNT OFF;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spSYSA_IsRoleNameInactive] TO [db_executor]
    AS [dbo];

