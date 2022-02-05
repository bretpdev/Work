CREATE PROCEDURE [webmanager].[GetRoleAccessById]
	@RoleId INT = NULL
AS
	
	SELECT
		C.ControllerId, 
		C.Name [ControllerName], 
		CA.ActionName,
		CA.ControllerActionId,
	    CAST(CASE WHEN RCA.RoleControllerActionId IS NOT NULL THEN 1 ELSE 0 END AS BIT) AS [HasAccess]
	FROM
		webapi.Controllers C
		INNER JOIN webapi.ControllerActions CA 
			ON CA.ControllerId = C.ControllerId AND CA.RetiredAt IS NULL
		LEFT JOIN webapi.RoleControllerActions RCA 
			ON RCA.ControllerActionId = CA.ControllerActionId AND RCA.InactivatedAt IS NULL AND RCA.RoleId = @RoleId
	WHERE
		C.RetiredAt IS NULL

RETURN 0
