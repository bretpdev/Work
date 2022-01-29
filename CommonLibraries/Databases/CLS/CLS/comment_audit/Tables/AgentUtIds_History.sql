CREATE TABLE [comment_audit].[AgentUtIds_History] (
    [AgentUtIdHistoryId] INT          IDENTITY (1, 1) NOT NULL,
    [AgentUtId]          INT          NOT NULL,
    [AgentId]            INT          NOT NULL,
    [UtId]               CHAR (6)     NOT NULL,
    [UpdatedAt]          DATETIME     DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]          VARCHAR (50) DEFAULT (suser_sname()) NOT NULL,
    [Deleted]            BIT          NOT NULL,
    PRIMARY KEY CLUSTERED ([AgentUtIdHistoryId] ASC)
);

