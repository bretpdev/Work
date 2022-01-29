DROP TABLE IF EXISTS #DATA


SELECT DISTINCT
	 DF_PRS_ID,
	 DF_SPE_ACC_ID
INTO #DATA
FROM
(
SELECT DISTINCT
	PD10.DF_PRS_ID,
	PD10.DF_SPE_ACC_ID
FROM
	UDW..PD30_PRS_ADR PD30
	INNER JOIN UDW..PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = PD30.DF_PRS_ID
	INNER JOIN UDW..LN10_LON LN10
		ON LN10.BF_SSN = PD30.DF_PRS_ID
WHERE
	DC_DOM_ST = 'WA'
	AND DC_ADR = 'L'
	AND DD_VER_ADR <= '12/31/2020'
	AND DI_VLD_ADR = 'Y'

UNION ALL

SELECT DISTINCT
	PD31.DF_PRS_ID,
	PD10.DF_SPE_ACC_ID
FROM
	UDW..PD31_PRS_INA PD31
	INNER JOIN UDW..PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = PD31.DF_PRS_ID
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
			DD_VER_ADR_HST <= '12/31/2020'
			AND DC_ADR_HST = 'L'
		GROUP BY
			DF_PRS_ID
	) MPD31
		ON MPD31.DF_PRS_ID = PD31.DF_PRS_ID
		AND MPD31.DN_ADR_SEQ = PD31.DN_ADR_SEQ
WHERE
	DC_DOM_ST_HST = 'WA'
) POP

--SELECT * FROM #DATA

SELECT
	DF_SPE_ACC_ID AS ACCOUNT_NUMBER,
	LN10.LN_SEQ
FROM
	#DATA D
	INNER JOIN UDW..LN10_LON LN10
		ON D.DF_PRS_ID = LN10.BF_SSN
WHERE
	LN10.LA_CUR_PRI > 0
	AND LN10.LC_STA_LON10 = 'R'
SELECT 
	DF_SPE_ACC_ID AS ACCOUNT_NUMBER,
	LN10.LN_SEQ,
	LN10.LD_LON_EFF_ADD
FROM #DATA D
INNER JOIN UDW..LN10_LON LN10
	ON D.DF_PRS_ID = LN10.BF_SSN
WHERE

	LN10.LA_CUR_PRI > 0
	AND LN10.LC_STA_LON10 = 'R'
	AND	YEAR(LN10.LD_LON_EFF_ADD) = 2020
	