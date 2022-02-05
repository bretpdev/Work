CREATE PROCEDURE [batchesp].[SetParentPlusLoanDetailAsProcessed]
	@ParentPlusLoanDetailInformationId INT
AS

	UPDATE
		batchesp.ParentPlusLoanDetails
	SET
		ProcessedAt = GETDATE(),
		ProcessedBy = SYSTEM_USER
	WHERE
		ParentPlusLoanDetailsId = @ParentPlusLoanDetailInformationId
	
RETURN 0
