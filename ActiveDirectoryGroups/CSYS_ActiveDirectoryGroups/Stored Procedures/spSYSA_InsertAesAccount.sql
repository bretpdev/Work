CREATE PROCEDURE [dbo].[spSYSA_InsertAesAccount]
	@SqlUserId int,
	@AesAccount char(10)
AS
	INSERT INTO SYSA_LST_AesAccounts(SqlUserId, AesAccount, AddedAt, AddedBy)
	VALUES(@SqlUserId, @AesAccount, GETDATE(), SUSER_SNAME())
RETURN 0