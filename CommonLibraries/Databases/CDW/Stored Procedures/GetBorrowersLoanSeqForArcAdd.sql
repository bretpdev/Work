CREATE PROCEDURE [dbo].[GetBorrowersLoanSeqForArcAdd]
	@AccountNumber varchar(10)
	
AS
	SELECT DISTINCT
		LN_SEQ
	FROM	
		LN10_Loan
	WHERE 
		DF_SPE_ACC_ID = @AccountNumber
RETURN 0
