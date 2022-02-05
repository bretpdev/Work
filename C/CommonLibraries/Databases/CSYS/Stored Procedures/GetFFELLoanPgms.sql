CREATE PROCEDURE [dbo].[GetFFELLoanPgms]
	
AS
	SELECT 
		loan_program
	FROM
		[GENR_LST_Loan_Program]
	WHERE
		direct = 0
RETURN 0
