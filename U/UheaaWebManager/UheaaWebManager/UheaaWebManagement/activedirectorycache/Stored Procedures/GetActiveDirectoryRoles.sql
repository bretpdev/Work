CREATE PROCEDURE [activedirectorycache].[GetActiveDirectoryRoles]
AS

	SELECT
		R.RoleId,
		R.ActiveDirectoryRoleName,
		G.GroupId
	FROM
		webapi.Roles R
		LEFT JOIN activedirectorycache.Groups G
			ON G.RoleId = R.RoleId
	WHERE
		InactivatedAt IS NULL

RETURN 0
