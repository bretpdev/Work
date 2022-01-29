CREATE PROCEDURE [dbo].[spSYSA_DeleteAesAccount]
	@AesAccountId int
AS
	UPDATE
		SYSA_LST_AesAccounts
	SET
		DeletedAt = GETDATE(),
		DeletedBy = SUSER_SNAME()
	WHERE
		AesAccountId = @AesAccountId
RETURN 0