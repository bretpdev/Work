CREATE PROCEDURE [dbo].[GetAttributeValues]

AS
    SELECT
        [TagAttributeValueId],
        [Attribute],
        [Value]
    FROM
        [dbo].[TagAttributeValues]
RETURN 0
