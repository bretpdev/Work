CREATE PROCEDURE [projectrequest].[GetNotificationRecipients]
AS

SELECT 
	[RecipientEmail]
FROM
	[projectrequest].[NotificationRecipients]
WHERE
	Active = 1
