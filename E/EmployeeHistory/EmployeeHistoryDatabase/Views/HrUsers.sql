CREATE VIEW [dbo].[HrUsers]
	AS 
	
	SELECT 
		AddedAt [Updated Time Stamp],
		HireDate [Start Date],
		TerminationDate [End Date],
		UT.AvatierCode [Provisioning Event],
		[Role] [User Provisioning Role],
		FirstName [First Name],
		MiddleName [Middle Name],
		LastName [Last Name],
		Title,
		Department,
		--LEFT(ISNULL(NULLIF(EmployeeId, ''), REPLACE(UserGUID, '-', '')), 16) [EmployeeID],
		EmployeeId,
		UserGUID [EmployeeNumber],
		ManagerEmployeeId [Manager Employee ID]
	FROM 
		AvatierHistory AH
	JOIN
		UpdateTypes UT on AH.UpdateTypeId = UT.UpdateTypeId

		
		
	
