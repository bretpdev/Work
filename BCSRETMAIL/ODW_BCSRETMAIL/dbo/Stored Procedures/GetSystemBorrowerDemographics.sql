
CREATE PROCEDURE [dbo].[GetSystemBorrowerDemographics]
	@AccountIdentifier VARCHAR(10)
AS

DECLARE @SSN VARCHAR(9)
IF LEN(@AccountIdentifier) = 10
	SELECT @SSN = (SELECT DF_PRS_ID FROM PD01_PDM_INF WHERE DF_SPE_ACC_ID = @AccountIdentifier)
ELSE
	SET @SSN = @AccountIdentifier


SELECT
	PD01.DF_SPE_ACC_ID [AccountNumber],
	PD01.DF_PRS_ID [Ssn],
 	RTRIM(PD01.DM_PRS_1) AS [FirstName],
	RTRIM(PD01.DM_PRS_MID) AS [MiddleInitial],
	RTRIM(PD01.DM_PRS_LST) AS [LastName],
 	CASE WHEN ADR_DATA.BorrAddress1 != '' THEN ADR_DATA.BorrAddress1 ELSE 'No address record on file' END AS [Address1],
 	ISNULL(ADR_DATA.BorrAddress2,'') AS [Address2],
 	ISNULL(ADR_DATA.BorrCity,'') AS [City],
 	ISNULL(ADR_DATA.BorrState,'') AS [State],
 	ISNULL(ADR_DATA.BorrZip,'') AS [ZipCode],
	ADR_DATA.IsValidAddress [IsValidAddress],
	ADR_DATA.AddressValidityDate [AddressValidityDate]
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
			ADR_RNK.IsValidAddress,
			ADR_RNK.AddressValidityDate,
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
 					ISNULL(RTRIM(PD03.DF_ZIP),'') AS BorrZip,
					ISNULL(RTRIM(PD03.DI_VLD_ADR),'') AS IsValidAddress,
					ISNULL(RTRIM(PD03.DD_ADR_EFF),'') AS AddressValidityDate
  				FROM
 					PD03_PRS_ADR_PHN PD03
 				WHERE
 					PD03.DF_PRS_ID = @SSN
 		) AS ADR_RNK
 	) AS ADR_DATA
 		ON ADR_DATA.DF_PRS_ID = PD01.DF_PRS_ID
 		AND ADR_DATA.ADR_PRIORITY = 1
WHERE
 	@AccountIdentifier IN (PD01.DF_PRS_ID, PD01.DF_SPE_ACC_ID)