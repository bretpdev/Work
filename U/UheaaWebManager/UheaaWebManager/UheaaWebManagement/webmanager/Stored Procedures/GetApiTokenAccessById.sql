CREATE PROCEDURE [webmanager].[GetApiTokenAccessById]
	@ApiTokenId INT = NULL
AS
	
	SELECT
		C.ControllerId, 
		C.Name [ControllerName], 
		CA.ActionName,
		CA.ControllerActionId,
	    CAST(CASE WHEN ATCA.ApiTokenControllerActionId IS NOT NULL THEN 1 ELSE 0 END AS BIT) AS [HasAccess]
	FROM
		webapi.Controllers C
		INNER JOIN webapi.ControllerActions CA 
			ON CA.ControllerId = C.ControllerId AND CA.RetiredAt IS NULL
		LEFT JOIN webapi.ApiTokenControllerActions ATCA 
			ON ATCA.ControllerActionId = CA.ControllerActionId AND ATCA.InactivatedAt IS NULL AND ATCA.ApiTokenId = @ApiTokenId
	WHERE
		C.RetiredAt IS NULL

RETURN 0
