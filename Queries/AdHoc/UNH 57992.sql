SELECT DISTINCT
	LN10.BF_SSN AS SSN,
	LN10.LN_SEQ AS LOAN,
	LN16.LN_DLQ_MAX + 1 AS DAYS_DELINQUENT,
	FT.Label AS LOAN_STATUS,
	LN10.IC_LON_PGM AS LOAN_TYPE

FROM
	UDW..LN10_LON LN10
	INNER JOIN UDW..LN16_LON_DLQ_HST LN16
		ON LN10.BF_SSN = LN16.BF_SSN
		AND LN10.LN_SEQ = LN16.LN_SEQ
		AND LN16.LC_STA_LON16 = '1'
	INNER JOIN UDW..DW01_DW_CLC_CLU DW01
		ON LN10.BF_SSN = DW01.BF_SSN
		AND LN10.LN_SEQ = DW01.LN_SEQ
	LEFT JOIN UDW..FormatTranslation FT
		ON DW01.WC_DW_LON_STA = FT.Start
		AND FT.FmtName = '$LONSTA' 

WHERE
	LN10.LC_STA_LON10 = 'R'
	AND LN10.LA_CUR_PRI > 0
	AND LN16.LN_DLQ_MAX + 1 >= 365

