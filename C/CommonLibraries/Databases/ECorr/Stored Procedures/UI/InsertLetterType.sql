CREATE PROCEDURE [dbo].[InsertLetterType]
    @LetterType varchar(50)
AS

    INSERT INTO [dbo].[LetterTypes]([LetterType])
    VALUES(@LetterType)

RETURN 0
