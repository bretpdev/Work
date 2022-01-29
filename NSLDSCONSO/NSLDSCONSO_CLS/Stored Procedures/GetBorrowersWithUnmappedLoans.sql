CREATE PROCEDURE [nsldsconso].[GetBorrowersWithUnmappedLoans]
AS

	SELECT DISTINCT
		B.Ssn,
		B.BorrowerId,
		B.Name
	FROM
		nsldsconso.Borrowers B
	INNER JOIN
		nsldsconso.BorrowerUnderlyingLoans BUL on BUL.BorrowerId = B.BorrowerId
	WHERE
		BUL.NsldsLabel IS NULL 
		OR
		BUL.NewLoanId IS NULL

RETURN 0
