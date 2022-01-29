USE CDW
GO

SELECT
	PDXX.DF_SPE_ACC_ID
	,LNXX.LN_SEQ
	,DFXX.LC_DFR_TYP
	,LNXX.LC_STA_LONXX
	,DFXX.LC_DFR_STA
	,LNXX.LC_DFR_RSP
	--,DFXX.LF_DFR_CTL_NUM
	--,LNXX.LA_CUR_PRI
	--,LNXX.LC_STA_LONXX
	,LNXX.LD_DFR_BEG
	,LNXX.LD_DFR_END
	--,DATEDIFF(DAY, LNXX.LD_DFR_BEG, LNXX.LD_DFR_END) [DAYS]
	,CAST(CAST(DATEDIFF(DAY, LNXX.LD_DFR_BEG, LNXX.LD_DFR_END) AS DECIMAL(XX,X))/XXX*XX AS DECIMAL(XX,X)) [MONTHS],
	DATEDIFF(MONTH, LNXX.LD_DFR_BEG, LNXX.LD_DFR_END)
FROM
	LNXX_LON LNXX
	INNER JOIN PDXX_PRS_NME PDXX
		ON LNXX.BF_SSN = PDXX.DF_PRS_ID
	INNER JOIN LNXX_BR_DFR_APV LNXX
		ON LNXX.BF_SSN = LNXX.BF_SSN
		AND LNXX.LN_SEQ = LNXX.LN_SEQ
	INNER JOIN DFXX_BR_DFR_REQ DFXX
		ON LNXX.BF_SSN = DFXX.BF_SSN
		AND LNXX.LF_DFR_CTL_NUM = DFXX.LF_DFR_CTL_NUM
WHERE
	DFXX.LC_DFR_TYP IN (XX)
	AND DATEDIFF(MONTH, LNXX.LD_DFR_BEG, LNXX.LD_DFR_END) > X
	AND LNXX.LC_STA_LONXX = 'A'
	AND DFXX.LC_DFR_STA = 'A'
	AND LNXX.LC_DFR_RSP IN (XXX,XXX)--approval codes
ORDER BY
	PDXX.DF_SPE_ACC_ID
	,LNXX.LN_SEQ