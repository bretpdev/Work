DECLARE @BEGIN AS DATE = '2018-01-01'
DECLARE @END AS DATE = '2018-06-01'


SELECT DISTINCT
	PD10.DF_SPE_ACC_ID
	,CONCAT(RTRIM(PD10.DM_PRS_1), ' ', RTRIM(PD10.DM_PRS_MID), ' ', RTRIM(PD10.DM_PRS_LST)) AS NAME
	,CONCAT(FS10.LF_FED_AWD, FORMAT (FS10.LN_FED_AWD_SEQ, '000')) AS AWARD_ID
	,LN90.LD_FAT_APL 
	,LN90.LD_FAT_EFF
	,LN90.PC_FAT_SUB_TYP
	,LN90.LA_FAT_CUR_PRI
	,LN90.LA_FAT_NSI
FROM 
	CDW.DBO.LN90_FIN_ATY LN90
	INNER JOIN CDW.DBO.PD10_PRS_NME PD10
		ON LN90.BF_SSN = PD10.DF_PRS_ID
	INNER JOIN CDW.DBO.FS10_DL_LON FS10
		ON LN90.BF_SSN = FS10.BF_SSN
		AND LN90.LN_SEQ = FS10.LN_SEQ
WHERE 
	LN90.PC_FAT_TYP = '10'
	AND LN90.LC_FAT_REV_REA IN ('4','9')
	AND LN90.LD_FAT_APL BETWEEN @BEGIN AND @END