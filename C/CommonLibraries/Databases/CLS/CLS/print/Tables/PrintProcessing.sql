CREATE TABLE [print].[PrintProcessing] (
    [PrintProcessingId]      INT           IDENTITY (1, 1) NOT NULL,
    [AccountNumber]          CHAR (10)     NOT NULL,
    [EmailAddress]           VARCHAR (256) NULL,
    [ScriptDataId]           INT           NOT NULL,
    [SourceFile]             VARCHAR (100) NULL,
    [LetterData]             VARCHAR (MAX) NOT NULL,
    [CostCenter]             VARCHAR (10)  NOT NULL,
    [OnEcorr]                BIT           DEFAULT ((0)) NOT NULL,
    [ArcAddProcessingId]     BIGINT        NULL,
    [ArcNeeded]              BIT           DEFAULT ((0)) NOT NULL,
    [ImagedAt]               DATETIME      NULL,
    [ImagingNeeded]          BIT           DEFAULT ((0)) NOT NULL,
    [EcorrDocumentCreatedAt] DATETIME      NULL,
    [PrintedAt]              DATETIME      NULL,
    [AddedBy]                VARCHAR (50)  NOT NULL,
    [AddedAt]                DATETIME      DEFAULT (getdate()) NOT NULL,
    [DeletedAt]              DATE          NULL,
    [DeletedBy]              VARCHAR (50)  NULL,
    PRIMARY KEY NONCLUSTERED ([PrintProcessingId] ASC) WITH (FILLFACTOR = 95),
    FOREIGN KEY ([ScriptDataId]) REFERENCES [print].[ScriptData] ([ScriptDataId]),
    FOREIGN KEY ([ScriptDataId]) REFERENCES [print].[ScriptData] ([ScriptDataId]),
    CONSTRAINT [FK_PrintProcessing_ToArcAddProcessing] FOREIGN KEY ([ArcAddProcessingId]) REFERENCES [dbo].[ArcAddProcessing] ([ArcAddProcessingId])
);






GO


