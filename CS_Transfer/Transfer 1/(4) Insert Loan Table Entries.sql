INSERT INTO CDW..CS_Transfer1Loans(BF_SSN, LN_SEQ, LoanProgram, LoanSaleId)
SELECT DISTINCT
	LN10.BF_SSN,
	LN10.LN_SEQ,
	CASE WHEN LN10.LC_FED_PGM_YR = 'LNC' THEN 'LNC'
         WHEN LN10.LC_FED_PGM_YR = 'DLO' THEN 'DLO'
	ELSE '' END AS LoanProgram,
	NULL AS LoanSaleId
FROM
	CDW..CS_Transfer1 T1 
	INNER JOIN CDW..LN10_LON LN10 
		ON LN10.BF_SSN = T1.BF_SSN 
WHERE 
	LN10.LC_STA_LON10 = 'R' 
	AND LN10.LA_CUR_PRI > 0.00