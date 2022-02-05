CREATE PROCEDURE [webmanager].[GetApiTokens]
AS

	SELECT
		API.ApiTokenId, 
		RIGHT(CAST(API.GeneratedToken AS VARCHAR(36)), 12) GeneratedTokenLast12, 
		API.Notes, 
		COUNT(ATCA.ApiTokenControllerActionId) [ControllerCount]
	FROM
		webapi.ApiTokens API
		LEFT JOIN webapi.ApiTokenControllerActions ATCA ON ATCA.ApiTokenId = API.ApiTokenId
	WHERE
		GETDATE() BETWEEN StartDate AND ISNULL(EndDate, GETDATE())
		AND 
		API.InactivatedAt IS NULL
		AND
		ATCA.InactivatedAt IS NULL
	GROUP BY
		API.ApiTokenId, API.GeneratedToken, API.Notes

RETURN 0
