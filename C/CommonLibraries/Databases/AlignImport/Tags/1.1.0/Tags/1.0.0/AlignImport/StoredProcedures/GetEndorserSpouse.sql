CREATE PROCEDURE [dbo].[GetEndorserSpouse]
    @SSN varchar(9)
AS
    SELECT DISTINCT
        
        'Y' AS PDEM_DAT_ENT_IND,
        '' AS DC_LAG_FGN,
        '' AS DC_SEX,
        '' AS DC_ST_DRV_LIC,
        dbo.CONVERT_DATE(ed.br_date_of_birth) AS DD_BRT,
        '' AS DD_DRV_LIC_REN,
        GETDATE() AS DD_NME_VER_LST,
        '' AS DF_ALN_RGS,
        '' AS DF_DRV_LIC,
        BWR.br_ssn AS DF_PRS_ID,
        '' AS DI_US_CTZ,
        LEFT(ed.[br_name_first], 13) AS DM_PRS_1, 
        LEFT(ed.[br_name_last], 23) AS DM_PRS_LST,
        '' AS DM_PRS_LST_SFX,
         ed.[br_name_mi] AS DM_PRS_MID,
        'L' AS DC_ADR,
        '01' AS DC_SRC_ADR,
        ed.[st_state_code] AS DC_DOM_ST,
        GETDATE() AS DD_STA_PDEM30,
        GETDATE() AS DD_VER_ADR,
        ed.[br_perm_zip_code] AS DF_ZIP_CDE,
        case
            when ed.[br_address_status] = '' then 'Y'
            else 'N'
        end AS DI_VLD_ADR,
        ed.[br_perm_city] AS DM_CT,
        '' AS DM_FGN_CNY,
        '' AS DC_FGN_CNY,
        '' AS DM_FGN_ST,
        ed.[br_perm_address_1] AS DX_STR_ADR_1,
        ed.[br_perm_address_2] AS DX_STR_ADR_2,
        '' AS DX_STR_ADR_3,
        '' AS DD_DSB_ADR_BEG,
        '' AS DD_DSB_ADR_END,
        'H' AS DC_PHN,
        GETDATE() AS DD_PHN_VER,
        CASE
            WHEN ed.[br_perm_phone_status] = '' THEN 'Y'
            ELSE 'N'
        END AS DI_PHN_VLD,
        '' AS DI_PHN_WTS,
        ed.[br_perm_area_code] AS DN_DOM_PHN_ARA,
        SUBSTRING(ed.[br_perm_phone_number],5,3) AS DN_DOM_PHN_XCH,
        SUBSTRING(ed.[br_perm_phone_number],8,4) AS DN_DOM_PHN_LCL,
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
        MAP.CompassLoanProgram AS IC_LON_PGM, 
        map.CommpassLoanSeq AS LN_SEQ,
        'M' AS LC_EDS_TYP,
        '06' AS LC_REL_TO_BR,
        loan.[br_comaker_ssn] AS LF_EDS,
        '' AS AF_APL_ID,
        '0' AS EDS_LON26_DAT_OCC_CNT
    FROM
        [dbo].[ITELSQLDF_Loan] loan
		left join [dbo].[ITEJSQLDF_Borrower] bwr
			on bwr.br_ssn = loan.br_ssn
        left join [dbo].[CompassLoanMapping] map
			on map.br_ssn = loan.br_ssn
			and map.NelNetLoanSeq = loan.ln_num
		left join [dbo].[ITEJSQLDF_Borrower] ed
			on ed.br_ssn = loan.br_comaker_ssn
		LEFT JOIN 
			(
				SELECT 
					br_ssn,
					email_address
				FROM
				   [dbo].[ITLOSQLDF_Email] 
				WHERE
					email_indicator = 'E'
			) EM ON EM.br_ssn = loan.br_comaker_ssn
    WHERE 
	loan.br_ssn = @SSN
	and loan.br_comaker_ssn != ''

RETURN 0
