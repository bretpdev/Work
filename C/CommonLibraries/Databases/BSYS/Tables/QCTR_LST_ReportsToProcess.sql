CREATE TABLE [dbo].[QCTR_LST_ReportsToProcess] (
    [ReportName]       VARCHAR (20)  NOT NULL,
    [Subject]          VARCHAR (100) NULL,
    [ReportType]       CHAR (2)      NULL,
    [RequiredDays]     INT           NULL,
    [PriorityCategory] VARCHAR (200) NULL,
    [PriorityUrgency]  VARCHAR (200) NULL,
    [BusinessUnit]     NVARCHAR (50) NULL,
    CONSTRAINT [PK_QCTR_LST_ReportsToProcess] PRIMARY KEY CLUSTERED ([ReportName] ASC),
    CONSTRAINT [FK_QCTR_LST_ReportsToProcess_GENR_REF_PriorityCatgryOps] FOREIGN KEY ([PriorityCategory]) REFERENCES [dbo].[GENR_REF_PriorityCatgryOps] ([CatOption]) ON UPDATE CASCADE,
    CONSTRAINT [FK_QCTR_LST_ReportsToProcess_GENR_REF_PriorityUrgencyOps] FOREIGN KEY ([PriorityUrgency]) REFERENCES [dbo].[GENR_REF_PriorityUrgencyOps] ([UrgOption]) ON UPDATE CASCADE,
    CONSTRAINT [FK_QCTR_LST_ReportsToProcess_QCTR_LST_ReportsToProcess] FOREIGN KEY ([BusinessUnit]) REFERENCES [dbo].[GENR_LST_BusinessUnits] ([BusinessUnit]) ON UPDATE CASCADE
);

