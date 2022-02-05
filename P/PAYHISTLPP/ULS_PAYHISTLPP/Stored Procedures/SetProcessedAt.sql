CREATE PROCEDURE [payhistlpp].[SetProcessedAt]
	@AccountsId INT
AS
	UPDATE
		ULS.payhistlpp.Accounts
	SET
		ProcessedAt = GETDATE()
	WHERE
		AccountsId = @AccountsId