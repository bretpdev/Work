CREATE TABLE [dbo].[QCTR_DAT_UserIntervention] (
    [ID]           BIGINT         IDENTITY (1, 1) NOT NULL,
    [ReportName]   VARCHAR (20)   NULL,
    [UserID]       VARCHAR (15)   NULL,
    [ActivityDate] DATETIME       NULL,
    [Description]  VARCHAR (1000) NULL,
    [RequiredDays] INT            NULL,
    [BusinessUnit] VARCHAR (50)   NULL,
    [SavedDate]    DATETIME       NULL,
    CONSTRAINT [PK_QCTR_DAT_UserIntervention] PRIMARY KEY CLUSTERED ([ID] ASC)
);

