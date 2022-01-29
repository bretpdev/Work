﻿CREATE PROCEDURE [dbo].[LT_US13B9COSI_FormFields]
	@BF_SSN VARCHAR(9),
	@IsCoborrower BIT = 0,
	@RN_ATY_SEQ_PRC INT
AS
BEGIN

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @PF_REQ_ACT VARCHAR(5) = (SELECT AC11.PF_REQ_ACT FROM AC11_ACT_REQ_LTR AC11 WHERE PF_LTR = 'US13B9COSI')

IF @IsCoborrower = 0
	BEGIN
		SELECT DISTINCT
			RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_LST) AS BorrName,
			RTRIM(PD30.DX_STR_ADR_1) AS BorrAddress1,
			RTRIM(PD30.DX_STR_ADR_2) AS BorrAddress2,
			RTRIM(PD30.DM_FGN_CNY) as BorrCountry,
			RTRIM(PD30.DM_CT) AS BorrCity,
			CASE WHEN LEN(RTRIM(PD30.DM_FGN_ST)) > 0 
				THEN RTRIM(PD30.DM_FGN_ST)
				ELSE RTRIM(PD30.DC_DOM_ST)
			END AS BorrState,
			CASE WHEN LEN(RTRIM(PD30.DF_ZIP_CDE)) = 9 
				THEN LEFT(PD30.DF_ZIP_CDE, 5) + '-' + RIGHT(RTRIM(PD30.DF_ZIP_CDE), 4)
				ELSE RTRIM(PD30.DF_ZIP_CDE)
			END AS BorrZip,
			PD42.BorrPhone AS BorrPhone
		FROM
			PD10_PRS_NME PD10
			INNER JOIN LN10_LON LN10
				ON LN10.BF_SSN = PD10.DF_PRS_ID
			INNER JOIN PD30_PRS_ADR PD30
				ON PD30.DF_PRS_ID = PD10.DF_PRS_ID	
				AND DC_ADR = 'L'
			INNER JOIN
			(
				SELECT
					LN85.BF_SSN,
					LN85.LN_SEQ
				FROM
					LN85_LON_ATY LN85
					INNER JOIN AY10_BR_LON_ATY AY10
						ON AY10.BF_SSN = LN85.BF_SSN
						AND AY10.LN_ATY_SEQ = LN85.LN_ATY_SEQ
				WHERE   
					AY10.PF_REQ_ACT = @PF_REQ_ACT
					AND AY10.BF_SSN = @BF_SSN
					AND AY10.LN_ATY_SEQ = @RN_ATY_SEQ_PRC
					--Active flag ignored, as LT20 provides the exact record that is tied to this request
			)LN85
				ON LN85.BF_SSN = LN10.BF_SSN
				AND LN85.LN_SEQ = LN10.LN_SEQ 
			LEFT JOIN 
			( 
				SELECT 
					PHN_RNK.DF_PRS_ID, 
					ISNULL(PHN_RNK.BorrPhone, '') AS BorrPhone,  
					ROW_NUMBER() OVER (PARTITION BY PHN_RNK.DF_PRS_ID ORDER BY PHN_RNK.RNK) AS PHN_PRIORITY 
				FROM 
				( 
					SELECT 
						PD42.DF_PRS_ID, 
						CASE PD42.DC_PHN  
							WHEN 'H' THEN 1 
							WHEN 'A' THEN 2 
							WHEN 'W' THEN 3 
							WHEN 'M' THEN 4 
							ELSE 5 
						END AS RNK, 
						COALESCE 
						( 
							CAST(NULLIF 
							( 
								RTRIM(PD42.DN_DOM_PHN_ARA)  +  
								RTRIM(PD42.DN_DOM_PHN_XCH)  +  
								RTRIM(PD42.DN_DOM_PHN_LCL) 
								, '--' 
							) AS VARCHAR(250)),    
							NULLIF 
							( 
								RTRIM(PD42.DN_FGN_PHN_INL)  +  
								RTRIM(PD42.DN_FGN_PHN_CNY)  +  
								RTRIM(PD42.DN_FGN_PHN_CT)   +  
								RTRIM(PD42.DN_FGN_PHN_LCL) 
								, '---' 
							), 
							'NO PHONE NUMBER ON RECORD' 
						) [BorrPhone] 
					FROM 
						PD42_PRS_PHN PD42 
					WHERE 
						PD42.DI_PHN_VLD = 'Y' 
				) PHN_RNK 
			) PD42 
				ON PD42.DF_PRS_ID = PD10.DF_PRS_ID
				AND PD42.PHN_PRIORITY = 1
		WHERE
			PD10.DF_PRS_ID = @BF_SSN
	END
