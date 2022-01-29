USE ODW 
GO
DROP TABLE IF EXISTS #LETTER_POP
DROP TABLE IF EXISTS #EMAIL_POP
DROP TABLE IF EXISTS #ALL_POP
DECLARE @LETTERID VARCHAR(10) = 'COVIDPOP3'
DECLARE @SCRIPTDATAID INT = 
(
SELECT
	SD.ScriptDataId
FROM
	ULS.[print].ScriptData SD
	INNER JOIN ULS.[print].Letters L
		ON L.LetterId = SD.LetterId
WHERE
	L.Letter = @LETTERID
)
DECLARE @EMAILID VARCHAR(10) = 'COVIDEML3'
DECLARE @EMAIL_CAMPAIGN INT =
(
SELECT 
	EC.EmailCampaignId
FROM
	ULS.emailbatch.EmailCampaigns EC
	INNER JOIN ULS.emailbatch.HTMLFiles HT
		ON HT.HTMLFileId = EC.HTMLFileId
WHERE
	REPLACE(HT.HTMLFile,'.HTML','') = @EMAILID 
)
--SELECT @EMAIL_CAMPAIGN

SELECT DISTINCT
	CentralData.dbo.CreateACSKeyline(PD01.DF_PRS_ID, 'B', 'L') AS ACS,
	PD01.DF_SPE_ACC_ID AS AccountNumber,
	RTRIM(PD01.DM_PRS_1) AS FirstName,
	RTRIM(PD01.DM_PRS_LST) AS LastName,
	REPLACE(RTRIM(PD03.DX_STR_ADR_1),',','') AS Address1,
	REPLACE(RTRIM(PD03.DX_STR_ADR_2),',','')  AS Address2,
	REPLACE(RTRIM(PD03.DM_CT),',','')  AS City,
	REPLACE(RTRIM(PD03.DC_DOM_ST),',','')  AS [State],
	REPLACE(RTRIM(PD03.DF_ZIP),',','')  AS Zip,
	REPLACE(RTRIM(PD03.DM_FGN_CNY),',','')  AS Country
into #letter_pop
FROM
	ODW..DC01_LON_CLM_INF DC01
	INNER JOIN ODW..PD01_PDM_INF PD01
		ON PD01.DF_PRS_ID = DC01.BF_SSN
	--INNER JOIN
	--(
	--	SELECT
	--		AF_APL_ID,
	--		AF_APL_ID_SFX,
	--		MAX(LF_CRT_DTS_DC10) AS LF_CRT_DTS_DC10
	--	FROM
	--		ODW..DC01_LON_CLM_INF
	--	GROUP BY
	--		AF_APL_ID,
	--		AF_APL_ID_SFX
	--) DC01M
	--	ON DC01M.AF_APL_ID = DC01.AF_APL_ID
	--	AND DC01M.AF_APL_ID_SFX = DC01.AF_APL_ID_SFX
	--	AND DC01M.LF_CRT_DTS_DC10 = DC01.LF_CRT_DTS_DC10
	--INNER JOIN ODW..DC02_BAL_INT DC02
	--	ON DC02.AF_APL_ID = DC01.AF_APL_ID
	--	AND DC02.AF_APL_ID_SFX = DC01.AF_APL_ID_SFX
	--	AND DC02.LF_CRT_DTS_DC10 = DC01.LF_CRT_DTS_DC10
	INNER JOIN ODW..PD03_PRS_ADR_PHN PD03
		ON PD03.DF_PRS_ID = DC01.BF_SSN
		AND PD03.DC_ADR = 'L'
		AND PD03.DI_VLD_ADR = 'Y'
	LEFT JOIN 
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
WHERE
		DC01.LC_STA_DC10 = '04'
		AND RankedEmail.DF_PRS_ID IS NULL
	--AND DC02.LA_CLM_BAL > 0.00
	AND DC01.LC_AUX_STA IN ('10','12')
	AND DC01.LD_STA_UPD_DC10 between '03/13/2020' and '05/24/2021'
	AND DC01.LC_PCL_REA IN ('DF','DB')
	--AND DC01.LD_CLM_ASN_DOE IS NULL
	--AND LTRIM(ISNULL(DC01.LC_AUX_STA,'')) = ''
	--AND ISNULL(DC01.LC_REA_CLM_ASN_DOE,'') = ''

