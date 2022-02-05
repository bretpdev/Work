USE [ULS]
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[log].NotificationInsert') AND type in ('P', 'PC'))
DROP PROCEDURE [log].NotificationInsert
GO
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
GRANT EXECUTE ON [log].NotificationInsert TO db_executor
GO