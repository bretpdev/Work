CREATE PROCEDURE [tilpcrost].[SetProcessedAt]
	@AccountsId INT
AS
	UPDATE
		tilpcrost.Accounts
	SET
		ProcessedAt = GETDATE()
	WHERE
		AccountsId = @AccountsId
RETURN 0