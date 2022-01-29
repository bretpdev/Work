SELECT DISTINCT
	PDXX.DF_SPE_ACC_ID,
	LNXX.LN_SEQ,
	LNXX.IC_LON_PGM,
	LNXX.LD_LON_X_DSB,
	LNXX.LC_STA_LONXX,
	IIF(LNXX.LA_CUR_PRI > X.XX, '>X','X') AS BALANCE
FROM
	CDW..LNXX_LON LNXX
	INNER JOIN CDW..PDXX_PRS_NME PDXX
		ON LNXX.BF_SSN = PDXX.DF_PRS_ID
WHERE
	LNXX.IC_LON_PGM = 'DLPCNS'
	AND LNXX.LD_LON_X_DSB > CONVERT(DATE,'XXXXXXXX')
	AND LNXX.LA_CUR_PRI > X.XX
	AND LNXX.LC_STA_LONXX = 'R'
ORDER BY
	LD_LON_X_DSB
;