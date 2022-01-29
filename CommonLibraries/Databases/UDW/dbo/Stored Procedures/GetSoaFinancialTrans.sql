﻿CREATE PROCEDURE [dbo].[GetSoaFinancialTrans]
		@BF_SSN char(9)
AS
	SELECT
		LN90.LN_SEQ AS [Loan Seq],
		CONVERT(VARCHAR(10),LN90.LD_FAT_EFF ,101) AS [Date],
		isnull(FT.Label, (LN90.PC_FAT_TYP + LN90.PC_FAT_SUB_TYP)) AS [Transaction Type],
		'$' + CONVERT(VARCHAR(20),ABS(ISNULL(LN90.LA_FAT_PCL_FEE,0) + ISNULL(LN90.LA_FAT_NSI,0) + ISNULL(LN90.LA_FAT_LTE_FEE,0) + ISNULL(LN90.LA_FAT_ILG_PRI,0) + ISNULL(LN90.LA_FAT_CUR_PRI,0)),1) AS [Total Amount of Transaction],
		'$' + CONVERT(VARCHAR(20),ABS(ISNULL(LN90.LA_FAT_ILG_PRI,0) + ISNULL(LN90.LA_FAT_CUR_PRI,0)),1) AS [Amount to Principal],
		'$' + CONVERT(VARCHAR(20),ABS(ISNULL(LN90.LA_FAT_NSI,0)),1) AS [Amount to Interest]
	FROM
		LN90_FIN_ATY LN90 
	LEFT JOIN FormatTranslation FT
		ON FT.Start = LN90.PC_FAT_TYP + LN90.PC_FAT_SUB_TYP
		AND FT.FmtName = '$FATRAN'
	WHERE
		LN90.BF_SSN = @BF_SSN
		AND LN90.LC_STA_LON90 = 'A'
		AND (LN90.LC_FAT_REV_REA = '' or LN90.LC_FAT_REV_REA is null)
		AND (ABS(ISNULL(LN90.LA_FAT_PCL_FEE,0) + ISNULL(LN90.LA_FAT_NSI,0) + ISNULL(LN90.LA_FAT_LTE_FEE,0) + ISNULL(LN90.LA_FAT_ILG_PRI,0) + ISNULL(LN90.LA_FAT_CUR_PRI,0))) > 0
	ORDER BY 
		LN90.LN_SEQ, 
		LN90.LD_FAT_EFF 
RETURN 0