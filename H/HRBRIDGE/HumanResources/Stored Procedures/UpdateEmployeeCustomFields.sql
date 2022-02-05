CREATE PROCEDURE [hrbridge].[UpdateEmployeeCustomFields]
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
