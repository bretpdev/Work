CREATE PROCEDURE [nsldsconso].[UpdateBorrowerLoans]
	@BorrowerLoanUpdates BorrowerUnderlyingLoansUpdateTT readonly
AS
	
	UPDATE
		bul
	SET
		bul.NewLoanId = blu.NewLoanId,
		bul.NsldsLabel = blu.NsldsLabel
	FROM
		nsldsconso.BorrowerUnderlyingLoans bul
	INNER JOIN
		@BorrowerLoanUpdates blu on blu.BorrowerUnderlyingLoanId = bul.BorrowerUnderlyingLoanId

RETURN 0
