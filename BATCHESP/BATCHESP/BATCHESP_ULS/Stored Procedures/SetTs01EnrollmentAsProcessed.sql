CREATE PROCEDURE [batchesp].[SetTs01EnrollmentAsProcessed]
	@TS01EnrollmentId INT
AS
	UPDATE
		batchesp.TS01Enrollments
	SET
		ProcessedAt = GETDATE(),
		ProcessedBy = SYSTEM_USER
	WHERE
		TS01EnrollmentId = @TS01EnrollmentId
	
RETURN 0
