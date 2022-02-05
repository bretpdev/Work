CREATE PROCEDURE [dbo].[GetDisbData]
    @SSN varchar(9)
AS
    SELECT DISTINCT 
        disbursement_id as DisbDataId,
        'Y' AS LON15_DAT_ENT_IND,
        LNMAP.CompassLoanProgram AS IC_LON_PGM, 
        LNMAP.CommpassLoanSeq AS LN_SEQ, 
        '' AS AF_DSB_RPT,
        [db_prin_amount] AS LA_DSB,
        CASE
			WHEN dbo.CONVERT_DATE(db_cancel_date) IS NOT NULL AND dbo.CONVERT_DATE(db_cancel_date) > dbo.CONVERT_DATE(db_date) AND [db_prin_amount] = '' AND da_reason_code != '' and ln.ln_school_ref_wo_cancel != '0000000' THEN ln.ln_school_ref_wo_cancel
			WHEN dbo.CONVERT_DATE(db_cancel_date) IS NOT NULL AND dbo.CONVERT_DATE(db_cancel_date) > dbo.CONVERT_DATE(db_date) THEN [db_prin_amount]
            ELSE '' 
        END AS LA_DSB_CAN,
        CASE
            WHEN [db_cancel_date] IS NOT NULL AND [da_reason_code] IN ('SR','33') THEN 'Q'
            WHEN [db_cancel_date] IS NOT NULL AND [da_reason_code] IN ('30','34') THEN 'O'
            WHEN [db_cancel_date] IS NOT NULL THEN 'W'
            ELSE
                ' ' 
        END AS LC_DSB_CAN_TYP,
        '2' AS LC_DSB_TYP,
        CASE
            WHEN [db_eft_indicator] = 'Y' THEN '3'
            ELSE 'M'
        END AS LC_LDR_DSB_MDM,
        '' AS LC_RCP_CHK_DSB,
        '' AS LD_CAN_RPT_GTR, 
        dbo.CONVERT_DATE([db_date]) AS LD_DSB,
		CASE
			WHEN dbo.CONVERT_DATE(db_cancel_date) IS NOT NULL AND dbo.CONVERT_DATE(db_cancel_date) > dbo.CONVERT_DATE(db_date) THEN dbo.CONVERT_DATE(db_cancel_date)
            ELSE NULL 
        END AS LD_DSB_CAN,
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
        '' AS LA_PCV_LDR_CHK, 
        CASE
			WHEN dbo.CONVERT_DATE(db_cancel_date) IS NOT NULL AND dbo.CONVERT_DATE(db_cancel_date) > dbo.CONVERT_DATE(db_date) THEN db_prin_amount
            ELSE '' 
        END AS LA_PRE_DSB_CAN, 
        CASE
			WHEN dbo.CONVERT_DATE(db_cancel_date) IS NOT NULL AND dbo.CONVERT_DATE(db_cancel_date) > dbo.CONVERT_DATE(db_date) THEN dbo.CONVERT_DATE(db_cancel_date)
            ELSE NULL
        END AS LD_PRE_DSB_CAN, 
        CASE
            WHEN [db_cancel_date] IS NOT NULL AND [da_reason_code] IN ('SR','33') THEN '0165'
            WHEN [db_cancel_date] IS NOT NULL AND [da_reason_code] IN ('30','34') THEN '0153'
            WHEN [db_cancel_date] IS NOT NULL THEN '0154'
            ELSE
                '' 
        END AS LC_DSB_CAN_REA, 
        '' AS AF_APL_ID, 
        '' AS AN_LC_APL_SEQ, 
        '' AS LA_DSB_CAN_PCV_RFD, 
        '' AS LA_DSB_CAN_RFD,
        '' AS LD_DSB_CAN_RFD, 
        '' AS LA_PRE_DSB_CAN_RPT, 
        '' AS LD_PRE_DSB_CAN_RPT, 
        '' AS LA_DL_DSB_REB, 
        '' AS LA_DSB_REB_CAN, 
        '' AS LN_DL_DSB_PAY_SEQ, 
        '' AS LI_DSB_CMNLN_ERR, 
        '' AS LN_LON18_DAT_OCC_CNT --THIS WILL BE POPULATED IN CODE.

    FROM
        [ITEMSQLDF_Disbursement] DSB
    INNER JOIN [dbo].[ITELSQLDF_Loan_NoZeroBalances] LN
        ON LN.br_ssn = DSB.br_ssn
        AND LN.ln_num = DSB.ln_num
    INNER JOIN [dbo].[CompassLoanMapping] LNMAP
        ON LNMAP.br_ssn = DSB.br_ssn
        and LNMAP.NelNetLoanSeq = DSB.ln_num
        
    WHERE
        DSB.br_ssn = @SSN
    ORDER BY
        LNMAP.CommpassLoanSeq,
        dbo.CONVERT_DATE(DSB.db_date)

RETURN 0
