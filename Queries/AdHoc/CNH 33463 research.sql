SELECT
	BF_SSN,
	LN_SEQ,
	--LN_FAT_SEQ,
	--LC_FAT_REV_REA,
	--LN_FAT_SEQ_REV,
	LD_FAT_APL,
	--LD_FAT_EFF,
	--LD_FAT_PST,
	--LC_CSH_ADV,
	--LC_STA_LONXX,
	PC_FAT_TYP,
	PC_FAT_SUB_TYP,
	SUM(COALESCE(LA_FAT_NSI,X)) AS AppliedToInt,
	SUM(COALESCE(LA_FAT_CUR_PRI,X)) AS AppliedToPrin,
	SUM(COALESCE(LA_FAT_NSI,X) + COALESCE(LA_FAT_CUR_PRI,X)) AS Total
FROM
	CDW..LNXX_FIN_ATY
WHERE
	(
		LD_FAT_APL BETWEEN 'XXXX-XX-XX' AND 'XXXX-XX-XX'
		OR LD_FAT_PST BETWEEN 'XXXX-XX-XX' AND 'XXXX-XX-XX'
		OR LD_FAT_EFF BETWEEN 'XXXX-XX-XX' AND 'XXXX-XX-XX'
	)
	AND PC_FAT_TYP = 'XX'
	AND LC_FAT_REV_REA = ''
	AND LC_STA_LONXX = 'A'
GROUP BY
	BF_SSN,
	LN_SEQ,
	PC_FAT_TYP,
	PC_FAT_SUB_TYP,
	LD_FAT_APL
ORDER BY
	LD_FAT_APL,
	BF_SSN,
	LN_SEQ


	
	--This guy maybe based on payment amount, posted date, payment applied to loan sequence X LN_FAT_SEQ XXX
	select * from cdw..LNXX_FIN_ATY where BF_SSN = 'XXXXXXXXX' AND (
		LD_FAT_APL BETWEEN 'XXXX-XX-XX' AND 'XXXX-XX-XX'
		OR LD_FAT_PST BETWEEN 'XXXX-XX-XX' AND 'XXXX-XX-XX'
		OR LD_FAT_EFF BETWEEN 'XXXX-XX-XX' AND 'XXXX-XX-XX'
	)
	AND PC_FAT_TYP = 'XX'
	AND LC_FAT_REV_REA = ''
	AND LC_STA_LONXX = 'A' order by LN_SEQ, LN_FAT_SEQ
	select * from cdw..PDXX_PRS_NME where DF_PRS_ID = 'XXXXXXXXX'
	select * from cls..CheckByPhone where AccountNumber = 'XXXXXXXXXX'
	select * from openquery(legend, 'select * from webflsX.RMXX_ONL_PAY where bf_ssn = ''XXXXXXXXX''')

	select * from cls..CheckByPhone where AccountHolderName like '%KERN%'
	select * from openquery(legend, 'select * from webflsX.RMXX_ONL_PAY where BN_LST like ''%KERN%''')
