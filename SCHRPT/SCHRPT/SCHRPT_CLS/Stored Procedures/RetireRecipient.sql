CREATE PROCEDURE [schrpt].[RetireRecipient]
	@RecipientId INT,
	@WindowsUserName VARCHAR(50)
AS

	UPDATE
		schrpt.Recipients
	SET
		DeletedAt = GETDATE(), DeletedBy = @WindowsUserName
	WHERE
		RecipientId = @RecipientId


RETURN 0
