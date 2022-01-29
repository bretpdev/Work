CREATE TABLE [dbo].[Entities] (
    [EntityId]      INT           IDENTITY (1, 1) NOT NULL,
    [EntityTypeId]  INT           NOT NULL,
    [EntityName]    VARCHAR (256) NULL,
    [CreatedAt]     DATETIME      CONSTRAINT [DF_Entities_CreatedAt] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]     INT           NOT NULL,
    [InactivatedAt] DATETIME      NULL,
    [InactivatedBy] INT           NULL,
    [Active]        AS
				CAST((CASE WHEN InactivatedBy IS NULL
					THEN 1
					ELSE 0
				END) AS bit)
    CONSTRAINT [PK_Entities] PRIMARY KEY CLUSTERED ([EntityId] ASC),
    --CONSTRAINT [FK_Entities_Entities_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Entities] ([EntityId]),
    CONSTRAINT [FK_Entities_Entities_InactivatedBy] FOREIGN KEY ([InactivatedBy]) REFERENCES [dbo].[Entities] ([EntityId]),
    CONSTRAINT [FK_Entities_EntityTypes] FOREIGN KEY ([EntityTypeId]) REFERENCES [dbo].[EntityTypes] ([EntityTypeId])
);

GO

CREATE TRIGGER [dbo].[Trigger_UpdateInactivatedAt_Entities]
    ON [dbo].[Entities]
    FOR UPDATE, INSERT
    AS
    BEGIN
        SET NoCount ON
			UPDATE E
			SET InactivatedAt = case when i.InactivatedBy is null then null else GETDATE() end
			FROM Entities E
            JOIN inserted i on i.EntityId = E.EntityId
    END







