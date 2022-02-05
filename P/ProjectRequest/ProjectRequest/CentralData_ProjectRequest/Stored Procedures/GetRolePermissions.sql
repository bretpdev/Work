CREATE PROCEDURE [projectrequest].[GetRolePermissions]
AS

SELECT
	R.[Role],
	P.[Read],
	P.[Create],
    P.[Score],
    P.[ScoreFinance],
    P.[ScoreRequestor],
    P.[ScoreUrgency],
    P.[ScoreResources],
    P.[Archive],
    P.[Admin]
FROM
	[projectrequest].[Roles] R
	INNER JOIN [projectrequest].[Permissions] P
		ON R.PermissionId = P.PermissionId
WHERE
	R.DeletedAt IS NULL
	AND R.DeletedBy IS NULL