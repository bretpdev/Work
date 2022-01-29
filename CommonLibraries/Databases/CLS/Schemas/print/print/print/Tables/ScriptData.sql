CREATE TABLE [print].[ScriptData] (
    [ScriptDataId]              INT           IDENTITY (1, 1) NOT NULL,
    [ScriptId]                  VARCHAR (10)  NOT NULL,
    [SourceFile]                VARCHAR (100) NULL,
    [LetterId]                  INT           NOT NULL,
    [DocIdId]                   INT           NULL,
    [FileHeaderId]              INT           NOT NULL,
    [ProcessAllFiles]           BIT           NOT NULL,
    [IsEndorser]                BIT           DEFAULT ((0)) NOT NULL,
    [EndorsersBorrowerSSNIndex] INT           NULL,
    [Priority]                  INT           NULL,
    [AddBarCodes]               BIT           DEFAULT ((0)) NOT NULL,
    [DoNotProcessEcorr]         BIT           DEFAULT ((0)) NOT NULL,
    [LastProcessed]             DATETIME      DEFAULT (getdate()) NOT NULL,
    [AddedBy]                   VARCHAR (50)  NULL,
    [AddedAt]                   DATETIME      DEFAULT (getdate()) NOT NULL,
    [Active]                    BIT           DEFAULT ((1)) NOT NULL,
    [CheckForCoBorrower]    BIT           CONSTRAINT [DF__ScriptDat__DoNot__04721D3E] DEFAULT ((0)) NOT NULL,
	[BarcodeOffset]             INT           NULL,
    PRIMARY KEY CLUSTERED ([ScriptDataId] ASC) WITH (FILLFACTOR = 95),
    CONSTRAINT [CK_ScriptData_EndorserBorrowerSSNIndex] CHECK ([IsEndorser]=(0) OR [EndorsersBorrowerSSNIndex] IS NOT NULL),
    CONSTRAINT [CK_ScriptData_EndorserBorrowerSSNIndex2] CHECK ([IsEndorser]=(1) OR [EndorsersBorrowerSSNIndex] IS NULL),
    CONSTRAINT [FK_ScriptFiles_ToDocIds] FOREIGN KEY ([DocIdId]) REFERENCES [print].[DocIds] ([DocIdId]),
    CONSTRAINT [FK_ScriptFiles_ToFileHeaders] FOREIGN KEY ([FileHeaderId]) REFERENCES [print].[FileHeaders] ([FileHeaderId]),
    CONSTRAINT [FK_ScriptFiles_ToLetters] FOREIGN KEY ([LetterId]) REFERENCES [print].[Letters] ([LetterId]),
    CONSTRAINT [AK_ScriptData_Priority] UNIQUE NONCLUSTERED ([Priority] ASC) WITH (FILLFACTOR = 95)
);



