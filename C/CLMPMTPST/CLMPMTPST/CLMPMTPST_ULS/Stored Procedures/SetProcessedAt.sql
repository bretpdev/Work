CREATE PROCEDURE [clmpmtpst].[SetProcessedAt]
	@ClaimPaymentId INT,
	@ProcessedManually BIT,
	@BatchNumber VARCHAR(30) = NULL
AS
	
	UPDATE
		clmpmtpst.ClaimPayments
	SET
		ProcessedAt = GETDATE(),
		ProcessedManually = @ProcessedManually,
		BatchNumber = @BatchNumber
	WHERE
		ClaimPaymentId = @ClaimPaymentId

RETURN 0
