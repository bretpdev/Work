CREATE TABLE [dbo].[EntityTypeAttributes] (
    [EntityTypeAttributeId] INT      IDENTITY (1, 1) NOT NULL,
    [EntityTypeId]          INT      NOT NULL,
    [AttributeId]           INT      NOT NULL,
    [CreatedAt]             DATETIME CONSTRAINT [DF_EntityTypeAttributes_CreatedAt] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]             INT      NOT NULL,
    [InactivatedAt]         DATETIME NULL,
    [InactivatedBy]         INT      NULL,
    [Active]        AS
				CAST((CASE WHEN InactivatedBy IS NULL
					THEN 1
					ELSE 0
				END) AS bit),
    CONSTRAINT [PK_EntityTypeAttributes] PRIMARY KEY CLUSTERED ([EntityTypeAttributeId] ASC),
    CONSTRAINT [FK_EntityTypeAttributes_Attributes] FOREIGN KEY ([AttributeId]) REFERENCES [dbo].[Attributes] ([AttributeId]),
    CONSTRAINT [FK_EntityTypeAttributes_Entities] FOREIGN KEY ([InactivatedBy]) REFERENCES [dbo].[Entities] ([EntityId]),
    CONSTRAINT [FK_EntityTypeAttributes_Entities_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Entities] ([EntityId]),
    CONSTRAINT [FK_EntityTypeAttributes_EntityTypes] FOREIGN KEY ([EntityTypeId]) REFERENCES [dbo].[EntityTypes] ([EntityTypeId]), 
    CONSTRAINT [AK_EntityTypeAttributes_EntityTypeIdAttributeId] UNIQUE (EntityTypeId, AttributeId)
);

GO

CREATE TRIGGER [dbo].[Trigger_UpdateInactivatedAt_EntityTypeAttribute]
    ON [dbo].[EntityTypeAttributes]
    FOR UPDATE, INSERT
    AS
    BEGIN
        SET NoCount ON
			UPDATE eta
			SET InactivatedAt = case when i.InactivatedBy is null then null else GETDATE() end
			FROM EntityTypeAttributes eta
            JOIN inserted i on i.EntityTypeAttributeId = eta.EntityTypeAttributeId
    END





