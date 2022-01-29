USE HumanResources
GO

ALTER PROCEDURE [hrbridge].[UpdateFTE]
	@FTE FTERecord_BambooHR READONLY
AS
BEGIN TRANSACTION
BEGIN TRY

--Everytime an employees records are updated, the API sends all records for that employee so we can remove existing records
DELETE FTE
FROM 
	hrbridge.FTE_BambooHR FTE
	INNER JOIN 
	(
		SELECT DISTINCT
			UPDATED_FTE.EmployeeId
		FROM
			@FTE UPDATED_FTE
	) UPDATED_FTE
		ON UPDATED_FTE.EmployeeId = FTE.EmployeeId
	
INSERT INTO hrbridge.FTE_BambooHR(EmployeeId,UpdatedAt,FTEEffectiveDate,FTE,Notes,FTERaw)
SELECT
	FTE.EmployeeId,
	FTE.UpdatedAt,
	FTE.FTEEffectiveDate,
	TRY_CONVERT(DECIMAL, FTE.FTE),
	FTE.Notes,
	FTE.FTE AS FTERaw
FROM
	@FTE FTE

COMMIT TRANSACTION
PRINT 'Transaction committed'
END TRY
BEGIN CATCH
	PRINT 'Transaction NOT committed'
	ROLLBACK TRANSACTION;
	THROW;
END CATCH;
RETURN 0
GO

ALTER PROCEDURE [hrbridge].[UpdateAllocation]
	@Allocation AllocationRecord_BambooHR READONLY
AS
BEGIN TRANSACTION
BEGIN TRY

--Everytime an employees records are updated, the API sends all records for that employee so we can remove existing records
DELETE ALLOC 
FROM 
	hrbridge.Allocations_BambooHR ALLOC
	INNER JOIN 
	(
		SELECT DISTINCT
			UPDATED_ALLOC.EmployeeId
		FROM
			@Allocation UPDATED_ALLOC
	) UPDATED_ALLOC
		ON UPDATED_ALLOC.EmployeeId = ALLOC.EmployeeId
	
INSERT INTO hrbridge.Allocations_BambooHR(EmployeeId,UpdatedAt,BusinessUnit,CostCenter,Account,FTE,AllocationEffectiveDate,SquareFootage,FTERaw)
SELECT
	ALLOC.EmployeeId,
	ALLOC.UpdatedAt,
	ALLOC.BusinessUnit,
	ALLOC.CostCenter,
	ALLOC.Account,
	TRY_CONVERT(DECIMAL,ALLOC.FTE),
	ALLOC.AllocationEffectiveDate,
	ALLOC.SquareFootage,
	ALLOC.FTE AS FTERaw
FROM
	@Allocation ALLOC

COMMIT TRANSACTION
PRINT 'Transaction committed'
END TRY
BEGIN CATCH
	PRINT 'Transaction NOT committed'
	ROLLBACK TRANSACTION;
	THROW;
END CATCH;

RETURN 0
GO

ALTER PROCEDURE [hrbridge].[UpdateCompensation]
	@Compensation CompensationRecord_BambooHR READONLY
AS

BEGIN TRANSACTION
BEGIN TRY
--Everytime an employees records are updated, the API sends all records for that employee so we can remove existing records
DELETE COMP 
FROM 
	hrbridge.Compensation_BambooHR COMP
	INNER JOIN 
	(
		SELECT DISTINCT
			UPDATED_COMP.EmployeeId
		FROM
			@Compensation UPDATED_COMP
	) UPDATED_COMP
		ON UPDATED_COMP.EmployeeId = COMP.EmployeeId
	
INSERT INTO hrbridge.Compensation_BambooHR(EmployeeId,UpdatedAt,StartDate,Rate,[Type],Exempt,Reason,Comment,PaidPer,PaySchedule,NeedsUpdated,RateRaw)
SELECT
	COMP.EmployeeId,
	COMP.UpdatedAt,
	COMP.StartDate,
	TRY_CONVERT(MONEY, COMP.Rate),
	COMP.[Type],
	COMP.Exempt,
	COMP.Reason,
	COMP.Comment,
	COMP.PaidPer,
	COMP.PaySchedule,
	CASE WHEN MAX(COMP.StartDate) OVER (PARTITION BY COMP.EmployeeId) = COMP.StartDate AND Employees.EmployeeId IS NOT NULL THEN 1 ELSE 0 END, --Only add it as needing updated if the employee exists in the employees table,
	COMP.Rate AS RateRaw
