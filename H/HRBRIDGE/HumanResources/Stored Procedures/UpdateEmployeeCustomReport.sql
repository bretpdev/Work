CREATE PROCEDURE [hrbridge].[UpdateEmployeeCustomReport]
	@Employees hrbridge.CustomReport READONLY
AS

DECLARE @DefaultDate DATE = CAST('1900-01-01' AS DATE)
BEGIN TRANSACTION
BEGIN TRY
MERGE 
	hrbridge.EmployeeCustomReport AS Employees
USING
(
	SELECT DISTINCT
		EmployeeId,
		FirstName,
		LastName,
		WorkEmail,
		HireDate,
		Ethnicity,
		EmployeeNumber,
		EEOJobCategory,
		Clearance,
		Birthdate,
		EmployeeLevel,
		Gender,
		SCACode,
		VacationCategory,
		DepartmentId,
		EffectiveDate
	FROM 
		@Employees E
) AS UpdatedEmployees(EmployeeId,FirstName,LastName,WorkEmail,HireDate,Ethnicity,
	EmployeeNumber,EEOJobCategory,Clearance,Birthdate,EmployeeLevel,Gender,SCACode,VacationCategory,DepartmentId,EffectiveDate)
ON
	Employees.EmployeeId = UpdatedEmployees.EmployeeId
	AND ISNULL(Employees.FirstName, '') = ISNULL(UpdatedEmployees.FirstName, '')
	AND ISNULL(Employees.LastName, '') = ISNULL(UpdatedEmployees.LastName, '')
	AND ISNULL(Employees.WorkEmail, '') = ISNULL(UpdatedEmployees.WorkEmail, '')
	AND Employees.HireDate = UpdatedEmployees.HireDate
	AND ISNULL(Employees.Ethnicity, '') = ISNULL(UpdatedEmployees.Ethnicity, '')
	AND ISNULL(Employees.EmployeeNumber, '') = ISNULL(UpdatedEmployees.EmployeeNumber, '')
	AND ISNULL(Employees.EEOJobCategory, '') = ISNULL(UpdatedEmployees.EEOJobCategory, '')
	AND ISNULL(Employees.Clearance, '') = ISNULL(UpdatedEmployees.Clearance, '')
	AND 
	(
		Employees.Birthdate = TRY_CONVERT(DATE, UpdatedEmployees.Birthdate)
		OR (Employees.Birthdate IS NULL AND TRY_CONVERT(DATE, UpdatedEmployees.Birthdate) IS NULL)
	)
	AND ISNULL(Employees.EmployeeLevel, '') = ISNULL(UpdatedEmployees.EmployeeLevel, '')
	AND ISNULL(Employees.Gender, '') = ISNULL(UpdatedEmployees.Gender, '')
	AND ISNULL(Employees.SCACode, '') = ISNULL(UpdatedEmployees.SCACode, '')
	AND ISNULL(Employees.VacationCategory, '') = ISNULL(UpdatedEmployees.VacationCategory, '')
	AND ISNULL(Employees.DepartmentId, '') = ISNULL(UpdatedEmployees.DepartmentId, '')
	AND ISNULL(Employees.EffectiveDate, @DefaultDate) = ISNULL(UpdatedEmployees.EffectiveDate, @DefaultDate)
WHEN MATCHED THEN
	UPDATE SET
		Employees.FirstName = UpdatedEmployees.FirstName,
		Employees.LastName = UpdatedEmployees.LastName,
		Employees.WorkEmail = UpdatedEmployees.WorkEmail,
		Employees.HireDate = UpdatedEmployees.HireDate,
		Employees.Ethnicity = UpdatedEmployees.Ethnicity,
		Employees.EmployeeNumber = UpdatedEmployees.EmployeeNumber,
		Employees.EEOJobCategory = UpdatedEmployees.EEOJobCategory,
		Employees.Clearance = UpdatedEmployees.Clearance,
		Employees.Birthdate = TRY_CONVERT(DATE,UpdatedEmployees.Birthdate),
		Employees.EmployeeLevel = UpdatedEmployees.EmployeeLevel,
		Employees.Gender = UpdatedEmployees.Gender,
		Employees.SCACode = UpdatedEmployees.SCACode,
		Employees.VacationCategory = UpdatedEmployees.VacationCategory,
		Employees.DepartmentId = UpdatedEmployees.DepartmentId,
		Employees.EffectiveDate = UpdatedEmployees.EffectiveDate,
		Employees.UpdatedAt = GETDATE(),
		Employees.BirthdateRaw = UpdatedEmployees.Birthdate
WHEN NOT MATCHED BY TARGET THEN
	INSERT(EmployeeId,FirstName,LastName,WorkEmail,HireDate,Ethnicity,
	EmployeeNumber,EEOJobCategory,Clearance,Birthdate,EmployeeLevel,Gender,SCACode,VacationCategory,DepartmentId,EffectiveDate,UpdatedAt,BirthdateRaw)
	VALUES
	(
		UpdatedEmployees.EmployeeId,
		UpdatedEmployees.FirstName,
		UpdatedEmployees.LastName,
		UpdatedEmployees.WorkEmail,
		UpdatedEmployees.HireDate,
		UpdatedEmployees.Ethnicity,
		UpdatedEmployees.EmployeeNumber,
		UpdatedEmployees.EEOJobCategory,
		UpdatedEmployees.Clearance,
		TRY_CONVERT(DATE,UpdatedEmployees.Birthdate),
		UpdatedEmployees.EmployeeLevel,
		UpdatedEmployees.Gender,
		UpdatedEmployees.SCACode,
		UpdatedEmployees.VacationCategory,
		UpdatedEmployees.DepartmentId,
		UpdatedEmployees.EffectiveDate,
		GETDATE(),
		UpdatedEmployees.Birthdate
	)
WHEN NOT MATCHED BY SOURCE THEN DELETE;

COMMIT TRANSACTION
PRINT 'Transaction committed'
END TRY
BEGIN CATCH
	PRINT 'Transaction NOT committed'
	ROLLBACK TRANSACTION;
	THROW;
END CATCH;
