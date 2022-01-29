CREATE PROCEDURE [rcemailpul].[RemoveUnsubscribedEmail]
	@EmailAddress VARCHAR(320),
	@Source VARCHAR(20)
AS
	
	DELETE FROM
		rcemailpul.UnsubscribedEmails 
	WHERE 
		EmailAddress = @EmailAddress
		AND
		[Source] = @Source

RETURN 0
