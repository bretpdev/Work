CREATE PROCEDURE [webmanager].[GetRoleById]
	@RoleId INT
AS
	
	SELECT
		R.RoleId,
		R.ActiveDirectoryRoleName,
		R.Notes,
		R.InactivatedAt
	FROM
		webapi.Roles R
	WHERE
		R.RoleId = @RoleId
	GROUP BY
		R.RoleId, R.ActiveDirectoryRoleName, R.Notes, R.InactivatedAt

RETURN 0
