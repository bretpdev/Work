CREATE PROCEDURE [onelinkaap].[AddProcessNotificationId]
	@ArcAddProcessingId BIGINT,
	@ProcessLogId BIGINT,
	@ProcessNotificationId BIGINT
AS

INSERT INTO ArcAddProcessLoggerMapping(ArcAddProcessingId, ProcessLogId, ProcessNotificationId)
VALUES(@ArcAddProcessingId, @ProcessLogId, @ProcessNotificationId)	
	
RETURN 0
