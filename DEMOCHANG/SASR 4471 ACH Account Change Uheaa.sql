USE ULS
GO

DECLARE @PREVIOUSWORKDAY DATE = 
(--if run on Monday then do Saturday, else do yesterday
	SELECT
		CASE
			WHEN DATENAME(WEEKDAY,GETDATE()) = 'Monday'
			THEN DATEADD(DAY,-3,GETDATE())
			ELSE DATEADD(DAY,-1,GETDATE())
		END
);

DECLARE @YESTERDAY DATE = DATEADD(DAY,-1,GETDATE());

DECLARE @Emails TABLE(DF_PRS_ID CHAR(9), DX_ADR_EML VARCHAR(254), DF_LST_DTS_PD32 DATETIME, DM_PRS_1 VARCHAR(13), DM_PRS_LST VARCHAR(23), EmailPriority INT)
INSERT INTO @Emails(DF_PRS_ID, DX_ADR_EML, DF_LST_DTS_PD32, DM_PRS_1, DM_PRS_LST, EmailPriority)
SELECT
	EMAIL.DF_PRS_ID,
	EMAIL.DX_ADR_EML,
	EMAIL.DF_LST_DTS_PD32,
	EMAIL.DM_PRS_1,
	EMAIL.DM_PRS_LST,
	ROW_NUMBER() OVER (PARTITION BY Email.DF_PRS_ID ORDER BY Email.PriorityNumber, Email.DF_LST_DTS_PD32 DESC) AS EmailPriority -- number in order of Email.PriorityNumber
	FROM
	(
		SELECT
			PD32.DF_PRS_ID,
			PD32.DX_ADR_EML,
			CASE 
				WHEN DC_ADR_EML = 'H' THEN 1 -- home
				WHEN DC_ADR_EML = 'A' THEN 2 -- alternate
				WHEN DC_ADR_EML = 'W' THEN 3 -- work
			END AS PriorityNumber,
			PD32.DF_LST_DTS_PD32,
			PD10.DM_PRS_1,
			PD10.DM_PRS_LST
		FROM
			UDW..PD32_PRS_ADR_EML PD32
			INNER JOIN UDW..PD10_PRS_NME PD10
				ON PD10.DF_PRS_ID = PD32.DF_PRS_ID
		WHERE
			PD32.DI_VLD_ADR_EML = 'Y' -- valid email address
			AND PD32.DC_STA_PD32 = 'A' -- active email address record
			AND PD32.DF_PRS_ID NOT LIKE 'P%'
	) Email

DECLARE @EmailCampaignId INT = (SELECT EC.EmailCampaignId FROM ULS.emailbatch.EmailCampaigns EC INNER JOIN ULS.emailbatch.HTMLFiles H ON H.HTMLFileId = EC.HTMLFileId WHERE H.HTMLFile = 'DEMUPDNUH.html') 

INSERT INTO ULS.emailbatch.EmailProcessing(EmailCampaignId, AccountNumber, ActualFile, EmailData, ArcNeeded, ProcessingAttempts, AddedAt, AddedBy)
SELECT DISTINCT
	@EmailCampaignId AS EmailCampaignId,
	PD10.DF_SPE_ACC_ID AS AccountNumber,
	NULL AS ActualFile,
	CAST(PD10.DF_SPE_ACC_ID AS VARCHAR(10)) + ',' 
		+ COALESCE(LTRIM(RTRIM(PD10.DM_PRS_1)),'') + ','
		+ COALESCE(PH05.DX_CNC_EML_ADR, PD32.DX_ADR_EML) AS EmailData,
	CASE WHEN EA.Arc IS NOT NULL THEN 1 ELSE 0 END AS ArcNeeded,
	0 AS ProcessingAttempts,
	GETDATE() AS AddedAt,
	SUSER_NAME() AS AddedBy
FROM
	UDW..PD10_PRS_NME PD10
	INNER JOIN UDW..LN10_LON LN10
		ON LN10.BF_SSN = PD10.DF_PRS_ID
		AND LN10.LC_STA_LON10 = 'R'
		AND LN10.LA_CUR_PRI > 0.00
	INNER JOIN UDW..LN83_EFT_TO_LON LN83
		ON LN83.BF_SSN = PD10.DF_PRS_ID
		AND LN83.LC_STA_LN83 = 'A'
	INNER JOIN UDW..BR30_BR_EFT BR30
		ON BR30.BF_SSN = LN83.BF_SSN
		AND BR30.BN_EFT_SEQ = LN83.BN_EFT_SEQ
		AND BR30.BC_EFT_STA = 'A'
	INNER JOIN UDW..BR30_BR_EFT History
		ON History.BF_SSN = BR30.BF_SSN
		AND History.BC_EFT_STA != 'A'
		AND 
		(
			History.BF_EFT_ABA != BR30.BF_EFT_ABA
			OR History.BF_EFT_ACC != BR30.BF_EFT_ACC
		)
		AND CAST(History.BF_LST_DTS_BR30 AS DATE) BETWEEN @PREVIOUSWORKDAY AND @YESTERDAY
	LEFT JOIN UDW..PH05_CNC_EML PH05
		ON PH05.DF_SPE_ID = PD10.DF_SPE_ACC_ID
		AND PH05.DI_VLD_CNC_EML_ADR = 'Y'
	LEFT JOIN @Emails PD32
		ON PD32.DF_PRS_ID = PD10.DF_PRS_ID
		AND PD32.EmailPriority = 1
	LEFT JOIN ULS.emailbatch.EmailProcessing ExistingRequest
		ON ExistingRequest.EmailCampaignId = @EmailCampaignId
		AND ExistingRequest.AccountNumber = PD10.DF_SPE_ACC_ID
		AND ExistingRequest.EmailData = CAST(PD10.DF_SPE_ACC_ID AS VARCHAR(10)) + ',' 
										+ COALESCE(LTRIM(RTRIM(PD10.DM_PRS_1)),'') + ','
										+ COALESCE(PH05.DX_CNC_EML_ADR, PD32.DX_ADR_EML)
		AND CAST(ExistingRequest.AddedAt AS DATE) = CAST(GETDATE() AS DATE)
	LEFT JOIN ULS.emailbatch.EmailCampaigns EC
		ON EC.EmailCampaignId = @EmailCampaignId
		AND EC.DeletedAt IS NULL
	LEFT JOIN ULS.emailbatch.Arcs EA
		ON EA.ArcId = EC.ArcId
