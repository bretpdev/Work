CREATE PROCEDURE [dbo].[GetEndorserSpouse]
    @SSN varchar(9)
AS
    SELECT DISTINCT
        ed.EndorserSpouseId,
        'Y' AS PDEM_DAT_ENT_IND,
        '' AS DC_LAG_FGN,
        '' AS DC_SEX,
        '' AS DC_ST_DRV_LIC,
        dbo.CONVERT_DATE(ed.[co_birth_date]) AS DD_BRT,
        '' AS DD_DRV_LIC_REN,
        GETDATE() AS DD_NME_VER_LST,
        '' AS DF_ALN_RGS,
        '' AS DF_DRV_LIC,
        BWR.br_comaker_ssn AS DF_PRS_ID,
        '' AS DI_US_CTZ,
        ed.[co_name_first] AS DM_PRS_1,
        ed.[co_name_last] AS DM_PRS_LST,
        '' AS DM_PRS_LST_SFX,
        ed.[co_mi] AS DM_PRS_MID,
        'L' AS DC_ADR,
        '01' AS DC_SRC_ADR,
        ed.[co_state_code] AS DC_DOM_ST,
        GETDATE() AS DD_STA_PDEM30,
        GETDATE() AS DD_VER_ADR,
        ed.[co_zip_code] AS DF_ZIP_CDE,
        CASE
            WHEN ed.[co_address_status] = 'B' THEN 'N'
            ELSE 'Y'
        END AS DI_VLD_ADR,
        ed.[co_city] AS DM_CT,
        '' AS DM_FGN_CNY,
        '' AS DC_FGN_CNY,
        '' AS DM_FGN_ST,
        ed.[co_address_1] AS DX_STR_ADR_1,
        ed.[co_address_2] AS DX_STR_ADR_2,
        '' AS DX_STR_ADR_3,
        '' AS DD_DSB_ADR_BEG,
        '' AS DD_DSB_ADR_END,
        'H' AS DC_PHN,
        GETDATE() AS DD_PHN_VER,
        CASE
            WHEN ed.[co_home_phone_sts] = 'B' THEN 'N'
            ELSE 'Y'
        END AS DI_PHN_VLD,
        '' AS DI_PHN_WTS,
        ed.[co_home_area_code] AS DN_DOM_PHN_ARA,
        SUBSTRING(ed.[co_home_phone_number],5,3) AS DN_DOM_PHN_XCH,
        SUBSTRING(ed.[co_home_phone_number],8,4) AS DN_DOM_PHN_LCL,
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
        EM.email_address AS DX_ADR_EML_TXT,
        '' AS DC_DL_COR_TYP,
        MAP.CompassLoanProgram AS IC_LON_PGM, --TODO NEED TP MAP
        map.CommpassLoanSeq AS LN_SEQ,
        'M' AS LC_EDS_TYP,
        '06' AS LC_REL_TO_BR,
        ISNULL(LN.[br_comaker_ssn],'') AS LF_EDS,
        '' AS AF_APL_ID,
        '0' AS EDS_LON26_DAT_OCC_CNT
    FROM
        [dbo].[ITGHSQLDF_Endorser] ED
        INNER JOIN 
            (
                SELECT DISTINCT
                    LN.br_comaker_ssn,
                    LN.ln_num
                FROM
                    [dbo].[ITELSQLDF_Loan] LN
                INNER JOIN[dbo].[ITGHSQLDF_Endorser] ED
                    ON ed.co_ssn = ln.br_comaker_ssn
                WHERE 
                    LN.ln_loan_type IN ('CONS', 'CONU')
            ) BWR ON BWR.br_comaker_ssn = ED.co_ssn
        INNER JOIN [dbo].[CompassLoanMapping] MAP
            ON MAP.br_ssn = ed.br_ssn
            and MAP.NelNetLoanSeq = bwr.ln_num
		LEFT JOIN 
			(
				SELECT 
					br_ssn,
					email_address
				FROM
				   [dbo].[ITLOSQLDF_Email] 
				WHERE
					email_indicator = 'E'
			) EM ON EM.br_ssn = ED.co_ssn
		LEFT JOIN [dbo].[ITELSQLDF_Loan] LN
			ON LN.br_ssn = ED.br_ssn
			AND LN.co_number = ED.co_number
    WHERE ED.br_ssn = @SSN

RETURN 0
