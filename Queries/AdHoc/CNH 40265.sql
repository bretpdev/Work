USE CDW
GO

SELECT
	PDXX.DF_SPE_ACC_ID,
	LNXX.LN_SEQ
FROM
	CDW..LNXX_LON LNXX
	INNER JOIN CDW..PDXX_PRS_NME PDXX
		ON PDXX.DF_PRS_ID = LNXX.BF_SSN
	INNER JOIN CDW..DFXX_BR_DFR_REQ DFXX
		ON DFXX.BF_SSN = LNXX.BF_SSN
	INNER JOIN CDW..LNXX_BR_DFR_APV LNXX
		ON LNXX.BF_SSN = LNXX.BF_SSN
		AND LNXX.LN_SEQ = LNXX.LN_SEQ
		AND LNXX.LF_DFR_CTL_NUM = DFXX.LF_DFR_CTL_NUM
WHERE
	LNXX.LA_CUR_PRI > X.XX
	AND LNXX.LC_STA_LONXX = 'R'
	AND LNXX.IC_LON_PGM IN ('DLPLUS','DLPLGB')
	AND LNXX.LD_LON_X_DSB < 'XX-XX-XXXX'
	AND DFXX.LC_DFR_TYP = 'XX'
	AND DFXX.LC_DFR_STA = 'A'
	AND LNXX.LC_STA_LONXX = 'A'
	AND DFXX.LC_STA_DFRXX = 'A'