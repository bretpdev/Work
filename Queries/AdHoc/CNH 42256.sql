USE CDW
GO
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT
	*
FROM OPENQUERY(LEGEND,
'
SELECT 
	LNXX.BF_SSN,
	LNXX.LN_SEQ,
	LNXX.LD_DSB AS LAST_DSB_DATE,
	LNXX.LA_DSB,
	LNXX.LA_DSB_CAN,
	LNXX.TOTAL_DSB,
	LNXX.LA_CUR_PRI AS CURRENT_PRINCIPAL,
	GRXX.WD_NDS_LN_DSB_RPT,
	LNXX.LD_FAT_PST
FROM
	PKUB.LNXX_LON LNXX
	INNER JOIN PKUB.GRXX_RPT_LON_APL GRXX
		ON GRXX.BF_SSN = LNXX.BF_SSN
		AND GRXX.LN_SEQ = LNXX.LN_SEQ
	INNER JOIN 
	(
		SELECT	
			LNXX.BF_SSN,
			LNXX.LN_SEQ,
			SUM(LA_DSB) AS LA_DSB,
			SUM(COALESCE(LA_DSB_CAN, X)) AS LA_DSB_CAN,
			MAX(LD_DSB) AS LD_DSB,
			SUM(LA_DSB - COALESCE(LA_DSB_CAN, X)) AS TOTAL_DSB
		FROM
			PKUB.LNXX_DSB LNXX
		GROUP BY
			LNXX.BF_SSN,
			LNXX.LN_SEQ
		
	) LNXX
		ON LNXX.BF_SSN = LNXX.BF_SSN
		AND LNXX.LN_SEQ = LNXX.LN_SEQ
	LEFT JOIN
	(
		SELECT
			BF_SSN,
			LN_SEQ,
			MAX(LD_FAT_PST) AS LD_FAT_PST
		FROM
			PKUB.LNXX_FIN_ATY LNXX
		WHERE
			LNXX.PC_FAT_TYP = ''XX''
			AND LNXX.PC_FAT_SUB_TYP = ''XX''
			AND LNXX.LC_STA_LONXX = ''A''
			AND COALESCE(LNXX.LC_FAT_REV_REA,'''') = ''''
		GROUP BY
			BF_SSN,
			LN_SEQ
		
	) LNXX
		ON LNXX.BF_SSN = LNXX.BF_SSN
		AND LNXX.LN_SEQ = LNXX.LN_SEQ

WHERE
	LNXX.LA_CUR_PRI > X.XX
	AND (LNXX.LA_CUR_PRI > ((LNXX.TOTAL_DSB * .X) + LNXX.TOTAL_DSB))
	AND YEAR(LNXX.LD_DSB) BETWEEN XXXX AND XXXX
ORDER BY
	LNXX.BF_SSN,
	LNXX.LN_SEQ
')