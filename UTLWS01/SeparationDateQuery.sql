USE UDW
GO

DECLARE 
	@Today DATE = GETDATE(),
	@ScriptId VARCHAR(10) = 'UTLWS01',
	@CostCenter VARCHAR(10) = 'MA2324',
	@Letter VARCHAR(10) = 'SEPLTR'
DECLARE 
	@LastWeek DATE = DATEADD(DAY, -7, @Today),
	@ValidAddressScriptDataId INT = (SELECT ScriptDataId FROM ULS.[print].ScriptData WHERE ScriptID = @ScriptId AND InvalidAddressFile = 0),
	@InvalidAddressScriptDataId INT = (SELECT ScriptDataId FROM ULS.[print].ScriptData WHERE ScriptID = @ScriptId AND InvalidAddressFile = 1),
	@NumberOfRows INT = 50,
	@ArcNeeded BIT = 0,
	@ImagingNeeded BIT = 0,
	@DoNotProcessEcorr BIT = 0


--Valid address is needed to separate the populations because the letters are tied to different arcs
--that we have been told still need to be set in this way
DECLARE @Population TABLE (
	SSN VARCHAR(9),
	LN_SEQ INT,
	ValidAddress BIT,
	LD_SCL_SPR DATE,
	LF_LST_DTS_SD10 DATE,
	WD_LON_RPD_SR DATE
)

INSERT INTO 
	@Population(SSN,LN_SEQ,ValidAddress,LD_SCL_SPR,LF_LST_DTS_SD10,	WD_LON_RPD_SR)
SELECT DISTINCT
	LN13.BF_SSN,
	LN13.LN_SEQ,
	CASE WHEN PD30.DI_VLD_ADR = 'Y' 
		THEN 1
		ELSE 0
	END AS ValidAddress,
	SD10.LD_SCL_SPR,
	SD10.LF_LST_DTS_SD10,
	DW01.WD_LON_RPD_SR
FROM
	LN13_LON_STU_OSD LN13
	INNER JOIN SD10_STU_SPR SD10
		ON LN13.LF_STU_SSN = SD10.LF_STU_SSN
		AND LN13.LN_STU_SPR_SEQ = SD10.LN_STU_SPR_SEQ
		AND SD10.LC_STA_STU10 = 'A'
	INNER JOIN LN10_LON LN10
		ON LN13.BF_SSN = LN10.BF_SSN
		AND LN13.LN_SEQ = LN10.LN_SEQ
	INNER JOIN PD10_PRS_NME PD10
		ON LN10.BF_SSN = PD10.DF_PRS_ID
	INNER JOIN PD30_PRS_ADR PD30
		ON PD10.DF_PRS_ID = PD30.DF_PRS_ID
		AND PD30.DC_ADR = 'L'
	INNER JOIN DW01_DW_CLC_CLU DW01
		ON LN10.BF_SSN = DW01.BF_SSN
		AND LN10.LN_SEQ = DW01.LN_SEQ
	INNER JOIN
	(
		SELECT	
			SD10.LF_STU_SSN,
			SD10.LN_STU_SPR_SEQ,
		    CAST(SD10.LF_LST_DTS_SD10 AS DATE) AS LF_LST_DTS_SD10,
			SD10.LD_SCL_SPR
		FROM 
			LN13_LON_STU_OSD LN13
			INNER JOIN SD10_STU_SPR SD10
				ON LN13.LF_STU_SSN = SD10.LF_STU_SSN
				AND LN13.LN_STU_SPR_SEQ = SD10.LN_STU_SPR_SEQ
				AND SD10.LC_STA_STU10 = 'I'
		WHERE
			LN13.LC_STA_LON13 = 'I'
	) SD10_INACTIVE
		ON SD10_INACTIVE.LF_LST_DTS_SD10 = CAST(SD10.LF_LST_DTS_SD10 AS DATE)
		AND SD10_INACTIVE.LD_SCL_SPR != SD10.LD_SCL_SPR
		AND SD10_INACTIVE.LF_STU_SSN = SD10.LF_STU_SSN
