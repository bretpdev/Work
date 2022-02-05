CREATE PROCEDURE [dbo].[GetBorrowersLoanPgmForArcAdd]
	@AccountNumber varchar(10)
	
AS
	SELECT DISTINCT
		IC_LON_PGM
	FROM	
		LN10_Loan
	WHERE 
		DF_SPE_ACC_ID = @AccountNumber
RETURN 0