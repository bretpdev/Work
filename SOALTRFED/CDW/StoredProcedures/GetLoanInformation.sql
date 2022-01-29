CREATE PROCEDURE [soaltrfed].[GetLoanInformation]
	@AccountNumber VARCHAR(10),
	@IsCoborrower BIT = 0,
	@BorrowerSSN VARCHAR(9)
AS
BEGIN

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

IF @IsCoborrower = 0
	BEGIN
		SELECT 
			LN10.LN_SEQ AS [Loan Seq],
			COALESCE(FMT.Label, LN10.IC_LON_PGM) AS [Loan Program],
			CONVERT(VARCHAR,LN10.LD_LON_1_DSB,101) AS [1st Disb Date],
			CASE WHEN LN90Summary.PC_FAT_TYP = '02' THEN ABS(COALESCE(LN90Summary.LA_FAT_CUR_PRI, 0))
				 WHEN LN90Summary.PC_FAT_TYP = '01' THEN ABS(COALESCE(LN90Summary.DisbSum, 0))
				 ELSE ABS(COALESCE(LN10.LA_LON_AMT_GTR, 0))
			END AS [Principal Balance at Transfer],
			COALESCE(LN10.LA_CUR_PRI,0) AS [Current Principal Balance]
		FROM	
			PD10_PRS_NME PD10
			INNER JOIN LN10_LON LN10
				ON LN10.BF_SSN = PD10.DF_PRS_ID
			LEFT JOIN
			(
				SELECT
					LN90.BF_SSN,
					LN90.LN_SEQ,
					PrinBal.DisbSum,
					LN90.LA_FAT_CUR_PRI,
					LN90.LD_FAT_EFF,
					LN90.LD_FAT_APL,
					LN90.PC_FAT_TYP
				FROM
					LN90_FIN_ATY LN90
					INNER JOIN
					(
						SELECT
							LN90.BF_SSN,
							LN90.LN_SEQ,
							MAX(LN15.Dsb) AS DisbSum,
							MIN(CAST(LN90.LD_FAT_EFF AS DATE)) AS MIN_LD_FAT_EFF,
							MIN(CAST(LN90.LD_FAT_APL AS DATE)) AS MIN_LD_FAT_APL
						FROM
							LN90_FIN_ATY LN90
							LEFT JOIN 
							(
								SELECT
									BF_SSN,
									LN_SEQ,
									SUM(COALESCE(LA_DSB,0) - COALESCE(LA_DSB_CAN,0)) AS Dsb
								FROM
									LN15_DSB LN15
								WHERE
									LN15.LC_STA_LON15 = '1'
									AND LN15.LC_DSB_TYP = '2' --actual disbursement
								GROUP BY
									LN15.BF_SSN,
									LN15.LN_SEQ
							) LN15
								ON LN15.BF_SSN = LN90.BF_SSN
								AND LN15.LN_SEQ = LN90.LN_SEQ
						
						WHERE
							(LN90.PC_FAT_TYP = '02'
							OR 
							(
								LN90.PC_FAT_SUB_TYP = '01' 
								AND LN90.PC_FAT_TYP = '01'
							))
							AND LN90.LC_STA_LON90 ='A'
							AND COALESCE(LN90.LC_FAT_REV_REA,'') = ''
						GROUP BY 
							LN90.BF_SSN,
							LN90.LN_SEQ
					) PrinBal
						ON LN90.BF_SSN = PrinBal.BF_SSN
						AND LN90.LN_SEQ = PrinBal.LN_SEQ
						AND CAST(LN90.LD_FAT_APL AS DATE) = PrinBal.MIN_LD_FAT_APL
						AND CAST(LN90.LD_FAT_EFF AS DATE) = PrinBal.MIN_LD_FAT_EFF
					WHERE
						(LN90.PC_FAT_TYP = '02'
						OR 
						(
							LN90.PC_FAT_SUB_TYP = '01' 
							AND LN90.PC_FAT_TYP = '01'
						))
						AND LN90.LC_STA_LON90 ='A'
						AND COALESCE(LN90.LC_FAT_REV_REA,'') = ''
				) LN90Summary
					ON LN90Summary.BF_SSN = LN10.BF_SSN
					AND LN90Summary.LN_SEQ = LN10.LN_SEQ	
			LEFT JOIN FormatTranslation FMT
				ON FMT.Start = LN10.IC_LON_PGM
				AND FMT.FmtName = '$LNPROG'
		WHERE 
			PD10.DF_SPE_ACC_ID = @AccountNumber
	END
