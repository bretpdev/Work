CREATE TABLE [fp].[ScriptFiles] (
    [ScriptFileId]          INT           IDENTITY (1, 1) NOT NULL,
    [ScriptID]              VARCHAR (10)  NOT NULL,
    [SourceFile]            VARCHAR (100) NOT NULL,
    [LetterId]              INT           NULL,
    [DocId]                 INT           NULL,
    [ArcId]                 INT           NULL,
    [FileHeaderId]          INT           NULL,
    [AddedBy]               VARCHAR (50)  NULL,
    [AddedAt]               DATETIME      DEFAULT (getdate()) NOT NULL,
    [Active]                BIT           DEFAULT ((1)) NOT NULL,
    [ProcessAllFiles]       BIT           NOT NULL,
    [AccountNumberLocation] INT           NULL,
    PRIMARY KEY CLUSTERED ([ScriptFileId] ASC),
    CONSTRAINT [FK_ScriptFiles_Arcs] FOREIGN KEY ([ArcId]) REFERENCES [fp].[Arcs] ([ArcId]),
    CONSTRAINT [FK_ScriptFiles_DocIds] FOREIGN KEY ([DocId]) REFERENCES [fp].[DocIds] ([DocId]),
    CONSTRAINT [FK_ScriptFiles_FileHeaders] FOREIGN KEY ([FileHeaderId]) REFERENCES [fp].[FileHeaders] ([FileHeaderId]),
    CONSTRAINT [FK_ScriptFiles_Letters] FOREIGN KEY ([LetterId]) REFERENCES [fp].[Letters] ([LetterId])
);



