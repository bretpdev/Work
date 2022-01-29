CREATE TABLE [peps].[COA] (
    [COA_ID]          BIGINT        IDENTITY (1, 1) NOT NULL,
    [RecordType]      VARCHAR (2)   NULL,
    [NewOpeId]        VARCHAR (8)   NULL,
    [ChangeIndicator] VARCHAR (1)   NULL,
    [PreviousOpeId]   VARCHAR (8)   NULL,
    [CoaActnCd]       VARCHAR (1)   NULL,
    [CoaEfftDt]       VARCHAR (8)   NULL,
    [DefaultCoaCd]    VARCHAR (1)   NULL,
    [Filler]          VARCHAR (256) NULL,
    [ProcessedAt]     DATETIME      NULL,
    [DeletedAt]       DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([COA_ID] ASC) WITH (FILLFACTOR = 95)
);

