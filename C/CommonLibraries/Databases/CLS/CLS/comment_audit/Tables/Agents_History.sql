CREATE TABLE [comment_audit].[Agents_History] (
    [AgentHistoryId]  INT            IDENTITY (1, 1) NOT NULL,
    [AgentId]         INT            NOT NULL,
    [FullName]        NVARCHAR (100) NOT NULL,
    [AuditPercentage] DECIMAL (5, 2) NOT NULL,
    [Active]          BIT            NOT NULL,
    [UpdatedAt]       DATETIME       DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]       VARCHAR (50)   DEFAULT (suser_sname()) NOT NULL,
    [Deleted]         BIT            NOT NULL,
    PRIMARY KEY CLUSTERED ([AgentHistoryId] ASC)
);