WHERE
	ExistingRequest.AccountNumber IS NULL --Wasnt already added today.
	AND CAST(BR30.BF_LST_DTS_BR30 AS DATE) BETWEEN @PREVIOUSWORKDAY AND @YESTERDAY
	AND COALESCE(PH05.DX_CNC_EML_ADR, PD32.DX_ADR_EML) IS NOT NULL
	AND PD10.DF_PRS_ID NOT LIKE 'P%'

UNION ALL

SELECT DISTINCT
	@EmailCampaignId AS EmailCampaignId,
	PD10.DF_SPE_ACC_ID AS AccountNumber,
	NULL AS ActualFile,
	CAST(PD10E.DF_SPE_ACC_ID AS VARCHAR(10)) + ',' 
		+ COALESCE(LTRIM(RTRIM(PD10E.DM_PRS_1)),'') + ','
		+ COALESCE(PH05.DX_CNC_EML_ADR, PD32.DX_ADR_EML) AS EmailData,
	CASE WHEN EA.Arc IS NOT NULL THEN 1 ELSE 0 END AS ArcNeeded,
	0 AS ProcessingAttempts,
	GETDATE() AS AddedAt,
	SUSER_NAME() AS AddedBy
FROM
	UDW..PD10_PRS_NME PD10
	INNER JOIN UDW..LN10_LON LN10
		ON LN10.BF_SSN = PD10.DF_PRS_ID
		AND LN10.LC_STA_LON10 = 'R'
		AND LN10.LA_CUR_PRI > 0.00
	INNER JOIN UDW..LN20_EDS LN20
		ON LN20.BF_SSN = LN10.BF_SSN
		AND LN20.LN_SEQ = LN10.LN_SEQ
		AND LN20.LC_EDS_TYP = 'M'
		AND LN20.LC_STA_LON20 = 'A'
	INNER JOIN UDW..PD10_PRS_NME PD10E
		ON PD10E.DF_PRS_ID = LN20.LF_EDS
	INNER JOIN UDW..LN83_EFT_TO_LON LN83
		ON LN83.BF_SSN = PD10.DF_PRS_ID
		AND LN83.LC_STA_LN83 = 'A'
	INNER JOIN UDW..BR30_BR_EFT BR30
		ON BR30.BF_SSN = LN83.BF_SSN
		AND BR30.BN_EFT_SEQ = LN83.BN_EFT_SEQ
		AND BR30.BC_EFT_STA = 'A'
	INNER JOIN UDW..BR30_BR_EFT History
		ON History.BF_SSN = BR30.BF_SSN
		AND LTRIM(RTRIM(History.BF_DDT_PAY_EDS)) = PD10E.DF_PRS_ID
		AND History.BC_DDT_PAY_PRS_TYP = 'E'
		AND History.BC_EFT_STA != 'A'
		AND 
		(
			History.BF_EFT_ABA != BR30.BF_EFT_ABA
			OR History.BF_EFT_ACC != BR30.BF_EFT_ACC
		)
		AND CAST(History.BF_LST_DTS_BR30 AS DATE) BETWEEN @PREVIOUSWORKDAY AND @YESTERDAY
	LEFT JOIN UDW..PH05_CNC_EML PH05
		ON PH05.DF_SPE_ID = PD10E.DF_SPE_ACC_ID
		AND PH05.DI_VLD_CNC_EML_ADR = 'Y'
	LEFT JOIN @Emails PD32
		ON PD32.DF_PRS_ID = PD10E.DF_PRS_ID
		AND PD32.EmailPriority = 1
	LEFT JOIN ULS.emailbatch.EmailProcessing ExistingRequest
		ON ExistingRequest.EmailCampaignId = @EmailCampaignId
		AND ExistingRequest.AccountNumber = PD10.DF_SPE_ACC_ID
		AND ExistingRequest.EmailData = CAST(PD10E.DF_SPE_ACC_ID AS VARCHAR(10)) + ',' 
										+ COALESCE(LTRIM(RTRIM(PD10E.DM_PRS_1)),'') + ','
										+ COALESCE(PH05.DX_CNC_EML_ADR, PD32.DX_ADR_EML)
		AND CAST(ExistingRequest.AddedAt AS DATE) = CAST(GETDATE() AS DATE)
	LEFT JOIN ULS.emailbatch.EmailCampaigns EC
		ON EC.EmailCampaignId = @EmailCampaignId
		AND EC.DeletedAt IS NULL
	LEFT JOIN ULS.emailbatch.Arcs EA
		ON EA.ArcId = EC.ArcId
WHERE
	ExistingRequest.AccountNumber IS NULL --Wasnt already added today.
	AND CAST(BR30.BF_LST_DTS_BR30 AS DATE) BETWEEN @PREVIOUSWORKDAY AND @YESTERDAY
	AND COALESCE(PH05.DX_CNC_EML_ADR, PD32.DX_ADR_EML) IS NOT NULL
	AND PD10E.DF_PRS_ID NOT LIKE 'P%'