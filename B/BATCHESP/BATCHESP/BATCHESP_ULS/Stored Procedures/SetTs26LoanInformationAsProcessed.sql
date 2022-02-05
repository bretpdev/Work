CREATE PROCEDURE [batchesp].[SetTs26LoanInformationAsProcessed]
	@Ts26LoanInformationId INT
AS
	UPDATE
		batchesp.Ts26LoanInformation
	SET
		ProcessedAt = GETDATE(),
		ProcessedBy = SYSTEM_USER
	WHERE
		Ts26LoanInformationId = @Ts26LoanInformationId
	
RETURN 0