FROM
	@Compensation COMP
	LEFT JOIN hrbridge.Employees_BambooHR Employees
		ON Employees.EmployeeId = COMP.EmployeeId

COMMIT TRANSACTION
PRINT 'Transaction committed'
END TRY
BEGIN CATCH
	PRINT 'Transaction NOT committed'
	ROLLBACK TRANSACTION;
	THROW;
END CATCH;

RETURN 0
GO

ALTER PROCEDURE [hrbridge].[GetCompensation]
AS
	--Update the older records so that they don't get consolidated with Bridge
	SELECT
		EmployeeId,
		UpdatedAt,
		StartDate,
		TRY_CONVERT(VARCHAR(500), Rate),
		[Type],
		Exempt,
		Reason,
		Comment,
		PaidPer,
		PaySchedule
	FROM
		hrbridge.Compensation_BambooHR
	WHERE
		NeedsUpdated = 1
GO

ALTER PROCEDURE [hrbridge].[UpdateEmployeeCustomFields]
	@Employee EmployeeCustomField READONLY
AS
	
BEGIN TRANSACTION
BEGIN TRY
MERGE 
	hrbridge.EmployeeCustomFields AS ECF
USING
(
	SELECT
		EmployeeId,
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
		DepartmentId
	FROM 
		@Employee
) AS UpdatedECF(EmployeeId,HireDate,Ethnicity,EmployeeNumber,EEOJobCategory,Clearance,Birthdate,EmployeeLevel,Gender,SCACode,VacationCategory,DepartmentId)
ON
	ECF.EmployeeId = UpdatedECF.EmployeeId
WHEN MATCHED THEN
	UPDATE SET
		ECF.EmployeeId = UpdatedECF.EmployeeId,
		ECF.HireDate = TRY_CONVERT(DATE, UpdatedECF.HireDate),
		ECF.Ethnicity = UpdatedECF.Ethnicity,
		ECF.EmployeeNumber = UpdatedECF.EmployeeNumber,
		ECF.EEOJobCategory = UpdatedECF.EEOJobCategory,
		ECF.Clearance = UpdatedECF.Clearance,
		ECF.Birthdate = TRY_CONVERT(DATE, UpdatedECF.Birthdate),
		ECF.EmployeeLevel = UpdatedECF.EmployeeLevel,
		ECF.Gender = UpdatedECF.Gender,
		ECF.SCACode = UpdatedECF.SCACode,
		ECF.VacationCategory = UpdatedECF.VacationCategory,
		ECF.DepartmentId = UpdatedECF.DepartmentId,
		ECF.NewHire = CASE WHEN UpdatedECF.HireDate > DATEADD(DAY, -31, GETDATE()) THEN 'Yes' ELSE 'No' END,
		ECF.UpdatedAt = GETDATE(),
		ECF.NeedsUpdated = 
			CASE 
				WHEN 
					ECF.EmployeeId = UpdatedECF.EmployeeId
					AND ECF.HireDate = UpdatedECF.HireDate
					AND ECF.Ethnicity = UpdatedECF.Ethnicity
					AND ECF.EmployeeNumber = UpdatedECF.EmployeeNumber
					AND ECF.EEOJobCategory = UpdatedECF.EEOJobCategory
					AND ECF.Clearance = UpdatedECF.Clearance
					AND ECF.Birthdate = UpdatedECF.Birthdate
					AND ECF.EmployeeLevel = UpdatedECF.EmployeeLevel
					AND ECF.Gender = UpdatedECF.Gender
					AND ECF.SCACode = UpdatedECF.SCACode
					AND ECF.VacationCategory = UpdatedECF.VacationCategory
					AND ECF.DepartmentId = UpdatedECF.DepartmentId
					AND ECF.NewHire = CASE WHEN UpdatedECF.HireDate > DATEADD(DAY, -31, GETDATE()) THEN 'Yes' ELSE 'No' END
				THEN 0
				ELSE 1
			END,
		ECF.HireDateRaw = UpdatedECF.HireDate,
		ECF.BirthdateRaw = UpdatedECF.Birthdate
