CREATE PROCEDURE [dbo].[GetLoanPgmType]
	@LoanPgm varchar(6)
AS
	SELECT 
		LP.direct
	FROM
		GENR_LST_Loan_Program LP
	WHERE
		LP.loan_program = @LoanPgm

RETURN 0
