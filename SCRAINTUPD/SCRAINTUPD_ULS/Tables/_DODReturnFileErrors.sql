CREATE TABLE [scra].[_DODReturnFileErrors] (
    [DODReturnFileErrorsId] INT           IDENTITY (1, 1) NOT NULL,
    [SourceFile]            VARCHAR (54)  NULL,
    [LineData]              VARCHAR (255) NULL,
    [ErrorCode]             INT           NULL,
    [ErrorColumn]           INT           NULL,
    [ErrorDescription]      VARCHAR (255) NULL,
    [DateRun]               DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([DODReturnFileErrorsId] ASC) WITH (FILLFACTOR = 95)
);

