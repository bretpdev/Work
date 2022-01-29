CREATE PROCEDURE [dbo].[LT_US06CTQRT_Loans]
	@LetterId VARCHAR(10),
	@BF_SSN VARCHAR(9),
	@IsCoborrower BIT = 0,
	@RN_ATY_SEQ_PRC INT
AS
BEGIN

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @PF_REQ_ACT VARCHAR(5) = (SELECT AC11.PF_REQ_ACT FROM AC11_ACT_REQ_LTR AC11 WHERE PF_LTR = @LetterId)

IF @IsCoborrower = 0
	BEGIN
		SELECT
			ISNULL(LN10.IC_LON_PGM, '') AS [Loan Program],
			CASE 
				WHEN (SUBSTRING(OW10.IF_OWN, 1, 6) IN ('828476', '826717','830248','829769','971357')) THEN 'UHEAA'
				WHEN OW10.IF_OWN IN ('900749') THEN 'Complete Student Loans'
				ELSE dbo.ToProperCase(OW10.IM_OWN_FUL) 
			END AS [Current Owner],
			LN10.LN_SEQ AS [Loan Sequence],
			CONVERT(VARCHAR, LN10.LD_LON_1_DSB, 101) AS [Date Disbursed],
			CONVERT(VARCHAR(10), LN72.LR_ITR, 0) + '%' AS [Current Interest Rate],
			CASE 
				WHEN ISNULL(DW01.WX_OVR_DW_LON_STA, '') != '' THEN dbo.ToProperCase(DW01.WX_OVR_DW_LON_STA)
				ELSE dbo.ToProperCase(FT_LS.Label)
			END AS [Current Loan Status],
			CASE WHEN DW01.WC_DW_LON_STA IN ('03','06','07','08','09','10','11','13','14','15','16','18','20') -- In repayment
				THEN
					CASE WHEN LN80.LD_BIL_DU_LON IS NOT NULL AND DATEADD(DAY, 30, GETDATE()) < LN80.LD_BIL_DU_LON THEN
						CASE WHEN RS.GraduatedPlan = 1 
							THEN --graduated plan calculation for paid ahead
								CASE WHEN ISNULL(RS.TermsRemaining, 0) - DATEDIFF(MONTH, DATEADD(MONTH, RS.GraduatedGradationMonths, RS.TermStartDate), LN80.LD_BIL_DU_LON) - ISNULL(LN80.Satisfied,0) > 0
									THEN CAST(ISNULL(RS.TermsRemaining, 0) - DATEDIFF(MONTH, DATEADD(MONTH, RS.GraduatedGradationMonths, RS.TermStartDate), LN80.LD_BIL_DU_LON) - ISNULL(LN80.Satisfied,0) AS VARCHAR(10))
									ELSE ''
								END
							ELSE --non graduated plan calculation for paid ahead						
								CASE WHEN ISNULL(RS.TermsRemaining, 0) - DATEDIFF(MONTH, DATEADD(MONTH, RS.GradationMonths, RS.TermStartDate), LN80.LD_BIL_DU_LON) - ISNULL(LN80.Satisfied,0) > 0
									THEN CAST(ISNULL(RS.TermsRemaining, 0) - DATEDIFF(MONTH, DATEADD(MONTH, RS.GradationMonths, RS.TermStartDate), LN80.LD_BIL_DU_LON) - ISNULL(LN80.Satisfied,0) AS VARCHAR(10))
									ELSE ''
								END
						END
					ELSE --in repayment, not paid ahead
						CASE WHEN ISNULL(RS.TermsRemaining, 0) - ISNULL(LN80.Satisfied,0) > 0
							THEN CAST(ISNULL(RS.TermsRemaining, 0) - ISNULL(LN80.Satisfied,0) AS VARCHAR(10))
							ELSE ''
						END
					END
			ELSE '' -- the borrower is not in repayment
			END AS [Repay Term in Months],
			CASE WHEN  DW01.WC_DW_LON_STA IN ('03','06','07','08','09','10','11','13','14','15','16','18','20')
				THEN '$' + CONVERT(VARCHAR(15), ISNULL(RS.LA_RPS_ISL, 0), 1) 
				ELSE ''
			END AS [Installment Amount],
			'$' + CONVERT(VARCHAR(15), ISNULL(LN10.LA_CUR_PRI, 0), 1) AS [Current Principal],
			'$' + CONVERT(VARCHAR(15), CASE WHEN ISNULL(LN90_Paid.PrincipalPaid, 0) <= 0 THEN 0 ELSE LN90_Paid.PrincipalPaid END, 1) AS [Principal Paid Since Last Statement],
			'$' + CONVERT(VARCHAR(15), ISNULL(DW01.WA_TOT_BRI_OTS, 0), 1) AS [Current Outstanding Interest],
			'$' + CONVERT(VARCHAR(15), CASE WHEN ISNULL(LN90_Paid.InterestPaid, 0) <= 0 THEN 0 ELSE LN90_Paid.InterestPaid END, 1) AS [Interest Paid Since Last Statement],
			'$' + CONVERT(VARCHAR(15), ISNULL(LN10.LA_LTE_FEE_OTS, 0), 1) AS [Late Fees]
		FROM 
			LN10_LON LN10
			INNER JOIN
			(
				SELECT
					LN85.BF_SSN,
					LN85.LN_SEQ
				FROM
					LN85_LON_ATY LN85
					INNER JOIN AY10_BR_LON_ATY AY10
						ON AY10.BF_SSN = LN85.BF_SSN
						AND AY10.LN_ATY_SEQ = LN85.LN_ATY_SEQ
				WHERE   
					AY10.PF_REQ_ACT = @PF_REQ_ACT
					AND AY10.BF_SSN = @BF_SSN
					AND AY10.LN_ATY_SEQ = @RN_ATY_SEQ_PRC
					--Active flag ignored, as LT20 provides the exact record that is tied to this request
			)LN85
				ON LN85.BF_SSN = LN10.BF_SSN
				AND LN85.LN_SEQ = LN10.LN_SEQ 
			INNER JOIN 
			(
				SELECT
					LN72.BF_SSN,
					LN72.LN_SEQ,
					LN72.LR_INT_RDC_PGM_ORG,
					LN72.LC_ITR_TYP,
					LN72.LR_ITR,
					ROW_NUMBER() OVER (PARTITION BY LN72.BF_SSN, LN72.LN_SEQ ORDER BY LD_STA_LON72 DESC) AS SEQ
				FROM
					LN72_INT_RTE_HST LN72
				WHERE
					LN72.LC_STA_LON72 = 'A'
					AND	CAST(GETDATE() AS DATE) BETWEEN LN72.LD_ITR_EFF_BEG AND LN72.LD_ITR_EFF_END
					AND LN72.BF_SSN = @BF_SSN
			) LN72
				ON LN10.LN_SEQ = LN72.LN_SEQ
				AND LN10.BF_SSN = LN72.BF_SSN
				AND LN72.SEQ = 1
			INNER JOIN DW01_DW_CLC_CLU DW01
				ON LN10.BF_SSN = DW01.BF_SSN
				AND LN10.LN_SEQ = DW01.LN_SEQ
			INNER JOIN OW10_OWN OW10
				ON LN10.LF_LON_CUR_OWN = OW10.IF_OWN
			LEFT JOIN
			(
				SELECT
					LN90.BF_SSN,
					LN90.LN_SEQ,
					ABS(SUM(LN90.LA_FAT_CUR_PRI)) AS [PrincipalPaid],
					ABS(SUM(LN90.LA_FAT_NSI)) AS [InterestPaid]
				FROM 
					LN90_FIN_ATY LN90
					INNER JOIN
					(
						SELECT DISTINCT
							LN85.BF_SSN,
							LN85.LN_SEQ,
							MAX(AY10.LD_ATY_REQ_RCV) AS MaxDate,
							CASE WHEN MIN(AY10.LD_ATY_REQ_RCV) = MAX(AY10.LD_ATY_REQ_RCV) THEN '1900-01-01' ELSE MIN(AY10.LD_ATY_REQ_RCV) END AS MinDate
						FROM
							LN85_LON_ATY LN85
							INNER JOIN --GETS THE MOST RECENT 2 ARCs LEFT ON THE BORROWERS ACCOUNT TO GET THE LOAN SEQ'S THE LETTER APPLIES TO
							(
								SELECT TOP 2
									AY10.BF_SSN,
									AY10.LN_ATY_SEQ,
									AY10.LD_ATY_REQ_RCV
								FROM 
									AY10_BR_LON_ATY AY10
								WHERE
									AY10.PF_REQ_ACT = @PF_REQ_ACT
									AND AY10.BF_SSN = @BF_SSN
									AND AY10.LC_STA_ACTY10 = 'A'
								ORDER BY
									AY10.LD_ATY_REQ_RCV
								DESC
							)AY10
								ON AY10.BF_SSN = LN85.BF_SSN
								AND AY10.LN_ATY_SEQ = LN85.LN_ATY_SEQ
						GROUP BY
							LN85.BF_SSN,
							LN85.LN_SEQ
					)LN85
						ON LN85.BF_SSN = LN90.BF_SSN
						AND LN85.LN_SEQ = LN90.LN_SEQ
				WHERE
					LN90.PC_FAT_TYP = '10'
					AND LN90.PC_FAT_SUB_TYP = '10'
					AND ISNULL(LN90.LC_FAT_REV_REA,'') = ''
					AND LN90.LC_STA_LON90 = 'A'
					AND CAST(LN90.LD_FAT_EFF AS DATE) BETWEEN CAST(LN85.MinDate AS DATE) AND CAST(LN85.MaxDate AS DATE)
				GROUP BY
					LN90.BF_SSN,
					LN90.LN_SEQ
			) LN90_Paid
				ON LN90_Paid.BF_SSN = LN10.BF_SSN
				AND LN90_Paid.LN_SEQ = LN10.LN_SEQ
			LEFT JOIN 
			(
				SELECT
					SUM(RS.LN_RPS_TRM) - SUM(CASE WHEN RS.CurrentGradation = 1 AND RS.GradationMonths > 0 THEN RS.GradationMonths ELSE 0 END) AS TermsRemaining,
					SUM(CASE WHEN RS.CurrentGradation = 1 THEN RS.LA_RPS_ISL ELSE 0 END) AS LA_RPS_ISL,
					MAX(CASE WHEN RS.CurrentGradation = 1 THEN RS.TermStartDate ELSE CAST('1900-1-1' AS DATE) END) AS TermStartDate,
					MAX(CASE WHEN RS.CurrentGradation = 1 AND RS.GradationMonths > 0 THEN RS.GradationMonths ELSE 0 END) AS GradationMonths,
					MAX(
						CASE WHEN RS.CurrentGradation = 1 
							THEN CASE WHEN RS.LC_TYP_SCH_DIS IN ('G' , 'EG', 'S2', 'S5') THEN 1 ELSE 0 END
							ELSE 0
						END
					) AS GraduatedPlan,
					CASE WHEN SUM(CASE WHEN RS.CurrentGradation = 1 AND RS.GradationMonths > 0 THEN RS.GradationMonths ELSE 0 END) - SUM(CASE WHEN RS.TermStartDate < RS_Cur.TermStartDate THEN RS.LN_RPS_TRM ELSE 0 END) > 0 
						THEN SUM(CASE WHEN RS.CurrentGradation = 1 AND RS.GradationMonths > 0 THEN RS.GradationMonths ELSE 0 END) - SUM(CASE WHEN RS.TermStartDate < RS_Cur.TermStartDate THEN RS.LN_RPS_TRM ELSE 0 END)
						ELSE 0
					END AS GraduatedGradationMonths,
					RS.BF_SSN,
					RS.LN_SEQ
				FROM
					calc.RepaymentSchedules RS
					INNER JOIN calc.RepaymentSchedules RS_Cur
						ON RS.BF_SSN = RS_Cur.BF_SSN
						AND RS.LN_SEQ = RS_Cur.LN_SEQ
						AND RS_Cur.CurrentGradation = 1
				GROUP BY
					RS.BF_SSN,
					RS.LN_SEQ
			)	RS
				ON LN10.BF_SSN = RS.BF_SSN
				AND LN10.LN_SEQ = RS.LN_SEQ
			LEFT JOIN 
			(
				SELECT	
					LN80.BF_SSN,
					LN80.LN_SEQ,
					MAX(CASE WHEN LN80.LA_TOT_BIL_STS = LN80.LA_BIL_CUR_DU THEN 1 ELSE 0 END) AS Satisfied,
					MAX(LN80.LD_BIL_DU_LON) AS LD_BIL_DU_LON
				FROM
					LN80_LON_BIL_CRF LN80
					INNER JOIN
					(
						SELECT	
							LN80.BF_SSN,
							LN80.LN_SEQ,
							MAX(LN80.LD_BIL_DU_LON) AS MAX_LD_BIL_DU_LON,
							MAX(LN80.LN_BIL_OCC_SEQ) AS MAX_LN_BIL_OCC_SEQ
						FROM
							LN80_LON_BIL_CRF LN80
						WHERE
							LN80.LC_STA_LON80 = 'A'
							AND LN80.LC_BIL_TYP_LON = 'P'
						GROUP BY
							LN80.BF_SSN,
							LN80.LN_SEQ
					)LN80_MAX
						ON LN80_MAX.BF_SSN = LN80.BF_SSN
						AND LN80_MAX.LN_SEQ = LN80.LN_SEQ
						AND LN80_MAX.MAX_LD_BIL_DU_LON = LN80.LD_BIL_DU_LON
						AND LN80_MAX.MAX_LN_BIL_OCC_SEQ = LN80.LN_BIL_OCC_SEQ
				WHERE
					LN80.LC_STA_LON80 = 'A'
					AND LN80.LC_BIL_TYP_LON = 'P'
				GROUP BY
					LN80.BF_SSN,
					LN80.LN_SEQ
			)LN80
				ON LN80.BF_SSN = LN10.BF_SSN
				AND LN80.LN_SEQ = LN10.LN_SEQ
			LEFT JOIN FormatTranslation FT_LS
				ON DW01.WC_DW_LON_STA = FT_LS.Start
				AND FT_LS.FmtName = '$LONSTA'
			LEFT JOIN 
			(
				SELECT
					LN35.BF_SSN,
					LN35.LN_SEQ,
					LN35.IF_OWN,
					LN35.IF_BND_ISS
				FROM
					LN35_LON_OWN LN35
					INNER JOIN
					(
						SELECT
							LN35.BF_SSN,
							LN35.LN_SEQ,
							LN35.IF_OWN,
							MAX(LN35.LN_LON_OWN_SEQ) AS LN_LON_OWN_SEQ
						FROM
							LN35_LON_OWN LN35
						WHERE
							LN35.LC_STA_LON35 = 'A'
						GROUP BY
							LN35.BF_SSN,
							LN35.LN_SEQ,
							LN35.IF_OWN
					) LN35_MAX
						ON LN35.BF_SSN = LN35_MAX.BF_SSN
						AND LN35.LN_SEQ = LN35_MAX.LN_SEQ
						AND LN35.IF_OWN = LN35_MAX.IF_OWN
						AND LN35.LN_LON_OWN_SEQ = LN35_MAX.LN_LON_OWN_SEQ
				WHERE
					LN35.LC_STA_LON35 = 'A'					
			) LN35
				--We should not expect duplication on LN10 because we take the max owner sequence and join LN10 on current owner
				ON LN10.BF_SSN = LN35.BF_SSN
				AND LN10.LN_SEQ = LN35.LN_SEQ
				AND LN10.LF_LON_CUR_OWN = LN35.IF_OWN
		WHERE 
			LN10.BF_SSN = @BF_SSN
			AND LN10.LA_CUR_PRI > 0.00
			AND LN10.LC_STA_LON10 = 'R'
		ORDER BY
			LN10.LN_SEQ
	END
