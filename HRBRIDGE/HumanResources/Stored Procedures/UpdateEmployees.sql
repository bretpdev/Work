CREATE PROCEDURE [hrbridge].[UpdateEmployees]
	@Employees Employee_BambooHR READONLY
AS
BEGIN TRANSACTION
BEGIN TRY
MERGE 
	hrbridge.Employees_BambooHR AS Employees
USING
(
	SELECT
		EmployeeId,
		FirstName,
		LastName,
		WorkEmail,
		Department,
		[Location],
		JobTitle,
		Supervisor,
		Division
	FROM 
		@Employees 
) AS UpdatedEmployees(EmployeeId,FirstName,LastName,WorkEmail,Department,[Location],JobTitle,Supervisor,Division)
ON
	Employees.EmployeeId = UpdatedEmployees.EmployeeId
WHEN MATCHED THEN
	UPDATE SET
		Employees.FirstName = UpdatedEmployees.FirstName,
		Employees.LastName = UpdatedEmployees.LastName,
		Employees.WorkEmail = UpdatedEmployees.WorkEmail,
		Employees.Department = UpdatedEmployees.Department,
		Employees.[Location] = UpdatedEmployees.[Location],
		Employees.JobTitle = UpdatedEmployees.JobTitle,
		Employees.Supervisor = UpdatedEmployees.Supervisor,
		Employees.Division = UpdatedEmployees.Division,
		Employees.UpdatedAt = GETDATE(),
		Employees.NeedsUpdated = 
			CASE 
				WHEN 
					Employees.FirstName = UpdatedEmployees.FirstName
					AND Employees.LastName = UpdatedEmployees.LastName
					AND Employees.WorkEmail = UpdatedEmployees.WorkEmail
					AND Employees.Department = UpdatedEmployees.Department
					AND Employees.JobTitle = UpdatedEmployees.JobTitle
					AND Employees.Supervisor = UpdatedEmployees.Supervisor
					AND Employees.Division = UpdatedEmployees.Division
					AND Employees.NeedsUpdated = 0
					AND Employees.DeletedAt IS NULL
				THEN 0
				ELSE 1
			END,
		Employees.DeletedAt = NULL
WHEN NOT MATCHED THEN
	INSERT(EmployeeId,FirstName,LastName,WorkEmail,Department,[Location],JobTitle,Supervisor,Division,UpdatedAt,NeedsUpdated,DeletedAt)
	VALUES
	(
		UpdatedEmployees.EmployeeId,
		UpdatedEmployees.FirstName,
		UpdatedEmployees.LastName,
		UpdatedEmployees.WorkEmail,
		UpdatedEmployees.Department,
		UpdatedEmployees.[Location],
		UpdatedEmployees.JobTitle,
		UpdatedEmployees.Supervisor,
		UpdatedEmployees.Division,
		GETDATE(),
		1,
		NULL
	)
--If a user in our table no longer appears in the source then they are expected to have been deleted/terminated.
--The script sets a flag determining if the user needs to have their Bridge account deleted
WHEN NOT MATCHED BY SOURCE THEN
	UPDATE SET
		DeletedAt = CASE WHEN DeletedAt IS NULL THEN GETDATE() ELSE DeletedAt END, 
		NeedsUpdated = CASE WHEN DeletedAt IS NULL THEN 1 ELSE NeedsUpdated END;
COMMIT TRANSACTION
PRINT 'Transaction committed'
END TRY
BEGIN CATCH
	PRINT 'Transaction NOT committed'
	ROLLBACK TRANSACTION;
	THROW;
END CATCH;
