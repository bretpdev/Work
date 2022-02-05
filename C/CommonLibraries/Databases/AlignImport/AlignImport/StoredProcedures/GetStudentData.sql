CREATE PROCEDURE [dbo].[GetStudentData]
	@SSN varchar(9)
AS
	--TODO for plus loans we need to take the benefiting student ssn and join to the borrower table to get the benefiting student ssn's data
	--when can use a coalesce on that table for each instance where we need to check if it is a plus loan.
	SELECT DISTINCT
		CASE
			WHEN loan.ln_loan_type = 'PLUS' OR loan.ln_loan_type in ('GPSL', 'GRAD') THEN 'Y' ELSE 'N'
		END AS [PDEM_DAT_ENT_IND],
		'' AS [DC_LAG_FGN],
		'' AS [DC_SEX],
		'' AS [DC_ST_DRV_LIC],
		dbo.CONVERT_DATE(BWR.br_date_of_birth) AS [DD_BRT], 
		'' AS [DD_DRV_LIC_REN],
		GETDATE() AS [DD_NME_VER_LST],
		'' AS [DF_ALN_RGS],
		'' AS [DF_DRV_LIC],
		CASE
			WHEN loan.ln_loan_type = 'PLUS' OR loan.ln_loan_type in ('GPSL', 'GRAD')
			THEN loan.[br_benefiting_student_ssn] ELSE loan.br_ssn
		END AS [DF_PRS_ID],
		'' AS [DI_US_CTZ],
		left(BWR.br_name_first, 13) AS [DM_PRS_1], 
		left(BWR.br_name_last, 23) AS [DM_PRS_LST], 
		'' AS [DM_PRS_LST_SFX],
		BWR.br_name_mi AS [DM_PRS_MID],
		'L' AS [DC_ADR],
		'01' AS [DC_SRC_ADR], 
		BWR.st_state_code AS [DC_DOM_ST], 
		GETDATE() AS [DD_STA_PDEM30],
		GETDATE() AS [DD_VER_ADR],
		BWR.br_perm_zip_code AS [DF_ZIP_CDE], 
		CASE
            WHEN BWR.[br_address_status] = '' THEN 'Y'
            ELSE 'N'
        END AS DI_VLD_ADR,
		BWR.br_perm_city AS [DM_CT], 
		'' AS [DM_FGN_CNY],
		'' AS [DC_FGN_CNY],
		'' AS [DM_FGN_ST],
		BWR.br_perm_address_1 AS [DX_STR_ADR_1], 
		BWR.br_perm_address_2 AS [DX_STR_ADR_2], 
		'' AS [DX_STR_ADR_3],
		'' AS [DD_DSB_ADR_BEG],
		'' AS [DD_DSB_ADR_END],
		'H' AS [DC_PHN],
		GETDATE() AS [DD_PHN_VER],
		CASE
            WHEN [br_perm_phone_status] = ' ' THEN 'Y'
            ELSE 'N'
        END AS DI_PHN_VLD,
        '' AS DI_PHN_WTS,
        [br_perm_area_code] AS DN_DOM_PHN_ARA,
        SUBSTRING([br_perm_phone_number],5,3) AS DN_DOM_PHN_XCH,
        SUBSTRING([br_perm_phone_number],8,4) AS DN_DOM_PHN_LCL,
		'' AS [DN_FGN_PHN_CNY],
		'' AS [DN_FGN_PHN_CT],
		'' AS [DN_FGN_PHN_INL],
		'' AS [DN_FGN_PHN_LCL],
		'' AS [DN_PHN_XTN],
		'' AS [DT_PHN_BST_CL],
		'' AS [DX_PHN_TME_ZNE],
		'N' AS [DC_ALW_ADL_PHN],
		'H' AS DC_ADR_EML,
        GETDATE() AS DD_VER_ADR_EML,
        '31' AS DC_SRC_ADR_EML,
        'Y' AS DI_VLD_ADR_EML,
        ISNULL(EM.email_address, '') AS DX_ADR_EML_TXT,
		'' AS [DC_DL_COR_TYP],
		CASE
			WHEN loan.ln_loan_type = 'PLUS' OR loan.ln_loan_type in ('GPSL', 'GRAD') THEN LOAN.[br_benefiting_student_ssn] ELSE LOAN.br_ssn
		END AS [LF_STU_SSN],
		CASE
			WHEN loan.ln_loan_type = 'PLUS' OR loan.ln_loan_type in ('GPSL', 'GRAD') THEN 'Y' ELSE 'N'
		END AS [STU10_DAT_ENT_IND],
		'16' AS [LC_REA_SCL_SPR],
		'BC' AS [LC_SCR_SCL_SPR],
		'A' AS [LC_STA_STU10],
		'' AS [LD_NTF_SCL_SPR], 
		dbo.CONVERT_DATE(loan.ln_grad_sep_date) AS [LD_SCL_SPR],
		GETDATE() AS [LD_STA_STU10],
		'' AS [IF_HSP], 
		loan.si_school_code + loan.si_branch AS [LF_DOE_SCL_ENR_CUR], 
		'1' AS [LN_STU_SPR_SEQ],
		'' AS [LD_SCL_CER_STU_STA], 
		CASE
			WHEN loan.ln_loan_type in ('STAF', 'STAU') then 'Y'
			ELSE 'N'
		END AS [STU20_DAT_ENT_IND],
		CASE BWR.br_enroll_sts
			WHEN ' ' THEN 'D' 
			WHEN 'X' THEN 'L' 
			ELSE BWR.br_enroll_sts
		END [LC_STA_STU_ENR], 
		dbo.CONVERT_DATE(bwr.br_enroll_date) AS [LD_ENR_BEG], 
		dbo.CONVERT_DATE(bwr.br_grad_sep_date) AS [LD_ENR_END], 
		'1' AS [LN_STU_ENR_SEQ],
		'' AS [IF_HSP1], 
		loan.si_school_code + loan.si_branch AS [IF_DOE_SCL], 
		'' AS [LON13_DAT_ENT_IND],
		'' AS [LN_SEQ],
		'' AS [LN_STU_SPR_SEQ1], 
		'' AS [LD_END_GRC_PRD_ALI],
		'' AS [LON03_DAT_ENT_IND],
		'' AS [LN_STU_SPR_SEQ1], 
		'' AS [LN_MTH_GRC_PRD_AGG],
		'' AS [LD_END_GRC_PRD_AGG]
	FROM
		[ITELSQLDF_Loan_NoZeroBalances] loan
    INNER JOIN [dbo].[ITEJSQLDF_Borrower] BWR
        ON BWR.br_ssn = LOAN.br_ssn
    LEFT JOIN 
    (
        SELECT 
            br_ssn,
            email_address
        FROM
           [dbo].[ITLOSQLDF_Email] 
        WHERE
            email_indicator = 'B'
    ) EM
        ON EM.br_ssn = BWR.br_ssn
	WHERE
		loan.br_ssn = @SSN
RETURN 0