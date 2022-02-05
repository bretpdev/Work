CREATE PROCEDURE [webmanager].[GetRoles]
AS
	
	SELECT
		R.RoleId, 
		R.ActiveDirectoryRoleName, 
		R.Notes,
		COUNT(DISTINCT RCA.ControllerActionId) [RoleControllerCount]
	FROM
		webapi.Roles R
		LEFT JOIN webapi.RoleControllerActions RCA ON RCA.RoleId = R.RoleId AND RCA.InactivatedAt IS NULL
	WHERE
		R.InactivatedAt IS NULL
	GROUP BY
		R.RoleId, R.ActiveDirectoryRoleName, R.Notes

RETURN 0
