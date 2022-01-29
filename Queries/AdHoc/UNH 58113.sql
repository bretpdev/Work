USE UDW
GO

SELECT DISTINCT
	LN10.BF_SSN AS SSN
	,LN65.LC_TYP_SCH_DIS AS SCHEDULE_TYPE
	,LN10.LD_LON_IBR_ENT AS DATE_SUBSIDY_BEGAN

	--for validation:
	,LN65.LC_STA_LON65
	,CAST(LN65.LD_CRT_LON65 AS DATE) AS LD_CRT_LON65
	,LN10.LD_LON_IBR_ENT
	,RS05.BC_IBR_INF_SRC_VER
FROM
	LN10_LON LN10
	INNER JOIN LN65_LON_RPS LN65
		ON LN10.BF_SSN = LN65.BF_SSN
		AND LN10.LN_SEQ = LN65.LN_SEQ
	INNER JOIN RS05_IBR_RPS RS05
		ON LN10.BF_SSN = RS05.BF_SSN
WHERE	
	LN65.LC_TYP_SCH_DIS IN ('IB','I3')
	AND LN65.LC_STA_LON65 = 'A'
	AND LN65.LD_CRT_LON65 BETWEEN CAST('10/1/2015' AS DATE) AND CAST('12/31/2017' AS DATE)
	AND LN10.LD_LON_IBR_ENT BETWEEN CAST('10/1/2012' AS DATE) AND CAST('12/31/2017' AS DATE)
	AND RS05.BC_IBR_INF_SRC_VER = 'ALT'

;
