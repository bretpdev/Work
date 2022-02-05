SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @8DAYSAGO DATE = DATEADD(DAY, - 8, GETDATE()) --TODO:  restore this line if modifed for testing

DROP TABLE IF EXISTS #BIL_FINAL

SELECT
	--included here so value is determined by RFILE in addition to account number
	DENSE_RANK() OVER(ORDER BY BOR_ACC_ID, RFILE, LD_BIL_CRT) AS BOR_CNT,
	CentralData.DBO.CreateScanLine
	(			
		BF_SSN,
		LD_BIL_CRT,
		RIGHT('00' + CONVERT(VARCHAR,LN_SEQ_BIL_WI_DTE),2), --2-char BL10.LN_SEQ_BIL_WI_DTE with leading zeros
		RIGHT('00000000' + REPLACE(CONVERT(VARCHAR,SUM(LA_BIL_DU_PRT_LN) OVER (PARTITION BY BOR_ACC_ID, RFILE)),'.',''),8), --8-char LA_BIL_DU_PRT_LN with leading zeros and no comma or decimal
		BOR_ACC_ID
	) AS SCANLN, 
	MAX(LN_DLQ_MAX) OVER (PARTITION BY BOR_ACC_ID, RFILE, LD_BIL_CRT) AS ACCT_LN_DLQ_MAX,
	SUM(RPS_PMT_LOAN) OVER (PARTITION BY BOR_ACC_ID, RFILE, LD_BIL_CRT) AS RPS_PMT_ACCT,
	SUM(LA_BIL_PAS_DU_LN) OVER (PARTITION BY BOR_ACC_ID, RFILE, LD_BIL_CRT) AS LA_BIL_PAS_DU,
	SUM(LA_TOT_INT_DU_LN) OVER (PARTITION BY BOR_ACC_ID, RFILE, LD_BIL_CRT) AS LA_TOT_INT_DU,
	SUM(LA_BIL_DU_PRT_LN) OVER (PARTITION BY BOR_ACC_ID, RFILE, LD_BIL_CRT) AS LA_BIL_DU_PRT,
	SUM(LA_CUR_INT_DU_LN) OVER (PARTITION BY BOR_ACC_ID, RFILE, LD_BIL_CRT) AS LA_CUR_INT_DU,
	SUM(WA_TOT_BRI_OTS_LN) OVER (PARTITION BY BOR_ACC_ID, RFILE, LD_BIL_CRT) AS WA_TOT_BRI_OTS,
	CAST(0.00 AS DECIMAL(18,2)) AS ACCT_LA_FAT_CUR_PRI, --placeholder for value calculated in UPDATE below
	CAST(0.00 AS DECIMAL(18,2)) AS ACCT_LA_FAT_NSI, --placeholder for value calculated in UPDATE below
	CAST(0.00 AS DECIMAL(18,2)) AS ACCT_LA_FAT_LTE_FEE, --placeholder for value calculated in UPDATE below
	CAST(0.00 AS DECIMAL(18,2)) AS ACCT_TAP, --placeholder for value calculated in UPDATE below
	SUM(LA_AGG_PRI) OVER (PARTITION BY BOR_ACC_ID, RFILE, LD_BIL_CRT)  AS ACCT_LA_AGG_PRI,
	SUM(LA_AGG_INT) OVER (PARTITION BY BOR_ACC_ID, RFILE, LD_BIL_CRT)  AS ACCT_LA_AGG_INT,
	SUM(LA_AGG_TOT) OVER (PARTITION BY BOR_ACC_ID, RFILE, LD_BIL_CRT)  AS ACCT_LA_AGG_TOT,
	SUM(AGG_TOT_PRN_BAL_LN) OVER (PARTITION BY BOR_ACC_ID, RFILE, LD_BIL_CRT)  AS AGG_TOT_PRN_BAL,
	SUM(LA_AGG_FEE) OVER (PARTITION BY BOR_ACC_ID, RFILE, LD_BIL_CRT)  AS ACCT_LA_AGG_FEE,
	*,
	CAST(0.00 AS DECIMAL(18,2)) AS NEXT_PMT_DUE, --placeholder for value calculated in UPDATE below
	CAST(NULL AS DATE) AS NEXT_PMT_DUE_DATE --placeholder for value calculated in UPDATE below
	INTO #BIL_FINAL --TODO: uncomment for production