--,[AccountNumber] ,[EmailAddress],[ScriptDataId] ,[SourceFile]
--      ,[LetterData],[CostCenter] ,[InValidAddress],[DoNotProcessEcorr]
--      ,[OnEcorr] ,[ArcAddProcessingId] ,[ArcNeeded] ,[ImagedAt]
--      ,[ImagingNeeded]  ,[EcorrDocumentCreatedAt] ,[PrintedAt]  ,[AddedBy]
--      ,[AddedAt] ,[DeletedAt] ,[DeletedBy]
--Keyline,AccountNumber,FirstName,LastName,Address1,Address2,City,State,Zip,Country
insert into uls.[print].PrintProcessing
SELECT
	AccountNumber,
	'UHEAA@UHEAA.ORG',
	@SCRIPTDATAID,
	'' AS SOURCE_FILE,
	ACS + ',' + AccountNumber +',' +FirstName + ',' + LastName + ',' + Address1 + ',' + Address2+ ',' + City+ ',' + [State]+ ',' + Zip+ ',' + Country AS LETTER_DATA,
	'MA2329' AS COST_CENTER,
	0 AS ISiNVALIDADDRESS,
	1 AS DONOTPROCESSECORR,
	0 AS ONECORR,
	NULL AS APP,
	1 as arcneeded,
	NULL as imagedat,
	0 as imageneeded,
	NULL AS ECORRCREATED,
	NULL AS PRINTED,
	'UNH 71969' AS ADDEDBY,
	GETDATE() AS ADDEDAT,
	NULL,
	NULL
FROM
	#letter_pop

--SELECT DISTINCT
--	PD01.DF_SPE_ACC_ID,
--	rtrim(PD01.DM_PRS_1) as firstName,
--	rtrim(PD01.DM_PRS_LST) as lastName,
--	RankedEmail.DX_EML_ADR
--into #email_pop
--FROM
--	ODW..DC01_LON_CLM_INF DC01
--	INNER JOIN ODW..PD01_PDM_INF PD01
--		ON PD01.DF_PRS_ID = DC01.BF_SSN
--	--INNER JOIN
--	--(
--	--	SELECT
--	--		AF_APL_ID,
--	--		AF_APL_ID_SFX,
--	--		MAX(LF_CRT_DTS_DC10) AS LF_CRT_DTS_DC10
--	--	FROM
--	--		ODW..DC01_LON_CLM_INF
--	--	GROUP BY
--	--		AF_APL_ID,
--	--		AF_APL_ID_SFX
--	--) DC01M
--	--	ON DC01M.AF_APL_ID = DC01.AF_APL_ID
--	--	AND DC01M.AF_APL_ID_SFX = DC01.AF_APL_ID_SFX
--	--	AND DC01M.LF_CRT_DTS_DC10 = DC01.LF_CRT_DTS_DC10
--	--INNER JOIN ODW..DC02_BAL_INT DC02
--	--	ON DC02.AF_APL_ID = DC01.AF_APL_ID
--	--	AND DC02.AF_APL_ID_SFX = DC01.AF_APL_ID_SFX
--	--	AND DC02.LF_CRT_DTS_DC10 = DC01.LF_CRT_DTS_DC10
--	INNER JOIN 
--	(
--		SELECT
--			Email.DF_PRS_ID,
--			Email.DX_EML_ADR,
--			RANK() OVER(PARTITION BY Email.DF_PRS_ID ORDER BY EmailRank) AS EmailPriority
--		FROM
--		(
--			SELECT
--				PD03.DF_PRS_ID,
--				PD03.DX_EML_ADR,
--				CASE WHEN PD03.DC_ADR = 'L' THEN 1
--					 WHEN PD03.DC_ADR = 'T' THEN 2
--					 WHEN PD03.DC_ADR = 'A' THEN 3
--					 WHEN PD03.DC_ADR = 'I' THEN 4
--					 ELSE 5
--				END AS EmailRank
--			FROM
--				ODW..PD03_PRS_ADR_PHN PD03
--			WHERE
--				ISNULL(PD03.DX_EML_ADR,'') != ''
--				AND PD03.DI_EML_ADR_VAL = 'Y' --Valid email
--		) Email
--	)RankedEmail
--		ON RankedEmail.DF_PRS_ID = PD01.DF_PRS_ID
--		AND RankedEmail.EmailPriority = 1 --Most important
--WHERE
--	DC01.LC_STA_DC10 = '04'
--	--AND DC02.LA_CLM_BAL > 0.00
--	AND DC01.LC_AUX_STA IN ('10','12')
--	AND DC01.LD_STA_UPD_DC10 between '03/13/2020' and '05/24/2021'
--	AND DC01.LC_PCL_REA IN ('DF','DB')
--	--AND DC01.LD_CLM_ASN_DOE IS NULL
--	--AND LTRIM(ISNULL(DC01.LC_AUX_STA,'')) = ''
--	--AND ISNULL(DC01.LC_REA_CLM_ASN_DOE,'') = ''


