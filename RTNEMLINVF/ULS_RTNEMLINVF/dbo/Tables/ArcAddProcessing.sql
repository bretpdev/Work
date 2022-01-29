﻿CREATE TABLE [dbo].[ArcAddProcessing] (
    [ArcAddProcessingId] BIGINT         IDENTITY (1, 1) NOT NULL,
    [ArcTypeId]          INT            NOT NULL,
    [ArcResponseCodeId]  INT            NULL,
    [AccountNumber]      CHAR (10)      NOT NULL,
    [RecipientId]        CHAR (9)       NULL,
    [ARC]                VARCHAR (5)    NOT NULL,
    [ActivityType]       VARCHAR (2)    NULL,
    [ActivityContact]    VARCHAR (2)    NULL,
    [ScriptId]           CHAR (10)      NOT NULL,
    [ProcessOn]          DATETIME       NOT NULL,
    [Comment]            VARCHAR (1233) NULL,
    [IsReference]        BIT            NOT NULL,
    [IsEndorser]         BIT            NOT NULL,
    [ProcessFrom]        DATETIME       NULL,
    [ProcessTo]          DATETIME       NULL,
    [NeededBy]           DATETIME       NULL,
    [RegardsTo]          CHAR (9)       NULL,
    [RegardsCode]        CHAR (1)       NULL,
    [LN_ATY_SEQ]         INT            NULL,
    [ProcessingAttempts] INT            DEFAULT ((0)) NOT NULL,
    [CreatedAt]          DATETIME       DEFAULT (getdate()) NOT NULL,
    [CreatedBy]          VARCHAR (50)   DEFAULT (suser_sname()) NOT NULL,
    [ProcessedAt]        DATETIME       NULL,
    CONSTRAINT [PK_ArcAddProcessing] PRIMARY KEY CLUSTERED ([ArcAddProcessingId] ASC) WITH (FILLFACTOR = 95),
    CONSTRAINT [CK_ArcAddProcessing_ActivtyType_ActivityContact] CHECK ([ActivityType] IS NULL AND [ActivityContact] IS NULL OR [ActivityType] IS NOT NULL AND [ActivityContact] IS NOT NULL),
    CONSTRAINT [FK_ArcAdd_ArcType] FOREIGN KEY ([ArcTypeId]) REFERENCES [dbo].[ArcType] ([ArcTypeId]),
    CONSTRAINT [FK_ArcAddProcessing_ArcResponseCode] FOREIGN KEY ([ArcResponseCodeId]) REFERENCES [dbo].[ArcResponseCodes] ([ArcResponseCodeId])
);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedBy]
    ON [dbo].[ArcAddProcessing]([CreatedBy] ASC)
    INCLUDE([ArcAddProcessingId], [ArcTypeId], [ArcResponseCodeId], [AccountNumber], [RecipientId], [ARC], [ActivityType], [ActivityContact], [ScriptId], [ProcessOn], [Comment], [IsReference], [IsEndorser], [ProcessFrom], [ProcessTo], [NeededBy], [RegardsTo], [RegardsCode], [LN_ATY_SEQ], [ProcessingAttempts], [CreatedAt], [ProcessedAt]) WITH (FILLFACTOR = 95);
