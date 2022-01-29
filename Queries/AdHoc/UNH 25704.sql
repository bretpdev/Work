
DECLARE @BanaLoan VARCHAR(10) = '829769', --loans from BANA conversion
		@ErrorCode VARCHAR(10) ='04084' --designated error code

SELECT
	LN10.BF_SSN 'Borrower_SSN',
	LN10.LN_SEQ 'Loan_Seq',
	LN10.LD_MPN_EXP 'OLD LD_MPN_EXP',
	DCER.NoteDate 'New LD_MPN_EXP'
FROM
	UDW.[dbo].[LN10_LON] LN10
	INNER JOIN EA27_BANA_2.[dbo].[Borrower_Errors] BE
		ON LN10.BF_SSN = BE.br_ssn
	INNER JOIN EA27_BANA_2.[dbo].[_07_08DisbClaimEnrollRecord] DCER
		ON DCER.BorrowerSSN = BE.br_ssn
WHERE
	ISNULL(LN10.LD_MPN_EXP, '') = ''
	AND BE.error_code = @ErrorCode
	AND LN10.LF_LON_CUR_OWN = @BanaLoan