FROM 
	(--BIL_FINAL
		SELECT DISTINCT
			CentralData.DBO.CreateACSKeyline(BL10.BF_SSN,'B',PD30.DC_ADR) AS ACSKEY,
			PD10.DF_SPE_ACC_ID AS BOR_ACC_ID,
			BL10.BF_SSN,
			LN10.LN_SEQ,
			PD10.DM_PRS_1 AS DM_PRS_1,
			PD10.DM_PRS_MID,
			PD10.DM_PRS_LST,
			PD30.DX_STR_ADR_1,
			PD30.DX_STR_ADR_2,
			PD30.DM_CT,
			PD30.DC_DOM_ST,
			PD30.DF_ZIP_CDE,
			PD30.DM_FGN_CNY,
			LN10.IC_LON_PGM,
			LNTYP.Label AS LN_TYPE_DESC,
			LN10.LD_LON_1_DSB,
			LN80.LR_INT_BIL,
			LN15.ORG_PRI,
			COALESCE(LN10.LA_CUR_PRI,0) + COALESCE(DW01.WA_TOT_BRI_OTS,0) + COALESCE(LN10.LA_LTE_FEE_OTS,0) AS LA_CUR_PRN_BIL,
			CASE
				WHEN DW01.WC_DW_LON_STA IN ('04', '05') THEN LN16.LN_DLQ_MAX
				ELSE LN16.LN_DLQ_MAX + 1 
			END AS LN_DLQ_MAX,
			BL10.LD_BIL_CRT,
			LastPayment.LD_FAT_EFF AS LD_FAT_EFF,  --date of most recent payment during the last billing cycle
			--the folowing NULL amounts are not actually needed for the printed bill
			NULL AS LA_FAT_CUR_PRI,
			NULL AS LA_FAT_NSI,
			NULL AS TAP,
			BL10.LD_BIL_DU,
			CASE 
				WHEN BL10.LC_BIL_TYP = 'C' AND LC_IND_BIL_SNT = 'T' THEN 0 
				ELSE COALESCE(LN80.LA_BIL_PAS_DU,0) 
			END AS LA_BIL_PAS_DU_LN,
			CASE
				WHEN BL10.LC_BIL_TYP = 'C' AND BL10.LC_IND_BIL_SNT = 'T' THEN COALESCE(LN80.LA_BIL_DU_PRT,0) + COALESCE(LN80.LA_LTE_FEE_OTS_PRT,0)
				ELSE                    COALESCE(LN80.LA_BIL_PAS_DU,0) + COALESCE(LN80.LA_BIL_DU_PRT,0) + COALESCE(LN80.LA_LTE_FEE_OTS_PRT,0)
			END AS LA_TOT_INT_DU_LN,
			CASE
				WHEN BL10.LC_BIL_TYP = 'C' AND BL10.LC_IND_BIL_SNT = 'T' THEN COALESCE(LN80.LA_BIL_DU_PRT,0)
				ELSE                    COALESCE(LN80.LA_BIL_PAS_DU,0) + COALESCE(LN80.LA_BIL_DU_PRT,0) 
			END AS LA_BIL_DU_PRT_LN,
			COALESCE(LN80.LA_BIL_DU_PRT,0) AS LA_CUR_INT_DU_LN,
			' ' AS LF_EDS, --hold over from when the SAS picked up both borrowers and endorsers
			0 AS IS_EDR, --hold over from when the SAS picked up both borrowers and endorsers
			PD30.DC_DOM_ST AS STATE_IND,
			BL10.LC_IND_BIL_SNT,
			BL10.LC_BIL_TYP,
			LN10.LD_LON_ACL_ADD,
			BL10.LN_SEQ_BIL_WI_DTE,
			LN10.LC_LON_SND_CHC,
			COALESCE(LN80.LA_PRI_PD_2DT_BIL,0) AS LA_AGG_PRI,
			COALESCE(LN80.LA_INT_PD_2DT_BIL,0) AS LA_AGG_INT,
			COALESCE(LN80.LA_FEE_PD_2DT_BIL,0) AS LA_AGG_FEE,
			COALESCE(LN80.LA_PRI_PD_2DT_BIL,0) + COALESCE(LN80.LA_INT_PD_2DT_BIL,0) + COALESCE(LN80.LA_FEE_PD_2DT_BIL,0) AS LA_AGG_TOT,
			COALESCE(DW01.WA_TOT_BRI_OTS, DW01.LA_NSI_OTS, 0) AS WA_TOT_BRI_OTS_LN,
			COALESCE(BL10.LA_PRI_PD_LST_STM,0) AS ACCT_LA_FAT_CUR_PRI_LN,
			COALESCE(BL10.LA_INT_PD_LST_STM,0) AS ACCT_LA_FAT_NSI_LN,
			COALESCE(BL10.LA_FEE_PD_LST_STM,0) AS ACCT_LA_FAT_LTE_FEE_LN,
			COALESCE(BL10.LA_PRI_PD_LST_STM,0) + COALESCE(BL10.LA_INT_PD_LST_STM,0)  + COALESCE(BL10.LA_FEE_PD_LST_STM,0) AS ACCT_TAP_LN,
			COALESCE(LN10.LA_CUR_PRI,0) AS AGG_TOT_PRN_BAL_LN,
			LTRIM(RTRIM(PD30.DM_CT)) + ', ' + LTRIM(RTRIM(PD30.DC_DOM_ST)) + ' ' + LTRIM(RTRIM(PD30.DF_ZIP_CDE)) AS CITYSTATEZIP,
			LN80.LD_NXT_PAY_DUE_AHD,
			CAST(LN80.LA_NXT_TOT_DUE_AHD AS DECIMAL(18,2)) AS LA_NXT_TOT_DUE_AHD, 
			CASE
				WHEN BR30.BF_SSN IS NOT NULL 
				THEN 1
				ELSE 0
			END AS ON_ACH,
			CASE
				WHEN PH05.DF_SPE_ID IS NOT NULL 
				THEN 1
				ELSE 0
			END AS ON_ECORR,
			CASE --RFILE
				WHEN DATENAME(dw,GETDATE())  = 'Monday' AND (  DATEDIFF(DAY,LN10.LD_LON_ACL_ADD,GETDATE()) = 3 OR DATEDIFF(DAY,LN10.LD_LON_ACL_ADD,GETDATE()) = 2 ) THEN 'R9' --today is Monday and loan was added on Friday or Saturday
				WHEN DATENAME(dw,GETDATE()) != 'Monday' AND DATEDIFF(DAY,LN10.LD_LON_ACL_ADD,GETDATE()) = 1 THEN 'R9'
				WHEN BL10.LC_IND_BIL_SNT IN ('1','2','4','7','F','R') AND BL10.LC_BIL_TYP = 'P'
					THEN 
						CASE 
							WHEN BR30.BF_SSN IS NOT NULL THEN 'R3' --ON_ACH
							WHEN BL10.LC_BIL_TYP = 'C' AND BL10.LC_IND_BIL_SNT = 'T' AND LN80.LD_NXT_PAY_DUE_AHD IS NOT NULL AND COALESCE(LN80.LA_BIL_DU_PRT,0) < .01 THEN  'R22' --LA_BIL_DU_PRT < .01
							WHEN LN80.LD_NXT_PAY_DUE_AHD IS NOT NULL AND COALESCE(LN80.LA_BIL_DU_PRT,0) + COALESCE(LN80.LA_BIL_PAS_DU,0) < .01 THEN  'R22' --LA_BIL_DU_PRT < .01
							WHEN MAX(COALESCE(LN16BB.LN_DLQ_MAX,0)) OVER (PARTITION BY LN16BB.BF_SSN) <  1 /*AND LN16_BRW.BF_SSN IS NULL*/ THEN 'R2'
							WHEN MAX(COALESCE(LN16BB.LN_DLQ_MAX,0)) OVER (PARTITION BY LN16BB.BF_SSN) >= 1 /*OR LN16_BRW.BF_SSN IS NOT NULL*/ THEN 'R4'
						END
				WHEN BL10.LC_IND_BIL_SNT IN ('G', 'I', 'H') AND BL10.LC_BIL_TYP = 'P' THEN 'R3' --borrowers are on ACH but the bills were not sent due to insufficient time
				WHEN BL10.LC_IND_BIL_SNT = 'T' AND BL10.LC_BIL_TYP = 'C' THEN 'R5'
				WHEN BL10.LC_BIL_TYP = 'I' THEN 'R16'
			END AS RFILE,
			MAX(COALESCE(LN16.LN_DLQ_MAX,0)) OVER (PARTITION BY LN16.BF_SSN) as md,
			TERMS_REMAINING.RPS_TRMS_REM,
			LN80.LA_BIL_CUR_DU AS RPS_PMT_LOAN		

		FROM 
			CDW..BL10_BR_BIL BL10
			INNER JOIN CDW..LN80_LON_BIL_CRF LN80
				ON BL10.BF_SSN = LN80.BF_SSN
				AND BL10.LD_BIL_CRT = LN80.LD_BIL_CRT
				AND BL10.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE

			INNER JOIN CDW..LN10_LON LN10
				ON LN80.BF_SSN = LN10.BF_SSN
				AND LN80.LN_SEQ = LN10.LN_SEQ
			INNER JOIN CDW..DW01_DW_CLC_CLU DW01
				ON DW01.BF_SSN = LN10.BF_SSN
				AND DW01.LN_SEQ = LN10.LN_SEQ
			INNER JOIN CDW..PD10_PRS_NME PD10
				ON BL10.BF_SSN = PD10.DF_PRS_ID
			INNER JOIN /*rank addresses to get most useful one*/
			(	
				SELECT
					*,
					ROW_NUMBER() OVER (PARTITION BY DF_PRS_ID ORDER BY PriorityNumber) [AddressPriority]
				FROM
				(
					SELECT
						PD30.DF_PRS_ID,
						PD30.DX_STR_ADR_1,
						PD30.DX_STR_ADR_2,
						PD30.DM_CT,
						PD30.DC_DOM_ST,
						PD30.DF_ZIP_CDE,
						PD30.DM_FGN_CNY,
						PD30.DM_FGN_ST,
						PD30.DC_ADR,
						CASE
							WHEN PD30.DC_ADR = 'B' THEN 1
							WHEN PD30.DC_ADR = 'L' THEN 2
							WHEN PD30.DC_ADR = 'D' THEN 3
						END AS [PriorityNumber]
					FROM
						CDW..PD30_PRS_ADR PD30
					WHERE
						PD30.DI_VLD_ADR = 'Y'
				) Addr
			) PD30
				ON PD30.DF_PRS_ID = PD10.DF_PRS_ID
				AND PD30.AddressPriority = 1 --highest priority address only
			LEFT JOIN 
				(-- TERMS_REMAINING
					SELECT
						RPS.BF_SSN,
						RPS.LN_SEQ,
						RPS.TermsToDate - RPS.GradationMonths AS RPS_TRMS_REM
					FROM
						(
							SELECT 
								RS.BF_SSN,
								RS.LN_SEQ,
								MAX(TermsToDate) AS MX_TRMS_TO_DATE
							FROM
								CDW.calc.RepaymentSchedules RS
							GROUP BY
								RS.BF_SSN,
								RS.LN_SEQ
						) MaxTermsToDate
						INNER JOIN CDW.calc.RepaymentSchedules RPS
							ON MaxTermsToDate.BF_SSN = RPS.BF_SSN
							AND MaxTermsToDate.LN_SEQ = RPS.LN_SEQ
							AND MaxTermsToDate.MX_TRMS_TO_DATE = RPS.TermsToDate 
				
				) TERMS_REMAINING		
					ON BL10.BF_SSN = TERMS_REMAINING.BF_SSN
					AND LN10.LN_SEQ = TERMS_REMAINING.LN_SEQ
			LEFT JOIN CDW..PH05_CNC_EML PH05 
				ON PD10.DF_SPE_ACC_ID = PH05.DF_SPE_ID
				AND PH05.DI_VLD_CNC_EML_ADR = 'Y' -- valid email
				AND PH05.DI_CNC_EBL_OPI = 'Y' -- opted in to ebill
			LEFT JOIN 
			( --MAX_BARC
				SELECT
					BF_SSN,
					MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
				FROM
					CDW..AY10_BR_LON_ATY 
				WHERE 
					PF_REQ_ACT IN ('BILLS','BILLC')
					AND LC_STA_ACTY10 = 'A'
				GROUP BY 
					BF_SSN
			) MAX_BARC -- most recent active billing actitity record
				ON BL10.BF_SSN = MAX_BARC.BF_SSN
			LEFT JOIN 
			(
				SELECT
					LN16.BF_SSN,
					LN16.LN_SEQ,
					MAX(LN16.LN_DLQ_MAX) AS LN_DLQ_MAX
				FROM
					CDW..LN16_LON_DLQ_HST LN16 --loan level delinquency
					INNER JOIN CDW..BL10_BR_BIL BL10
						ON BL10.BF_SSN = LN16.BF_SSN
					INNER JOIN CDW..LN80_LON_BIL_CRF LN80
						ON BL10.BF_SSN = LN80.BF_SSN
						AND BL10.LD_BIL_CRT = LN80.LD_BIL_CRT
						AND BL10.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
						AND LN16.LN_SEQ = LN80.LN_SEQ
					INNER JOIN CDW..LN10_LON LN10
						ON LN10.BF_SSN = LN80.BF_SSN
						AND LN10.LN_SEQ = LN80.LN_SEQ 
				WHERE
					LN16.LC_STA_LON16 = '1'
					AND BL10.LC_STA_BIL10 = 'A'
					AND LN80.LC_STA_LON80 = 'A'
					AND LN10.LA_CUR_PRI > 0
					AND LN10.LC_STA_LON10 = 'R'
					AND BL10.LD_BIL_CRT >= @8DAYSAGO
					AND BL10.LC_IND_BIL_SNT NOT IN ('N','9','0','5','6','')
				GROUP BY
					LN16.BF_SSN,
					LN16.LN_SEQ
			)LN16
				ON LN10.BF_SSN = LN16.BF_SSN
				AND LN10.LN_SEQ = LN16.LN_SEQ
			LEFT JOIN 
			(
				SELECT
					LN16.BF_SSN,
					MAX(LN16.LN_DLQ_MAX) AS LN_DLQ_MAX
				FROM
					CDW..LN16_LON_DLQ_HST LN16 --loan level delinquency
					INNER JOIN CDW..BL10_BR_BIL BL10
						ON BL10.BF_SSN = LN16.BF_SSN
					INNER JOIN CDW..LN80_LON_BIL_CRF LN80
						ON BL10.BF_SSN = LN80.BF_SSN
						AND BL10.LD_BIL_CRT = LN80.LD_BIL_CRT
						AND BL10.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
						AND LN16.LN_SEQ = LN80.LN_SEQ
					INNER JOIN CDW..LN10_LON LN10
						ON LN10.BF_SSN = LN80.BF_SSN
						AND LN10.LN_SEQ = LN80.LN_SEQ 
				WHERE
					LN16.LC_STA_LON16 = '1'
					AND BL10.LC_STA_BIL10 = 'A'
					AND LN80.LC_STA_LON80 = 'A'
					AND LN10.LA_CUR_PRI > 0
					AND LN10.LC_STA_LON10 = 'R'
					AND BL10.LD_BIL_CRT >= @8DAYSAGO
					AND BL10.LC_IND_BIL_SNT NOT IN ('N','9','0','5','6','')
				GROUP BY
					LN16.BF_SSN,
					LN16.LN_SEQ
			)LN16BB
				ON LN10.BF_SSN = LN16BB.BF_SSN
			LEFT JOIN 
			(
				SELECT
					BF_SSN,
					LN_SEQ,
					SUM(LA_DSB) - SUM(COALESCE(LA_DSB_CAN,0)) AS ORG_PRI
				FROM
					CDW..LN15_DSB 
				GROUP BY
					BF_SSN,
					LN_SEQ
			) LN15
				ON LN10.BF_SSN = LN15.BF_SSN
				AND LN10.LN_SEQ = LN15.LN_SEQ
			LEFT JOIN
			( --BR30
				SELECT
					BR30.BF_SSN,
					LN83.LN_SEQ,
					MAX(BR30.BN_EFT_SEQ) AS BN_EFT_SEQ
				FROM
					CDW..BR30_BR_EFT BR30
					INNER JOIN CDW..LN83_EFT_TO_LON LN83
						ON LN83.BF_SSN = BR30.BF_SSN 
						AND LN83.BN_EFT_SEQ = BR30.BN_EFT_SEQ
				WHERE
					BR30.BC_EFT_STA = 'A'
				GROUP BY
					BR30.BF_SSN,
					LN83.LN_SEQ
			) BR30
				ON BR30.BF_SSN = LN80.BF_SSN
				AND BR30.LN_SEQ = LN80.LN_SEQ
			LEFT JOIN
			( --LastPayment (most recent effective date during last billing cycle)
				SELECT DISTINCT
					LN90.BF_SSN,
					LN90.LN_SEQ,
					MAX(LN90.LD_FAT_EFF) OVER (PARTITION BY LN90.BF_SSN) AS LD_FAT_EFF
				FROM
					CDW..LN90_FIN_ATY LN90
					INNER JOIN 
					(--begin and end of billing cycle for each loan
						SELECT
							POP.BF_SSN,
							POP.LN_SEQ,
							MAX(CASE WHEN POP.RN = 2 THEN POP.LD_BIL_CRT END) AS BEGIN_RANGE, --begin of billing cycle
							MAX(CASE WHEN POP.RN = 1 THEN POP.LD_BIL_CRT END) AS END_RANGE -- end of billing cycle
			
						FROM
						( --ordered list of bill create dates from which most recent and next most recent create dates are selected as begin and end dates of billing cycle
							SELECT 
								BILLS.BF_SSN, 
								BILLS.LD_BIL_CRT,
								BILLS.LN_SEQ,
								ROW_NUMBER() OVER(PARTITION BY BILLS.BF_SSN, BILLS.LN_SEQ ORDER BY BILLS.LD_BIL_CRT DESC) AS RN
							FROM 
							( --list of bills for target population
								SELECT DISTINCT
									BL10.BF_SSN,
									POP.LN_SEQ,
									BL10.LD_BIL_CRT
								FROM
									CDW..BL10_BR_BIL BL10
									INNER JOIN CDW..LN80_LON_BIL_CRF LN80
										ON BL10.BF_SSN = LN80.BF_SSN
										AND BL10.LD_BIL_CRT = LN80.LD_BIL_CRT
										AND BL10.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
									INNER JOIN
									( --target population
										SELECT
											BL10.BF_SSN,
											LN80.LN_SEQ
										FROM
											CDW..BL10_BR_BIL BL10
											INNER JOIN CDW..LN80_LON_BIL_CRF LN80
												ON BL10.BF_SSN = LN80.BF_SSN
												AND BL10.LD_BIL_CRT = LN80.LD_BIL_CRT
												AND BL10.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
										WHERE 
											BL10.LC_STA_BIL10 = 'A'
											AND LN80.LC_STA_LON80 = 'A'
											AND BL10.LD_BIL_CRT >= @8DAYSAGO
											AND BL10.LC_IND_BIL_SNT NOT IN ('N','9','0','5','6','')
									) POP
										ON POP.BF_SSN = BL10.BF_SSN
										AND POP.LN_SEQ = LN80.LN_SEQ
								WHERE
									BL10.LC_STA_BIL10 = 'A'
									AND BL10.LD_BIL_CRT <= CAST(GETDATE() AS DATE)
									AND BL10.LC_IND_BIL_SNT NOT IN ('N','9','0','5','6','')
							) BILLS
						) POP
							WHERE POP.RN IN (1,2) --get most recent and next most recent bill create dates
						GROUP BY 
							POP.BF_SSN,
							POP.LN_SEQ

					) EffDates
						ON LN90.BF_SSN = EffDates.BF_SSN
						AND LN90.LN_SEQ = EffDates.LN_SEQ
				WHERE
					LN90.LD_FAT_EFF BETWEEN EffDates.BEGIN_RANGE AND EffDates.END_RANGE --payments during billing cycle
					AND LN90.LC_STA_LON90 ='A'
					AND COALESCE(LN90.LC_FAT_REV_REA,'') = ''
					AND LN90.PC_FAT_TYP = '10'
					AND LN90.PC_FAT_SUB_TYP IN ('10','11','12','15','35','41')
			) LastPayment
					ON LastPayment.BF_SSN = LN10.BF_SSN
					AND LastPayment.LN_SEQ = LN10.LN_SEQ
			LEFT JOIN CDW..FormatTranslation LNTYP --get loan type description for the loan type
				ON LN10.IC_LON_PGM = LNTYP.[Start]
				AND LNTYP.FmtName = '$LNPROG'
		WHERE 
			BL10.LC_STA_BIL10 = 'A'
			AND LN80.LC_STA_LON80 = 'A'
			AND LN10.LA_CUR_PRI > 0
			AND LN10.LC_STA_LON10 = 'R'
			AND BL10.LD_BIL_CRT >= @8DAYSAGO
			AND COALESCE(MAX_BARC.LD_ATY_REQ_RCV,@8DAYSAGO) < BL10.LD_BIL_CRT --TODO:  restore this line if commented out for testing
			AND BL10.LC_IND_BIL_SNT NOT IN ('N','9','0','5','6','')
			--AND PD10.DF_SPE_ACC_ID = '' --TODO:  for testing, comment out for prod
			--AND LN80.BF_SSN = '632601540' --TODO:  for testing, comment out for prod
	) AS BIL_FINAL


