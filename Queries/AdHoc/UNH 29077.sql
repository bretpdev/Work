SELECT
	*
FROM OPENQUERY(DUSTER,
'
	SELECT 
		LN65.BF_SSN,
		LN10.LD_LON_1_DSB,
		COUNT(DISTINCT LN65.LN_RPS_SEQ)
	FROM
		OLWHRM1.LN10_LON LN10
		INNER JOIN OLWHRM1.LN65_LON_RPS LN65
			ON LN65.BF_SSN = LN10.BF_SSN
			AND LN65.LN_SEQ = LN10.LN_SEQ
			AND LN65.LC_STA_LON65 = ''A''
		LEFT OUTER JOIN OLWHRM1.RS10_BR_RPD RS10
			ON RS10.BF_SSN = LN10.BF_SSN
			AND RS10.LN_RPS_SEQ = LN65.LN_RPS_SEQ
			AND LC_STA_RPST10 = ''A''
	WHERE
		LN10.IC_LON_PGM IN(''SUBCNS'',''UNCNS'')
	GROUP BY
		LN65.BF_SSN,
		LD_LON_1_DSB
	HAVING
		COUNT(DISTINCT LN65.LN_RPS_SEQ)  >1
'
)

SELECT
	*
FROM OPENQUERY(DUSTER,
'
	SELECT 
		LN65.BF_SSN,
		LN10.LD_LON_1_DSB,
		COUNT(DISTINCT LN65.LN_RPS_SEQ)
	FROM
		OLWHRM1.LN10_LON LN10
		INNER JOIN OLWHRM1.LN65_LON_RPS LN65
			ON LN65.BF_SSN = LN10.BF_SSN
			AND LN65.LN_SEQ = LN10.LN_SEQ
			AND LN65.LC_STA_LON65 = ''A''
		LEFT OUTER JOIN OLWHRM1.RS10_BR_RPD RS10
			ON RS10.BF_SSN = LN10.BF_SSN
			AND RS10.LN_RPS_SEQ = LN65.LN_RPS_SEQ
			AND LC_STA_RPST10 = ''A''
	WHERE
		LN10.IC_LON_PGM IN(''SUBSPC'',''UNSPC'')
	GROUP BY
		LN65.BF_SSN,
		LD_LON_1_DSB
	HAVING
		COUNT(DISTINCT LN65.LN_RPS_SEQ)  >1	
'
)