CREATE PROCEDURE [dbo].[GetBorrowerIdFromSSN]
	@SSN CHAR(9)
AS
	
	SELECT
		borrower_id AS BorrowerId 
	FROM
		[dbo].Borrowers
	WHERE
		SSN = @SSN
RETURN 0