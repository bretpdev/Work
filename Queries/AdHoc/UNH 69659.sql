USE UDW
GO

SELECT DISTINCT
	COUNT(DISTINCT ln20.BF_SSN) AS BWR_COUNT
FROM
	UDW..LN20_EDS LN20
	INNER JOIN UDW..LN10_LON LN10
		ON LN10.BF_SSN = LN20.BF_SSN
		AND LN10.LN_SEQ = LN20.LN_SEQ
WHERE
	LN10.LA_CUR_PRI > 0
	AND LN10.LC_STA_LON10 = 'R'
	AND LN20.LC_EDS_TYP = 'M'
	AND ln20.LC_STA_LON20 = 'a'