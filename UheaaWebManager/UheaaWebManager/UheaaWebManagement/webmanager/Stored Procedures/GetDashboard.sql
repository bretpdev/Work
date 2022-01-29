CREATE PROCEDURE [webmanager].[GetDashboard]
AS
	
	DECLARE @ApiTokenCount INT
	DECLARE @ExpiredApiTokenCount INT

	DECLARE @RoleCount INT
	DECLARE @ExpiredRoleCount INT

	DECLARE @WebAppCount INT
	DECLARE @ExpiredWebAppCount INT
	
	SELECT 
		@ApiTokenCount = COUNT(*)
	FROM 
		webapi.ApiTokens API
	WHERE
		GETDATE() BETWEEN StartDate AND ISNULL(EndDate, GETDATE())
		AND InactivatedAt IS NULL

	SELECT
		@ExpiredApiTokenCount = COUNT(*)
	FROM
		webapi.ApiTokens API
	WHERE
		InactivatedAt IS NOT NULL
		OR
		GETDATE() < StartDate
		OR
		GETDATE() > EndDate

	SELECT
		@RoleCount = COUNT(*)
	FROM
		webapi.Roles R
	WHERE
		R.InactivatedAt IS NULL

	SELECT
		@ExpiredRoleCount = COUNT(*)
	FROM
		webapi.Roles
	WHERE
		InactivatedAt IS NOT NULL

	SELECT
		@WebAppCount = COUNT(*)
	FROM
		webapps.WebApps WA
	WHERE
		WA.InactivatedAt IS NULL

	SELECT
		@ExpiredWebAppCount = COUNT(*)
	FROM
		webapps.WebApps WA
	WHERE
		WA.InactivatedAt IS NOT NULL

	SELECT
		@ApiTokenCount [ApiTokenCount],
		@ExpiredApiTokenCount [ExpiredApiTokenCount],
		@RoleCount [RoleCount],
		@ExpiredRoleCount [ExpiredRoleCount],
		@WebAppCount [WebAppCount],
		@ExpiredWebAppCount [ExpiredWebAppCount]

RETURN 0
