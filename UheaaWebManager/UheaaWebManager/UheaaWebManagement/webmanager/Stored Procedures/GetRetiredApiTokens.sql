CREATE PROCEDURE [webmanager].[GetRetiredApiTokens]
AS
	
	SELECT
		API.ApiTokenId, 
		RIGHT(CAST(API.GeneratedToken AS VARCHAR(36)), 12) GeneratedTokenLast12, 
		API.Notes, 
		COUNT(ATCA.ApiTokenControllerActionId) [ControllerCount]
	FROM
		webapi.ApiTokens API
		LEFT JOIN webapi.ApiTokenControllerActions ATCA ON ATCA.ApiTokenId = API.ApiTokenId AND ATCA.InactivatedAt IS NULL
	WHERE
		EndDate < GETDATE()
		OR 
		API.InactivatedAt IS NOT NULL
	GROUP BY
		API.ApiTokenId, API.GeneratedToken, API.Notes

RETURN 0
