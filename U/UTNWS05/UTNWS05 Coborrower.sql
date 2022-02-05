SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @8DAYSAGO DATE = DATEADD(DAY, -8, GETDATE()) --TODO:  restore this line if modifed for testing

DROP TABLE IF EXISTS #BIL_FINAL

SELECT
	DENSE_RANK() OVER(ORDER BY BOR_ACC_ID, RFILE, LD_BIL_CRT) AS EDS_CNT, --included here so EDS_CNT is determined by RFILE in addition to account number
	CentralData.DBO.CreateScanLine
	(			
		BF_SSN,
		LD_BIL_CRT,
		RIGHT('00' + CONVERT(VARCHAR,LN_SEQ_BIL_WI_DTE),2), --2-char BL10.LN_SEQ_BIL_WI_DTE with leading zeros
		RIGHT('00000000' + REPLACE(CONVERT(VARCHAR,SUM(LA_BIL_DU_PRT_LN) OVER (PARTITION BY BOR_ACC_ID, RFILE)),'.',''),8), --8-char LA_BIL_DU_PRT_LN with leading zeros and no comma or decimal
		BOR_ACC_ID
	) AS SCANLN,
	MAX(LN_DLQ_MAX) OVER (PARTITION BY BOR_ACC_ID, RFILE, LD_BIL_CRT, LF_EDS) AS ACCT_LN_DLQ_MAX,
	SUM(RPS_PMT_LOAN) OVER (PARTITION BY BOR_ACC_ID, RFILE, LD_BIL_CRT, LF_EDS) AS RPS_PMT_ACCT,
	SUM(LA_BIL_PAS_DU_LN) OVER (PARTITION BY BOR_ACC_ID, RFILE, LD_BIL_CRT, LF_EDS) AS LA_BIL_PAS_DU,
	SUM(LA_TOT_INT_DU_LN) OVER (PARTITION BY BOR_ACC_ID, RFILE, LD_BIL_CRT, LF_EDS) AS LA_TOT_INT_DU,
	SUM(LA_BIL_DU_PRT_LN) OVER (PARTITION BY BOR_ACC_ID, RFILE, LD_BIL_CRT, LF_EDS) AS LA_BIL_DU_PRT,
	SUM(LA_CUR_INT_DU_LN) OVER (PARTITION BY BOR_ACC_ID, RFILE, LD_BIL_CRT, LF_EDS) AS LA_CUR_INT_DU,
	SUM(WA_TOT_BRI_OTS_LN) OVER (PARTITION BY BOR_ACC_ID, RFILE, LD_BIL_CRT, LF_EDS) AS WA_TOT_BRI_OTS,
	SUM(LA_FAT_CUR_PRI) OVER (PARTITION BY BOR_ACC_ID, RFILE, LD_BIL_CRT, LF_EDS)  AS ACCT_LA_FAT_CUR_PRI,
	SUM(LA_FAT_NSI) OVER (PARTITION BY BOR_ACC_ID, RFILE, LD_BIL_CRT, LF_EDS) AS ACCT_LA_FAT_NSI,
	SUM(LA_FAT_LTE_FEE) OVER (PARTITION BY BOR_ACC_ID, RFILE, LD_BIL_CRT, LF_EDS)  AS ACCT_LA_FAT_LTE_FEE,
	SUM(TAP) OVER (PARTITION BY BOR_ACC_ID, RFILE, LD_BIL_CRT, LF_EDS)  AS ACCT_TAP,
	SUM(LA_AGG_PRI) OVER (PARTITION BY BOR_ACC_ID, RFILE, LD_BIL_CRT, LF_EDS) AS ACCT_LA_AGG_PRI,
	SUM(LA_AGG_INT) OVER (PARTITION BY BOR_ACC_ID, RFILE, LD_BIL_CRT, LF_EDS)  AS ACCT_LA_AGG_INT,
	SUM(LA_AGG_TOT) OVER (PARTITION BY BOR_ACC_ID, RFILE, LD_BIL_CRT, LF_EDS)  AS ACCT_LA_AGG_TOT,
	SUM(AGG_TOT_PRN_BAL_LN) OVER (PARTITION BY BOR_ACC_ID, RFILE, LD_BIL_CRT, LF_EDS)  AS AGG_TOT_PRN_BAL,
	SUM(LA_AGG_FEE) OVER (PARTITION BY BOR_ACC_ID, RFILE, LD_BIL_CRT, LF_EDS)  AS ACCT_LA_AGG_FEE,
	*,
	CAST(0.00 AS DECIMAL(18,2)) AS NEXT_PMT_DUE,
	CAST(NULL AS DATE) AS NEXT_PMT_DUE_DATE
	INTO #BIL_FINAL --TODO: uncomment for production
