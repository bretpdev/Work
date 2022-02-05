CREATE PROCEDURE [dbo].[spSYSA_UserAccessForApplication] 
	@SqlUserId		INT,
	@Application	VARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;

	IF (@Application = 'NO_SYSTEM' OR @Application = '')
		BEGIN
			SELECT 
				Keys.[Application],
				keys.UserKey AS Name,
				Keys.[Description],
				RoleKey.RoleID,
				Users.SqlUserId
			FROM SYSA_DAT_RoleKeyAssignment RoleKey
				INNER JOIN SYSA_LST_UserKeys Keys
					ON RoleKey.UserKeyID = Keys.ID
				JOIN SYSA_DAT_Users Users
					ON RoleKey.RoleID = Users.[Role]
			WHERE Users.SqlUserId = @SqlUserId
				AND RoleKey.EndDate IS NULL
			ORDER BY Name
		END
	ELSE
		BEGIN
			SELECT 
				Keys.[Application],
				keys.UserKey AS Name,
				Keys.[Description],
				RoleKey.RoleID,
				Users.SqlUserId
			FROM SYSA_DAT_RoleKeyAssignment RoleKey
				INNER JOIN SYSA_LST_UserKeys Keys
					ON RoleKey.UserKeyID = Keys.ID
				JOIN SYSA_DAT_Users Users
					ON RoleKey.RoleID = Users.[Role]
			WHERE Users.SqlUserId = @SqlUserId
				AND RoleKey.EndDate IS NULL
				AND keys.[Application] = @Application
			ORDER BY Name
		END
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spSYSA_UserAccessForApplication] TO [db_executor]
    AS [dbo];


