CREATE PROCEDURE [cslsltrfed].[GetDischargeDataDetail]
	@AccountNumber VARCHAR(10),
	@IsCoborrower BIT
AS
	IF @IsCoborrower = 0
		BEGIN
			SELECT
				COALESCE(FT.Label, LN10.IC_LON_PGM) AS [Loan Program],
				CONVERT(VARCHAR(10),LN10.LD_LON_1_DSB,101) AS [First Disbursement Date],
				'$' + CAST(LN10.LA_CUR_PRI AS VARCHAR) AS [Current Principal Balance],
				LN72.LR_ITR AS [Interest Rate]
			FROM
				CDW..PD10_PRS_NME PD10
				INNER JOIN CDW..LN10_LON LN10
					ON LN10.BF_SSN = PD10.DF_PRS_ID
					AND LN10.LC_STA_LON10 = 'R'
				INNER JOIN
				(--Adding Active Interest Rate
					SELECT
						LN72.BF_SSN,
						LN72.LN_SEQ,
						LN72.LR_ITR,
						ROW_NUMBER() OVER (PARTITION BY LN72.BF_SSN, LN72.LN_SEQ ORDER BY LD_STA_LON72 DESC) AS SEQ
					FROM
						CDW..LN72_INT_RTE_HST LN72
						INNER JOIN CDW..PD10_PRS_NME PD10
							ON PD10.DF_PRS_ID = LN72.BF_SSN
					WHERE
						LN72.LC_STA_LON72 = 'A'
						AND CAST(GETDATE() AS DATE) BETWEEN LN72.LD_ITR_EFF_BEG AND LN72.LD_ITR_EFF_END
						AND PD10.DF_SPE_ACC_ID = @AccountNumber
				) LN72
					ON LN10.BF_SSN = LN72.BF_SSN
					AND LN10.LN_SEQ = LN72.LN_SEQ
					AND LN72.SEQ = 1
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
				CONVERT(VARCHAR(10),LN10.LD_LON_1_DSB,101) AS [First Disbursement Date],
				'$' + CAST(LN10.LA_CUR_PRI AS VARCHAR) AS [Current Principal Balance],
				LN72.LR_ITR AS [Interest Rate]
			FROM
				CDW..PD10_PRS_NME PD10
				INNER JOIN CDW..LN10_LON LN10
					ON LN10.BF_SSN = PD10.DF_PRS_ID
					AND LN10.LC_STA_LON10 = 'R'
				INNER JOIN
				(--Adding Active Interest Rate
					SELECT
						LN72.BF_SSN,
						LN72.LN_SEQ,
						LN72.LR_ITR,
						ROW_NUMBER() OVER (PARTITION BY LN72.BF_SSN, LN72.LN_SEQ ORDER BY LD_STA_LON72 DESC) AS SEQ
					FROM
						CDW..LN72_INT_RTE_HST LN72
						INNER JOIN CDW..PD10_PRS_NME PD10
							ON PD10.DF_PRS_ID = LN72.BF_SSN
						INNER JOIN CDW..LN20_EDS LN20
							ON LN72.BF_SSN = LN20.BF_SSN
							AND LN72.LN_SEQ = LN20.LN_SEQ
							AND LN20.LC_STA_LON20 = 'A'
							AND LN20.LC_EDS_TYP = 'M' --Coborrower
					WHERE
						LN72.LC_STA_LON72 = 'A'
						AND CAST(GETDATE() AS DATE) BETWEEN LN72.LD_ITR_EFF_BEG AND LN72.LD_ITR_EFF_END
						AND PD10.DF_SPE_ACC_ID = @AccountNumber
				) LN72
					ON LN10.BF_SSN = LN72.BF_SSN
					AND LN10.LN_SEQ = LN72.LN_SEQ
					AND LN72.SEQ = 1
				LEFT JOIN CDW..FormatTranslation FT
					ON LN10.IC_LON_PGM = FT.[Start]
					AND FT.FmtName = '$LNPROG'
			WHERE 
				PD10.DF_SPE_ACC_ID = @AccountNumber
			ORDER BY
				LN10.LN_SEQ
		END

RETURN 0