CREATE PROCEDURE [dbo].[GetFinancialData]
	@SSN varchar(9)
AS

SELECT DISTINCT
		'N' AS [LON05_DAT_ENT_IND],
		'' AS [LA_NSI_AGG],
		'' AS [LA_PRI_CUR_AGG],
		'' AS [LA_R78_INT_MAX_AGG],
		'' AS [LA_R78_INT_PD_AGG],
		'' AS [LA_R78_INT_UPD_AGG],
		'' AS [LA_SIN_AGG],
		'' AS [LD_NSI_ACR_THU_AGG],
		'' AS [LD_NSI_PD_THU_AGG],
		'' AS [LD_SIN_ACR_THU_AGG],
		'' AS [LD_SIN_PD_THU_AGG],
		'' AS [LF_BR_OWN_ACC_AGG], 
		'' AS [LN_ACC_CVN_SEQ], 
		'' AS [LI_CAP_ATH_AGG],
		'' AS [LD_PCV_LST_CAP_AGG],
		'' AS [LA_AGG_LTE_FEE_OTS],
		'' AS [LD_AGG_LTE_FEE_WAV],
		'' AS [LN_RDC_PAY_PCV_AGG],
		'Y' AS [LON09_DAT_ENT_IND],
		'' AS [LA_R78_INT_UPD],
		disc.LD_NPD_PCV AS [LD_NPD_PCV],
		'' AS [LD_NSI_LST_PD_PCV],
		'' AS [LI_RBD_RGL_CAT],
		TOT_BAL.BAL AS [LA_PT_PAY_PCV], 
		'' AS [LA_TOT_INT_PAID_PCV],
		ISNULL(IBR.standard_standard_amount, '') AS [LA_STD_STD_ISL_PCV],
		dbo.CONVERT_DATE(IBR.ibr_begin_date) AS [LD_25_YR_FGV_PCV], 
		ISNULL(IBR.ibr_forgiveness_months_counter, '') AS [LN_IBR_QLF_PAY_PCV],
		LST_PMT.LAST_PMT_DTE AS [LD_BRW_LST_PMT_PCV], 
		dbo.CONVERT_DATE(loan.ln_rehab_date) AS [LD_LON_RHB_PCV], 
		'' AS [LA_LON_RHB_PCV], 
		'' AS [LA_TOT_PRI_PD_PCV], 
		'' AS [LA_ORG_CAP_INT], 
		'' AS [LN_IBR_EHD_DFR_USE],
		'' AS [IF_LON_SRV_DFL_LON],
		'' AS [LA_TOT_INT_OTS_RHB],
		'' AS [LA_TOT_COL_CST_RHB],
		'' AS [LN_PSV_FGV_PAY_CTR],
		IBR.principal_balance_at_repayment_begin_date AS [LA_PRI_BAL_RPY_BEG], 
		IBR.ibr_forgiveness_months_counter AS [LN_IBR_FGV_MTH_CTR], 
		'' AS [LN_ICR_FGV_MTH_CTR],
		'' AS [LN_ICR_ON_TME_PAY],
		'' AS [LD_NEG_AMR_BEG], 
		'' AS [LA_NEG_AMR_PAY], 
		'' AS [LN_ICR_NEG_AMR_MTH],
		'' AS [LD_ICR_CAP_INT],
		'' AS [LI_TEN_CAP_THD_RCH], 
		'' AS [LA_CUM_NEG_INT_CAP], 
		'' AS [LA_IBR_NEG_AMT_INT], 
		'' AS [LA_ICR_INT_CAP_LTR],
		'' AS [LC_SPS_INC_SRC], 
		'' AS [LC_CON_LON_DSB_PIO], 
		'' AS [LC_RPD_TYP], 
		CASE
			WHEN GRP.gr_num_term_period_remain = '000' THEN ''
			ELSE GRP.gr_num_term_period_remain
		END AS [LN_RPD_TRM_RMN], 
		'' AS [LI_RPD_IBR_DLQ_FOR], 
		'' AS [LN_MTH_GRC_DFR_DLC],
		'Y' AS [LON10_DAT_ENT_IND],
		map.CommpassLoanSeq AS [LN_SEQ], 
		map.CompassLoanProgram as [IC_LON_PGM], 
		BL.lender_code AS [IF_DOE_LDR], 
		CASE
			WHEN loan.ga_id = 'CSAC' THEN '000706'
			WHEN loan.ga_id = 'CSLP' THEN '000708'
			WHEN loan.ga_id = 'USAF' OR loan.ga_id = 'USAG' THEN '000800'
			WHEN loan.ga_id = 'ECMC' THEN '000951' 
			WHEN loan.ga_id = 'NSLP' THEN '000731'
            WHEN loan.ga_id = 'MGSLP' THEN '000730'
            WHEN loan.ga_id = 'EAC' THEN '000755'
            ELSE ''
		END AS [IF_GTR],
		CASE
			WHEN loan.ln_loan_type = 'PLUS' THEN loan.[br_benefiting _student_ssn] ELSE loan.br_ssn
		END AS [LF_STU_SSN],
		'' AS [LA_CUR_ILG],
		loan.ln_curr_principal AS [LA_CUR_PRI],
		'' AS [LA_ILG],
		loan.ln_orig_loaned_amt AS [LA_LON_AMT_GTR],
		cast(cast(CAST(ln_curr_interest AS MONEY) / 100000 as decimal(10,2)) * 100 as int) AS [LA_NSI_OTS],
		'' AS [LA_R78_INT_MAX],
		'' AS [LA_R78_INT_PD],
		'' AS [LI_1_TME_BR],
		'' AS [LC_RPR_TYP],
		'' AS [LD_CAP_LST_PIO_CVN],
		CASE
			WHEN loan.ln_loan_type IN ('STAF', 'STAU') THEN disc.LD_END_GRC_PRD 
            ELSE '' 
		END AS [LD_END_GRC_PRD],
		'' AS [LD_GTE_LOS], 
		'' AS [LD_ILG_NTF],
		dbo.CONVERT_DATE(loan.ln_guarantee_date) AS [LD_LON_GTR],
		'05/04/2014' AS [LD_NSI_ACR_THU], 
		dbo.CONVERT_DEFFORB_DATE(loan.ln_promissory_note_date) AS [LD_PNT_SIG],
		'' AS [LD_SCL_CLS_NTF],
		dbo.CONVERT_DATE(loan.ln_period_starting_date) AS [LD_TRM_BEG],
		dbo.CONVERT_DATE(loan.ln_period_ending_date) AS [LD_TRM_END],
		loan.si_school_code + loan.si_branch AS [LF_DOE_SCL_ORG],
		LEFT(loan.ln_guarantee_loan_id, 12) AS [LF_GTR_RFR],
		'Y' AS [LI_CAP_ALW],
		'Y' AS [LI_ELG_SPA],
		'N' AS [LI_FGV_PGM],
		'N' AS [LI_GTR_NAT],
		CASE
			WHEN loan.ln_loan_type IN ('STAF' ,'STAU') THEN loan.ln_length_of_grace
            ELSE ''
		END AS [LN_MTH_GRC_PRD_DSC],
		'' AS [LA_SIN_OTS_PCV],
		'05/04/2014' AS [LD_SIN_ACR_THU_PCV],
		'05/04/2014' AS [LD_SIN_LST_PD_PCV],
		'' AS [LC_STA_NEW_BR],
		'' AS [LC_SCY_PGA],
		dbo.CONVERT_DATE(loan.ln_1st_disb_date) AS [LD_LON_1_DSB],
		loan.ln_grade_level AS [LC_ACA_GDE_LEV], 
		'' AS [LC_SCY_PGA_PGM_YR],
		'' AS [IC_HSP_CSE],
		CASE
			WHEN loan.ln_loan_type = 'CONS' OR loan.ln_loan_type = 'CONU' THEN 'N'
		END AS [LI_TL4_793_XCL_CON],
		bor.br_school_defer_requested AS [LI_DFR_REQ_ON_APL],
		'' AS [LI_LN_PT_COM_APL],
		CASE
			WHEN loan.ln_loan_type = 'CONS' OR loan.ln_loan_type = 'CONU' THEN loan.ln_interest_rate
		END AS [LR_WIR_CON_LON],
		'' AS [LC_ELG_RDC_PGM],
		'' AS [LD_ELG_RDC_PGM],
		'' AS [LC_RPD_SLE],
		'' AS [LR_ITR_ORG],
		'' AS [LC_ITR_TYP_ORG],
		'' AS [LC_TIR_GRP],
		'' AS [IF_TIR_PCE],
		'' AS [LN_RDC_PGM_PAY_PCV],
		CASE	
			WHEN BB.br_ssn IS NULL THEN 'X'
			WHEN BB.br_ssn IS NOT NULL THEN 'Q'
			ELSE 'X'
		END AS [LI_RTE_RDC_ELG],
		CASE
			WHEN loan.ln_curr_fees = '0000000' THEN ''
			ELSE loan.ln_curr_fees
		END AS [LA_LTE_FEE_OTS],
		'' AS [LD_LON_LTE_FEE_WAV],
		CASE
			WHEN GRP_BB.B02 = 'Y' AND GRP_BB.B03 != 'Y' AND GRP_BB.B07 != 'Y' AND GRP_BB.C02 != 'Y' AND GRP_BB.B06 != 'Y' THEN 'O24'
			WHEN GRP_BB.B07 = 'Y' AND GRP_BB.B02 != 'Y' AND GRP_BB.B03 != 'Y' AND GRP_BB.C02 != 'Y' AND GRP_BB.B06 != 'Y' THEN 'W24'
			WHEN GRP_BB.C02 = 'Y' AND GRP_BB.B02 != 'Y' AND GRP_BB.B03 != 'Y' AND GRP_BB.B07 != 'Y' AND GRP_BB.B06 != 'Y' THEN 'W24'
			WHEN GRP_BB.B03 = 'Y' AND GRP_BB.B02 != 'Y' AND GRP_BB.C02 != 'Y' AND GRP_BB.B07 != 'Y' AND GRP_BB.B06 != 'Y' THEN 'GRD'
			WHEN GRP_BB.B03 = 'Y' AND GRP_BB.B02 = 'Y' THEN 'WY2'
			WHEN (GRP_BB.B03 = 'Y' AND GRP_BB.B07 = 'Y') OR GRP_BB.C02 = 'Y' THEN 'WY3'
			ELSE 'O24'
		END AS [LC_CUR_RDC_PGM_NME],
		'' AS [LI_RIR_SCY_ELG],
		loan.ln_cml_unique_loan_id AS [LF_LON_ALT],
		loan.ln_cml_seq_nbr AS [LN_LON_ALT_SEQ],
		'N' AS [LI_LDR_LST_RST_DSB],
		CASE
			WHEN loan.ln_loan_type = 'CONS' THEN
				CASE
					WHEN loan.ln_application_date <> '' OR loan.ln_application_date IS NOT NULL THEN dbo.CONVERT_DATE(loan.ln_application_date)
					WHEN loan.ln_application_date = '' OR loan.ln_application_date IS NULL THEN dbo.CONVERT_DATE(loan.ln_1st_disb_date)
				END
			WHEN loan.ln_loan_type = 'CONU' THEN
				CASE
					WHEN loan.ln_application_date <> '' OR loan.ln_application_date IS NOT NULL THEN dbo.CONVERT_DATE(loan.ln_application_date)
					WHEN loan.ln_application_date = '' OR loan.ln_application_date IS NULL THEN dbo.CONVERT_DATE(loan.ln_1st_disb_date)
				END
		END AS [LD_LON_APL_RCV],
		CASE
			WHEN loan.ln_loan_type = 'STAF' OR loan.ln_loan_type = 'STAU' THEN 'M'
			WHEN loan.ln_loan_type = 'PLUS' THEN 'Q'
			WHEN loan.ln_loan_type = 'GRAD' THEN 'G'
		END AS [LC_MPN_TYP],
		'' AS [LD_MPN_EXP],
		CASE
			WHEN loan.ln_loan_type NOT IN ('CONS','CONU') THEN 'A'
			ELSE ''
		END AS [LC_MPN_SRL_LON],
		'' AS [LC_MPN_REV_REA],
		'' AS [LF_ORG_RGN],
		'' AS [LD_AMR_BEG],
		'' AS [LD_ORG_XPC_GRD],
		'' AS [LR_SCL_SUB],
		'' AS [LI_LDR_BG_APL],
		CASE
			WHEN loan.ln_loan_type IN ('CONS' ,'CONU') THEN 
				(SELECT SUM((CAST(ln.ln_orig_loaned_amt AS int)))  FROM ITELSQLDF_Loan ln WHERE ln.br_ssn = bor.br_ssn)
		END AS [LA_TOT_EDU_DET],
		'' AS [LF_MN_MST_NTE],
		'' AS [LN_MN_MST_NTE_SEQ],
		'' AS [PC_PNT_YR],
		'' AS [LF_CRD_RTE_SRE],
		'' AS [LC_ST_BR_RSD_APL],
		'' AS [LA_INT_FEE_URP_IRS],
		'' AS [LI_MN_PSD_BS],
		'' AS [LF_ESG_SRC],
		'' AS [LI_MNT_BIL_RCP],
		'' AS [LA_BS_POI],
		'' AS [LC_ESG],
		CASE
			WHEN loan.ln_loan_type = 'CONS' OR loan.ln_loan_type = 'CONU' THEN 'B'
		END AS [LC_UDL_DSB_COF],
		CASE	
			WHEN BB.br_ssn IS NULL THEN '0'
			WHEN BB.br_ssn IS NOT NULL THEN '25'
			ELSE '0'
		END AS [LN_BBS_PCV_PAY_MOT],
		'' AS [LI_BR_LT_HT],
		'' AS [LC_ESP_RPD_OPT_SEL],
		'' AS [LD_BBS_DSQ],
		'' AS [LC_BBS_DSQ_REA],
		'' AS [LC_ELG_95_SPA_BIL],
		'' AS [LF_GTR_RFR_XTN],
		'' AS [LC_SGM_COS_PRC],
		'' AS [LI_OO_PST_ENR_DFR],
		'' AS [LD_OO_PST_ENR_DFR],
		CASE
			WHEN loan.ln_loan_type = 'PLUS' THEN 'N'
			WHEN LOAN.ln_loan_type = 'CONU' THEN ''
			WHEN loan.ln_loan_type IN ('STAF','STAU', 'GRAD', 'CONS') THEN 'Y'
			ELSE ''
		END AS [LC_TL4_IBR_ELG], 
		'' AS [LA_MSC_FEE_OTS],
		'' AS [LA_MSC_FEE_PCV_OTS],
		'' AS [LF_FED_CLC_RSK],
		'' AS [LF_FED_FFY_1_DSB],
		'' AS [LC_FED_PGM_YR],
		'' AS [LF_PRV_GTR],
		'' AS [LA_INT_RCV_GOV],
		'' AS [LC_VRS_ALT_APL],
		'' AS [LF_LON_DCV_CLI],
		'' AS [LN_LON_SEQ_DCV_CLI],
		COALESCE(dbo.CONVERT_DATE(IBR.ibr_begin_date), DISC_DATE.Newest_Disc_Date, NULL) AS [LD_LON_IBR_ENT],
		CASE 
			WHEN BAL.TOTAL_BALANCE IS NULL THEN 'N' 
			WHEN BAL.TOTAL_BALANCE > CAST(30000 AS MONEY) THEN 'Y'
			ELSE 'N'
		END  AS [LI_BR_DET_RPD_XTN],
		'' AS [LD_EFF_LBR_RTE],
		'' AS [LD_FAT_PRI_BAL_ZRO],
		'N' AS [LN_LON12_DAT_OCC_CNT],
		'Y' AS [LON35_DAT_ENT_IND],
		BL.lender_code AS [IF_OWN],
		'00002014' AS [IF_BND_ISS], 
		'' AS [IF_LON_SLE],
		'SVT' AS [LC_LOC_PNT], 
		'' AS [LD_OWN_EFF_SR],
		'' AS [LF_BR_LON_OWN_ACC], 
		'' AS [LF_CUR_POR],
		'' AS [LF_OWN_ORG_POR],
		'' AS [IF_TIR_PCE1],
		'' AS [LD_LON_IRL_SLE_TRF],
		'' AS [LF_OWN_EFT_RIR_ASN],
		'' AS [LA_LON_LVL_TRF_FEE],
		'' AS [LD_PRE_CVN_OWN_BEG],
		'' AS [LN_LON32_DAT_OCC_CNT],
		'Y' AS [LON72_DAT_ENT_IND],
		CASE
			WHEN loan.ln_loan_type = 'STAF' OR loan.ln_loan_type = 'CONS' THEN 'Y' ELSE 'N'
		END AS [LC_ELG_SIN],
		CASE
			WHEN loan.ln_loan_type IN ('CONS' ,'CONU' ,'GRAD') THEN 'F1'
			WHEN loan.ln_loan_type = 'PLUS' AND dbo.CONVERT_DATE(loan.ln_1st_disb_date) >= '07/01/06' THEN 'F1'
			WHEN loan.ln_loan_type = 'PLUS' AND dbo.CONVERT_DATE(loan.ln_1st_disb_date) < '07/01/06' THEN 'SV'
			WHEN (loan.ln_loan_type IN ('STAF', 'STAU') AND dbo.CONVERT_DATE(loan.ln_1st_disb_date) >= '07/01/06') THEN 'F1'
			WHEN (loan.ln_loan_type IN ('STAF', 'STAU')
				AND (dbo.CONVERT_DATE(loan.ln_1st_disb_date) >= '07/01/95' AND dbo.CONVERT_DATE(loan.ln_1st_disb_date) < '07/01/06')
				AND (dbo.CONVERT_DATE(loan.ln_conversion_date) > '05/05/2014')) THEN 'C1' 
			WHEN (loan.ln_loan_type IN ('STAF', 'STAU')
				AND (dbo.CONVERT_DATE(loan.ln_1st_disb_date) >= '07/01/95' AND dbo.CONVERT_DATE(loan.ln_1st_disb_date) < '07/01/06')
				AND (dbo.CONVERT_DATE(loan.ln_conversion_date) <=  '05/05/2014')
				AND (LEFT(loan.ln_status, 1) = 'D')) THEN 'C1' 
			WHEN (loan.ln_loan_type IN ('STAF' ,'STAU')
				AND (dbo.CONVERT_DATE(loan.ln_1st_disb_date) >= '07/01/95' AND dbo.CONVERT_DATE(loan.ln_1st_disb_date) < '07/01/06')
				AND (dbo.CONVERT_DATE(loan.ln_conversion_date) <=  '05/05/2014')
				AND (LEFT(loan.ln_status, 1) <> 'D')) THEN 'C2' 
			WHEN (loan.ln_loan_type IN ('STAF', 'STAU')
				AND dbo.CONVERT_DATE(loan.ln_1st_disb_date) < '07/01/95'
				AND loan.ln_interest_rate = ' ') THEN 'F1'
			WHEN (loan.ln_loan_type IN ('STAF' ,'STAU')
				AND dbo.CONVERT_DATE(loan.ln_1st_disb_date) < '07/01/95'
				AND loan.ln_interest_rate <> ' ') THEN 'SV'
		END AS [LC_ITR_TYP],
		'A' AS [LC_STA_LON72],
		GETDATE() AS [LD_STA_LON72],
		'N' AS [LI_SPC_ITR],
		loan.ln_interest_rate AS [LR_ITR],
		'' AS [LR_INT_RDC_PGM_ORG],
		'Y' AS [LON84_DAT_ENT_IND],
		CASE
			WHEN GRP_BB.B02 = 'Y' AND GRP_BB.B03 != 'Y' AND GRP_BB.B07 != 'Y' AND GRP_BB.C02 != 'Y' AND GRP_BB.B06 != 'Y' THEN 'O24'
			WHEN GRP_BB.B07 = 'Y' AND GRP_BB.B02 != 'Y' AND GRP_BB.B03 != 'Y' AND GRP_BB.C02 != 'Y' AND GRP_BB.B06 != 'Y' THEN 'W24'
			WHEN GRP_BB.C02 = 'Y' AND GRP_BB.B02 != 'Y' AND GRP_BB.B03 != 'Y' AND GRP_BB.B07 != 'Y' AND GRP_BB.B06 != 'Y' THEN 'W24'
			WHEN GRP_BB.B03 = 'Y' AND GRP_BB.B02 != 'Y' AND GRP_BB.C02 != 'Y' AND GRP_BB.B07 != 'Y' AND GRP_BB.B06 != 'Y' THEN 'GRD'
			WHEN GRP_BB.B03 = 'Y' AND GRP_BB.B02 = 'Y' THEN 'WY2'
			WHEN (GRP_BB.B03 = 'Y' AND GRP_BB.B07 = 'Y') OR GRP_BB.C02 = 'Y' THEN 'WY3'
			ELSE 'O24'
		END AS [LC_RDC_PGM_NME],
		'' AS [LD_RDC_EFF_BEG],
		'' AS [LD_RDC_EFF_END],
		'A' AS [LC_STA_LON84],
		CASE
			WHEN GRP_BB.B02 = 'Y' AND GRP_BB.B03 != 'Y' AND GRP_BB.B07 != 'Y' AND GRP_BB.C02 != 'Y' AND GRP_BB.B06 != 'Y' THEN '01000'
			WHEN GRP_BB.B07 = 'Y' AND GRP_BB.B02 != 'Y' AND GRP_BB.B03 != 'Y' AND GRP_BB.C02 != 'Y' AND GRP_BB.B06 != 'Y' THEN '01000'
			WHEN GRP_BB.C02 = 'Y' AND GRP_BB.B02 != 'Y' AND GRP_BB.B03 != 'Y' AND GRP_BB.B07 != 'Y' AND GRP_BB.B06 != 'Y' THEN '01000'
			WHEN GRP_BB.B03 = 'Y' AND GRP_BB.B02 != 'Y' AND GRP_BB.C02 != 'Y' AND GRP_BB.B07 != 'Y' AND GRP_BB.B06 != 'Y' THEN '01000'
			WHEN GRP_BB.B03 = 'Y' AND GRP_BB.B02 = 'Y' THEN '02000'
			WHEN (GRP_BB.B03 = 'Y' AND GRP_BB.B07 = 'Y') OR GRP_BB.C02 = 'Y' THEN '03000'
			ELSE '01000'
		END AS [LR_RDC],
		'N' AS [LON86_DAT_ENT_IND],
		'' AS [LC_RDC_PGM_NME1],
		'' AS [LD_RDC_EFF_AGG_BEG],
		'' AS [LD_RDC_EFF_AGG_END],
		'' AS [LR_RDC_AGG],
		CASE
            WHEN TDI.unpaid_oid_orig_fee != '' OR TDI.unpaid_oid_cap_int != '' THEN 'Y'
            ELSE 'N'
        END AS [TAX_DAT_ENT_IND], 
		CASE
			WHEN TDI.unpaid_oid_orig_fee IS NOT NULL THEN LOAN_BAL.BALANCE
			ELSE NULL
		END AS [TA_OID_PD_RPT], 
		ISNULL(TDI.unpaid_oid_cap_int, '') AS [TA_CAP_ELG], 
		ISNULL(TDI.unpaid_oid_orig_fee, '') AS [TA_GTR_OID_ELG], 
		'' AS [TA_GTR_CAP_ELG], 
		CASE
			WHEN loan.ln_loan_type IN ('CONS' ,'CONU') THEN '1'
            ELSE '0'
		END AS [LN_LON74_DAT_OCC_CNT],
		'' AS [LI_LON_WIR_OVR],
		'' AS [LR_LON_WIR_PRV],
		'0' AS [LN_LON26_DAT_OCC_CNT],
		'N' AS [FS10_DAT_ENT_IND],
		'' AS [LF_FED_AWD],
		'' AS [LN_FED_AWD_SEQ],
		'' AS [LD_ITL_LN_SLD_DOE],
		'' AS [LF_ORG_DL_BR_SSN],
		'' AS [LD_ORG_DL_BR_DOB],
		'' AS [LF_ORG_DL_STU_SSN],
		'' AS [LD_ORG_DL_STU_DOB],
		'' AS [LF_ORG_DL_EDS_SSN],
		'' AS [LF_DL_FIN_AWD_YR],
		'' AS [LD_DL_STU_ACA_BEG],
		'' AS [LD_DL_STU_ACA_END],
		'' AS [LI_DL_BR_ELG_HPA],
		'' AS [LC_DL_STU_DEP_STA],
		'' AS [LF_ORG_DL_LON],
		'' AS [LC_DL_STA_MPN],
		'' AS [LF_DL_MPN],
		'' AS [LI_DL_XED_USB_LMT],
		'' AS [LI_FRC_ICR],
		'' AS [LC_DL_PRV_RPY_PLN],
		'' AS [LF_FED_LON_PR_GRP]
	FROM
		ITELSQLDF_Loan loan
		LEFT JOIN
		(
			SELECT 
				br_ssn,
				ln_num,
				cast(cast((CAST(ln_curr_interest AS MONEY) / 100000) + (CAST(ln_curr_principal AS MONEY ) / 100) as decimal(10,2)) * 100 as int) AS BALANCE
			FROM [dbo].[ITELSQLDF_Loan] 
		) LOAN_BAL
		ON LOAN_BAL.br_ssn = loan.br_ssn
		AND LOAN_BAL.ln_num = loan.ln_num
		LEFT JOIN
		(
			SELECT DISTINCT
				br_ssn,
				SUM((CAST(ln_curr_interest AS MONEY) / 100000) + (CAST(ln_curr_principal AS MONEY ) / 100)) AS TOTAL_BALANCE
			FROM
				[dbo].[ITELSQLDF_Loan]
			GROUP BY 
				br_ssn
		) BAL
			ON BAL.br_ssn = loan.br_ssn
		INNER JOIN [dbo].[BorrowerLenderCodeMapping] BL
			ON loan.br_ssn = BL.br_ssn
		LEFT JOIN
		(
			SELECT DISTINCT
				Loan.br_ssn,
				Loan.ln_num,
				cast((((CAST(G.br_partial_pmt_amt AS MONEY) / 100) * (CAST(Loan.ln_orig_loaned_amt  AS MONEY) / 100)) / ORIG_SUM.ORIG_AMT_LOANED) as MONEY) AS BAL
			FROM
				[dbo].[ITELSQLDF_Loan] Loan
			INNER JOIN 
			(
				SELECT DISTINCT
					l.br_ssn,
					l.gr_id,
					SUM((CAST(ln_orig_loaned_amt AS MONEY) / 100)) AS ORIG_AMT_LOANED
				FROM
					[dbo].[ITELSQLDF_Loan] L
				INNER JOIN [dbo].[ITEKSQLDF_Group] G
					ON G.br_ssn = L.br_ssn
					AND L.gr_id = G.gr_id
				GROUP BY 
					l.br_ssn,
					l.gr_id
			) ORIG_SUM
				ON ORIG_SUM.br_ssn = Loan.br_ssn
				and ORIG_SUM.gr_id = Loan.gr_id
			INNER JOIN [dbo].[ITEKSQLDF_Group] g
				on g.br_ssn = loan.br_ssn
				and g.gr_id = loan.gr_id
			
		) AS TOT_BAL
			ON TOT_BAL.br_ssn = loan.br_ssn
			and TOT_BAL.ln_num = loan.ln_num
		LEFT JOIN
		(
			SELECT DISTINCT
				br_ssn,
				ln_num,
				MAX(CASE WHEN incentive_code='A01' THEN 'Y' ELSE '' END) AS A01,
				MAX(CASE WHEN incentive_code='B02' THEN 'Y' ELSE '' END) AS B02,
				MAX(CASE WHEN incentive_code='B03' THEN 'Y' ELSE '' END) AS B03,
				MAX(CASE WHEN incentive_code='B06' THEN 'Y' ELSE '' END) AS B06,
				MAX(CASE WHEN incentive_code='B07' THEN 'Y' ELSE '' END) AS B07,
				MAX(CASE WHEN incentive_code='C02' THEN 'Y' ELSE '' END) AS C02
			FROM [dbo].[ITLTSQLDF_Borr_Bene]
			WHERE [status] = 'A'
			GROUP BY 
				br_ssn,
				ln_num
		) GRP_BB
			ON GRP_BB.br_ssn = loan.br_ssn
			AND GRP_BB.ln_num = loan.ln_num
		LEFT JOIN
		(
			SELECT DISTINCT
				br_ssn
			FROM 
				[dbo].[ITLTSQLDF_Borr_Bene]
			WHERE 
				[status] = 'A'
				AND incentive_code IN ('B02','B03', 'B07','C02')
		) BB
			ON BB.br_ssn = loan.br_ssn
		LEFT JOIN
		(
			SELECT
				br_ssn,
				loan_num,
				RIGHT(MAX(unpaid_oid_orig_fee), 9) AS unpaid_oid_orig_fee,
				RIGHT(MAX(unpaid_oid_cap_int), 8) AS unpaid_oid_cap_int
			FROM
				[dbo].[ITGNSQLDF_TDI_OID]
			WHERE
				[year] = '2013'
			GROUP BY
				br_ssn,
				loan_num
		) TDI
            ON TDI.br_ssn = loan.br_ssn
            AND TDI.loan_num = loan.ln_num
          
        LEFT JOIN [dbo].[CompassLoanMapping] MAP
            ON MAP.br_ssn = LOAN.br_ssn
            AND MAP.NelNetLoanSeq = LOAN.ln_num
        LEFT JOIN ITLSSQLDF_IBR IBR
            ON IBR.br_ssn = loan.br_ssn
            AND IBR.ln_num = loan.ln_num
		LEFT JOIN ITEJSQLDF_Borrower bor
			ON loan.br_ssn = bor.br_ssn
		LEFT JOIN [dbo].[ITEKSQLDF_Group] GRP
			ON GRP.br_ssn = loan.br_ssn
			AND GRP.gr_id = loan.gr_id
        LEFT JOIN
            (
                SELECT DISTINCT
                    br_ssn,
                    gr_id,
                    MAX(dbo.CONVERT_DATE(gr_last_pmt_date)) AS LAST_PMT_DTE
                FROM
                    [dbo].[ITEKSQLDF_Group]
                GROUP BY 
                    br_ssn,
                    gr_id
            )LST_PMT
                on LST_PMT.br_ssn = loan.br_ssn
                and LST_PMT.gr_id = loan.gr_id
		LEFT JOIN
		(
			SELECT DISTINCT
				D.br_ssn,
				group_id,
				MAX(dbo.CONVERT_DATE([disclosure_date])) AS Newest_Disc_Date
			FROM
				[dbo].[ITLQSQLDF_Disclosure] D
			INNER JOIN [dbo].[ITLSSQLDF_IBR] I
				ON I.br_ssn = D.br_ssn
			GROUP BY 
				D.br_ssn,
				group_id
		) DISC_DATE
			ON DISC_DATE.br_ssn = loan.br_ssn
			and DISC_DATE.group_id = loan.gr_id
		LEFT JOIN 
			(
				SELECT DISTINCT
					grp.br_ssn,
					grp.gr_id,
					MAX(dbo.CONVERT_DATE(grp.gr_due_date)) AS [LD_NPD_PCV],
					MAX(dbo.CONVERT_DATE(grp.gr_last_pmt_date)) AS [LD_BRW_LST_PMT_PCV],
					MAX(dbo.CONVERT_DATE(grp.gr_conversion_date)) AS [LD_END_GRC_PRD]
				FROM
					ITEKSQLDF_Group grp
				GROUP BY
					grp.br_ssn,
					grp.gr_id,
					grp.gr_due_date,
					grp.gr_last_pmt_date,
					grp.gr_conversion_date
						
			) disc ON bor.br_ssn = disc.br_ssn
				AND loan.gr_id = disc.gr_id
	WHERE
		loan.br_ssn = @SSN
RETURN 0