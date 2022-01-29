CREATE PROCEDURE [hrbridge].[UpdateJobInfo]
	@JobInfo JobInfo_BambooHR READONLY
AS

BEGIN TRANSACTION
BEGIN TRY
--Everytime an employees records are updated, the API sends all records for that employee so we can remove existing records
DELETE JI
FROM 
	hrbridge.JobInfo JI
	INNER JOIN 
	(
		SELECT DISTINCT
			UPDATED_JI.EmployeeId
		FROM
			@JobInfo UPDATED_JI
	) UPDATED_JI
		ON UPDATED_JI.EmployeeId = JI.EmployeeId
	
INSERT INTO hrbridge.JobInfo(EmployeeId,UpdatedAt,[Date],[Location],Department,Division,JobTitle,ReportsTo)
SELECT
	UPDATED_JI.EmployeeId,
	UPDATED_JI.UpdatedAt,
	UPDATED_JI.[Date],
	UPDATED_JI.[Location],
	UPDATED_JI.Department,
	UPDATED_JI.Division,
	UPDATED_JI.JobTitle,
	UPDATED_JI.ReportsTo
FROM
	@JobInfo UPDATED_JI

COMMIT TRANSACTION
PRINT 'Transaction committed'
END TRY
BEGIN CATCH
	PRINT 'Transaction NOT committed'
	ROLLBACK TRANSACTION;
	THROW;
END CATCH;

RETURN 0