ELSE
	BEGIN
		SELECT 
			LN10.LN_SEQ AS [Loan Seq],
			COALESCE(FMT.Label, LN10.IC_LON_PGM) AS [Loan Program],
			CONVERT(VARCHAR,LD_LON_1_DSB,101) AS [1st Disb Date],
			CASE WHEN LN90Summary.PC_FAT_TYP = '02' THEN ABS(COALESCE(LN90Summary.LA_FAT_CUR_PRI, 0))
			     WHEN LN90Summary.PC_FAT_TYP = '01' THEN ABS(COALESCE(LN90Summary.DisbSum, 0))
				 ELSE ABS(COALESCE(LN10.LA_LON_AMT_GTR, 0))
			END AS [Principal Balance at Transfer],
			COALESCE(LN10.LA_CUR_PRI,0) AS [Current Principal Balance]
		FROM	
			PD10_PRS_NME PD10
			INNER JOIN LN20_EDS LN20
				ON LN20.LF_EDS = PD10.DF_PRS_ID
				AND LN20.LC_EDS_TYP = 'M'
				AND LN20.LC_STA_LON20 = 'A'
				AND LN20.BF_SSN = @BorrowerSSN
			INNER JOIN LN10_LON LN10
				ON LN10.BF_SSN = LN20.BF_SSN
				AND LN10.LN_SEQ = LN20.LN_SEQ
			LEFT JOIN
			(
				SELECT
					LN90.BF_SSN,
					LN90.LN_SEQ,
					PrinBal.DisbSum,
					LN90.LA_FAT_CUR_PRI,
					LN90.LD_FAT_EFF,
					LN90.LD_FAT_APL,
					LN90.PC_FAT_TYP
				FROM
					LN90_FIN_ATY LN90
					INNER JOIN
					(
						SELECT
							LN90.BF_SSN,
							LN90.LN_SEQ,
							MAX(LN15.Dsb) AS DisbSum,
							MIN(CAST(LN90.LD_FAT_EFF AS DATE)) AS MIN_LD_FAT_EFF,
							MIN(CAST(LN90.LD_FAT_APL AS DATE)) AS MIN_LD_FAT_APL
						FROM
							LN90_FIN_ATY LN90
							LEFT JOIN 
							(
								SELECT
									BF_SSN,
									LN_SEQ,
									SUM(COALESCE(LA_DSB,0) - COALESCE(LA_DSB_CAN,0)) AS Dsb
								FROM
									LN15_DSB LN15
								WHERE
									LN15.LC_STA_LON15 = '1'
									AND LN15.LC_DSB_TYP = '2' --actual disbursement
								GROUP BY
									LN15.BF_SSN,
									LN15.LN_SEQ
							) LN15
								ON LN15.BF_SSN = LN90.BF_SSN
								AND LN15.LN_SEQ = LN90.LN_SEQ
						
						WHERE
							(LN90.PC_FAT_TYP = '02'
							OR 
							(
								LN90.PC_FAT_SUB_TYP = '01' 
								AND LN90.PC_FAT_TYP = '01'
							))
							AND LN90.LC_STA_LON90 ='A'
							AND COALESCE(LN90.LC_FAT_REV_REA,'') = ''
						GROUP BY 
							LN90.BF_SSN,
							LN90.LN_SEQ
					) PrinBal
						ON LN90.BF_SSN = PrinBal.BF_SSN
						AND LN90.LN_SEQ = PrinBal.LN_SEQ
						AND CAST(LN90.LD_FAT_APL AS DATE) = PrinBal.MIN_LD_FAT_APL
						AND CAST(LN90.LD_FAT_EFF AS DATE) = PrinBal.MIN_LD_FAT_EFF
					WHERE
						(LN90.PC_FAT_TYP = '02'
						OR 
						(
							LN90.PC_FAT_SUB_TYP = '01' 
							AND LN90.PC_FAT_TYP = '01'
						))
						AND LN90.LC_STA_LON90 ='A'
						AND COALESCE(LN90.LC_FAT_REV_REA,'') = ''
				) LN90Summary
					ON LN90Summary.BF_SSN = LN10.BF_SSN
					AND LN90Summary.LN_SEQ = LN10.LN_SEQ
			LEFT JOIN FormatTranslation FMT
				ON FMT.Start = LN10.IC_LON_PGM
				AND FMT.FmtName = '$LNPROG'
		WHERE 
			PD10.DF_SPE_ACC_ID = @AccountNumber
	END
END