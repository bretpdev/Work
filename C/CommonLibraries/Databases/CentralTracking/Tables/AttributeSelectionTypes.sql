CREATE TABLE [dbo].[AttributeSelectionTypes] (
    [AttributeSelectionTypeId] INT          IDENTITY (1, 1) NOT NULL,
    [AttributeSelectionTypeDescription]   VARCHAR (50) NOT NULL,
    [CreatedAt]                DATETIME     CONSTRAINT [DF_AttributeSelectionTypes_CreatedAt] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]                INT          NOT NULL,
    [InactivatedAt]            DATETIME     NULL,
    [InactivatedBy]            INT          NULL,
    [Active]        AS
				CAST((CASE WHEN InactivatedBy IS NULL
					THEN 1
					ELSE 0
				END) AS bit),
    CONSTRAINT [PK_AttributeSelectionTypes] PRIMARY KEY CLUSTERED ([AttributeSelectionTypeId] ASC),
    CONSTRAINT [FK_AttributeSelectionTypes_Entities] FOREIGN KEY ([InactivatedBy]) REFERENCES [dbo].[Entities] ([EntityId]),
    CONSTRAINT [FK_AttributeSelectionTypes_Entities_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Entities] ([EntityId]), 
    CONSTRAINT [AK_AttributeSelectionTypes_AttributeSelectionTypeDescription] UNIQUE ([AttributeSelectionTypeDescription])
);






GO

CREATE TRIGGER [dbo].[Trigger_UpdateInactivatedAt_AttributeSelectionTypes]
    ON [dbo].[AttributeSelectionTypes]
    FOR UPDATE, INSERT
    AS
    BEGIN
        SET NoCount ON
			UPDATE st
			SET InactivatedAt = case when i.InactivatedBy is null then null else GETDATE() end
			FROM AttributeSelectionTypes st
            JOIN inserted i on i.AttributeSelectionTypeId = st.AttributeSelectionTypeId
    END