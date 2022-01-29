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
    CONSTRAINT [PK_ArcAddProcessing] PRIMARY KEY CLUSTERED ([ArcAddProcessingId] ASC) WITH (FILLFACTOR = 95),
    CONSTRAINT [FK_ArcAddProcessing_ArcType] FOREIGN KEY ([ArcTypeId]) REFERENCES [dbo].[ArcType] ([ArcTypeId])
);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedAt>]
    ON [dbo].[ArcAddProcessing]([ARC] ASC, [CreatedAt] ASC)
    INCLUDE([AccountNumber], [Comment]) WITH (FILLFACTOR = 95);

