CREATE PROCEDURE [hrbridge].[GetEmployeeNameFromId]
	@EmployeeId INT
AS
	SELECT
		FirstName + ' ' + LastName AS [Name]
	FROM
		hrbridge.Employees_BambooHR
	WHERE
		EmployeeId = @EmployeeId
		AND DeletedAt IS NULL
RETURN 0
