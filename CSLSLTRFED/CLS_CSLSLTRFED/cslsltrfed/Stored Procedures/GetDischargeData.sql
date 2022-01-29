CREATE PROCEDURE [cslsltrfed].[GetDischargeData]
	@AccountNumber VARCHAR(10),
	@IsCoborrower BIT
AS
	IF @IsCoborrower = 0
		BEGIN
			SELECT
				COALESCE(FT.Label, LN10.IC_LON_PGM) AS [Loan Program],
				CONVERT(VARCHAR(10),LN10.LD_LON_1_DSB,101) AS [Date Disbursed],
				'$' + CAST(LN10.LA_CUR_PRI AS VARCHAR) AS [Current Principal]
			FROM
				CDW..PD10_PRS_NME PD10
				INNER JOIN CDW..LN10_LON LN10
					ON LN10.BF_SSN = PD10.DF_PRS_ID
					AND LN10.LC_STA_LON10 = 'R'
				LEFT JOIN CDW..FormatTranslation FT
					ON LN10.IC_LON_PGM = FT.[Start]
					AND FT.FmtName = '$LNPROG'
			WHERE 
				PD10.DF_SPE_ACC_ID = @AccountNumber
			ORDER BY
				LN10.LN_SEQ
		END
	ELSE
		BEGIN
			SELECT
				COALESCE(FT.Label, LN10.IC_LON_PGM) AS [Loan Program],
				CONVERT(VARCHAR(10),LN10.LD_LON_1_DSB,101) AS [Date Disbursed],
				'$' + CAST(LN10.LA_CUR_PRI AS VARCHAR) AS [Current Principal]
			FROM
				CDW..PD10_PRS_NME PD10
				INNER JOIN CDW..LN10_LON LN10
					ON LN10.BF_SSN = PD10.DF_PRS_ID
					AND LN10.LC_STA_LON10 = 'R'
				INNER JOIN CDW..LN20_EDS LN20
					ON LN10.BF_SSN = LN20.BF_SSN
					AND LN10.LN_SEQ = LN20.LN_SEQ
					AND LN20.LC_STA_LON20 = 'A'
					AND LN20.LC_EDS_TYP = 'M' --Coborrower
				LEFT JOIN CDW..FormatTranslation FT
					ON LN10.IC_LON_PGM = FT.[Start]
					AND FT.FmtName = '$LNPROG'
			WHERE 
				PD10.DF_SPE_ACC_ID = @AccountNumber
			ORDER BY
				LN10.LN_SEQ
		END
RETURN 0