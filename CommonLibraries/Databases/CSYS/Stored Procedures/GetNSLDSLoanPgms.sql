CREATE PROCEDURE [dbo].[GetNSLDSLoanPgms]

AS
	SELECT 
		isnull(NSLDSLoanCode, '') as NSLDSLoanCode,
		loan_program as LoanPgm
	FROM
		dbo.GENR_LST_Loan_Program
RETURN 0
