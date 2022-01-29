USE UDW
GO

SELECT DISTINCT
	PD10.DF_SPE_ACC_ID AS AccountNumber,
	RTRIM(PD10.DM_PRS_1) AS FirstName,
	RTRIM(PD10.DM_PRS_LST) AS LastName,
	LN16.LN_SEQ AS LoanSeq,
	LN16.LN_DLQ_MAX AS DaysPastDue
FROM
	UDW..WQ20_TSK_QUE WQ20
	INNER JOIN UDW..PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = WQ20.BF_SSN
	INNER JOIN UDW..LN10_LON LN10
		ON WQ20.BF_SSN = LN10.BF_SSN
	INNER JOIN 
	(
		SELECT
			LN16.BF_SSN,
			LN16.LN_SEQ,
			LN16.LD_DLQ_OCC,
			LN16.LN_DLQ_MAX + 1 AS LN_DLQ_MAX
		FROM
			UDW..LN16_LON_DLQ_HST LN16
			INNER JOIN
			(
				SELECT
					MinOcc.BF_SSN,
					MinOcc.LN_SEQ,
					MIN(MinOcc.LD_DLQ_OCC) OVER(PARTITION BY MinOcc.BF_SSN) AS MinOccurance,
					MaxDelq.MaxDelq
				FROM
					UDW..LN16_LON_DLQ_HST MinOcc
					INNER JOIN
					(
						SELECT
							BF_SSN,
							LN_SEQ,
							MAX(LD_DLQ_MAX) AS MaxDelq
						FROM
							UDW..LN16_LON_DLQ_HST
						WHERE
							LC_STA_LON16 = '1'
						GROUP BY
							BF_SSN,
							LN_SEQ			
					) MaxDelq
						ON MinOcc.BF_SSN = MaxDelq.BF_SSN
						AND MinOcc.LN_SEQ = MaxDelq.LN_SEQ
						AND MinOcc.LD_DLQ_MAX = MaxDelq.MaxDelq
			) EarliestOcc
				ON EarliestOcc.BF_SSN = LN16.BF_SSN
				AND EarliestOcc.LN_SEQ = LN16.LN_SEQ
				AND EarliestOcc.MaxDelq = LN16.LD_DLQ_MAX
				AND EarliestOcc.MinOccurance = LN16.LD_DLQ_OCC
	) LN16
		ON LN10.BF_SSN = LN16.BF_SSN
		AND LN10.LN_SEQ = LN16.LN_SEQ
WHERE
	WQ20.WF_QUE = 'RE'
	AND LN10.LA_CUR_PRI > 0.00
	AND LN10.LC_STA_LON10 = 'R'
ORDER BY 
	AccountNumber,
	LoanSeq