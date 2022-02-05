USE [CDW]
GO

DECLARE @ScriptId VARCHAR(10) = 'GRAEMPCFED'
DECLARE @Letter VARCHAR(10) = 'GRADLETFED'
DECLARE @LetterId INT = (SELECT TOP 1 LetterId FROM CLS.[print].Letters WHERE Letter = @Letter)
DECLARE @ScriptDataId INT = (SELECT TOP 1 ScriptDataId FROM CLS.[print].ScriptData WHERE ScriptId = @ScriptId AND LetterId = @LetterId)
INSERT INTO [CLS].[print].[PrintProcessing]
           ([AccountNumber]
           ,[EmailAddress]
           ,[ScriptDataId]
           ,[SourceFile]
           ,[LetterData]
           ,[CostCenter]
           ,[DoNotProcessEcorr]
           ,[OnEcorr]
           ,[ArcAddProcessingId]
           ,[ArcNeeded]
           ,[ImagedAt]
           ,[ImagingNeeded]
           ,[EcorrDocumentCreatedAt]
           ,[PrintedAt]
           ,[AddedBy]
           ,[AddedAt]
           ,[DeletedAt]
           ,[DeletedBy])
SELECT
	POP.AccountNumber,
	POP.EmailAddress,
	POP.ScriptDataId,
	POP.SourceFile,
	POP.LetterData,
	POP.CostCenter,
	POP.DoNotProcessEcorr,
	POP.OnEcorr,
	POP.ArcAddProcessingId,
	CASE WHEN ASD.ScriptDataId IS NOT NULL THEN 1 ELSE 0 END AS ArcNeeded,
	POP.ImagedAt,
	CASE WHEN PP.DocIdId IS NOT NULL THEN 1 ELSE 0 END AS ImagingNeeded,
	POP.EcorrDocumentCreatedAt,
	POP.PrintedAt,
	POP.AddedBy,
	POP.AddedAt,
	POP.DeletedAt,
	POP.DeletedBy
