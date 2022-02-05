CREATE PROCEDURE [dbo].[GetAlternateFormatRecords]

AS
	SELECT
		DD.DocumentDetailsId, 
		ADDR_ACCT_NUM AS AccountNumber,
		CF.NTISCode AS AltFormatType,
		CF.CorrespondenceFormat AS AltFormatDescription,
		dd.[Path] AS FilePath,
		PD10.DM_PRS_1 AS FirstName,
		PD10.DM_PRS_LST AS LastName,
		PD30.DX_STR_ADR_1 AS Address1,
		PD30.DX_STR_ADR_2 AS Address2,
		'' AS Address3, --We do not use addresses 3, 4, & 5
		'' AS Address4,
		'' AS Address5,
		PD30.DM_CT AS City,
		PD30.DC_DOM_ST AS [State],
		PD30.DM_FGN_ST AS ForeignState,
		PD30.DM_FGN_CNY as Country,
		CASE 
			WHEN PD30.DC_DOM_ST = '' THEN ''
			ELSE SUBSTRING(DF_ZIP_CDE, 1, 5) --Only want the first five digits if 5-dig zip encountered
		END AS ZipFiveDigits,
		CASE
			WHEN LEN(DF_ZIP_CDE) = 9 THEN SUBSTRING(DF_ZIP_CDE, 6, 4)
			ELSE ''
		END AS ZipFourDigits,
		CASE 
			WHEN ISNULL(DM_FGN_CNY, '') != ''  THEN DF_ZIP_CDE --When foreign country field not NULL or empty
			ELSE ''
		END AS InternationalZip
	FROM
		DocumentDetails DD
		INNER JOIN CorrespondenceFormats CF
			ON CF.CorrespondenceFormatId = DD.CorrespondenceFormatId
		INNER JOIN [CDW].[dbo].PD10_PRS_NME PD10
			ON PD10.DF_SPE_ACC_ID = DD.ADDR_ACCT_NUM
		LEFT JOIN [CDW].[dbo].PD30_PRS_ADR PD30
			ON PD30.DF_PRS_ID = PD10.DF_PRS_ID
		WHERE
			DD.CorrespondenceFormatId != 1
			AND DD.CorrespondenceFormatSentDate IS NULL
			AND DD.Active = 1
			
RETURN 0
