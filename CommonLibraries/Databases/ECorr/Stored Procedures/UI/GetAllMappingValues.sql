CREATE PROCEDURE [dbo].[GetAllMappingValues]

AS
    SELECT 
        [TagAttributeValueMappingId],
        MAP.[TagId],
        TAG.[Tag],
        MAP.[TagAttributeValueId],
        TAV.[Attribute]
    FROM
        [dbo].[TagAttributeValueMapping] MAP
    INNER JOIN [dbo].[TagAttributeValues] TAV
        ON TAV.TagAttributeValueId = MAP.TagAttributeValueId
    INNER JOIN [dbo].[Tags] TAG
        ON TAG.TagId = MAP.TagId
RETURN 0
