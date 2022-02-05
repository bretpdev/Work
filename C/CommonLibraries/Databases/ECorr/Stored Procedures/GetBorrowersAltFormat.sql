CREATE PROCEDURE [dbo].[GetBorrowersAltFormat]
	@AccountNumber VARCHAR(10)
AS
	SELECT COALESCE((
					SELECT 
						CorrespondenceFormatId
					FROM
						BorrowerCorrespondenceFormats
					WHERE
						AccountNumber = @AccountNumber
					), 1)  AS CorrespondenceFormatId
RETURN 0
