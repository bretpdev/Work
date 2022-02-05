CREATE PROCEDURE [payhistlpp].[DeleteUnprocessedAccounts]
	@RunId INT
AS
	UPDATE
		ULS.payhistlpp.Accounts
	SET
		DeletedAt = GETDATE(),
		DeletedBy = USER_NAME()
	WHERE
		RunId = @RunId