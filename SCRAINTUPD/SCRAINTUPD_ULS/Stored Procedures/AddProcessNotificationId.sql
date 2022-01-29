CREATE PROCEDURE [scra].[AddProcessNotificationId]
	@ScriptProcessingId INT,
	@ProcessLogId INT,
	@ProcessNotificationId INT
AS

INSERT INTO ScriptProcessLoggerMapping(ScriptProcessingId, ProcessLogId, ProcessNotificationId)
VALUES(@ScriptProcessingId, @ProcessLogId, @ProcessNotificationId)	
	
RETURN 0