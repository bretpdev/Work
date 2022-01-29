
USE CDW
GO


SELECT DISTINCT
	PDXX.DF_SPE_ACC_ID,
	LNXX.LN_SEQ,
	DWXX.WC_DW_LON_STA
FROM
	CDW..LNXX_LON LNXX
	INNER JOIN CDW..PDXX_PRS_NME PDXX
		ON PDXX.DF_PRS_ID = LNXX.BF_SSN
	INNER JOIN CDW..DWXX_DW_CLC_CLU DWXX
		ON DWXX.BF_SSN = LNXX.BF_SSN
		AND DWXX.LN_SEQ = LNXX.LN_SEQ
	INNER JOIN
	(
		SELECT
			LNXX.BF_SSN,
			CAST(LNXX.LN_SEQ AS VARCHAR(X)) AS LN_SEQ
		FROM
			CDW..LNXX_LON LNXX
			INNER JOIN CDW..DWXX_DW_CLC_CLU DWXX
				ON DWXX.BF_SSN = LNXX.BF_SSN
				AND DWXX.LN_SEQ = LNXX.LN_SEQ 
			INNER JOIN CDW..PDXX_PRS_BKR PDXX
				ON PDXX.DF_PRS_ID = LNXX.BF_SSN
		WHERE
			LNXX.LA_CUR_PRI > X.XX 
			AND LNXX.LC_STA_LONXX = 'R'
			AND DWXX.WC_DW_LON_STA = 'XX'
			AND PDXX.DC_BKR_STA = 'XX'
	) BKR
		ON BKR.BF_SSN = LNXX.BF_SSN
WHERE
	LNXX.LA_CUR_PRI > X.XX 
	AND LNXX.LC_STA_LONXX = 'R'
	AND DWXX.WC_DW_LON_STA != 'XX'