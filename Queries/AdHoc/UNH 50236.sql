SELECT
	LN10.*
FROM
	tempdb..UNH50236 UNH
	INNER JOIN OPENQUERY
	(
		DUSTER,
		'
			SELECT
				LN10.BF_SSN,
				LN10.LN_SEQ,
				LN10.LD_LON_EFF_ADD
			FROM
				OLWHRM1.LN10_LON LN10
		'	
	) LN10 ON LN10.BF_SSN = UNH.BF_SSN AND LN10.LN_SEQ = UNH.LN_SEQ
ORDER BY
	LN10.BF_SSN,
	LN10.LN_SEQ
;