FROM
(
	SELECT
		PD10.DF_SPE_ACC_ID AS AccountNumber,
		COALESCE(PH05.DX_CNC_EML_ADR, 'Ecorr@MyCornerStoneLoan.org') AS EmailAddress,
		@ScriptDataId AS ScriptDataId, --ScriptDataId For Letter
		NULL AS SourceFile, --Source File Name
		--LetterData, (Keyline,Name,Address1,Address2,City,State,Zip,ForeignState,Country,StaticCurrentDate,AccountNumber,StatusDate,ExitDate,School)
			RTRIM(CentralData.dbo.CreateACSKeyline(PD10.DF_PRS_ID,'B','L')) + ',' + --Keyline
			RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_LST) + ',' + --Name
			'"' + RTRIM(PD30.DX_STR_ADR_1) + '"' + ',' + --Address1
			'"' + RTRIM(PD30.DX_STR_ADR_2) + '"' + ',' + --Address2
			'"' + RTRIM(PD30.DM_CT) + '"' + ',' + --City
			RTRIM(PD30.DC_DOM_ST) + ',' + --State
			RTRIM(PD30.DF_ZIP_CDE) + ',' + --Zip
			'"' + RTRIM(PD30.DM_FGN_ST) + '"' + ',' + --ForeignState
			RTRIM(PD30.DM_FGN_CNY) + ',' + --Country
			RTRIM(CONVERT(VARCHAR,GETDATE(),101)) + ',' + --StaticCurrentDate
			RTRIM(PD10.DF_SPE_ACC_ID) + ',' + --AccountNumber
			RTRIM(CONVERT(VARCHAR,SD10.LD_STA_STU10,101)) + ',' + --StatusDate
			RTRIM(CONVERT(VARCHAR,SD10.LD_SCL_SPR,101)) + ',' + --ExitDate
			'"' + RTRIM(SC10.IM_SCL_FUL) + '"' AS LetterData, --School
		'MA4481' AS CostCenter, --Cost Ceneter
		0 AS DoNotProcessEcorr,
		CASE WHEN PH05.DF_SPE_ID IS NOT NULL THEN 1 ELSE 0 END AS OnEcorr, --OnEcorr
		NULL AS ArcAddProcessingId, --ArcAddProcessingId
		1 AS ArcNeeded, --ArcNeeded
		NULL AS ImagedAt, --ImagedAt
		0 AS ImagingNeeded, --ImagingNeeded
		NULL AS EcorrDocumentCreatedAt, --EcorrDocumentCreatedAt
		NULL AS PrintedAt, --PrintedAt
		SUSER_NAME() AS AddedBy, --AddedBy
		GETDATE() AS AddedAt, --AddedAt
		NULL AS DeletedAt, --DeletedAt
		NULL AS DeletedBy --DeletedBy
	FROM
		PD10_PRS_NME PD10
		INNER JOIN
		(
			SELECT
				LN10.BF_SSN,
				SUM(LA_CUR_PRI) AS LA_CUR_PRI
			FROM
				LN10_LON LN10
			WHERE 
				LN10.LC_STA_LON10 = 'R'
				AND LN10.IC_LON_PGM NOT IN ('DLPCNS', 'DLPLUS')
				AND LN10.LA_CUR_PRI > 0
			GROUP BY
				LN10.BF_SSN
		) LN10
			ON PD10.DF_PRS_ID = LN10.BF_SSN
		INNER JOIN 
		(
			SELECT
				SD10.LF_STU_SSN,
				MAX(SD10.LD_STA_STU10) AS LD_STA_STU10
			FROM
				SD10_STU_SPR SD10
			WHERE
				SD10.LC_STA_STU10 = 'A'
				AND CONVERT(DATE,LD_STA_STU10) <= CONVERT(DATE,GETDATE())
			GROUP BY
				SD10.LF_STU_SSN
		) SD10_Max
			ON LN10.BF_SSN = SD10_Max.LF_STU_SSN
		INNER JOIN SD10_STU_SPR SD10
			ON SD10_Max.LF_STU_SSN = SD10.LF_STU_SSN
			AND SD10_Max.LD_STA_STU10 = SD10.LD_STA_STU10
		INNER JOIN SC10_SCH_DMO SC10
			ON SD10.LF_DOE_SCL_ENR_CUR = SC10.IF_DOE_SCL
		LEFT JOIN
		(
			SELECT
				AY10.BF_SSN,
				MAX(AY10.LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
			FROM
				AY10_BR_LON_ATY AY10
			WHERE
				AY10.PF_REQ_ACT = 'CODGD'
				AND LC_STA_ACTY10 = 'A'
			GROUP BY
				AY10.BF_SSN
		) AY10
			ON LN10.BF_SSN = AY10.BF_SSN
		LEFT JOIN 
		(
			SELECT
				PD30_PRIO.DF_PRS_ID, 
				PD30_PRIO.DD_VER_ADR,
				ROW_NUMBER() OVER (PARTITION BY PD30_PRIO.DF_PRS_ID ORDER BY PD30_PRIO.[PRIORITY] ASC, PD30_PRIO.DD_VER_ADR DESC) AS [PRIORITY]
			FROM
			(
				SELECT
					PD30.DF_PRS_ID,
					MAX(PD30.DD_VER_ADR) AS DD_VER_ADR,
					0 AS [PRIORITY]
				FROM
					CDW..PD30_PRS_ADR PD30
				WHERE
					PD30.DC_ADR = 'L'
					AND PD30.DI_VLD_ADR = 'Y'
				GROUP BY
					PD30.DF_PRS_ID
				UNION
				SELECT
					PD30.DF_PRS_ID,
					MAX(PD30.DD_VER_ADR) AS DD_VER_ADR,
					1 AS [PRIORITY]
				FROM
					CDW..PD30_PRS_ADR PD30
				WHERE
					PD30.DC_ADR = 'L'
					AND PD30.DI_VLD_ADR != 'Y' --Take non-valid address if there is no valid address
				GROUP BY
					PD30.DF_PRS_ID
			) PD30_PRIO
		) PD30_Max
			ON PD10.DF_PRS_ID = PD30_Max.DF_PRS_ID
			AND PD30_Max.[PRIORITY] = 1
		LEFT JOIN PD30_PRS_ADR PD30
			ON PD10.DF_PRS_ID = PD30.DF_PRS_ID
			AND PD30_Max.DD_VER_ADR = PD30.DD_VER_ADR
			AND PD30.DC_ADR = 'L'
			--AND PD30.DI_VLD_ADR = 'Y'
		LEFT JOIN PH05_CNC_EML PH05
			ON PD10.DF_SPE_ACC_ID = PH05.DF_SPE_ID
			AND PH05.DI_VLD_CNC_EML_ADR = 'Y'
			AND PH05.DI_CNC_ELT_OPI = 'Y'

	WHERE
		LN10.LA_CUR_PRI > 0.00
		AND 
		(
			AY10.LD_ATY_REQ_RCV < DATEADD(DAY, -365, GETDATE()) 
			OR AY10.BF_SSN IS NULL
		)
		AND SD10.LC_REA_SCL_SPR = '01'
		AND SD10.LC_STA_STU10 = 'A'
		AND
		(
			--If the day is Monday, look for records from the previous FRIDAY, SATURDAY, and SUNDAY
			(DATENAME(dw,GETDATE()) = 'Monday' 
			AND 
			(
				CONVERT(DATE,SD10_Max.LD_STA_STU10) = CONVERT(DATE,DATEADD(DAY, -1, GETDATE())) 
				OR CONVERT(DATE,SD10_Max.LD_STA_STU10) = CONVERT(DATE,DATEADD(DAY, -2, GETDATE()))
				OR CONVERT(DATE,SD10_Max.LD_STA_STU10) = CONVERT(DATE,DATEADD(DAY, -3, GETDATE()))
			))
			OR 
			--Or if it is not Monday, look for records from the previous day
			(DATENAME(dw,GETDATE()) != 'Monday'
			AND CONVERT(DATE,SD10_Max.LD_STA_STU10) = CONVERT(DATE,DATEADD(DAY, -1, GETDATE())))	
		)
		AND 
		(
			(
				PD30.DF_PRS_ID IS NOT NULL
				AND PD30_Max.DF_PRS_ID IS NOT NULL
				AND PD30.DI_VLD_ADR = 'Y'
			)
			OR 
			(
				PH05.DF_SPE_ID IS NOT NULL
			)
		)
) POP
LEFT JOIN
(
	SELECT
		PP.AccountNumber,
		PP.LetterData,
		L.Letter,
		SD.ScriptDataId,
		SD.DocIdId
	FROM
		CLS.[print].PrintProcessing PP
		INNER JOIN CLS.[print].ScriptData SD
			ON PP.ScriptDataId = SD.ScriptDataId
		INNER JOIN CLS.[print].Letters L
			ON L.LetterId = SD.LetterId
	WHERE
		SD.ScriptId = @ScriptId
		AND L.Letter = @Letter
		AND SD.Active = '1'
		AND PP.DeletedAt IS NULL
		AND CAST(PP.AddedAt AS DATE) = CAST(GETDATE() AS DATE)

) PP
	ON PP.AccountNumber = POP.AccountNumber
	AND PP.LetterData = POP.LetterData
LEFT JOIN CLS.[print].ArcScriptDataMapping ASD
	ON ASD.ScriptDataId = PP.ScriptDataId
WHERE
	PP.AccountNumber IS NULL