
/********************************************************
*Routine Name	: [dbo].[spSYSA_GetRoles]
*Purpose		: Get a list of all the Roles
*Revision History
*Version	Date		Person			Description
*=======	==========	============	================
*1.0.0		07/23/2012  Bret Pehrson
*1.0.1		
********************************************************/

CREATE PROCEDURE [dbo].[spSYSA_GetRoles]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT RoleID, RoleName FROM SYSA_LST_Role
	WHERE EndDate IS NULL

	SET NOCOUNT OFF;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spSYSA_GetRoles] TO [db_executor]
    AS [dbo];