UPDATE
	BF1
 SET 
	BF1.NEXT_PMT_DUE = CAST(M.LA_NXT_TOT_DUE_AHD AS DECIMAL(18,2)),
	BF1.NEXT_PMT_DUE_DATE = CAST(M.LD_NXT_PAY_DUE_AHD AS DATE)
FROM
	#BIL_FINAL BF1
	INNER JOIN
	(
		SELECT
			BF.BOR_ACC_ID,
			BF.RFILE,
			BF.LD_NXT_PAY_DUE_AHD,
			SUM(CAST(LA_NXT_TOT_DUE_AHD AS DECIMAL(18,2))) AS LA_NXT_TOT_DUE_AHD
		FROM
			#BIL_FINAL BF
			INNER JOIN 
			(
				SELECT
					BOR_ACC_ID,
					RFILE,
					CAST(MIN(LD_NXT_PAY_DUE_AHD) AS DATE) AS LD_NXT_PAY_DUE_AHD
				FROM
					#BIL_FINAL
				GROUP BY
					BOR_ACC_ID,
					RFILE
			) M
				ON M.BOR_ACC_ID = BF.BOR_ACC_ID
				AND M.RFILE = BF.RFILE
				AND M.LD_NXT_PAY_DUE_AHD = BF.LD_NXT_PAY_DUE_AHD
		GROUP BY
			BF.BOR_ACC_ID,
			BF.RFILE,
			BF.LD_NXT_PAY_DUE_AHD
	) M 
		ON M.BOR_ACC_ID = BF1.BOR_ACC_ID
		AND M.RFILE = BF1.RFILE

