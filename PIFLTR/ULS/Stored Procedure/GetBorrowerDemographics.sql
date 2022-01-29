CREATE PROCEDURE [pifltr].[GetBorrowerDemographics]
	@AccountNumber VARCHAR(10)	
AS
	SELECT DISTINCT
		PQ.AccountNumber AS AccountNumber,
		PQ.Ssn AS BorrowerSsn,
		RTRIM(PD10.DM_PRS_1) AS BorrowerFirstName, 
		CASE 
			WHEN (RTRIM(ISNULL(PD10.DM_PRS_LST_SFX, '')) = '') THEN RTRIM(PD10.DM_PRS_LST)
			ELSE CONCAT(RTRIM(PD10.DM_PRS_LST), ' ', RTRIM(PD10.DM_PRS_LST_SFX))
		END AS BorrowerLastName,
		RTRIM(DX_STR_ADR_1) AS Address1,
		RTRIM(DX_STR_ADR_2) AS Address2,
		RTRIM(DM_CT) AS City,
		RTRIM(DC_DOM_ST) AS DomesticState,
		RTRIM(DF_ZIP_CDE) AS ZIPCode,
		RTRIM(DM_FGN_CNY) AS ForeignCountry,
		RTRIM(DM_FGN_ST) AS ForeignState,
		CASE 
			WHEN DI_VLD_ADR = 'Y' THEN 1 
			ELSE 0
		END AS IsValid
	FROM 
		UDW..PD30_PRS_ADR PD30
		INNER JOIN ULS.pifltr.ProcessingQueue PQ
			ON PQ.Ssn = PD30.DF_PRS_ID
		INNER JOIN UDW..PD10_PRS_NME PD10
			ON PD10.DF_PRS_ID = PD30.DF_PRS_ID
	WHERE
		DC_ADR = 'L'
		AND PQ.AccountNumber = @AccountNumber

RETURN 0

