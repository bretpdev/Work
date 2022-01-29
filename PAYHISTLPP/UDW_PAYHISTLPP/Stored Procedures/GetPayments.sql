﻿CREATE PROCEDURE [payhistlpp].[GetPayments]
	@UniqueId VARCHAR(23),
	@UniqueIdSeq VARCHAR(2)
AS
	SELECT
		LN90.LD_FAT_EFF AS [TransactionEffectiveDate],
		LN90.LD_FAT_PST AS [PostEffectiveDate],
		LN90.LD_FAT_APL AS [ApplicationDate],
		CONCAT(RTRIM(LK10_TYP.PX_DSC_SHO),' ',RTRIM(LK10_SUB.PX_DSC_SHO)) AS [TransactionType],
		ISNULL(LN90.LA_FAT_CUR_PRI,0.00) + ISNULL(LN90.LA_FAT_NSI,0.00) + ISNULL(LN90.LA_FAT_LTE_FEE,0.00) AS [TransactionAmount],
		COALESCE(LN90.LA_FAT_CUR_PRI,0.00) AS [Principal],
		COALESCE(LN90.LA_FAT_NSI,0.00) AS [Interest],
		COALESCE(LN90.LA_FAT_LTE_FEE,0.00) AS [Fees],
		SUM(COALESCE(LN90.LA_FAT_CUR_PRI,0.00)) OVER (PARTITION BY LN90.BF_SSN, LN90.LN_SEQ) AS [Balance]
	FROM
		UDW..LN90_FIN_ATY LN90
		INNER JOIN UDW..LN10_LON LN10
			ON LN10.BF_SSN = LN90.BF_SSN
			AND LN10.LN_SEQ = LN90.LN_SEQ
			AND	RTRIM(LN10.LF_LON_ALT) = RTRIM(@UniqueId) 
			AND RIGHT('00' + CAST(LN10.LN_LON_ALT_SEQ AS VARCHAR(2)),2) = @UniqueIdSeq
		INNER JOIN UDW..LK10_LS_CDE_LKP LK10_TYP
			ON LK10_TYP.PX_ATR_VAL = LN90.PC_FAT_TYP
			AND LK10_TYP.PM_ATR = 'PC-FAT-TYP'
		INNER JOIN UDW..LK10_LS_CDE_LKP LK10_SUB
			ON LK10_SUB.PX_ATR_VAL = LN90.PC_FAT_SUB_TYP
			AND LK10_SUB.PM_ATR = 'PC-FAT-SUB-TYP'
	WHERE
		LN90.LC_STA_LON90 = 'A'
		AND ISNULL(LN90.LC_FAT_REV_REA, '') = ''
		AND LN90.LA_FAT_CUR_PRI > 0.00