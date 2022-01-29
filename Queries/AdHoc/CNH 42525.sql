/****** Script for SelectTopNRows command from SSMS  ******/
SELECT 
	PDXX.DF_SPE_ACC_ID AS ACCOUNT_NUMBER,
	LNXX.LN_SEQ,
	RTRIM(PDXX.DM_PRS_X) + ' ' + RTRIM(PDXX.DM_PRS_LST) AS [NAME],
	LNXX.LD_FAT_PST,
	LNXX.PC_FAT_TYP,
	LNXX.PC_FAT_SUB_TYP,
	LNXX.LA_FAT_CUR_PRI AS LA_FAT_CUR_PRI,
	LNXX.LA_FAT_NSI AS LA_FAT_NSI
 FROM 
	[CDW].[dbo].[RMXX_BR_RMT_PST] RMXX
	INNER JOIN CDW..PDXX_PRS_NME PDXX
		ON PDXX.DF_PRS_ID = RMXX.BF_SSN
LEFT JOIN CDW..LNXX_FIN_ATY LNXX
	ON LNXX.BF_SSN = RMXX.BF_SSN
	AND LNXX.LD_FAT_PST >= 'XX/XX/XXXX'
	AND LNXX.LC_STA_LONXX = 'A'
	AND ISNULL(LNXX.LC_FAT_REV_REA,'') = ''
 WHERE
	LD_RMT_BCH_INI = 'XX/XX/XXXX'
	AND LC_RMT_BCH_SRC_IPT = 'T'
	and LN_RMT_BCH_SEQ in (X,X,X)
ORDER BY 
	ACCOUNT_NUMBER, 
	LN_SEQ,
	LD_FAT_PST