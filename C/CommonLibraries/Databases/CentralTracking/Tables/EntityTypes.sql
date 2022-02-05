CREATE TABLE [dbo].[EntityTypes] (
    [EntityTypeId]  INT          IDENTITY (1, 1) NOT NULL,
    [EntityTypeDescription]    VARCHAR (50) NOT NULL,
    [CreatedAt]     DATETIME      NOT NULL DEFAULT getdate() ,
    [CreatedBy]     INT          NOT NULL,
    [InactivatedAt] DATETIME     NULL,
    [InactivatedBy] INT          NULL,
    [Active]        AS
				CAST((CASE WHEN InactivatedBy IS NULL
					THEN 1
					ELSE 0
				END) AS bit)
    CONSTRAINT [PK_EntityTypes] PRIMARY KEY CLUSTERED ([EntityTypeId] ASC),
    CONSTRAINT [FK_EntityTypes_Entities_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Entities] ([EntityId]),
    CONSTRAINT [FK_EntityTypes_Entities_InactivatedBy] FOREIGN KEY ([InactivatedBy]) REFERENCES [dbo].[Entities] ([EntityId]), 
    CONSTRAINT [AK_EntityTypes_Column] UNIQUE (EntityTypeDescription), 
);



GO

CREATE TRIGGER [dbo].[Trigger_UpdateInactivatedAt_EntityType]
    ON [dbo].[EntityTypes]
    FOR UPDATE, INSERT
    AS
    BEGIN
        SET NoCount ON
			UPDATE et
			SET InactivatedAt = case when i.InactivatedBy is null then null else GETDATE() end
			FROM EntityTypes et
            JOIN inserted i on i.EntityTypeId = et.EntityTypeId
    END