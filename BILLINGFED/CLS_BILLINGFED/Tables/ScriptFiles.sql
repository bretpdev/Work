CREATE TABLE [billing].[ScriptFiles] (
    [ScriptFileId]                  INT           IDENTITY (1, 1) NOT NULL,
    [ScriptID]                      VARCHAR (10)  NOT NULL,
    [SourceFile]                    VARCHAR (100) NOT NULL,
    [AddedBy]                       VARCHAR (50)  NULL,
    [AddedAt]                       DATETIME      NULL,
    [Active]                        BIT           DEFAULT ((1)) NOT NULL,
    [ProcessAllFiles]               BIT           NOT NULL,
    [BorrowerAccountNumberLocation] INT           NULL,
    [EndorserAccountNumberLocation] INT           NULL,
    [BorrowerCounterLocation]       INT           NULL,
    [UsesBulkLoadId]                BIT           CONSTRAINT [DF_ScriptFiles_UsesBulkLoad] DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([ScriptFileId] ASC)
);

