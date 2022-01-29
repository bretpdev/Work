CREATE PROCEDURE [pridrcrp].[CalculateLoanSequence]
(
	@LoanSequenceMappingRecord LoanSequenceMappingRecord READONLY
)
AS

SELECT DISTINCT
	T.Ssn,
	T.DisbursementDate,
	T.DisbursementAmount,
	T.InterestRate,
	T.GuaranteeDate,
	T.LoanNum,
	LN15.LN_SEQ AS LoanSequence
FROM
	@LoanSequenceMappingRecord T
	INNER JOIN
	(
		SELECT DISTINCT
			LSMR.Ssn,
			LSMR.LoanNum,
			LSMR.DisbursementDate,
			SUM(LSMR.DisbursementAmount) AS DSB_AMT
		FROM
			@LoanSequenceMappingRecord LSMR
		GROUP BY
			LSMR.Ssn,
			LSMR.LoanNum,
			LSMR.DisbursementDate
	) DataFileRows
		ON T.Ssn = DataFileRows.Ssn
		AND T.LoanNum = DataFileRows.LoanNum
		AND CAST(T.DisbursementDate AS DATE) = CAST(DataFileRows.DisbursementDate AS DATE)
	LEFT JOIN 
	(
		SELECT
			BF_SSN,
			LN_SEQ,
			CAST(MIN(LD_DSB) AS DATE) AS LD_DSB,
			SUM(LA_DSB - (ISNULL(LA_DSB_CAN,0))) AS DSB_AMT
		FROM
			CDW..LN15_DSB
		GROUP BY
			BF_SSN,
			LN_SEQ
	) LN15
		ON LN15.BF_SSN = DataFileRows.Ssn
		AND LN15.LD_DSB = CAST(DataFileRows.DisbursementDate AS DATE)
		AND LN15.DSB_AMT = DataFileRows.DSB_AMT
GO

