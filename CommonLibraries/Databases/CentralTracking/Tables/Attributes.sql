CREATE TABLE [dbo].[Attributes] (
    [AttributeId]              INT          IDENTITY (1, 1) NOT NULL,
    [AttributeDescription]            VARCHAR (50) NOT NULL,
    [AttributeLongDescription]        VARCHAR(MAX) NOT NULL,
    [AttributeDataTypeId]      INT          NOT NULL,
    [AttributeSelectionTypeId] INT          NOT NULL,
    [CreatedAt]                DATETIME     CONSTRAINT [DF_Attributes_CreatedAt] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]                INT          NOT NULL,
    [InactivatedAt]            DATETIME     NULL,
    [InactivatedBy]            INT          NULL,
    [Active]                   AS
				CAST((CASE WHEN InactivatedBy IS NULL
					THEN 1
					ELSE 0
				END) AS bit),
    CONSTRAINT [PK_Attributes] PRIMARY KEY CLUSTERED ([AttributeId] ASC),
    CONSTRAINT [FK_Attributes_AttributeDataTypes] FOREIGN KEY ([AttributeDataTypeId]) REFERENCES [dbo].[AttributeDataTypes] ([AttributeDataTypeId]),
    CONSTRAINT [FK_Attributes_AttributeSelectionTypes] FOREIGN KEY ([AttributeSelectionTypeId]) REFERENCES [dbo].[AttributeSelectionTypes] ([AttributeSelectionTypeId]),
    CONSTRAINT [FK_Attributes_Entities] FOREIGN KEY ([InactivatedBy]) REFERENCES [dbo].[Entities] ([EntityId]),
    CONSTRAINT [FK_Attributes_Entities_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Entities] ([EntityId]), 
    CONSTRAINT [AK_Attributes_AttributeDescription] UNIQUE (AttributeDescription)
);

GO 

CREATE TRIGGER [dbo].[Trigger_UpdateInactivatedAt_Attributes]
    ON [dbo].[Attributes]
    FOR UPDATE, INSERT
    AS
    BEGIN
        SET NoCount ON
			UPDATE att
			SET InactivatedAt = case when i.InactivatedBy is null then null else GETDATE() end
			FROM Attributes att
            JOIN inserted i on i.AttributeId = att.AttributeId
    END





