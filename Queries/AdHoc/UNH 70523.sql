SELECT
	COUNT(DISTINCT DC01.BF_SSN) AS BORROWER_COUNT,
	COUNT(*) AS LOAN_COUNT,
	SUM(AA_CUR_PRI) AS TOTAL_PRINCIPAL
FROM
	ODW..DC01_LON_CLM_INF DC01
	INNER JOIN [ODW].[dbo].[PD03_PRS_ADR_PHN] PD03
		ON PD03.DF_PRS_ID = DC01.BF_SSN
		AND DC_DOM_ST = 'RI'
		AND DI_VLD_ADR = 'Y'
	INNER JOIN [ODW].[dbo].[GA10_LON_APP] GA10
		ON GA10.AF_APL_ID = DC01.AF_APL_ID
		AND GA10.AF_APL_ID_SFX = DC01.AF_APL_ID_SFX
	LEFT JOIN
	(
		SELECT DISTINCT
			DC11.AF_APL_ID,
			DC11.AF_APL_ID_SFX,
			DC11.LF_CRT_DTS_DC10,
			SUM(DC11.LA_TRX) OVER(PARTITION BY DC11.AF_APL_ID, DC11.AF_APL_ID_SFX, DC11.LF_CRT_DTS_DC10, DC11.LD_TRX_EFF) AS LA_TRX,
			DC11.LD_TRX_EFF
		FROM
			ODW..DC11_LON_FAT DC11
			INNER JOIN 
			(
				SELECT
					MaxDC11.AF_APL_ID,
					MaxDC11.AF_APL_ID_SFX,
					MaxDC11.LF_CRT_DTS_DC10,
					MAX(MaxDC11.LD_TRX_EFF) AS LD_TRX_EFF
				FROM
					ODW..DC11_LON_FAT MaxDC11
				WHERE
					MaxDC11.LC_REV_IND_TYP = ''
					AND MaxDC11.LC_TRX_TYP = 'BR'
				GROUP BY
					MaxDC11.AF_APL_ID,
					MaxDC11.AF_APL_ID_SFX,
					MaxDC11.LF_CRT_DTS_DC10
			) MaxDC11
				ON MaxDC11.AF_APL_ID = DC11.AF_APL_ID
				AND MaxDC11.AF_APL_ID_SFX = DC11.AF_APL_ID_SFX
				AND MaxDC11.LF_CRT_DTS_DC10 = DC11.LF_CRT_DTS_DC10
				AND MaxDC11.LD_TRX_EFF = DC11.LD_TRX_EFF
		WHERE
			DC11.LC_REV_IND_TYP = ''
			AND DC11.LC_TRX_TYP = 'BR'
	)DC11
		ON DC11.AF_APL_ID = DC01.AF_APL_ID
		AND DC11.AF_APL_ID_SFX = DC01.AF_APL_ID_SFX
		AND DC11.LF_CRT_DTS_DC10 = DC01.LF_CRT_DTS_DC10

WHERE
	DC01.LC_PCL_REA = 'DF'
	AND DC01.LC_STA_DC10 = '03'
	AND DC01.LD_CLM_ASN_DOE IS NULL
	AND LTRIM(ISNULL(DC01.LC_AUX_STA,'')) = ''
	AND ISNULL(DC01.LC_REA_CLM_ASN_DOE,'') = ''


SELECT
	COUNT(DISTINCT DC01.BF_SSN) AS BORROWER_COUNT,
	COUNT(*) AS LOAN_COUNT,
	SUM(AA_CUR_PRI) AS TOTAL_PRINCIPAL
