CREATE FUNCTION [dbo].[GetAttributeEntitySelectioTypeId]
(
    @attributeId int
)
RETURNS INT
AS
BEGIN
    DECLARE @id INT =
     (SELECT 
        AttributeSelectionTypeId
    FROM 
        Attributes att
    WHERE 
        AttributeId = @attributeId)

    RETURN @id
END 
