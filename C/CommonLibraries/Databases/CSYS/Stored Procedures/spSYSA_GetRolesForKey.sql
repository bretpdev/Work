
/********************************************************
*Routine Name	: [dbo].[spSYSA_GetRolesForKey]
*Purpose		: Get a list of all the Roles assigned to the specified key
*Revision History
*Version	Date		Person			Description
*=======	==========	============	================
*1.0.0		11/26/2012  Jay Davis
	
********************************************************/

CREATE PROCEDURE [dbo].[spSYSA_GetRolesForKey]
	@UserKey	varchar(100)
AS
BEGIN
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT	C.RoleName
	FROM	dbo.SYSA_LST_UserKeys A
			INNER JOIN dbo.SYSA_DAT_RoleKeyAssignment B 
				ON A.ID = B.UserKeyID
			INNER JOIN dbo.SYSA_LST_Role C
				ON B.RoleID = C.RoleID
				
	WHERE	A.UserKey = @UserKey
			AND A.EndDate IS NULL
			AND B.EndDate IS NULL
			AND C.EndDate IS NULL
	
	SET NOCOUNT OFF;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spSYSA_GetRolesForKey] TO [db_executor]
    AS [dbo];

