CREATE TABLE [dbo].[LocalLoadData] (
    [LocalLoadDataID]   INT           IDENTITY (1, 1) NOT NULL,
    [LocalLoadFileID]   INT           NOT NULL,
    [ReportNumber]      INT           NOT NULL,
    [SasCodeName]       VARCHAR (250) NOT NULL,
    [LastSuccessfulRun] DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([LocalLoadDataID] ASC)
);


