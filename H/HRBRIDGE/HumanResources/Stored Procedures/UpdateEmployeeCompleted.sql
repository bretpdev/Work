CREATE PROCEDURE [hrbridge].[UpdateEmployeeCompleted]
	@EmployeeId INT
AS

BEGIN TRANSACTION
BEGIN TRY

	UPDATE
		hrbridge.Employees_BambooHR
	SET
		NeedsUpdated = 0
	WHERE
		EmployeeId = @EmployeeId

	UPDATE
		hrbridge.EmployeeCustomFields
	SET
		NeedsUpdated = 0
	WHERE
		EmployeeId = @EmployeeId

COMMIT TRANSACTION
PRINT 'Transaction committed'
END TRY
BEGIN CATCH
	PRINT 'Transaction NOT committed'
	ROLLBACK TRANSACTION;
	THROW;
END CATCH;

RETURN 0
