USE CDW
GO

SELECT DISTINCT
	LNXX.BF_SSN AS SSN,
	RTRIM(PDXX.DM_PRS_LST) AS LAST_NAME,
	RTRIM(PDXX.DM_PRS_X) AS FIRST_NAME,
	(FSXX.LF_FED_AWD + RIGHT('XXX' + CAST(FSXX.LN_FED_AWD_SEQ AS VARCHAR(XX)), X)) as LOAN_ID,
	CASE 
		WHEN LNXX.LC_FED_PGM_YR = 'DLO' THEN 'PSAOO'
		WHEN LNXX.LC_FED_PGM_YR = 'LNC' THEN 'PSAOP'
		ELSE NULL
	END AS LOAN_PROGRAM_TYPE,
	GRXX.WD_LON_GTR AS GUARANTY_DATE
FROM
	CDW..LNXX_FIN_ATY LNXX
	INNER JOIN CDW..LNXX_LON LNXX
		ON LNXX.BF_SSN = LNXX.BF_SSN
		AND LNXX.LN_SEQ = LNXX.LN_SEQ
	INNER JOIN CDW..GRXX_RPT_LON_APL GRXX
		ON GRXX.BF_SSN = LNXX.BF_SSN
		AND GRXX.LN_SEQ = LNXX.LN_SEQ
	INNER JOIN CDW..FSXX_DL_LON FSXX
		ON FSXX.BF_SSN = LNXX.BF_SSN
		AND FSXX.LN_SEQ = LNXX.LN_SEQ
	INNER JOIN CDW..PDXX_PRS_NME PDXX
		ON PDXX.DF_PRS_ID = LNXX.BF_SSN
	INNER JOIN 
	(
		SELECT	
			BF_SSN,
			SUM(LA_CUR_PRI) AS LA_CUR_PRI
		FROM
			CDW..LNXX_LON
		GROUP BY
			BF_SSN
		HAVING
			SUM(LA_CUR_PRI) <= X
	)LNXXB
		ON LNXXB.BF_SSN = LNXX.BF_SSN
WHERE
	(LNXX.PC_FAT_TYP = 'XX' AND LNXX.PC_FAT_SUB_TYP = 'XX')
	OR
	(LNXX.PC_FAT_TYP = 'XX' AND LNXX.PC_FAT_SUB_TYP = 'XX')
	OR 
	(LNXX.PC_FAT_TYP = 'XX' AND LNXX.PC_FAT_SUB_TYP = 'XX')
	AND 
	LNXX.LC_STA_LONXX = 'A'
	AND ISNULL(LNXX.LC_FAT_REV_REA,'') = ''
ORDER BY 
	LNXX.BF_SSN