USE UDW
GO

DECLARE @Today DATE = GETDATE();
DECLARE @HtmlId INT = (SELECT HTMLFileId FROM ULS.emailbatch.HTMLFiles WHERE HTMLFile = 'NTDSFBEXUH.html');
DECLARE @EmailCampaignId INT = (SELECT EmailCampaignId FROM ULS.emailbatch.EmailCampaigns WHERE HTMLFileId = @HtmlId);

INSERT INTO ULS.emailbatch.EmailProcessing(EmailCampaignId, AccountNumber, EmailData, ArcNeeded, AddedAt, AddedBy)
SELECT DISTINCT
	@EmailCampaignId AS EmailCampaignId,
	PD10.DF_SPE_ACC_ID AS AccountNumber,
	CONCAT(PD10.DF_SPE_ACC_ID, ',', CentralData.dbo.PascalCase(PD10.DM_PRS_1), ',', ISNULL(PH05.DX_CNC_EML_ADR, PD32.DX_ADR_EML)) AS EmailData,
	1 AS ArcNeeded,
	@Today AS AddedAt,
	SUSER_SNAME() AS AddedBy
FROM
	UDW..PD10_PRS_NME PD10
	INNER JOIN UDW..LN10_LON LN10
		ON LN10.BF_SSN = PD10.DF_PRS_ID
		AND LN10.LC_STA_LON10 = 'R'
		AND LN10.LA_CUR_PRI > 0.00
	INNER JOIN UDW..LN60_BR_FOR_APV LN60
		ON LN60.BF_SSN = LN10.BF_SSN
		AND LN60.LN_SEQ = LN10.LN_SEQ
		AND LN60.LC_FOR_RSP IN('000','001')
		AND LN60.LC_STA_LON60 = 'A'
		AND CAST(LN60.LD_FOR_END AS DATE) BETWEEN @Today AND CAST(DATEADD(DAY,7,@Today) AS DATE)
	INNER JOIN UDW..FB10_BR_FOR_REQ FB10
		ON FB10.BF_SSN = LN60.BF_SSN
		AND FB10.LF_FOR_CTL_NUM = LN60.LF_FOR_CTL_NUM
		AND FB10.LC_STA_FOR10 = 'A'
		AND FB10.LC_FOR_STA = 'A'
		AND FB10.LC_FOR_TYP IN ('40') 
	LEFT JOIN UDW..PH05_CNC_EML PH05
		ON PH05.DF_SPE_ID = PD10.DF_SPE_ACC_ID
		AND PH05.DI_CNC_ELT_OPI = 'Y'
		AND PH05.DI_VLD_CNC_EML_ADR = 'Y'
	LEFT JOIN 
	(
		SELECT
			Email.*,
			ROW_NUMBER() OVER (PARTITION BY Email.DF_PRS_ID ORDER BY Email.PriorityNumber) AS EmailPriority -- number in order of Email.PriorityNumber
			FROM
			(
				SELECT
					PD32.DF_PRS_ID,
					PD32.DX_ADR_EML,
					CASE 
						WHEN DC_ADR_EML = 'H' THEN 1 -- home
						WHEN DC_ADR_EML = 'A' THEN 2 -- alternate
						WHEN DC_ADR_EML = 'W' THEN 3 -- work
					END AS PriorityNumber
				FROM
					UDW..PD32_PRS_ADR_EML PD32
					INNER JOIN UDW..LN10_LON LN10
						ON LN10.BF_SSN = PD32.DF_PRS_ID
				WHERE
					PD32.DI_VLD_ADR_EML = 'Y' -- valid email address
					AND PD32.DC_STA_PD32 = 'A' -- active email address record
			) Email
	) PD32 
		ON PD32.DF_PRS_ID = PD10.DF_PRS_ID
		AND PD32.EmailPriority = 1 --highest priority email only
	LEFT JOIN ULS.emailbatch.EmailProcessing EP
		ON EP.AccountNumber = PD10.DF_SPE_ACC_ID
		AND EP.EmailCampaignId = @EmailCampaignId
		AND CAST(EP.AddedAt AS DATE) >= CAST(DATEADD(DAY,-7,@Today) AS DATE) --Dont allow record to have been added in last 7 days
WHERE
	EP.AccountNumber IS NULL --No record in past 7 days
	AND COALESCE(PH05.DX_CNC_EML_ADR, PD32.DX_ADR_EML, '') != '' --Has an email
		