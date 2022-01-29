CREATE TABLE [comment_audit].[AgentUtIds] (
    [AgentUtId] INT      IDENTITY (1, 1) NOT NULL,
    [AgentId]   INT      NOT NULL,
    [UtId]      CHAR (5) NOT NULL,
    PRIMARY KEY CLUSTERED ([AgentUtId] ASC),
    CONSTRAINT [FK_AgentUtIds_Agents] FOREIGN KEY ([AgentId]) REFERENCES [comment_audit].[Agents] ([AgentId]) ON DELETE CASCADE,
    CONSTRAINT [AK_UtId] UNIQUE NONCLUSTERED ([UtId] ASC)
);


GO

CREATE TRIGGER [comment_audit].[Trigger_AgentUtIds_Update_Insert]
    ON [comment_audit].[AgentUtIds]
    FOR INSERT, UPDATE
    AS
    BEGIN
        SET NoCount ON

		insert into
			AgentUtIds_History (AgentUtId, AgentId, UtId, Deleted)
		select
			AgentUtId, AgentId, UtId, 0
		from
			inserted
    END
GO

CREATE TRIGGER [comment_audit].[Trigger_AgentUtIds_Delete]
    ON [comment_audit].[AgentUtIds]
    FOR DELETE
    AS
    BEGIN
        SET NoCount ON

		insert into
			AgentUtIds_History (AgentUtId, AgentId, UtId, Deleted)
		select
			AgentUtId, AgentId, UtId, 1
		from
			deleted
    END