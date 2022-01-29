CREATE PROCEDURE [dbo].[ArcAdd_AddProcessNotificationId]
	@ArcAddProcessingId BIGINT,
	@ProcessLogId INT,
	@ProcessNotificationId INT
AS

INSERT INTO ArcAddProcessLoggerMapping(ArcAddProcessingId, ProcessLogId, ProcessNotificationId)
VALUES(@ArcAddProcessingId, @ProcessLogId, @ProcessNotificationId)	
	
RETURN 0