----,[EmailCampaignId] ,[AccountNumber]
----      ,[ActualFile]  ,[EmailData]
----      ,[EmailSentAt] ,[ArcNeeded]
----      ,[ArcAddProcessingId]   ,[ProcessingAttempts]
----      ,[AddedBy] ,[AddedAt]
----      ,[DeletedBy]  ,[DeletedAt]
--SELECT DISTINCT
--	PD01.DF_SPE_ACC_ID,
--	rtrim(PD01.DM_PRS_1) as firstName,
--	rtrim(PD01.DM_PRS_LST) as lastName
--into #all_pop
--FROM
--	ODW..DC01_LON_CLM_INF DC01
--	INNER JOIN ODW..PD01_PDM_INF PD01
--		ON PD01.DF_PRS_ID = DC01.BF_SSN
--	--INNER JOIN
--	--(
--	--	SELECT
--	--		AF_APL_ID,
--	--		AF_APL_ID_SFX,
--	--		MAX(LF_CRT_DTS_DC10) AS LF_CRT_DTS_DC10
--	--	FROM
--	--		ODW..DC01_LON_CLM_INF
--	--	GROUP BY
--	--		AF_APL_ID,
--	--		AF_APL_ID_SFX
--	--) DC01M
--	--	ON DC01M.AF_APL_ID = DC01.AF_APL_ID
--	--	AND DC01M.AF_APL_ID_SFX = DC01.AF_APL_ID_SFX
--	--	AND DC01M.LF_CRT_DTS_DC10 = DC01.LF_CRT_DTS_DC10
--	--INNER JOIN ODW..DC02_BAL_INT DC02
--	--	ON DC02.AF_APL_ID = DC01.AF_APL_ID
--	--	AND DC02.AF_APL_ID_SFX = DC01.AF_APL_ID_SFX
--	--	AND DC02.LF_CRT_DTS_DC10 = DC01.LF_CRT_DTS_DC10

--WHERE
--	DC01.LC_STA_DC10 = '04'
--	--AND DC02.LA_CLM_BAL > 0.00
--	AND DC01.LC_AUX_STA IN ('10','12')
--	AND DC01.LD_STA_UPD_DC10 between '03/13/2020' and '05/24/2021'
--	AND DC01.LC_PCL_REA IN ('DF','DB')
--	--AND DC01.LD_CLM_ASN_DOE IS NULL
--	--AND LTRIM(ISNULL(DC01.LC_AUX_STA,'')) = ''
--	--AND ISNULL(DC01.LC_REA_CLM_ASN_DOE,'') = ''


--select 
--	@EMAIL_CAMPAIGN,
--	DF_SPE_ACC_ID,
--	DF_SPE_ACC_ID + ',' +  firstName + ' ' + lastName + ',' + DX_EML_ADR,
--	null as emailSent,
--	1 as arcneeded,
--	null as aap, 
--	0 as processingAttempts,
--	'unh' as addedby,
--	getdate() as addedat,
--	null,
--	null
--from 
--	#email_pop


--select * from #letter_pop
--select * from #email_pop
--select 
--*
--from
--	#all_pop ap
--	left join
--	(
--		select
--			AccountNumber
--		from
--			#letter_pop
--		union all

--		select
--			df_spe_acc_id
--		from
--			#email_pop
--	) p
--		on p.AccountNumber = ap.DF_SPE_ACC_ID
--where p.AccountNumber is null

