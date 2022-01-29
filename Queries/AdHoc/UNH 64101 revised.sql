DROP TABLE IF EXISTS #DATA


SELECT DISTINCT
	 DF_PRS_ID
INTO #DATA
FROM
(
SELECT DISTINCT
	DF_PRS_ID
FROM
	UDW..PD30_PRS_ADR PD30
	INNER JOIN UDW..LN10_LON LN10
		ON LN10.BF_SSN = PD30.DF_PRS_ID
WHERE
	DC_DOM_ST = 'WA'
	AND DC_ADR = 'L'
	AND DD_VER_ADR <= '12/31/2018'
	AND DI_VLD_ADR = 'Y'

UNION ALL

SELECT DISTINCT
	PD31.DF_PRS_ID

FROM
	UDW..PD31_PRS_INA PD31
	INNER JOIN UDW..LN10_LON LN10
		ON LN10.BF_SSN = PD31.DF_PRS_ID
	INNER JOIN
	(
		SELECT
			DF_PRS_ID,
			MAX(DN_ADR_SEQ) AS DN_ADR_SEQ
		FROM
			UDW..PD31_PRS_INA
		WHERE
			DD_VER_ADR_HST <= '12/31/2018'
			AND DC_ADR_HST = 'L'
		GROUP BY
			DF_PRS_ID
	) MPD31
		ON MPD31.DF_PRS_ID = PD31.DF_PRS_ID
		AND MPD31.DN_ADR_SEQ = PD31.DN_ADR_SEQ
WHERE
	DC_DOM_ST_HST = 'WA'
) POP

SELECT
	COUNT(DISTINCT BF_SSN) AS TotalBorrowers,
	SUM(LoanCount30+LoanCount60+LoanCount90+LoanCount91) AS TotalLoans,
	SUM(LoanCount30) AS LoanCount30,
	SUM(LoanCount60) AS LoanCount60,
	SUM(LoanCount90) AS LoanCount90,
	SUM(LoanCount91) AS LoanCount91,
	SUM(Balance30) AS BalanceTotal30,
	SUM(Balance60) AS BalanceTotal60,
	SUM(Balance90) AS BalanceTotal90,
	SUM(Balance91) AS BalanceTotal91,
	CAST(SUM(Balance30) / SUM(LoanCount30) AS DECIMAL(14,2)) AS Average30,
	CAST(SUM(Balance60) / SUM(LoanCount60) AS DECIMAL(14,2)) AS Average60,
	CAST(SUM(Balance90) / SUM(LoanCount90) AS DECIMAL(14,2)) AS Average90,
	CAST(SUM(Balance91) / SUM(LoanCount91) AS DECIMAL(14,2)) AS Average91
FROM
(
	SELECT DISTINCT
		BF_SSN,
		SUM(CASE WHEN PrinSub30 IS NOT NULL THEN 1 ELSE 0 END) AS LoanCount30,
		SUM(CASE WHEN PrinSub60 IS NOT NULL THEN 1 ELSE 0 END) AS LoanCount60,
		SUM(CASE WHEN PrinSub90 IS NOT NULL THEN 1 ELSE 0 END) AS LoanCount90,
		SUM(CASE WHEN PrinOver90 IS NOT NULL THEN 1 ELSE 0 END) AS LoanCount91,
		SUM(CASE WHEN PrinSub30 IS NOT NULL THEN PrinSub30 ELSE 0 END) AS Balance30,
		SUM(CASE WHEN PrinSub60 IS NOT NULL THEN PrinSub60 ELSE 0 END) AS Balance60,
		SUM(CASE WHEN PrinSub90 IS NOT NULL THEN PrinSub90 ELSE 0 END) AS Balance90,
		SUM(CASE WHEN PrinOver90 IS NOT NULL THEN PrinOver90 ELSE 0 END) AS Balance91
	FROM
	(
		SELECT DISTINCT
			BF_SSN,
			LN_SEQ,
			CASE WHEN Sub30 = 1 THEN LA_CUR_PRI ELSE NULL END AS PrinSub30,
			CASE WHEN Sub60 = 1 THEN LA_CUR_PRI ELSE NULL END AS PrinSub60,
			CASE WHEN Sub90 = 1 THEN LA_CUR_PRI ELSE NULL END AS PrinSub90,
			CASE WHEN Over90 = 1 THEN LA_CUR_PRI ELSE NULL END AS PrinOver90
		FROM
		(
			SELECT DISTINCT
				BF_SSN,
				LN_SEQ,
				LA_CUR_PRI,
				CASE WHEN DaysDelqAsOf < 30 THEN 1 ELSE NULL END AS Sub30,
				CASE WHEN DaysDelqAsOf >= 30 AND DaysDelqAsOf <= 60 THEN 1 ELSE NULL END AS Sub60,
				CASE WHEN DaysDelqAsOf > 60 AND DaysDelqAsOf <= 90 THEN 1 ELSE NULL END AS Sub90,
				CASE WHEN DaysDelqAsOf > 90 THEN 1 ELSE NULL END AS Over90
			FROM
			(
				SELECT DISTINCT
					LN10.BF_SSN,
					LN10.LN_SEQ,
					LN10.LA_CUR_PRI,
					CASE WHEN LN16.BF_SSN IS NULL THEN 0 WHEN CAST(LN16.LD_DLQ_MAX AS DATE) > '2019-09-30' THEN DATEDIFF(DAY, LN16.LD_DLQ_OCC, '2019-09-30') ELSE DATEDIFF(DAY, LN16.LD_DLQ_OCC, LN16.LD_DLQ_MAX) END AS DaysDelqAsOf
				FROM
					#DATA D
					INNER JOIN AuditUDW..LN10_LON_Sep2019 LN10
						ON LN10.BF_SSN = D.DF_PRS_ID
						AND LN10.LC_STA_LON10 = 'R'
						AND	LN10.LA_CUR_PRI > 0
					LEFT JOIN
					(
						SELECT DISTINCT
							Delq.BF_SSN,
							Delq.LN_SEQ,
							Delq.LD_DLQ_OCC,
							Delq.LD_DLQ_MAX
						FROM
							UDW..LN16_LON_DLQ_HST Delq
							INNER JOIN
							(
								SELECT DISTINCT
									BF_SSN,
									LN_SEQ,
									MAX(LN_DLQ_SEQ) AS LN_DLQ_SEQ
								FROM
									UDW..LN16_LON_DLQ_HST 
								WHERE
									CAST(LD_DLQ_OCC AS DATE) <= '2019-09-30'
									AND CAST(LD_DLQ_MAX AS DATE) >= '2019-09-01'
								GROUP BY
									BF_SSN,
									LN_SEQ
							) MaxOCC
								ON MaxOcc.LN_DLQ_SEQ = Delq.LN_DLQ_SEQ
								AND MaxOCC.BF_SSN = Delq.BF_SSN
								AND MaxOCC.LN_SEQ = Delq.LN_SEQ
					) LN16
						ON LN16.BF_SSN = LN10.BF_SSN
						AND LN16.LN_SEQ = LN10.LN_SEQ				
			) Summary
		) Grouped
	) Overall
	GROUP BY
		BF_SSN
)Totals