CREATE PROCEDURE [rcemailpul].[AddUnsubscribedEmail]
	@EmailAddress VARCHAR(320),
	@Source VARCHAR(20)
AS
	
	INSERT INTO rcemailpul.UnsubscribedEmails (EmailAddress, [Source])
	VALUES (@EmailAddress, @Source)

RETURN 0
