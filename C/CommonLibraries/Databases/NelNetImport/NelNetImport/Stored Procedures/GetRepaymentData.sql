CREATE PROCEDURE [dbo].[GetRepaymentData]
    @SSN varchar(9)
AS
    SELECT distinct
        loan.FinancialDataId AS RepaymentDataId,
        'Y' AS RPST10_DAT_ENT_IND,
        'M' AS LC_FRQ_PAY,
        '1' AS LC_RPD_DIS,
        'A' AS LC_STA_RPST10,
        '' AS LD_RPS_1_PAY_DU,
        '' AS LD_RTN_RPD_DIS, 
        DBO.CONVERT_DATE([gr_conversion_date]) AS LD_SNT_RPD_DIS,
        GETDATE() AS LD_STA_RPST10,
        'N' AS LI_SIG_RPD_DIS,
        '' AS LN_RPS_SEQ, 
        '' AS LC_RPY_FIX_TRM_AMT, 
        'N' AS LON06_DAT_ENT_IND,
        '' AS LA_CPI_RPD_DIS_AGG, 
        '' AS LA_IC_RPD_DIS_AGG, 
        '' AS LA_RPD_INT_DIS_AGG, 
        '' AS LA_TOT_RPD_DIS_AGG, 
        '' AS LC_STA_LON06, 
        '' AS LR_INT_RPD_DIS_AGG, 
        '' AS LC_TYP_SCH_DIS, 
        '' AS LA_ACR_INT_RPD_AGG, 
        '0' AS LN_LON07_DAT_OCC_CNT, 
        'Y' AS LON65_DAT_ENT_IND, 
        loan.ln_num AS LN_SEQ, 
        '' AS LA_CPI_RPD_DIS, 
        '' AS LA_RPD_INT_DIS, 
        '' AS LA_TOT_RPD_DIS, 
        'A' AS LC_STA_LON65,
        '' AS LR_INT_RPD_DIS,
        '' AS LA_ANT_CAP, 
        CASE
            WHEN DISC.graduated_payment_flag = 'E' THEN 'EL'  
            WHEN DISC.graduated_payment_flag = 'G' THEN 'G'  
            WHEN DISC.graduated_payment_flag = 'X' THEN 'EG'  
            WHEN DISC.graduated_payment_flag = 'P' THEN 'IL'  
            WHEN DISC.graduated_payment_flag = 'F' THEN 'IB'  
            WHEN DISC.graduated_payment_flag = 'I' THEN 'IS'   
            WHEN ISNULL(DISC.graduated_payment_flag, '') = '' THEN 'L'
            ELSE '^'
        END AS LC_TYP_SCH_DIS_1,
        '' AS LA_ACR_INT_RPD, 
        '' AS LD_RPD_MAX_TRM_SR, 
        '' AS LN_RPD_MAX_TRM_REQ, 
        '' AS LN_LON66_DAT_OCC_CNT, 
        '0' AS LN_LON04_DAT_OCC_CNT, 
        CASE
            WHEN GRP.gr_pmt_schedule IN ('F','P') THEN 'Y'
            ELSE ''
        END AS RS05_DAT_ENT_IND,
        DBO.CONVERT_DATE(IBR.ibr_begin_date) AS BD_CRT_RS05, 
        '' AS BN_IBR_SEQ, 
        '' AS BF_SSN_SPO, --BISPOS if provided ON THE IBR TABLE FILE IT SAYS NOT PROVIDED SO WE WILL SEE ON THIS ONE....
        '' AS BC_IBR_INF_SRC_VER, 
        'H' AS BC_IRS_TAX_FIL_STA, 
        IBR.agi AS BA_AGI, 
        IBR.family_size AS BN_MEM_HSE_HLD, 
        '' AS BI_JNT_BR_SPO_RPY, --Y IF SPOUSE SSN IS POPULATED (SPOUSE SSN NOT IN NELNET VERSION)
        'Y' AS RS20_DAT_ENT_IND,
        IBR.pfh_amount AS LA_PFH_PAY, 
        IBR.perm_standard_amount AS LA_PMN_STD_PAY 
    FROM
        [dbo].[ITELSQLDF_Loan] LOAN
        LEFT JOIN [dbo].[ITEKSQLDF_Group] GRP
            ON GRP.br_ssn = LOAN.br_ssn
            AND GRP.gr_id = LOAN.gr_id
        LEFT JOIN [dbo].[ITLQSQLDF_Disclosure] DISC
            ON DISC.br_ssn = GRP.br_ssn
            AND DISC.group_id = GRP.gr_id
        LEFT JOIN[dbo].[CompassLoanMapping] MAP
            ON MAP.br_ssn = LOAN.br_ssn
            AND MAP.NelNetLoanSeq = LOAN.ln_num
        LEFT JOIN [dbo].[ITLSSQLDF_IBR] IBR
            ON IBR.br_ssn = LOAN.br_ssn
            AND IBR.ln_num = LOAN.ln_num
    WHERE 
        GRP.br_ssn = @SSN

RETURN 0
