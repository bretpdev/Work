CREATE TABLE [dbo].[EntityAttributeValues] (
    [EntityAttributeValueId] INT      IDENTITY (1, 1) NOT NULL,
    [EntityId]               INT      NOT NULL,
    [AttributeId]            INT      NOT NULL,
    [ValueId]                INT      NOT NULL,
    [CreatedAt]              DATETIME CONSTRAINT [DF_EntityAttributeValues_CreatedAt] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]              INT      NOT NULL,
    CONSTRAINT [PK_EntityAttributeValues] PRIMARY KEY CLUSTERED ([EntityAttributeValueId] ASC),
    CONSTRAINT [FK_EntityAttributeValues_Attributes] FOREIGN KEY ([AttributeId]) REFERENCES [dbo].[Attributes] ([AttributeId]),
    CONSTRAINT [FK_EntityAttributeValues_Entities] FOREIGN KEY ([EntityId]) REFERENCES [dbo].[Entities] ([EntityId]),
    CONSTRAINT [FK_EntityAttributeValues_Entities_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Entities] ([EntityId]), 
    CONSTRAINT [FK_EntityAttributeValues_Values] FOREIGN KEY ([ValueId]) REFERENCES [dbo].[Values]([ValueId]), 
    CONSTRAINT [CK_EntityAttributeValues_Column] CHECK (1=1)
    
);

GO

CREATE TRIGGER [dbo].[EntityAttributeValuesHistoryTrigger]
    ON [dbo].[EntityAttributeValues] 
    FOR UPDATE, INSERT, DELETE
    AS
    BEGIN
        
        DECLARE @INSERTED TABLE (InsertedId INT IDENTITY(1,1) , EntityAttributeValueId INT , EntityId INT ,AttributeId INT ,ValueId INT ,CreatedAt DATETIME,CreatedBy INT)
        INSERT INTO @INSERTED(EntityAttributeValueId  , EntityId  ,AttributeId  ,ValueId  ,CreatedAt ,CreatedBy)
        SELECT EntityAttributeValueId  , EntityId  ,AttributeId  ,ValueId  ,CreatedAt ,CreatedBy FROM inserted;

        DECLARE @DELETED TABLE (DeletedId INT IDENTITY(1,1), EntityAttributeValueId INT , EntityId INT ,AttributeId INT ,ValueId INT ,CreatedAt DATETIME,CreatedBy INT)
        INSERT INTO @DELETED(EntityAttributeValueId  , EntityId  ,AttributeId  ,ValueId  ,CreatedAt ,CreatedBy)
        SELECT EntityAttributeValueId  , EntityId  ,AttributeId  ,ValueId  ,CreatedAt ,CreatedBy FROM deleted;

        --UPDATE    
        INSERT INTO EntityAttributeValuesHistory([EntityAttributeValueId],[EntityId],[AttributeId],[ValueId],[CreatedAt],[CreatedBy],[HistoryStatusTypeId],[HistoryStatusCreatedBy])
        SELECT I.EntityAttributeValueId  , I.EntityId  ,I.AttributeId  ,I.ValueId  ,I.CreatedAt ,D.CreatedBy, 1 /*UPDATE*/, I.CreatedBy FROM @INSERTED I
            INNER JOIN  @DELETED D
                ON I.EntityAttributeValueId = D.EntityAttributeValueId
                AND I.EntityId = D.EntityId
                AND I.AttributeId = D.AttributeId
                AND I.ValueId = D.ValueId
                AND I.CreatedAt = D.CreatedAt
                AND I.CreatedBy = D.CreatedBy
            

            --INSERT
            INSERT INTO EntityAttributeValuesHistory([EntityAttributeValueId],[EntityId],[AttributeId],[ValueId],[CreatedAt],[CreatedBy],[HistoryStatusTypeId], [HistoryStatusCreatedBy])
            SELECT I.EntityAttributeValueId  , I.EntityId  ,I.AttributeId  ,I.ValueId  ,I.CreatedAt ,I.CreatedBy, 2 /*INSERT*/, I.CreatedBy FROM @INSERTED I
            LEFT JOIN  @DELETED D
                ON I.EntityAttributeValueId = D.EntityAttributeValueId
                AND I.EntityId = D.EntityId
                AND I.AttributeId = D.AttributeId
                AND I.ValueId = D.ValueId
                AND I.CreatedAt = D.CreatedAt
                AND I.CreatedBy = D.CreatedBy
            WHERE D.DeletedId IS NULL

            --DELETE
            INSERT INTO EntityAttributeValuesHistory([EntityAttributeValueId],[EntityId],[AttributeId],[ValueId],[CreatedAt],[CreatedBy],[HistoryStatusTypeId],[HistoryStatusCreatedBy])
            SELECT D.EntityAttributeValueId  , D.EntityId  ,D.AttributeId  ,D.ValueId  ,D.CreatedAt ,D.CreatedBy, 3 /*DELETE*/, D.CreatedBy FROM @DELETED D
            LEFT JOIN  @INSERTED I
                ON I.EntityAttributeValueId = D.EntityAttributeValueId
                AND I.EntityId = D.EntityId
                AND I.AttributeId = D.AttributeId
                AND I.ValueId = D.ValueId
                AND I.CreatedAt = D.CreatedAt
                AND I.CreatedBy = D.CreatedBy
            WHERE I.InsertedId IS NULL
                
    END
GO
