
CREATE PROCEDURE [scra].[GetBorrowerId]
	@BorrowerAccountNumber char(10)
AS
	SELECT
		BorrowerId
	FROM
		scra.Borrowers
	WHERE
		BorrowerAccountNumber = @BorrowerAccountNumber
RETURN 0