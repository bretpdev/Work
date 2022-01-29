USE ODW
GO

ALTER PROCEDURE [dbo].[GetBorrowerDemographics]
	@AccountIdentifier VARCHAR(10)
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @Borrower varchar(10) = 
	(
		SELECT
			TOP 1 GA01.DF_PRS_ID_BR
		FROM
			GA01_APP GA01
			INNER JOIN PD01_PDM_INF PD01
				ON GA01.DF_PRS_ID_BR = PD01.DF_PRS_ID
		WHERE
			@AccountIdentifier IN (PD01.DF_PRS_ID, PD01.DF_SPE_ACC_ID)
	)

DECLARE @Student varchar(10)
IF @Borrower = '' OR @Borrower IS NULL
	SET @Student = 
		(
			SELECT
				TOP 1 GA01.DF_PRS_ID_STU
			FROM
				GA01_APP GA01
				INNER JOIN PD01_PDM_INF PD01
					ON GA01.DF_PRS_ID_STU = PD01.DF_PRS_ID
			WHERE
				@AccountIdentifier IN (PD01.DF_PRS_ID, PD01.DF_SPE_ACC_ID)
		)

IF LEN(@Student) > 0
	SET @AccountIdentifier = 
		(
			SELECT
				TOP 1 DF_PRS_ID_BR
			FROM
				GA01_APP
			WHERE
				DF_PRS_ID_STU = @Student
		)

DECLARE @SSN VARCHAR(9) = (SELECT DF_PRS_ID FROM PD01_PDM_INF WHERE @AccountIdentifier IN (DF_PRS_ID, DF_SPE_ACC_ID))

SELECT DISTINCT
	PD01.DF_PRS_ID [AccountIdentifier],
	PD01.DF_SPE_ACC_ID [AccountNumber],
	PD01.DF_PRS_ID [Ssn],
 	ISNULL(NULLIF(RTRIM(PD01.DM_PRS_1) + ' ' + RTRIM(PD01.DM_PRS_MID) +  ' ' + RTRIM(PD01.DM_PRS_LST), ' '),'') AS [Name],  
 	CASE WHEN ADR_DATA.BorrAddress1 != '' THEN ADR_DATA.BorrAddress1 ELSE 'No address record on file' END AS [Address1], 
 	ISNULL(ADR_DATA.BorrAddress2,'') AS [Address2],
 	ISNULL(ADR_DATA.BorrCity,'') AS [City], 
 	ISNULL(ADR_DATA.BorrState,'') AS [State], 
 	ISNULL(ADR_DATA.BorrZip,'') AS [Zip],
	0 [IsFederal],
	1 [IsOneLink]
FROM  
 	PD01_PDM_INF PD01
 	LEFT JOIN  
 	( 
 		SELECT 
  			ADR_RNK.DF_PRS_ID,  
  			ADR_RNK.BorrAddress1,  
 			ADR_RNK.BorrAddress2, 
 			ADR_RNK.BorrCountry, 
 			ADR_RNK.BorrCity, 
 			ADR_RNK.BorrState, 
 			ADR_RNK.BorrZip, 
  			ROW_NUMBER() OVER (PARTITION BY ADR_RNK.DF_PRS_ID ORDER BY ADR_RNK.VLD, ADR_RNK.RNK) AS ADR_PRIORITY 
 			FROM  
 			(  
  				SELECT  
  					PD03.DF_PRS_ID,  
 					CASE PD03.DC_ADR   
  						WHEN 'L' THEN 1  
  						WHEN 'B' THEN 2  
  						WHEN 'D' THEN 3  
  						ELSE 4  
  					END AS RNK,  
  					CASE PD03.DI_VLD_ADR  
  						WHEN 'Y' THEN 1  
  						WHEN 'N' THEN 2  
  						ELSE 3  
  					END AS VLD,  
  					ISNULL(RTRIM(PD03.DX_STR_ADR_1),'') AS BorrAddress1, 
 					ISNULL(RTRIM(PD03.DX_STR_ADR_2),'') AS BorrAddress2, 
 					ISNULL(RTRIM(PD03.DM_FGN_CNY),'') AS BorrCountry, 
 					ISNULL(RTRIM(PD03.DM_CT),'') AS BorrCity, 
 					ISNULL(RTRIM(PD03.DC_DOM_ST),'') AS BorrState, 
 					ISNULL(RTRIM(PD03.DF_ZIP),'') AS BorrZip 
  				FROM 
 					PD03_PRS_ADR_PHN PD03
 				WHERE  
 					PD03.DF_PRS_ID = @SSN 
 		) AS ADR_RNK  
 	) AS ADR_DATA  
 		ON ADR_DATA.DF_PRS_ID = PD01.DF_PRS_ID  
 		AND ADR_DATA.ADR_PRIORITY = 1
	LEFT JOIN GA01_APP GA01
		ON PD01.DF_PRS_ID = GA01.DF_PRS_ID_BR
	LEFT JOIN GA10_LON_APP GA10
		ON GA01.AF_APL_ID = GA10.AF_APL_ID