WHERE
	CAST(SD10.LF_LST_DTS_SD10 AS DATE) = CAST(DATEADD(DAY, -1, @Today) AS DATE)
	AND SD10.LD_SCL_SPR < @Today
	AND SD10.LC_SCR_SCL_SPR != 'IN'
	AND LN10.LC_STA_LON10 = 'R'
	AND LN13.LC_STA_LON13 = 'A'

--FOR TESTING, to see what is in the base population 
--SELECT * FROM @Population

--Valid Address Population(Previously R2)
;WITH LoanDetail(NumberOfRow) AS 
(
	SELECT 1
	UNION ALL
	SELECT NumberOfRow + 1
	FROM LoanDetail
	WHERE NumberOfRow < @NumberOfRows
)
INSERT INTO ULS.[print].PrintProcessing(AccountNumber, EmailAddress, ScriptDataId, LetterData, CostCenter, DoNotProcessEcorr, OnEcorr, ArcNeeded, ImagingNeeded, AddedBy, AddedAt)
SELECT
	LETTER_INFO.AccountNumber,
	LETTER_INFO.EmailAddress,
	LETTER_INFO.ScriptDataId,
	LETTER_INFO.LetterData,
	LETTER_INFO.CostCenter,
	LETTER_INFO.DoNotProcessEcorr,
	LETTER_INFO.OnEcorr,
	LETTER_INFO.ArcNeeded,
	LETTER_INFO.ImagingNeeded,
	LETTER_INFO.AddedBy,
	LETTER_INFO.AddedAt
