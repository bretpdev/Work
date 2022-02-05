CREATE PROCEDURE [dbo].[AvatierHistoryGetMostRecent]
	@UserGuid VARCHAR(50)
AS
	SELECT
		AvatierHistoryId,
		EmployeeId, 
		Role, 
		Title, 
		Department, 
		ManagerEmployeeId, 
		FirstName, 
		MiddleName, 
		LastName, 
		HireDate, 
		TerminationDate, 
		UpdateTypeId
	FROM
		AvatierHistory
	WHERE
		UserGuid = @UserGuid
		AND
		AvatierHistoryId = (SELECT TOP 1 AvatierHistoryId FROM AvatierHistory WHERE UserGuid = @UserGuid ORDER BY AddedAt DESC)
RETURN 0
