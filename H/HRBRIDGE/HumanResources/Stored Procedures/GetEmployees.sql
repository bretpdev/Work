CREATE PROCEDURE [hrbridge].[GetEmployees]
AS
	SELECT 
		E.EmployeeId,
		E.FirstName,
		E.LastName,
		E.WorkEmail,
		E.Department,
		E.[Location],
		E.JobTitle,
		E.Supervisor,
		E.Division,
		TRY_CONVERT(VARCHAR(500),ECF.HireDate),
		ECF.EmployeeNumber,
		ECF.NewHire,
		ECF.EmployeeLevel,
		ECF.DepartmentId,
		E.DeletedAt
	FROM
		hrbridge.Employees_BambooHR E
		INNER JOIN hrbridge.EmployeeCustomFields ECF
			ON ECF.EmployeeId = E.EmployeeId
	WHERE
		( --Update the account
			DeletedAt IS NULL
			AND (E.NeedsUpdated = 1 OR ECF.NeedsUpdated = 1)
		)
		OR
		( --Delete the account
			DeletedAt IS NOT NULL
			AND E.NeedsUpdated = 1
		)
