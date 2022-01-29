DECLARE @TODAY DATE = GETDATE();
INSERT INTO OLS.trueaccord.Placements(AccountNumber, AF_APL_ID, AF_APL_ID_SFX, AccountNamespace, AccountRelationship, AccountOpenDate, BrandId, ProductType, DateAssigned, ExpectedRetractionDate, TransactionDate, FirstName, LastName, MiddleName, Prefix, Suffix, EmailAddress1, EmailAddress2, EmailAddress3, Telephone1, Telephone2, Telephone3, AddressLine1, AddressLine2, AddressLine3, AddressType, City, [State], ZipCode, TotalAmountDue, CurrentAmountDue, CurrentBalance, TotalDelinquentAmount, DelinquencyDate, CyclesDelinquent, LastPaymentAmount, LastPaymentDate, MonthToDateFeesPaid, MonthToDateInterestPaid, MonthToDatePrincipalPaid, PlacementNumber, CreatedAt, RetractedAt, RetractedBy, DeletedAt, DeletedBy)
SELECT
	PD01.DF_SPE_ACC_ID AS AccountNumber,
	DC01.AF_APL_ID,
	DC01.AF_APL_ID_SFX,
	'UHEAA' AS AccountNamespace,
	'Signer' AS AccountRelationship,
	CONVERT(VARCHAR,GA14.AD_LON_STA,1) AS AccountOpenDate,
	'UHEAA' AS BrandId,
	'FFELP Student Loan(s)' AS ProductType,
	CONVERT(VARCHAR,@TODAY,1) AS DateAssigned,
	CONVERT(VARCHAR,CAST(DATEADD(MONTH,6,@TODAY) AS DATE),1) AS ExpectedRetractionDate,
	CONVERT(VARCHAR,@TODAY,1) AS TransactionDate,
	LTRIM(RTRIM(PD01.DM_PRS_1)) AS FirstName,
	LTRIM(RTRIM(PD01.DM_PRS_LST)) AS LastName,
	LTRIM(RTRIM(PD01.DM_PRS_MID)) AS MiddleName,
	'' AS Prefix,
	'' AS Suffix,
	RTRIM(RankedEmail.DX_EML_ADR) AS EmailAddress1,
	'' AS EmailAddress2,
	'' AS EmailAddress3,
	IIF(PD03Demos.DI_PHN_VLD = 'Y' AND PD03Demos.DI_FGN_PHN = 'N', PD03Demos.DN_PHN, '') AS Telephone1,
	IIF(PD03Demos.DI_ALT_PHN_VLD = 'Y' AND PD03Demos.DI_FGN_PHN = 'N', PD03Demos.DN_ALT_PHN, '') AS Telephone2,
	IIF(PD03Demos.DI_OTH_PHN_VLD = 'Y' AND PD03Demos.DI_FGN_PHN = 'N', PD03Demos.DN_OTH_PHN, '') AS Telephone3,
	IIF(PD03Demos.DC_ADR = 'L' AND PD03Demos.DI_VLD_ADR = 'Y', RTRIM(PD03Demos.DX_STR_ADR_1), '') AS AddressLine1,
	IIF(PD03Demos.DC_ADR = 'L' AND PD03Demos.DI_VLD_ADR = 'Y', RTRIM(PD03Demos.DX_STR_ADR_2), '') AS AddressLine2,
	'' AS AddressLine3,
	IIF(PD03Demos.DC_ADR = 'L' AND PD03Demos.DI_VLD_ADR = 'Y', 'Home', '') AS AddressType,
	IIF(PD03Demos.DC_ADR = 'L' AND PD03Demos.DI_VLD_ADR = 'Y', RTRIM(PD03Demos.DM_CT), '') AS City,
	IIF(PD03Demos.DC_ADR = 'L' AND PD03Demos.DI_VLD_ADR = 'Y', RTRIM(PD03Demos.DC_DOM_ST), '') AS [State],
	IIF(PD03Demos.DC_ADR = 'L' AND PD03Demos.DI_VLD_ADR = 'Y', LEFT(RTRIM(PD03Demos.DF_ZIP),5), '') AS ZipCode,
	DC02.LA_CLM_BAL AS TotalAmountDue,
	DC02.LA_CLM_BAL AS CurrentAmountDue,
	DC02.LA_CLM_BAL AS CurrentBalance,
	DC02.LA_CLM_BAL AS TotalDelinquentAmount,
	CONVERT(VARCHAR,GA14.AD_LON_STA,1) AS DelinquencyDate,
	1 AS CyclesDelinquent,
	DC01.LA_TRX AS LastPaymentAmount,
	CONVERT(VARCHAR,DC01.LD_TRX_EFF,1) AS LastPaymentDate,
	0.00 AS MonthToDateFeesPaid,
	0.00 AS MonthToDateInterestPaid,
	0.00 AS MonthToDatePrincipalPaid,
	1 AS PlacementNumber,
	GETDATE() AS CreatedAt,
	NULL AS RetractedAt,
	NULL AS RetractedBy,
	NULL AS DeletedAt,
	NULL AS DeletedBy
