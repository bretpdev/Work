CREATE TABLE [fp].[FileProcessing] (
    [FileProcessingId] INT           IDENTITY (1, 1) NOT NULL,
    [GroupKey]         VARCHAR (50)  NULL,
    [ScriptFileId]     INT           NULL,
    [SourceFile]       VARCHAR (100) NOT NULL,
    [ProcessedAt]      DATETIME      NULL,
    [CreatedBy]        VARCHAR (50)  NOT NULL,
    [AddedAt]          DATETIME      CONSTRAINT [DF__tmp_ms_xx__Added__21A1C21B] DEFAULT (getdate()) NOT NULL,
    [OnEcorr]          BIT           CONSTRAINT [DF_OnEcorr] DEFAULT ((0)) NOT NULL,
    [DeletedAt]        DATETIME      NULL,
    [DeletedBy]        VARCHAR (50)  NULL,
    CONSTRAINT [PK__tmp_ms_x__E5FE7B781FB979A9] PRIMARY KEY CLUSTERED ([FileProcessingId] ASC) WITH (FILLFACTOR = 95),
    CONSTRAINT [FK_FileProcessing_ScriptFiles] FOREIGN KEY ([ScriptFileId]) REFERENCES [fp].[ScriptFiles] ([ScriptFileId])
);





