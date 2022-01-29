USE [UDW]
GO
/****** Object:  StoredProcedure [dbo].[GetBorrowerDemographics]    Script Date: 4/13/2018 11:00:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[GetBorrowerDemographics]
	@AccountIdentifier VARCHAR(10)
AS

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

SELECT 
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
 WHERE  
	@AccountIdentifier IN (PD10.DF_PRS_ID, PD10.DF_SPE_ACC_ID)