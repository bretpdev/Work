CREATE TABLE [dbo].[ArcAddProcessLoggerMapping]
(
	[ArcAddProcessLoggerMappingId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ArcAddProcessingId] BIGINT NOT NULL, 
    [ProcessLogId] BIGINT NOT NULL, 
    [ProcessNotificationId] BIGINT NOT NULL,
	CONSTRAINT [FK_ArcAddProcessLoggerMapping_ToArcAddProcessing] FOREIGN KEY (ArcAddProcessingId) REFERENCES ArcAddProcessing(ArcAddProcessingId)
)