FROM
	ODW..DC01_LON_CLM_INF DC01
	INNER JOIN [ODW].[dbo].[PD03_PRS_ADR_PHN] PD03
		ON PD03.DF_PRS_ID = DC01.BF_SSN
		AND DC_DOM_ST = 'RI'
		AND DI_VLD_ADR = 'Y'
	INNER JOIN [ODW].[dbo].[GA10_LON_APP] GA10
		ON GA10.AF_APL_ID = DC01.AF_APL_ID
		AND GA10.AF_APL_ID_SFX = DC01.AF_APL_ID_SFX
	LEFT JOIN
	(
		SELECT DISTINCT
			DC11.AF_APL_ID,
			DC11.AF_APL_ID_SFX,
			DC11.LF_CRT_DTS_DC10,
			SUM(DC11.LA_TRX) OVER(PARTITION BY DC11.AF_APL_ID, DC11.AF_APL_ID_SFX, DC11.LF_CRT_DTS_DC10, DC11.LD_TRX_EFF) AS LA_TRX,
			DC11.LD_TRX_EFF
		FROM
			ODW..DC11_LON_FAT DC11
			INNER JOIN 
			(
				SELECT
					MaxDC11.AF_APL_ID,
					MaxDC11.AF_APL_ID_SFX,
					MaxDC11.LF_CRT_DTS_DC10,
					MAX(MaxDC11.LD_TRX_EFF) AS LD_TRX_EFF
				FROM
					ODW..DC11_LON_FAT MaxDC11
				WHERE
					MaxDC11.LC_REV_IND_TYP = ''
					AND MaxDC11.LC_TRX_TYP = 'BR'
				GROUP BY
					MaxDC11.AF_APL_ID,
					MaxDC11.AF_APL_ID_SFX,
					MaxDC11.LF_CRT_DTS_DC10
			) MaxDC11
				ON MaxDC11.AF_APL_ID = DC11.AF_APL_ID
				AND MaxDC11.AF_APL_ID_SFX = DC11.AF_APL_ID_SFX
				AND MaxDC11.LF_CRT_DTS_DC10 = DC11.LF_CRT_DTS_DC10
				AND MaxDC11.LD_TRX_EFF = DC11.LD_TRX_EFF
		WHERE
			DC11.LC_REV_IND_TYP = ''
			AND DC11.LC_TRX_TYP = 'BR'
	)DC11
		ON DC11.AF_APL_ID = DC01.AF_APL_ID
		AND DC11.AF_APL_ID_SFX = DC01.AF_APL_ID_SFX
		AND DC11.LF_CRT_DTS_DC10 = DC01.LF_CRT_DTS_DC10

WHERE
	DC01.LC_PCL_REA = 'DF'
	AND DC01.LC_STA_DC10 = '03'
	AND DC01.LD_CLM_ASN_DOE IS NULL
	AND LTRIM(ISNULL(DC01.LC_AUX_STA,'')) = ''
	AND ISNULL(DC01.LC_REA_CLM_ASN_DOE,'') = ''
	and year(dc01.BD_VER_CLM_PAY) = '2020'

DROP TABLE IF EXISTS #COMPASS
SELECT
*
INTO #COMPASS
FROM
(
SELECT DISTINCT
	PD10.DF_PRS_ID,
	PD10.DF_SPE_ACC_ID,
	LN10.LN_SEQ,
	LN10.LA_CUR_PRI
FROM
	UDW..PD30_PRS_ADR PD30
	INNER JOIN UDW..PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = PD30.DF_PRS_ID
	INNER JOIN [AuditUDW].[dbo].[LN10_LON_Dec2020] LN10
		ON LN10.BF_SSN = PD30.DF_PRS_ID
WHERE
	DC_DOM_ST = 'RI'
	AND DC_ADR = 'L'
	AND DD_VER_ADR <= '12/31/2020'
	AND DI_VLD_ADR = 'Y'
	AND LN10.LA_CUR_PRI > 0
	AND LN10.LC_STA_LON10 = 'R'

UNION

SELECT DISTINCT
	PD31.DF_PRS_ID,
	PD10.DF_SPE_ACC_ID,
	LN10.LN_SEQ,
	LN10.LA_CUR_PRI
FROM
	UDW..PD31_PRS_INA PD31
	INNER JOIN UDW..PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = PD31.DF_PRS_ID
	INNER JOIN [AuditUDW].[dbo].[LN10_LON_Dec2020] LN10
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
	DC_DOM_ST_HST = 'RI'
	AND LN10.LA_CUR_PRI > 0
	AND LN10.LC_STA_LON10 = 'R'
) P

SELECT
COUNT(DISTINCT DF_PRS_ID) AS BORROWER_COUNT,
COUNT(*) AS LOAN_COUNT,
SUM(LA_CUR_PRI) AS TOTAL_PRINCIPAL
FROM
	#COMPASS

SELECT
	COUNT(DISTINCT DF_PRS_ID) AS BORROWER_COUNT,
	COUNT(*) AS LOAN_COUNT,
	SUM(LA_CUR_PRI) AS TOTAL_PRINCIPAL
FROM
	#COMPASS C
	INNER JOIN UDW..LN90_FIN_ATY LN90
		ON LN90.BF_SSN = C.DF_PRS_ID
		AND LN90.LN_SEQ = C.LN_SEQ
WHERE
	LN90.PC_FAT_TYP = '50'
	AND LN90.PC_FAT_SUB_TYP IN ('01','02','03')
	AND LN90.LC_STA_LON90 = 'A'
	AND ISNULL(LN90.LC_FAT_REV_REA, '') = ''

SELECT DISTINCT
	C.DF_SPE_ACC_ID
FROM
	#COMPASS C
	INNER JOIN UDW..LN90_FIN_ATY LN90
		ON LN90.BF_SSN = C.DF_PRS_ID
		AND LN90.LN_SEQ = C.LN_SEQ
WHERE
	LN90.PC_FAT_TYP = '50'
	AND LN90.PC_FAT_SUB_TYP IN ('01','02','03')
	AND LN90.LC_STA_LON90 = 'A'
	AND ISNULL(LN90.LC_FAT_REV_REA, '') = ''