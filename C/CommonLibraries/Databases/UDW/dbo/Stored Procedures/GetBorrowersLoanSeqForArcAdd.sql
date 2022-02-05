CREATE PROCEDURE [dbo].[GetBorrowersLoanSeqForArcAdd]
	@AccountNumber varchar(10)
	
AS
	SELECT DISTINCT
		cast(LN_SEQ as varchar(4))
	FROM	
		LN10_Loan
	WHERE 
		DF_SPE_ACC_ID = @AccountNumber
RETURN 0