--UTNWS01 : REPAYMENT DISCLOSURES - COBORROWERS

USE CDW;
GO

DROP TABLE IF EXISTS #MAINPOP,#RPSB_BasePop,#LN_LoanDetails,#RPS_Sums,#RD_Terms,#GD2_MonthlyRepaymentSchedule,#Output_TotalRepay,#LoanDetail,#CoBorrower_MRS_nonplus,#CoBorrower_MRS_plus,#Output_MonthlyRepaymentSchedule,#CoBorrower_nonplus,#CoBorrower_plus,#Output_LoanDetail,#POP

DECLARE @CURRENT_DATETIME DATETIME = GETDATE();
DECLARE
		@TODAY DATE = @CURRENT_DATETIME,
		@CostCenter VARCHAR(6) = 'MA4481',
		@ScriptData_ScriptId VARCHAR(10) = 'RPMTDISCFD', --script id in print.ScriptData table
		@EcorrEmail VARCHAR(27) = 'Ecorr@MyCornerStoneLoan.org'
		--,@SSN_TEST VARCHAR(9) = ''   --TEST
		--,@ACCT_TEST VARCHAR(10) = '' --TEST
;

BEGIN TRY
	BEGIN TRANSACTION;

		--borrower loans with associated repayment sequences
		SELECT DISTINCT
			LN65.BF_SSN,
			LN65.LN_SEQ,
			LN65.LN_RPS_SEQ
		INTO
			#MAINPOP
		FROM
			LN10_LON LN10
			INNER JOIN LN65_LON_RPS LN65
				ON LN10.BF_SSN = LN65.BF_SSN
				AND LN10.LN_SEQ = LN65.LN_SEQ
			INNER JOIN RS10_BR_RPD RS10
				ON LN65.BF_SSN = RS10.BF_SSN
				AND LN65.LN_RPS_SEQ = RS10.LN_RPS_SEQ
			LEFT JOIN
			(--get all loans with active MSRPD arcs
				SELECT
					AY10.BF_SSN,
					LN85.LN_SEQ
				FROM
					AY10_BR_LON_ATY AY10
					INNER JOIN LN85_LON_ATY LN85
						ON LN85.BF_SSN = AY10.BF_SSN
						AND LN85.LN_ATY_SEQ = AY10.LN_ATY_SEQ
				WHERE
					AY10.PF_REQ_ACT = 'MSRPD'
					AND AY10.LC_STA_ACTY10 = 'A'
			) ARC
				ON ARC.BF_SSN = LN10.BF_SSN
				AND ARC.LN_SEQ = LN10.LN_SEQ
		WHERE
			RS10.LD_SNT_RPD_DIS >= DATEADD(MONTH,-1,@TODAY) --always looks back a month because borrowers with invalid addresses can be updated and sent out if it is within 30 days
			AND RS10.LD_SNT_RPD_DIS <= @TODAY
			AND ARC.BF_SSN IS NULL
			AND RS10.LC_STA_RPST10 = 'A'
			AND LN65.LC_STA_LON65 = 'A'
			AND LN10.LA_CUR_PRI > 0.00
			AND LN10.LC_STA_LON10 = 'R'
			AND LN10.LD_LON_ACL_ADD > CONVERT(DATE,'20141201')
		;
		--select * from #MAINPOP where bf_ssn = @SSN_TEST

		--base population: gets borrowers with all their associated coborrower data
		SELECT DISTINCT
			RS10.BF_SSN --borrower SSN
			,PD10_BORR.DF_SPE_ACC_ID --borrower account
			,LN20.LF_EDS AS co_BF_SSN --coborrower SSN
			,PD10.DF_SPE_ACC_ID AS co_DF_SPE_ACC_ID --coborrower account
			,RS10.LN_RPS_SEQ
			,LN66.LN_GRD_RPS_SEQ
			,IIF(LN10.IC_LON_PGM IN ('DLPLGB', 'DLPLUS', 'PLUS', 'PLUSGB'), 'PLUS', 'NONPLUS') AS PGM_TYP
			,IIF(PD10.DM_PRS_MID = '', (RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_LST)), (RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_MID) + ' ' + RTRIM(PD10.DM_PRS_LST))) AS [NAME]
			,REPLACE(PD30.DX_STR_ADR_1, ',', '') AS DX_STR_ADR_1
			,REPLACE(PD30.DX_STR_ADR_2, ',', '') AS DX_STR_ADR_2
			,REPLACE(PD30.DM_CT, ',', '')		 AS DM_CT
			,REPLACE(PD30.DC_DOM_ST, ',', '')	 AS DC_DOM_ST
			,REPLACE(PD30.DF_ZIP_CDE, ',', '')	 AS DF_ZIP_CDE
			,REPLACE(PD30.DM_FGN_CNY, ',', '')	 AS DM_FGN_CNY
			,REPLACE(PD30.DC_ADR, ',', '')		 AS DC_ADR
			,LN65.LC_TYP_SCH_DIS
			,COALESCE(LN65.LA_ANT_CAP,0)	 AS LA_ANT_CAP
			,COALESCE(LN65.LA_RPD_INT_DIS,0) AS LA_RPD_INT_DIS
			,COALESCE(LN65.LA_TOT_RPD_DIS,0) AS LA_TOT_RPD_DIS
			,LN66.LN_RPS_TRM
			,LN66.LA_RPS_ISL
			,RS10.LD_RPS_1_PAY_DU
			,LN90.LA_FAT_NSI
			,LN10.LD_LON_1_DSB
			,LN65.LN_SEQ
			,CASE 
				WHEN LN10.IC_LON_PGM IN ('DLSCST', 'DLSCUN', 'DLSCPL', 'DLSCPG', 'DLSCSL', 'DLSCSC', 'DLSCUC', 'DLSCCN', 'DLPCNS', 'DLSCNS', 'DLSSPL', 'DLUCNS', 'DLUSPL', 'CNSLDN', 'SUBCNS', 'SUBSPC', 'UNCNS', 'UNSPC')   
				THEN LN10.LR_WIR_CON_LON --consolidation loans
				ELSE LP06.PR_ITR_MIN  --non-consolidation loans
			END AS STAT_RATE  -- statutory rate
			,CASE
				WHEN LN10.IC_LON_PGM IN ('DLSCST', 'DLSCUN', 'DLSCPL', 'DLSCPG', 'DLSCSL', 'DLSCSC', 'DLSCUC', 'DLSCCN') THEN 'Direct Special Consolidation'
				WHEN LN10.IC_LON_PGM IN ('DLPCNS', 'DLSCNS', 'DLSSPL', 'DLUCNS', 'DLUSPL') THEN 'Direct Consolidation'
				WHEN LN10.IC_LON_PGM IN ('CNSLDN', 'SUBCNS', 'SUBSPC', 'UNCNS', 'UNSPC')   THEN 'FFEL Consolidation'
				WHEN LN10.IC_LON_PGM IN ('DLSTFD', 'DLUNST') THEN 'Direct Stafford'
				WHEN LN10.IC_LON_PGM IN ('STFFRD', 'UNSTFD') THEN 'FFEL Stafford'
				WHEN LN10.IC_LON_PGM IN ('DLPLGB', 'DLPLUS') THEN 'Direct Plus'
				WHEN LN10.IC_LON_PGM IN ('PLUS', 'PLUSGB')	 THEN 'FFEL Plus'
				WHEN LN10.IC_LON_PGM IN ('TEACH') THEN 'Direct Teach'
				WHEN LN10.IC_LON_PGM IN ('SLS')	  THEN 'FFEL SLS'
				ELSE 'ERROR'
			END AS IC_LON_PGM
			,DW01.WD_LON_RPD_SR
			,COALESCE(LN65.LA_CPI_RPD_DIS,0) AS LA_CPI_RPD_DIS
			,COALESCE(LN65.LA_ACR_INT_RPD,0) AS LA_ACR_INT_RPD
			,DEFR.LD_DFR_END
		INTO 
			#RPSB_BasePop
		FROM 
			#MAINPOP MP
			INNER JOIN LN20_EDS LN20
				ON LN20.BF_SSN = MP.BF_SSN
				AND LN20.LN_SEQ = MP.LN_SEQ
			INNER JOIN LN10_LON LN10
				ON LN20.BF_SSN = LN10.BF_SSN
				AND LN20.LN_SEQ = LN10.LN_SEQ
			INNER JOIN LN65_LON_RPS LN65
				ON LN10.BF_SSN = LN65.BF_SSN
				AND LN10.LN_SEQ = LN65.LN_SEQ
			INNER JOIN LN66_LON_RPS_SPF LN66
				ON LN65.BF_SSN = LN66.BF_SSN
				AND LN65.LN_SEQ = LN66.LN_SEQ
				AND LN65.LN_RPS_SEQ = LN66.LN_RPS_SEQ
			INNER JOIN RS10_BR_RPD RS10
				ON LN65.BF_SSN = RS10.BF_SSN
				AND LN65.LN_RPS_SEQ = RS10.LN_RPS_SEQ
			INNER JOIN PD10_PRS_NME PD10
				ON LN20.LF_EDS = PD10.DF_PRS_ID --coborrower
			INNER JOIN PD10_PRS_NME PD10_BORR
				ON LN10.BF_SSN = PD10_BORR.DF_PRS_ID --gets borrower acct#
			INNER JOIN PD30_PRS_ADR PD30
				ON PD10.DF_PRS_ID = PD30.DF_PRS_ID --gets coborrower address
				AND PD30.DC_ADR = 'L'
		--LN72:  get LN72.LC_ITR_TYP needed for join to LP06
			INNER JOIN 
			(
				SELECT
					LN72.BF_SSN,
					LN72.LN_SEQ,
					LN72.LC_ITR_TYP,
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
					AND	@TODAY BETWEEN LN72.LD_ITR_EFF_BEG AND LN72.LD_ITR_EFF_END
			) LN72
				ON LN10.LN_SEQ = LN72.LN_SEQ
				AND LN10.BF_SSN = LN72.BF_SSN
				AND LN72.SEQ = 1
			LEFT JOIN DW01_DW_CLC_CLU DW01
				ON LN65.BF_SSN = DW01.BF_SSN
				AND LN65.LN_SEQ = DW01.LN_SEQ
			LEFT JOIN
			(--deferments
				SELECT
					LN50.BF_SSN,
					LN50.LN_SEQ,
					LN50.LD_DFR_END
				FROM
					DF10_BR_DFR_REQ DF10
					INNER JOIN LN50_BR_DFR_APV LN50
						ON DF10.BF_SSN = LN50.BF_SSN
						AND DF10.LF_DFR_CTL_NUM = LN50.LF_DFR_CTL_NUM
					INNER JOIN LN20_EDS LN20
						ON LN20.BF_SSN = LN50.BF_SSN
						AND LN20.LN_SEQ = LN50.LN_SEQ
				WHERE
					DF10.LC_STA_DFR10 = 'A'
					AND DF10.LC_DFR_STA = 'A'
					AND DF10.LC_DFR_TYP = '46' --plus 5 month grace
					AND LN50.LC_STA_LON50 = 'A'
					AND LN50.LC_DFR_RSP != '003' --response code 003: deferment request denied
					AND LN50.LD_DFR_BEG < @TODAY
					AND LN50.LD_DFR_END > @TODAY
					AND LN20.LC_EDS_TYP = 'M'
					AND LN20.LC_STA_LON20 = 'A'
			) DEFR
				ON LN10.BF_SSN = DEFR.BF_SSN
				AND LN10.LN_SEQ = DEFR.LN_SEQ
		--LP06 to get statutory rate
			LEFT JOIN LP06_ITR_AND_TYP LP06
				ON LN10.IC_LON_PGM = LP06.IC_LON_PGM
				AND LN10.LF_RGL_CAT_LP06 = LP06.PF_RGL_CAT
				AND LN10.IF_GTR = LP06.IF_GTR
				AND LN10.LF_LON_CUR_OWN = LP06.IF_OWN
				AND @TODAY BETWEEN LP06.PD_EFF_SR_LPD06 AND LP06.PD_EFF_END_LPD06
				AND LN72.LC_ITR_TYP = LP06.PC_ITR_TYP
				AND LP06.PC_STA_LPD06 = 'A'
		--LN90: get sum of LA_FAT_NSI
			LEFT JOIN 
			(
				SELECT 
					LN90.BF_SSN
					,LN90.LN_SEQ
					,SUM(COALESCE(LN90.LA_FAT_NSI,0)) AS LA_FAT_NSI
				FROM 
					LN90_FIN_ATY LN90
					INNER JOIN LN20_EDS LN20
						ON LN20.BF_SSN = LN90.BF_SSN
						AND LN20.LN_SEQ = LN90.LN_SEQ
				WHERE 
					LN90.LC_STA_LON90 = 'A'
					AND COALESCE(LN90.LC_FAT_REV_REA,'') = ''
					AND LN90.PC_FAT_TYP = '10'
					AND LN90.PC_FAT_SUB_TYP IN ('10','11','15','16','20','21','26','35','36','37')
					AND LN20.LC_EDS_TYP = 'M'
					AND LN20.LC_STA_LON20 = 'A'
				GROUP BY 
					LN90.BF_SSN
					,LN90.LN_SEQ
			) LN90
				ON LN10.BF_SSN = LN90.BF_SSN
				AND LN10.LN_SEQ = LN90.LN_SEQ

		WHERE 
			RS10.LD_SNT_RPD_DIS <= @TODAY --do not print future printed disclosures
			AND RS10.LC_STA_RPST10 = 'A'
			AND LN65.LC_STA_LON65 = 'A'
			AND LN20.LC_EDS_TYP = 'M'
			AND LN20.LC_STA_LON20 = 'A'
			AND LN10.LA_CUR_PRI > 0.00
			AND LN10.LC_STA_LON10 = 'R'
		;
		--select * from #RPSB_BasePop where DF_SPE_ACC_ID = @ACCT_TEST

		/*****************************************************************
			EXTRACT PORTIONS FROM MAIN DATA PULL
		******************************************************************/
		--loan data for loan detail sheet
		SELECT DISTINCT
			 BF_SSN
			,DF_SPE_ACC_ID
			,co_BF_SSN
			,co_DF_SPE_ACC_ID
			,LN_RPS_SEQ
			,IC_LON_PGM
			,LN_SEQ
			,PGM_TYP
			,LA_CPI_RPD_DIS
			,LA_ACR_INT_RPD
			,CONVERT(VARCHAR(10),LD_LON_1_DSB,101)	AS LD_LON_1_DSB
			,CONVERT(VARCHAR(10),WD_LON_RPD_SR,101) AS WD_LON_RPD_SR
			,CONVERT(VARCHAR(10),LD_DFR_END,101)	AS LD_DFR_END
		INTO
			#LN_LoanDetails
		FROM
			#RPSB_BasePop
		;
		--select * from #LN_LoanDetails where DF_SPE_ACC_ID  = @ACCT_TEST
		--order by co_BF_SSN,ln_seq,LN_RPS_SEQ

		--summing values
		SELECT
			 DL.BF_SSN
			,DL.DF_SPE_ACC_ID
			,DL.co_BF_SSN
			,DL.co_DF_SPE_ACC_ID
			,DL.LN_RPS_SEQ
			,DL.IC_LON_PGM
			,DL.LN_SEQ
			,DL.LC_TYP_SCH_DIS
			,DL.PGM_TYP
			,CONVERT(VARCHAR(10),DL.LD_RPS_1_PAY_DU,101) AS LD_RPS_1_PAY_DU
			,CONVERT(DECIMAL(10,3),DL.STAT_RATE) AS STAT_RATE
			,SUM(COALESCE(DL.LA_CPI_RPD_DIS,0)) AS LA_CPI_RPD_DIS --Current principal at time of repayment disclosure
			,SUM(COALESCE(DL.LA_ANT_CAP,0)) AS LA_ANT_CAP --Amount of anticipated interest capitalization 
		   ,(SUM(COALESCE(DL.LA_ANT_CAP,0)) + SUM(COALESCE(DL.LA_CPI_RPD_DIS,0))) AS RS_PRI_RPY
			,SUM(COALESCE(DL.LA_RPD_INT_DIS,0)) AS LA_RPD_INT_DIS --Disclosed amount of interest to be repaid 
			,SUM(COALESCE(DL.LA_TOT_RPD_DIS,0)) AS LA_TOT_RPD_DIS --Total repayment amount (principal and interest) to be repaid 
			,SUM(COALESCE(BP.LA_FAT_NSI,0)) * -1 AS LA_FAT_NSI --Amount of non-subsidized (borrower) interest financial activity transaction
		INTO
			#RPS_Sums
		FROM
			(--remove repayment gradation level attributes to put data set at DISCLOSURE LEVEL
				SELECT DISTINCT
					 BF_SSN
					,DF_SPE_ACC_ID
					,co_BF_SSN
					,co_DF_SPE_ACC_ID
					,LN_RPS_SEQ
					,LD_RPS_1_PAY_DU
					,LN_SEQ
					,LC_TYP_SCH_DIS
					,LA_CPI_RPD_DIS
					,LA_ACR_INT_RPD
					,STAT_RATE
					,LA_ANT_CAP
					,LA_RPD_INT_DIS
					,LA_TOT_RPD_DIS
					,PGM_TYP
					,IC_LON_PGM
					,LD_DFR_END
					,LD_LON_1_DSB
					,WD_LON_RPD_SR
				FROM
					#RPSB_BasePop
			) DL --Disclosure Level
			LEFT JOIN
			(--Amount of non-subsidized (borrower) interest financial activity transaction (LA_FAT_NSI)
				SELECT DISTINCT
					 BF_SSN
					,DF_SPE_ACC_ID
					,co_BF_SSN
					,co_DF_SPE_ACC_ID
					,LN_SEQ
					,LA_FAT_NSI
				FROM
					#RPSB_BasePop
				WHERE
					LA_FAT_NSI IS NOT NULL
			) BP --base pop
				ON DL.BF_SSN = BP.BF_SSN
				AND DL.LN_SEQ = BP.LN_SEQ
				AND DL.co_BF_SSN = BP.co_BF_SSN
		GROUP BY
			 DL.BF_SSN
			,DL.DF_SPE_ACC_ID
			,DL.co_BF_SSN
			,DL.co_DF_SPE_ACC_ID
			,DL.LN_RPS_SEQ
			,DL.IC_LON_PGM
			,DL.LN_SEQ
			,DL.LC_TYP_SCH_DIS
			,DL.PGM_TYP
			,DL.LD_RPS_1_PAY_DU
			,DL.STAT_RATE
		;
		--select * from #RPS_Sums where DF_SPE_ACC_ID = @ACCT_TEST

		/***************************************************************
			NEW GRADATION LOGIC
		***************************************************************/
		SELECT
			TS.DF_SPE_ACC_ID,
			TS.BF_SSN,
			TS.co_BF_SSN,
			TS.co_DF_SPE_ACC_ID,
			TS.TermStartDate,
			TS.LN_RPS_TRM,
			TS.TermTotalPayment,
			TS.GradationtotalPayment,
			0.00 AS PaymentAmount,
			NULL AS NumPayments,
			NULL AS FirstPayDue,
			TS.TermSequenceASC,
			TS.PGM_TYP
		INTO
			#RD_Terms
		FROM
			( -- term sequenced population
				SELECT
					DF_SPE_ACC_ID,
					BF_SSN,
					co_BF_SSN,
					co_DF_SPE_ACC_ID,
					TermStartDate,
					LN_RPS_TRM,
					TermTotalPayment,
					GradationtotalPayment,
					PGM_TYP,
					ROW_NUMBER() OVER (PARTITION BY DF_SPE_ACC_ID, BF_SSN, co_BF_SSN, co_DF_SPE_ACC_ID, TermStartDate, PGM_TYP ORDER BY LN_RPS_TRM) AS TermSequenceASC
				FROM
					( -- total payments by term and gradation
						SELECT DISTINCT
							BP.DF_SPE_ACC_ID,
							BP.BF_SSN,
							BP.co_BF_SSN,
							BP.co_DF_SPE_ACC_ID,
							RS.TermStartDate,
							RS.LN_RPS_TRM,
							BP.PGM_TYP,
							SUM(RS.LA_RPS_ISL) OVER (PARTITION BY BP.BF_SSN, BP.DF_SPE_ACC_ID, BP.co_BF_SSN, BP.co_DF_SPE_ACC_ID, RS.TermStartDate, BP.PGM_TYP) AS GradationTotalPayment,
							SUM(RS.LA_RPS_ISL) OVER (PARTITION BY BP.BF_SSN, BP.DF_SPE_ACC_ID, BP.co_BF_SSN, BP.co_DF_SPE_ACC_ID, RS.TermStartDate, RS.LN_RPS_TRM, BP.PGM_TYP) AS TermTotalPayment
						FROM 
							calc.RepaymentSchedules RS
							INNER JOIN LN20_EDS LN20 
								ON RS.BF_SSN = LN20.BF_SSN
								AND RS.LN_SEQ = LN20.LN_SEQ
								AND LN20.LC_EDS_TYP = 'M'
								AND LN20.LC_STA_LON20 = 'A'
							INNER JOIN LN10_LON LN10
								ON LN10.BF_SSN = LN20.BF_SSN
								AND LN10.LN_SEQ = LN20.LN_SEQ
								AND LN10.LA_CUR_PRI > 0.00
								AND LN10.LC_STA_LON10 = 'R'
							INNER JOIN #RPSB_BasePop BP
								ON RS.BF_SSN = BP.BF_SSN
								AND RS.LN_SEQ = BP.LN_SEQ
								AND LN20.LF_EDS = BP.co_BF_SSN
								AND RS.LN_RPS_SEQ = BP.LN_RPS_SEQ
								AND RS.LN_GRD_RPS_SEQ = BP.LN_GRD_RPS_SEQ
							
					) TP --total payments
			) TS --term sequenced population
		;
		--select * from #RD_Terms where DF_SPE_ACC_ID = @ACCT_TEST

		--MONTHLY REPAYMENT SCHEDULE
		;WITH SUMS AS
		(
			SELECT
				RD.DF_SPE_ACC_ID,
				RD.BF_SSN,
				RD.co_BF_SSN,
				RD.co_DF_SPE_ACC_ID,
				RD.TermStartDate,
				RD.LN_RPS_TRM,
				RD.TermTotalPayment,
				RD.GradationtotalPayment,
				RD.TermSequenceASC,
				PaymentAmount = RD.GradationTotalPayment, -- start with first term
				NumPayments = RD.LN_RPS_TRM,
				FirstPayDue = RD.TermStartDate,
				RD.PGM_TYP
			FROM
				#RD_Terms RD
			WHERE
				RD.TermSequenceASC = 1 -- start with sequence 1
	
			UNION ALL
	
			SELECT
				RD.DF_SPE_ACC_ID,
				RD.BF_SSN,
				RD.co_BF_SSN,
				RD.co_DF_SPE_ACC_ID,
				RD.TermStartDate,
				RD.LN_RPS_TRM,
				RD.TermTotalPayment,
				RD.GradationtotalPayment,
				RD.TermSequenceASC,
				S.PaymentAmount - S.TermTotalPayment,
				RD.LN_RPS_TRM - S.LN_RPS_TRM,
				DATEADD(MONTH, S.LN_RPS_TRM, RD.TermStartDate), 
				RD.PGM_TYP
			FROM
				SUMS S
				INNER JOIN #RD_Terms RD 
					ON RD.TermSequenceASC = S.TermSequenceASC + 1 
					AND RD.BF_SSN = S.BF_SSN 
					AND RD.co_BF_SSN = S.co_BF_SSN
					AND RD.TermStartDate = S.TermStartDate
					AND RD.PGM_TYP = S.PGM_TYP
		)
		SELECT
			DF_SPE_ACC_ID,
			BF_SSN,
			co_BF_SSN,
			co_DF_SPE_ACC_ID,
			LN_RPS_TRM,
			IIF(LA_RPS_ISL >= 1000.00, CONCAT('"',FORMAT(LA_RPS_ISL,'C','en-US'),'"'), CONCAT('$',LA_RPS_ISL)) AS LA_RPS_ISL,
			CONVERT(VARCHAR(10),LD_RPS_1_PAY_DU,101) AS LD_RPS_1_PAY_DU,
			PGM_TYP,
			SchedCount
		INTO
			#GD2_MonthlyRepaymentSchedule
		FROM
			(--CTE calculated schedule
				SELECT
					S.DF_SPE_ACC_ID,
					S.BF_SSN,
					S.co_BF_SSN,
					S.co_DF_SPE_ACC_ID,
					S.NumPayments		 AS LN_RPS_TRM,
					SUM(S.PaymentAmount) AS LA_RPS_ISL,
					S.FirstPayDue		 AS LD_RPS_1_PAY_DU,
					S.PGM_TYP,
					COUNT(*) OVER(PARTITION BY S.BF_SSN,S.co_BF_SSN, S.PGM_TYP) AS SchedCount --partition PLUS and NONPLUS
				FROM
					SUMS S
				GROUP BY
					S.DF_SPE_ACC_ID,
					S.BF_SSN,
					S.co_BF_SSN,
					S.co_DF_SPE_ACC_ID,
					S.NumPayments,
					S.FirstPayDue,
					S.PGM_TYP
			) GD
		;
		--select * from #GD2_MonthlyRepaymentSchedule where DF_SPE_ACC_ID = @ACCT_TEST
		
		--TOTAL AMOUNT TO BE REPAID summing & formatting
		SELECT DISTINCT
			 PGM_TYP
			,DF_SPE_ACC_ID
			,BF_SSN
			,co_DF_SPE_ACC_ID
			,co_BF_SSN
			,DC_DOM_ST
			,(COALESCE(DF_SPE_ACC_ID,'')
				+ ',' + COALESCE(ACSKeyLine,'')
				+ ',' + COALESCE([NAME],'')
				+ ',' + COALESCE(LTRIM(RTRIM(DX_STR_ADR_1)),'')
				+ ',' + COALESCE(LTRIM(RTRIM(DX_STR_ADR_2)),'')
				+ ',' + COALESCE(LTRIM(RTRIM(DM_CT)),'')
				+ ',' + COALESCE(LTRIM(RTRIM(DC_DOM_ST)),'')
				+ ',' + COALESCE(LTRIM(RTRIM(DF_ZIP_CDE)),'')
				+ ',' + COALESCE(LTRIM(RTRIM(DM_FGN_CNY)),'')
				+ ',' + COALESCE(CASE
						WHEN LC_TYP_SCH_DIS = 'AG' THEN 'Modified Graduated Repay Sched For LA'
						WHEN LC_TYP_SCH_DIS = 'CA' THEN 'PAYE-PFH'
						WHEN LC_TYP_SCH_DIS = 'CL' THEN 'Contingent Level'
						WHEN LC_TYP_SCH_DIS = 'CP' THEN 'PAYE Permanent Standard'
						WHEN LC_TYP_SCH_DIS = 'CQ' THEN 'ICR Permanent Standard'
						WHEN LC_TYP_SCH_DIS = 'C1' THEN 'Income Contingent Repayment 1'
						WHEN LC_TYP_SCH_DIS = 'C2' THEN 'Income Contingent Repayment 2'
						WHEN LC_TYP_SCH_DIS = 'C3' THEN 'Income Contingent Repayment 3'
						WHEN LC_TYP_SCH_DIS = 'EG' THEN 'Extended Grad'
						WHEN LC_TYP_SCH_DIS = 'EL' THEN 'Extended Level'
						WHEN LC_TYP_SCH_DIS = 'E2' THEN 'Extended Select 2 Repayment Option'
						WHEN LC_TYP_SCH_DIS = 'E5' THEN 'Extended Select 5 Repayment Option'
						WHEN LC_TYP_SCH_DIS = 'E7' THEN 'Extended Select 7 Repayment Option'
						WHEN LC_TYP_SCH_DIS = 'E8' THEN 'Extended Select 8 Repayment Option'
						WHEN LC_TYP_SCH_DIS = 'FG' THEN 'Fixed Graduated Amount Or Term'
						WHEN LC_TYP_SCH_DIS = 'FS' THEN 'Fixed Amount Or Fixed Term'
						WHEN LC_TYP_SCH_DIS = 'G'  THEN 'Graduated'
						WHEN LC_TYP_SCH_DIS = 'GT' THEN 'Gate Repay Sch'
						WHEN LC_TYP_SCH_DIS = 'IA' THEN 'REPAYE Alternative'
						WHEN LC_TYP_SCH_DIS = 'IB' THEN 'Partial Financial Hardship'
						WHEN LC_TYP_SCH_DIS = 'IL' THEN 'Permanent Standard'
						WHEN LC_TYP_SCH_DIS = 'IP' THEN 'IBR2014 PS'
						WHEN LC_TYP_SCH_DIS = 'IS' THEN 'Income Sensitve'
						WHEN LC_TYP_SCH_DIS = 'I3' THEN 'IBR2014 PFH'
						WHEN LC_TYP_SCH_DIS = 'I5' THEN 'Revised Pay As You Earn'
						WHEN LC_TYP_SCH_DIS = 'L'  THEN 'Level'
						WHEN LC_TYP_SCH_DIS = 'LJ' THEN 'Litigation'
						WHEN LC_TYP_SCH_DIS = 'LO' THEN 'Level Option'
						WHEN LC_TYP_SCH_DIS = 'L1' THEN '8/10% Level Repayment'
						WHEN LC_TYP_SCH_DIS = 'MG' THEN 'Modified Graduated'
						WHEN LC_TYP_SCH_DIS = 'PG' THEN 'Graduated'
						WHEN LC_TYP_SCH_DIS = 'PL' THEN 'Extended Level'
						WHEN LC_TYP_SCH_DIS = 'P1' THEN 'Program Option 1'
						WHEN LC_TYP_SCH_DIS = 'P2' THEN 'Program Option 2'
						WHEN LC_TYP_SCH_DIS = 'RE' THEN 'Exit Counseling - REPAYE'
						WHEN LC_TYP_SCH_DIS = 'RP' THEN 'Reduced Payments'
						WHEN LC_TYP_SCH_DIS = 'S1' THEN 'Select Option 1'
						WHEN LC_TYP_SCH_DIS = 'S2' THEN 'Select Option 2'
						WHEN LC_TYP_SCH_DIS = 'S3' THEN 'Select Option 1'
						WHEN LC_TYP_SCH_DIS = 'S4' THEN 'Select Option 4'
						WHEN LC_TYP_SCH_DIS = 'S5' THEN 'Select Option 5'
						WHEN LC_TYP_SCH_DIS = 'S7' THEN 'Select Option 7'
						WHEN LC_TYP_SCH_DIS = 'S8' THEN 'Select Option 8'
						ELSE 'ERROR'
					END,'')
				+ ',' + COALESCE(IIF(TOT_LA_CPI_RPD_DIS >= 1000.00	,CONCAT('"',FORMAT(TOT_LA_CPI_RPD_DIS, 'C', 'en-us'),'"'),CONCAT('$',TOT_LA_CPI_RPD_DIS)),'')
				+ ',' + COALESCE(IIF(RS_LA_ANT_CAP >= 1000.00		,CONCAT('"',FORMAT(RS_LA_ANT_CAP, 'C', 'en-us'),'"')	 ,CONCAT('$',RS_LA_ANT_CAP)),'')
				+ ',' + COALESCE(IIF(RS_PRI_RPY >= 1000.00			,CONCAT('"',FORMAT(RS_PRI_RPY, 'C', 'en-us'),'"')		 ,CONCAT('$',RS_PRI_RPY)),'')
				+ ',' + COALESCE(IIF(RS_LA_RPD_INT_DIS >= 1000.00	,CONCAT('"',FORMAT(RS_LA_RPD_INT_DIS, 'C', 'en-us'),'"') ,CONCAT('$',RS_LA_RPD_INT_DIS)),'')
				+ ',' + COALESCE(IIF(RS_LA_TOT_RPD_DIS >= 1000.00	,CONCAT('"',FORMAT(RS_LA_TOT_RPD_DIS, 'C', 'en-us'),'"') ,CONCAT('$',RS_LA_TOT_RPD_DIS)),'')
				+ ',' + COALESCE(IIF(LA_FAT_NSI >= 1000.00			,CONCAT('"',FORMAT(LA_FAT_NSI, 'C', 'en-us'),'"')		 ,CONCAT('$',LA_FAT_NSI)),'')
			) AS LetterData
		INTO
			#Output_TotalRepay
		FROM
			(--sum up values w/demographic data attached
				SELECT DISTINCT
					 SUMS.PGM_TYP
					,SUMS.BF_SSN
					,DEMO.DF_SPE_ACC_ID
					,DEMO.co_BF_SSN
					,DEMO.co_DF_SPE_ACC_ID
					,DEMO.ACSKeyLine
					,DEMO.[NAME]
					,DEMO.DX_STR_ADR_1
					,DEMO.DX_STR_ADR_2
					,DEMO.DM_CT
					,DEMO.DC_DOM_ST
					,DEMO.DF_ZIP_CDE
					,DEMO.DM_FGN_CNY
					,SUMS.LC_TYP_SCH_DIS
					,SUM(CAST(SUMS.LA_FAT_NSI AS MONEY))	 AS LA_FAT_NSI
					,SUM(CAST(SUMS.LA_CPI_RPD_DIS AS MONEY)) AS TOT_LA_CPI_RPD_DIS
					,SUM(CAST(SUMS.LA_ANT_CAP AS MONEY))	 AS RS_LA_ANT_CAP
					,SUM(CAST(SUMS.RS_PRI_RPY AS MONEY))	 AS RS_PRI_RPY
					,SUM(CAST(SUMS.LA_RPD_INT_DIS AS MONEY)) AS RS_LA_RPD_INT_DIS
					,SUM(CAST(SUMS.LA_TOT_RPD_DIS AS MONEY)) AS RS_LA_TOT_RPD_DIS
				FROM
					(--get demographic data
						SELECT DISTINCT
							 PGM_TYP 
							,DF_SPE_ACC_ID
							,BF_SSN
							,co_DF_SPE_ACC_ID
							,co_BF_SSN
							,LN_SEQ
							,[NAME]
							,DX_STR_ADR_1
							,DX_STR_ADR_2
							,DM_CT
							,DC_DOM_ST
							,DF_ZIP_CDE
							,DM_FGN_CNY
							,DC_ADR --DC_ADR should always be populated; if null, ACSKeyline C# script will break. Might have to do a hierarchy of address types or what to do in case of null
							,CentralData.dbo.CreateACSKeyline(BF_SSN, 'B', DC_ADR) AS ACSKeyLine
							,LC_TYP_SCH_DIS
						FROM 
							#RPSB_BasePop
					) DEMO
					INNER JOIN #RPS_Sums SUMS
						 ON SUMS.BF_SSN = DEMO.BF_SSN
						AND SUMS.co_BF_SSN = DEMO.co_BF_SSN
						AND SUMS.LN_SEQ = DEMO.LN_SEQ
						AND SUMS.PGM_TYP = DEMO.PGM_TYP
						AND SUMS.LC_TYP_SCH_DIS = DEMO.LC_TYP_SCH_DIS
				GROUP BY
					 SUMS.PGM_TYP
					,SUMS.BF_SSN
					,DEMO.DF_SPE_ACC_ID
					,DEMO.co_BF_SSN
					,DEMO.co_DF_SPE_ACC_ID
					,DEMO.ACSKeyLine
					,DEMO.[NAME]
					,DEMO.DX_STR_ADR_1
					,DEMO.DX_STR_ADR_2
					,DEMO.DM_CT
					,DEMO.DC_DOM_ST
					,DEMO.DF_ZIP_CDE
					,DEMO.DM_FGN_CNY
					,SUMS.LC_TYP_SCH_DIS
			) TRU --total_repay_unformatted
		;
		--select * from #Output_TotalRepay where DF_SPE_ACC_ID = @ACCT_TEST
		
		--LOAN DETAIL SHEET prepared for xml transposing
		SELECT DISTINCT
			 LD.PGM_TYP
			,LD.BF_SSN
			,BP.DF_SPE_ACC_ID
			,BP.co_BF_SSN
			,BP.co_DF_SPE_ACC_ID
			,LD.LD_LON_1_DSB
			,LD.LN_SEQ
			,SUMS.STAT_RATE
			,LD.IC_LON_PGM
			,LD.WD_LON_RPD_SR
			,IIF(LD.LA_CPI_RPD_DIS >= 1000.00, CONCAT('"',FORMAT(LD.LA_CPI_RPD_DIS, 'C', 'en-us'),'"'), CONCAT('$',LD.LA_CPI_RPD_DIS)) AS LA_CPI_RPD_DIS
			,IIF(LD.LA_ACR_INT_RPD >= 1000.00, CONCAT('"',FORMAT(LD.LA_ACR_INT_RPD, 'C', 'en-us'),'"'), CONCAT('$',LD.LA_ACR_INT_RPD)) AS LA_ACR_INT_RPD
			,LD.LD_DFR_END
			,COUNT(*) OVER(PARTITION BY LD.BF_SSN, BP.co_BF_SSN) AS LoanCount
		INTO
			#LoanDetail
		FROM
			#RPSB_BasePop BP
			INNER JOIN #LN_LoanDetails LD
				ON BP.BF_SSN = LD.BF_SSN
				AND BP.co_BF_SSN = LD.co_BF_SSN
				AND BP.LN_SEQ = LD.LN_SEQ
			INNER JOIN #RPS_Sums SUMS
				ON SUMS.BF_SSN = LD.BF_SSN
				AND SUMS.co_BF_SSN = LD.co_BF_SSN
				AND SUMS.LN_SEQ = LD.LN_SEQ
				AND SUMS.LN_RPS_SEQ = LD.LN_RPS_SEQ
		GROUP BY
			 LD.PGM_TYP
			,LD.BF_SSN
			,BP.DF_SPE_ACC_ID
			,BP.co_BF_SSN
			,BP.co_DF_SPE_ACC_ID
			,LD.LD_LON_1_DSB
			,LD.LN_SEQ
			,SUMS.STAT_RATE
			,LD.IC_LON_PGM
			,LD.WD_LON_RPD_SR
			,LD.LA_CPI_RPD_DIS
			,LD.LA_ACR_INT_RPD
			,LD.LD_DFR_END
		;
		--select * from #LoanDetail where DF_SPE_ACC_ID = @ACCT_TEST

		/*******************************************************************************************
			FOR XML TRANSPOSING:
		This section of code gets loan data into a comma delimited list
		********************************************************************************************/

		/**********************************************************
			MONTHLY REPAYMENT SCHEDULE PROCESSING - PLUS & NONPLUS
		**********************************************************/
		--NONPLUS:
		DECLARE @NumberOfSchedule INT = 30; --the number of lines in the Monthly Repayment Schedule letter

		;WITH GradationLevels(Gradation) AS 
		(
			SELECT 1 
			UNION ALL
			SELECT Gradation + 1 FROM GradationLevels
			WHERE Gradation < @NumberOfSchedule
		)
		SELECT DISTINCT
			 LD.PGM_TYP
			,LD.DF_SPE_ACC_ID
			,LD.BF_SSN
			,LD.co_DF_SPE_ACC_ID
			,LD.co_BF_SSN
			,LD.LN_RPS_TRM
			,LD.LA_RPS_ISL
			,LD.LD_RPS_1_PAY_DU
			,GL.Gradation
		INTO
			#CoBorrower_MRS_nonplus
		FROM 
			GradationLevels GL
			CROSS JOIN 
			(
				SELECT
					 PGM_TYP
					,DF_SPE_ACC_ID
					,BF_SSN
					,co_BF_SSN
					,co_DF_SPE_ACC_ID
					,LN_RPS_TRM
					,LA_RPS_ISL
					,LD_RPS_1_PAY_DU
				FROM 
					#GD2_MonthlyRepaymentSchedule
				WHERE
					SchedCount <= @NumberOfSchedule
					AND PGM_TYP = 'NONPLUS'
			) LD --loan data
		ORDER BY
			 LD.PGM_TYP
			,LD.BF_SSN
		;
		--select * from #CoBorrower_MRS_nonplus where DF_SPE_ACC_ID = @ACCT_TEST
		
		--PLUS
		;WITH GradationLevels(Gradation) AS 
		(
			SELECT 1 
			UNION ALL
			SELECT Gradation + 1 FROM GradationLevels
			WHERE Gradation < @NumberOfSchedule
		)
		SELECT DISTINCT
			 LD.PGM_TYP
			,LD.DF_SPE_ACC_ID
			,LD.BF_SSN
			,LD.co_DF_SPE_ACC_ID
			,LD.co_BF_SSN
			,LD.LN_RPS_TRM
			,LD.LA_RPS_ISL
			,LD.LD_RPS_1_PAY_DU
			,GL.Gradation
		INTO
			#CoBorrower_MRS_plus
		FROM 
			GradationLevels GL
			CROSS JOIN 
			(
				SELECT
					 PGM_TYP
					,DF_SPE_ACC_ID
					,BF_SSN
					,co_DF_SPE_ACC_ID
					,co_BF_SSN
					,LN_RPS_TRM
					,LA_RPS_ISL
					,LD_RPS_1_PAY_DU
				FROM 
					#GD2_MonthlyRepaymentSchedule
				WHERE
					SchedCount <= @NumberOfSchedule
					AND PGM_TYP = 'PLUS'
			) LD --loan data
		ORDER BY
			 LD.PGM_TYP
			,LD.BF_SSN
		;
		--select * from #CoBorrower_MRS_plus where DF_SPE_ACC_ID = @ACCT_TEST

		--combine PLUS and NONPLUS monthly repayment schedules
		SELECT
			 PGM_TYP
			,DF_SPE_ACC_ID
			,BF_SSN
			,co_DF_SPE_ACC_ID
			,co_BF_SSN
			,LetterData
		INTO
			#Output_MonthlyRepaymentSchedule
		FROM
			(	--NONPLUS
				--Retrieve comma delimited list of terms, payment amounts, and payment dates for repayment disclosure
				SELECT DISTINCT
					 OCB.PGM_TYP
					,OCB.DF_SPE_ACC_ID
					,OCB.BF_SSN
					,OCB.co_DF_SPE_ACC_ID
					,OCB.co_BF_SSN
					,LetterData = STUFF
					(
						( 
							SELECT
								',' + LN_RPS_TRM + ',' + LA_RPS_ISL + ',' + LD_RPS_1_PAY_DU
							FROM    
								( 
									SELECT DISTINCT
										ICB.Gradation
										--,LD.BF_SSN --TEST
										,COALESCE(CAST(LD.LN_RPS_TRM AS VARCHAR(3)),'')	AS LN_RPS_TRM
										,COALESCE(CAST(LD.LA_RPS_ISL AS VARCHAR(25)),'') AS LA_RPS_ISL
										,COALESCE(CAST(LD.LD_RPS_1_PAY_DU AS VARCHAR(25)),'') AS LD_RPS_1_PAY_DU
									FROM 
										#CoBorrower_MRS_nonplus ICB --inner coborrower data set
										LEFT JOIN
										(
											SELECT 
												DF_SPE_ACC_ID,
												BF_SSN,
												co_BF_SSN,
												co_DF_SPE_ACC_ID,
												LN_RPS_TRM,
												LA_RPS_ISL,
												LD_RPS_1_PAY_DU,
												PGM_TYP,
												SchedCount,
												ROW_NUMBER() OVER(PARTITION BY BF_SSN, co_BF_SSN ORDER BY BF_SSN, co_BF_SSN, CONVERT(DATE,LD_RPS_1_PAY_DU)) AS LN_SEQ
											FROM
												#GD2_MonthlyRepaymentSchedule
											WHERE
												PGM_TYP = 'NONPLUS'
										) LD --loan data
											ON LD.BF_SSN = ICB.BF_SSN
											AND LD.co_BF_SSN = ICB.co_BF_SSN
											AND LD.LN_SEQ = ICB.Gradation
									WHERE
										OCB.BF_SSN = ICB.BF_SSN --outer coborrower = inner coborrower
										AND OCB.co_BF_SSN = ICB.co_BF_SSN
								) x
							ORDER BY
								BF_SSN
							FOR XML PATH(''), TYPE
						).value('.','VARCHAR(MAX)'),
						1,1,''
					)
				FROM
					( 
						SELECT DISTINCT
							PGM_TYP,
							DF_SPE_ACC_ID,
							BF_SSN,
							co_DF_SPE_ACC_ID,
							co_BF_SSN,
							LN_RPS_TRM,
							LA_RPS_ISL,
							LD_RPS_1_PAY_DU,
							Gradation
						FROM 
							#CoBorrower_MRS_nonplus OCB --outer coborrower data set
						WHERE
							OCB.PGM_TYP = 'NONPLUS'
					) OCB  --outer coborrower data set
				--order by
				--	ocb.df_spe_acc_id

				UNION ALL

				--PLUS
				--Retrieve comma delimited list of terms, payment amounts, and payment dates for repayment disclosure
				SELECT DISTINCT
					 OCB.PGM_TYP
					,OCB.DF_SPE_ACC_ID
					,OCB.BF_SSN
					,OCB.co_DF_SPE_ACC_ID
					,OCB.co_BF_SSN
					,LetterData = STUFF
					(
						( 
							SELECT
								',' + LN_RPS_TRM + ',' + LA_RPS_ISL + ',' + LD_RPS_1_PAY_DU
							FROM    
								( 
									SELECT DISTINCT
										ICB.Gradation
										--,LD.BF_SSN --TEST
										,COALESCE(CAST(LD.LN_RPS_TRM AS VARCHAR(3)),'')	AS LN_RPS_TRM
										,COALESCE(CAST(LD.LA_RPS_ISL AS VARCHAR(25)),'') AS LA_RPS_ISL
										,COALESCE(CAST(LD.LD_RPS_1_PAY_DU AS VARCHAR(25)),'') AS LD_RPS_1_PAY_DU
									FROM 
										#CoBorrower_MRS_plus ICB --inner coborrower data set
										LEFT JOIN
										(
											SELECT 
												DF_SPE_ACC_ID,
												BF_SSN,
												co_BF_SSN,
												co_DF_SPE_ACC_ID,
												LN_RPS_TRM,
												LA_RPS_ISL,
												LD_RPS_1_PAY_DU,
												PGM_TYP,
												SchedCount,
												ROW_NUMBER() OVER(PARTITION BY BF_SSN, co_BF_SSN ORDER BY BF_SSN,co_BF_SSN, CONVERT(DATE,LD_RPS_1_PAY_DU)) AS LN_SEQ
											FROM
												#GD2_MonthlyRepaymentSchedule
											WHERE
												PGM_TYP = 'PLUS'
										) LD --loan data
											ON LD.BF_SSN = ICB.BF_SSN
											AND LD.co_BF_SSN = ICB.co_BF_SSN
											AND LD.LN_SEQ = ICB.Gradation
									WHERE
										OCB.BF_SSN = ICB.BF_SSN --outer coborrower = inner coborrower
										AND OCB.co_BF_SSN = ICB.co_BF_SSN
								) x
							ORDER BY
								BF_SSN
							FOR XML PATH(''), TYPE
						).value('.','VARCHAR(MAX)'),
						1,1,''
					)
				FROM
					( 
						SELECT DISTINCT
							PGM_TYP,
							DF_SPE_ACC_ID,
							BF_SSN,
							co_DF_SPE_ACC_ID,
							co_BF_SSN,
							LN_RPS_TRM,
							LA_RPS_ISL,
							LD_RPS_1_PAY_DU,
							Gradation
						FROM 
							#CoBorrower_MRS_plus OCB --outer coborrower data set
						WHERE
							OCB.PGM_TYP = 'PLUS'
					) OCB --outer coborrower data set
				--order by
				--	ocb.df_spe_acc_id
			) UNIONED
		;
		--select * from #Output_MonthlyRepaymentSchedule where DF_SPE_ACC_ID = @ACCT_TEST

		/**********************************************************
			LOAN DETAIL PROCESSING - PLUS & NONPLUS
		**********************************************************/
		--NONPLUS:
		DECLARE @NumberOfLoans INT = 30 --the number of loans on the loan detail sheet

		;WITH GradationLevels(Gradation) AS 
		(
			SELECT 1 
			UNION ALL
			SELECT Gradation + 1 FROM GradationLevels
			WHERE Gradation < @NumberOfLoans
		)
		SELECT DISTINCT
			 LD.PGM_TYP
			,LD.BF_SSN
			,LD.DF_SPE_ACC_ID
			,LD.co_BF_SSN
			,LD.co_DF_SPE_ACC_ID
			,LD.LD_LON_1_DSB
			,LD.LN_SEQ
			,LD.STAT_RATE
			,LD.IC_LON_PGM
			,LD.WD_LON_RPD_SR
			,LD.LA_CPI_RPD_DIS
			,LD.LA_ACR_INT_RPD
			,LD.LD_DFR_END
			,GL.Gradation
		INTO
			#CoBorrower_nonplus
		FROM 
			GradationLevels GL
			CROSS JOIN 
			(
				SELECT 
					 PGM_TYP
					,BF_SSN
					,DF_SPE_ACC_ID
					,co_BF_SSN
					,co_DF_SPE_ACC_ID
					,LD_LON_1_DSB
					,LN_SEQ
					,STAT_RATE
					,IC_LON_PGM
					,WD_LON_RPD_SR
					,LA_CPI_RPD_DIS
					,LA_ACR_INT_RPD
					,LD_DFR_END
				FROM 
					#LoanDetail
				WHERE
					LoanCount <= @NumberOfLoans
					AND PGM_TYP = 'NONPLUS'
			) LD --Loan Detail
		ORDER BY
			 LD.DF_SPE_ACC_ID
			,LD.LN_SEQ
		;
		--select * from #CoBorrower_nonplus where DF_SPE_ACC_ID = @ACCT_TEST

		--PLUS
		;WITH GradationLevels(Gradation) AS 
		(
			SELECT 1 
			UNION ALL
			SELECT Gradation + 1 FROM GradationLevels
			WHERE Gradation < @NumberOfLoans
		)
		SELECT DISTINCT
			 LD.PGM_TYP
			,LD.BF_SSN
			,LD.DF_SPE_ACC_ID
			,LD.co_BF_SSN
			,LD.co_DF_SPE_ACC_ID
			,LD.LD_LON_1_DSB
			,LD.LN_SEQ
			,LD.STAT_RATE
			,LD.IC_LON_PGM
			,LD.WD_LON_RPD_SR
			,LD.LA_CPI_RPD_DIS
			,LD.LA_ACR_INT_RPD
			,LD.LD_DFR_END
			,GL.Gradation
		INTO
			#CoBorrower_plus
		FROM 
			GradationLevels GL
			CROSS JOIN 
			(
				SELECT 
					 PGM_TYP
					,BF_SSN
					,DF_SPE_ACC_ID
					,co_BF_SSN
					,co_DF_SPE_ACC_ID
					,LD_LON_1_DSB
					,LN_SEQ
					,STAT_RATE
					,IC_LON_PGM
					,WD_LON_RPD_SR
					,LA_CPI_RPD_DIS
					,LA_ACR_INT_RPD
					,LD_DFR_END
				FROM 
					#LoanDetail
				WHERE
					LoanCount <= @NumberOfLoans
					AND PGM_TYP = 'PLUS'
			) LD --Loan Detail
		ORDER BY
			 LD.DF_SPE_ACC_ID
			,LD.LN_SEQ
		;
		--select * from #CoBorrower_plus where DF_SPE_ACC_ID = @ACCT_TEST

		--combine all Loan Detail data
		SELECT
			 PGM_TYP
			,DF_SPE_ACC_ID
			,BF_SSN
			,co_DF_SPE_ACC_ID
			,co_BF_SSN
			,LetterData
		INTO
			#Output_LoanDetail
		FROM
			(	--NONPLUS loan detail sheet: Retrieve comma delimited list of loan detail items for repayment disclosure
				SELECT DISTINCT
					 OCB.PGM_TYP
					,OCB.DF_SPE_ACC_ID
					,OCB.BF_SSN
					,OCB.co_DF_SPE_ACC_ID
					,OCB.co_BF_SSN
					,LetterData = STUFF
					(
						( 
							SELECT
								',' + LD_LON_1_DSB + ',' + LN_SEQ + ',' + STAT_RATE + ',' + IC_LON_PGM + ',' +  WD_LON_RPD_SR + ',' + LA_CPI_RPD_DIS + ',' + LA_ACR_INT_RPD + ',' + LD_DFR_END
							FROM    
								( 
									SELECT DISTINCT
										ICB.Gradation,
										DENSE_RANK() OVER(PARTITION BY ICB.BF_SSN, ICB.co_BF_SSN ORDER BY COALESCE(LD.LN_SEQ,-1)) AS LN --force null loans to have a low rank (below 0) and order by that to force them to one side of the set
										--,LD.BF_SSN --TEST
										,IIF(COALESCE(LD.IC_LON_PGM,'') = '', '', COALESCE(LD.LD_LON_1_DSB,'')) AS LD_LON_1_DSB
										,COALESCE(CAST(LD.LN_SEQ AS VARCHAR(3)),'')	AS LN_SEQ
										,COALESCE(CAST(LD.STAT_RATE AS VARCHAR(25)),'') AS STAT_RATE
										,COALESCE(LD.IC_LON_PGM,'')		AS IC_LON_PGM
										,COALESCE(LD.WD_LON_RPD_SR,'')	AS WD_LON_RPD_SR
										,COALESCE(LD.LA_CPI_RPD_DIS,'')	AS LA_CPI_RPD_DIS
										,COALESCE(LD.LA_ACR_INT_RPD,'')	AS LA_ACR_INT_RPD
										,COALESCE(LD.LD_DFR_END,'')		AS LD_DFR_END
									FROM 
										#CoBorrower_nonplus ICB --inner coborrower data set
										LEFT JOIN #LoanDetail LD
											ON LD.BF_SSN = ICB.BF_SSN
											AND LD.co_BF_SSN = ICB.co_BF_SSN
											AND LD.LN_SEQ = ICB.Gradation
											AND LD.PGM_TYP = ICB.PGM_TYP
											AND LD.PGM_TYP = 'NONPLUS'
									WHERE
										OCB.BF_SSN = ICB.BF_SSN --outer coborrower = inner coborrower
										AND OCB.co_BF_SSN = ICB.co_BF_SSN
								) x
							ORDER BY
								CASE 
									WHEN LN = 1 
									THEN 20000 
									ELSE LN 
								END, --partition sets 1 for rank on null rows, so force those to the end
								BF_SSN				
							FOR XML PATH(''), TYPE
						).value('.','VARCHAR(MAX)'),
						1,1,''
					)
				FROM
					( 
						SELECT DISTINCT
							PGM_TYP,
							BF_SSN,
							DF_SPE_ACC_ID,
							co_BF_SSN,
							co_DF_SPE_ACC_ID,
							LD_LON_1_DSB,
							LN_SEQ,
							STAT_RATE,
							IC_LON_PGM,
							WD_LON_RPD_SR,
							LA_CPI_RPD_DIS,
							LA_ACR_INT_RPD,
							LD_DFR_END,
							Gradation
						FROM 
							#CoBorrower_nonplus OCB --outer coborrower data set
						WHERE
							PGM_TYP = 'NONPLUS'
					) OCB --outer coborrower data set
				--order by
				--	ocb.df_spe_acc_id
	
				UNION ALL
	
				--PLUS loan detail sheet: Retrieve comma delimited list of loan detail items for repayment disclosure
				SELECT DISTINCT
					 OCB.PGM_TYP
					,OCB.DF_SPE_ACC_ID
					,OCB.BF_SSN
					,OCB.co_DF_SPE_ACC_ID
					,OCB.co_BF_SSN
					,LetterData = STUFF
					(
						( 
							SELECT
								',' + LD_LON_1_DSB + ',' + LN_SEQ + ',' + STAT_RATE + ',' + IC_LON_PGM + ',' +  WD_LON_RPD_SR + ',' + LA_CPI_RPD_DIS + ',' + LA_ACR_INT_RPD + ',' + LD_DFR_END
							FROM    
								( 
									SELECT DISTINCT
										ICB.Gradation,
										DENSE_RANK() OVER(PARTITION BY ICB.BF_SSN, ICB.co_BF_SSN ORDER BY COALESCE(LD.LN_SEQ,-1)) AS LN --force null loans to have a low rank (below 0) and order by that to force them to one side of the set
										--,LD.BF_SSN --TEST
										,IIF(COALESCE(LD.IC_LON_PGM,'') = '', '', COALESCE(LD.LD_LON_1_DSB,'')) AS LD_LON_1_DSB
										,COALESCE(CAST(LD.LN_SEQ AS VARCHAR(3)),'')	AS LN_SEQ
										,COALESCE(CAST(LD.STAT_RATE AS VARCHAR(25)),'') AS STAT_RATE
										,COALESCE(LD.IC_LON_PGM,'')		AS IC_LON_PGM
										,COALESCE(LD.WD_LON_RPD_SR,'')	AS WD_LON_RPD_SR
										,COALESCE(LD.LA_CPI_RPD_DIS,'')	AS LA_CPI_RPD_DIS
										,COALESCE(LD.LA_ACR_INT_RPD,'')	AS LA_ACR_INT_RPD
										,COALESCE(LD.LD_DFR_END,'')		AS LD_DFR_END
									FROM 
										#CoBorrower_plus ICB --inner coborrower data set
										LEFT JOIN #LoanDetail LD
											ON ICB.BF_SSN = LD.BF_SSN
											AND LD.co_BF_SSN = ICB.co_BF_SSN
											AND LD.LN_SEQ = ICB.Gradation
											AND LD.PGM_TYP = ICB.PGM_TYP
											AND LD.PGM_TYP = 'PLUS'
									WHERE
										OCB.BF_SSN = ICB.BF_SSN --outer coborrower = inner coborrower
										AND OCB.co_BF_SSN = ICB.co_BF_SSN
								) x
							ORDER BY
								CASE 
									WHEN LN = 1 
									THEN 20000 
									ELSE LN 
								END, --partition sets 1 for rank on null rows, so force those to the end
								BF_SSN				
							FOR XML PATH(''), TYPE
						).value('.','VARCHAR(MAX)'),
						1,1,''
					)
				FROM
					( 
						SELECT DISTINCT
							PGM_TYP,
							BF_SSN,
							DF_SPE_ACC_ID,
							co_BF_SSN,
							co_DF_SPE_ACC_ID,
							LD_LON_1_DSB,
							LN_SEQ,
							STAT_RATE,
							IC_LON_PGM,
							WD_LON_RPD_SR,
							LA_CPI_RPD_DIS,
							LA_ACR_INT_RPD,
							LD_DFR_END,
							Gradation
						FROM 
							#CoBorrower_plus OCB --outer coborrower data set
						WHERE
							PGM_TYP = 'PLUS'
					) OCB --outer coborrower data set
				--order by
				--	ocb.df_spe_acc_id
			) UNIONED
		;
		--select * from #Output_LoanDetail where DF_SPE_ACC_ID = @ACCT_TEST
		
		--combine all data for PrintProcessing
		SET CONCAT_NULL_YIELDS_NULL OFF
		SELECT DISTINCT
			 TR.PGM_TYP
			,TR.DF_SPE_ACC_ID
			,TR.BF_SSN
			,TR.co_DF_SPE_ACC_ID
			,TR.co_BF_SSN
			,(TR.LetterData + ',' + MRS.LetterData + ',' + LD.LetterData + ','	+ CONVERT(VARCHAR(10),@TODAY,101) + ',' + TR.DC_DOM_ST + ',' + @CostCenter) AS LetterData
			,IIF(TR.LetterData IS NULL OR MRS.LetterData IS NULL OR LD.LetterData IS NULL, 1, 0) AS ErrorFlag
			,CASE
				WHEN TR.LetterData IS NULL	THEN '#Output_TotalRepay is null'
				WHEN MRS.LetterData IS NULL THEN '#Output_MonthlyRepaymentSchedule is null'
				WHEN LD.LetterData IS NULL	THEN '#Output_LoanDetail is null'
				WHEN TR.DC_DOM_ST IS NULL	THEN 'DC_DOM_ST is null'
			END AS ErrorLocation
		INTO
			#POP
		FROM
			#Output_TotalRepay TR 
			LEFT JOIN #Output_MonthlyRepaymentSchedule MRS 
				ON TR.DF_SPE_ACC_ID = MRS.DF_SPE_ACC_ID
				AND TR.co_BF_SSN = MRS.co_BF_SSN
				AND TR.PGM_TYP = MRS.PGM_TYP
			LEFT JOIN #Output_LoanDetail LD
				ON TR.DF_SPE_ACC_ID = LD.DF_SPE_ACC_ID
				AND TR.co_BF_SSN = LD.co_BF_SSN
				AND TR.PGM_TYP = LD.PGM_TYP
		;
		SET CONCAT_NULL_YIELDS_NULL ON 

		--select DISTINCT * from #POP where DF_SPE_ACC_ID = @ACCT_TEST
		--select * from #Output_TotalRepay where DF_SPE_ACC_ID = @ACCT_TEST
		--select * from #Output_MonthlyRepaymentSchedule where DF_SPE_ACC_ID = @ACCT_TEST
		--select * from #Output_LoanDetail where DF_SPE_ACC_ID = @ACCT_TEST

		/********************************************
			FINAL ECORR OUTPUT - R2 NONPLUS (RPDISCFED letter)
		********************************************/

		INSERT INTO CLS.[print].PrintProcessingCoBorrower
		(
			AccountNumber
			,EmailAddress
			,ScriptDataId
			,LetterData
			,CostCenter
			,DoNotProcessEcorr
			,OnEcorr
			,ArcNeeded
			,ImagingNeeded
			,AddedBy
			,AddedAt
			,BorrowerSsn
		)
		SELECT DISTINCT
			 R2.co_DF_SPE_ACC_ID AS AccountNumber
			,COALESCE(PH05.DX_CNC_EML_ADR,@EcorrEmail) AS EmailAddress
			,LetterData.ScriptDataId AS ScriptDataId
			,R2.LetterData
			,@CostCenter AS CostCenter
			,COALESCE(LetterData.DoNotProcessEcorr,0) AS DoNotProcessEcorr
			,IIF(PH05.DI_CNC_ELT_OPI = 'Y' AND PH05.DI_VLD_CNC_EML_ADR = 'Y',1,0) AS OnEcorr
			,IIF(LetterData.ArcId IS NULL,0,1) AS ArcNeeded
			,IIF(LetterData.DocIdId IS NULL,0,1) AS ImagingNeeded
			,SYSTEM_USER AS AddedBy
			,@CURRENT_DATETIME AS AddedAt
			,R2.BF_SSN AS BorrowerSsn
		FROM
			#POP R2
			LEFT JOIN
			(
				SELECT
					DI_CNC_ELT_OPI,
					DI_VLD_CNC_EML_ADR,
					DF_SPE_ID,
					DX_CNC_EML_ADR
				FROM
					PH05_CNC_EML --active flags included in SELECT to be used later
			) PH05
				ON R2.co_DF_SPE_ACC_ID = PH05.DF_SPE_ID
			LEFT JOIN PD10_PRS_NME PD10
				ON PD10.DF_SPE_ACC_ID = R2.co_DF_SPE_ACC_ID
			LEFT JOIN PD30_PRS_ADR PD30 --active flag in WHERE clause
				ON PD30.DF_PRS_ID = PD10.DF_PRS_ID
			LEFT JOIN
			(
				SELECT
					SD.ScriptId,
					SD.ScriptDataId, 
					L.LetterId,
					L.Letter,
					SD.DoNotProcessEcorr,
					SD.DocIdId,
					SDM.ArcId
				FROM
					CLS.[print].Letters L
					INNER JOIN CLS.[print].ScriptData SD
						ON SD.ScriptId = @ScriptData_ScriptId
						AND SD.LetterId = L.LetterId
					LEFT JOIN CLS.[print].ArcScriptDataMapping SDM
						ON SDM.ScriptDataId = SD.ScriptDataId
				WHERE 
					SD.Active = 1
					AND L.Letter = 'RPDISCFED'
			) LetterData
				ON 1 = 1
			LEFT JOIN CLS.[print].PrintProcessingCoBorrower EXISTING_DATA
				ON EXISTING_DATA.AccountNumber = PD10.DF_SPE_ACC_ID
				AND EXISTING_DATA.EmailAddress = COALESCE(PH05.DX_CNC_EML_ADR,@EcorrEmail)
				AND EXISTING_DATA.ScriptDataId = LetterData.ScriptDataId
				AND CONCAT(SUBSTRING(EXISTING_DATA.LetterData, 1, 10), SUBSTRING(EXISTING_DATA.LetterData, CHARINDEX('#,', EXISTING_DATA.LetterData)+1, CHARINDEX(@CostCenter, EXISTING_DATA.LetterData)+6))
					= CONCAT(SUBSTRING(R2.LetterData, 1, 10), SUBSTRING(R2.LetterData, CHARINDEX('#,', R2.LetterData)+1, CHARINDEX(@CostCenter, R2.LetterData)+6)) --eliminates match on ACSKeyline
				AND EXISTING_DATA.CostCenter = @CostCenter
				AND EXISTING_DATA.DoNotProcessEcorr = COALESCE(LetterData.DoNotProcessEcorr,0)
				AND EXISTING_DATA.OnEcorr = IIF(PH05.DI_CNC_ELT_OPI = 'Y' AND PH05.DI_VLD_CNC_EML_ADR = 'Y',1,0)
				AND EXISTING_DATA.ArcNeeded = IIF(LetterData.ArcId IS NULL,0,1)
				AND EXISTING_DATA.ImagingNeeded = IIF(LetterData.DocIdId IS NULL,0,1)
				AND CONVERT(DATE,EXISTING_DATA.AddedAt) >= DATEADD(DAY,-4,@TODAY)
				AND CONVERT(DATE,EXISTING_DATA.AddedAt) <= @TODAY
				AND EXISTING_DATA.BorrowerSsn = R2.BF_SSN
		WHERE
			R2.PGM_TYP = 'NONPLUS'
			AND R2.ErrorFlag = 0
			AND (--has valid address or they are on ecorr
					(PD30.DI_VLD_ADR = 'Y' AND PD30.DC_ADR = 'L')
					OR (PH05.DI_CNC_ELT_OPI = 'Y' AND PH05.DI_VLD_CNC_EML_ADR = 'Y')
				)
			AND EXISTING_DATA.AccountNumber IS NULL --removes anyone already added within past 4 days
		;
		--select * from CLS.[print].PrintProcessingCoBorrower where ScriptDataId = 1 --TEST

		/********************************************
			FINAL ECORR OUTPUT - R3 PLUS (PLRPYMTFED letter)
		********************************************/

		INSERT INTO CLS.[print].PrintProcessingCoBorrower
		(
			 AccountNumber
			,EmailAddress
			,ScriptDataId
			,LetterData
			,CostCenter
			,DoNotProcessEcorr
			,OnEcorr
			,ArcNeeded
			,ImagingNeeded
			,AddedBy
			,AddedAt
			,BorrowerSsn
		)
		SELECT DISTINCT
			 R3.co_DF_SPE_ACC_ID AS AccountNumber
			,COALESCE(PH05.DX_CNC_EML_ADR,@EcorrEmail) AS EmailAddress
			,LetterData.ScriptDataId AS ScriptDataId
			,R3.LetterData
			,@CostCenter AS CostCenter
			,COALESCE(LetterData.DoNotProcessEcorr,0) AS DoNotProcessEcorr
			,IIF(PH05.DI_CNC_ELT_OPI = 'Y' AND PH05.DI_VLD_CNC_EML_ADR = 'Y', 1, 0) AS OnEcorr
			,IIF(LetterData.ArcId IS NULL,0,1) AS ArcNeeded
			,IIF(LetterData.DocIdId IS NULL,0,1) AS ImagingNeeded
			,SYSTEM_USER AS AddedBy
			,@CURRENT_DATETIME AS AddedAt
			,R3.BF_SSN
		FROM
			#POP R3
			LEFT JOIN
			(
				SELECT
					DI_CNC_ELT_OPI,
					DI_VLD_CNC_EML_ADR,
					DF_SPE_ID,
					DX_CNC_EML_ADR
				FROM
					PH05_CNC_EML --active flags included in SELECT to be used later
			) PH05
				ON R3.co_DF_SPE_ACC_ID = PH05.DF_SPE_ID
			LEFT JOIN PD10_PRS_NME PD10
				ON PD10.DF_SPE_ACC_ID = R3.co_DF_SPE_ACC_ID
			LEFT JOIN PD30_PRS_ADR PD30
				ON PD30.DF_PRS_ID = PD10.DF_PRS_ID
			LEFT JOIN
			(
				SELECT
					SD.ScriptId,
					SD.ScriptDataId, 
					L.LetterId,
					L.Letter,
					SD.DoNotProcessEcorr,
					SD.DocIdId,
					SDM.ArcId
				FROM
					CLS.[print].Letters L
					INNER JOIN CLS.[print].ScriptData SD
						ON SD.ScriptId = @ScriptData_ScriptId
						AND SD.LetterId = L.LetterId
					LEFT JOIN CLS.[print].ArcScriptDataMapping SDM
						ON SDM.ScriptDataId = SD.ScriptDataId
				WHERE 
					SD.Active = 1
					AND L.Letter = 'PLRPYMTFED'
			) LetterData
				ON 1 = 1
			LEFT JOIN CLS.[print].PrintProcessingCoBorrower EXISTING_DATA
				ON EXISTING_DATA.AccountNumber = PD10.DF_SPE_ACC_ID
				AND EXISTING_DATA.EmailAddress = COALESCE(PH05.DX_CNC_EML_ADR,@EcorrEmail)
				AND EXISTING_DATA.ScriptDataId = LetterData.ScriptDataId
				AND CONCAT(SUBSTRING(EXISTING_DATA.LetterData, 1, 10), SUBSTRING(EXISTING_DATA.LetterData, CHARINDEX('#,', EXISTING_DATA.LetterData)+1, CHARINDEX(@CostCenter, EXISTING_DATA.LetterData)+6))
					= CONCAT(SUBSTRING(R3.LetterData, 1, 10), SUBSTRING(R3.LetterData, CHARINDEX('#,', R3.LetterData)+1, CHARINDEX(@CostCenter, R3.LetterData)+6)) --eliminates match on ACSKeyline
				AND EXISTING_DATA.CostCenter = @CostCenter
				AND EXISTING_DATA.DoNotProcessEcorr = COALESCE(LetterData.DoNotProcessEcorr,0)
				AND EXISTING_DATA.OnEcorr = IIF(PH05.DI_CNC_ELT_OPI = 'Y' AND PH05.DI_VLD_CNC_EML_ADR = 'Y', 1, 0)
				AND EXISTING_DATA.ArcNeeded = IIF(LetterData.ArcId IS NULL,0,1)
				AND EXISTING_DATA.ImagingNeeded = IIF(LetterData.DocIdId IS NULL,0,1)
				AND CONVERT(DATE,EXISTING_DATA.AddedAt) >= DATEADD(DAY,-4,@TODAY)
				AND CONVERT(DATE,EXISTING_DATA.AddedAt) <= @TODAY
				AND EXISTING_DATA.BorrowerSsn = R3.BF_SSN	
		WHERE
			R3.PGM_TYP = 'PLUS'
			AND R3.ErrorFlag = 0
			AND ( --has valid address or they are on ecorr
					(PD30.DI_VLD_ADR = 'Y' AND PD30.DC_ADR = 'L')
					OR (PH05.DI_CNC_ELT_OPI = 'Y' AND PH05.DI_VLD_CNC_EML_ADR = 'Y')
				)
			AND EXISTING_DATA.AccountNumber IS NULL --removes anyone already added within past 4 days
		;
		--select * from CLS.[print].PrintProcessingCoBorrower where ScriptDataId = 2 --TEST

		--list of all borrowers with message indicating failure for Process Logger
		DECLARE @ProcessLogId INT, @ProcessNotificationId INT;
		INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy)
		SELECT TOP 1
			@CURRENT_DATETIME,
			@CURRENT_DATETIME,
			'UTNWS01',
			'uheaa',
			SUSER_SNAME()
		FROM
			#POP
		WHERE
			ErrorFlag = 1;

		SET @ProcessLogId = SCOPE_IDENTITY();

		INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId, NotificationSeverityTypeId, ProcessLogId, ResolvedAt, ResolvedBy)
		SELECT TOP 1 --ties all errors to only one ID
			(SELECT TOP 1 NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'),
			(SELECT TOP 1 NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'),
			@ProcessLogId,
			NULL,
			NULL
		FROM
			#POP
		WHERE
			ErrorFlag = 1;

		SET @ProcessNotificationId = SCOPE_IDENTITY();

		INSERT INTO CLS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage)
		SELECT
			@ProcessNotificationId,
			'Unable to insert a row into CLS.[print].PrintProcessingCoBorrower for account #' + DF_SPE_ACC_ID + '. Letter Type: ' + PGM_TYP + '. LetterData incomplete because ' + ErrorLocation + '. LetterData currently returned: ' + LetterData  AS LogMessage
		FROM
			#POP
		WHERE
			ErrorFlag = 1;

	COMMIT TRANSACTION;
END TRY
BEGIN CATCH
	PRINT 'UTNWS01_coborrower.sql transaction NOT committed.';
	ROLLBACK TRANSACTION;
	THROW;
END CATCH;