DECLARE @CURRENT_COUNT INT = (SELECT COUNT(*) FROM CDW..CS_Transfer3)

DECLARE @NUMBER_TO_PULL INT =  (250000 - @CURRENT_COUNT)
INSERT INTO CDW..CS_Transfer3
SELECT
	POP.*,
	SUM(CASE WHEN LN10.LC_FED_PGM_YR = 'LNC' THEN 1 ELSE 0 END) AS LNC_Count,
	SUM(CASE WHEN LN10.LC_FED_PGM_YR = 'DLO' THEN 1 ELSE 0 END) AS DLO_Count,
	NULL AS LNC_SaleId,
	NULL AS DLO_SaleId,
	NULL,
	NULL
FROM
(
SELECT DISTINCT TOP (@NUMBER_TO_PULL)
	DW01.BF_SSN,
	'GRACE BORROWER' AS [POPULATION]
FROM
	CDW..DW01_DW_CLC_CLU DW01
	INNER JOIN CDW..LN10_LON LN10
		ON LN10.BF_SSN = DW01.BF_SSN
		AND LN10.LN_SEQ = DW01.LN_SEQ
		AND LN10.LA_CUR_PRI > 0
		AND LN10.LC_STA_LON10 = 'R'
	LEFT JOIN CDW..CS_Transfer2 T1
		ON T1.BF_SSN = DW01.BF_SSN
	LEFT JOIN CDW..CS_Transfer3 T2
		ON T2.BF_SSN = DW01.BF_SSN
	LEFT JOIN CDW..CS_Transfer3_Exclusions EX
		ON EX.BF_SSN = DW01.BF_SSN
WHERE
	DW01.WC_DW_LON_STA = '01'
	AND T1.BF_SSN IS NULL
	AND T2.BF_SSN IS NULL
	AND EX.BF_SSN IS NULL
) POP
INNER JOIN CDW..LN10_LON LN10
	ON LN10.BF_SSN = POP.BF_SSN
	AND LN10.LA_CUR_PRI > 0
	AND LN10.LC_STA_LON10 = 'R'
GROUP BY
	POP.BF_SSN,
	POP.[POPULATION]