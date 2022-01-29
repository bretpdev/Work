USE CDW
GO

DECLARE @StartDate DATE = 'XXXX-X-X'
DECLARE @EndDate DATE = GETDATE()

-- Transactions
SELECT DISTINCT
	PDXX.DF_SPE_ACC_ID [Account Number],
	RTRIM(PDXX.DM_PRS_X) + ' ' + RTRIM(PDXX.DM_PRS_MID) + ' ' + RTRIM(PDXX.DM_PRS_LST) [Full Name],
	FSXX.LF_FED_AWD + CAST(FSXX.LN_FED_AWD_SEQ AS VARCHAR(XX)) [Award Id],
	LNXX.LD_FAT_APL [Applied Date],
	LNXX.LD_FAT_EFF [Effective Date],
	LNXX.PC_FAT_SUB_TYP [Payment Sub-type],
	ISNULL(LNXX.LA_FAT_CUR_PRI, X.XX) [Principal],
	ISNULL(LNXX.LA_FAT_NSI, X.XX) [Interest]
FROM
	CDW..PDXX_PRS_NME PDXX
	INNER JOIN CDW..LNXX_FIN_ATY LNXX ON PDXX.DF_PRS_ID = LNXX.BF_SSN
	INNER JOIN CDW..FSXX_DL_LON FSXX ON LNXX.BF_SSN = FSXX.BF_SSN AND LNXX.LN_SEQ = FSXX.LN_SEQ
WHERE
	LNXX.PC_FAT_TYP = 'XX'
	AND 
	LNXX.PC_FAT_SUB_TYP IN ('XX', 'XX')
	AND 
	LNXX.LD_FAT_APL BETWEEN @StartDate AND @EndDate


-- Principal Adjustments
SELECT DISTINCT
	PDXX.DF_SPE_ACC_ID [Account Number],
	RTRIM(PDXX.DM_PRS_X) + ' ' + RTRIM(PDXX.DM_PRS_MID) + ' ' + RTRIM(PDXX.DM_PRS_LST) [Full Name],
	FSXX.LF_FED_AWD + CAST(FSXX.LN_FED_AWD_SEQ AS VARCHAR(XX)) [Award Id],
	LNXX.LD_FAT_APL [Applied Date],
	LNXX.LD_FAT_EFF [Effective Date],
	LNXX.PC_FAT_SUB_TYP [Payment Sub-type],
	ISNULL(LNXX.LA_FAT_CUR_PRI, X.XX) [Principal],
	ISNULL(LNXX.LA_FAT_NSI, X.XX) [Interest]
FROM
	CDW..PDXX_PRS_NME PDXX
	INNER JOIN CDW..LNXX_FIN_ATY LNXX ON PDXX.DF_PRS_ID = LNXX.BF_SSN
	INNER JOIN CDW..FSXX_DL_LON FSXX ON LNXX.BF_SSN = FSXX.BF_SSN AND LNXX.LN_SEQ = FSXX.LN_SEQ
WHERE
	LNXX.PC_FAT_TYP = 'XX'
	AND 
	LNXX.LC_FAT_REV_REA IN ('X', 'X')
	AND 
	LNXX.LD_FAT_APL BETWEEN @StartDate AND @EndDate
