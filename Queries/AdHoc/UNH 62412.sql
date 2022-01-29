SELECT
	PD10.DF_SPE_ACC_ID,
	LN10.LN_SEQ,
	LN10.LC_STA_LON10,
	LN10.LA_CUR_PRI
FROM
	UDW..LN10_LON LN10
	INNER JOIN UDW..PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = LN10.BF_SSN
WHERE 
	IC_LON_PGM = 'TILP'
	AND LA_CUR_PRI > 0
	AND LC_STA_LON10 = 'R'