CREATE TABLE [dbo].[CreateFsaReports] (
    [ReportId]   INT           IDENTITY (1, 1) NOT NULL,
    [ReportName] VARCHAR (150) NOT NULL,
    [FileName]   VARCHAR (50)  NOT NULL,
    [Occurance]  VARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_CreateFsaReports] PRIMARY KEY CLUSTERED ([ReportId] ASC)
);

