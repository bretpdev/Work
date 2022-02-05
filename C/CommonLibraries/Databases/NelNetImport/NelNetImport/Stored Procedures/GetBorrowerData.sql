CREATE PROCEDURE [dbo].[GetBorrowerData]
    @SSN varchar(9)

AS
    SELECT 
        'Y' AS PDEM_DAT_ENT_IND,
        '' AS DC_LAG_FGN,
        '' AS DC_SEX,
        '' AS DC_ST_DRV_LIC,
        dbo.CONVERT_DATE([br_date_of_birth]) AS DD_BRT,
        '' AS DD_DRV_LIC_REN,
        GETDATE() AS DD_NME_VER_LST,
        ISNULL(br_alien_id_number, '') AS DF_ALN_RGS,
        br_drivers_license AS DF_DRV_LIC,
        BWR.[br_ssn] AS DF_PRS_ID,
        CASE
            WHEN br_citizenship = 'U' THEN 'U'
            ELSE 'N'
        END AS DI_US_CTZ,
        [br_name_first] AS DM_PRS_1, --TODO MAY HAVE TO TRUNCATE
        [br_name_last] AS DM_PRS_LST, --TODO MAY HAVE TO TRUNCATE
        '' AS DM_PRS_LST_SFX,
        [br_name_mi] AS DM_PRS_MID,
        'L' AS DC_ADR,
        '01' AS DC_SRC_ADR,
        [st_state_code] AS DC_DOM_ST,
        GETDATE() AS DD_STA_PDEM30,
        GETDATE() AS DD_VER_ADR,
        [br_perm_zip_code] AS DF_ZIP_CDE, --TODO DO WE NEED TO REMOVE HYPHEN?
        case
            when [br_address_status] = '' then 'Y'
            else 'N'
        end AS DI_VLD_ADR,
        [br_perm_city] AS DM_CT,
        '' AS DM_FGN_CNY,
        '' AS DC_FGN_CNY, 
        '' AS DM_FGN_ST,
        [br_perm_address_1] AS DX_STR_ADR_1,
        [br_perm_address_2] AS DX_STR_ADR_2,
        '' AS DX_STR_ADR_3,
        '' AS DD_DSB_ADR_BEG,
        '' AS DD_DSB_ADR_END,
        'H' AS DC_PHN,
        GETDATE() AS DD_PHN_VER,
        CASE
            WHEN [br_perm_phone_status] = ' ' THEN 'Y'
            ELSE 'N'
        END AS DI_PHN_VLD,
        '' AS DI_PHN_WTS,
        [br_perm_area_code] AS DN_DOM_PHN_ARA,
        SUBSTRING([br_perm_phone_number],5,3) AS DN_DOM_PHN_XCH,
        SUBSTRING([br_perm_phone_number],8,4) AS DN_DOM_PHN_LCL,
        '' AS DN_FGN_PHN_CNY,
        '' AS DN_FGN_PHN_CT, 
        '' AS DN_FGN_PHN_INL,
        '' AS DN_FGN_PHN_LCL,
        [br_perm_phone_ext]AS DN_PHN_XTN, --TODO MAY HAVE TO TRUNCATE
        '' AS DT_PHN_BST_CL,
        '' AS DX_PHN_TME_ZNE,
        'N' AS DC_ALW_ADL_PHN,
        'H' AS DC_ADR_EML,
        GETDATE() AS DD_VER_ADR_EML,
        '31' AS DC_SRC_ADR_EML,
        'Y' AS DI_VLD_ADR_EML,
        ISNULL(EM.email_address, '') AS DX_ADR_EML_TXT,
        '' AS DC_DL_COR_TYP,
        '' AS BI_PD_AHD_VER,
        '' AS BD_FED_LON_SLE, 
        '' AS PF_FED_DEAL_NBR, 
        '' AS PF_OWN_PRV,
        '' AS PM_SER_PRV,
        '' AS PF_FED_SEC_INT_LON,
        'E' AS PC_TYP_CVN 
    FROM
        [dbo].[ITEJSQLDF_Borrower] BWR
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
    WHERE BWR.br_ssn = @SSN

    UNION ALL

        SELECT 
        'Y' AS PDEM_DAT_ENT_IND,
        '' AS DC_LAG_FGN,
        '' AS DC_SEX,
        '' AS DC_ST_DRV_LIC,
        dbo.CONVERT_DATE([br_date_of_birth]) AS DD_BRT,
        '' AS DD_DRV_LIC_REN,
        GETDATE() AS DD_NME_VER_LST,
        ISNULL(br_alien_id_number, '') AS DF_ALN_RGS,
        br_drivers_license AS DF_DRV_LIC,
        BWR.[br_ssn] AS DF_PRS_ID,
        CASE
            WHEN br_citizenship = 'U' THEN 'U'
            ELSE 'N'
        END AS DI_US_CTZ,
        [br_name_first] AS DM_PRS_1, --TODO MAY HAVE TO TRUNCATE
        [br_name_last] AS DM_PRS_LST, --TODO MAY HAVE TO TRUNCATE
        '' AS DM_PRS_LST_SFX,
        [br_name_mi] AS DM_PRS_MID,
        'L' AS DC_ADR,
        '01' AS DC_SRC_ADR,
        [st_state_code] AS DC_DOM_ST,
        GETDATE() AS DD_STA_PDEM30,
        GETDATE() AS DD_VER_ADR,
        [br_perm_zip_code] AS DF_ZIP_CDE, --TODO DO WE NEED TO REMOVE HYPHEN?
        case
            when [br_address_status] = '' then 'Y'
            else 'N'
        end AS DI_VLD_ADR,
        [br_perm_city] AS DM_CT,
        '' AS DM_FGN_CNY,
        '' AS DC_FGN_CNY, 
        '' AS DM_FGN_ST, 
        [br_perm_address_1] AS DX_STR_ADR_1,
        [br_perm_address_2] AS DX_STR_ADR_2,
        '' AS DX_STR_ADR_3,
        '' AS DD_DSB_ADR_BEG,
        '' AS DD_DSB_ADR_END,
        'A' AS DC_PHN,
        GETDATE() AS DD_PHN_VER,
        CASE
            WHEN [br_temp_phone_status] = ' ' THEN 'Y'
            ELSE 'N'
        END AS DI_PHN_VLD,
        '' AS DI_PHN_WTS,
        [br_temp_area_code] AS DN_DOM_PHN_ARA,
        SUBSTRING([br_temp_phone_number],5,3) AS DN_DOM_PHN_XCH,
        SUBSTRING([br_temp_phone_number],8,4) AS DN_DOM_PHN_LCL,
        '' AS DN_FGN_PHN_CNY,
        '' AS DN_FGN_PHN_CT, 
        '' AS DN_FGN_PHN_INL,
        '' AS DN_FGN_PHN_LCL,
        [br_temp_phone_ext] AS DN_PHN_XTN, --TODO MAY HAVE TO TRUNCATE
        '' AS DT_PHN_BST_CL,
        '' AS DX_PHN_TME_ZNE,
        'N' AS DC_ALW_ADL_PHN,
        'H' AS DC_ADR_EML,
        GETDATE() AS DD_VER_ADR_EML,
        '31' AS DC_SRC_ADR_EML, 
        'Y' AS DI_VLD_ADR_EML,
        ISNULL(EM.email_address, '') AS DX_ADR_EML_TXT,
        '' AS DC_DL_COR_TYP,
        '' AS BI_PD_AHD_VER,
        '' AS BD_FED_LON_SLE, 
        '' AS PF_FED_DEAL_NBR, 
        '' AS PF_OWN_PRV,
        '' AS PM_SER_PRV,
        '' AS PF_FED_SEC_INT_LON,
        'E' AS PC_TYP_CVN 
    FROM
        [dbo].[ITEJSQLDF_Borrower] BWR
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
    WHERE BWR.br_ssn = @SSN
    AND [br_temp_area_code] != ' '
    AND [br_temp_phone_number] != '00000000000'

        UNION ALL

        SELECT 
        'Y' AS PDEM_DAT_ENT_IND,
        '' AS DC_LAG_FGN,
        '' AS DC_SEX,
        '' AS DC_ST_DRV_LIC,
        dbo.CONVERT_DATE([br_date_of_birth]) AS DD_BRT,
        '' AS DD_DRV_LIC_REN,
        GETDATE() AS DD_NME_VER_LST,
        ISNULL(br_alien_id_number, '') AS DF_ALN_RGS,
        br_drivers_license AS DF_DRV_LIC,
        BWR.[br_ssn] AS DF_PRS_ID,
        CASE
            WHEN br_citizenship = 'U' THEN 'U'
            ELSE 'N'
        END AS DI_US_CTZ,
        [br_name_first] AS DM_PRS_1, --TODO MAY HAVE TO TRUNCATE
        [br_name_last] AS DM_PRS_LST, --TODO MAY HAVE TO TRUNCATE
        '' AS DM_PRS_LST_SFX,
        [br_name_mi] AS DM_PRS_MID,
        'L' AS DC_ADR,
        '01' AS DC_SRC_ADR,
        [st_state_code] AS DC_DOM_ST,
        GETDATE() AS DD_STA_PDEM30,
        GETDATE() AS DD_VER_ADR,
        [br_perm_zip_code] AS DF_ZIP_CDE, --TODO DO WE NEED TO REMOVE HYPHEN?
        case
            when [br_address_status] = '' then 'Y'
            else 'N'
        end AS DI_VLD_ADR,
        [br_perm_city] AS DM_CT,
        '' AS DM_FGN_CNY,
        '' AS DC_FGN_CNY, 
        '' AS DM_FGN_ST, 
        [br_perm_address_1] AS DX_STR_ADR_1,
        [br_perm_address_2] AS DX_STR_ADR_2,
        '' AS DX_STR_ADR_3,
        '' AS DD_DSB_ADR_BEG,
        '' AS DD_DSB_ADR_END,
        'W' AS DC_PHN,
        GETDATE() AS DD_PHN_VER,
        CASE
            WHEN [br_ther_phone_status] = ' ' THEN 'Y'
            ELSE 'N'
        END AS DI_PHN_VLD,
        '' AS DI_PHN_WTS,
        [br_other_phone_area_code] AS DN_DOM_PHN_ARA,
        SUBSTRING([br_other_phone_number],5,3) AS DN_DOM_PHN_XCH,
        SUBSTRING([br_other_phone_number],8,4) AS DN_DOM_PHN_LCL,
        '' AS DN_FGN_PHN_CNY,
        '' AS DN_FGN_PHN_CT, 
        '' AS DN_FGN_PHN_INL,
        '' AS DN_FGN_PHN_LCL,
        '' AS DN_PHN_XTN, --NO VALUE FOR OTHER PHONE FIELDS IN DB
        '' AS DT_PHN_BST_CL,
        '' AS DX_PHN_TME_ZNE,
        'N' AS DC_ALW_ADL_PHN,
        'H' AS DC_ADR_EML,
        GETDATE() AS DD_VER_ADR_EML,
        '31' AS DC_SRC_ADR_EML, 
        'Y' AS DI_VLD_ADR_EML,
        ISNULL(EM.email_address, '') AS DX_ADR_EML_TXT,
        '' AS DC_DL_COR_TYP,
        '' AS BI_PD_AHD_VER,
        '' AS BD_FED_LON_SLE, 
        '' AS PF_FED_DEAL_NBR, 
        '' AS PF_OWN_PRV,
        '' AS PM_SER_PRV,
        '' AS PF_FED_SEC_INT_LON,
        'E' AS PC_TYP_CVN --TODO
    FROM
        [dbo].[ITEJSQLDF_Borrower] BWR
            LEFT JOIN 
            (
                SELECT 
                    br_ssn,
                    email_address
                FROM
                   [dbo].[ITLOSQLDF_Email] 
                WHERE
                    email_indicator = 'B'
            ) EM ON EM.br_ssn = BWR.br_ssn
    WHERE BWR.br_ssn = @SSN
        AND [br_other_phone_area_code] != ' '
        AND [br_other_phone_number] != '00000000000'

RETURN 0
