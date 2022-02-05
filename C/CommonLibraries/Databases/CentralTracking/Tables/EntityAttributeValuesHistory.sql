CREATE TABLE [dbo].[EntityAttributeValuesHistory] (
    [EntityAttributeValuesHistoryId] INT     IDENTITY (1, 1) NOT NULL,
    [EntityAttributeValueId]          INT       NOT NULL,
    [EntityId]                        INT      NOT NULL,
    [AttributeId]                     INT      NOT NULL,
    [ValueId]						  INT	   NOT NULL, 
    [CreatedAt]                       DATETIME  NOT NULL,
    [CreatedBy]                       INT      NOT NULL,
    [HistoryStatusTypeId]             INT      NOT NULL,
    [HistoryStatusDate] DATETIME NOT NULL DEFAULT (getdate()), 
    [HistoryStatusCreatedBy] INT NOT NULL, 
    CONSTRAINT [PK_EntityAttributeValuesHistory] PRIMARY KEY CLUSTERED ([EntityAttributeValuesHistoryId] ASC),
    CONSTRAINT [FK_EntityAttributeValuesHistory_Attributes] FOREIGN KEY ([AttributeId]) REFERENCES [dbo].[Attributes] ([AttributeId]),
    CONSTRAINT [FK_EntityAttributeValuesHistory_Entities] FOREIGN KEY ([EntityId]) REFERENCES [dbo].[Entities] ([EntityId]),
    CONSTRAINT [FK_EntityAttributeValuesHistory_Entities_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Entities] ([EntityId]),
    CONSTRAINT [FK_EntityAttributeValuesHistory_HistoryStatusTypes] FOREIGN KEY ([HistoryStatusTypeId]) REFERENCES [dbo].[HistoryStatusTypes] ([HistoryStatusTypeId]),
	CONSTRAINT [FK_EntityAttributeValuesHistory_Values] FOREIGN KEY ([ValueId]) REFERENCES [dbo].[Values] (ValueId)
);



