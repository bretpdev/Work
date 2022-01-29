﻿
CREATE PROCEDURE [dbo].[GetSystemBorrowerDemographics]
	@AccountIdentifier VARCHAR(10) 
AS
BEGIN
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	SET NOCOUNT ON;
	DECLARE @SSN CHAR(9)
	IF LEN(@AccountIdentifier) = 10
	BEGIN
		SET @SSN = (SELECT DF_PRS_ID FROM  PD10_PRS_NME WHERE DF_SPE_ACC_ID = @AccountIdentifier)
	END
	ELSE
		SET @SSN = @AccountIdentifier

	SELECT DISTINCT
		--borrower info
		PD10.DF_SPE_ACC_ID AS AccountNumber,
		PD10.DF_PRS_ID AS SSN,
		LTRIM(RTRIM(PD10.DM_PRS_1)) AS FirstName,
		LTRIM(RTRIM(PD10.DM_PRS_MID))  AS MiddleInitial,
		LTRIM(RTRIM(PD10.DM_PRS_LST))  AS LastName,
		CONVERT(VARCHAR, PD10.DD_BRT, 101) AS DateOfBirth,
		--address info
		LTRIM(RTRIM(PD30.DX_STR_ADR_1))  AS Address1,
		LTRIM(RTRIM(PD30.DX_STR_ADR_2))  AS Address2,
		LTRIM(RTRIM(PD30.DM_CT))  AS City,
		LTRIM(RTRIM(PD30.DC_DOM_ST))  AS [State],
		LTRIM(RTRIM(PD30.DF_ZIP_CDE))  AS ZipCode,
		LTRIM(RTRIM(PD30.DM_FGN_CNY))  AS Country,
		LTRIM(RTRIM(PD30.DM_FGN_ST))  AS ForeignState,
		PD30.DI_VLD_ADR AS IsValidAddress,
		CONVERT(VARCHAR, PD30.DD_VER_ADR, 101) AS AddressValidityDate,
		--home phone info
		MAX(CASE 
			WHEN PD40.DC_PHN = 'H' THEN ISNULL(LTRIM(RTRIM(PD40.DN_DOM_PHN_ARA)) + LTRIM(RTRIM(PD40.DN_DOM_PHN_XCH)) + LTRIM(RTRIM(PD40.DN_DOM_PHN_LCL)), '')
			ELSE ''
		END) OVER (PARTITION BY PD40.DF_PRS_ID) AS PrimaryPhone,
		'H' AS PrimaryPhoneType,
		MAX(CASE 
			WHEN PD40.DC_PHN = 'H' THEN CASE 
											WHEN PD40.DC_ALW_ADL_PHN IN ('N','P') THEN 'M' 
											WHEN PD40.DC_ALW_ADL_PHN IN ('L','Q') THEN 'L' 
											WHEN PD40.DC_ALW_ADL_PHN IN ('U','X') THEN 'U' 
										END
			ELSE ''
		END) OVER (PARTITION BY PD40.DF_PRS_ID) AS PrimaryMBLIndicator,
		MAX(CASE 
			WHEN PD40.DC_PHN = 'H' THEN CASE 
											WHEN PD40.DC_ALW_ADL_PHN IN ('L','P','X') THEN 'Y' 
											ELSE 'N'
										END
			ELSE ''
		END) OVER (PARTITION BY PD40.DF_PRS_ID) AS PrimaryConsentIndicator,
		CONVERT(BIT,MAX(CASE 
			WHEN PD40.DC_PHN = 'H' THEN (CASE WHEN ISNULL(PD40.DI_PHN_VLD, 'N') = 'Y' THEN 1 ELSE 0 END)
			ELSE 0
		END) OVER (PARTITION BY PD40.DF_PRS_ID)) AS IsPrimaryPhoneValid,
		MAX(CASE 
			WHEN PD40.DC_PHN = 'H' THEN ISNULL(CONVERT(VARCHAR, PD40.DD_PHN_VER, 101), '')
			ELSE ''
		END) OVER (PARTITION BY PD40.DF_PRS_ID) AS PhoneValidityDate,
		--alternate phone info
		MAX(CASE 
			WHEN PD40.DC_PHN = 'A' THEN ISNULL(LTRIM(RTRIM(PD40.DN_DOM_PHN_ARA)) + LTRIM(RTRIM(PD40.DN_DOM_PHN_XCH)) + LTRIM(RTRIM(PD40.DN_DOM_PHN_LCL)), '')
			ELSE ''
		END) OVER (PARTITION BY PD40.DF_PRS_ID) AS AlternatePhone,
		'A' AS AlternatePhoneType,
		MAX(CASE 
			WHEN PD40.DC_PHN = 'A' THEN CASE 
											WHEN PD40.DC_ALW_ADL_PHN IN ('N','P') THEN 'M' 
											WHEN PD40.DC_ALW_ADL_PHN IN ('L','Q') THEN 'L' 
											WHEN PD40.DC_ALW_ADL_PHN IN ('U','X') THEN 'U' 
										END
			ELSE ''
		END) OVER (PARTITION BY PD40.DF_PRS_ID) AS AlternateMBLIndicator,
		MAX(CASE 
			WHEN PD40.DC_PHN = 'A' THEN CASE 
											WHEN PD40.DC_ALW_ADL_PHN IN ('L','P','X') THEN 'Y' 
											ELSE 'N'
										END
			ELSE ''
		END) OVER (PARTITION BY PD40.DF_PRS_ID) AS AlternateConsentIndicator,
		MAX(CASE 
			WHEN PD40.DC_PHN = 'A' THEN (CASE WHEN ISNULL(PD40.DI_PHN_VLD, 'N') = 'Y' THEN 1 ELSE 0 END)
			ELSE 0
		END) OVER (PARTITION BY PD40.DF_PRS_ID) AS IsAlternatePhoneValid,
		MAX(CASE 
			WHEN PD40.DC_PHN = 'A' THEN ISNULL(CONVERT(VARCHAR, PD40.DD_PHN_VER,101), '')
			ELSE ''
		END) OVER (PARTITION BY PD40.DF_PRS_ID) AS AlternamtePhoneValidityDate,
		--home email info
		MAX(CASE WHEN PD32.DC_ADR_EML = 'H' THEN ISNULL(PD32.DX_ADR_EML, '') ELSE '' END) OVER (PARTITION BY PD32.DF_PRS_ID) AS EmailAddress,
		MAX(CASE WHEN PD32.DC_ADR_EML = 'H' THEN (CASE WHEN ISNULL(PD32.DI_VLD_ADR_EML, 'N') = 'Y' THEN 1 ELSE 0 END) ELSE '' END) OVER (PARTITION BY PD32.DF_PRS_ID) AS IsValidEmail,
		MAX(CASE WHEN PD32.DC_ADR_EML = 'H' THEN ISNULL(CONVERT(VARCHAR,PD32.DD_VER_ADR_EML,101), '') ELSE '' END) OVER (PARTITION BY PD32.DF_PRS_ID) AS EmailValidityDate
	FROM
		PD10_PRS_NME PD10 
		INNER JOIN PD30_PRS_ADR PD30 ON
			PD10.DF_PRS_ID = PD30.DF_PRS_ID
		LEFT JOIN PD32_PRS_ADR_EML PD32
			ON PD32.DF_PRS_ID = PD10.DF_PRS_ID
			AND PD32.DC_STA_PD32 = 'A'	
		LEFT JOIN PD40_PRS_PHN PD40
			ON PD40.DF_PRS_ID = PD10.DF_PRS_ID						  
	WHERE 
		PD10.DF_PRS_ID = @SSN
		AND PD30.DC_ADR = 'L'
END


GRANT EXECUTE ON GetSystemBorrowerDemographics TO db_executor