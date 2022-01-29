CREATE PROCEDURE [dbo].[AvatierHistoryInsert]
    @EmployeeId VARCHAR(50) = NULL, 
	@UserGuid VARCHAR(50) = NULL,
    @Role VARCHAR(50) = NULL, 
    @Title VARCHAR(50) = NULL, 
    @Department VARCHAR(50) = NULL, 
    @ManagerEmployeeId VARCHAR(50) = NULL, 
    @FirstName VARCHAR(50) = NULL, 
    @MiddleName VARCHAR(50) = NULL, 
    @LastName VARCHAR(50) = NULL, 
    @HireDate DATETIME = NULL, 
    @TerminationDate DATETIME = NULL, 
    @UpdateTypeId INT
AS
	INSERT INTO AvatierHistory (EmployeeId, UserGuid, [Role], Title, Department, ManagerEmployeeId, FirstName, MiddleName, LastName, HireDate, TerminationDate, UpdateTypeId)
	VALUES (@EmployeeId, @UserGuid, @Role, @Title, @Department, @ManagerEmployeeId, @FirstName, @MiddleName, @LastName, @HireDate, @TerminationDate, @UpdateTypeId)

RETURN 0
