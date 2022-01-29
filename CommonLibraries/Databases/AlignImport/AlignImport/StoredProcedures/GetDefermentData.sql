CREATE PROCEDURE [dbo].[GetDefermentData]
    @SSN varchar(9)

AS
    SELECT DISTINCT
        'Y' AS DFR10_DAT_ENT_IND,
        CASE
            WHEN [f1_dependent_student_ssn] != '' AND [f1_dependent_student_ssn] != DEF.[br_ssn] THEN [f1_dependent_student_ssn]
            ELSE DEF.[br_ssn]
        END AS LF_STU_SSN,
        '' AS LC_DFR_REQ_COR,
        'A' AS LC_DFR_STA,
        MAP.compass_code AS LC_DFR_TYP,
		'' AS LC_ENR_STA,
        dbo.CONVERT_DEFFORB_DATE([f1_start_date]) AS LD_DFR_REQ_BEG,
        dbo.CONVERT_DEFFORB_DATE([f1_end_date]) AS LD_DFR_REQ_END,
        Replace(Str(def.control_num + convert(int, isnull(EB.MAX_DEF_CONTROL_NUM, 0)), 3), ' ' , '0') AS LF_DFR_CTL_NUM, 
        CASE
            WHEN DEF.[f1_type_of_defer_forb] IN ('D10','D11','D12','D13','D15','D40') THEN def.[si_school_code] + def.[si_branch]
            ELSE '' 
        END AS LF_DOE_SCL_DFR,
        'Y' AS LI_CAP_DFR_INT_REQ,
        '' AS LD_DFR_CER, 
        '' AS LC_DFR_SUB_TYP, 
        '' AS LD_BR_REQ_DFR_BEG, 
        '' AS LD_DFR_SPT_DOC_BEG, 
        '' AS LD_DFR_SPT_DOC_END, 
        '' AS LI_DFR_SPT_DOC_ACP, 
        '' AS LC_DFR_DNL_USR_ENT, 
        '' AS LI_DFR_DOC_SPT_REQ, 
        '' AS LI_REQ_PST_DFR_DFR, 
        '' AS LI_REQ_IN_SCL_DFR, 
        '' AS LD_STP_ENR_MIN_HT, 
        '' AS LC_ARA_TSH, 
        '' AS LD_NTF_DFR_END, 
        '' AS LD_DFR_INF_CER, 
        '' AS LA_BR_PAY_CHK_JOB,
        '' AS LC_BR_PAY_CHK_FRQ,
        '' AS LC_BR_EMP_STA, 
        '' AS LN_BR_FAM_SIZ, 
        '' AS LC_FED_POV_GID_ST, 
        '' AS LA_MTH_FED_MIN_WGE, 
        '' AS LA_BR_CLC_POV, 
        '' AS LC_SEL_EHD_DFR_TYP, 
        'Y' AS LON50_DAT_ENT_IND,
        LNMAP.CommpassLoanSeq AS LN_SEQ, 
        '000' AS LC_DFR_RSP,
        '' AS LD_DFR_APL, 
        dbo.CONVERT_DEFFORB_DATE([f1_start_date]) AS LD_DFR_BEG,
        dbo.CONVERT_DEFFORB_DATE([f1_end_date]) AS LD_DFR_END,
        '' AS LD_DFR_GRC_END, 
        '' AS LC_LON_LEV_DFR_CAP, 
        '' AS LI_DLQ_CAP 

    FROM
        [dbo].[ITGGSQLDF_Def_Forb] DEF
    INNER JOIN [dbo].[Deferment_Forbearance_Mapping] MAP
        ON MAP.nelnet_code = DEF.f1_type_of_defer_forb
    INNER JOIN [dbo].[CompassLoanMapping] LNMAP
        ON LNMAP.br_ssn = DEF.br_ssn
        and LNMAP.NelNetLoanSeq = DEF.f1_loan_number
	INNER JOIN [dbo].ITELSQLDF_Loan_NoZeroBalances ZBL on ZBL.br_ssn = LNMAP.br_ssn and ZBL.ln_num = LNMAP.NelNetLoanSeq
	LEFT JOIN [dbo].[ExistingBorrowers] EB on EB.BF_SSN = DEF.br_ssn and EB.CompassLoanSeq = LNMAP.CommpassLoanSeq
    WHERE 
        DEF.br_ssn = @SSN
        AND DEF.[f1_type_of_defer_forb] IN ('D10','D11','D12','D13','D14',
                                            'D16','D19','D20','D22','D24',
                                            'D25','D26','D28','D30','D32',
                                            'D34','D36','D38','D40','D44',
                                            'D46','D50','D55','D56','D48','D47','D45')
        AND ((dbo.CONVERT_DEFFORB_DATE([f1_end_date]) IS NOT NULL) OR (dbo.CONVERT_DEFFORB_DATE([f1_start_date]) IS NOT NULL))
		AND f1_canceled_flag = ''
    ORDER BY
        dbo.CONVERT_DEFFORB_DATE([f1_start_date]),
        LNMAP.CommpassLoanSeq

RETURN 0
