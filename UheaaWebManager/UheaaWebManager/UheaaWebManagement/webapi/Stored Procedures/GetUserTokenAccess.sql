CREATE PROCEDURE [webapi].[GetUserTokenAccess]
	@Token VARCHAR(36) = NULL
AS

	DECLARE @Today DATETIME = GETDATE()

	DECLARE @ValidUserTokenId INT

	SELECT
		@ValidUserTokenId = UserTokenId
	FROM
		webapi.UserTokens
	WHERE
		CAST(GeneratedToken AS VARCHAR(36)) = @Token
		AND
		(@Today BETWEEN AddedAt AND TokenExpiresAt)

	SELECT 
		c.[Name] AS [ControllerName],
		CA.ControllerId,
		CA.ControllerActionId,
		CA.ActionName
	FROM
		webapi.UserTokens UT
		INNER JOIN activedirectorycache.Users U on U.AssociatedWindowsUsername = UT.AssociatedWindowsUsername
		INNER JOIN activedirectorycache.UserGroups UG on UG.UserId = U.UserId
		INNER JOIN activedirectorycache.Groups G on G.GroupId = UG.GroupId
		INNER JOIN webapi.Roles R ON R.RoleId = G.RoleId
		LEFT JOIN webapi.RoleControllerActions RCA ON R.RoleId = RCA.RoleId AND RCA.InactivatedAt IS NULL
		LEFT JOIN webapi.ControllerActions CA ON CA.ControllerActionId = RCA.ControllerActionId
		LEFT JOIN webapi.Controllers C ON C.ControllerId = CA.ControllerId
	WHERE
		UT.UserTokenId = @ValidUserTokenId

RETURN 0
