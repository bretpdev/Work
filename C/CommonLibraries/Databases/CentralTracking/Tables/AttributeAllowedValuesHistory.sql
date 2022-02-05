CREATE TABLE [dbo].[AttributeAllowedValuesHistory] (
    [AttributeAllowedValuesHistoryId] INT      NOT NULL,
    [AttributeAllowedValueId]         INT      NOT NULL,
    [AttributeId]                     INT      NOT NULL,
    [ValueId]                         INT      NOT NULL,
    [CreatedAt]                       DATETIME CONSTRAINT [DF_AttributeAllowedValuesHistory_CreatedAt] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]                       INT      NOT NULL,
    [InactivatedAt]                   DATETIME NULL,
    [InactivatedBy]                   INT      NULL,
    [Action] INT NOT NULL, 
    CONSTRAINT [PK_AttributeAllowedValuesHistory] PRIMARY KEY CLUSTERED ([AttributeAllowedValuesHistoryId] ASC),
    CONSTRAINT [FK_AttributeAllowedValuesHistory_Attributes] FOREIGN KEY ([AttributeId]) REFERENCES [dbo].[Attributes] ([AttributeId]),
    CONSTRAINT [FK_AttributeAllowedValuesHistory_Entities] FOREIGN KEY ([InactivatedBy]) REFERENCES [dbo].[Entities] ([EntityId]),
    CONSTRAINT [FK_AttributeAllowedValuesHistory_Entities_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Entities] ([EntityId]),
    CONSTRAINT [FK_AttributeAllowedValuesHistory_Values] FOREIGN KEY ([ValueId]) REFERENCES [dbo].[Values] ([ValueId])
);