FROM
	ODW..PD01_PDM_INF PD01
	INNER JOIN 
	(
		SELECT
			Email.DF_PRS_ID,
			Email.DX_EML_ADR,
			RANK() OVER(PARTITION BY Email.DF_PRS_ID ORDER BY EmailRank) AS EmailPriority
		FROM
		(
			SELECT
				PD03.DF_PRS_ID,
				PD03.DX_EML_ADR,
				CASE WHEN PD03.DC_ADR = 'L' THEN 1
					 WHEN PD03.DC_ADR = 'T' THEN 2
					 WHEN PD03.DC_ADR = 'A' THEN 3
					 WHEN PD03.DC_ADR = 'I' THEN 4
					 ELSE 5
				END AS EmailRank
			FROM
				ODW..PD03_PRS_ADR_PHN PD03
			WHERE
				ISNULL(PD03.DX_EML_ADR,'') != ''
				AND PD03.DI_EML_ADR_VAL = 'Y' --Valid email
		) Email
	)RankedEmail
		ON RankedEmail.DF_PRS_ID = PD01.DF_PRS_ID
		AND RankedEmail.EmailPriority = 1 --Most important
	INNER JOIN 
	(
		SELECT DISTINCT
			DC01.BF_SSN,
			DC01.AF_APL_ID,
			DC01.AF_APL_ID_SFX,
			DC01.LF_CRT_DTS_DC10,
			ISNULL(DC11.LA_TRX,0.00) AS LA_TRX,
			DC11.LD_TRX_EFF,
			MAX(DC01.LF_CRT_DTS_DC10) AS MaxDate
		FROM
			ODW..DC01_LON_CLM_INF DC01
			LEFT JOIN
			(
				SELECT DISTINCT
					DC11.AF_APL_ID,
					DC11.AF_APL_ID_SFX,
					DC11.LF_CRT_DTS_DC10,
					SUM(DC11.LA_TRX) OVER(PARTITION BY DC11.AF_APL_ID, DC11.AF_APL_ID_SFX, DC11.LF_CRT_DTS_DC10, DC11.LD_TRX_EFF) AS LA_TRX,
					DC11.LD_TRX_EFF
				FROM
					ODW..DC11_LON_FAT DC11
					INNER JOIN 
					(
						SELECT
							MaxDC11.AF_APL_ID,
							MaxDC11.AF_APL_ID_SFX,
							MaxDC11.LF_CRT_DTS_DC10,
							MAX(MaxDC11.LD_TRX_EFF) AS LD_TRX_EFF
						FROM
							ODW..DC11_LON_FAT MaxDC11
						WHERE
							MaxDC11.LC_REV_IND_TYP = ''
							AND MaxDC11.LC_TRX_TYP = 'BR'
						GROUP BY
							MaxDC11.AF_APL_ID,
							MaxDC11.AF_APL_ID_SFX,
							MaxDC11.LF_CRT_DTS_DC10
					) MaxDC11
						ON MaxDC11.AF_APL_ID = DC11.AF_APL_ID
						AND MaxDC11.AF_APL_ID_SFX = DC11.AF_APL_ID_SFX
						AND MaxDC11.LF_CRT_DTS_DC10 = DC11.LF_CRT_DTS_DC10
						AND MaxDC11.LD_TRX_EFF = DC11.LD_TRX_EFF
				WHERE
					DC11.LC_REV_IND_TYP = ''
					AND DC11.LC_TRX_TYP = 'BR'
			)DC11
				ON DC11.AF_APL_ID = DC01.AF_APL_ID
				AND DC11.AF_APL_ID_SFX = DC01.AF_APL_ID_SFX
				AND DC11.LF_CRT_DTS_DC10 = DC01.LF_CRT_DTS_DC10
		WHERE
			DC01.LC_PCL_REA = 'DF'
			AND DC01.LC_STA_DC10 = '03'
			AND DC01.LD_CLM_ASN_DOE IS NULL
			AND LTRIM(ISNULL(DC01.LC_AUX_STA,'')) = ''
			AND ISNULL(DC01.LC_REA_CLM_ASN_DOE,'') = ''
			AND 
			(
				DC11.LD_TRX_EFF <= CAST(DATEADD(MONTH,-12,@TODAY) AS DATE) --no payment in last 12 months
				OR DC11.LD_TRX_EFF IS NULL
			)
		GROUP BY
			DC01.BF_SSN,
			DC01.AF_APL_ID,
			DC01.AF_APL_ID_SFX,
			DC01.LF_CRT_DTS_DC10,
			DC11.LA_TRX,
			DC11.LD_TRX_EFF
	) DC01
		ON DC01.BF_SSN = PD01.DF_PRS_ID
	INNER JOIN ODW..DC02_BAL_INT DC02
		ON DC02.AF_APL_ID = DC01.AF_APL_ID
		AND DC02.AF_APL_ID_SFX = DC01.AF_APL_ID_SFX
		AND DC02.LF_CRT_DTS_DC10 = DC01.LF_CRT_DTS_DC10
		AND DC02.LF_CRT_DTS_DC10 = DC01.MaxDate
		AND DC02.LA_CLM_BAL > 0.00
	INNER JOIN ODW..GA14_LON_STA GA14
		ON GA14.AF_APL_ID = DC01.AF_APL_ID
		AND GA14.AF_APL_ID_SFX = DC01.AF_APL_ID_SFX
		AND GA14.AC_STA_GA14 = 'A'
		AND GA14.AC_LON_STA_TYP = 'CP'
		AND GA14.AC_LON_STA_REA = 'DF'
	LEFT JOIN ODW..PD03_PRS_ADR_PHN PD03Demos
		ON PD03Demos.DF_PRS_ID = PD01.DF_PRS_ID
		AND PD03Demos.DC_ADR = 'L'
	LEFT JOIN
	(
		SELECT
			AY01.DF_PRS_ID,
			MAX(AY01.BD_ATY_PRF) AS LastContact
		FROM
			ODW..AY01_BR_ATY AY01
		WHERE
			(
				AY01.BC_ATY_TYP = 'TC'
				AND AY01.BC_ATY_CNC_TYP IN('03','04')
			)
			OR
			(
				AY01.BC_ATY_TYP IN('LT','EM')
				AND AY01.BC_ATY_CNC_TYP = '04'
			)
		GROUP BY
			AY01.DF_PRS_ID
	) AY01
		ON AY01.DF_PRS_ID = DC01.BF_SSN
	LEFT JOIN OLS.trueaccord.Placements Existing
		ON Existing.AF_APL_ID = DC01.AF_APL_ID
		AND Existing.AF_APL_ID_SFX = DC01.AF_APL_ID_SFX
		AND Existing.DeletedAt IS NULL
		AND Existing.RetractedAt IS NULL
WHERE
	ISNULL(AY01.LastContact,'1900-01-01') <= CAST(DATEADD(MONTH,-6,@TODAY) AS DATE) --no contact in last 6 months
	AND Existing.AF_APL_ID IS NULL --Dont put people into placement table that are already there
