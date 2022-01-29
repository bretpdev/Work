CREATE TABLE [billing].[PrintProcessing] (
    [PrintProcessingId]      INT           IDENTITY (1, 1) NOT NULL,
    [BulkLoadId]             BIGINT        NULL,
    [AccountNumber]          CHAR (10)     NOT NULL,
    [ScriptFileId]           INT           NULL,
    [SourceFile]             VARCHAR (100) NOT NULL,
    [ArcAddedAt]             DATETIME      NULL,
    [ImagedAt]               DATETIME      NULL,
    [EcorrDocumentCreatedAt] DATETIME      NULL,
    [PrintedAt]              DATETIME      NULL,
    [CreatedBy]              VARCHAR (50)  NOT NULL,
    [AddedAt]                DATETIME      DEFAULT (getdate()) NOT NULL,
    [OnEcorr]                BIT           DEFAULT ((0)) NOT NULL,
    [DeletedAt]              DATETIME      NULL,
    [DeletedBy]              VARCHAR (50)  NULL,
    PRIMARY KEY NONCLUSTERED ([PrintProcessingId] ASC) WITH (FILLFACTOR = 95),
    FOREIGN KEY ([ScriptFileId]) REFERENCES [billing].[ScriptFiles] ([ScriptFileId])
);


GO
CREATE CLUSTERED INDEX [ICX_AccountNumber]
    ON [billing].[PrintProcessing]([AccountNumber] ASC, [SourceFile] ASC) WITH (FILLFACTOR = 90, PAD_INDEX = ON);

