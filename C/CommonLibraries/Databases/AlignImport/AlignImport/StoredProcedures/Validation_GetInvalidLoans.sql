CREATE PROCEDURE [dbo].[Validation_GetInvalidLoans]
AS
	SELECT
		loan.br_ssn as Ssn, 
		MAP.CommpassLoanSeq as Num
	FROM 
		ITELSQLDF_Loan loan
	INNER JOIN [dbo].[CompassLoanMapping] MAP
		ON MAP.br_ssn = loan.br_ssn
		AND MAP.NelNetLoanSeq = loan.ln_num
	WHERE 
		replace(loan.ln_curr_principal, '0', '') = ''
RETURN 0
