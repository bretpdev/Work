CREATE PROCEDURE [rcemailpul].[GetUnsubscribedEmails]
AS

	SELECT 
		EmailAddress, [Source]
	FROM
		rcemailpul.UnsubscribedEmails

RETURN 0
