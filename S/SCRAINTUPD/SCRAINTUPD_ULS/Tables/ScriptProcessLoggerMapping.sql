CREATE TABLE [scra].[ScriptProcessLoggerMapping] (
    [ScriptProcessLoggerMappingId] INT    IDENTITY (1, 1) NOT NULL,
    [ScriptProcessingId]           INT    NOT NULL,
    [ProcessLogId]                 BIGINT NOT NULL,
    [ProcessNotificationId]        BIGINT NOT NULL,
    PRIMARY KEY CLUSTERED ([ScriptProcessLoggerMappingId] ASC) WITH (FILLFACTOR = 95),
    CONSTRAINT [FK_ScriptProcessLoggerMapping_ScriptProcessing] FOREIGN KEY ([ScriptProcessingId]) REFERENCES [scra].[ScriptProcessing] ([ScriptProcessingId])
);

