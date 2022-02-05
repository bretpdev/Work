CREATE PROCEDURE [compfafsa].[GetNotificationRecipients]
AS
	SELECT
		Recipient
	FROM
		compfafsa.NotificationRecipients
	WHERE
		DeletedAt IS NULL
		AND DeletedBy IS NULL
RETURN 0
