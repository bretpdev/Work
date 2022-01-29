--This view is created to allow a unique constraint to be created on EntityAttributeValue which will be applied except for when the AttributeSelectionType is MultipleAnswer
CREATE VIEW [dbo].[EntityAttributeValueUnqiueConstraint] WITH SCHEMABINDING
    AS SELECT EntityId, eav.AttributeId FROM [dbo].[EntityAttributeValues] eav
    INNER JOIN [dbo].[Attributes] att
        on att.AttributeId = eav.AttributeId
    WHERE AttributeSelectionTypeId != 3 --Multiple Answer
    GO

    CREATE UNIQUE CLUSTERED INDEX CIX_EntityAttributeValue ON [dbo].[EntityAttributeValueUnqiueConstraint] (EntityId, AttributeId)
    GO
