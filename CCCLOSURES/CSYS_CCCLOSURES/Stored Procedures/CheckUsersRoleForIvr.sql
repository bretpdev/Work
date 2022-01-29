CREATE PROCEDURE [ccclosures].[CheckUsersRoleForIvr]
	@WindowsUserId varchar(50)
AS
	
	SELECT
		COUNT(*)
	FROM
		SYSA_DAT_Users U
		INNER JOIN [ccclosures].IvrRoles I
			ON I.RoleID = U.[Role]
	WHERE
		WindowsUserName = @WindowsUserId
RETURN 0