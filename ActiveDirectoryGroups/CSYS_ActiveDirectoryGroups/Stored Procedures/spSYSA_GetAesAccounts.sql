CREATE PROCEDURE [dbo].[spSYSA_GetAesAccounts]
	@SqlUserId int
AS
	SELECT
		AesAccountId,
		AesAccount
	FROM
		SYSA_LST_AesAccounts
	WHERE
		SqlUserId = @SqlUserId
		AND DeletedAt IS NULL

RETURN 0