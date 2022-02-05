CREATE PROCEDURE [dbo].[GetDisbData]
    @SSN varchar(9)
AS
    SELECT 
        disbursement_id as DisbDataId,
        'Y' AS LON15_DAT_ENT_IND,
        DBO.GET_LOAN_PROGRAM(LN.ln_loan_type, DBO.CONVERT_DATE(DSB.db_date), LN.br_comaker_ssn) AS IC_LON_PGM, --TODO NEED TO CONVERT THIS TO COMPASSS VALUE MAYBE WE WILL DO IT IN CODE....
        LNMAP.CommpassLoanSeq AS LN_SEQ, 
        '' AS AF_DSB_RPT,
        [db_prin_amount] AS LA_DSB,
        CASE
            WHEN [db_cancel_date] != '0000000' THEN [db_prin_amount] --TODO MAKE SURE THIS IS THE CORRECT VALUE
            ELSE '' 
        END AS LA_DSB_CAN,
        CASE
            WHEN [db_cancel_date] IS NOT NULL AND [da_reason_code] IN ('SR','33') THEN 'Q'
            WHEN [db_cancel_date] IS NOT NULL AND [da_reason_code] IN ('30','34') THEN 'O'
            WHEN [db_cancel_date] IS NOT NULL THEN 'W'
            ELSE
                ' ' --TODO MAKE SURE IF NONE OF CONDITION ABOVE THEN SHOULD BE BLANK
        END AS LC_DSB_CAN_TYP,
        '2' AS LC_DSB_TYP,
        CASE
            WHEN [db_eft_indicator] = 'Y' THEN '3'
            ELSE 'M'
        END AS LC_LDR_DSB_MDM,
        '' AS LC_RCP_CHK_DSB,
        '' AS LD_CAN_RPT_GTR, 
        [DBO].CONVERT_DATE([db_date]) AS LD_DSB,
        dbo.CONVERT_DATE([db_cancel_date]) AS LD_DSB_CAN,
        '' AS LD_DSB_ROS_PRT,
        CASE
            WHEN [db_check_number] IN ('0', '00', '000', '000000000000000') THEN ''
            ELSE [db_check_number]
        END AS LF_DSB_CHK,
        '' AS LF_RCP_DSB_CHK,
        '' AS LI_LTE_DSB_APV,
        '' AS LN_BR_DSB_SEQ, --THIS WILL BE POPULATED IN CODE.
        '' AS LR_DSB_ITR, 
        '1' AS LC_STA_LON15,
        '' AS LC_LON_EXT_ORG_SRC, 
        '' AS LN_LON_DSB_SEQ, --THIS WILL BE POPULATED IN CODE.
        '00000000' AS LA_PCV_LDR_CHK, --TODO
        '00000000' AS LA_PRE_DSB_CAN, --TODO
        '' AS LD_PRE_DSB_CAN, --TODO
        CASE
            WHEN [db_cancel_date] IS NOT NULL AND [da_reason_code] IN ('SR','33') THEN '0165'
            WHEN [db_cancel_date] IS NOT NULL AND [da_reason_code] IN ('30','34') THEN '0153'
            WHEN [db_cancel_date] IS NOT NULL THEN '0154'
            ELSE
                ' ' --TODO MAKE SURE IF NONE OF CONDITION ABOVE THEN SHOULD BE BLANK
        END AS LC_DSB_CAN_REA, 
        '' AS AF_APL_ID, 
        '' AS AN_LC_APL_SEQ, 
        '00000000' AS LA_DSB_CAN_PCV_RFD, --TODO
        '00000000' AS LA_DSB_CAN_RFD, --TODO
        '' AS LD_DSB_CAN_RFD, --TODO
        '000000000000' AS LA_PRE_DSB_CAN_RPT, --TODO
        '' AS LD_PRE_DSB_CAN_RPT, --TODO
        '' AS LA_DL_DSB_REB, 
        '' AS LA_DSB_REB_CAN, 
        '' AS LN_DL_DSB_PAY_SEQ, 
        '' AS LI_DSB_CMNLN_ERR, 
        '' AS LN_LON18_DAT_OCC_CNT --THIS WILL BE POPULATED IN CODE.

    FROM
        [ITEMSQLDF_Disbursement] DSB
    LEFT JOIN [dbo].[ITELSQLDF_Loan] LN
        ON LN.br_ssn = DSB.br_ssn
        AND LN.ln_num = DSB.ln_num
    INNER JOIN [dbo].[CompassLoanMapping] LNMAP
        ON LNMAP.br_ssn = DSB.br_ssn
        and LNMAP.NelNetLoanSeq = DSB.ln_num
        
    WHERE
        DSB.br_ssn = @SSN
    ORDER BY
        LNMAP.CommpassLoanSeq,
        DBO.CONVERT_DATE(DSB.DB_DATE)

RETURN 0
