CREATE PROCEDURE [dbo].[UpdateAttributes]
   
   @TagAttributeValueId int,
   @Attribute varchar(250),
   @Value varchar(250)

AS
    
    UPDATE
        [dbo].[TagAttributeValues]
    SET
        [Attribute] = @Attribute,
        [Value] = @Value
    WHERE
        [TagAttributeValueId] = @TagAttributeValueId

RETURN 0
