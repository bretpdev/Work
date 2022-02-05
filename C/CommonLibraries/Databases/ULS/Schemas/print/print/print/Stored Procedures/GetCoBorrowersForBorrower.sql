CREATE PROCEDURE [print].[GetCoBorrowersForBorrower]
(
	@BorrowerSSN CHAR(9)
)
AS

SELECT
	POP.CoBorrowerSSN,
	POP.FirstName,
	POP.MiddleName,
	POP.LastName,
	POP.AccountNumber,
	POP.ValidAddress,
	POP.[Address1],
	POP.[Address2],
	POP.[Address3],
	POP.City,
	POP.[State],
	POP.Zip,
	POP.ForeignState,
	POP.ForeignCountry,
	MAX(POP.OnEcorr)
FROM
(
	SELECT
		LN20.LF_EDS AS CoBorrowerSSN,
		PD10.DM_PRS_1 AS FirstName,
		PD10.DM_PRS_MID AS MiddleName,
		PD10.DM_PRS_LST AS LastName,
		PD10.DF_SPE_ACC_ID AS AccountNumber,
		PD30.DI_VLD_ADR AS ValidAddress,
		PD30.DX_STR_ADR_1 AS [Address1],
		PD30.DX_STR_ADR_2 AS [Address2],
		PD30.DX_STR_ADR_3 AS [Address3],
		PD30.DM_CT AS City,
		PD30.DC_DOM_ST AS [State],
		PD30.DF_ZIP_CDE AS Zip,
		PD30.DM_FGN_ST AS ForeignState,
		PD30.DM_FGN_CNY AS ForeignCountry,
		CASE WHEN PH05.DC_ELT_OPI_SRC = 'Y' THEN 1 ELSE 0 END AS OnEcorr
	FROM 
		UDW..LN10_LON LN10
		INNER JOIN UDW..LN20_EDS LN20
			ON LN20.BF_SSN = LN10.BF_SSN
			AND LN20.LN_SEQ = LN10.LN_SEQ
			AND LN20.LC_STA_LON20 = 'A'
			AND LN20.LC_EDS_TYP = 'M' -- Coborrower
		INNER JOIN UDW..PD10_PRS_NME PD10
			ON LN20.LF_EDS = PD10.DF_PRS_ID
		LEFT JOIN UDW..PH05_CNC_EML PH05
			ON PD10.DF_SPE_ACC_ID = PH05.DF_SPE_ID
		LEFT JOIN UDW..PD30_PRS_ADR PD30
			ON LN20.LF_EDS = PD30.DF_PRS_ID
			AND PD30.DC_ADR = 'L' 
	WHERE
		(PD30.DI_VLD_ADR = 'Y' 
		OR 
		(
			PH05.DX_CNC_EML_ADR IS NOT NULL 
			AND PH05.DI_VLD_CNC_EML_ADR  = 'Y'
			AND PH05.DC_ELT_OPI_SRC = 'Y'
		))
		AND LN10.BF_SSN = @BorrowerSSN

	UNION

	SELECT
		GA01.DF_PRS_ID_EDS AS CoBorrowerSSN,
		PD01.DM_PRS_1 AS FirstName,
		PD01.DM_PRS_MID AS MiddleName,
		PD01.DM_PRS_LST AS LastName,
		PD01.DF_SPE_ACC_ID AS AccountNumber,
		PD01.DI_VLD_ADR AS ValidAddress,
		PD01.DX_STR_ADR_1 AS [Address1],
		PD01.DX_STR_ADR_2 AS [Address2],
		'' AS [Address3],
		PD01.DM_CT AS City,
		PD01.DC_DOM_ST AS [State],
		PD01.DF_ZIP AS Zip,
		'' AS ForeignState,
		PD01.DM_FGN_CNY AS ForeignCountry,
		0 AS OnEcorr
	FROM
		ODW..GA01_APP GA01
		INNER JOIN ODW..GA10_LON_APP GA10
			ON GA01.AF_APL_ID = GA10.AF_APL_ID
			AND GA10.AA_CUR_PRI > 0
		INNER JOIN ODW..PD01_PDM_INF PD01
			ON GA01.DF_PRS_ID_EDS = PD01.DF_PRS_ID
			AND PD01.DI_VLD_ADR = 'Y'
	WHERE
		GA01.AC_APL_TYP = 'C'
		AND GA01.DF_PRS_ID_BR = @BorrowerSSN
)	POP
GROUP BY 
	POP.CoBorrowerSSN,
	POP.FirstName,
	POP.MiddleName,
	POP.LastName,
	POP.AccountNumber,
	POP.ValidAddress,
	POP.[Address1],
	POP.[Address2],
	POP.[Address3],
	POP.City,
	POP.[State],
	POP.Zip,
	POP.ForeignState,
	POP.ForeignCountry
