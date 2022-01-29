CREATE PROCEDURE [webmanager].[GetApiTokenById]
	@ApiTokenId INT
AS
	
	SELECT
		API.ApiTokenId,
		API.GeneratedToken,
		API.StartDate,
		API.EndDate, 
		API.Notes,
		API.InactivatedAt,
		COUNT(DISTINCT ATCA.ControllerActionId) [ControllerCount]
	FROM
		webapi.ApiTokens API
		LEFT JOIN webapi.ApiTokenControllerActions ATCA ON ATCA.ApiTokenId = API.ApiTokenId AND ATCA.InactivatedAt IS NULL
	WHERE
		API.ApiTokenId = @ApiTokenId
	GROUP BY
		API.ApiTokenId, API.GeneratedToken, API.StartDate, API.EndDate, API.Notes, API.InactivatedAt

RETURN 0
