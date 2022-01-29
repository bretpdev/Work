﻿CREATE PROCEDURE [soaltrfed].[GetFinancialTransactions]
	@AccountNumber VARCHAR(10),
	@IsCoborrower BIT = 0,
	@BorrowerSSN VARCHAR(9)
AS
BEGIN

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

IF @IsCoborrower = 0
	BEGIN
		SELECT
			LN90.LN_SEQ AS [Loan Seq],
			CONVERT(VARCHAR,LN90.LD_FAT_EFF,101) AS [Transaction Date],
			COALESCE(FMT.Label, LN90.PC_FAT_TYP + LN90.PC_FAT_SUB_TYP) AS [Transaction Type],
			ABS(COALESCE(LA_FAT_PCL_FEE,0) + COALESCE(LA_FAT_NSI,0) + COALESCE(LA_FAT_LTE_FEE,0) + COALESCE(LA_FAT_ILG_PRI,0) + COALESCE(LA_FAT_CUR_PRI,0)) AS [Total Amount of Transaction],
			ABS(COALESCE(LA_FAT_ILG_PRI,0) + COALESCE(LA_FAT_CUR_PRI,0)) AS [Amount to Principal],
			ABS(COALESCE(LA_FAT_NSI,0)) AS [Amount to Interest]
		FROM
			PD10_PRS_NME PD10
			INNER JOIN LN90_FIN_ATY LN90
				ON LN90.BF_SSN = PD10.DF_PRS_ID
				AND LN90.LC_STA_LON90 ='A'
				AND COALESCE(LN90.LC_FAT_REV_REA,'') = ''
			LEFT JOIN FormatTranslation FMT
				ON FMT.Start = LN90.PC_FAT_TYP + LN90.PC_FAT_SUB_TYP
				AND FMT.FmtName = '$FATRAN'
		WHERE
			PD10.DF_SPE_ACC_ID = @AccountNumber
		ORDER BY 
			LN90.LN_SEQ, 
			CAST(LN90.LD_FAT_EFF AS DATE)
	END
ELSE
	BEGIN
		SELECT
			LN90.LN_SEQ AS [Loan Seq],
			CONVERT(VARCHAR,LN90.LD_FAT_EFF,101) AS [Transaction Date],
			COALESCE(FMT.Label, LN90.PC_FAT_TYP + LN90.PC_FAT_SUB_TYP) AS [Transaction Type],
			ABS(COALESCE(LA_FAT_PCL_FEE,0) + COALESCE(LA_FAT_NSI,0) + COALESCE(LA_FAT_LTE_FEE,0) + COALESCE(LA_FAT_ILG_PRI,0) + COALESCE(LA_FAT_CUR_PRI,0)) AS [Total Amount of Transaction],
			ABS(COALESCE(LA_FAT_ILG_PRI,0) + COALESCE(LA_FAT_CUR_PRI,0)) AS [Amount to Principal],
			ABS(COALESCE(LA_FAT_NSI,0)) AS [Amount to Interest]
		FROM
			PD10_PRS_NME PD10
			INNER JOIN LN20_EDS LN20
				ON LN20.LF_EDS = PD10.DF_PRS_ID
				AND LN20.LC_EDS_TYP = 'M'
				AND LN20.LC_STA_LON20 = 'A'
				AND LN20.BF_SSN = @BorrowerSSN
			INNER JOIN LN90_FIN_ATY LN90
				ON LN90.BF_SSN = LN20.BF_SSN
				AND LN90.LN_SEQ = LN20.LN_SEQ
				AND LN90.LC_STA_LON90 ='A'
				AND COALESCE(LN90.LC_FAT_REV_REA,'') = ''
			LEFT JOIN FormatTranslation FMT
				ON FMT.Start = LN90.PC_FAT_TYP + LN90.PC_FAT_SUB_TYP
				AND FMT.FmtName = '$FATRAN'
		WHERE
			PD10.DF_SPE_ACC_ID = @AccountNumber
		ORDER BY 
			LN90.LN_SEQ, 
			CAST(LN90.LD_FAT_EFF AS DATE)
	END
END