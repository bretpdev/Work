USE [UDW]
GO
/****** Object:  StoredProcedure [dbo].[spMD_GetCompassBorrowerLevelData]    Script Date: 11/13/2019 8:22:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[spMD_GetCompassBorrowerLevelData]

	@AccountIdentifier	VARCHAR(10) ,
	@IdentifierType	VARCHAR(50) 
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	 --SET NOCOUNT ON added to prevent extra result sets from
	 --interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @AccountNumber VARCHAR(10) = (SELECT DF_SPE_ACC_ID FROM PD10_PRS_NME WHERE DF_SPE_ACC_ID = @AccountIdentifier OR DF_PRS_ID = @AccountIdentifier)
	IF @AccountNumber IS NOT NULL
	BEGIN
		--get all borrower level data with exception of ACH and Employer information
		SELECT	
			PD10.DF_PRS_ID as SSN,
			PD10.DF_SPE_ACC_ID as AccountNumber,

			COALESCE(SUMM.WA_TOT_BRI_OTS, 0.00) as Interest,
			COALESCE(SUMM.COHORT, '') as CohortYear,
			COALESCE(SUMM.COHORT_IND, 'N') as CohortAlertIndicator,
			COALESCE(SUMM.LA_CUR_PRI, 0.00) as Principal,
			COALESCE(SUMM.LR_ITR_DLY, 0.00) as DailyInterest,
			COALESCE(SUMM.LR_ITR_MONTH, 0.00) as MonthlyInterestAccrual,
			COALESCE(SUMM.LR_ITR_MONTH_5, 0.00) as MonthlyInterestAccrualPlus5,
		
			COALESCE(REPAY.DUE_DAY, '') as NextDueDate,
			COALESCE(REPAY.LD_CRT_LON65, '') as LastRPSPrintDate,
			COALESCE(REPAY.MONTH_AMT, '') as MonthlyPaymentAmount,
			COALESCE(REPAY.MULT_DUE_DT, 'N') as MultipleDueDatesAlertIndicator,
			COALESCE(REPAY.DUE_DAY, '') as DueDay,

			COALESCE(DUE.CUR_DUE, 0.00) as CurrentAmountDue, 
			COALESCE(DUE.PAST_DUE, 0.00) as AmountPastDue,
			COALESCE(DUE.TOT_DUE, 0.00) as TotalAmountDue,
			COALESCE(DUE.TOT_DUE_FEE, 0.00) as TotalAmountPlusLateFees,

			COALESCE(BILL.MULT_BIL_MTD, 'N') as MultipleBillMethodAlertIndicator,
			COALESCE(BILL.BIL_MTD, '') as BillType,

			COALESCE(LPMT.LST_PMT_RCVD, '') as LastPaymentReceivedDate,
			COALESCE(ENDR.COBORROWER, 'N') as CoborrowerAlertIndicator,
			COALESCE(FORB.FORB_36, 'N') as Exceeded36MonthForbAlertIndicator,
			COALESCE(DELQ.LD_DLQ_OCC, '') as DateDelinquencyOccured,
			CAST(COALESCE(DELQ.CUR_DLQ , 0) AS INT) as DaysDelinquent,
			COALESCE(FEE.FEE_WAV_DOL, 0) as TotalLateFeesWaived,
			COALESCE(FEE.FEE_WAV_CT, 0) as TotalNumberOfTimesLateFeesHaveBeenWaived,
			COALESCE(VIPSS,'N') AS VipAlertIndicator,
			COALESCE(SPHAN,'N') AS SpecialHandlingAlertIndicator,
			COALESCE(M1411.LX_ATY, '') as Info411,
			COALESCE(DL200.DL200, 0) as NumberOf20DayLettersSent,
			COALESCE(REHAB.LD_LON_RHB_PCV, '') as Rehabilitated,
			COALESCE(REHAB.REHAB_IND, 'N') AS RehabilitatedAlertIndicator,
			COALESCE(PDISB.PendDisb, 'N') AS HasPendingDisbursement,			
			COALESCE(RM31.LA_BR_RMT_PST, 0.00) AS PaymentsInSuspense
		FROM	
			dbo.PD10_PRS_NME PD10
			LEFT JOIN 
			(
				SELECT
					PD10.DF_SPE_ACC_ID,
					CAST(SUM(CAST(SUBSTRING(COMBINED_COMMENT.LX_ATY, (CHARINDEX('$',COMBINED_COMMENT.LX_ATY)+1), (CHARINDEX('}',COMBINED_COMMENT.LX_ATY) -3)) AS MONEY)) AS DECIMAL(8,2)) AS FEE_WAV_DOL,
					COUNT(*) AS FEE_WAV_CT
				FROM
					PD10_PRS_NME PD10
					INNER JOIN
					(
						SELECT DISTINCT
							AY20.BF_SSN,
							STUFF(
							(
								SELECT 
										' ' + SUB.LX_ATY AS [text()]
								FROM 
									AY20_ATY_TXT SUB
									INNER JOIN PD10_PRS_NME PD10
										ON PD10.DF_PRS_ID = AY20.BF_SSN
								WHERE
									SUB.BF_SSN = AY20.BF_SSN
									AND SUB.LN_ATY_SEQ = AY20.LN_ATY_SEQ
									AND PD10.DF_SPE_ACC_ID = @AccountNumber
							FOR XML PATH('')
							)
							,1,1, '') AS LX_ATY
			
						FROM	
							AY10_BR_LON_ATY AY10
							INNER JOIN AY20_ATY_TXT AY20
								ON AY10.BF_SSN = AY20.BF_SSN
								AND AY10.LN_ATY_SEQ = AY20.LN_ATY_SEQ
							INNER JOIN PD10_PRS_NME PD10
								ON PD10.DF_PRS_ID = AY20.BF_SSN
						WHERE
							AY10.PF_REQ_ACT = 'DRLFA'
							AND AY20.LX_ATY like '%{WAVEFEES}%'
							AND PD10.DF_SPE_ACC_ID = @AccountNumber
					) COMBINED_COMMENT
						ON COMBINED_COMMENT.BF_SSN = PD10.DF_PRS_ID
				GROUP BY
					PD10.DF_SPE_ACC_ID
			) FEE ON
				PD10.DF_SPE_ACC_ID = FEE.DF_SPE_ACC_ID
			LEFT JOIN
			(
				SELECT 
					AY10.BF_SSN,
					CONVERT(VARCHAR(10), MAX(LD_ATY_REQ_RCV), 101) AS LAST_DL200, 
					COUNT(DISTINCT DATEDIFF(MONTH, LD_ATY_REQ_RCV, GETDATE())) AS DL200
				FROM
					AY10_BR_LON_ATY AY10
					INNER JOIN PD10_PRS_NME PD10
						ON PD10.DF_PRS_ID = AY10.BF_SSN
				WHERE
					AY10.PF_REQ_ACT = 'DL200'
					AND LD_ATY_REQ_RCV >= DATEADD(YEAR, - 3, GETDATE())
					AND PD10.DF_SPE_ACC_ID = @AccountNUmber
					AND  PF_RSP_ACT = 'PRNTD'
				GROUP BY
					AY10.BF_SSN
			) DL200 ON
				PD10.DF_PRS_ID = DL200.BF_SSN
			LEFT JOIN  
			(
				SELECT 
					AY20.BF_SSN,
					STUFF(
					(
						SELECT 
								' ' + SUB.LX_ATY AS [text()]
						FROM 
							AY20_ATY_TXT SUB
						WHERE
							SUB.BF_SSN = AY20.BF_SSN
							AND SUB.LN_ATY_SEQ = AY20.LN_ATY_SEQ
					FOR XML PATH('')
					)
					,1,1, '')
					AS LX_ATY
				FROM	
					AY20_ATY_TXT AY20
					INNER JOIN
					(
						SELECT
							AY10.BF_SSN,
							MAX(AY10.LN_ATY_SEQ) AS LN_ATY_SEQ
						FROM
							AY10_BR_LON_ATY AY10
							INNER JOIN PD10_PRS_NME PD10
								ON PD10.DF_PRS_ID = AY10.BF_SSN
						WHERE
							AY10.PF_REQ_ACT = 'M1411'
							and PD10.DF_SPE_ACC_ID = @AccountNumber
						GROUP BY
							AY10.BF_SSN
					) MAX_ARC
						ON MAX_ARC.BF_SSN = AY20.BF_SSN
						AND MAX_ARC.LN_ATY_SEQ = AY20.LN_ATY_SEQ
			) M1411
				ON M1411.BF_SSN = PD10.DF_PRS_ID
			LEFT JOIN 
			(
				SELECT	
					RM31.BF_SSN,
					SUM(RM31.LA_BR_RMT_PST) AS LA_BR_RMT_PST
				FROM
					PD10_PRS_NME PD10
					INNER JOIN RM31_BR_RMT_PST RM31
						ON RM31.BF_SSN = PD10.DF_PRS_ID
				WHERE
					PD10.DF_SPE_ACC_ID = @AccountNumber
					AND RM31.LC_RMT_STA_PST = 'S'
					AND RM31.PC_FAT_TYP = '10' 
					AND RM31.PC_FAT_SUB_TYP = '10'
				GROUP BY
					RM31.BF_SSN
			) RM31
				ON RM31.BF_SSN = PD10.DF_PRS_ID
			LEFT JOIN
			(
				SELECT DISTINCT
					LN10.BF_SSN,
					SUM(ISNULL(CUR.CUR_DUE,0)) AS CUR_DUE,
					SUM(ISNULL(PST.PAST_DUE,0)) AS PAST_DUE,
					SUM(COALESCE(CUR.CUR_DUE,0) + COALESCE(PST.PAST_DUE,0)) AS TOT_DUE,
					SUM(COALESCE(CUR.CUR_DUE,0) + COALESCE(PST.PAST_DUE,0) + COALESCE(LN10.LA_LTE_FEE_OTS,0)) AS TOT_DUE_FEE
				FROM 
					LN10_LON LN10
					INNER JOIN PD10_PRS_NME PD10
						ON PD10.DF_PRS_ID = LN10.BF_SSN
					LEFT JOIN 
					(
						SELECT
							LN80.BF_SSN,
							LN80.LN_SEQ,
							SUM(COALESCE(LN80.LA_BIL_CUR_DU,0) - COALESCE(LN80.LA_TOT_BIL_STS,0)) AS CUR_DUE
						FROM 
							LN80_LON_BIL_CRF LN80
							INNER JOIN 
							(
								SELECT
									LN80.BF_SSN,
									LN80.LN_SEQ,
									MIN(LN80.LD_BIL_DU_LON) AS LD_BIL_DU_LON
								FROM 
									LN80_LON_BIL_CRF LN80
									INNER JOIN PD10_PRS_NME PD10
										ON PD10.DF_PRS_ID = LN80.BF_SSN
								WHERE 
									LN80.LC_STA_LON80 = 'A'
									AND LN80.LC_LON_STA_BIL = '1'
									AND LN80.LD_BIL_DU_LON > GETDATE()
									AND PD10.DF_SPE_ACC_ID = @AccountNumber
								GROUP BY 
									LN80.BF_SSN,
									LN80.LN_SEQ
							) MIN_DTE
								ON LN80.BF_SSN = MIN_DTE.BF_SSN
									AND LN80.LN_SEQ = MIN_DTE.LN_SEQ
									AND LN80.LD_BIL_DU_LON = MIN_DTE.LD_BIL_DU_LON
						WHERE 
							LN80.LC_STA_LON80 = 'A'
							AND LN80.LC_LON_STA_BIL = '1'
						GROUP BY 
							LN80.BF_SSN,
							LN80.LN_SEQ
						) CUR
							ON LN10.BF_SSN = CUR.BF_SSN
							AND LN10.LN_SEQ = CUR.LN_SEQ
					LEFT JOIN 
					(
						SELECT
							LN80.BF_SSN,
							LN80.LN_SEQ,
							SUM(COALESCE(LN80.LA_BIL_CUR_DU,0) - COALESCE(LN80.LA_TOT_BIL_STS,0)) AS PAST_DUE,
							SUM(LN80.LA_BIL_CUR_DU) AS LA_BIL_CUR_DU,
							SUM(LN80.LA_TOT_BIL_STS) AS LA_TOT_BIL_STS
						FROM 
							LN80_LON_BIL_CRF LN80
						WHERE 
							LN80.LC_STA_LON80 = 'A'
							AND LN80.LC_LON_STA_BIL = '1'
							AND LN80.LD_BIL_DU_LON <= GETDATE()
						GROUP BY 
							LN80.BF_SSN,
							LN80.LN_SEQ
					) PST
						ON LN10.BF_SSN = PST.BF_SSN
						AND LN10.LN_SEQ = PST.LN_SEQ
				WHERE 
					LN10.LA_CUR_PRI > 0
					AND LN10.LC_STA_LON10 = 'R'
					AND PD10.DF_SPE_ACC_ID = @AccountNumber
				GROUP BY 
					LN10.BF_SSN
			) DUE
				ON DUE.BF_SSN = PD10.DF_PRS_ID
			LEFT JOIN
			(
				SELECT DISTINCT 
					LN10.BF_SSN, 
					SUM(LN10.LA_CUR_PRI) AS LA_CUR_PRI, 
					SUM(DW01.WA_TOT_BRI_OTS) AS WA_TOT_BRI_OTS, 
					CAST(SUM(CAST(CAST(((CASE WHEN DW01.WC_DW_LON_STA IN ('01', '02', '04') AND LN10.IC_LON_PGM IN ('SUBSPC', 'DSCON', 'DSS', 'SUBCNS', 'STFFRD') THEN 0 ELSE LN10.LA_CUR_PRI END)) * COALESCE (LN72.LR_ITR, 0) / 365 AS INTEGER) AS NUMERIC(8, 2)) / 100) AS NUMERIC(8, 2)) AS LR_ITR_DLY, 
					CAST(SUM(CAST(CAST(((CASE WHEN DW01.WC_DW_LON_STA IN ('01', '02', '04') AND LN10.IC_LON_PGM IN ('SUBSPC', 'DSCON', 'DSS', 'SUBCNS', 'STFFRD') THEN 0 ELSE LN10.LA_CUR_PRI END)) * COALESCE (LN72.LR_ITR, 0) / 365 AS INTEGER) AS NUMERIC(8, 2)) / 100) AS NUMERIC(8, 2)) * 31 AS LR_ITR_MONTH,
					CAST(CASE WHEN SUM(LN10.LA_CUR_PRI) > 0 THEN SUM(CAST(CAST(((CASE WHEN DW01.WC_DW_LON_STA IN ('01', '02', '04') AND LN10.IC_LON_PGM IN ('SUBSPC', 'DSCON', 'DSS', 'SUBCNS', 'STFFRD') THEN 0 ELSE LN10.LA_CUR_PRI END)) * COALESCE (LN72.LR_ITR, 0) / 365 AS INTEGER) AS NUMERIC(8, 2)) / 100) * 31 + 5 ELSE 0 END AS NUMERIC(8, 2)) AS LR_ITR_MONTH_5,  
					CONVERT(VARCHAR(4), YEAR(DATEADD(MONTH, 3, MAX(CAST(DW01.WD_LON_RPD_SR AS DATETIME))))) AS COHORT,
					CASE 
						WHEN YEAR(DATEADD(MONTH, 3, MAX(CAST(WD_LON_RPD_SR AS DATETIME)))) >= YEAR(DATEADD(YEAR, - 2, (DATEADD(MONTH, 3, GETDATE())))) THEN 'Y' 
						ELSE 'N' 
					END AS COHORT_IND
				FROM        
					DBO.LN10_LON LN10
					INNER JOIN PD10_PRS_NME PD10
						ON PD10.DF_PRS_ID = LN10.BF_SSN
					INNER JOIN DBO.DW01_DW_CLC_CLU DW01
						ON DW01.BF_SSN = LN10.BF_SSN AND LN10.LN_SEQ = DW01.LN_SEQ 
					LEFT JOIN
					(
						SELECT
							LN72.BF_SSN,
							LN72.LN_SEQ,
							LN72.LR_ITR,
							LN72.LR_INT_RDC_PGM_ORG,
							ROW_NUMBER() OVER (PARTITION BY LN72.BF_SSN, LN72.LN_SEQ ORDER BY LD_STA_LON72 DESC) AS SEQ
						FROM	
							LN72_INT_RTE_HST LN72
							INNER JOIN PD10_PRS_NME PD10
								ON PD10.DF_PRS_ID = LN72.BF_SSN
						WHERE
							LN72.LC_STA_LON72 = 'A'
							AND
							GETDATE() BETWEEN LN72.LD_ITR_EFF_BEG AND LN72.LD_ITR_EFF_END
							AND 
							PD10.DF_SPE_ACC_ID = @AccountNumber

						) LN72 ON LN10.BF_SSN = LN72.BF_SSN	AND LN10.LN_SEQ = LN72.LN_SEQ AND LN72.SEQ = 1
				WHERE
					PD10.DF_SPE_ACC_ID = @AccountNumber
				GROUP BY
					LN10.BF_SSN
			) SUMM
				ON SUMM.BF_SSN  = PD10.DF_PRS_ID	
			LEFT JOIN
			(
				SELECT
				   BILL.DF_SPE_ACC_ID,
				   BIL_MTD = STUFF(
										(
											SELECT 
												( ', ' + BIL_MTD  ) AS [text()]
											FROM 
											(
												SELECT DISTINCT
													LB.DF_SPE_ACC_ID,
													LB.BIL_MTD
												FROM
													LOAN_Bill LB
											) SUB
										   WHERE 
												BILL.DF_SPE_ACC_ID = SUB.DF_SPE_ACC_ID
										   ORDER BY 
												DF_SPE_ACC_ID,
												BIL_MTD
										   FOR XML PATH( '' )
										  ),1,2,''),
					MULTI_TYP.MULT_BIL_MTD
				FROM 
					LOAN_Bill BILL
					LEFT JOIN 
					(
						SELECT 
							BILL.DF_SPE_ACC_ID,
							CASE WHEN COUNT(DISTINCT BILL.BIL_MTD) > 1 THEN 'Y' ELSE 'N' END AS MULT_BIL_MTD
						FROM
							LOAN_Bill BILL
						WHERE
							DF_SPE_ACC_ID = @AccountNumber
						GROUP BY 
							BILL.DF_SPE_ACC_ID
					) MULTI_TYP
						ON BILL.DF_SPE_ACC_ID = MULTI_TYP.DF_SPE_ACC_ID
				WHERE
					BILL.DF_SPE_ACC_ID = @AccountNumber
				GROUP BY 
					BILL.DF_SPE_ACC_ID, 
					MULTI_TYP.MULT_BIL_MTD

			)BILL
				ON BILL.DF_SPE_ACC_ID = PD10.DF_SPE_ACC_ID
			LEFT JOIN
			(
				SELECT DISTINCT
					PD10.DF_SPE_ACC_ID,
					CONVERT(VARCHAR, LN16.LD_DLQ_OCC, 101) [LD_DLQ_OCC],
					(LN_DLQ_MAX+1) [CUR_DLQ]
				FROM
					LN16_LON_DLQ_HST LN16
					INNER JOIN PD10_PRS_NME PD10
						ON PD10.DF_PRS_ID = LN16.BF_SSN
				WHERE
					LN16.LC_STA_LON16 = '1'
					AND PD10.DF_SPE_ACC_ID = @AccountNumber
					AND DATEDIFF(DAY, LN16.LD_DLQ_MAX, GETDATE()) < 4 --DOES NOT PULL BORROWERS THAT ARE A DELIQUENCY TRANSFER BUT WILL PULL BORROWERS AFTER A HOLIDAY
			)DELQ
				ON DELQ.DF_SPE_ACC_ID = PD10.DF_SPE_ACC_ID
			LEFT JOIN
			(
				SELECT DISTINCT 
					PD10.DF_SPE_ACC_ID, 
					'Y' AS COBORROWER
				FROM 
					PD10_PRS_NME PD10
					INNER JOIN LN20_EDS LN20
						ON LN20.BF_SSN = PD10.DF_PRS_ID
				WHERE
					PD10.DF_SPE_ACC_ID = @AccountNumber
			)ENDR
				ON ENDR.DF_SPE_ACC_ID = PD10.DF_SPE_ACC_ID
			LEFT JOIN
			(
				SELECT DISTINCT
					PD10.DF_SPE_ACC_ID,
					CASE WHEN SUM(DATEDIFF(MONTH, LN60.LD_FOR_BEG, LN60.LD_FOR_END)) OVER(PARTITION BY PD10.DF_PRS_ID) >= 36 THEN 'Y' ELSE 'N' END FORB_36
				FROM
					PD10_PRS_NME PD10
					INNER JOIN 
					(
						SELECT DISTINCT
							LN60.BF_SSN,
							LN60.LD_FOR_BEG,
							CASE 
								WHEN LN60.LD_FOR_END > GETDATE() THEN CAST(GETDATE() AS DATE)
								ELSE LN60.LD_FOR_END
							END AS LD_FOR_END
						FROM
							FB10_BR_FOR_REQ FB10
							INNER JOIN PD10_PRS_NME PD10
								ON PD10.DF_PRS_ID = FB10.BF_SSN
							INNER JOIN LN60_BR_FOR_APV LN60
								ON LN60.BF_SSN = FB10.BF_SSN
								AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
						WHERE
							FB10.LC_FOR_STA = 'A'
							AND 
							FB10.LC_STA_FOR10 = 'A'
							AND 
							FB10.LC_FOR_TYP = '05'
							AND
							LN60.LC_STA_LON60 = 'A'
							AND 
							LN60.LC_FOR_RSP != '003' --003 = FORBEARANCE REQUEST DENIED
							AND
							LN60.LD_FOR_BEG <= GETDATE()
							AND 
							PD10.DF_SPE_ACC_ID = @AccountNumber
					)LN60
						ON LN60.BF_SSN = PD10.DF_PRS_ID
			)FORB
				ON FORB.DF_SPE_ACC_ID = PD10.DF_SPE_ACC_ID
			LEFT JOIN
			(
				SELECT
					PD10.DF_SPE_ACC_ID,
					CONVERT(VARCHAR, MAX_BORR_PAYMENT.LD_FAT_EFF, 101) AS LST_PMT_RCVD,
					CONVERT(DECIMAL(9, 2), MAX_BORR_PAYMENT.TRANS_AMT) AS TRAN_AMT,
					CONVERT(VARCHAR, MAX_IVR_PAYMENT.LD_FAT_EFF ,112) AS LST_PMT_IVR,
					CONVERT(DECIMAL(9, 2), MAX_IVR_PAYMENT.TRANS_AMT) AS LST_AMT_IVR,
					CONVERT(varchar, MAX_BORR_PAYMENT.LD_FAT_EFF, 103) AS LST_PMT_RCVD_IVR
				FROM
					PD10_PRS_NME PD10
					LEFT JOIN --GETS THE LAST PAYMENT OF TYPE 10
					(
						SELECT
							PD10.DF_SPE_ACC_ID,
							MAX(LN90.LD_FAT_EFF) OVER (PARTITION BY PD10.DF_SPE_ACC_ID) AS LD_FAT_EFF,
							SUM(ABS(ISNULL(LA_FAT_CUR_PRI,0) + ISNULL(LA_FAT_NSI,0) + ISNULL(LA_FAT_LTE_FEE,0))) OVER  (PARTITION BY PD10.DF_SPE_ACC_ID, LN90.LD_FAT_EFF) AS TRANS_AMT,
							ROW_NUMBER() OVER (PARTITION BY LN90.BF_SSN, LN90.PC_FAT_TYP ORDER BY LN90.LD_FAT_EFF DESC) [RNK]
						FROM	
							PD10_PRS_NME PD10
							INNER JOIN LN90_FIN_ATY LN90
								ON LN90.BF_SSN = PD10.DF_PRS_ID
						WHERE
							LN90.LC_STA_LON90 = 'A'
							AND LN90.LC_FAT_REV_REA = ''--NOT REVERSED
							AND LN90.PC_FAT_TYP = '10'
							AND PD10.DF_SPE_ACC_ID = @AccountNumber
					) MAX_BORR_PAYMENT
						ON MAX_BORR_PAYMENT.DF_SPE_ACC_ID = PD10.DF_SPE_ACC_ID
						AND MAX_BORR_PAYMENT.RNK = 1
					LEFT JOIN --GETS THE LAST PAYMENT OF TYPE 10 SUB TYPE 10
					(
						SELECT
							PD10.DF_SPE_ACC_ID,
							MAX(LN90.LD_FAT_EFF) OVER (PARTITION BY PD10.DF_SPE_ACC_ID) AS LD_FAT_EFF,
							SUM(ABS(ISNULL(LA_FAT_CUR_PRI,0) + ISNULL(LA_FAT_NSI,0) + ISNULL(LA_FAT_LTE_FEE,0))) OVER  (PARTITION BY PD10.DF_SPE_ACC_ID, LN90.LD_FAT_EFF) AS TRANS_AMT,
							ROW_NUMBER() OVER (PARTITION BY LN90.BF_SSN, LN90.PC_FAT_TYP ORDER BY LN90.LD_FAT_EFF DESC) [RNK]
						FROM	
							PD10_PRS_NME PD10
							INNER JOIN LN90_FIN_ATY LN90
								ON LN90.BF_SSN = PD10.DF_PRS_ID
						WHERE
							LN90.LC_STA_LON90 = 'A'
							AND LN90.LC_FAT_REV_REA = ''--NOT REVERSED
							AND LN90.PC_FAT_TYP = '10'
							AND LN90.PC_FAT_SUB_TYP = '10'
							AND PD10.DF_SPE_ACC_ID = @AccountNumber
					) MAX_IVR_PAYMENT
						ON MAX_IVR_PAYMENT.DF_SPE_ACC_ID = PD10.DF_SPE_ACC_ID
						AND MAX_IVR_PAYMENT.RNK = 1
				WHERE
					PD10.DF_SPE_ACC_ID = @AccountNumber
					AND MAX_BORR_PAYMENT.DF_SPE_ACC_ID IS NOT NULL 
			)LPMT
				ON LPMT.DF_SPE_ACC_ID = PD10.DF_SPE_ACC_ID
			LEFT JOIN 
			(
				SELECT DISTINCT
					PD10.DF_SPE_ACC_ID,
					CONVERT(VARCHAR, MAX(COALESCE(RS10.LD_SNT_RPD_DIS, RS.TermStartDate, FORB.FORB_CREATE)) OVER (PARTITION BY RS10.BF_SSN), 101) AS LD_CRT_LON65,
					CASE WHEN FORB.BF_SSN IS NOT NULL THEN FORB.DUE_DAY
					ELSE STUFF(
					(
						SELECT distinct
							', ' + CASE WHEN LTRIM(RTRIM(DW01.WX_OVR_DW_LON_STA)) = 'LITIGATION' THEN '' ELSE CAST(DAY(R.LD_RPS_1_PAY_DU) AS VARCHAR(20)) END AS [text()]
						FROM
							[calc].RepaymentSchedules R
							INNER JOIN DW01_DW_CLC_CLU DW01
								ON R.BF_SSN = DW01.BF_SSN
								AND R.LN_SEQ = DW01.LN_SEQ
						WHERE
		
							 RS.BF_SSN = R.BF_SSN
							 AND R.CurrentGradation = 1

					FOR XML PATH('')
					)
					,1,1,'')  END AS DUE_DAY,
					CASE WHEN FORB.BF_SSN IS NOT NULL THEN FORB.MONTH_AMT 
					ELSE STUFF(
					(
						SELECT distinct
							', ' + CASE WHEN LTRIM(RTRIM(DW01.WX_OVR_DW_LON_STA)) = 'LITIGATION' THEN '' ELSE '$' + CAST(SUM(R.LA_RPS_ISL) OVER (PARTITION BY DAY(R.LD_RPS_1_PAY_DU))  AS VARCHAR(20)) END AS [text()]
						FROM
							[calc].RepaymentSchedules R
							INNER JOIN DW01_DW_CLC_CLU DW01
								ON R.BF_SSN = DW01.BF_SSN
								AND R.LN_SEQ = DW01.LN_SEQ
						WHERE
							 RS.BF_SSN = R.BF_SSN
							 AND R.CurrentGradation = 1

					FOR XML PATH('')
					)
					,1,1,'') END AS MONTH_AMT,
					CASE
						WHEN RS_COUNT.BF_SSN IS NOT NULL THEN 'Y' ELSE 'N'
					END AS MULT_DUE_DT 
				FROM	
					PD10_PRS_NME PD10
					LEFT JOIN [calc].RepaymentSchedules RS
						ON PD10.DF_PRS_ID = RS.BF_SSN
					LEFT JOIN
					(
						SELECT	
							BF_SSN,
							COUNT(DISTINCT DAY(LD_RPS_1_PAY_DU)) AS DAY_COUNT
						FROM
							[calc].RepaymentSchedules RS
							INNER JOIN PD10_PRS_NME PD10
								ON PD10.DF_PRS_ID = RS.BF_SSN
						WHERE
							PD10.DF_SPE_ACC_ID = @AccountNumber
						GROUP BY
							BF_SSN
						HAVING COUNT(DISTINCT DAY(LD_RPS_1_PAY_DU)) > 1
					) RS_COUNT
						ON RS_COUNT.BF_SSN = RS.BF_SSN
					LEFT JOIN RS10_BR_RPD RS10
						ON RS10.BF_SSN = RS.BF_SSN
						AND RS10.LN_RPS_SEQ = RS.LN_RPS_SEQ
					LEFT JOIN
					(
						SELECT
							FB10.BF_SSN,
							MAX(FB10.LD_STA_FOR10) AS FORB_CREATE,
							'RPF=$' + CONVERT(VARCHAR, (SUM(FB10.LA_REQ_RDC_PAY) OVER (PARTITION BY FB10.BF_SSN, FB10.LF_FOR_CTL_NUM)), 1) AS MONTH_AMT,
							CAST(DAY(DATEADD(s, - 1, DATEADD(mm, DATEDIFF(m, 0, GETDATE()) + 1, 0))) AS VARCHAR) AS DUE_DAY
						FROM
							FB10_BR_FOR_REQ FB10
							INNER JOIN LN60_BR_FOR_APV LN60
								ON LN60.BF_SSN = FB10.BF_SSN
								AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
							INNER JOIN PD10_PRS_NME PD10
								ON PD10.DF_PRS_ID = FB10.BF_SSN
						WHERE
							FB10.LC_FOR_STA = 'A'
							AND 
							FB10.LC_STA_FOR10 = 'A'
							AND
							LN60.LC_STA_LON60 = 'A'
							AND 
							LN60.LC_FOR_RSP != '003'
							AND
							FB10.LA_REQ_RDC_PAY > 0
							AND 
							LN60.LD_FOR_BEG <= GETDATE()
							AND 
							LN60.LD_FOR_END >= GETDATE()
							AND
							PD10.DF_SPE_ACC_ID = @AccountNumber
						GROUP BY
							FB10.BF_SSN,
							FB10.LA_REQ_RDC_PAY,
							FB10.LF_FOR_CTL_NUM
					) FORB
						ON FORB.BF_SSN = pd10.DF_PRS_ID

				WHERE
					(
						(RS10.LC_STA_RPST10 = 'A' AND RS.CurrentGradation = 1)
						 OR 
						 FORB.BF_SSN IS NOT NULL
					)
					AND PD10.DF_SPE_ACC_ID = @AccountNumber
			
			)REPAY
				ON REPAY.DF_SPE_ACC_ID = PD10.DF_SPE_ACC_ID
			LEFT JOIN
			(
				SELECT     
					PD10.DF_SPE_ACC_ID, 
					CONVERT(VARCHAR(10), MAX(LD_LON_RHB_PCV), 101) AS LD_LON_RHB_PCV, 
					'Y' AS REHAB_IND
				FROM
					dbo.LN09_RPD_PIO_CVN LN09
					INNER JOIN PD10_PRS_NME PD10
						ON PD10.DF_PRS_ID = LN09.BF_SSN
				WHERE
					LD_LON_RHB_PCV IS NOT NULL
					AND PD10.DF_SPE_ACC_ID = @AccountNumber
				GROUP BY 
					PD10.DF_SPE_ACC_ID
			)REHAB
				ON REHAB.DF_SPE_ACC_ID = PD10.DF_SPE_ACC_ID
			LEFT JOIN
			(
				SELECT DISTINCT
					PD10.DF_SPE_ACC_ID,
					CASE WHEN AY10.PF_REQ_ACT = 'VIPSS' THEN 'Y' ELSE 'N' END AS VIPSS,
					CASE WHEN AY10.PF_REQ_ACT = 'SPHAN' THEN 'Y' ELSE 'N' END AS SPHAN
				FROM
					PD10_PRS_NME PD10
					INNER JOIN AY10_BR_LON_ATY AY10
						ON AY10.BF_SSN = PD10.DF_PRS_ID
				WHERE
					AY10.PF_REQ_ACT IN ('VIPSS','SPHAN')
					AND AY10.LC_STA_ACTY10 = 'A'
					AND PD10.DF_SPE_ACC_ID = @AccountNumber
			) VIPSS
				ON VIPSS.DF_SPE_ACC_ID = PD10.DF_SPE_ACC_ID
			LEFT JOIN
			(
				SELECT DISTINCT 
					PD10.DF_SPE_ACC_ID, 
					'Y' AS PendDisb
				FROM 
					PD10_PRS_NME PD10
					INNER JOIN  dbo.LN15_DSB LN15
						ON PD10.DF_PRS_ID = LN15.BF_SSN
				WHERE
					 (LC_DSB_TYP = '1')
					 AND ISNULL(LC_DSB_CAN_TYP,'') = ''
					 AND LC_STA_LON15 = 'A'
					 AND PD10.DF_SPE_ACC_ID = @AccountNumber

			)PDISB
				ON PDISB.DF_SPE_ACC_ID = PD10.DF_SPE_ACC_ID
		WHERE
				PD10.DF_SPE_ACC_ID = @AccountNumber
	END
END