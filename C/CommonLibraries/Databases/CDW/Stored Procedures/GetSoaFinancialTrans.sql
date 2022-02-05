CREATE PROCEDURE [dbo].[GetSoaFinancialTrans]
	@AccountNumber char(10)
AS
	SELECT
		LN_SEQ AS [Loan Seq],
		LD_FAT_EFF as [Date],
		FT.Label AS [Transaction Type],
		ABS(LA_FAT_PCL_FEE + LA_FAT_NSI + LA_FAT_LTE_FEE + LA_FAT_ILG_PRI + LA_FAT_CUR_PRI) AS [Total Amount of Transaction],
		ABS(LA_FAT_ILG_PRI + LA_FAT_CUR_PRI) as [Amount to Principal],
		ABS(LA_FAT_NSI) as [Amount to Interest]
	FROM
		LN90_FinancialHistory FH 
	LEFT JOIN FormatTranslation FT
		ON FT.Start = fh.PC_FAT_TYP + FH.PC_FAT_SUB_TYP
		AND FT.FmtName = '$FATRAN'
	WHERE
		FH.DF_SPE_ACC_ID = @AccountNumber
		AND LC_STA_LON90 = 'A'
		AND (LC_FAT_REV_REA = '' or LC_FAT_REV_REA is null)

	ORDER BY FH.LN_SEQ, cast(LD_FAT_EFF as date)
RETURN 0
