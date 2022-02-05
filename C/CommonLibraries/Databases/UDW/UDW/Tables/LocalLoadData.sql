CREATE TABLE [dbo].[LocalLoadData]
(
	[LocalLoadDataID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [LocalLoadFileID] INT NOT NULL, 
    [ReportNumber] INT NOT NULL, 
    [SasCodeName] VARCHAR(250) NOT NULL, 
    [LastSuccessfulRun] DATETIME NULL
)
