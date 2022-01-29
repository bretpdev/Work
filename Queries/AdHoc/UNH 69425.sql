--Select 500 defaulted borrowers with x amount of loans
--The BU wanted it to only pull 500 borrowers with 1 loan. I told debbie we may not have that so she asked if we could make it pull a specific amount of loans for each borrower(example all borrowers have 1 loan, or 3 loans  no mix matching of some with 1 and some with 10 )

SELECT TOP(500)
	ISNULL([Original Account Number],'') AS [Original Account Number],
	ISNULL([Account Number],'') AS [Account Number],
	ISNULL([Transaction ID],'') AS [Transaction ID],
	[Creditor], --static text
	[Original Creditor], --left blank
	[Current Creditor], --left blank
	[Merchant], --left blank
	RTRIM(LTRIM(ISNULL([First Name],''))) AS [First Name],
	RTRIM(LTRIM(ISNULL([Middle Name Or Initial],''))) AS [Middle Name Or Initial],
	RTRIM(LTRIM(ISNULL([Last Name],''))) AS [Last Name],
	[Suffix], --left blank
	RTRIM(LTRIM(ISNULL([Email Address 1],''))) AS [Email Address 1],
	[Email Address 2],--left blank
	RTRIM(LTRIM(ISNULL([Phone Number 1: Home Phone],''))) AS [Phone Number 1: Home Phone],
	RTRIM(LTRIM(ISNULL([Phone Number 2: Mobile Phone],''))) AS [Phone Number 2: Mobile Phone],
	RTRIM(LTRIM(ISNULL([Street Address (1)],''))) AS [Street Address (1)],
	[Apartment/suit(1)],--left blank
	RTRIM(LTRIM(ISNULL([City(1)],''))) AS [City(1)],
	RTRIM(LTRIM(ISNULL([State(1)],''))) AS [State(1)],
	RTRIM(LTRIM(ISNULL([Zip Code(1)],''))) AS [Zip Code(1)],
	RTRIM(LTRIM(ISNULL([Street Address (2)],''))) AS [Street Address (2)],
	[Apartment/suit(2)],--left blank
	RTRIM(LTRIM(ISNULL([City(2)],''))) AS [City(2)],
	RTRIM(LTRIM(ISNULL([State(2)],'')))AS [State(2)],
	RTRIM(LTRIM(ISNULL([Zip(2)],''))) AS [Zip(2)],
	[Service/Product], --static text
	'$' + CONVERT(VARCHAR(12),CAST(ISNULL([Outstanding Balance],0.00) AS money),1) AS [Outstanding Balance],
	'$' + CONVERT(VARCHAR(12),CAST(ISNULL([Principle Balance],0.00) AS money),1) AS [Principle Balance],
	'$' + CONVERT(VARCHAR(12),CAST(ISNULL([Current Interest],0.00) AS money),1) AS [Current Interest],
	'$' + CONVERT(VARCHAR(12),CAST(ISNULL([Current Fees],0.00) AS money),1) AS [Current Fees],
	--ISNULL([Origination Date],'') AS [Origination Date],
	ISNULL(RTRIM(CONVERT(VARCHAR(10),[Origination Date],101)),'') AS [Origination Date],
	--ISNULL([Default Date],'') AS [Default Date],
	ISNULL(RTRIM(CONVERT(VARCHAR(10),[Default Date],101)),'') AS [Default Date],
	[Charge-off Date],--blank
	--ISNULL([Date of Last Payment],'') AS [Date of Last Payment],
	ISNULL(RTRIM(CONVERT(VARCHAR(10),[Date of Last Payment],101)),'') AS [Date of Last Payment],
	[Date of Last Transaction], --left blank
	[Amount due at Charge- of], --left blank
	[Interest accrued since charge of], --left blank 
	[Amount Paid Since Charge-off], --left blank
	[Non- Interest/Fees accrued since charge-off], --left blank
	[Out of Statute Date], --left blank
	[Date of Obsolesc], --left blank
	'', --left blank
	ISNULL([Notes],'') as [Notes]
 FROM
 (
	SELECT
		COUNT(DC01.AF_APL_ID) OVER (PARTITION BY PD01.DF_SPE_ACC_ID) AS LOANS, --get count of loans so only borrowers with X number of loans will be included so all borrowers will have the same number of loans
		PD01.DF_SPE_ACC_ID AS [Original Account Number],
		PD01.DF_SPE_ACC_ID AS [Account Number],
		DC01.AF_APL_ID AS [Transaction ID],
		'UHEAA' AS [Creditor],
		'' AS [Original Creditor],
		'' AS [Current Creditor],
		'' AS [Merchant],
		PD01.DM_PRS_1 AS [First Name],
		PD01.DM_PRS_MID AS [Middle Name Or Initial],
		PD01.DM_PRS_LST AS [Last Name],
		'' AS [Suffix],
		PD03.DX_EML_ADR AS [Email Address 1],
		'' AS [Email Address 2],
		CASE
			WHEN 
				PD03.DI_PHN_VLD = 'Y'
				AND PD03.DI_FGN_PHN = 'N'
				AND PD03.DC_CEP NOT IN ('P','N')
			THEN PD03.DN_PHN
			WHEN PD03.DI_ALT_PHN_VLD = 'Y'
				AND PD03.DI_FGN_PHN = 'N'
				AND PD03.DC_ALT_CEP NOT IN ('P','N')
			THEN PD03.DN_ALT_PHN
			WHEN PD03.DI_OTH_PHN_VLD = 'Y'
				AND PD03.DI_FGN_PHN = 'N'
				AND PD03.DC_OTH_CEP NOT IN ('P','N')
			THEN PD03.DN_OTH_PHN
			ELSE ''
		END AS [Phone Number 1: Home Phone],
		CASE
			WHEN PD03.DI_PHN_VLD = 'Y'
				AND PD03.DI_FGN_PHN = 'N'
				AND PD03.DC_CEP IN ('P','N')
			THEN PD03.DN_PHN
			WHEN PD03.DI_ALT_PHN_VLD = 'Y'
				AND PD03.DI_FGN_PHN = 'N'
				AND PD03.DC_ALT_CEP IN ('P','N')
			THEN PD03.DN_ALT_PHN
			WHEN PD03.DI_OTH_PHN_VLD = 'Y'
				AND PD03.DI_FGN_PHN = 'N'
				AND PD03.DC_OTH_CEP IN ('P','N')
			THEN PD03.DN_OTH_PHN
			ELSE ''
		END AS [Phone Number 2: Mobile Phone],
		COALESCE(PD03L.DX_STR_ADR_1,PD03A.DX_STR_ADR_1,PD03T.DX_STR_ADR_1) AS [Street Address (1)],
	-- it was decided to leave this blank
		--COALESCE ----PD03L.DX_STR_AD1/DX_STR_ADR_2  AS [Apartment/suit(1)], --TODO: = There is no direct field for this, would need to parsed from 
		--	(
		--		CASE
		--			WHEN PATINDEX('%UNIT %',PD03L.DX_STR_ADR_2) > 0 THEN RTRIM(LTRIM(SUBSTRING(PD03L.DX_STR_ADR_2,PATINDEX('%UNIT %',PD03L.DX_STR_ADR_2),35))) --get UNIT first because sometimes APT or suite comes after UNIT and this includes all of it
		--			WHEN RTRIM(LTRIM(SUBSTRING(PD03L.DX_STR_ADR_2,PATINDEX('%APT %',PD03L.DX_STR_ADR_2),35))) = 'APT' THEN '' --weeds out numerous addresses with APT at the end but no apartment number
		--			WHEN PATINDEX('%APT %',PD03L.DX_STR_ADR_2) > 0 THEN RTRIM(LTRIM(SUBSTRING(PD03L.DX_STR_ADR_2,PATINDEX('%APT %',PD03L.DX_STR_ADR_2),35)))
		--			WHEN PATINDEX('% STE %',PD03L.DX_STR_ADR_2) > 0 THEN RTRIM(LTRIM(SUBSTRING(PD03L.DX_STR_ADR_2,PATINDEX('% STE %',PD03L.DX_STR_ADR_2),35)))
		--			WHEN PATINDEX('%SUITE %',PD03L.DX_STR_ADR_2) > 0 THEN RTRIM(LTRIM(SUBSTRING(PD03L.DX_STR_ADR_2,PATINDEX('%SUITE %',PD03L.DX_STR_ADR_2),35)))
		--		END,
		--		CASE
		--			WHEN PATINDEX('%UNIT %',PD03L.DX_STR_ADR_1) > 0 THEN RTRIM(LTRIM(SUBSTRING(PD03L.DX_STR_ADR_1,PATINDEX('%UNIT %',PD03L.DX_STR_ADR_1),35))) --get UNIT first because sometimes APT or suite comes after UNIT and this includes all of it
		--			WHEN RTRIM(LTRIM(SUBSTRING(PD03L.DX_STR_ADR_1,PATINDEX('%APT %',PD03L.DX_STR_ADR_1),35))) = 'APT' THEN '' --weeds out numerous addresses with APT at the end but no apartment number
		--			WHEN PATINDEX('%APT %',PD03L.DX_STR_ADR_1) > 0 THEN RTRIM(LTRIM(SUBSTRING(PD03L.DX_STR_ADR_1,PATINDEX('%APT %',PD03L.DX_STR_ADR_1),35)))
		--			WHEN PATINDEX('% STE %',PD03L.DX_STR_ADR_1) > 0 THEN RTRIM(LTRIM(SUBSTRING(PD03L.DX_STR_ADR_1,PATINDEX('% STE %',PD03L.DX_STR_ADR_1),35)))
		--			WHEN PATINDEX('%SUITE %',PD03L.DX_STR_ADR_1) > 0 THEN RTRIM(LTRIM(SUBSTRING(PD03L.DX_STR_ADR_1,PATINDEX('%SUITE %',PD03L.DX_STR_ADR_1),35)))
		--	 END,
		--	''
		--	) AS [Apartment/suit(1)],
		'' AS [Apartment/suit(1)],
		COALESCE(PD03L.DM_CT,PD03A.DM_CT,PD03T.DM_CT) AS [City(1)],
		COALESCE(PD03L.DC_DOM_ST,PD03A.DC_DOM_ST,PD03T.DC_DOM_ST) AS [State(1)],
		COALESCE(PD03L.DF_ZIP,PD03A.DF_ZIP,PD03T.DF_ZIP) AS [Zip Code(1)],
		CASE
			WHEN PD03L.DX_STR_ADR_1 IS NOT NULL AND PD03A.DX_STR_ADR_1 IS NOT NULL THEN PD03A.DX_STR_ADR_1 --L addr used for addr 1 so use A for addr 2
			WHEN PD03L.DX_STR_ADR_1 IS NULL AND PD03A.DX_STR_ADR_1 IS NOT NULL THEN PD03T.DX_STR_ADR_1 --A addr used for addr 1 so use T for addr 2
			ELSE '' --all available addresses already used
		END AS [Street Address (2)],
		----it was decided to leave this blank
		--COALESCE --PD03AT.DX_STR_AD1/DX_STR_ADR_2 AS [Apartment/suit(2)], --TODO: = There is no direct field for this, would need to parsed from 
		--	(
		--		CASE
		--			WHEN PATINDEX('%UNIT %',PD03AT.DX_STR_ADR_2) > 0 THEN RTRIM(LTRIM(SUBSTRING(PD03AT.DX_STR_ADR_2,PATINDEX('%UNIT %',PD03AT.DX_STR_ADR_2),35))) --get UNIT first because sometimes APT or suite comes after UNIT and this includes all of it
		--			WHEN RTRIM(LTRIM(SUBSTRING(PD03AT.DX_STR_ADR_2,PATINDEX('%APT %',PD03AT.DX_STR_ADR_2),35))) = 'APT' THEN '' --weeds out numerous addresses with APT at the end but no apartment number
		--			WHEN PATINDEX('%APT %',PD03AT.DX_STR_ADR_2) > 0 THEN RTRIM(LTRIM(SUBSTRING(PD03AT.DX_STR_ADR_2,PATINDEX('%APT %',PD03AT.DX_STR_ADR_2),35)))
		--			WHEN PATINDEX('% STE %',PD03AT.DX_STR_ADR_2) > 0 THEN RTRIM(LTRIM(SUBSTRING(PD03AT.DX_STR_ADR_2,PATINDEX('% STE %',PD03AT.DX_STR_ADR_2),35)))
		--			WHEN PATINDEX('%SUITE %',PD03AT.DX_STR_ADR_2) > 0 THEN RTRIM(LTRIM(SUBSTRING(PD03AT.DX_STR_ADR_2,PATINDEX('%SUITE %',PD03AT.DX_STR_ADR_2),35)))
		--		END,
		--		CASE
		--			WHEN PATINDEX('%UNIT %',PD03AT.DX_STR_ADR_1) > 0 THEN RTRIM(LTRIM(SUBSTRING(PD03AT.DX_STR_ADR_1,PATINDEX('%UNIT %',PD03AT.DX_STR_ADR_1),35))) --get UNIT first because sometimes APT or suite comes after UNIT and this includes all of it
		--			WHEN RTRIM(LTRIM(SUBSTRING(PD03AT.DX_STR_ADR_1,PATINDEX('%APT %',PD03AT.DX_STR_ADR_1),35))) = 'APT' THEN '' --weeds out numerous addresses with APT at the end but no apartment number
		--			WHEN PATINDEX('%APT %',PD03AT.DX_STR_ADR_1) > 0 THEN RTRIM(LTRIM(SUBSTRING(PD03AT.DX_STR_ADR_1,PATINDEX('%APT %',PD03AT.DX_STR_ADR_1),35)))
		--			WHEN PATINDEX('% STE %',PD03AT.DX_STR_ADR_1) > 0 THEN RTRIM(LTRIM(SUBSTRING(PD03AT.DX_STR_ADR_1,PATINDEX('% STE %',PD03AT.DX_STR_ADR_1),35)))
		--			WHEN PATINDEX('%SUITE %',PD03AT.DX_STR_ADR_1) > 0 THEN RTRIM(LTRIM(SUBSTRING(PD03AT.DX_STR_ADR_1,PATINDEX('%SUITE %',PD03AT.DX_STR_ADR_1),35)))
		--	 END,
		--	''
		--	) AS [Apartment/suit(2)],
		'' AS [Apartment/suit(2)],
		CASE
			WHEN PD03L.DM_CT IS NOT NULL AND PD03A.DM_CT IS NOT NULL THEN PD03A.DM_CT --L addr used for addr 1 so use A for addr 2
			WHEN PD03L.DM_CT IS NULL AND PD03A.DM_CT IS NOT NULL THEN PD03T.DM_CT --A addr used for addr 1 so use T for addr 2
			ELSE '' --all available addresses already used
		END AS [City(2)],
		CASE
			WHEN PD03L.DC_DOM_ST IS NOT NULL AND PD03A.DC_DOM_ST IS NOT NULL THEN PD03A.DC_DOM_ST
			WHEN PD03L.DC_DOM_ST IS NULL AND PD03A.DC_DOM_ST IS NOT NULL THEN PD03T.DC_DOM_ST
		END AS [State(2)],
		CASE
			WHEN PD03L.DF_ZIP IS NOT NULL AND PD03A.DF_ZIP IS NOT NULL THEN PD03A.DF_ZIP
			WHEN PD03L.DF_ZIP IS NULL AND PD03A.DF_ZIP IS NOT NULL THEN PD03T.DF_ZIP
		END AS [Zip(2)],
		'FFELP Student Loan(s)' AS [Service/Product],
		DC02.LA_CLM_BAL AS [Outstanding Balance], --Field is already summed to outstanding balance with fees and int
		SUM(DC01.LA_CLM_PRI + DC01.LA_CLM_INT - DC01.LA_PRI_COL) OVER(PARTITION BY DC01.AF_APL_ID) AS [Principle Balance],
		 DC02.LA_CLM_INT_ACR AS [Current Interest],	--Field already is summed to outstanding int
		SUM((DC02.LA_CLM_PRJ_COL_CST  + DC01.LA_LEG_CST_ACR + DC01.LA_OTH_CHR_ACR + DC01.LA_COL_CST_ACR) - (DC01.LA_LEG_CST_ACR + DC01.LA_OTH_CHR_COL + DC01.LA_COL_CST_COL)) OVER(PARTITION BY DC01.AF_APL_ID) AS [Current Fees],
		MIN_GA11.AD_DSB_ADJ AS [Origination Date],
		GA14.AD_LON_STA AS [Default Date],
		'' AS [Charge-off Date],
		DC01.LD_LST_BR_PAY AS [Date of Last Payment],
		'' AS [Date of Last Transaction],
		'' AS [Amount due at Charge- of],
		'' AS [Interest accrued since charge of], 
		'' AS [Amount Paid Since Charge-off],
		'' AS [Non- Interest/Fees accrued since charge-off], 
		'' AS [Out of Statute Date],
		'' AS [Date of Obsolesc]
		,
		CASE
			WHEN PD03.DC_CEP = 'P'
				OR PD03.DC_ALT_CEP  = 'P'
				OR PD03.DC_OTH_CEP = 'P'
			THEN 'Mobile phone consent Y'
			ELSE 'Mobile phone consent N'
		END AS [Notes]

	FROM
		ODW..PD01_PDM_INF PD01
		INNER JOIN ODW..PD03_PRS_ADR_PHN PD03 -- used for [Email Address 1], [Phone Number 1: Home Phone], and [Phone Number 2: Mobile Phone] and to only include borrowers with a valid e-mail address TODOv/: does this need to be legal (DC_ADR = 'L')?
			ON PD03.DF_PRS_ID = PD01.DF_PRS_ID
	--Get the max updated DC01 Recorded  --v/TODO
		INNER JOIN 
		(
			SELECT
				DC01M.BF_SSN,
				DC01M.AF_APL_ID,
				DC01M.AF_APL_ID_SFX,
				MAX(DC01M.LF_CRT_DTS_DC10) AS LF_CRT_DTS_DC10
			FROM
				ODW..DC01_LON_CLM_INF DC01M
			WHERE
				DC01M.LC_PCL_REA = 'DF'
				AND DC01M.LC_STA_DC10 = '03'
				AND DC01M.LD_CLM_ASN_DOE IS NULL
				AND DC01M.LC_REA_CLM_ASN_DOE = ''
			GROUP BY
				DC01M.BF_SSN,
				DC01M.AF_APL_ID,
				DC01M.AF_APL_ID_SFX	
		) MAX_DC01
			ON MAX_DC01.BF_SSN = PD01.DF_PRS_ID
		INNER JOIN ODW..DC01_LON_CLM_INF DC01
			ON DC01.BF_SSN = MAX_DC01.BF_SSN
			AND DC01.AF_APL_ID = MAX_DC01.AF_APL_ID
			AND DC01.AF_APL_ID_SFX = MAX_DC01.AF_APL_ID_SFX
			AND DC01.LF_CRT_DTS_DC10 = MAX_DC01.LF_CRT_DTS_DC10
		INNER JOIN ODW..DC02_BAL_INT DC02
			ON DC02.AF_APL_ID = DC01.AF_APL_ID
			AND DC02.AF_APL_ID_SFX = DC01.AF_APL_ID_SFX
		LEFT JOIN ODW..GA14_LON_STA GA14
			ON GA14.AF_APL_ID = DC01.AF_APL_ID
			AND GA14.AF_APL_ID_SFX = DC01.AF_APL_ID_SFX
			AND GA14.AC_STA_GA14 = 'A'
			AND GA14.AC_LON_STA_TYP = 'CP'
			AND GA14.AC_LON_STA_REA = 'DF'
		LEFT JOIN ODW..PD03_PRS_ADR_PHN PD03L
			ON PD03L.DF_PRS_ID = PD01.DF_PRS_ID
			AND PD03L.DC_ADR = 'L'
			AND PD03L.DI_VLD_ADR = 'Y'
		LEFT JOIN ODW..PD03_PRS_ADR_PHN PD03A
			ON PD03A.DF_PRS_ID = PD01.DF_PRS_ID
			AND PD03A.DC_ADR  = 'A'
			AND PD03A.DI_VLD_ADR = 'Y'
		LEFT JOIN ODW..PD03_PRS_ADR_PHN PD03T
			ON PD03L.DF_PRS_ID = PD01.DF_PRS_ID
			AND PD03L.DC_ADR = 'T'
			AND PD03L.DI_VLD_ADR = 'Y'
		LEFT JOIN
	--Origination Date = MIN(GA11.AD_DSB_ADJ)
		(
			SELECT
				GA11.AF_APL_ID,
				GA11.AF_APL_ID_SFX,
				MIN(GA11.AD_DSB_ADJ) AS AD_DSB_ADJ
			FROM
				ODW..GA11_LON_DSB_ATY GA11
			WHERE 
				GA11.AC_DSB_ADJ = 'A'
				AND GA11.AC_DSB_ADJ_STA = 'A'
			GROUP BY
				GA11.AF_APL_ID,
				GA11.AF_APL_ID_SFX
		) MIN_GA11
			ON MIN_GA11.AF_APL_ID = DC01.AF_APL_ID
			AND MIN_GA11.AF_APL_ID_SFX = DC01.AF_APL_ID_SFX
	WHERE
		PD03.DI_EML_ADR_VAL = 'Y' --only include borrowers with a valid e-mail address
		AND DC01.LC_PCL_REA = 'DF'
		AND DC01.LC_STA_DC10 = '03'
		AND DC01.LD_CLM_ASN_DOE IS NULL
		AND DC01.LC_REA_CLM_ASN_DOE = ''
		AND GA14.AC_STA_GA14 = 'A'
		AND GA14.AC_LON_STA_TYP = 'CP'
		AND GA14.AC_LON_STA_REA = 'DF'
) POP
WHERE
	POP.LOANS = 1