CREATE TABLE [dbo].[QCTR_DAT_Email] (
    [IssueID] INT          NOT NULL,
    [Email]   VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_QCTR_LST_Email] PRIMARY KEY CLUSTERED ([IssueID] ASC, [Email] ASC)
);

