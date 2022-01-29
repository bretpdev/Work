CREATE PROCEDURE [batchesp].[SetTaskAsRequiresReview]
	@ESPEnrollmentId INT
AS
	UPDATE
		batchesp.ESPEnrollments
	SET 
		RequiresReview = 1,
		UpdatedAt = GETDATE()
	WHERE
		ESPEnrollmentId = @ESPEnrollmentId
RETURN 0
