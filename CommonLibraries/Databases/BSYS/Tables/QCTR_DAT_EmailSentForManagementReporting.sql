CREATE TABLE [dbo].[QCTR_DAT_EmailSentForManagementReporting] (
    [ReportType] VARCHAR (50) NOT NULL,
    [DateSent]   DATETIME     CONSTRAINT [DF_QCTR_DAT_EmailSentForManagementReporting_DateSent] DEFAULT (getdate()) NOT NULL
);

