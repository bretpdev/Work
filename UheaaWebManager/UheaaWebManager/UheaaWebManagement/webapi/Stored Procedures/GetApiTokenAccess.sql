CREATE PROCEDURE [webapi].[GetApiTokenAccess]
	@Token VARCHAR(36) = NULL
AS

	DECLARE @Today DATETIME = GETDATE()

	DECLARE @ValidApiTokenId INT

	SELECT
		@ValidApiTokenId = ApiTokenId
	FROM
		webapi.ApiTokens
	WHERE
		CAST(GeneratedToken AS VARCHAR(36)) = @Token
		AND
		(@Today BETWEEN StartDate AND ISNULL(EndDate, @Today))
		AND
		InactivatedAt IS NULL

	SELECT 
		c.[Name] AS [ControllerName],
		c.ControllerId,
		atca.ControllerActionId,
		ca.ActionName
	FROM
		webapi.ApiTokenControllerActions atca
		INNER JOIN webapi.ControllerActions ca on ca.ControllerActionId = atca.ControllerActionId
		INNER JOIN webapi.Controllers c ON c.ControllerId = ca.ControllerId
	WHERE
		atca.ApiTokenId = @ValidApiTokenId
		AND 
		InactivatedAt IS NULL

RETURN 0
