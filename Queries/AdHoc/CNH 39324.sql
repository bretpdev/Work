USE CDW;
GO

SELECT DISTINCT
	DFXX.BF_SSN,
	LTRIM(RTRIM(PDXX.DM_PRS_X)) + ' ' + LTRIM(RTRIM(PDXX.DM_PRS_LST)) AS [NAME],
	CAST(WQXX.WF_CRT_DTS_WQXX AS DATE) AS PROCESSED_DATE,
	WQXX.WF_USR_ASN_TSK AS PROCESSOR_UT#
FROM
	WQXX_TSK_QUE_HST WQXX
	INNER JOIN PDXX_PRS_NME PDXX
		ON PDXX.DF_PRS_ID = WQXX.BF_SSN	
	INNER JOIN LNXX_LON LNXX
		ON PDXX.DF_PRS_ID = LNXX.BF_SSN
	INNER JOIN LNXX_BR_DFR_APV LNXX
		ON LNXX.BF_SSN = LNXX.BF_SSN
		AND LNXX.LN_SEQ = LNXX.LN_SEQ
	INNER JOIN DFXX_BR_DFR_REQ DFXX
		ON LNXX.BF_SSN = DFXX.BF_SSN
		AND LNXX.LF_DFR_CTL_NUM = DFXX.LF_DFR_CTL_NUM
WHERE
	DFXX.LC_DFR_TYP IN (XX,XX)
	AND WQXX.WC_STA_WQUEXX = 'C' --completed
	AND CAST(WQXX.WF_CRT_DTS_WQXX AS DATE) >= CONVERT(DATE,'XXXXXXXX')
	AND WQXX.WF_USR_ASN_TSK LIKE 'UT%'
	AND DFXX.LC_STA_DFRXX = 'A'
	AND DFXX.LC_DFR_STA = 'A'
	AND LNXX.LC_STA_LONXX = 'A'
	AND LNXX.LA_CUR_PRI > X.XX
	AND LNXX.LC_STA_LONXX = 'R'
ORDER BY
	PROCESSED_DATE
;