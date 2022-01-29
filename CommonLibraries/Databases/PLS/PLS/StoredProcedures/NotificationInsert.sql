CREATE PROCEDURE [log].[NotificationInsert]
	@ProcessNotificationId int,
	@LogMessage varchar(max)
AS
	INSERT INTO 
		[log].ProcessLogMessages
	(
		ProcessNotificationId,
		LogMessage
	)
	VALUES
	(
		@ProcessNotificationId, 
		@LogMessage
	)
RETURN 0
