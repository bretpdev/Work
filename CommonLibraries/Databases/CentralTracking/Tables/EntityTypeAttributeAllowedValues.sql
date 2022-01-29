CREATE TABLE [dbo].[EntityTypeAttributeAllowedValues]
(
	[EntityTypeAttributeAllowedValuesId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [EntityTypeAttributeId] INT NOT NULL, 
    [ValueId] INT NOT NULL, 
    [CreatedAt] DATETIME NOT NULL DEFAULT (getdate()), 
    [CreatedBy] INT NOT NULL
)

GO

CREATE TRIGGER [dbo].[EntityTypeAttributeAllowedValuesHistoryTrigger]
    ON [dbo].[EntityTypeAttributeAllowedValues] 
    FOR UPDATE, INSERT, DELETE
    AS
    BEGIN
        
        DECLARE @INSERTED TABLE (InsertedId INT IDENTITY(1,1) , EntityTypeAttributeId INT , ValueId INT ,CreatedAt DATETIME,CreatedBy INT)
        INSERT INTO @INSERTED(EntityTypeAttributeId, ValueId, CreatedAt, CreatedBy)
        SELECT EntityTypeAttributeId, ValueId, CreatedAt, CreatedBy FROM inserted;

        DECLARE @DELETED TABLE (DeletedId INT IDENTITY(1,1),  EntityTypeAttributeId INT , ValueId INT ,CreatedAt DATETIME,CreatedBy INT)
        INSERT INTO @DELETED(EntityTypeAttributeId, ValueId, CreatedAt, CreatedBy)
        SELECT EntityTypeAttributeId, ValueId, CreatedAt, CreatedBy FROM deleted;

        --UPDATE    
        INSERT INTO EntityTypeAttributeAllowedValuesHistory([EntityTypeAttributeId],[ValueId],[CreatedAt],[CreatedBy],[HistoryStatusTypeId],[HistoryStatusCreatedBy])
        SELECT I.EntityTypeAttributeId, I.ValueId, I.CreatedAt, I.CreatedBy, 1 /*UPDATE*/, D.CreatedBy FROM @INSERTED I
            INNER JOIN  @DELETED D
                ON I.EntityTypeAttributeId = D.EntityTypeAttributeId
                AND I.ValueId = D.ValueId
                AND I.CreatedAt = D.CreatedAt
                AND I.CreatedBy = D.CreatedBy
            

            --INSERT
            INSERT INTO EntityTypeAttributeAllowedValuesHistory([EntityTypeAttributeId],[ValueId],[CreatedAt],[CreatedBy],[HistoryStatusTypeId],[HistoryStatusCreatedBy])
           SELECT I.EntityTypeAttributeId, I.ValueId, I.CreatedAt, I.CreatedBy, 2 /*INSERT*/, I.CreatedBy FROM @INSERTED I
            LEFT JOIN  @DELETED D
                ON I.EntityTypeAttributeId = D.EntityTypeAttributeId
                AND I.ValueId = D.ValueId
                AND I.CreatedAt = D.CreatedAt
                AND I.CreatedBy = D.CreatedBy
            WHERE D.DeletedId IS NULL

            --DELETE
            INSERT INTO EntityTypeAttributeAllowedValuesHistory([EntityTypeAttributeId],[ValueId],[CreatedAt],[CreatedBy],[HistoryStatusTypeId],[HistoryStatusCreatedBy])
            SELECT D.EntityTypeAttributeId, D.ValueId, D.CreatedAt, D.CreatedBy, 3 /*DELETE*/, D.CreatedBy FROM @DELETED D
            LEFT JOIN  @INSERTED I
                ON I.EntityTypeAttributeId = D.EntityTypeAttributeId
                AND I.ValueId = D.ValueId
                AND I.CreatedAt = D.CreatedAt
                AND I.CreatedBy = D.CreatedBy
            WHERE I.InsertedId IS NULL
                
    END
GO