WHERE  
 	@AccountIdentifier IN (PD01.DF_PRS_ID, PD01.DF_SPE_ACC_ID)
	AND
	GA10.AA_CUR_PRI > 0

UNION

SELECT DISTINCT
	BR03.DF_PRS_ID_RFR [AccountIdentifier],
	'' [AccountNumber],
	'' [Ssn],
	RTRIM(BR03.BM_RFR_1) + ' ' + RTRIM(BR03.BM_RFR_MID) +  ' ' + RTRIM(BR03.BM_RFR_LST) [Name],  
 	CASE WHEN BR03.BX_RFR_STR_ADR_1 != '' THEN BR03.BX_RFR_STR_ADR_1 ELSE 'No address record on file' END AS [Address1], 
 	ISNULL(RTRIM(BR03.BX_RFR_STR_ADR_2),'') AS [Address2],
 	ISNULL(RTRIM(BR03.BM_RFR_CT),'') AS [City], 
 	ISNULL(RTRIM(BR03.BC_RFR_ST),'') AS [State], 
 	ISNULL(RTRIM(BR03.BF_RFR_ZIP),'') AS [Zip],
	0 [IsFederal],
	1 [IsOneLink]
FROM
	BR03_BR_REF BR03
WHERE
 	BR03.DF_PRS_ID_RFR = @AccountIdentifier
GO

ALTER PROCEDURE [docid].[CaseNumberSearch]
	@CaseNumber varchar(12)
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @DVAL DATE = DATEADD(YEAR, -10, GETDATE());

--OneLink bankruptcy
SELECT DISTINCT 
	PD01.DF_PRS_ID [AccountIdentifier]
	,RTRIM(PD01.DM_PRS_1) + ' ' + RTRIM(PD01.DM_PRS_LST) [Name],
	1 IsOneLink
FROM
	ODW..DC01_LON_CLM_INF DC01
	INNER JOIN ODW..DC18_BKR DC18
		ON DC01.AF_APL_ID = DC18.AF_APL_ID
		AND DC01.AF_APL_ID_SFX = DC18.AF_APL_ID_SFX
		AND DC01.LF_CRT_DTS_DC10 = DC18.LF_CRT_DTS_DC10
	INNER JOIN ODW..PD01_PDM_INF PD01
		ON DC01.BF_SSN = PD01.DF_PRS_ID
WHERE
	DC18.LD_BKR_FIL > @DVAL
	AND
	REPLACE(RTRIM(DC18.LF_BKR_DKT), '-', '') = REPLACE(RTRIM(@CaseNumber), '-', '')

RETURN 0;
GO

