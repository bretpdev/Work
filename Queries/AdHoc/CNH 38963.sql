USE CDW;
GO

SELECT DISTINCT
	PDXX.DF_SPE_ACC_ID,
	LNXX.LN_SEQ,
	LNXX.LD_LON_X_DSB,
	LNXX.LD_LON_APL_RCV,
	LNXX.IC_LON_PGM,
	LNXX.LC_ITR_TYP,
	LNXX.LF_RGL_CAT_LPXX,
	LNXX.LC_STA_LONXX,
	IIF(LNXX.LA_CUR_PRI > X.XX,'>X','X') AS LA_CUR_PRI
FROM 
	PDXX_PRS_NME PDXX
	INNER JOIN LNXX_LON LNXX
		ON PDXX.DF_PRS_ID = LNXX.BF_SSN
	INNER JOIN LNXX_INT_RTE_HST LNXX
		ON LNXX.BF_SSN = LNXX.BF_SSN
		AND LNXX.LN_SEQ = LNXX.LN_SEQ
WHERE
	LNXX.LC_STA_LONXX = 'R'
	AND LNXX.LA_CUR_PRI > X.XX
	AND LNXX.LC_ITR_TYP = 'SV'
	AND LNXX.LD_LON_APL_RCV <= CONVERT(DATE,'XXXXXXXX')
	AND (
			LNXX.IC_LON_PGM IN ('DLPCNS','DLUSPL','DLUCNS','DLSSPL','DLSCNS') --consolodation loan programs
			OR LNXX.LF_RGL_CAT_LPXX IN 
			(
				'XXXXXXX', 'XXXXXXX', 'XXXXXXX',			--DLPCNS
				'XXXXXXX', 'XXXXXXX', 'XXXXXXX', 'XXXXXXX',	--DLUSPL
				'XXXXXXX', 'XXXXXXX', 'XXXXXXX', 'XXXXXXX',	--DLUCNS
				'XXXXXXX', 'XXXXXXX', 'XXXXXXX', 'XXXXXXX',	--DLSSPL
				'XXXXXXX', 'XXXXXXX', 'XXXXXXX', 'XXXXXXX'	--DLSCNS
			)
		)
ORDER BY
	PDXX.DF_SPE_ACC_ID
;
	

	
	

