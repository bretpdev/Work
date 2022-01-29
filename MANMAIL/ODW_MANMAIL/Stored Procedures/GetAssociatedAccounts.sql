CREATE PROCEDURE [manmail].[GetAssociatedAccounts]
	@AccountIdentifier varchar(10)
AS
	SELECT DISTINCT -- Pulls Borrower
		PD01.DF_PRS_ID [AccountIdentifier],
		RTRIM(PD01.DM_PRS_1) + ' ' + RTRIM(PD01.DM_PRS_LST) [Name],
		RTRIM(PD03.DX_STR_ADR_1) [Address1],
		RTRIM(PD03.DX_STR_ADR_2) [Address2],
		RTRIM(PD03.DM_CT) [City],
		RTRIM(PD03.DC_DOM_ST) [State],
		RTRIM(PD03.DF_ZIP) [Zip],
		0 [Priority],
		'OneLink' [Region]
	FROM
		PD01_PDM_INF PD01
		INNER JOIN PD03_PRS_ADR_PHN PD03
			ON PD01.DF_PRS_ID = PD03.DF_PRS_ID
	WHERE
		@AccountIdentifier IN (PD01.DF_PRS_ID, PD01.DF_SPE_ACC_ID)

	UNION

	SELECT DISTINCT -- Pulls Borrower for Reference
		BR03.DF_PRS_ID_BR [AccountIdentifier],
		RTRIM(PD01.DM_PRS_1) + ' ' + RTRIM(PD01.DM_PRS_LST) [Name],
		RTRIM(PD03.DX_STR_ADR_1) [Address1],
		RTRIM(PD03.DX_STR_ADR_2) [Address2],
		RTRIM(PD03.DM_CT) [City],
		RTRIM(PD03.DC_DOM_ST) [State],
		RTRIM(PD03.DF_ZIP) [Zip],
		1 [Priority],
		'OneLink' [Region]
	FROM
		BR03_BR_REF BR03
		INNER JOIN PD01_PDM_INF PD01
			ON BR03.DF_PRS_ID_BR = PD01.DF_PRS_ID
		INNER JOIN PD03_PRS_ADR_PHN PD03
			ON BR03.DF_PRS_ID_BR = PD03.DF_PRS_ID
	WHERE
		@AccountIdentifier IN (BR03.DF_PRS_ID_RFR)

	UNION

	SELECT DISTINCT -- Pulls Reference for Borrower
		BR03.DF_PRS_ID_RFR [AccountIdentifier],
		RTRIM(BR03.BM_RFR_1) + ' ' + RTRIM(BR03.BM_RFR_LST) [Name],
		RTRIM(BR03.BX_RFR_STR_ADR_1) [Address1],
		RTRIM(BR03.BX_RFR_STR_ADR_2) [Address2],
		RTRIM(BR03.BM_RFR_CT) [City],
		RTRIM(BR03.BC_RFR_ST) [State],
		RTRIM(BR03.BF_RFR_ZIP) [Zip],
		2 [Priority],
		'OneLink' [Region]
	FROM
		BR03_BR_REF BR03
		INNER JOIN PD01_PDM_INF PD01
			ON BR03.DF_PRS_ID_BR = PD01.DF_PRS_ID
	WHERE
		@AccountIdentifier IN (PD01.DF_PRS_ID, PD01.DF_SPE_ACC_ID)

	UNION

	SELECT DISTINCT -- Pulls Borrower for Endorser
		PD01.DF_PRS_ID [AccountIdentifier],
		RTRIM(PD01.DM_PRS_1) + ' ' + RTRIM(PD01.DM_PRS_LST) [Name],
		RTRIM(PD03.DX_STR_ADR_1) [Address1],
		RTRIM(PD03.DX_STR_ADR_2) [Address2],
		RTRIM(PD03.DM_CT) [City],
		RTRIM(PD03.DC_DOM_ST) [State],
		RTRIM(PD03.DF_ZIP) [Zip],
		3 [Priority],
		'OneLink' [Region]
	FROM 
		PD01_PDM_INF PD01
		INNER JOIN PD03_PRS_ADR_PHN PD03
			ON PD01.DF_PRS_ID = PD03.DF_PRS_ID
		INNER JOIN GA01_APP GA01
			ON PD01.DF_PRS_ID = GA01.DF_PRS_ID_BR
		INNER JOIN PD01_PDM_INF PD01_Eds
			ON GA01.DF_PRS_ID_EDS = PD01_Eds.DF_PRS_ID
	WHERE 
		@AccountIdentifier IN (PD01_Eds.DF_PRS_ID, PD01_Eds.DF_SPE_ACC_ID)

	UNION

	SELECT DISTINCT --Pull Endorser when Endorser is passed in
		PD01.DF_PRS_ID [AccountIdentifier],
		RTRIM(PD01.DM_PRS_1) + ' ' + RTRIM(PD01.DM_PRS_LST) [Name],
		RTRIM(PD03.DX_STR_ADR_1) [Address1],
		RTRIM(PD03.DX_STR_ADR_2) [Address2],
		RTRIM(PD03.DM_CT) [City],
		RTRIM(PD03.DC_DOM_ST) [State],
		RTRIM(PD03.DF_ZIP) [Zip],
		4 [Priority],
		'OneLink' [Region]
	FROM
		GA01_APP GA01
		INNER JOIN PD01_PDM_INF PD01
			ON GA01.DF_PRS_ID_EDS = PD01.DF_PRS_ID
		INNER JOIN PD03_PRS_ADR_PHN PD03
			ON PD01.DF_PRS_ID = PD03.DF_PRS_ID
	WHERE
		@AccountIdentifier IN (PD01.DF_PRS_ID, PD01.DF_SPE_ACC_ID)

	UNION

	SELECT DISTINCT --Pulls Endorsers for Borrower
		PD01_Eds.DF_PRS_ID [AccountIdentifier],
		RTRIM(PD01_Eds.DM_PRS_1) + ' ' + RTRIM(PD01_Eds.DM_PRS_LST) [Name],
		RTRIM(PD03.DX_STR_ADR_1) [Address1],
		RTRIM(PD03.DX_STR_ADR_2) [Address2],
		RTRIM(PD03.DM_CT) [City],
		RTRIM(PD03.DC_DOM_ST) [State],
		RTRIM(PD03.DF_ZIP) [Zip],
		5 [Priority],
		'OneLink' [Region]
	FROM
		GA01_APP GA01
		INNER JOIN PD01_PDM_INF PD01
			ON GA01.DF_PRS_ID_BR = PD01.DF_PRS_ID
		INNER JOIN PD01_PDM_INF PD01_Eds
			ON GA01.DF_PRS_ID_EDS = PD01_Eds.DF_PRS_ID
		INNER JOIN PD03_PRS_ADR_PHN PD03
			ON PD01_Eds.DF_PRS_ID = PD03.DF_PRS_ID
	WHERE
		@AccountIdentifier IN (PD01.DF_PRS_ID, PD01.DF_SPE_ACC_ID)
	
	UNION

	SELECT DISTINCT --Pulls Reference when Reference is sent in
		BR03.DF_PRS_ID_RFR [AccountIdentifier],
		RTRIM(BR03.BM_RFR_1) + ' ' + RTRIM(BR03.BM_RFR_LST) [Name],
		RTRIM(BR03.BX_RFR_STR_ADR_1) [Address1],
		RTRIM(BR03.BX_RFR_STR_ADR_2) [Address2],
		RTRIM(BR03.BM_RFR_CT) [City],
		RTRIM(BR03.BC_RFR_ST) [State],
		RTRIM(BR03.BF_RFR_ZIP) [Zip],
		6 [Priority],
		'OneLink' [Region]
	FROM
		BR03_BR_REF BR03
	WHERE 
		@AccountIdentifier IN (BR03.DF_PRS_ID_RFR)
	ORDER BY
		[Priority]

RETURN 0