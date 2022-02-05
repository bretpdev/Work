CREATE TABLE [dbo].[AttributeAllowedValues] (
    [AttributeAllowedValueId] INT      IDENTITY (1, 1) NOT NULL,
    [AttributeId]             INT      NOT NULL,
    [ValueId]                 INT      NOT NULL,
    [CreatedAt]               DATETIME CONSTRAINT [DF_AttributeAllowedValues_CreatedAt] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]               INT      NOT NULL,
    [InactivatedAt]           DATETIME NULL,
    [InactivatedBy]           INT      NULL,
    [Active]                  BIT      CONSTRAINT [DF_AttributeAllowedValues_Active] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_AttributeAllowedValues] PRIMARY KEY CLUSTERED ([AttributeAllowedValueId] ASC),
    CONSTRAINT [FK_AttributeAllowedValues_Attributes] FOREIGN KEY ([AttributeId]) REFERENCES [dbo].[Attributes] ([AttributeId]),
    CONSTRAINT [FK_AttributeAllowedValues_Entities] FOREIGN KEY ([InactivatedBy]) REFERENCES [dbo].[Entities] ([EntityId]),
    CONSTRAINT [FK_AttributeAllowedValues_Entities_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Entities] ([EntityId]),
    CONSTRAINT [FK_AttributeAllowedValues_Values] FOREIGN KEY ([ValueId]) REFERENCES [dbo].[Values] ([ValueId]), 
    CONSTRAINT [AK_AttributeAllowedValues_AttributeIdValueId] UNIQUE (AttributeId, ValueId)
);






