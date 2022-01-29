CREATE PROCEDURE [dbo].[spSYSA_CheckIfRoleHasAccess]
	@UserKey VARCHAR(100), 
	@RoleName VARCHAR(64)
AS
BEGIN

	SET NOCOUNT ON;
	
	DECLARE @Result bit = 0
	
	SELECT
		@Result = 1
	FROM 
		SYSA_DAT_RoleKeyAssignment a
			JOIN SYSA_LST_UserKeys b
				ON a.UserKeyID = b.ID
			JOIN SYSA_LST_Role c
				ON a.RoleID = c.RoleID
	WHERE 
		c.RoleName = @RoleName
		AND b.UserKey = @UserKey
		AND a.EndDate IS NULL
		
	SELECT @Result
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spSYSA_CheckIfRoleHasAccess] TO [db_executor]
    AS [dbo];

