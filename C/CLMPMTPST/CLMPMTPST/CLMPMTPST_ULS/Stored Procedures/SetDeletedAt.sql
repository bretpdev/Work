CREATE PROCEDURE [clmpmtpst].[SetDeletedAt]
	@ClaimPaymentId INT
AS
	
	UPDATE
		clmpmtpst.ClaimPayments
	SET
		DeletedAt = GETDATE()
	WHERE
		ClaimPaymentId = @ClaimPaymentId

RETURN 0