--aggregate loan level (really bill seq level) values at the RFILE level
UPDATE
	BF4
 SET 
	BF4.ACCT_LA_FAT_CUR_PRI = CAST(RFILE_AGG.ACCT_LA_FAT_CUR_PRI AS DECIMAL(18,2)),
	BF4.ACCT_LA_FAT_NSI = CAST(RFILE_AGG.ACCT_LA_FAT_NSI AS DECIMAL(18,2)),
	BF4.ACCT_LA_FAT_LTE_FEE = CAST(RFILE_AGG.ACCT_LA_FAT_LTE_FEE AS DECIMAL(18,2)),
	BF4.ACCT_TAP = CAST(RFILE_AGG.ACCT_TAP AS DECIMAL(18,2)),
	BF4.LD_FAT_EFF = RFILE_AGG.LD_FAT_EFF
FROM
	#BIL_FINAL BF4
	INNER JOIN
		(
			SELECT
				BOR_ACC_ID,
				RFILE,
				--force last payment amounts to 0 when there were no payments in LN90 for the billing cycle (LD_FAT_EFF IS NULL)
				CASE WHEN LD_FAT_EFF IS NULL THEN 0 ELSE ACCT_LA_FAT_CUR_PRI END AS ACCT_LA_FAT_CUR_PRI,
				CASE WHEN LD_FAT_EFF IS NULL THEN 0 ELSE ACCT_LA_FAT_NSI END AS ACCT_LA_FAT_NSI,
				CASE WHEN LD_FAT_EFF IS NULL THEN 0 ELSE ACCT_LA_FAT_LTE_FEE END AS ACCT_LA_FAT_LTE_FEE,
				CASE WHEN LD_FAT_EFF IS NULL THEN 0 ELSE ACCT_TAP END AS ACCT_TAP,
				LD_FAT_EFF
			FROM
				(
					SELECT
						-- aggregate the distinct list at the account/RFILE level
						AMT.BOR_ACC_ID,
						AMT.RFILE,
						SUM(AMT.ACCT_LA_FAT_CUR_PRI_LN) AS  ACCT_LA_FAT_CUR_PRI,
						SUM(AMT.ACCT_LA_FAT_NSI_LN) AS ACCT_LA_FAT_NSI,
						SUM(AMT.ACCT_LA_FAT_LTE_FEE_LN) AS  ACCT_LA_FAT_LTE_FEE,
						SUM(AMT.ACCT_TAP_LN) AS  ACCT_TAP,
						MAX(AMT.LD_FAT_EFF) AS LD_FAT_EFF
					FROM
						(
							--get a distinct list of the values for each account, RFILE, and bill seq to eliminate duplicate values for the same bill sequence but different loans
							SELECT DISTINCT
								BOR_ACC_ID,
								RFILE,
								LN_SEQ_BIL_WI_DTE,
								ACCT_LA_FAT_CUR_PRI_LN,
								ACCT_LA_FAT_NSI_LN,
								ACCT_LA_FAT_LTE_FEE_LN,
								ACCT_TAP_LN,
								LD_FAT_EFF
							FROM
								#BIL_FINAL
						) AMT
					GROUP BY
						AMT.BOR_ACC_ID,
						AMT.RFILE
				) PD
		) RFILE_AGG
				ON BF4.BOR_ACC_ID = RFILE_AGG.BOR_ACC_ID
				AND BF4.RFILE = RFILE_AGG.RFILE


				--select * from #BIL_FINAL