FROM 
	(--BIL_FINAL
		SELECT DISTINCT
			CentralData.DBO.CreateACSKeyline(LN20.LF_EDS,'B',PD30.DC_ADR) AS ACSKEY,
			PD10_BOR.DF_SPE_ACC_ID AS BOR_ACC_ID,
			BL10.BF_SSN,
			LN10.LN_SEQ,
			PD10_EDS.DM_PRS_1 AS DM_PRS_1,
			PD10_EDS.DM_PRS_MID,
			PD10_EDS.DM_PRS_LST,
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
				WHEN DW01.WC_DW_LON_STA IN ('04', '05') THEN LN16_EDS.LN_DLQ_MAX
				ELSE LN16_EDS.LN_DLQ_MAX + 1 
			END AS LN_DLQ_MAX,
			BL10.LD_BIL_CRT,
			LastPayment.LD_FAT_EFF, --date of most recent payment during the last billing cycle
			--these three LastPayment fields aren't needed on the bill and were only included here for testing the account level (ACCT_) amounts
			LastPayment.LA_FAT_CUR_PRI,
			LastPayment.LA_FAT_NSI,
			LastPayment.LA_FAT_LTE_FEE,
			LastPayment.LA_FAT_CUR_PRI + LastPayment.LA_FAT_NSI  AS TAP,
			BL10.LD_BIL_DU,
			--borrowers with LC_BIL_TYP = 'C' are in a forb/defer/grace/school so they technically don�t have an amount past due
			CASE 
				WHEN BL10.LC_BIL_TYP = 'C' AND LC_IND_BIL_SNT = 'T' THEN 0 
				ELSE COALESCE(LN80.LA_BIL_PAS_DU,0) 
			END AS LA_BIL_PAS_DU_LN,
			CASE
				WHEN BL10.LC_BIL_TYP = 'C' AND LC_IND_BIL_SNT = 'T' THEN COALESCE(LN80.LA_BIL_DU_PRT,0) + COALESCE(LN80.LA_LTE_FEE_OTS_PRT,0)
				ELSE                    COALESCE(LN80.LA_BIL_PAS_DU,0) + COALESCE(LN80.LA_BIL_DU_PRT,0) + COALESCE(LN80.LA_LTE_FEE_OTS_PRT,0)
			END AS LA_TOT_INT_DU_LN,
			CASE
				WHEN BL10.LC_BIL_TYP = 'C' AND LC_IND_BIL_SNT = 'T' THEN COALESCE(LN80.LA_BIL_DU_PRT,0)
				ELSE                    COALESCE(LN80.LA_BIL_PAS_DU,0) + COALESCE(LN80.LA_BIL_DU_PRT,0) 
			END AS LA_BIL_DU_PRT_LN,
			COALESCE(LN80.LA_BIL_DU_PRT,0) AS LA_CUR_INT_DU_LN,
			PD10_EDS.DF_SPE_ACC_ID AS LF_EDS,
			1 AS IS_EDR,
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
			LastPayment.AGG_LA_FAT_CUR_PRI AS ACCT_LA_FAT_CUR_PRI_LN,
			LastPayment.AGG_LA_FAT_NSI AS ACCT_LA_FAT_NSI_LN,
			LastPayment.AGG_LA_FAT_LTE_FEE AS ACCT_LA_FAT_LTE_FEE_LN,
			LastPayment.AGG_LA_FAT_CUR_PRI + LastPayment.AGG_LA_FAT_LTE_FEE + LastPayment.AGG_LA_FAT_NSI AS ACCT_TAP_LN,
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
				WHEN DATENAME(dw,GETDATE())  = 'Monday' AND (  DATEDIFF(DAY,LN10.LD_LON_ACL_ADD,GETDATE()) = 3 OR DATEDIFF(DAY,LN10.LD_LON_ACL_ADD,GETDATE()) = 2 ) THEN 'R15' --today is Monday and loan was added on Friday or Saturday
				WHEN DATENAME(dw,GETDATE()) != 'Monday' AND DATEDIFF(DAY,LN10.LD_LON_ACL_ADD,GETDATE()) = 1 THEN 'R15'
				WHEN BL10.LC_IND_BIL_SNT IN ('1','2','4','7','F','R') AND BL10.LC_BIL_TYP = 'P'
					THEN 
						CASE 
							WHEN BR30.BF_SSN IS NOT NULL THEN 'R11' --ON_ACH
							WHEN BL10.LC_BIL_TYP = 'C' AND BL10.LC_IND_BIL_SNT = 'T' AND LN80.LD_NXT_PAY_DUE_AHD IS NOT NULL AND COALESCE(LN80.LA_BIL_DU_PRT,0) < .01 THEN  'R23' --LA_BIL_DU_PRT < .01
							WHEN LN80.LD_NXT_PAY_DUE_AHD IS NOT NULL AND COALESCE(LN80.LA_BIL_PAS_DU,0) + COALESCE(LN80.LA_BIL_DU_PRT,0)  < .01 THEN  'R23' --LA_BIL_DU_PRT < .01
							WHEN MAX(COALESCE(LN16_EDS.LN_DLQ_MAX,0)) OVER (PARTITION BY LN16_EDS.BF_SSN, LN16_EDS.LF_EDS ) <  1 AND LN16_EDS.BF_SSN IS NULL THEN 'R10'
							WHEN MAX(COALESCE(LN16_EDS.LN_DLQ_MAX,0)) OVER (PARTITION BY LN16_EDS.BF_SSN, LN16_EDS.LF_EDS )  >= 1 OR LN16_EDS.BF_SSN IS NOT NULL THEN 'R12'
						END
				WHEN BL10.LC_IND_BIL_SNT IN ('G', 'I', 'H') AND BL10.LC_BIL_TYP = 'P' THEN 'R11' --borrowers are on ACH but the bills were not sent due to insufficient time
				WHEN BL10.LC_IND_BIL_SNT = 'T' AND BL10.LC_BIL_TYP = 'C' THEN 'R13'
				WHEN BL10.LC_BIL_TYP = 'I' THEN 'R17'
			END AS RFILE,
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
			INNER JOIN 	CDW..LN20_EDS LN20 --only include loans for active endorsers
				ON LN10.BF_SSN = LN20.BF_SSN
				AND LN10.LN_SEQ = LN20.LN_SEQ
				AND LN20.LC_STA_LON20 = 'A'			
			INNER JOIN CDW..DW01_DW_CLC_CLU DW01
				ON DW01.BF_SSN = LN10.BF_SSN
				AND DW01.LN_SEQ = LN10.LN_SEQ
			INNER JOIN CDW..PD10_PRS_NME PD10_BOR --join to PD10 to get borrower account number
				ON BL10.BF_SSN = PD10_BOR.DF_PRS_ID
			INNER JOIN CDW..PD10_PRS_NME PD10_EDS --join to PD10 to get endorser account number
				ON LN20.LF_EDS = PD10_EDS.DF_PRS_ID
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
				ON PD30.DF_PRS_ID = PD10_EDS.DF_PRS_ID --join on endorser
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
				ON PD10_EDS.DF_SPE_ACC_ID = PH05.DF_SPE_ID --join on endorser
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
			( --coborrower level delinquency (if the coborrower has any delinquent loans, all of the coborrower's loans need to be on the delinquent bill)

				SELECT
					LN16.BF_SSN,
					LN20.LF_EDS,
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
					INNER JOIN CDW..LN20_EDS LN20
						ON LN10.BF_SSN = LN20.BF_SSN
						AND LN10.LN_SEQ = LN20.LN_SEQ 
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
					LN20.LF_EDS,
					LN16.LN_SEQ
			) LN16_EDS
				ON LN10.BF_SSN = LN16_EDS.BF_SSN
				AND LN20.LN_SEQ = LN16_EDS.LN_SEQ
			LEFT JOIN 
			(
				SELECT
					LN15.BF_SSN,
					LN15.LN_SEQ,
					SUM(LA_DSB) - SUM(COALESCE(LA_DSB_CAN,0)) AS ORG_PRI
				FROM
					CDW..LN15_DSB LN15
					INNER JOIN 	CDW..LN20_EDS LN20 --only include loans for active endorsers
						ON LN15.BF_SSN = LN20.BF_SSN
						AND LN15.LN_SEQ = LN20.LN_SEQ
						AND LN20.LC_STA_LON20 = 'A'
				GROUP BY
					LN15.BF_SSN,
					LN15.LN_SEQ
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
					INNER JOIN 	CDW..LN20_EDS LN20 --only include loans for active endorsers
						ON LN83.BF_SSN = LN20.BF_SSN
						AND LN83.LN_SEQ = LN20.LN_SEQ
						AND LN20.LC_STA_LON20 = 'A'
				WHERE
					BR30.BC_EFT_STA = 'A'
				GROUP BY
					BR30.BF_SSN,
					LN83.LN_SEQ
			) BR30
				ON BR30.BF_SSN = LN80.BF_SSN
				AND BR30.LN_SEQ = LN80.LN_SEQ
			LEFT JOIN
			( --LastPayment (sum of payments during last billing cycle)
				SELECT DISTINCT
					LN90.BF_SSN,
					EffDates.LF_EDS,
					LN90.LN_SEQ,
					MAX(LN90.LD_FAT_EFF) OVER (PARTITION BY LN90.BF_SSN,EffDates.LF_EDS) AS LD_FAT_EFF ,
					SUM(ABS(COALESCE(LN90.LA_FAT_CUR_PRI,0))) OVER (PARTITION BY LN90.BF_SSN,EffDates.LF_EDS, LN90.LN_SEQ) AS LA_FAT_CUR_PRI,
					SUM(ABS(COALESCE(LN90.LA_FAT_NSI,0))) OVER (PARTITION BY LN90.BF_SSN,EffDates.LF_EDS, LN90.LN_SEQ) AS LA_FAT_NSI,
					SUM(ABS(COALESCE(LN90.LA_FAT_LTE_FEE,0))) OVER (PARTITION BY LN90.BF_SSN,EffDates.LF_EDS, LN90.LN_SEQ) AS LA_FAT_LTE_FEE,
					SUM(ABS(COALESCE(LN90.LA_FAT_CUR_PRI,0))) OVER(PARTITION BY LN90.BF_SSN,EffDates.LF_EDS) AS AGG_LA_FAT_CUR_PRI,
					SUM(ABS(COALESCE(LN90.LA_FAT_NSI,0))) OVER(PARTITION BY LN90.BF_SSN,EffDates.LF_EDS) AS AGG_LA_FAT_NSI,
					SUM(ABS(COALESCE(LN90.LA_FAT_LTE_FEE,0))) OVER(PARTITION BY LN90.BF_SSN,EffDates.LF_EDS) AS AGG_LA_FAT_LTE_FEE
				FROM
					CDW..LN90_FIN_ATY LN90
					INNER JOIN 
					(--begin and end of billing cycle for each loan
						SELECT
							POP.BF_SSN,
							POP.LF_EDS,
							POP.LN_SEQ,
							MAX(CASE WHEN POP.RN = 2 THEN POP.LD_BIL_CRT END) AS BEGIN_RANGE, --begin of billing cycle
							MAX(CASE WHEN POP.RN = 1 THEN POP.LD_BIL_CRT END) AS END_RANGE -- end of billing cycle
			
						FROM
						( --ordered list of bill create dates from which most recent and next most recent create dates are selected as begin and end dates of billing cycle
							SELECT 
								BILLS.BF_SSN, 
								BILLS.LF_EDS,
								BILLS.LD_BIL_CRT,
								BILLS.LN_SEQ,
								ROW_NUMBER() OVER(PARTITION BY BILLS.BF_SSN, BILLS.LN_SEQ ORDER BY BILLS.LD_BIL_CRT DESC) AS RN
							FROM 
							( --list of bills for target population
								SELECT DISTINCT
									BL10.BF_SSN,
									POP.LF_EDS,
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
											LN20.LN_SEQ,
											LN20.LF_EDS
										FROM
											CDW..BL10_BR_BIL BL10
											INNER JOIN CDW..LN80_LON_BIL_CRF LN80
												ON BL10.BF_SSN = LN80.BF_SSN
												AND BL10.LD_BIL_CRT = LN80.LD_BIL_CRT
												AND BL10.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
											INNER JOIN 	CDW..LN20_EDS LN20 --only include loans for active endorsers
												ON LN80.BF_SSN = LN20.BF_SSN
												AND LN80.LN_SEQ = LN20.LN_SEQ
												AND LN20.LC_STA_LON20 = 'A'
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
							POP.LF_EDS,
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
					AND LastPayment.LF_EDS = LN20.LF_EDS
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
			AND LN20.LC_STA_LON20 = 'A'
			AND 
				(
					LN20.LC_EDS_TYP = 'M'  -- always bill Co-borrower (Spousal Loans)
					OR (
							LN20.LC_EDS_TYP = 'S' 
							AND LN16_EDS.LN_DLQ_MAX >= 1  -- only bill Endorser (PLUS Loans) when loan is delinquent
						)
				)
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

--SELECT * FROM #BIL_FINAL --TODO: for testing, comment out for production

DECLARE @PrintProcessingIds TABLE(PrintProcessingId INT, EDS_ACC_ID VARCHAR(10), ScriptFileId INT, LF_EDS CHAR(10), BOR_CNT INT)  --create table variable for results of insert into billing.printprocessing to capture PrintProcessingId

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0

INSERT INTO CLS.billing.PrintProcessing (AccountNumber, scriptFileId, SourceFile, ArcAddedAt, ImagedAt, EcorrDocumentCreatedAt, PrintedAt, CreatedBy, AddedAt, OnEcorr, DeletedAt, DeletedBy, LF_EDS, BOR_CNT) --TODO:  uncomment for production
		OUTPUT -- capture PrintProcessingId and variables needed to join to #BIL_FINAL so PrintProcessingId can be captured for instert into billing.LineData
			INSERTED.PrintProcessingId, 
			INSERTED.AccountNumber, 
			INSERTED.ScriptFileId,
			INSERTED.LF_EDS,
			inserted.BOR_CNT
		INTO @PrintProcessingIds(PrintProcessingId, EDS_ACC_ID, ScriptFileId, LF_EDS, BOR_CNT)	
	SELECT DISTINCT
		FIN.LF_EDS AS AccountNumber,
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
		FIN.LF_EDS, --added to keep bills at the coborrower level before it was decided the account number should be the coborrower account number which does the same thing
		FIN.EDS_CNT

	FROM
		#BIL_FINAL FIN
		INNER JOIN CLS.billing.ScriptFiles FIL
			ON FIL.SourceFile LIKE '%' + FIN.RFILE  + '.*%'  --select the row for the loan where the RILE is in the SourceFile
			AND FIL.Active = 1
		LEFT JOIN CLS.billing.PrintProcessing DUPCHECK --check for duplicate records
			ON FIN.LF_EDS = DUPCHECK.AccountNumber
			AND FIL.ScriptFileId = DUPCHECK.ScriptFileId
			AND CONVERT(DATE, DUPCHECK.AddedAt) = CONVERT(DATE, GETDATE())
	WHERE
		DUPCHECK.AccountNumber IS NULL --not already added today
	GROUP BY
		FIN.BOR_ACC_ID,
		FIL.ScriptFileId,
		FIN.ON_ECORR, --not needed to get correct grouping as is the same for all rows for the BOR_ACC_ID but required in GROUP BY if included in SELECT
		FIN.LF_EDS,
		FIN.EDS_CNT	
SELECT @ERROR = @@ERROR

INSERT INTO CLS.billing.LineData(PrintProcessingId,LineData)
	SELECT
		IDS.PrintProcessingId,
		--LineData
			CONVERT(VARCHAR(10),BIL.EDS_CNT) + ',' + --EDS_CNT
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
			ON IDS.EDS_ACC_ID = BIL.LF_EDS
			AND IDS.ScriptFileId = BIL.ScriptFileId
			AND IDS.BOR_CNT = BIL.EDS_CNT
			AND IDS.LF_EDS = BIL.LF_EDS --TODO uncomment if the billing.printprocessing.LF_EDS field gets updated to char(10), delete for production otherwise
		LEFT JOIN CLS.billing.LineData DUPCHECK --check for duplicate records
			ON 
				CONVERT(VARCHAR(10),BIL.EDS_CNT) + ',' + --EDS_CNT
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
						WHEN BIL.NEXT_PMT_DUE IS NULL THEN ','
						ELSE'"$' + CONVERT(VARCHAR(12),CAST(COALESCE(BIL.NEXT_PMT_DUE,0) AS money),1) + '",' --NEXT_PMT_DUE
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
