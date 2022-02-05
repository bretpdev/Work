CREATE PROCEDURE [dbo].[UpdateLetterTypes]
    
@LetterTypeId int,
@LetterType varchar(50)

AS
    
    UPDATE
        [dbo].[LetterTypes]
    SET
        [LetterType] = @LetterType
    WHERE
        [LetterTypeId] = @LetterTypeId



RETURN 0
