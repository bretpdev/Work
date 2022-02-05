CREATE TABLE [dbo].[QCTR_DAT_Status] (
    [ID]      BIGINT       IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [IssueID] INT          NOT NULL,
    [Status]  VARCHAR (50) NOT NULL,
    [Updated] DATETIME     NOT NULL,
    [Ended]   DATETIME     NULL
);

