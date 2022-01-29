USE [CDW]
GO
/****** Object:  StoredProcedure [dbo].[GetBorrowerDemographics]    Script Date: X/XX/XXXX X:XX:XX AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[GetBorrowerDemographics]
	@AccountIdentifier VARCHAR(XX)
AS

DECLARE @Borrower varchar(XX) =
	(
		SELECT
			TOP X LNXX.BF_SSN
		FROM
			LNXX_LON LNXX
			INNER JOIN PDXX_PRS_NME PDXX
				ON LNXX.BF_SSN = PDXX.DF_PRS_ID
		WHERE
			@AccountIdentifier IN (PDXX.DF_PRS_ID, PDXX.DF_SPE_ACC_ID)
	)

DECLARE @Student varchar(XX)
IF @Borrower = '' OR @Borrower IS NULL
	SET @Student =
		(
			SELECT
				TOP X LNXX.LF_STU_SSN
			FROM
				LNXX_LON LNXX
				INNER JOIN PDXX_PRS_NME PDXX
					ON LNXX.LF_STU_SSN = PDXX.DF_PRS_ID
			WHERE
				@AccountIdentifier IN (PDXX.DF_PRS_ID, PDXX.DF_SPE_ACC_ID)
		)

IF LEN(@Student) > X
	SET @AccountIdentifier = 
		(
			SELECT
				TOP X BF_SSN
			FROM
				LNXX_LON
			WHERE
				LF_STU_SSN = @Student
		)

SELECT
	PDXX.DF_PRS_ID [AccountIdentifier],
	PDXX.DF_SPE_ACC_ID [AccountNumber],
	PDXX.DF_PRS_ID [Ssn],
 	ISNULL(NULLIF(RTRIM(PDXX.DM_PRS_X) + ' ' + RTRIM(PDXX.DM_PRS_MID) + ' '  + RTRIM(PDXX.DM_PRS_LST), ' '),'') AS [Name],
 	CASE WHEN ADR_DATA.BorrAddressX != '' THEN ADR_DATA.BorrAddressX ELSE 'No address record on file' END AS [AddressX],
 	ISNULL(ADR_DATA.BorrAddressX,'') AS [AddressX],
 	ISNULL(ADR_DATA.BorrCity,'') AS [City],
 	ISNULL(ADR_DATA.BorrState,'') AS [State],
 	ISNULL(ADR_DATA.BorrZip,'') AS [Zip],
	ISNULL(ADR_DATA.BorrCountry,'') AS [Country],
	X [IsFederal]
FROM
 	PDXX_PRS_NME PDXX
 	LEFT JOIN
 	(
 		SELECT
  			ADR_RNK.DF_PRS_ID,
  			ADR_RNK.BorrAddressX,
 			ADR_RNK.BorrAddressX,
 			ADR_RNK.BorrCountry,
 			ADR_RNK.BorrCity,
 			ADR_RNK.BorrState,
 			ADR_RNK.BorrZip,
  			ROW_NUMBER() OVER (PARTITION BY ADR_RNK.DF_PRS_ID ORDER BY ADR_RNK.VLD, ADR_RNK.RNK) AS ADR_PRIORITY
 		FROM
 		(
  			SELECT
  				PDXX.DF_PRS_ID,
 				CASE PDXX.DC_ADR
  					WHEN 'L' THEN X
  					WHEN 'B' THEN X
  					WHEN 'D' THEN X
  					ELSE X
  				END AS RNK,
  				CASE PDXX.DI_VLD_ADR
  					WHEN 'Y' THEN X
  					WHEN 'N' THEN X
  					ELSE X
  				END AS VLD,
  				ISNULL(RTRIM(PDXX.DX_STR_ADR_X),'') AS BorrAddressX,
 				ISNULL(RTRIM(PDXX.DX_STR_ADR_X),'') AS BorrAddressX,
 				ISNULL(RTRIM(PDXX.DM_FGN_CNY),'') AS BorrCountry,
 				ISNULL(RTRIM(PDXX.DM_CT),'') AS BorrCity,
 				ISNULL(RTRIM(COALESCE(NULLIF(PDXX.DC_DOM_ST,''),PDXX.DM_FGN_ST)),'') AS BorrState,
 				ISNULL(RTRIM(PDXX.DF_ZIP_CDE),'') AS BorrZip
  			FROM
 				PDXX_PRS_ADR PDXX
				INNER JOIN PDXX_PRS_NME PDXX
					ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
			WHERE
 				@AccountIdentifier IN (PDXX.DF_PRS_ID, PDXX.DF_SPE_ACC_ID)
 		) AS ADR_RNK
 	) AS ADR_DATA
 		ON ADR_DATA.DF_PRS_ID = PDXX.DF_PRS_ID
 		AND ADR_DATA.ADR_PRIORITY = X
WHERE
 	@AccountIdentifier IN (PDXX.DF_PRS_ID, PDXX.DF_SPE_ACC_ID)