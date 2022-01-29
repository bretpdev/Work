CREATE PROCEDURE [dbo].[GetBorrowersAltFormat]
	@AccountNumber VARCHAR(10)
AS
	SELECT COALESCE((
					SELECT 
						CorrespondenceFormatId
					FROM
						BorrowerCorrespondenceFormat
					WHERE
						DF_SPE_ACC_ID = @AccountNumber
					), 1)  AS CorrespondenceFormatId
RETURN 0