ALTER PROCEDURE [docid].[GetBorrowersForEndorser]
	@Endorser VARCHAR(10)
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @EndSsn VARCHAR(9)
	IF LEN(@Endorser) < 10
		BEGIN
			SET @EndSsn = (SELECT DF_PRS_ID FROM ODW..PD01_PDM_INF WHERE DF_SPE_ACC_ID = @Endorser)
		END

	SELECT
		PD01.DF_PRS_ID [AccountIdentifier],
		RTRIM(PD01.DM_PRS_1) + ' ' + RTRIM(PD01.DM_PRS_MID) + ' ' + RTRIM(PD01.DM_PRS_LST) [Name],
		0 [IsFederal],
		1 [IsOneLink]
	FROM
		[ODW].[dbo].[GA01_APP] GA01
		LEFT JOIN ODW..PD01_PDM_INF PD01
			ON GA01.DF_PRS_ID_BR = PD01.DF_PRS_ID
		LEFT JOIN ODW..GA10_LON_APP GA10
			ON GA01.AF_APL_ID = GA10.AF_APL_ID
	WHERE
		DF_PRS_ID_EDS IN (@Endorser, @EndSsn)
		AND
		GA10.AA_CUR_PRI > 0

RETURN 0
GO


-------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------
USE UDW
GO

ALTER PROCEDURE [dbo].[GetBorrowerDemographics]
	@AccountIdentifier VARCHAR(10)
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @Borrower varchar(10) =
	(
		SELECT
			TOP 1 LN10.BF_SSN
		FROM
			LN10_LON LN10
			INNER JOIN PD10_PRS_NME PD10
				ON LN10.BF_SSN = PD10.DF_PRS_ID
		WHERE
			@AccountIdentifier IN (PD10.DF_PRS_ID, PD10.DF_SPE_ACC_ID)
	)

DECLARE @Student varchar(10)
IF @Borrower = '' OR @Borrower IS NULL
	SET @Student = 
		(
			SELECT
				TOP 1 LN10.LF_STU_SSN
			FROM
				LN10_LON LN10
				INNER JOIN PD10_PRS_NME PD10
					ON LN10.LF_STU_SSN = PD10.DF_PRS_ID
			WHERE
				@AccountIdentifier IN (PD10.DF_PRS_ID, PD10.DF_SPE_ACC_ID)
		)

IF LEN(@Student) > 0
	SET @AccountIdentifier = 
		(
			SELECT
				TOP 1 BF_SSN
			FROM
				LN10_LON
			WHERE
				LF_STU_SSN = @Student
		)

SELECT DISTINCT
	PD10.DF_PRS_ID [AccountIdentifier],
	PD10.DF_SPE_ACC_ID [AccountNumber],
	PD10.DF_PRS_ID [Ssn],
 	ISNULL(NULLIF(RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_MID) +  ' ' + RTRIM(PD10.DM_PRS_LST), ' '),'') AS [Name],  
 	CASE WHEN ADR_DATA.BorrAddress1 != '' THEN ADR_DATA.BorrAddress1 ELSE 'No address record on file' END AS [Address1], 
 	ISNULL(ADR_DATA.BorrAddress2,'') AS [Address2],
 	ISNULL(ADR_DATA.BorrCity,'') AS [City], 
 	ISNULL(ADR_DATA.BorrState,'') AS [State], 
 	ISNULL(ADR_DATA.BorrZip,'') AS [Zip],
	ISNULL(ADR_DATA.BorrCountry,'') AS [Country],
	0 [IsFederal]
