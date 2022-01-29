CREATE PROCEDURE [dbo].[GetFinancialData]
	@SSN varchar(9)
AS
	SELECT
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
		'' AS [LF_BR_OWN_ACC_AGG], --TODO
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
		'' AS [LA_PT_PAY_PCV], --TODO: Need to figure out math
		'' AS [LA_TOT_INT_PAID_PCV],
		ISNULL(IBR.standard_standard_amount, '') AS [LA_STD_STD_ISL_PCV],
		ISNULL(DBO.CONVERT_DATE(IBR.ibr_begin_date), '') AS [LD_25_YR_FGV_PCV], 
		'' AS [LN_IBR_QLF_PAY_PCV], --TODO
		LST_PMT.LAST_PMT_DTE AS [LD_BRW_LST_PMT_PCV], 
		DBO.CONVERT_DATE(loan.ln_rehab_date) AS [LD_LON_RHB_PCV], 
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
		'' AS [LD_NEG_AMR_BEG], --TODO
		'' AS [LA_NEG_AMR_PAY], --TODO
		'' AS [LN_ICR_NEG_AMR_MTH],
		'' AS [LD_ICR_CAP_INT],
		'' AS [LI_TEN_CAP_THD_RCH], --TODO
		'' AS [LA_CUM_NEG_INT_CAP], --TODO
		'' AS [LA_IBR_NEG_AMT_INT], --TODO
		'' AS [LA_ICR_INT_CAP_LTR],
		'' AS [LC_SPS_INC_SRC], --TODO
		'' AS [LC_CON_LON_DSB_PIO], --TODO
		'' AS [LC_RPD_TYP], --TODO
		'' AS [LN_RPD_TRM_RMN], --TODO
		'' AS [LI_RPD_IBR_DLQ_FOR], --TODO
		'' AS [LN_MTH_GRC_DFR_DLC],
		'Y' AS [LON10_DAT_ENT_IND],
		map.CommpassLoanSeq AS [LN_SEQ], 
		map.CompassLoanProgram as [IC_LON_PGM], 
		'826717' AS [IF_DOE_LDR], --TODO this will change depending on ACH benefit which is yet to be determined
		CASE
			WHEN loan.ga_id = 'CSAC' THEN '000706'
			WHEN loan.ga_id = 'CSLP' THEN '000708'
			WHEN loan.ga_id = 'USAF' OR loan.ga_id = 'USAG' THEN '000800'
			WHEN loan.ga_id = 'ECMC' THEN '000927' 
			WHEN loan.ga_id = 'NSLP' THEN '000731'
            WHEN loan.ga_id = 'MGSLP' THEN '000730'
            WHEN loan.ga_id = 'EAC' THEN '000755'
            ELSE 'ERROR'
		END AS [IF_GTR],
		CASE
			WHEN loan.ln_loan_type = 'PLUS' THEN loan.[br_benefiting _student_ssn] ELSE loan.br_ssn
		END AS [LF_STU_SSN],
		'' AS [LA_CUR_ILG],
		loan.ln_curr_principal AS [LA_CUR_PRI],
		'' AS [LA_ILG],
		loan.ln_orig_loaned_amt AS [LA_LON_AMT_GTR],
		loan.ln_curr_interest AS [LA_NSI_OTS],
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
		'' AS [LD_NSI_ACR_THU], --TODO: TOC setup for ZC_TAXLA_CUTOFF_DT
		dbo.CONVERT_DATE(loan.ln_promissory_note_date) AS [LD_PNT_SIG],
		'' AS [LD_SCL_CLS_NTF],
		dbo.CONVERT_DATE(loan.ln_period_starting_date) AS [LD_TRM_BEG],
		dbo.CONVERT_DATE(loan.ln_period_ending_date) AS [LD_TRM_END],
		loan.si_school_code + loan.si_branch AS [LF_DOE_SCL_ORG],
		'' AS [LF_GTR_RFR],--TODO
		'Y' AS [LI_CAP_ALW],
		'Y' AS [LI_ELG_SPA],
		'N' AS [LI_FGV_PGM],
		'N' AS [LI_GTR_NAT],
		CASE
			WHEN loan.ln_loan_type IN ('STAF' ,'STAU') THEN loan.ln_length_of_grace
            ELSE ''
		END AS [LN_MTH_GRC_PRD_DSC],
		'' AS [LA_SIN_OTS_PCV],
		'' AS [LD_SIN_ACR_THU_PCV],
		'' AS [LD_SIN_LST_PD_PCV],
		'' AS [LC_STA_NEW_BR],
		'' AS [LC_SCY_PGA],
		dbo.CONVERT_DATE(loan.ln_1st_disb_date) AS [LD_LON_1_DSB],
		loan.ln_grade_level AS [LC_ACA_GDE_LEV], --TODO: May have to convert values
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
		'' AS [LI_RTE_RDC_ELG],
		loan.ln_curr_fees AS [LA_LTE_FEE_OTS],
		'' AS [LD_LON_LTE_FEE_WAV],
		'' AS [LC_CUR_RDC_PGM_NME],
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
		CASE
			WHEN loan.ln_loan_type <> 'STAF' OR loan.ln_loan_type <> 'GRAD' THEN 'A'
            ELSE '' 
		END AS [LD_MPN_EXP],
		'' AS [LC_MPN_SRL_LON],
		'' AS [LC_MPN_REV_REA],
		'' AS [LF_ORG_RGN],
		'' AS [LD_AMR_BEG],
		'' AS [LD_ORG_XPC_GRD],
		'' AS [LR_SCL_SUB],
		'' AS [LI_LDR_BG_APL],
		'' AS [LI_ESG],
		CASE
			WHEN loan.ln_loan_type IN ('CONS' ,'CONU') THEN 
				(SELECT SUM(CAST(ln.ln_orig_loaned_amt AS int)) FROM ITELSQLDF_Loan ln WHERE ln.br_ssn = bor.br_ssn)
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
		'' AS [LN_BBS_PCV_PAY_MOT],
		'' AS [LI_BR_LT_HT],
		'' AS [LC_ESP_RPD_OPT_SEL],
		'' AS [LD_BBS_DSQ],
		'' AS [LC_BBS_DSQ_REA],
		'' AS [LC_ELG_95_SPA_BIL],
		'' AS [LF_GTR_RFR_XTN],
		'' AS [LC_SGM_COS_PRC],
		'' AS [LI_OO_PST_ENR_DFR],
		'' AS [LD_OO_PST_ENR_DFR],
		'' AS [LC_TL4_IBR_ELG], --TODO
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
		ISNULL(IBR.ibr_begin_date, '') AS [LD_LON_IBR_ENT], 
		'' AS [LI_BR_DET_RPD_XTN], --TODO
		'' AS [LD_EFF_LBR_RTE],
		'' AS [LD_FAT_PRI_BAL_ZRO],
		'' AS [LN_LON12_DAT_OCC_CNT],
		'' AS [LON35_DAT_ENT_IND],
		loan.ld_lender_id AS [IF_OWN],
		'' AS [IF_BND_ISS], --TODO: Get from accounting
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
				AND (dbo.CONVERT_DATE(loan.ln_conversion_date) > GETDATE())) THEN 'C1' --TODO: Find out the cutoff date
			WHEN (loan.ln_loan_sold_date IN ('STAF', 'STAU')
				AND (dbo.CONVERT_DATE(loan.ln_1st_disb_date) >= '07/01/95' AND dbo.CONVERT_DATE(loan.ln_1st_disb_date) < '07/01/06')
				AND (dbo.CONVERT_DATE(loan.ln_conversion_date) <=  GETDATE())
				AND (LEFT(loan.ln_status, 1) = 'D')) THEN 'C1' --TODO: Find out the cutoff date
			WHEN (loan.ln_loan_type IN ('STAF' ,'STAU')
				AND (dbo.CONVERT_DATE(loan.ln_1st_disb_date) >= '07/01/95' AND dbo.CONVERT_DATE(loan.ln_1st_disb_date) < '07/01/06')
				AND (dbo.CONVERT_DATE(loan.ln_conversion_date) <=  GETDATE())
				AND (LEFT(loan.ln_status, 1) <> 'D')) THEN 'C2' --TODO: Find out the cutoff date
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
		'N' AS [LON84_DAT_ENT_IND],
		'' AS [LC_RDC_PGM_NME],
		'' AS [LD_RDC_EFF_BEG],
		'' AS [LD_RDC_EFF_END],
		'' AS [LC_STA_LON84],
		'' AS [LR_RDC],
		'N' AS [LON86_DAT_ENT_IND],
		'' AS [LC_RDC_PGM_NME1],
		'' AS [LD_RDC_EFF_AGG_BEG],
		'' AS [LD_RDC_EFF_AGG_END],
		'' AS [LR_RDC_AGG],
		CASE
            WHEN TDI.unpaid_oid_orig_fee != '' OR TDI.unpaid_oid_cap_int != '' THEN 'Y'
            ELSE 'N'
        END AS [TAX_DAT_ENT_IND], 
		REPLACE(ISNULL(TDI.unpaid_oid_orig_fee, '' ),'.','') AS [TA_OID_PD_RPT], 
		REPLACE(ISNULL(TDI.unpaid_oid_cap_int, ''),'.','') AS [TA_CAP_ELG], 
		'' AS [TA_GTR_OID_ELG], 
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
        LEFT JOIN [dbo].[ITGNSQLDF_TDI_OID] TDI
            ON TDI.br_ssn = loan.BR_SSN
            AND TDI.loan_num = loan.ln_num
        LEFT JOIN [dbo].[CompassLoanMapping] MAP
            ON MAP.br_ssn = LOAN.br_ssn
            AND MAP.NelNetLoanSeq = LOAN.ln_num
        LEFT JOIN ITLSSQLDF_IBR IBR
            ON IBR.br_ssn = loan.br_ssn
            AND IBR.ln_num = loan.ln_num
		LEFT JOIN ITEJSQLDF_Borrower bor
			ON loan.br_ssn = bor.br_ssn
        LEFT JOIN
            (
                SELECT DISTINCT
                    br_ssn,
                    gr_id,
                    MAX(DBO.CONVERT_DATE(gr_last_pmt_date)) AS LAST_PMT_DTE
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
				SELECT
					disc.br_ssn,
					grp.gr_id,
					dbo.CONVERT_DATE(grp.gr_due_date) AS [LD_NPD_PCV],
					dbo.CONVERT_DATE(grp.gr_last_pmt_date) AS [LD_BRW_LST_PMT_PCV],
					dbo.CONVERT_DATE(grp.gr_conversion_date) [LD_END_GRC_PRD]
				FROM
					ITLQSQLDF_Disclosure disc
					LEFT JOIN ITEKSQLDF_Group grp
						on disc.group_id = grp.gr_id
						AND disc.br_ssn = grp.br_ssn
				GROUP BY
					disc.br_ssn,
					grp.gr_id,
					grp.gr_due_date,
					grp.gr_last_pmt_date,
					grp.gr_conversion_date
						
			) disc ON bor.br_ssn = disc.br_ssn
				AND loan.gr_id = disc.gr_id
	WHERE
		loan.br_ssn = @SSN
RETURN 0