ELSE
	BEGIN
		SELECT
			ISNULL(LN10.IC_LON_PGM, '') AS [Loan Program],
			CASE 
				WHEN (SUBSTRING(OW10.IF_OWN, 1, 6) IN ('828476', '826717','830248','829769','971357')) THEN 'UHEAA'
				WHEN OW10.IF_OWN IN ('900749') THEN 'Complete Student Loans'
				ELSE dbo.ToProperCase(OW10.IM_OWN_FUL) 
			END AS [Current Owner],
			LN10.LN_SEQ AS [Loan Sequence],
			CONVERT(VARCHAR, LN10.LD_LON_1_DSB, 101) AS [Date Disbursed],
			CONVERT(VARCHAR(10), LN72.LR_ITR, 0) + '%' AS [Current Interest Rate],
			CASE 
				WHEN ISNULL(DW01.WX_OVR_DW_LON_STA, '') != '' THEN dbo.ToProperCase(DW01.WX_OVR_DW_LON_STA)
				ELSE dbo.ToProperCase(FT_LS.Label)
			END AS [Current Loan Status],
			CASE WHEN DW01.WC_DW_LON_STA IN ('03','06','07','08','09','10','11','13','14','15','16','18','20') -- In repayment
				THEN
					CASE WHEN LN80.LD_BIL_DU_LON IS NOT NULL AND DATEADD(DAY, 30, GETDATE()) < LN80.LD_BIL_DU_LON THEN
						CASE WHEN RS.GraduatedPlan = 1 
							THEN --graduated plan calculation for paid ahead
								CASE WHEN ISNULL(RS.TermsRemaining, 0) - DATEDIFF(MONTH, DATEADD(MONTH, RS.GraduatedGradationMonths, RS.TermStartDate), LN80.LD_BIL_DU_LON) - ISNULL(LN80.Satisfied,0) > 0
									THEN CAST(ISNULL(RS.TermsRemaining, 0) - DATEDIFF(MONTH, DATEADD(MONTH, RS.GraduatedGradationMonths, RS.TermStartDate), LN80.LD_BIL_DU_LON) - ISNULL(LN80.Satisfied,0) AS VARCHAR(10))
									ELSE ''
								END
							ELSE --non graduated plan calculation for paid ahead						
								CASE WHEN ISNULL(RS.TermsRemaining, 0) - DATEDIFF(MONTH, DATEADD(MONTH, RS.GradationMonths, RS.TermStartDate), LN80.LD_BIL_DU_LON) - ISNULL(LN80.Satisfied,0) > 0
									THEN CAST(ISNULL(RS.TermsRemaining, 0) - DATEDIFF(MONTH, DATEADD(MONTH, RS.GradationMonths, RS.TermStartDate), LN80.LD_BIL_DU_LON) - ISNULL(LN80.Satisfied,0) AS VARCHAR(10))
									ELSE ''
								END
						END
					ELSE --in repayment, not paid ahead
						CASE WHEN ISNULL(RS.TermsRemaining, 0) - ISNULL(LN80.Satisfied,0) > 0
							THEN CAST(ISNULL(RS.TermsRemaining, 0) - ISNULL(LN80.Satisfied,0) AS VARCHAR(10))
							ELSE ''
						END
					END
			ELSE '' -- the borrower is not in repayment
			END AS [Repay Term in Months],
			CASE WHEN  DW01.WC_DW_LON_STA IN ('03','06','07','08','09','10','11','13','14','15','16','18','20')
				THEN '$' + CONVERT(VARCHAR(15), ISNULL(RS.LA_RPS_ISL, 0), 1) 
				ELSE ''
			END AS [Installment Amount],
			'$' + CONVERT(VARCHAR(15), ISNULL(LN10.LA_CUR_PRI, 0), 1) AS [Current Principal],
			'$' + CONVERT(VARCHAR(15), CASE WHEN ISNULL(LN90_Paid.PrincipalPaid, 0) <= 0 THEN 0 ELSE LN90_Paid.PrincipalPaid END, 1) AS [Principal Paid Since Last Statement],
			'$' + CONVERT(VARCHAR(15), ISNULL(DW01.WA_TOT_BRI_OTS, 0), 1) AS [Current Outstanding Interest],
			'$' + CONVERT(VARCHAR(15), CASE WHEN ISNULL(LN90_Paid.InterestPaid, 0) <= 0 THEN 0 ELSE LN90_Paid.InterestPaid END, 1) AS [Interest Paid Since Last Statement],
			'$' + CONVERT(VARCHAR(15), ISNULL(LN10.LA_LTE_FEE_OTS, 0), 1) AS [Late Fees]
		FROM 
			LN10_LON LN10
			INNER JOIN LN20_EDS LN20
				ON LN20.BF_SSN = LN10.BF_SSN
				AND LN20.LN_SEQ = LN10.LN_SEQ
				AND LN20.LC_STA_LON20 = 'A'
				AND LN20.LC_EDS_TYP = 'M'
			INNER JOIN
			(
				SELECT
					LN85.BF_SSN,
					LN85.LN_SEQ
				FROM
					LN85_LON_ATY LN85
					INNER JOIN AY10_BR_LON_ATY AY10
						ON AY10.BF_SSN = LN85.BF_SSN
						AND AY10.LN_ATY_SEQ = LN85.LN_ATY_SEQ
					INNER JOIN LN20_EDS LN20
						ON LN20.BF_SSN = LN85.BF_SSN
						AND LN20.LN_SEQ = LN85.LN_SEQ
						AND LN20.LC_STA_LON20 = 'A'
						AND LN20.LC_EDS_TYP = 'M'
				WHERE   
					AY10.PF_REQ_ACT = @PF_REQ_ACT
					AND LN20.LF_EDS = @BF_SSN
					AND AY10.LN_ATY_SEQ = @RN_ATY_SEQ_PRC
					--Active flag ignored, as LT20 provides the exact record that is tied to this request
			)LN85
				ON LN85.BF_SSN = LN10.BF_SSN
				AND LN85.LN_SEQ = LN10.LN_SEQ 
			INNER JOIN 
			(
				SELECT
					LN72.BF_SSN,
					LN72.LN_SEQ,
					LN72.LR_INT_RDC_PGM_ORG,
					LN72.LC_ITR_TYP,
					LN72.LR_ITR,
					ROW_NUMBER() OVER (PARTITION BY LN72.BF_SSN, LN72.LN_SEQ ORDER BY LD_STA_LON72 DESC) AS SEQ
				FROM
					LN72_INT_RTE_HST LN72
					INNER JOIN LN20_EDS LN20
						ON LN20.BF_SSN = LN72.BF_SSN
						AND LN20.LN_SEQ = LN72.LN_SEQ
						AND LN20.LC_STA_LON20 = 'A'
						AND LN20.LC_EDS_TYP = 'M'
				WHERE
					LN72.LC_STA_LON72 = 'A'
					AND	CAST(GETDATE() AS DATE) BETWEEN LN72.LD_ITR_EFF_BEG AND LN72.LD_ITR_EFF_END
					AND	LN20.LF_EDS = @BF_SSN
			) LN72
				ON LN10.LN_SEQ = LN72.LN_SEQ
				AND LN10.BF_SSN = LN72.BF_SSN
				AND LN72.SEQ = 1
			INNER JOIN DW01_DW_CLC_CLU DW01
				ON LN10.BF_SSN = DW01.BF_SSN
				AND LN10.LN_SEQ = DW01.LN_SEQ
			INNER JOIN OW10_OWN OW10
				ON LN10.LF_LON_CUR_OWN = OW10.IF_OWN
			LEFT JOIN
			(
				SELECT
					LN90.BF_SSN,
					LN90.LN_SEQ,
					ABS(SUM(LN90.LA_FAT_CUR_PRI)) AS [PrincipalPaid],
					ABS(SUM(LN90.LA_FAT_NSI)) AS [InterestPaid]
				FROM 
					LN90_FIN_ATY LN90
					INNER JOIN
					(
						SELECT DISTINCT
							LN85.BF_SSN,
							LN85.LN_SEQ,
							MAX(AY10.LD_ATY_REQ_RCV) AS MaxDate,
							CASE WHEN MIN(AY10.LD_ATY_REQ_RCV) = MAX(AY10.LD_ATY_REQ_RCV) THEN '1900-01-01' ELSE MIN(AY10.LD_ATY_REQ_RCV) END AS MinDate
						FROM
							LN85_LON_ATY LN85
							INNER JOIN --GETS THE MOST RECENT 2 ARCs LEFT ON THE BORROWERS ACCOUNT TO GET THE LOAN SEQ'S THE LETTER APPLIES TO
							(
								SELECT TOP 2
									AY10.BF_SSN,
									AY10.LN_ATY_SEQ,
									AY10.LD_ATY_REQ_RCV
								FROM 
									AY10_BR_LON_ATY AY10
									INNER JOIN --Preventing duplication by loan
									(
										SELECT DISTINCT	
											LN20.BF_SSN,
											LN20.LF_EDS,
											LN20.LC_STA_LON20,
											LN20.LC_EDS_TYP
										FROM
											LN20_EDS LN20
										WHERE
											LN20.LF_EDS = @BF_SSN
									) LN20
										ON LN20.BF_SSN = AY10.BF_SSN
										AND LN20.LC_STA_LON20 = 'A'
										AND LN20.LC_EDS_TYP = 'M'
								WHERE
									AY10.PF_REQ_ACT = @PF_REQ_ACT
									AND LN20.LF_EDS = @BF_SSN
									AND AY10.LC_STA_ACTY10 = 'A'
								ORDER BY
									AY10.LD_ATY_REQ_RCV
								DESC
							)AY10
								ON AY10.BF_SSN = LN85.BF_SSN
								AND AY10.LN_ATY_SEQ = LN85.LN_ATY_SEQ
						GROUP BY
							LN85.BF_SSN,
							LN85.LN_SEQ
					)LN85
						ON LN85.BF_SSN = LN90.BF_SSN
						AND LN85.LN_SEQ = LN90.LN_SEQ
				WHERE
					LN90.PC_FAT_TYP = '10'
					AND LN90.PC_FAT_SUB_TYP = '10'
					AND ISNULL(LN90.LC_FAT_REV_REA,'') = ''
					AND LN90.LC_STA_LON90 = 'A'
					AND CAST(LN90.LD_FAT_EFF AS DATE) BETWEEN CAST(LN85.MinDate AS DATE) AND CAST(LN85.MaxDate AS DATE) --MAX AY10 date
				GROUP BY
					LN90.BF_SSN,
					LN90.LN_SEQ
			) LN90_Paid
				ON LN90_Paid.BF_SSN = LN10.BF_SSN
				AND LN90_Paid.LN_SEQ = LN10.LN_SEQ
			LEFT JOIN 
			(
				SELECT
					SUM(RS.LN_RPS_TRM) - SUM(CASE WHEN RS.CurrentGradation = 1 AND RS.GradationMonths > 0 THEN RS.GradationMonths ELSE 0 END) AS TermsRemaining,
					SUM(CASE WHEN RS.CurrentGradation = 1 THEN RS.LA_RPS_ISL ELSE 0 END) AS LA_RPS_ISL,
					MAX(CASE WHEN RS.CurrentGradation = 1 THEN RS.TermStartDate ELSE CAST('1900-1-1' AS DATE) END) AS TermStartDate,
					MAX(CASE WHEN RS.CurrentGradation = 1 AND RS.GradationMonths > 0 THEN RS.GradationMonths ELSE 0 END) AS GradationMonths,
					MAX(
						CASE WHEN RS.CurrentGradation = 1 
							THEN CASE WHEN RS.LC_TYP_SCH_DIS IN ('G' , 'EG', 'S2', 'S5') THEN 1 ELSE 0 END
							ELSE 0
						END
					) AS GraduatedPlan,
					CASE WHEN SUM(CASE WHEN RS.CurrentGradation = 1 AND RS.GradationMonths > 0 THEN RS.GradationMonths ELSE 0 END) - SUM(CASE WHEN RS.TermStartDate < RS_Cur.TermStartDate THEN RS.LN_RPS_TRM ELSE 0 END) > 0 
						THEN SUM(CASE WHEN RS.CurrentGradation = 1 AND RS.GradationMonths > 0 THEN RS.GradationMonths ELSE 0 END) - SUM(CASE WHEN RS.TermStartDate < RS_Cur.TermStartDate THEN RS.LN_RPS_TRM ELSE 0 END)
						ELSE 0
					END AS GraduatedGradationMonths,
					RS.BF_SSN,
					RS.LN_SEQ
				FROM
					calc.RepaymentSchedules RS
					INNER JOIN calc.RepaymentSchedules RS_Cur
						ON RS.BF_SSN = RS_Cur.BF_SSN
						AND RS.LN_SEQ = RS_Cur.LN_SEQ
						AND RS_Cur.CurrentGradation = 1
				GROUP BY
					RS.BF_SSN,
					RS.LN_SEQ
			)	RS
				ON LN10.BF_SSN = RS.BF_SSN
				AND LN10.LN_SEQ = RS.LN_SEQ
			LEFT JOIN 
			(
				SELECT	
					LN80.BF_SSN,
					LN80.LN_SEQ,
					MAX(CASE WHEN LN80.LA_TOT_BIL_STS = LN80.LA_BIL_CUR_DU THEN 1 ELSE 0 END) AS Satisfied,
					MAX(LN80.LD_BIL_DU_LON) AS LD_BIL_DU_LON
				FROM
					LN80_LON_BIL_CRF LN80
					INNER JOIN
					(
						SELECT	
							LN80.BF_SSN,
							LN80.LN_SEQ,
							MAX(LN80.LD_BIL_DU_LON) AS MAX_LD_BIL_DU_LON,
							MAX(LN80.LN_BIL_OCC_SEQ) AS MAX_LN_BIL_OCC_SEQ
						FROM
							LN80_LON_BIL_CRF LN80
						WHERE
							LN80.LC_STA_LON80 = 'A'
							AND LN80.LC_BIL_TYP_LON = 'P'
						GROUP BY
							LN80.BF_SSN,
							LN80.LN_SEQ
					)LN80_MAX
						ON LN80_MAX.BF_SSN = LN80.BF_SSN
						AND LN80_MAX.LN_SEQ = LN80.LN_SEQ
						AND LN80_MAX.MAX_LD_BIL_DU_LON = LN80.LD_BIL_DU_LON
						AND LN80_MAX.MAX_LN_BIL_OCC_SEQ = LN80.LN_BIL_OCC_SEQ
				WHERE
					LN80.LC_STA_LON80 = 'A'
					AND LN80.LC_BIL_TYP_LON = 'P'
				GROUP BY
					LN80.BF_SSN,
					LN80.LN_SEQ
			)LN80
				ON LN80.BF_SSN = LN10.BF_SSN
				AND LN80.LN_SEQ = LN10.LN_SEQ
			LEFT JOIN FormatTranslation FT_LS
				ON DW01.WC_DW_LON_STA = FT_LS.Start
				AND FT_LS.FmtName = '$LONSTA'
			LEFT JOIN 
			(
				SELECT
					LN35.BF_SSN,
					LN35.LN_SEQ,
					LN35.IF_OWN,
					LN35.IF_BND_ISS
				FROM
					LN35_LON_OWN LN35
					INNER JOIN
					(
						SELECT
							LN35.BF_SSN,
							LN35.LN_SEQ,
							LN35.IF_OWN,
							MAX(LN35.LN_LON_OWN_SEQ) AS LN_LON_OWN_SEQ
						FROM
							LN35_LON_OWN LN35
						WHERE
							LN35.LC_STA_LON35 = 'A'
						GROUP BY
							LN35.BF_SSN,
							LN35.LN_SEQ,
							LN35.IF_OWN
					) LN35_MAX
						ON LN35.BF_SSN = LN35_MAX.BF_SSN
						AND LN35.LN_SEQ = LN35_MAX.LN_SEQ
						AND LN35.IF_OWN = LN35_MAX.IF_OWN
						AND LN35.LN_LON_OWN_SEQ = LN35_MAX.LN_LON_OWN_SEQ
				WHERE
					LN35.LC_STA_LON35 = 'A'					
			) LN35
				--We should not expect duplication on LN10 because we take the max owner sequence and join LN10 on current owner
				ON LN10.BF_SSN = LN35.BF_SSN
				AND LN10.LN_SEQ = LN35.LN_SEQ
				AND LN10.LF_LON_CUR_OWN = LN35.IF_OWN
		WHERE 
			LN20.LF_EDS = @BF_SSN
			AND LN10.LA_CUR_PRI > 0.00
			AND LN10.LC_STA_LON10 = 'R'
		ORDER BY
			LN10.LN_SEQ
	END
END