CREATE PROCEDURE [dbo].[GetBorrowersLoansForArcAdd]
	@LoanSeq bit,
	@AccountNumber varchar(10)
	
AS
	SELECT
		CASE
			WHEN @LoanSeq = 1 THEN LN_SEQ
			ELSE IC_LON_PGM
		END AS LOAN

	FROM	
		LN10_Loan
	WHERE DF_SPE_ACC_ID = @AccountNumber
RETURN 0