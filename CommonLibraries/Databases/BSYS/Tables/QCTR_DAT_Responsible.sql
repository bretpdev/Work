CREATE TABLE [dbo].[QCTR_DAT_Responsible] (
    [IssueID] INT          NOT NULL,
    [UserID]  VARCHAR (20) NOT NULL,
    CONSTRAINT [PK_QCTR_DAT_Responsible] PRIMARY KEY CLUSTERED ([IssueID] ASC, [UserID] ASC) WITH (FILLFACTOR = 90)
);