DECLARE @PrintProcessingIds TABLE(PrintProcessingId INT, BOR_ACC_ID VARCHAR(10), ScriptFileId INT, BOR_CNT INT)  --create table variable for results of insert into billing.printprocessing to capture PrintProcessingId

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0

INSERT INTO CLS.billing.PrintProcessing (AccountNumber, scriptFileId, SourceFile, ArcAddedAt, ImagedAt, EcorrDocumentCreatedAt, PrintedAt, CreatedBy, AddedAt, OnEcorr, DeletedAt, DeletedBy, BOR_CNT) --TODO:  uncomment for production
		OUTPUT -- capture PrintProcessingId and variables needed to join to #BIL_FINAL so PrintProcessingId can be captured for instert into billing.LineData
			INSERTED.PrintProcessingId, 
			INSERTED.AccountNumber, 
			INSERTED.ScriptFileId,
			INSERTED.BOR_CNT
		INTO @PrintProcessingIds(PrintProcessingId, BOR_ACC_ID, ScriptFileId, BOR_CNT)	
	SELECT DISTINCT
		FIN.BOR_ACC_ID AS AccountNumber,
		FIL.ScriptFileId AS ScriptFileId,
		'' AS SourceFile,
		NULL AS ArcAddedAt,
		NULL AS ImagedAt,
		NULL AS EcorrDocumentCreatedAt,
		NULL AS PrintedAt,
		SUSER_SNAME() AS CreatedBy,
		GETDATE() AS AddedAt,
		FIN.ON_ECORR AS OnEcorr,
		NULL AS DeletedAt,
		NULL AS DeletedBy,
		FIN.BOR_CNT

	FROM
		#BIL_FINAL FIN
		INNER JOIN CLS.billing.ScriptFiles FIL
			ON FIL.SourceFile LIKE '%' + FIN.RFILE  + '.*%'  --select the row for the loan where the RILE is in the SourceFile
			AND FIL.Active = 1
		LEFT JOIN CLS.billing.PrintProcessing DUPCHECK --check for duplicate records
			ON FIN.BOR_ACC_ID = DUPCHECK.AccountNumber
			AND FIL.ScriptFileId = DUPCHECK.ScriptFileId
			AND CONVERT(DATE, DUPCHECK.AddedAt) = CONVERT(DATE, GETDATE())
	WHERE
		DUPCHECK.AccountNumber IS NULL --not already added today
	GROUP BY
		FIN.BOR_ACC_ID,
		FIL.ScriptFileId,
		FIN.ON_ECORR,
		FIN.BOR_CNT --not needed to get correct grouping as is the same for all rows for the BOR_ACC_ID but required in GROUP BY if included in SELECT
