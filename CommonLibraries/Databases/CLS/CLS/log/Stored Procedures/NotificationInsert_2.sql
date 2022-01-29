CREATE PROCEDURE [log].NotificationInsert
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
GO
GRANT EXECUTE
    ON OBJECT::[log].[NotificationInsert] TO [db_executor]
    AS [UHEAA\Developers];

