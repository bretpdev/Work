CREATE PROCEDURE [dbo].[spSYSA_CheckUserAccessByKey]
	@WindowsUserName varchar(50),
	@UserKey varchar(100)
AS
	DECLARE @RoleId int
	SET @RoleId = (SELECT
						[Role]
					FROM
						SYSA_DAT_Users
					WHERE
						WindowsUserName = @WindowsUserName)

	SELECT
		COUNT(keys.ID)
	FROM
		SYSA_LST_UserKeys keys
		JOIN SYSA_DAT_RoleKeyAssignment assign
			ON keys.ID = assign.UserKeyID
	WHERE
		assign.RoleID = @RoleId
		AND keys.UserKey = @UserKey
		AND assign.EndDate IS NULL
		AND keys.EndDate IS NULL
RETURN 0

GRANT EXECUTE ON spSYSA_CheckUserAccessByKey TO db_executor
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spSYSA_CheckUserAccessByKey] TO [db_executor]
    AS [dbo];

