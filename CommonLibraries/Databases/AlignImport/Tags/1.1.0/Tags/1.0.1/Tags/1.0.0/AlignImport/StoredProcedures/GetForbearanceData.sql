CREATE PROCEDURE [dbo].[GetForbearanceData]
    @SSN varchar(9)
AS
    SELECT DISTINCT
        'Y' AS FOR10_DAT_ENT_IND,
        '' AS LC_FOR_REQ_COR, 
        'A' AS LC_FOR_STA,
        '3' AS LC_FOR_SUB_TYP,
        MAP.compass_code AS LC_FOR_TYP,
        dbo.CONVERT_DEFFORB_DATE([f1_start_date]) AS LD_FOR_REQ_BEG,
        dbo.CONVERT_DEFFORB_DATE([f1_end_date]) AS LD_FOR_REQ_END,
        '' AS LF_FOR_CTL_NUM, 
        'Y' AS LI_CAP_FOR_INT_REQ,
        CASE    
            WHEN [f1_type_of_defer_forb] = 'FA9' THEN [f1_low_payment_amount]
            ELSE '' 
        END AS LA_REQ_RDC_PAY,
        '' AS LC_FORNEW_SUB_TYP, 
		'' AS LC_FOR_XCP_DCR_TYP,
        '' AS LD_FOR_INF_CER, 
        '' AS LF_DOE_SCL_FOR, 
        'Y' AS LON60_DAT_ENT_IND, 
        LNMAP.CommpassLoanSeq  AS LN_SEQ, 
        dbo.CONVERT_DEFFORB_DATE([f1_start_date]) AS LD_FOR_BEG,
        dbo.CONVERT_DEFFORB_DATE([f1_end_date]) AS LD_FOR_END,
        '' AS LD_FOR_APL, 
        CASE    
            WHEN [f1_type_of_defer_forb] = 'FA9' THEN [f1_low_payment_amount]
            ELSE '' 
        END AS LA_ACL_RDC_PAY,
        '' AS LC_LON_LEV_FOR_CAP, 
        '' AS LA_BR_PAY_CHK_JOB, 
        '' AS LC_BR_PAY_CHK_FRQ, 
        dbo.CONVERT_DEFFORB_DATE([f1_start_date]) AS LD_FOR_BR_REQ_BEG,
        dbo.CONVERT_DEFFORB_DATE([f1_end_date]) AS LD_FOR_BR_REQ_END,
        '' AS LC_FOR_DNL_USR_ENT, 
        '' AS LI_FOR_SPT_DOC_ACP, 
        '' AS LA_BR_MTH_IRL_ISL, 
        '' AS LA_BR_MTH_EXT_ISL, 
        '' AS LI_BRQ_TMP_DNL_FOR, 
        '' AS LI_BRQ_TMP_FOR_DLQ 
    FROM
        [dbo].[ITGGSQLDF_Def_Forb] FORB
    INNER JOIN [dbo].[Deferment_Forbearance_Mapping] MAP
        ON MAP.nelnet_code = FORB.f1_type_of_defer_forb
    INNER JOIN [dbo].[CompassLoanMapping] LNMAP
        ON LNMAP.br_ssn = FORB.br_ssn
        and LNMAP.NelNetLoanSeq = FORB.f1_loan_number
    WHERE 
        forb.br_ssn = @SSN
        AND [f1_type_of_defer_forb] IN ('FA0','FA4','FA6','FAF','FAD',
                                        'FA0','FBR','FBS','FCL','FDA',
                                        'FDD','FD0','FDP','FDR','FFB',
                                        'FFD','FFH','FFI','FFO','FFV',
                                        'FFW','FGF','FH1','FID','FIR',
                                        'FLC','FLF','FLT','FMH','FMM',
                                        'FNS','FO1','FPC','FRA','FRM',
                                        'FRO','FSD','FTF','FV1','FW1')
        AND ((dbo.CONVERT_DEFFORB_DATE([f1_end_date]) IS NOT NULL) OR (dbo.CONVERT_DEFFORB_DATE([f1_start_date]) IS NOT NULL))
		AND f1_canceled_flag =''
    ORDER BY
        dbo.CONVERT_DEFFORB_DATE([f1_start_date]),
        LNMAP.CommpassLoanSeq
RETURN 0
