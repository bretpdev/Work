--ENRHFTFED
--This query will identify and send a letter to CornerStone Borrowers  that have enrolled in school FT/HT or had a in school deferment applied
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @TODAY DATE = GETDATE()
DECLARE @DAYOFWEEK AS INT = DATEPART(WEEKDAY,@TODAY)


INSERT INTO CLS.[print].PrintProcessing(AccountNumber,EmailAddress,ScriptDataId,SourceFile,LetterData,CostCenter,DoNotProcessEcorr,OnEcorr,ArcAddProcessingId,ArcNeeded,ImagedAt,ImagingNeeded,EcorrDocumentCreatedAt,PrintedAt,AddedBy,AddedAt,DeletedAt,DeletedBy)
	SELECT DISTINCT
		PD10.DF_SPE_ACC_ID AS AccountNumber,
		COALESCE(PH05.DX_CNC_EML_ADR,'ecorr@mycornerstoneloan.org') AS EmailAddress,
		LetterData.ScriptDataId AS ScriptDataId,
		NULL AS SourceFile,
		--LetterData
			CentralData.dbo.CreateACSKeyline(LN10.BF_SSN, 'B', 'L') + ',' + -- LD_KeyLine
			LTRIM(RTRIM(PD10. DM_PRS_1)) + ' ' + LTRIM(RTRIM(PD10. DM_PRS_LST))  + ',' + -- LD_Name
			'"' + COALESCE(LTRIM(RTRIM(PD30.DX_STR_ADR_1)),'')   + '"' + ',' + -- LD_add1
			'"' + COALESCE(LTRIM(RTRIM(PD30.DX_STR_ADR_2)),'')   + '"' + ',' + -- LD_add2
			'"' + COALESCE(LTRIM(RTRIM(PD30.DM_CT)),'')  + '"' + ',' + -- LD_City
			COALESCE(LTRIM(RTRIM(PD30.DC_DOM_ST)),'') + ',' + -- LD_ST
			COALESCE(LTRIM(RTRIM(PD30.DF_ZIP_CDE)),'') + ',' + -- LD_Zip
			'"' + COALESCE(LTRIM(RTRIM(PD30.DM_FGN_ST)),'') + '"' + ',' + -- LD_fgnST
			COALESCE(LTRIM(RTRIM(PD30.DM_FGN_CNY)),'') + ',' + -- LD_fgnCny
			CONVERT(VARCHAR,@TODAY,101) + ',' + -- LD StaticCurrentDate
			PD10.DF_SPE_ACC_ID  + ',' + -- LD_accountNumber
			COALESCE(CONVERT(VARCHAR,SD10.LD_ENR_STA_EFF_CAM,101),'') + ',' + -- LD_NotificationReceived
			'In-School' -- LD_Status
			AS LetterData,
		--end of letter data
		'MA4481' AS CostCenter,
		COALESCE(LetterData.DoNotProcessEcorr,0) AS DoNotProcessEcorr,
		CASE 
			WHEN LetterData.DoNotProcessEcorr = 1 THEN 0
			WHEN PH05.DI_CNC_ELT_OPI = 'Y' AND PH05.DI_VLD_CNC_EML_ADR = 'Y' THEN 1
			ELSE 0 
		END OnEcorr,
		NULL AS ArcAddProcessingID,
		CASE 
			WHEN LetterData.ArcId IS NULL 
			THEN 0 
			ELSE 1 
		END AS ArcNeeded,
		NULL AS ImagedAt,
		CASE 
			WHEN LetterData.DocIdId IS NULL 
			THEN 0 
			ELSE 1 
		END AS ImagingNeeded,
		NULL AS EcorrDocumentCreatedAt,
		NULL AS PrintedAt,
		SYSTEM_USER AS AddedBy,
		@TODAY AS AddedAt,
		NULL AS DeletedAt,
		NULL AS DeletedBy
	FROM
		CDW..LN10_LON LN10 --released loans with a balance
		INNER JOIN CDW..DW01_DW_CLC_CLU DW01
			ON LN10.BF_SSN = DW01.BF_SSN
			AND LN10.LN_SEQ = DW01.LN_SEQ
			AND LN10.LC_STA_LON10 = 'R'
			AND LN10.LA_CUR_PRI > 0.00
		INNER JOIN CDW..PD10_PRS_NME PD10
			ON LN10.BF_SSN = PD10.DF_PRS_ID
		LEFT JOIN CDW..AY10_BR_LON_ATY AY10
			ON LN10.BF_SSN = AY10.BF_SSN
			AND AY10.PF_REQ_ACT = 'FEDED'
			AND CONVERT(DATE, AY10.LD_ATY_REQ_RCV) >= DATEADD(DAY,-30,@TODAY) --date arc left within past 30 days
		LEFT JOIN CDW..PD30_PRS_ADR PD30  --valid address
			ON LN10.BF_SSN = PD30.DF_PRS_ID
			AND PD30.DI_VLD_ADR = 'Y'
			AND PD30.DC_ADR = 'L'
		LEFT JOIN CDW..PH05_CNC_EML PH05 --on Ecorr and valid email
			ON PD10.DF_SPE_ACC_ID = PH05.DF_SPE_ID
			AND PH05.DI_VLD_CNC_EML_ADR = 'Y' --Email is Valid
			AND PH05.DI_CNC_ELT_OPI = 'Y' --on Ecorr
		LEFT JOIN --Enrolled Full or Half time 
			(
				SELECT
					LN10.BF_SSN,
					LN10.LN_SEQ,
					SD10MX.MAX_STU10,
					SD10.LD_ENR_STA_EFF_CAM
				FROM
					CDW..LN10_LON LN10
					INNER JOIN CDW..DW01_DW_CLC_CLU DW01
						ON LN10.BF_SSN = DW01.BF_SSN
						AND LN10.LN_SEQ = DW01.LN_SEQ
						AND DW01.WC_DW_LON_STA = '02' --In School
					INNER JOIN
						(
							SELECT
								MAX(SD10.LD_STA_STU10) AS MAX_STU10,
								SD10.LF_STU_SSN
							FROM
								CDW..SD10_STU_SPR SD10
							WHERE
								SD10.LC_STA_STU10 = 'A' --active school record
								AND SD10.LC_REA_SCL_SPR IN ('10','11','19') --halftime, fulltime, 3/4 time
							GROUP BY
								SD10.LF_STU_SSN
						) SD10MX		
							ON LN10.LF_STU_SSN = SD10MX.LF_STU_SSN
					INNER JOIN CDW..SD10_STU_SPR SD10
						ON LN10.LF_STU_SSN = SD10.LF_STU_SSN
						AND SD10.LC_STA_STU10 = 'A'
						AND SD10.LC_REA_SCL_SPR IN ('10','11','19')
						AND SD10.LD_STA_STU10 = SD10MX.MAX_STU10  --join SD10 to SD10 for MAX LD_STA_STU10 to get LD_ENR_STA_EFF_CAM for MAX LD_STA_STU10
				WHERE
					LN10.IC_LON_PGM NOT IN ('DLPCNS','DLPLUS')  --Exclude where all loans are Parent plus loans
					AND
						(
							(
								@DAYOFWEEK = 2 --Monday
								AND DATEDIFF(DAY,@TODAY, CONVERT(DATE, SD10MX.MAX_STU10)) IN (-1,-2,-3) --MAX LD_STA_STU10 = Sunday, Saturday, Friday
							)
							OR
							(
								@DAYOFWEEK != 2 -- not Monday
								AND DATEDIFF(DAY,@TODAY, CONVERT(DATE, SD10MX.MAX_STU10)) = -1 --MAX LD_STA_STU10 = previous day
							)
						)
			) ENRL  --Enrolled Full or Half time 
				ON LN10.BF_SSN = ENRL.BF_SSN
				AND LN10.LN_SEQ = ENRL.LN_SEQ
		LEFT JOIN CDW..SD10_STU_SPR SD10 --join to SD10 to get LD_ENR_STA_EFF_CAM for MAX SD10  record found for ENRL derived table
			ON LN10.LF_STU_SSN = SD10.LF_STU_SSN
			AND SD10.LC_STA_STU10 = 'A' --active school record
			AND SD10.LC_REA_SCL_SPR IN ('10','11','19')
			AND SD10.LD_STA_STU10 = ENRL.MAX_STU10 --join on MAX SD10  record found for ENRL derived table
		LEFT JOIN --get letter ID and other processing data
			(
				SELECT
					SD.ScriptId,
					SD.ScriptDataId, 
					L.LetterId,
					L.Letter,
					SD.DoNotProcessEcorr,
					SD.DocIdId,
					SDM.ArcId
				FROM
					CLS.[print].Letters L
					INNER JOIN CLS.[print].ScriptData SD
						ON SD.ScriptId = 'ENRHFTFED'
						AND SD.LetterId = L.LetterId
					LEFT JOIN CLS.[print].ArcScriptDataMapping SDM
						ON SDM.ScriptDataId = SD.ScriptDataId
				WHERE 
					SD.Active = 1
					AND L.Letter = 'ENRDEFNFED'	
			) LetterData
				ON 1 = 1	
			
		LEFT JOIN CLS.[print].PrintProcessing EXISTING_DATA --flag to exclude existing data added in last 30 days
			ON EXISTING_DATA.AccountNumber = PD10.DF_SPE_ACC_ID
			AND EXISTING_DATA.EmailAddress = COALESCE(PH05.DX_CNC_EML_ADR,'ecorr@mycornerstoneloan.org') 
			AND EXISTING_DATA.ScriptDataId = LetterData.ScriptDataId
			AND DATEDIFF(DAY,CAST(EXISTING_DATA.AddedAt AS DATE),@TODAY) <= 30
			AND EXISTING_DATA.LetterData = 
			CentralData.dbo.CreateACSKeyline(LN10.BF_SSN, 'B', 'L') + ',' + -- LD_KeyLine
			LTRIM(RTRIM(PD10. DM_PRS_1)) + ' ' + LTRIM(RTRIM(PD10. DM_PRS_LST))  + ',' + -- LD_Name
			'"' + COALESCE(LTRIM(RTRIM(PD30.DX_STR_ADR_1)),'')   + '"' + ',' + -- LD_add1
			'"' + COALESCE(LTRIM(RTRIM(PD30.DX_STR_ADR_2)),'')   + '"' + ',' + -- LD_add2
			'"' + COALESCE(LTRIM(RTRIM(PD30.DM_CT)),'')  + '"' + ',' + -- LD_City
			COALESCE(LTRIM(RTRIM(PD30.DC_DOM_ST)),'') + ',' + -- LD_ST
			COALESCE(LTRIM(RTRIM(PD30.DF_ZIP_CDE)),'') + ',' + -- LD_Zip
			'"' + COALESCE(LTRIM(RTRIM(PD30.DM_FGN_ST)),'') + '"' + ',' + -- LD_fgnST
			COALESCE(LTRIM(RTRIM(PD30.DM_FGN_CNY)),'') + ',' + -- LD_fgnCny
			CONVERT(VARCHAR,@TODAY,101) + ',' + -- LD StaticCurrentDate
			PD10.DF_SPE_ACC_ID  + ',' + -- LD_accountNumber
			COALESCE(CONVERT(VARCHAR,SD10.LD_ENR_STA_EFF_CAM,101),'') + ',' + -- LD_NotificationReceived
			'In-School' -- LD_Status
	WHERE 
		(PD30.DF_PRS_ID IS NOT NULL OR PH05.DF_SPE_ID IS NOT NULL) --valid address OR on Ecorr and valid email
		AND ENRL.BF_SSN IS NOT NULL --enrolled full or half time
		AND AY10.BF_SSN IS NULL -- exclude where arc FEDED left within past 30 days
		AND LN10.IC_LON_PGM NOT IN ('DLPCNS','DLPLUS')  --Exclude where all loans are Parent plus loans 
		AND EXISTING_DATA.AccountNumber IS NULL --wasn't already added in last 30 days