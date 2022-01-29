CREATE PROCEDURE [batchesp].[SetEspEnrollmentAsProcessed]
	@ESPEnrollmentId INT
AS
	UPDATE
		batchesp.ESPEnrollments
	SET 
		ProcessedAt = GETDATE(),
		ProcessedBy = SYSTEM_USER
	WHERE
		ESPEnrollmentId = @ESPEnrollmentId

RETURN 0
