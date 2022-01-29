CREATE PROCEDURE [dbo].[GetBorrowerPNote]
	@br_ssn varchar(9)
AS
	SELECT
		loan.br_ssn AS [SSN],
		dbo.CONVERT_DATE(loan.ln_1st_disb_date) AS [DisbursementDate],
		dbo.CONVERT_DATE(loan.ln_promissory_note_date) AS [SignedDate],
		map.CommpassLoanSeq AS [LoanSequence]
	FROM
		ITELSQLDF_Loan loan
		JOIN CompassLoanMapping map
			ON loan.br_ssn = map.br_ssn
			AND loan.ln_num = map.NelNetLoanSeq
	WHERE
		loan.br_ssn = @br_ssn
RETURN 0