SELECT DISTINCT
	LN10.BF_SSN,
	LN10.LN_SEQ,
	LN_IBR_QLF_FGV_MTH,
	LN_IBR_QLF_PAY_PCV,
	LN_IBR_FGV_MTH_CTR
FROM
	ODW..DC01_LON_CLM_INF DC01
	INNER JOIN UDW..LN10_LON LN10
		ON LN10.LF_LON_ALT = DC01.AF_APL_ID
		AND LN10.LN_LON_ALT_SEQ = CAST(DC01.AF_APL_ID_SFX AS INT)
	INNER JOIN UDW..LN09_RPD_PIO_CVN LN09
		ON LN09.BF_SSN = LN10.BF_SSN
		AND LN09.LN_SEQ = LN10.LN_SEQ
WHERE
	LN_IBR_QLF_FGV_MTH > 0
	AND LN10.LA_CUR_PRI > 0.00
	AND LN10.LC_STA_LON10 = 'R'
	AND
	(
		LN_IBR_QLF_PAY_PCV IS NULL
		OR
		LN_IBR_FGV_MTH_CTR IS NULL
	)
ORDER BY
	LN10.BF_SSN,
	LN10.LN_SEQ