FROM  
	PD10_PRS_NME PD10
 	LEFT JOIN  
 	( 
 		SELECT 
  			ADR_RNK.DF_PRS_ID,  
  			ADR_RNK.BorrAddress1,  
 			ADR_RNK.BorrAddress2, 
 			ADR_RNK.BorrCountry, 
 			ADR_RNK.BorrCity, 
 			ADR_RNK.BorrState, 
 			ADR_RNK.BorrZip, 
  			ROW_NUMBER() OVER (PARTITION BY ADR_RNK.DF_PRS_ID ORDER BY ADR_RNK.VLD, ADR_RNK.RNK) AS ADR_PRIORITY 
 		 FROM  
 		 (  
  			SELECT  
  				PD30.DF_PRS_ID,  
 				CASE PD30.DC_ADR   
  					WHEN 'L' THEN 1  
  					WHEN 'B' THEN 2  
  					WHEN 'D' THEN 3  
  					ELSE 4  
  				END AS RNK,  
  				CASE PD30.DI_VLD_ADR  
  					WHEN 'Y' THEN 1  
  					WHEN 'N' THEN 2  
  					ELSE 3  
  				END AS VLD,  
  				ISNULL(RTRIM(PD30.DX_STR_ADR_1),'') AS BorrAddress1, 
 				ISNULL(RTRIM(PD30.DX_STR_ADR_2),'') AS BorrAddress2, 
 				ISNULL(RTRIM(PD30.DM_FGN_CNY),'') AS BorrCountry, 
 				ISNULL(RTRIM(PD30.DM_CT),'') AS BorrCity, 
 				ISNULL(RTRIM(COALESCE(NULLIF(PD30.DC_DOM_ST,''),PD30.DM_FGN_ST)),'') AS BorrState,
 				ISNULL(RTRIM(PD30.DF_ZIP_CDE),'') AS BorrZip 
  			FROM 
 				PD30_PRS_ADR PD30 
				LEFT JOIN PD10_PRS_NME PD10
					ON PD30.DF_PRS_ID = PD10.DF_PRS_ID
 			WHERE  
 				@AccountIdentifier IN (PD10.DF_PRS_ID, PD10.DF_SPE_ACC_ID) 
 		) AS ADR_RNK  
 	) AS ADR_DATA  
 		ON ADR_DATA.DF_PRS_ID = PD10.DF_PRS_ID  
 		AND ADR_DATA.ADR_PRIORITY = 1
	LEFT JOIN LN10_LON LN10
		ON PD10.DF_PRS_ID = LN10.BF_SSN
 WHERE  
	@AccountIdentifier IN (PD10.DF_PRS_ID, PD10.DF_SPE_ACC_ID)
	AND
	LN10.LA_CUR_PRI > 0
	AND
	LN10.LC_STA_LON10 = 'R'
GO


ALTER PROCEDURE [docid].[CaseNumberSearch]
	@CaseNumber varchar(12)
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @DVAL DATE = DATEADD(YEAR, -10, GETDATE());

SELECT DISTINCT
	PD24.DF_PRS_ID [AccountIdentifier]
	,RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_LST) [Name],
	0 IsFederal
FROM
	UDW..PD24_PRS_BKR PD24
	LEFT JOIN UDW..PD10_PRS_NME PD10
		ON PD24.DF_PRS_ID = PD10.DF_PRS_ID
WHERE 
	PD24.DD_BKR_FIL > @DVAL
	AND
	REPLACE(RTRIM(PD24.DF_COU_DKT), '-', '') = REPLACE(RTRIM(@CaseNumber), '-', '')

RETURN 0;
GO


ALTER PROCEDURE [docid].[GetBorrowersForEndorser]
	@Endorser VARCHAR(10)
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @EndSsn VARCHAR(9)
	IF LEN(@Endorser) < 10
		BEGIN
			SET @EndSsn = (SELECT DF_PRS_ID FROM CDW..PD10_PRS_NME WHERE DF_SPE_ACC_ID = @Endorser)
		END

	SELECT DISTINCT
		LN20.BF_SSN [AccountIdentifier],
		RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_MID) + ' ' + RTRIM(PD10.DM_PRS_LST) [Name],
		0 [IsFederal]
	FROM
		UDW..LN20_EDS LN20
		LEFT JOIN UDW..PD10_PRS_NME PD10
			ON LN20.BF_SSN = PD10.DF_PRS_ID
		LEFT JOIN UDW..LN10_LON LN10
			ON LN20.BF_SSN = LN10.BF_SSN
	WHERE
		LN20.LF_EDS IN (@Endorser, @EndSsn)
		AND
		LN10.LA_CUR_PRI > 0
		AND
		LN10.LC_STA_LON10 = 'R'

RETURN 0