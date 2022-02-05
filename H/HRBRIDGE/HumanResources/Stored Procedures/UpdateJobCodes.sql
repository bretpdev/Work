CREATE PROCEDURE [hrbridge].[UpdateJobCodes]
	@JobCodes JobCode_BambooHR READONLY
AS
BEGIN TRANSACTION
BEGIN TRY

--Everytime an employees records are updated, the API sends all records for that employee so we can remove existing records
DELETE JOB
FROM 
	hrbridge.JobCodes_BambooHR JOB
	INNER JOIN 
	(
		SELECT DISTINCT
			UPDATED_JOB.EmployeeId
		FROM
			@JobCodes UPDATED_JOB
	) UPDATED_JOB
		ON UPDATED_JOB.EmployeeId = JOB.EmployeeId
	
INSERT INTO hrbridge.JobCodes_BambooHR(EmployeeId,UpdatedAt,JobCodeEffectiveDate,JobCode,UOfUJobTitle)
SELECT
	JOB.EmployeeId,
	JOB.UpdatedAt,
	JOB.JobCodeEffectiveDate,
	JOB.JobCode,
	JOB.UOfUJobTitle
FROM
	@JobCodes JOB

COMMIT TRANSACTION
PRINT 'Transaction committed'
END TRY
BEGIN CATCH
	PRINT 'Transaction NOT committed'
	ROLLBACK TRANSACTION;
	THROW;
END CATCH;
RETURN 0
