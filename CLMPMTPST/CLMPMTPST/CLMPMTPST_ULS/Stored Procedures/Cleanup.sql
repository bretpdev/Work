CREATE PROCEDURE [clmpmtpst].[Cleanup]
	
AS
	
	UPDATE
		clmpmtpst.ClaimPayments 
	SET
		DeletedAt = GETDATE(),
		DeletedBy = SUSER_SNAME()
	WHERE	
		ProcessedAt IS NULL
		AND DeletedAt IS NULL

RETURN 0
