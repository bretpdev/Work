CREATE PROCEDURE [dbo].[LT_TS13SKPSCH_FormFields]
	@AccountNumber VARCHAR(10),
	@IsCoborrower BIT = 0
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	SELECT
		--borrower info
		REPLACE(LTRIM(RTRIM(PD10.DM_PRS_1)),',','')  + ' ' + REPLACE(LTRIM(RTRIM(PD10.DM_PRS_LST)),',','') + ' ' + REPLACE(LTRIM(RTRIM(PD10.DM_PRS_LST_SFX)),',','')  AS BorrowerName,
		--address info
		PD10.DF_PRS_ID AS BorrowerSocial,
		REPLACE(LTRIM(RTRIM(PD30.DX_STR_ADR_1)),',','')  AS BorrowerAddress1,
		REPLACE(LTRIM(RTRIM(PD30.DX_STR_ADR_2)),',','')  AS BorrowerAddress2,
		REPLACE(LTRIM(RTRIM(PD30.DM_FGN_ST)),',','')  AS BorrowerForeignState,
		REPLACE(LTRIM(RTRIM(PD30.DM_FGN_CNY)),',','') AS BorrowerCountry,
		REPLACE(LTRIM(RTRIM(PD30.DM_CT)),',','')  AS BorrowerCity,
		REPLACE(LTRIM(RTRIM(PD30.DC_DOM_ST)),',','')  AS BorrowerState,
		CASE WHEN LEN(PD30.DF_ZIP_CDE) = 9 
			THEN LEFT(LTRIM(RTRIM(PD30.DF_ZIP_CDE)), 5) + '-' + RIGHT(LTRIM(RTRIM(PD30.DF_ZIP_CDE)), 4)
			ELSE LTRIM(RTRIM(PD30.DF_ZIP_CDE))
		END AS BorrowerZIP,
		PD40H.DN_DOM_PHN_ARA + PD40H.DN_DOM_PHN_XCH + PD40H.DN_DOM_PHN_LCL AS BorrowerPrimaryPhone,
		PD40A.DN_DOM_PHN_ARA + PD40A.DN_DOM_PHN_XCH + PD40A.DN_DOM_PHN_LCL AS BorrowerAltPhone,
		PD32.DX_ADR_EML AS BorrowerPrimaryEmail,
		SC10.IM_SCL_FUL AS SchoolName,
		SC10.IF_DOE_SCL AS SchoolCode
	FROM
		PD10_PRS_NME PD10 
		INNER JOIN PD30_PRS_ADR PD30 
			ON PD10.DF_PRS_ID = PD30.DF_PRS_ID
			AND PD30.DC_ADR = 'L'
		INNER JOIN LT20_LTR_REQ_PRC LT20
			ON LT20.DF_SPE_ACC_ID = PD10.DF_SPE_ACC_ID
			AND
			(
				(
					LT20.PrintedAt IS NULL 
					AND LT20.OnEcorr = 0
				) 
				OR LT20.EcorrDocumentCreatedAt IS NULL
			)
			AND LT20.InactivatedAt IS NULL
			AND LT20.RI_LTR_REQ_DEL_PRC = 'N'
		INNER JOIN AY10_BR_LON_ATY AY10
			ON LT20.RF_SBJ_PRC = AY10.BF_SSN
			AND LT20.RN_ATY_SEQ_PRC = AY10.LN_ATY_SEQ
		INNER JOIN SC10_SCH_DMO SC10
			ON SC10.IF_DOE_SCL = AY10.LF_ATY_RCP
		LEFT JOIN PD40_PRS_PHN PD40H
			ON PD40H.DF_PRS_ID = PD10.DF_PRS_ID
			AND PD40H.DC_PHN = 'H'
		LEFT JOIN PD40_PRS_PHN PD40A
			ON PD40H.DF_PRS_ID = PD10.DF_PRS_ID
			AND PD40H.DC_PHN = 'A'
		LEFT JOIN 
		( 
		SELECT 
			EMAIL.*, 
			ROW_NUMBER() OVER (PARTITION BY Email.DF_PRS_ID ORDER BY Email.PriorityNumber) AS EmailPriority -- number in order of Email.PriorityNumber 
			FROM 
			( 
				SELECT 
					PD32.DF_PRS_ID, 
					PD32.DX_ADR_EML, 
					CASE  
						WHEN DC_ADR_EML = 'H' THEN 1 -- home 
						WHEN DC_ADR_EML = 'A' THEN 2 -- alternate 
						WHEN DC_ADR_EML = 'W' THEN 3 -- work 
					END AS PriorityNumber 
				FROM 
					PD32_PRS_ADR_EML PD32 
					INNER JOIN LN10_LON LN10 
						ON LN10.BF_SSN = PD32.DF_PRS_ID 
				WHERE 
					PD32.DI_VLD_ADR_EML = 'Y' -- valid email address 
					AND PD32.DC_STA_PD32 = 'A' -- active email address record 
			) Email 
		) PD32  
			ON PD32.DF_PRS_ID = PD10.DF_PRS_ID 
			AND PD32.EmailPriority = 1 --highest priority email only 
	WHERE 
		SC10.IF_DOE_SCL = @AccountNumber