SELECT @ERROR = @@ERROR

INSERT INTO CLS.billing.LineData(PrintProcessingId,LineData)
	SELECT
		IDS.PrintProcessingId,
		--LineData
			CONVERT(VARCHAR(10),BIL.BOR_CNT) + ',' + --BOR_CNT
			BIL.ACSKEY  + ',' + --ACSKEY
			BIL.BOR_ACC_ID  + ',' + --DF_SPE_ACC_ID
			'"' + RTRIM(BIL.DM_PRS_1) + '",' +  --DM_PRS_1
			RTRIM(BIL.DM_PRS_MID) + ',' + --DM_PRS_MID
			'"' + RTRIM(BIL.DM_PRS_LST) + '",' + --DM_PRS_LST
			'"' + RTRIM(BIL.DX_STR_ADR_1) + '",' + --DX_STR_ADR_1
			'"' + RTRIM(BIL.DX_STR_ADR_2) + '",' + --DX_STR_ADR_2
			'"' + RTRIM(BIL.DM_CT) + '",' + --DM_CT
			BIL.DC_DOM_ST  + ',' + --DC_DOM_ST
			RTRIM(BIL.DF_ZIP_CDE)  + ',' + --DF_ZIP_CDE
			'"' + RTRIM(BIL.DM_FGN_CNY) + '",' + --DM_FGN_CNY
			'"' + RTRIM(BIL.LN_TYPE_DESC)  + '",' + --IC_LON_PGM
			CONVERT(VARCHAR(4), BIL.LN_SEQ)  + ',' + --LN_SEQ
			RTRIM(CONVERT(VARCHAR,BIL.LD_LON_1_DSB,101))  + ',' + --LD_LON_1_DSB
			CONVERT(VARCHAR(10),COALESCE(BIL.LR_INT_BIL,0))  + ',' + --LR_INT_BIL
			'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.ORG_PRI,0) AS money),1) + '",' + --ORG_PRI  --amounts converted to money to force commas into dollar amounts
			'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.LA_CUR_PRN_BIL,0) AS money),1) + '",' + --LA_CUR_PRN_BIL
			CONVERT(VARCHAR(4),COALESCE(BIL.LN_DLQ_MAX,0))  + ',' + --LN_DLQ_MAX
			RTRIM(CONVERT(VARCHAR,BIL.LD_BIL_CRT,101))  + ',' + -- LD_BIL_CRT
			COALESCE(RTRIM(CONVERT(VARCHAR,BIL.LD_FAT_EFF,101)),'')  + ',' + -- LD_FAT_EFF
			'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.LA_FAT_CUR_PRI,0) AS money),1) + '",' + --LA_FAT_CUR_PRI
			'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.LA_FAT_NSI,0) AS money),1) + '",' + --LA_FAT_NSI
			'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.TAP,0) AS money),1) + '",' + --TAP
			RTRIM(CONVERT(VARCHAR,BIL.LD_BIL_DU,101))  + ',' + --LD_BIL_DU
			'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.LA_BIL_PAS_DU,0) AS money),1) + '",' + --LA_BIL_PAS_DU
			'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.LA_BIL_DU_PRT,0) AS money),1) + '",' + --LA_BIL_DU_PRT
			BIL.LF_EDS  + ',' + --LF_EDS
			BIL.SCANLN  + ',' + --SCANLN
			BIL.STATE_IND  + ',' + --STATE_IND
			BIL.LC_BIL_TYP  + ',' + --LC_BIL_TYP
			CONVERT(VARCHAR(3),BIL.LN_SEQ_BIL_WI_DTE)  + ',' + --LN_SEQ_BIL_WI_DTE
			'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.LA_AGG_PRI,0) AS money),1) + '",' + --LA_AGG_PRI
			'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.LA_AGG_INT,0) AS money),1) + '",' + --LA_AGG_INT
			'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.LA_AGG_TOT,0) AS money),1) + '",' + --LA_AGG_TOT
			'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.LA_CUR_INT_DU,0) AS money),1) + '",' + --LA_CUR_INT_DU
			'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.WA_TOT_BRI_OTS,0) AS money),1) + '",' + --BIL.WA_TOT_BRI_OTS
			'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.ACCT_LA_FAT_CUR_PRI,0) AS money),1) + '",' + --ACCT_LA_FAT_CUR_PRI
			'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.ACCT_LA_FAT_NSI,0) AS money),1) + '",' + --ACCT_LA_FAT_NSI
			'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.ACCT_TAP,0) AS money),1) + '",' + --ACCT_TAP
			'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.ACCT_LA_AGG_PRI,0) AS money),1) + '",' + --ACCT_LA_AGG_PRI
			'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.ACCT_LA_AGG_INT,0) AS money),1) + '",' + --ACCT_LA_AGG_INT
			'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.ACCT_LA_AGG_TOT,0) AS money),1) + '",' + --ACCT_LA_AGG_TOT
			'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.AGG_TOT_PRN_BAL,0) AS money),1) + '",' + --AGG_TOT_PRN_BAL
			'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.ACCT_LA_AGG_FEE,0) AS money),1) + '",' + --ACCT_LA_AGG_FEE
			'"' + RTRIM(BIL.CITYSTATEZIP) + '",' + --CityStateZip
		CASE
			WHEN BIL.NEXT_PMT_DUE IS NULL THEN ',' -- write blank to the dataline if amount is NULL
			ELSE'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.NEXT_PMT_DUE,0) AS money),1) + '",' -- write formatted amount to the dataline if the amount is not NULL
		END + --NEXT_PMT_DUE
			COALESCE(RTRIM(CONVERT(VARCHAR,BIL.NEXT_PMT_DUE_DATE,101)),'') + ',' +  --NEXT_PMT_DUE_DATE
			CONVERT(VARCHAR(4),COALESCE(BIL.ACCT_LN_DLQ_MAX,0))  + ',' + --ACCT_LN_DLQ_MAX
			'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.ACCT_LA_FAT_LTE_FEE,0) AS money),1) + '",' + --ACCT_LA_FAT_LTE_FEE
			CONVERT(VARCHAR(4),COALESCE(BIL.RPS_TRMS_REM,0))  + ',' + --RPS_TRMS_REM
			'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.RPS_PMT_LOAN,0) AS money),1) + '",' + --RPS_PMT_LOAN
			'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.RPS_PMT_ACCT,0) AS money),1) + '"' --RPS_PMT_ACCT

		AS LineData
	FROM
		@PrintProcessingIds IDS --join to table variable with results of insert into billing.PrintProcessing to capture PrintProcessingId
		INNER JOIN -- join #BIL_FINAL to CLS.billing.ScriptFiles to get ScriptFileId for rows in #BIL_FINAL
		(
			SELECT
				FIN.*,
				FIL.ScriptFileId
			FROM
				#BIL_FINAL FIN
				INNER JOIN CLS.billing.ScriptFiles FIL
					ON FIL.SourceFile LIKE '%' + FIN.RFILE  + '.*%'  --select the row for the loan where the RILE is in the SourceFile
					AND FIL.Active = 1
		) BIL
			ON IDS.BOR_ACC_ID = BIL.BOR_ACC_ID
			AND IDS.BOR_CNT = BIL.BOR_CNT
			AND IDS.ScriptFileId = BIL.ScriptFileId
		LEFT JOIN CLS.billing.LineData DUPCHECK --check for duplicate records
			ON 
				CONVERT(VARCHAR(10),BIL.BOR_CNT) + ',' + --BOR_CNT
				BIL.ACSKEY  + ',' + --ACSKEY
				BIL.BOR_ACC_ID  + ',' + --DF_SPE_ACC_ID
				'"' + RTRIM(BIL.DM_PRS_1) + '",' +  --DM_PRS_1
				RTRIM(BIL.DM_PRS_MID) + ',' + --DM_PRS_MID
				'"' + RTRIM(BIL.DM_PRS_LST) + '",' + --DM_PRS_LST
				'"' + RTRIM(BIL.DX_STR_ADR_1) + '",' + --DX_STR_ADR_1
				'"' + RTRIM(BIL.DX_STR_ADR_2) + '",' + --DX_STR_ADR_2
				'"' + RTRIM(BIL.DM_CT) + '",' + --DM_CT
				BIL.DC_DOM_ST  + ',' + --DC_DOM_ST
				RTRIM(BIL.DF_ZIP_CDE)  + ',' + --DF_ZIP_CDE
				'"' + RTRIM(BIL.DM_FGN_CNY) + '",' + --DM_FGN_CNY
				'"' + RTRIM(BIL.LN_TYPE_DESC)  + '",' + --IC_LON_PGM
				CONVERT(VARCHAR(4), BIL.LN_SEQ)  + ',' + --LN_SEQ
				RTRIM(CONVERT(VARCHAR,BIL.LD_LON_1_DSB,101))  + ',' + --LD_LON_1_DSB  --amounts converted to money to force commas into dollar amounts
				CONVERT(VARCHAR(10),COALESCE(BIL.LR_INT_BIL,0))  + ',' + --LR_INT_BIL
				'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.ORG_PRI,0) AS money),1) + '",' + --ORG_PRI
				'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.LA_CUR_PRN_BIL,0) AS money),1) + '",' + --LA_CUR_PRN_BIL
				CONVERT(VARCHAR(4),COALESCE(BIL.LN_DLQ_MAX,0))  + ',' + --LN_DLQ_MAX
				RTRIM(CONVERT(VARCHAR,BIL.LD_BIL_CRT,101))  + ',' + -- LD_BIL_CRT
				COALESCE(RTRIM(CONVERT(VARCHAR,BIL.LD_FAT_EFF,101)),'')  + ',' + -- LD_FAT_EFF
				'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.LA_FAT_CUR_PRI,0) AS money),1) + '",' + --LA_FAT_CUR_PRI
				'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.LA_FAT_NSI,0) AS money),1) + '",' + --LA_FAT_NSI
				'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.TAP,0) AS money),1) + '",' + --TAP
				RTRIM(CONVERT(VARCHAR,BIL.LD_BIL_DU,101))  + ',' + --LD_BIL_DU
				'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.LA_BIL_PAS_DU,0) AS money),1) + '",' + --LA_BIL_PAS_DU
				'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.LA_BIL_DU_PRT,0) AS money),1) + '",' + --LA_BIL_DU_PRT
				BIL.LF_EDS  + ',' + --LF_EDS 
				BIL.SCANLN  + ',' + --SCANLN
				BIL.STATE_IND  + ',' + --STATE_IND
				BIL.LC_BIL_TYP  + ',' + --LC_BIL_TYP
				CONVERT(VARCHAR(3),BIL.LN_SEQ_BIL_WI_DTE)  + ',' + --LN_SEQ_BIL_WI_DTE
				'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.LA_AGG_PRI,0) AS money),1) + '",' + --LA_AGG_PRI
				'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.LA_AGG_INT,0) AS money),1) + '",' + --LA_AGG_INT
				'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.LA_AGG_TOT,0) AS money),1) + '",' + --LA_AGG_TOT
				'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.LA_CUR_INT_DU,0) AS money),1) + '",' + --LA_CUR_INT_DU
				'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.WA_TOT_BRI_OTS,0) AS money),1) + '",' + --BIL.WA_TOT_BRI_OTS
				'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.ACCT_LA_FAT_CUR_PRI,0) AS money),1) + '",' + --ACCT_LA_FAT_CUR_PRI
				'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.ACCT_LA_FAT_NSI,0) AS money),1) + '",' + --ACCT_LA_FAT_NSI
				'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.ACCT_TAP,0) AS money),1) + '",' + --ACCT_TAP
				'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.ACCT_LA_AGG_PRI,0) AS money),1) + '",' + --ACCT_LA_AGG_PRI
				'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.ACCT_LA_AGG_INT,0) AS money),1) + '",' + --ACCT_LA_AGG_INT
				'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.ACCT_LA_AGG_TOT,0) AS money),1) + '",' + --ACCT_LA_AGG_TOT
				'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.AGG_TOT_PRN_BAL,0) AS money),1) + '",' + --AGG_TOT_PRN_BAL
				'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.ACCT_LA_AGG_FEE,0) AS money),1) + '",' + --ACCT_LA_AGG_FEE
				'"' + RTRIM(BIL.CITYSTATEZIP) + '",' + --CityStateZip
				CASE
					WHEN BIL.NEXT_PMT_DUE IS NULL THEN ',' -- write blank to the dataline if amount is NULL
					ELSE'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.NEXT_PMT_DUE,0) AS money),1) + '",' -- write formatted amount to the dataline if the amount is not NULL
				END + --NEXT_PMT_DUE
				COALESCE(RTRIM(CONVERT(VARCHAR,BIL.NEXT_PMT_DUE_DATE,101)),'') + ',' +  --NEXT_PMT_DUE_DATE
				CONVERT(VARCHAR(4),COALESCE(BIL.ACCT_LN_DLQ_MAX,0))  + ',' + --ACCT_LN_DLQ_MAX
				'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.ACCT_LA_FAT_LTE_FEE,0) AS money),1) + '",' + --ACCT_LA_FAT_LTE_FEE
				CONVERT(VARCHAR(4),COALESCE(BIL.RPS_TRMS_REM,0))  + ',' + --RPS_TRMS_REM
				'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.RPS_PMT_LOAN,0) AS money),1) + '",' + --RPS_PMT_LOAN
				'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.RPS_PMT_ACCT,0) AS money),1) + '"' --RPS_PMT_ACCT
			= DUPCHECK.LineData
			AND CAST(AddedAt AS DATE) = CAST(GETDATE() AS DATE)
	WHERE
		DUPCHECK.LineData IS NULL --not already added today
SELECT @ERROR = @ERROR + @@ERROR

IF @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END