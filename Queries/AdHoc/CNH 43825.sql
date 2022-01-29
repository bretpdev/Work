SELECT DISTINCT
	PDXX.DF_SPE_ACC_ID,
	--SUM(ABS(COALESCE(LA_FAT_CUR_PRI,X.XX)) + abs(isnull(LNXX.LA_FAT_ILG_PRI,X.XX))) AS PRINCIPAL,
	ABS(SUM(COALESCE(LA_FAT_NSI,X.XX))) AS INTEREST
	--SUM(ABS(COALESCE(LA_FAT_CUR_PRI,X.XX)) + abs(isnull(LNXX.LA_FAT_ILG_PRI,X.XX)) + ABS((COALESCE(LA_FAT_NSI,X.XX)))) AS TOTAL_PAID

FROM
	CDW..LNXX_FIN_ATY LNXX
	INNER JOIN CDW..PDXX_PRS_NME PDXX
		ON PDXX.DF_PRS_ID = LNXX.BF_SSN
WHERE
	LNXX.LC_STA_LONXX = 'A'
	AND (LC_FAT_REV_REA = ' ' OR LC_FAT_REV_REA = '' OR LC_FAT_REV_REA IS NULL)
	AND LNXX.PC_FAT_TYP = 'XX'
	AND LNXX.LD_FAT_EFF >= 'XX/XX/XXXX'
	AND LNXX.PC_FAT_SUB_TYP in ('XX', 'XX','XX', 'XX', 'XX','XX','XX','XX','XX','XX','XX','XX','XX','XX', 'XX','XX', 'XX','XX','XX','XX','XX', 'XX','XX','XX', 'XX','XX','XX','XX','XX','XX')  
GROUP BY
	PDXX.DF_SPE_ACC_ID
HAVING 
	ABS(SUM(COALESCE(LA_FAT_NSI,X.XX))) >= XXX