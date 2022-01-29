USE HumanResources
GO

--Allocations Table Changes
ALTER TABLE hrbridge.Allocations_BambooHR ADD FTERaw VARCHAR(500) NULL
GO

BEGIN TRANSACTION
BEGIN TRY
	UPDATE A
	SET
		A.FTERaw = A.FTE
	FROM
		hrbridge.Allocations_BambooHR A

	UPDATE A	
	SET
		A.FTE = TRY_CONVERT(DECIMAL, A.FTE)
	FROM 
		hrbridge.Allocations_BambooHR A
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
END CATCH

ALTER TABLE hrbridge.Allocations_BambooHR ALTER COLUMN FTE DECIMAL NULL
GO

--Compensation Table
ALTER TABLE hrbridge.Compensation_BambooHR ADD RateRaw VARCHAR(500) NULL
GO

BEGIN TRANSACTION
BEGIN TRY
	UPDATE A
	SET
		A.RateRaw = A.Rate
	FROM
		hrbridge.Compensation_BambooHR A

	UPDATE A	
	SET
		A.Rate = TRY_CONVERT(MONEY, A.Rate)
	FROM 
		hrbridge.Compensation_BambooHR A
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
END CATCH

ALTER TABLE hrbridge.Compensation_BambooHR ALTER COLUMN Rate MONEY
GO

--EmployeeCustomFields Table
ALTER TABLE hrbridge.EmployeeCustomFields ADD HireDateRaw VARCHAR(500) NULL
GO

ALTER TABLE hrbridge.EmployeeCustomFields ADD BirthdateRaw VARCHAR(500) NULL
GO

BEGIN TRANSACTION
BEGIN TRY
	UPDATE A
	SET
		A.HireDateRaw = A.HireDate,
		A.BirthdateRaw = A.Birthdate 
	FROM
		hrbridge.EmployeeCustomFields A

	UPDATE A	
	SET
		A.HireDate = TRY_CONVERT(DATE, A.HireDate),
		A.Birthdate = TRY_CONVERT(DATE, A.Birthdate)
	FROM 
		hrbridge.EmployeeCustomFields A
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
END CATCH

ALTER TABLE hrbridge.EmployeeCustomFields ALTER COLUMN HireDate DATE
GO

ALTER TABLE hrbridge.EmployeeCustomFields ALTER COLUMN Birthdate DATE
GO

--EmployeeCustomReport Table
ALTER TABLE hrbridge.EmployeeCustomReport ADD BirthdateRaw VARCHAR(500) NULL
GO

BEGIN TRANSACTION
BEGIN TRY
	UPDATE A
	SET
		A.BirthdateRaw = A.Birthdate 
	FROM
		hrbridge.EmployeeCustomReport A

	UPDATE A	
	SET
		A.Birthdate = TRY_CONVERT(DATE, A.Birthdate)
	FROM 
		hrbridge.EmployeeCustomReport A
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
END CATCH

ALTER TABLE hrbridge.EmployeeCustomReport ALTER COLUMN Birthdate DATE
GO

--FTE Table Changes
ALTER TABLE hrbridge.FTE_BambooHR ADD FTERaw VARCHAR(500) NULL
GO

BEGIN TRANSACTION
BEGIN TRY
	UPDATE A
	SET
		A.FTERaw = A.FTE
	FROM
		hrbridge.FTE_BambooHR A

	UPDATE A	
	SET
		A.FTE = TRY_CONVERT(DECIMAL, A.FTE)
	FROM 
		hrbridge.FTE_BambooHR A
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
END CATCH

ALTER TABLE hrbridge.FTE_BambooHR ALTER COLUMN FTE DECIMAL
GO


