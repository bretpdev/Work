CREATE PROCEDURE [hrbridge].[UpdateEmploymentStatus]
	@EmploymentStatus EmploymentStatus_BambooHR READONLY
AS
BEGIN TRANSACTION
BEGIN TRY

--Everytime an employees records are updated, the API sends all records for that employee so we can remove existing records
DELETE ES
FROM 
	hrbridge.EmploymentStatus ES
	INNER JOIN 
	(
		SELECT DISTINCT
			UPDATED_ES.EmployeeId
		FROM
			@EmploymentStatus UPDATED_ES
	) UPDATED_ES
		ON UPDATED_ES.EmployeeId = ES.EmployeeId
	
INSERT INTO hrbridge.EmploymentStatus(EmployeeId,UpdatedAt,[Date],EmploymentStatus,EmploymentStatusComment,TerminationReason,TerminationType,ElligableForRehire)
SELECT
	ES.EmployeeId,
	ES.UpdatedAt,
	ES.[Date],
	ES.EmploymentStatus,
	ES.EmploymentStatusComment,
	ES.TerminationReason,
	ES.TerminationType,
	ES.ElligableForRehire
FROM
	@EmploymentStatus ES

COMMIT TRANSACTION
PRINT 'Transaction committed'
END TRY
BEGIN CATCH
	PRINT 'Transaction NOT committed'
	ROLLBACK TRANSACTION;
	THROW;
END CATCH;
RETURN 0