FROM
(
	SELECT
		POP.SSN AS Ssn,
		ADDR_INFO.AccountNumber AS AccountNumber,
		COALESCE(EMAIL_INFO.DX_CNC_EML_ADR, 'Ecorr@uheaa.org') AS EmailAddress,
		@ValidAddressScriptDataId AS ScriptDataId,
		LetterData = POP.SSN + ',' + ADDR_INFO.AccountNumber + ',' + ADDR_INFO.FirstName + ',' + ADDR_INFO.LastName + ',' + ADDR_INFO.Address1 + ',' + 
			ADDR_INFO.Address2 + ',' + ADDR_INFO.Address3 + ',' + ADDR_INFO.City + ',' + ADDR_INFO.[State] + ',' + ADDR_INFO.Country + ',' + ADDR_INFO.Zip + ',' + 
			ADDR_INFO.DI_VLD_ADR + ',' + CONVERT(VARCHAR(10), ISNULL(POP.LD_SCL_SPR, ''), 101) + ',' + CONVERT(VARCHAR(10), ISNULL(POP.LF_LST_DTS_SD10, ''), 101) + ',' + CONVERT(VARCHAR(10), ISNULL(POP.WD_LON_RPD_SR, ''), 101) + ',' + CentralData.dbo.CreateACSKeyLine(ADDR_INFO.AccountNumber, 'B', 'L') +
		--Loan Detil: IC_LON_PGM,LD_LON_1_DSB,LA_CUR_PRI
		STUFF
		(
			(
				SELECT	
					CASE WHEN x.Ssn IS NOT NULL THEN
						',' + COALESCE(FT.Label, LN10.IC_LON_PGM) --IC_LON_PGM
						+ ',' + CONVERT(VARCHAR(10), LN10.LD_LON_1_DSB, 101) --LD_LON_1_DSB
						+ ',' + CONVERT(VARCHAR(10), ISNULL(LN10.LA_CUR_PRI,0), 101) --LA_CUR_PRI
					ELSE
						',,,'
					END
				FROM
					LoanDetail LD
					LEFT JOIN 
					(	
						SELECT DISTINCT	
							POP_INNER.SSN,
							POP_INNER.LN_SEQ,
							ROW_NUMBER() OVER (ORDER BY POP_INNER.SSN, POP_INNER.LN_SEQ) AS RowNumber
						FROM
							@Population POP_INNER
						WHERE
							POP.SSN = POP_INNER.SSN
							AND POP_INNER.ValidAddress = 1
					) x
						ON x.RowNumber = LD.NumberOfRow	
						AND x.SSN = POP.SSN
					LEFT JOIN UDW..LN10_LON LN10
						ON x.SSN = LN10.BF_SSN
						AND x.LN_SEQ = LN10.LN_SEQ
						AND LN10.LA_CUR_PRI > 0.00
						AND LN10.LC_STA_LON10 IN ('R', 'L')
					LEFT JOIN UDW..FormatTranslation FT
						ON FT.[Start] = LN10.IC_LON_PGM
						AND FT.FmtName = '$LNPROG'
				ORDER BY
					LD.NumberOfRow
				FOR XML PATH(''), TYPE
			).value('.','VARCHAR(MAX)'),
			1,0,''
		) + ',' + ADDR_INFO.DC_DOM_ST + ',' + @CostCenter,
		@CostCenter AS CostCenter,
		@DoNotProcessEcorr AS DoNotProcessEcorr,
		CASE WHEN EMAIL_INFO.DI_VLD_CNC_EML_ADR = 'Y' AND EMAIL_INFO.DI_CNC_ELT_OPI = 'Y' 
			THEN 1 
			ELSE 0 
		END AS OnEcorr,
		@ArcNeeded AS ArcNeeded,
		@ImagingNeeded AS ImagingNeeded,
		GETDATE() AS AddedAt,
		SUSER_NAME() AS AddedBy
	FROM
	(
		SELECT DISTINCT
			SSN,
			LD_SCL_SPR,
			LF_LST_DTS_SD10,
			WD_LON_RPD_SR
		FROM
			@Population
		WHERE
			ValidAddress = 1
	) POP
	INNER JOIN 
	(
		SELECT
			BORR.DF_PRS_ID,
			--borrower info
			REPLACE(LTRIM(RTRIM(BORR.DM_PRS_1)),',','') AS FirstName,
			REPLACE(LTRIM(RTRIM(BORR.DM_PRS_LST)),',','') + ' ' + REPLACE(LTRIM(RTRIM(BORR.DM_PRS_LST_SFX)),',','') AS LastName,
			REPLACE(LTRIM(RTRIM(BORR.DM_PRS_1)),',','')  + ' ' + REPLACE(LTRIM(RTRIM(BORR.DM_PRS_LST)),',','') + ' ' + REPLACE(LTRIM(RTRIM(BORR.DM_PRS_LST_SFX)),',','')  AS Name,
			--address info
			REPLACE(LTRIM(RTRIM(ADDR.DX_STR_ADR_1)),',','')  AS Address1,
			REPLACE(LTRIM(RTRIM(ADDR.DX_STR_ADR_2)),',','')  AS Address2,
			REPLACE(LTRIM(RTRIM(ADDR.DX_STR_ADR_3)),',','')  AS Address3,
			REPLACE(LTRIM(RTRIM(ADDR.DM_FGN_CNY)),',','') AS Country,
			BORR.DF_SPE_ACC_ID AS AccountNumber,
			REPLACE(LTRIM(RTRIM(ADDR.DM_CT)),',','')  AS City,
			CASE
				WHEN LEN(RTRIM(ADDR.DM_FGN_ST)) > 0 THEN  RTRIM(ADDR.DM_FGN_ST)
				ELSE RTRIM(ADDR.DC_DOM_ST)
			END AS [State],
			ADDR.DC_DOM_ST AS DC_DOM_ST,
			CASE WHEN LEN(ADDR.DF_ZIP_CDE) = 9 
				THEN LEFT(LTRIM(RTRIM(ADDR.DF_ZIP_CDE)), 5) + '-' + RIGHT(LTRIM(RTRIM(ADDR.DF_ZIP_CDE)), 4)
				ELSE LTRIM(RTRIM(ADDR.DF_ZIP_CDE))
			END AS Zip,
			CASE WHEN DI_VLD_ADR = 'Y' 
				THEN 1
				ELSE 0
			END AS HasValidAddress,
			DI_VLD_ADR
		FROM
			UDW..PD10_PRS_NME BORR
			INNER JOIN UDW..PD30_PRS_ADR ADDR 
				ON ADDR.DF_PRS_ID = BORR.DF_PRS_ID
		WHERE
			--BORR.DF_PRS_ID = @BF_SSN
			ADDR.DC_ADR = 'L'
	) ADDR_INFO
		ON POP.SSN = ADDR_INFO.DF_PRS_ID 
	LEFT JOIN UDW..PH05_CNC_EML EMAIL_INFO
		ON EMAIL_INFO.DF_SPE_ID = ADDR_INFO.AccountNumber
) LETTER_INFO
--Check for duplicate letter for the day
LEFT JOIN
(
	SELECT
		PP.AccountNumber,
		SUBSTRING(PP.LetterData, CHARINDEX(',',PP.LetterData,0), LEN(PP.LetterData) - CHARINDEX(',',PP.LetterData,0)) AS LetterData
	FROM
		ULS.[print].PrintProcessing PP
		INNER JOIN ULS.[print].ScriptData SD
			ON PP.ScriptDataId = SD.ScriptDataId
		INNER JOIN ULS.[print].Letters L
			ON L.LetterId = SD.LetterId
	WHERE
		--PP.AccountNumber = @AccountNumber --**FOR TESTING ONLY
		SD.ScriptId = @ScriptId
		AND L.Letter = @Letter
		AND PP.DeletedAt IS NULL
		AND CAST(PP.AddedAt AS DATE) >= @LastWeek

) PP
	ON PP.AccountNumber = LETTER_INFO.AccountNumber
	AND PP.LetterData = SUBSTRING(LETTER_INFO.LetterData, CHARINDEX(',',LETTER_INFO.LetterData,0), LEN(LETTER_INFO.LetterData) - CHARINDEX(',',LETTER_INFO.LetterData,0))
