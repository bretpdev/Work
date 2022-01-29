SELECT DISTINCT
	LTRIM(RTRIM(PD10.DM_PRS_LST)) + ' ' +	LTRIM(RTRIM(PD10.DM_PRS_1)) AS NAME,
	pd10.DF_SPE_ACC_ID,
	LN10.LN_SEQ
FROM
	UDW..LN10_LON LN10
	INNER JOIN UDW..PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = LN10.BF_SSN
	INNER JOIN UDW..DW01_DW_CLC_CLU DW01
		ON DW01.BF_SSN = LN10.BF_SSN
		AND DW01.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN
	(
		SELECT
			RS10.BF_SSN,
			LN65.LN_SEQ,
			MAX(RS10.LD_RPS_1_PAY_DU) AS MD
		FROM
			UDW..RS10_BR_RPD RS10
			INNER JOIN UDW..LN65_LON_RPS LN65
				ON LN65.BF_SSN = RS10.BF_SSN
				AND LN65.LN_RPS_SEQ = RS10.LN_RPS_SEQ
		GROUP BY
			RS10.BF_SSN,
			LN65.LN_SEQ
	)MAX_SCH
		ON MAX_SCH.BF_SSN = LN10.BF_SSN
		AND MAX_SCH.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN
	(
		SELECT DISTINCT
			LN10.BF_SSN,
			LN10.LN_SEQ,
			SUM(CASE WHEN ISNULL(RS10.LC_STA_RPST10, ' ') = 'A' THEN 1 ELSE 0 END) AS ACTIVE_COUNT
		FROM
			UDW..LN10_LON LN10
			LEFT JOIN UDW..RS10_BR_RPD RS10
				ON RS10.BF_SSN = LN10.BF_SSN
		group by
			LN10.BF_SSN,
			LN10.LN_SEQ

	)ACT_COUNT
		ON ACT_COUNT.BF_SSN = LN10.BF_SSN
		AND ACT_COUNT.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN UDW..RS10_BR_RPD RS10
		ON RS10.BF_SSN = LN10.BF_SSN
	LEFT JOIN UDW..LN65_LON_RPS LN65
		ON LN65.BF_SSN = LN10.BF_SSN
		AND LN65.LN_SEQ = LN10.LN_SEQ
		AND LN65.LN_RPS_SEQ = RS10.LN_RPS_SEQ
WHERE
	LN10.LA_CUR_PRI > 0
	AND LN10.LC_STA_LON10 = 'R'
	AND DW01.WC_DW_LON_STA = '03'
	AND (
			MAX_SCH.LN_SEQ IS NULL 
			OR
			ACT_COUNT.ACTIVE_COUNT = 0
		)
order by 
	pd10.DF_SPE_ACC_ID,
	LN10.LN_SEQ