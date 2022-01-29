
CREATE PROCEDURE [dbo].[CheckLoanPgm]
	@LoanPgm varchar(6)
AS
	SELECT
		direct
	FROM
		GENR_LST_Loan_Program
	WHERE
		loan_program = @LoanPgm
RETURN 0