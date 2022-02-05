CREATE PROCEDURE [hrbridge].[UpdateCompensation]
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
	TRY_CONVERT(MONEY, REPLACE(COMP.Rate,'USD','')),
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