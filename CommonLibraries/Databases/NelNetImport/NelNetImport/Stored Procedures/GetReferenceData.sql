CREATE PROCEDURE [dbo].[GetReferenceData]
    @SSN varchar(9)
AS
    SELECT 
        'Y' AS PDEM_DAT_ENT_IND,
        '' AS DC_LAG_FGN, 
        '' AS DC_SEX, 
        '' AS DC_ST_DRV_LIC, 
        '' AS DD_BRT, 
        '' AS DD_DRV_LIC_REN, 
        '' AS DD_NME_VER_LST, 
        '' AS DF_ALN_RGS, 
        '' AS DF_DRV_LIC, 
        [rf_ssn] AS DF_PRS_ID,
        '' AS DI_US_CTZ, 
        [rf_name_first] AS DM_PRS_1,
        [rf_name_last] AS DM_PRS_LST,
        '' AS DM_PRS_LST_SFX, 
        [rf_mi] AS DM_PRS_MID,
        'H' AS DC_ADR, 
        '01' AS DC_SRC_ADR,
        [st_state_code] AS DC_DOM_ST,
        GETDATE() AS DD_STA_PDEM30,
        GETDATE() AS DD_VER_ADR,
        [rf_zip_code] AS DF_ZIP_CDE,
        CASE
            WHEN [rf_address_status] = 'B' THEN 'N'
            ELSE 'Y'
        END AS DI_VLD_ADR,
        [rf_city] AS DM_CT,
        '' AS DM_FGN_CNY,
        '' AS DC_FGN_CNY, 
        '' AS DM_FGN_ST, 
        [rf_address_1] AS DX_STR_ADR_1,
        [rf_address_2] AS DX_STR_ADR_2,
        '' AS DX_STR_ADR_3,
        '' AS DD_DSB_ADR_BEG,
        '' AS DD_DSB_ADR_END,
        'H' AS DC_PHN, 
        GETDATE() AS DD_PHN_VER,
        CASE
            WHEN [rf_home_phone_status] = 'B' THEN 'N'
            ELSE 'Y'
        END AS DI_PHN_VLD,
        '' AS DI_PHN_WTS,
        [rf_home_area_code] AS DN_DOM_PHN_ARA,
        SUBSTRING([rf_home_phone_number],5,3) AS DN_DOM_PHN_XCH,
        SUBSTRING([rf_home_phone_number],8,4) AS DN_DOM_PHN_LCL,
        '' AS DN_FGN_PHN_CNY, 
        '' AS DN_FGN_PHN_CT, 
        '' AS DN_FGN_PHN_INL, 
        '' AS DN_FGN_PHN_LCL, 
        '' AS DN_PHN_XTN, 
        '' AS DT_PHN_BST_CL, 
        '' AS DX_PHN_TME_ZNE, 
        '' AS DC_ALW_ADL_PHN,
        'H' AS DC_ADR_EML,
        GETDATE() AS DD_VER_ADR_EML,
        '31' AS DC_SRC_ADR_EML, 
        'Y' AS DI_VLD_ADR_EML,
        ISNULL(EM.email_address,'') AS DX_ADR_EML_TXT,
        '' AS DC_DL_COR_TYP,
        CASE
            WHEN [rf_relationship] = 'F' THEN '11'
            WHEN [rf_relationship] = 'P' THEN '02'
            WHEN [rf_relationship] = 'R' THEN '03'
            WHEN [rf_relationship] = 'U' THEN '01'
            ELSE '!' + [rf_relationship]
        END AS BC_RFR_REL_BR,
        'P' AS BC_RFR_TYP,
        GETDATE() AS BD_EFF_RFR,
        ISNULL(rf_ssn, '') AS BF_RFR, 
        '' AS BI_ATH_3_PTY, 
        '' AS BN_SEQ_RFR, --THIS WILL BE GENERATED IN CODE
        'A' AS BC_STA_REFR10 

    FROM
        [dbo].[ITEOSQLDF_Reference] REF
        LEFT JOIN 
    (
        SELECT 
            br_ssn,
            email_address
        FROM
           [dbo].[ITLOSQLDF_Email] 
        WHERE
            email_indicator = 'R'
    ) EM
        ON EM.br_ssn = REF.rf_ssn
    WHERE
        REF.br_ssn = @SSN

RETURN 0