WHEN NOT MATCHED THEN
	INSERT(EmployeeId,HireDate,Ethnicity,EmployeeNumber,EEOJobCategory,Clearance,Birthdate,EmployeeLevel,Gender,SCACode,VacationCategory,DepartmentId,NewHire,UpdatedAt,NeedsUpdated,HireDateRaw,BirthdateRaw)
	VALUES
	(
		UpdatedECF.EmployeeId,
		TRY_CONVERT(DATE, UpdatedECF.HireDate),
		UpdatedECF.Ethnicity,
		UpdatedECF.EmployeeNumber,
		UpdatedECF.EEOJobCategory,
		UpdatedECF.Clearance,
		TRY_CONVERT(DATE, UpdatedECF.Birthdate),
		UpdatedECF.EmployeeLevel,
		UpdatedECF.Gender,
		UpdatedECF.SCACode,
		UpdatedECF.VacationCategory,
		UpdatedECF.DepartmentId,
		CASE WHEN UpdatedECF.HireDate > DATEADD(DAY, -31, GETDATE()) THEN 'Yes' ELSE 'No' END,
		GETDATE(),
		1,
		UpdatedECF.HireDate,
		UpdatedECF.Birthdate
	);
COMMIT TRANSACTION
PRINT 'Transaction committed'
END TRY
BEGIN CATCH
	PRINT 'Transaction NOT committed'
	ROLLBACK TRANSACTION;
	THROW;
END CATCH;
GO

ALTER PROCEDURE [hrbridge].[GetEmployees]
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
GO

ALTER PROCEDURE [hrbridge].[UpdateEmployeeCustomReport]
	@Employees hrbridge.CustomReport READONLY
AS

DECLARE @DefaultDate DATE = CAST('1900-01-01' AS DATE)
BEGIN TRANSACTION
BEGIN TRY
MERGE 
	hrbridge.EmployeeCustomReport AS Employees
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
		ReportsTo,
		Division,
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
) AS UpdatedEmployees(EmployeeId,FirstName,LastName,WorkEmail,Department,[Location],JobTitle,ReportsTo,Division,HireDate,Ethnicity,
	EmployeeNumber,EEOJobCategory,Clearance,Birthdate,EmployeeLevel,Gender,SCACode,VacationCategory,DepartmentId,EffectiveDate)
ON
	Employees.EmployeeId = UpdatedEmployees.EmployeeId
	AND ISNULL(Employees.FirstName, '') = ISNULL(UpdatedEmployees.FirstName, '')
	AND ISNULL(Employees.LastName, '') = ISNULL(UpdatedEmployees.LastName, '')
	AND ISNULL(Employees.WorkEmail, '') = ISNULL(UpdatedEmployees.WorkEmail, '')
	AND ISNULL(Employees.Department, '') = ISNULL(UpdatedEmployees.Department, '')
	AND ISNULL(Employees.[Location], '') = ISNULL(UpdatedEmployees.[Location], '')
	AND ISNULL(Employees.JobTitle, '') = ISNULL(UpdatedEmployees.JobTitle, '')
	AND ISNULL(Employees.ReportsTo, '') = ISNULL(UpdatedEmployees.ReportsTo, '')
	AND ISNULL(Employees.Division, '') = ISNULL(UpdatedEmployees.Division, '')
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
		Employees.Department = UpdatedEmployees.Department,
		Employees.[Location] = UpdatedEmployees.[Location],
		Employees.JobTitle = UpdatedEmployees.JobTitle,
		Employees.ReportsTo = UpdatedEmployees.ReportsTo,
		Employees.Division = UpdatedEmployees.Division,
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
	INSERT(EmployeeId,FirstName,LastName,WorkEmail,Department,[Location],JobTitle,ReportsTo,Division,HireDate,Ethnicity,
	EmployeeNumber,EEOJobCategory,Clearance,Birthdate,EmployeeLevel,Gender,SCACode,VacationCategory,DepartmentId,EffectiveDate,UpdatedAt,BirthdateRaw)
	VALUES
	(
		UpdatedEmployees.EmployeeId,
		UpdatedEmployees.FirstName,
		UpdatedEmployees.LastName,
		UpdatedEmployees.WorkEmail,
		UpdatedEmployees.Department,
		UpdatedEmployees.[Location],
		UpdatedEmployees.JobTitle,
		UpdatedEmployees.ReportsTo,
		UpdatedEmployees.Division,
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
