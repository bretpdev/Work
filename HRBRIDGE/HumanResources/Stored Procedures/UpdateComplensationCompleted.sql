CREATE PROCEDURE [hrbridge].[UpdateCompensationCompleted]
	@EmployeeId INT
AS
	UPDATE
		hrbridge.Compensation_BambooHR
	SET
		NeedsUpdated = 0
	WHERE
		EmployeeId = @EmployeeId
RETURN 0
