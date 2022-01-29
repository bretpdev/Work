USE CDW
GO


SELECT DISTINCT
	pdXX.DF_SPE_ACC_ID,
	--
	lnXX.*
	--sum(la_fat_cur_pri)

FROM
	CDW..LNXX_FIN_ATY LNXX
	INNER JOIN CDW..PDXX_PRS_NME PDXX
		on PDXX.DF_PRS_ID = LNXX.BF_SSN
	INNER JOIN CDW..ADXX_PCV_ATY_ADJ LNXX
		ON LNXX.BF_SSN = LNXX.BF_SSN
		AND LNXX.LN_SEQ = LNXX.LN_SEQ
WHERE
	LNXX.PC_FAT_TYP = 'XX'
	and lnXX.PC_FAT_SUB_TYP = 'XX'
	--AND LD_FAT_APL BETWEEN  'XX/XX/XXXX' AND 'XX/XX/XXXX'
	AND ISNULL(lnXX.LC_FAT_REV_REA,'') != ''
	AND LC_STA_LONXX = 'A'
	and
	(LD_FAT_APL = 'XX/XX/XXXX' )
	--and lnXX.bf_ssn = 'XXXXXXXXX'
	--and lnXX.LA_FAT_CUR_PRI > X
AND LNXX.LC_WOF_WUP_REA = ''