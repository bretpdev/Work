CREATE TABLE [dbo].[OLD_NDHP_LST_Reports] (
    [ReportName]  VARCHAR (100)  NOT NULL,
    [ReportForm]  VARCHAR (50)   NOT NULL,
    [ReportQuery] VARCHAR (8000) NOT NULL
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SQL query statement.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLD_NDHP_LST_Reports', @level2type = N'COLUMN', @level2name = N'ReportQuery';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Name of the form to use to view the report.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLD_NDHP_LST_Reports', @level2type = N'COLUMN', @level2name = N'ReportForm';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Name of the report.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OLD_NDHP_LST_Reports', @level2type = N'COLUMN', @level2name = N'ReportName';

