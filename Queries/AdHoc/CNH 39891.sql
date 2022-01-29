USE CDW
GO

SELECT 
	LTRIM(RTRIM(PDXX.DM_PRS_X)) + ' ' +  LTRIM(RTRIM(PDXX.DM_PRS_LST)) AS [NAME],
	PDXX.DF_SPE_ACC_ID,
	LNXX.LN_SEQ,
	LNXX.IC_LON_PGM,
	LNXX.LD_DSB,
	LNXX.LA_DSB,
	CASE WHEN AYXX.BF_SSN IS NULL THEN 'NO' ELSE 'YES' END AS HAS_DICSK_ARC
	--LNXX.*
FROM
	CDW..LNXX_LON LNXX
	INNER JOIN CDW..PDXX_PRS_NME PDXX
		ON PDXX.DF_PRS_ID = LNXX.BF_SSN
	INNER JOIN CDW..LNXX_DSB LNXX
		ON LNXX.BF_SSN = LNXX.BF_SSN
		AND LNXX.LN_SEQ = LNXX.LN_SEQ
	LEFT JOIN CDW..AYXX_BR_LON_ATY AYXX
		ON AYXX.BF_SSN = LNXX.BF_SSN
		AND AYXX.LC_STA_ACTYXX = 'A'
		AND AYXX.PF_REQ_ACT = 'DICSK'
WHERE
	LNXX.LF_DOE_SCL_ORG = 'XXXXXXXX'
	AND LNXX.LD_DSB > 'XX/XX/XXXX'
	AND YEAR(LNXX.LD_DSB) = XXXX
	AND ISNULL(LA_DSB_CAN,X) - LNXX.LA_DSB != X
ORDER BY [NAME]