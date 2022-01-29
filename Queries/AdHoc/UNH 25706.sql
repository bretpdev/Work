
DECLARE @BanaLoan TABLE(LoanType VARCHAR(10))
		INSERT INTO @BanaLoan VALUES 
		('829769-01'), ('829769-02'), ('829769-03'), ('829769-04'), 
		('829769-05'), ('829769-06'), ('829769-07'), ('829769-08') --BANA lenders

SELECT DISTINCT
	LN10.BF_SSN 'Borrower SSN',
	LN10.LN_SEQ 'Loan Sequence',
	LN10.LI_BR_DET_RPD_XTN 'Old LI_BR_DET_RPD_XTN',
	'Y' [New LI_BR_DET_RPD_XTN],
	SUM(LN15.LA_DSB)-SUM(LN15.LA_DSB_CAN) 'SUM' --REMOVE FROM FINAL QUERY
FROM
	UDW.[dbo].[LN10_LON] LN10
	INNER JOIN UDW.[dbo].[PD10_Borrower] PD10
		ON LN10.BF_SSN = PD10.BF_SSN
	LEFT OUTER JOIN UDW.[dbo].[LN15_Disbursement] LN15
		ON PD10.DF_SPE_ACC_ID = LN15.DF_SPE_ACC_ID
WHERE
	ISNULL(LN10.LI_BR_DET_RPD_XTN, '') = '' --Sufficient Debt Indicator
	AND LN10.LF_LON_CUR_OWN IN 
		(
		SELECT LoanType 
		FROM @BanaLoan
		)
GROUP BY
	LN10.BF_SSN,
	LN10.LN_SEQ,
	LN10.LI_BR_DET_RPD_XTN
HAVING
	SUM(LN15.LA_DSB)-SUM(LN15.LA_DSB_CAN)  > 30000.00
ORDER BY
	LN10.BF_SSN





