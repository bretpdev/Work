CREATE TABLE [dbo].[AttributeDataTypes] (
    [AttributeDataTypeId] INT          IDENTITY (1, 1) NOT NULL,
    [AttributeDataTypeDescription]   VARCHAR (32) NOT NULL,
    [CreatedAt]           DATETIME     CONSTRAINT [DF_AttributeDataTypes_CreatedAt] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]           INT          NOT NULL,
    [InactivatedAt]       DATETIME     NULL,
    [InactivatedBy]       INT          NULL,
    [Active]        AS
				CAST((CASE WHEN InactivatedBy IS NULL
					THEN 1
					ELSE 0
				END) AS bit)
    CONSTRAINT [PK_AttributeDataTypes] PRIMARY KEY CLUSTERED ([AttributeDataTypeId] ASC), 
    CONSTRAINT [AK_AttributeDataTypes_AttributeDataTypeDescription] UNIQUE (AttributeDataTypeDescription)
);






GO

CREATE TRIGGER [dbo].[Trigger_UpdateInactivatedAt_AttributeDataTypes]
    ON [dbo].[AttributeDataTypes]
    FOR UPDATE, INSERT
    AS
    BEGIN
        SET NoCount ON
			UPDATE adt
			SET InactivatedAt = case when i.InactivatedBy is null then null else GETDATE() end
			FROM AttributeDataTypes adt
            JOIN inserted i on i.AttributeDataTypeId = adt.AttributeDataTypeId
    END