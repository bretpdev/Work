CREATE TABLE [dbo].[ArcAddProcessing] (
    [ArcAddProcessingId] BIGINT        IDENTITY (1, 1) NOT NULL,
    [ArcTypeId]          INT           NOT NULL,
    [ArcResponseCodeId]  INT           NULL,
    [AccountNumber]      CHAR (10)     NOT NULL,
    [RecipientId]        CHAR (9)      NULL,
    [ARC]                VARCHAR (5)   NOT NULL,
    [ScriptId]           CHAR (10)     NOT NULL,
    [ProcessOn]          DATETIME      NOT NULL,
    [Comment]            VARCHAR (300) NULL,
    [IsReference]        BIT           NOT NULL,
    [IsEndorser]         BIT           NOT NULL,
    [ProcessFrom]        DATETIME      NULL,
    [ProcessTo]          DATETIME      NULL,
    [NeededBy]           DATETIME      NULL,
    [RegardsTo]          CHAR (9)      NULL,
    [RegardsCode]        CHAR (1)      NULL,
    [LN_ATY_SEQ]         INT           NULL,
    [ProcessingAttempts] INT           DEFAULT ((0)) NOT NULL,
    [CreatedAt]          DATETIME      DEFAULT (getdate()) NOT NULL,
    [CreatedBy]          VARCHAR (50)  DEFAULT (suser_sname()) NOT NULL,
    [ProcessedAt]        DATETIME      NULL,
    CONSTRAINT [PK_ArcAddProcessing] PRIMARY KEY NONCLUSTERED ([ArcAddProcessingId] ASC),
    CONSTRAINT [FK_ArcAddProcessing_ArcType] FOREIGN KEY ([ArcTypeId]) REFERENCES [dbo].[ArcType] ([ArcTypeId])
);




GO
CREATE NONCLUSTERED INDEX [IX_ProcessOn_CreatedAt]
    ON [dbo].[ArcAddProcessing]([ProcessOn] ASC, [CreatedAt] ASC)
    INCLUDE([ArcAddProcessingId], [LN_ATY_SEQ], [ProcessingAttempts]);


GO
CREATE NONCLUSTERED INDEX [IX_ProcessedAt_ProcessOn]
    ON [dbo].[ArcAddProcessing]([ProcessedAt] ASC, [ProcessOn] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ArcTypeId_ARC_ScriptId_AccountNumber_ProcessOn]
    ON [dbo].[ArcAddProcessing]([ArcTypeId] ASC, [ARC] ASC, [ScriptId] ASC, [AccountNumber] ASC, [ProcessOn] ASC)
    INCLUDE([Comment]);


GO
CREATE NONCLUSTERED INDEX [IX_ARC_CreatedAt]
    ON [dbo].[ArcAddProcessing]([ARC] ASC, [CreatedAt] ASC)
    INCLUDE([AccountNumber], [Comment]);


GO
CREATE CLUSTERED INDEX [CIX_ArcAddProcessingId]
    ON [dbo].[ArcAddProcessing]([ArcAddProcessingId] ASC);

