CREATE PROCEDURE [dbo].[GetAllLetterTypeData]

AS
    SELECT
        [LetterTypeId],
        [LetterType]
    FROM
        [dbo].[LetterTypes]
RETURN 0
