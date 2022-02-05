CREATE TABLE [comment_audit].[Agents] (
    [AgentId]         INT            IDENTITY (1, 1) NOT NULL,
    [FullName]        NVARCHAR (100) NOT NULL,
    [AuditPercentage] DECIMAL (5, 2) NOT NULL,
    [Active]          BIT            NOT NULL,
    PRIMARY KEY CLUSTERED ([AgentId] ASC),
    CONSTRAINT [CK_Agents_AuditPercentage] CHECK ([AuditPercentage]>=(0) AND [AuditPercentage]<=(100)),
    CONSTRAINT [UK_Agents_FullName] UNIQUE NONCLUSTERED ([FullName] ASC)
);


GO

CREATE TRIGGER [comment_audit].[Trigger_Agents_Insert_Update]
    ON [comment_audit].[Agents]
    FOR INSERT, UPDATE
    AS
    BEGIN
        SET NoCount ON

		insert into 
			Agents_History (AgentId, FullName, AuditPercentage, Active, Deleted)
		select 
			AgentId, FullName, AuditPercentage, Active, 0
		from
			inserted
    END
GO


CREATE TRIGGER [comment_audit].[Trigger_Agents_Delete]
    ON [comment_audit].[Agents]
    FOR DELETE
    AS
    BEGIN
        SET NoCount ON

		insert into 
			Agents_History (AgentId, FullName, AuditPercentage, Active, Deleted)
		select 
			AgentId, FullName, AuditPercentage, Active, 1
		from
			deleted d
    END