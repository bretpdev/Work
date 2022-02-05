CREATE PROCEDURE [schrpt].[RetireSchoolRecipient]
	@SchoolRecipientId INT,
	@WindowsUserName VARCHAR(50)
AS
	
	UPDATE
		schrpt.SchoolRecipients
	SET
		DeletedAt = GETDATE(), DeletedBy = @WindowsUserName
	WHERE
		SchoolRecipientId = @SchoolRecipientId

RETURN 0
