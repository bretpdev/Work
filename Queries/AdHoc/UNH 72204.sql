SELECT
	--LN10.BF_SSN,
	--LN10.LN_SEQ,
	LR10.IM_LDR_FUL,
	COUNT(*) AS LOAN_COUNT
FROM
	UDW..LN10_LON LN10
	INNER JOIN UDW..LR10_LDR_DMO LR10
		ON LR10.IF_DOE_LDR = LN10.IF_DOE_LDR
WHERE
	LA_CUR_PRI > 0
	AND LC_STA_LON10 = 'R'
GROUP BY
	LR10.IM_LDR_FUL
--203,263