WHERE
	PP.AccountNumber IS NULL


--*****************************************
--Invalid Address Population(Previously R3)
;WITH LoanDetail(NumberOfRow) AS 
(
	SELECT 1
	UNION ALL
	SELECT NumberOfRow + 1
	FROM LoanDetail
	WHERE NumberOfRow < @NumberOfRows
)
INSERT INTO ULS.[print].PrintProcessing(AccountNumber, EmailAddress, ScriptDataId, LetterData, CostCenter, DoNotProcessEcorr, OnEcorr, ArcNeeded, ImagingNeeded, AddedBy, AddedAt)
SELECT
	LETTER_INFO.AccountNumber,
	LETTER_INFO.EmailAddress,
	LETTER_INFO.ScriptDataId,
	LETTER_INFO.LetterData,
	LETTER_INFO.CostCenter,
	LETTER_INFO.DoNotProcessEcorr,
	LETTER_INFO.OnEcorr,
	LETTER_INFO.ArcNeeded,
	LETTER_INFO.ImagingNeeded,
	LETTER_INFO.AddedBy,
	LETTER_INFO.AddedAt
FROM
(
	SELECT
		POP.SSN AS Ssn,
		ADDR_INFO.AccountNumber AS AccountNumber,
		COALESCE(EMAIL_INFO.DX_CNC_EML_ADR, 'Ecorr@uheaa.org') AS EmailAddress,
		@InvalidAddressScriptDataId AS ScriptDataId,
		LetterData = POP.SSN + ',' + ADDR_INFO.AccountNumber + ',' + ADDR_INFO.FirstName + ',' + ADDR_INFO.LastName + ',' + ADDR_INFO.Address1 + ',' + 
			ADDR_INFO.Address2 + ',' + ADDR_INFO.Address3 + ',' + ADDR_INFO.City + ',' + ADDR_INFO.[State] + ',' + ADDR_INFO.Country + ',' + ADDR_INFO.Zip + ',' + 
			ADDR_INFO.DI_VLD_ADR + ',' + CONVERT(VARCHAR(10), ISNULL(POP.LD_SCL_SPR, ''), 101) + ',' + CONVERT(VARCHAR(10), ISNULL(POP.LF_LST_DTS_SD10, ''), 101) + ',' + CONVERT(VARCHAR(10), ISNULL(POP.WD_LON_RPD_SR, ''), 101) + ',' + CentralData.dbo.CreateACSKeyLine(ADDR_INFO.AccountNumber, 'B', 'L') +
		--Loan Detil: IC_LON_PGM,LD_LON_1_DSB,LA_CUR_PRI
		STUFF
		(
			(
				SELECT	
					CASE WHEN x.Ssn IS NOT NULL THEN
						',' + COALESCE(FT.Label, LN10.IC_LON_PGM) --IC_LON_PGM
						+ ',' + CONVERT(VARCHAR(10), LN10.LD_LON_1_DSB, 101) --LD_LON_1_DSB
						+ ',' + CONVERT(VARCHAR(10), ISNULL(LN10.LA_CUR_PRI,0), 101) --LA_CUR_PRI
					ELSE
						',,,'
					END
				FROM
					LoanDetail LD
					LEFT JOIN 
					(	
						SELECT DISTINCT	
							POP_INNER.SSN,
							POP_INNER.LN_SEQ,
							ROW_NUMBER() OVER (ORDER BY POP_INNER.SSN, POP_INNER.LN_SEQ) AS RowNumber
						FROM
							@Population POP_INNER
						WHERE
							POP.SSN = POP_INNER.SSN
							AND POP_INNER.ValidAddress = 0
					) x
						ON x.RowNumber = LD.NumberOfRow	
						AND x.SSN = POP.SSN
					LEFT JOIN UDW..LN10_LON LN10
						ON x.SSN = LN10.BF_SSN
						AND x.LN_SEQ = LN10.LN_SEQ
						AND LN10.LA_CUR_PRI > 0.00
						AND LN10.LC_STA_LON10 IN ('R', 'L')
					LEFT JOIN UDW..FormatTranslation FT
						ON FT.[Start] = LN10.IC_LON_PGM
						AND FT.FmtName = '$LNPROG'
				ORDER BY
					LD.NumberOfRow
				FOR XML PATH(''), TYPE
			).value('.','VARCHAR(MAX)'),
			1,0,''
		),
		@CostCenter AS CostCenter,
		@DoNotProcessEcorr AS DoNotProcessEcorr,
		CASE WHEN EMAIL_INFO.DI_VLD_CNC_EML_ADR = 'Y' AND EMAIL_INFO.DI_CNC_ELT_OPI = 'Y' 
			THEN 1 
			ELSE 0 
		END AS OnEcorr,
		@ArcNeeded AS ArcNeeded,
		@ImagingNeeded AS ImagingNeeded,
		GETDATE() AS AddedAt,
		SUSER_NAME() AS AddedBy
	FROM
	(
		SELECT DISTINCT
			SSN,
			LD_SCL_SPR,
			LF_LST_DTS_SD10,
			WD_LON_RPD_SR
		FROM
			@Population
		WHERE
			ValidAddress = 0
	) POP
	INNER JOIN 
	(
		SELECT
			BORR.DF_PRS_ID,
			--borrower info
			REPLACE(LTRIM(RTRIM(BORR.DM_PRS_1)),',','') AS FirstName,
			REPLACE(LTRIM(RTRIM(BORR.DM_PRS_LST)),',','') + ' ' + REPLACE(LTRIM(RTRIM(BORR.DM_PRS_LST_SFX)),',','') AS LastName,
			REPLACE(LTRIM(RTRIM(BORR.DM_PRS_1)),',','')  + ' ' + REPLACE(LTRIM(RTRIM(BORR.DM_PRS_LST)),',','') + ' ' + REPLACE(LTRIM(RTRIM(BORR.DM_PRS_LST_SFX)),',','')  AS Name,
			--address info
			REPLACE(LTRIM(RTRIM(ADDR.DX_STR_ADR_1)),',','')  AS Address1,
			REPLACE(LTRIM(RTRIM(ADDR.DX_STR_ADR_2)),',','')  AS Address2,
			REPLACE(LTRIM(RTRIM(ADDR.DX_STR_ADR_3)),',','')  AS Address3,
			REPLACE(LTRIM(RTRIM(ADDR.DM_FGN_CNY)),',','') AS Country,
			BORR.DF_SPE_ACC_ID AS AccountNumber,
			REPLACE(LTRIM(RTRIM(ADDR.DM_CT)),',','')  AS City,
			CASE
				WHEN LEN(RTRIM(ADDR.DM_FGN_ST)) > 0 THEN  RTRIM(ADDR.DM_FGN_ST)
				ELSE RTRIM(ADDR.DC_DOM_ST)
			END AS [State],
			ADDR.DC_DOM_ST AS DC_DOM_ST,
			CASE WHEN LEN(ADDR.DF_ZIP_CDE) = 9 
				THEN LEFT(LTRIM(RTRIM(ADDR.DF_ZIP_CDE)), 5) + '-' + RIGHT(LTRIM(RTRIM(ADDR.DF_ZIP_CDE)), 4)
				ELSE LTRIM(RTRIM(ADDR.DF_ZIP_CDE))
			END AS Zip,
			CASE WHEN DI_VLD_ADR = 'Y' 
				THEN 1
				ELSE 0
			END AS HasValidAddress,
			DI_VLD_ADR
		FROM
			UDW..PD10_PRS_NME BORR
			INNER JOIN UDW..PD30_PRS_ADR ADDR 
				ON ADDR.DF_PRS_ID = BORR.DF_PRS_ID
		WHERE
			--BORR.DF_PRS_ID = @BF_SSN
			ADDR.DC_ADR = 'L'
	) ADDR_INFO
		ON POP.SSN = ADDR_INFO.DF_PRS_ID 
	LEFT JOIN UDW..PH05_CNC_EML EMAIL_INFO
		ON EMAIL_INFO.DF_SPE_ID = ADDR_INFO.AccountNumber
) LETTER_INFO
--Check for duplicate letter for the day
LEFT JOIN
(
	SELECT
		PP.AccountNumber,
		SUBSTRING(PP.LetterData, CHARINDEX(',',PP.LetterData,0), LEN(PP.LetterData) - CHARINDEX(',',PP.LetterData,0)) AS LetterData
	FROM
		ULS.[print].PrintProcessing PP
		INNER JOIN ULS.[print].ScriptData SD
			ON PP.ScriptDataId = SD.ScriptDataId
		INNER JOIN ULS.[print].Letters L
			ON L.LetterId = SD.LetterId
	WHERE
		--PP.AccountNumber = @AccountNumber --**FOR TESTING ONLY
		SD.ScriptId = @ScriptId
		AND L.Letter = @Letter
		AND PP.DeletedAt IS NULL
		AND CAST(PP.AddedAt AS DATE) >= @LastWeek

) PP
	ON PP.AccountNumber = LETTER_INFO.AccountNumber
	AND PP.LetterData = SUBSTRING(LETTER_INFO.LetterData, CHARINDEX(',',LETTER_INFO.LetterData,0), LEN(LETTER_INFO.LetterData) - CHARINDEX(',',LETTER_INFO.LetterData,0))
WHERE
	PP.AccountNumber IS NULL
