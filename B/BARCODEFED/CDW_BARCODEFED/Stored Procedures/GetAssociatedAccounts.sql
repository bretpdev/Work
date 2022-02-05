CREATE PROCEDURE [barcodefed].[GetAssociatedAccounts]
	@AccountIdentifier varchar(10)
AS
	SELECT DISTINCT --Pulls borrower
		PD10.DF_PRS_ID [AccountIdentifier],
		RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_LST) [Name],
		RTRIM(PD30.DX_STR_ADR_1) [Address1],
		RTRIM(PD30.DX_STR_ADR_2) [Address2],
		RTRIM(PD30.DM_CT) [City],
		RTRIM(PD30.DC_DOM_ST) [State],
		RTRIM(PD30.DF_ZIP_CDE) [Zip],
		0 [Priority],
		'Compass' [Region]
	FROM
		PD10_PRS_NME PD10
		INNER JOIN PD30_PRS_ADR PD30
			ON PD10.DF_PRS_ID = PD30.DF_PRS_ID
	WHERE
		@AccountIdentifier IN (PD10.DF_PRS_ID, PD10.DF_SPE_ACC_ID)

	UNION

	SELECT DISTINCT --Pulls borrower for Endorsers
		LN20.BF_SSN [AccountIdentifier],
		RTRIM(PD10_Bor.DM_PRS_1) + ' ' + RTRIM(PD10_Bor.DM_PRS_LST) [Name],
		RTRIM(PD30.DX_STR_ADR_1) [Address1],
		RTRIM(PD30.DX_STR_ADR_2) [Address2],
		RTRIM(PD30.DM_CT) [City],
		RTRIM(PD30.DC_DOM_ST) [State],
		RTRIM(PD30.DF_ZIP_CDE) [Zip],
		1 [Priority],
		'Compass' [Region]
	FROM
		LN20_EDS LN20
		INNER JOIN PD10_PRS_NME PD10_End
			ON LN20.LF_EDS = PD10_End.DF_PRS_ID
		INNER JOIN PD10_PRS_NME PD10_Bor
			ON LN20.BF_SSN = PD10_Bor.DF_PRS_ID
		INNER JOIN PD30_PRS_ADR PD30
			ON LN20.BF_SSN = PD30.DF_PRS_ID
	WHERE
		@AccountIdentifier IN (PD10_End.DF_PRS_ID, PD10_End.DF_SPE_ACC_ID)

	UNION

	SELECT DISTINCT --Pulls borrower for References
		RF10.BF_SSN [AccountIdentifier],
		RTRIM(PD10_Bor.DM_PRS_1) + ' ' + RTRIM(PD10_Bor.DM_PRS_LST) [Name],
		RTRIM(PD30.DX_STR_ADR_1) [Address1],
		RTRIM(PD30.DX_STR_ADR_2) [Address2],
		RTRIM(PD30.DM_CT) [City],
		RTRIM(PD30.DC_DOM_ST) [State],
		RTRIM(PD30.DF_ZIP_CDE) [Zip],
		2 [Priority],
		'Compass' [Region]
	FROM
		RF10_RFR RF10
		INNER JOIN PD10_PRS_NME PD10_Ref
			ON RF10.BF_RFR = PD10_Ref.DF_PRS_ID
		INNER JOIN PD10_PRS_NME PD10_Bor
			ON RF10.BF_SSN = PD10_Bor.DF_PRS_ID
		INNER JOIN PD30_PRS_ADR PD30
			ON RF10.BF_SSN = PD30.DF_PRS_ID
	WHERE
		@AccountIdentifier IN (PD10_Ref.DF_PRS_ID, PD10_Ref.DF_SPE_ACC_ID)

	UNION
	SELECT DISTINCT --Pull Endorser when Endorser is passed in
		LN20.LF_EDS [AccountIdentifier],
		RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_LST) [Name],
		RTRIM(PD30.DX_STR_ADR_1) [Address1],
		RTRIM(PD30.DX_STR_ADR_2) [Address2],
		RTRIM(PD30.DM_CT) [City],
		RTRIM(PD30.DC_DOM_ST) [State],
		RTRIM(PD30.DF_ZIP_CDE) [Zip],
		3 [Priority],
		'Compass' [Region]
	FROM
		LN20_EDS LN20
		INNER JOIN PD10_PRS_NME PD10
			ON LN20.LF_EDS = PD10.DF_PRS_ID
		INNER JOIN PD30_PRS_ADR PD30
			ON LN20.LF_EDS = PD30.DF_PRS_ID
	WHERE
		@AccountIdentifier IN (PD10.DF_PRS_ID, PD10.DF_SPE_ACC_ID)

	UNION

	SELECT DISTINCT --Pulls Endorsers for Borrower
		LN20.LF_EDS [AccountIdentifier],
		RTRIM(PD10_End.DM_PRS_1) + ' ' + RTRIM(PD10_End.DM_PRS_LST) [Name],
		RTRIM(PD30.DX_STR_ADR_1) [Address1],
		RTRIM(PD30.DX_STR_ADR_2) [Address2],
		RTRIM(PD30.DM_CT) [City],
		RTRIM(PD30.DC_DOM_ST) [State],
		RTRIM(PD30.DF_ZIP_CDE) [Zip],
		4 [Priority],
		'Compass' [Region]
	FROM
		LN20_EDS LN20
		INNER JOIN PD10_PRS_NME PD10_Bor
			ON LN20.BF_SSN = PD10_Bor.DF_PRS_ID
		INNER JOIN PD10_PRS_NME PD10_End
			ON LN20.LF_EDS = PD10_End.DF_PRS_ID
		INNER JOIN PD30_PRS_ADR PD30
			ON LN20.LF_EDS = PD30.DF_PRS_ID
	WHERE
		@AccountIdentifier IN (PD10_Bor.DF_PRS_ID, PD10_Bor.DF_SPE_ACC_ID)

	UNION

	SELECT DISTINCT --Pulls References for Borrower
		RF10.BF_RFR [AccountIdentifier],
		RTRIM(PD10_Ref.DM_PRS_1) + ' ' + RTRIM(PD10_Ref.DM_PRS_LST) [Name],
		RTRIM(PD30.DX_STR_ADR_1) [Address1],
		RTRIM(PD30.DX_STR_ADR_2) [Address2],
		RTRIM(PD30.DM_CT) [City],
		RTRIM(PD30.DC_DOM_ST) [State],
		RTRIM(PD30.DF_ZIP_CDE) [Zip],
		5 [Priority],
		'Compass' [Region]
	FROM
		RF10_RFR RF10
		INNER JOIN PD10_PRS_NME PD10_Bor
			ON RF10.BF_SSN = PD10_Bor.DF_PRS_ID
		INNER JOIN PD10_PRS_NME PD10_Ref
			ON RF10.BF_RFR = PD10_Ref.DF_PRS_ID
		INNER JOIN PD30_PRS_ADR PD30
			ON RF10.BF_RFR = PD30.DF_PRS_ID
	WHERE
		@AccountIdentifier IN (PD10_Bor.DF_PRS_ID, PD10_Bor.DF_SPE_ACC_ID)

	UNION

	SELECT --Pulls Reference when Reference is sent in
		RF10.BF_RFR [AccountIdentifier],
		RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_LST) [Name],
		RTRIM(PD30.DX_STR_ADR_1) [Address1],
		RTRIM(PD30.DX_STR_ADR_2) [Address2],
		RTRIM(PD30.DM_CT) [City],
		RTRIM(PD30.DC_DOM_ST) [State],
		RTRIM(PD30.DF_ZIP_CDE) [Zip],
		6 [Priority],
		'Compass' [Region]
	FROM
		RF10_RFR RF10
		INNER JOIN PD10_PRS_NME PD10
			ON RF10.BF_RFR = PD10.DF_PRS_ID
		INNER JOIN PD30_PRS_ADR PD30
			ON RF10.BF_RFR = PD30.DF_PRS_ID
	WHERE
		@AccountIdentifier IN (PD10.DF_PRS_ID, PD10.DF_SPE_ACC_ID)
	ORDER BY
		[Priority]

RETURN 0