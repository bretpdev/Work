﻿CREATE PROCEDURE [lnderlettr].[GetBorrowerData]
	@BF_SSN CHAR(9)
AS
	SELECT TOP 1
		PD10.DF_PRS_ID,
		PD10.DF_SPE_ACC_ID,
		PD10.DF_PRS_LST_4_SSN,
		PD10.DM_PRS_1,
		PD10.DM_PRS_MID,
		CASE WHEN LEN(RTRIM(PD10.DM_PRS_LST_SFX)) = 0 
			THEN PD10.DM_PRS_LST -- _SFX is mandantory; it is not NULL, it is empty.
			ELSE RTRIM(PD10.DM_PRS_LST) + ' ' + PD10.DM_PRS_LST_SFX
		END AS [DM_PRS_LST],
  		--PD10.DD_BRT,
		PD11.DM_PRS_1_HST, 
		PD11.DM_PRS_MID_HST, 
		PD11.DM_PRS_LST_HST,
		PD30.DX_STR_ADR_1,
		PD30.DX_STR_ADR_2,
		ISNULL(PD30.DM_CT, ' ') AS DM_CT,
		ISNULL(PD30.DC_DOM_ST, ' ') AS DC_DOM_ST,
		ISNULL(PD30.DM_FGN_ST, ' ') AS DM_FGN_ST,
		ISNULL(PD30.DM_FGN_CNY, ' ') AS DM_FGN_CNY,
		PD30.DF_ZIP_CDE,
		PD32.DX_ADR_EML,
		CASE WHEN LEN(LTRIM(RTRIM(PD42_H.DN_FGN_PHN_INL))) > 0 
				THEN COALESCE(PD42_H.DN_FGN_PHN_INL, '') + ' ' +  COALESCE(PD42_H.DN_FGN_PHN_CNY, '') + ' ' + COALESCE(PD42_H.DN_FGN_PHN_CT, '') + ' ' +  COALESCE(PD42_H.DN_FGN_PHN_LCL, '') + ' ' + COALESCE(PD42_H.DN_PHN_XTN, '') 
			WHEN LTRIM(RTRIM(PD42_H.DN_DOM_PHN_ARA)) != '' AND LTRIM(RTRIM(PD42_H.DN_DOM_PHN_XCH)) != '' AND LTRIM(RTRIM(PD42_H.DN_DOM_PHN_LCL)) != '' 
				THEN PD42_H.DN_DOM_PHN_ARA + '-' +  PD42_H.DN_DOM_PHN_XCH + '-' +  PD42_H.DN_DOM_PHN_LCL + ' ' + PD42_H.DN_PHN_XTN
			ELSE ''
		END AS HomePhone,
		CASE WHEN LEN(LTRIM(RTRIM(PD42_A.DN_FGN_PHN_INL))) > 0 
				THEN COALESCE(PD42_A.DN_FGN_PHN_INL, '') + ' ' +  COALESCE(PD42_A.DN_FGN_PHN_CNY, '') + ' ' + COALESCE(PD42_A.DN_FGN_PHN_CT, '')  + ' ' +  COALESCE(PD42_A.DN_FGN_PHN_LCL, '') + ' ' + COALESCE(PD42_A.DN_PHN_XTN, '') 
			 WHEN LTRIM(RTRIM( PD42_A.DN_DOM_PHN_ARA)) != '' AND LTRIM(RTRIM(PD42_A.DN_DOM_PHN_XCH)) != '' AND LTRIM(RTRIM(PD42_A.DN_DOM_PHN_LCL)) != '' 
				THEN PD42_A.DN_DOM_PHN_ARA + '-' +  PD42_A.DN_DOM_PHN_XCH + '-' +  PD42_A.DN_DOM_PHN_LCL + ' ' + PD42_A.DN_PHN_XTN
			ELSE ''
		END	AS AlternatePhone,
		CASE WHEN LEN(LTRIM(RTRIM(PD42_W.DN_FGN_PHN_INL))) > 0 
				THEN COALESCE(PD42_W.DN_FGN_PHN_INL, '') + ' ' + COALESCE(PD42_W.DN_FGN_PHN_CNY, '') + ' '  + COALESCE(PD42_W.DN_FGN_PHN_CT, '')  + ' ' +  COALESCE(PD42_W.DN_FGN_PHN_LCL, '') + ' ' +  COALESCE(PD42_W.DN_PHN_XTN, '') 
			 WHEN LTRIM(RTRIM(PD42_W.DN_DOM_PHN_ARA)) != '' AND LTRIM(RTRIM(PD42_W.DN_DOM_PHN_XCH)) != '' AND LTRIM(RTRIM(PD42_W.DN_DOM_PHN_LCL)) != '' 
				THEN PD42_W.DN_DOM_PHN_ARA + '-' +  PD42_W.DN_DOM_PHN_XCH + '-' +  PD42_W.DN_DOM_PHN_LCL + ' ' + PD42_W.DN_PHN_XTN
			ELSE ''
		END AS WorkPhone
	FROM
		UDW..PD10_PRS_NME PD10
		LEFT JOIN UDW..PD30_PRS_ADR PD30 
			ON PD10.DF_PRS_ID = PD30.DF_PRS_ID 
			AND PD30.DC_ADR = 'L'
		LEFT JOIN UDW..PD42_PRS_PHN PD42_H
			ON PD10.DF_PRS_ID = PD42_H.DF_PRS_ID 
			AND PD42_H.DC_PHN = 'H'
		LEFT JOIN UDW..PD42_PRS_PHN PD42_W 
			ON PD10.DF_PRS_ID = PD42_W.DF_PRS_ID 
			AND PD42_W.DC_PHN = 'W'
		LEFT JOIN UDW..PD42_PRS_PHN PD42_A 
			ON PD10.DF_PRS_ID = PD42_A.DF_PRS_ID 
			AND PD42_A.DC_PHN = 'A' 
		LEFT JOIN
		(
			SELECT 
				PD32.DF_PRS_ID,
				PD32.DX_ADR_EML 
			FROM 
				UDW..PD32_PRS_ADR_EML PD32
			WHERE 
				DC_ADR_EML = 'H'
				AND DC_STA_PD32 = 'A'
		) PD32 
			ON PD10.DF_PRS_ID = PD32.DF_PRS_ID
		LEFT JOIN /*Use most recent change*/
		(
			SELECT
				PD11Inner.DN_PRS_SEQ AS DN_PRS_SEQ,
				PD11Inner.DM_PRS_1_HST,
				PD11Inner.DM_PRS_MID_HST,
				CASE WHEN LEN(RTRIM(PD11Inner.DM_PRS_LST_SFX_HST)) = 0 THEN PD11Inner.DM_PRS_LST_HST
				ELSE  PD11Inner.DM_PRS_LST_HST + ' ' + RTRIM(PD11Inner.DM_PRS_LST_SFX_HST)
				END AS [DM_PRS_LST_HST],
				PD11Inner.DF_PRS_ID
			FROM
				UDW..PD11_PRS_NME_HST PD11Inner
				INNER JOIN 
				(
					SELECT 
						MAX(PD11Max.DN_PRS_SEQ) AS DN_PRS_SEQ_MAX,
						PD11Max.DF_PRS_ID AS DF_PRS_ID_MAX
					FROM 
						UDW..PD11_PRS_NME_HST PD11Max
					GROUP BY
						PD11Max.DF_PRS_ID
				) PD11Max
				ON PD11Max.DN_PRS_SEQ_MAX = PD11Inner.DN_PRS_SEQ
				AND PD11Max.DF_PRS_ID_MAX = PD11Inner.DF_PRS_ID					
		) PD11
			ON PD11.DF_PRS_ID = PD10.DF_PRS_ID
	WHERE
		PD10.DF_PRS_ID = @BF_SSN