CREATE TABLE [dbo].[SCKR_REF_TestIssues] (
    [Sequence]    INT           IDENTITY (1, 1) NOT NULL,
    [Request]     INT           NULL,
    [Class]       NVARCHAR (3)  NULL,
    [TestNo]      INT           NULL,
    [TestType]    NVARCHAR (50) NULL,
    [Unit]        NVARCHAR (50) NULL,
    [IssueDate]   SMALLDATETIME CONSTRAINT [DF_SCKR_REF_TestIssues_IssueDate] DEFAULT (CONVERT([datetime],floor(CONVERT([real],getdate(),0)),0)) NULL,
    [IssueType]   NVARCHAR (50) NULL,
    [Description] NTEXT         NULL,
    [Resolution]  NTEXT         NULL,
    [Agent]       NVARCHAR (50) NULL,
    CONSTRAINT [PK_SCKR_REF_TestIssues] PRIMARY KEY CLUSTERED ([Sequence] ASC)
);

