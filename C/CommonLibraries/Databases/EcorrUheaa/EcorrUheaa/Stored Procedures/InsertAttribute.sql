CREATE PROCEDURE [dbo].[InsertAttribute]

   @Attribute varchar(250),
   @Value varchar(250)

AS

    INSERT INTO [dbo].[TagAttributeValues]([Attribute],[Value])
    VALUES(@Attribute, @Value)

RETURN 0
