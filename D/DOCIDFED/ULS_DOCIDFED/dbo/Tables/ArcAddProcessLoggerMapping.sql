CREATE TABLE [dbo].[ArcAddProcessLoggerMapping] (
    [ArcAddProcessLoggerMappingId] INT    IDENTITY (1, 1) NOT NULL,
    [ArcAddProcessingId]           BIGINT NOT NULL,
    [ProcessLogId]                 BIGINT NOT NULL,
    [ProcessNotificationId]        BIGINT NOT NULL,
    PRIMARY KEY CLUSTERED ([ArcAddProcessLoggerMappingId] ASC) WITH (FILLFACTOR = 95),
    CONSTRAINT [FK_ArcAddProcessLoggerMapping_ToArcAddProcessing] FOREIGN KEY ([ArcAddProcessingId]) REFERENCES [dbo].[ArcAddProcessing] ([ArcAddProcessingId])
);