ELSE
	BEGIN
		SELECT DISTINCT
			RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_LST) AS BorrName,
			RTRIM(PD30.DX_STR_ADR_1) AS BorrAddress1,
			RTRIM(PD30.DX_STR_ADR_2) AS BorrAddress2,
			RTRIM(PD30.DM_FGN_CNY) as BorrCountry,
			RTRIM(PD30.DM_CT) AS BorrCity,
			CASE WHEN LEN(RTRIM(PD30.DM_FGN_ST)) > 0 
				THEN RTRIM(PD30.DM_FGN_ST)
				ELSE RTRIM(PD30.DC_DOM_ST)
			END AS BorrState,
			CASE WHEN LEN(RTRIM(PD30.DF_ZIP_CDE)) = 9 
				THEN LEFT(PD30.DF_ZIP_CDE, 5) + '-' + RIGHT(RTRIM(PD30.DF_ZIP_CDE), 4)
				ELSE RTRIM(PD30.DF_ZIP_CDE)
			END AS BorrZip,
			PD42.BorrPhone AS BorrPhone
		FROM
			PD10_PRS_NME PD10
			INNER JOIN LN10_LON LN10
				ON LN10.BF_SSN = PD10.DF_PRS_ID
			INNER JOIN LN20_EDS LN20
				ON LN20.BF_SSN = LN10.BF_SSN
				AND LN20.LN_SEQ = LN10.LN_SEQ
				AND LN20.LC_STA_LON20 = 'A'
				AND LN20.LC_EDS_TYP = 'M'
			INNER JOIN PD30_PRS_ADR PD30
				ON PD30.DF_PRS_ID = PD10.DF_PRS_ID	
				AND DC_ADR = 'L'
			INNER JOIN
			(
				SELECT
					LN85.BF_SSN,
					LN85.LN_SEQ
				FROM
					LN85_LON_ATY LN85
					INNER JOIN AY10_BR_LON_ATY AY10
						ON AY10.BF_SSN = LN85.BF_SSN
						AND AY10.LN_ATY_SEQ = LN85.LN_ATY_SEQ
					INNER JOIN LN20_EDS LN20
						ON LN20.BF_SSN = LN85.BF_SSN
						AND LN20.LN_SEQ = LN85.LN_SEQ
						AND LN20.LC_STA_LON20 = 'A'
						AND LN20.LC_EDS_TYP = 'M'
				WHERE   
					AY10.PF_REQ_ACT = @PF_REQ_ACT
					AND LN20.LF_EDS = @BF_SSN
					AND AY10.LN_ATY_SEQ = @RN_ATY_SEQ_PRC
					--Active flag ignored, as LT20 provides the exact record that is tied to this request
			)LN85
				ON LN85.BF_SSN = LN10.BF_SSN
				AND LN85.LN_SEQ = LN10.LN_SEQ 	
			LEFT JOIN 
			( 
				SELECT 
					PHN_RNK.DF_PRS_ID, 
					ISNULL(PHN_RNK.BorrPhone, '') AS BorrPhone,  
					ROW_NUMBER() OVER (PARTITION BY PHN_RNK.DF_PRS_ID ORDER BY PHN_RNK.RNK) AS PHN_PRIORITY 
				FROM 
				( 
					SELECT 
						PD42.DF_PRS_ID, 
						CASE PD42.DC_PHN  
							WHEN 'H' THEN 1 
							WHEN 'A' THEN 2 
							WHEN 'W' THEN 3 
							WHEN 'M' THEN 4 
							ELSE 5 
						END AS RNK, 
						COALESCE 
						( 
							CAST(NULLIF 
							( 
								RTRIM(PD42.DN_DOM_PHN_ARA)  +  
								RTRIM(PD42.DN_DOM_PHN_XCH)  +  
								RTRIM(PD42.DN_DOM_PHN_LCL) 
								, '--' 
							) AS VARCHAR(250)),    
							NULLIF 
							( 
								RTRIM(PD42.DN_FGN_PHN_INL)  +  
								RTRIM(PD42.DN_FGN_PHN_CNY)  +  
								RTRIM(PD42.DN_FGN_PHN_CT)   +  
								RTRIM(PD42.DN_FGN_PHN_LCL) 
								, '---' 
							), 
							'NO PHONE NUMBER ON RECORD' 
						) [BorrPhone] 
					FROM 
						PD42_PRS_PHN PD42 
					WHERE 
						PD42.DI_PHN_VLD = 'Y' 
				) PHN_RNK 
			) PD42 
				ON PD42.DF_PRS_ID = PD10.DF_PRS_ID
				AND PD42.PHN_PRIORITY = 1
		WHERE
			LN20.LF_EDS = @BF_SSN
	END
END