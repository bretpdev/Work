--BDAYEMLFD
--Select all CornerStone Borrowers and Co-Borrowers with an active balance where it is their birthday and they have a valid email address. 

DECLARE @CREATED_AT AS DATE = CONVERT(DATE,GETDATE())
DECLARE @THIS_DAY AS INT = DAY(GETDATE())
DECLARE @THIS_MONTH AS INT = MONTH(GETDATE())
DECLARE @SASID AS VarChar(10) = 'BDAYEMLFD'
DECLARE @TODAY DATE = GETDATE()
DECLARE @EmailCampaignId INT = (SELECT EmailCampaignId FROM CLS.emailbtcf.EmailCampaigns WHERE letterId = 'BRTHDYFED.html')

--borrowers
INSERT INTO CLS.emailbtcf.CampaignData(EmailCampaignId,AccountNumber,Recipient,FirstName,LastName,AddedAt,AddedBy,EmailSentAt,ArcProcessedAt,ArcAddProcessingId,InactivatedAt)
	SELECT DISTINCT
		@EmailCampaignId AS EmailCampaignId,
		PD10.DF_SPE_ACC_ID AS AccountNumber,
		COALESCE(PH05.DX_CNC_EML_ADR, PD32.ALT_EM) AS Recipient,
		SUBSTRING(PD10.DM_PRS_1,1,1) + RTRIM(LOWER(SUBSTRING(PD10.DM_PRS_1,2,13))) AS FirstName,
		'' AS LastName,
		@CREATED_AT AS AddedAt,	--CurrentDate
		@SASID AS AddedBy,				--SASID
		NULL AS EmailSentAt,
		NULL AS ArcProcessedAt,
		NULL AS ArcAddProcessingId,
		NULL AS InactivatedAt

	FROM
		CDW..LN10_LON LN10
		INNER JOIN CDW..PD10_PRS_NME PD10
			ON LN10.BF_SSN = PD10.DF_PRS_ID
		LEFT JOIN CDW..PH05_CNC_EML PH05 
			ON PH05.DF_SPE_ID = PD10.DF_SPE_ACC_ID --join to coborrower PD10 for coborrower email
			AND PH05.DI_VLD_CNC_EML_ADR = 'Y' -- valid email
		LEFT JOIN
			( -- email address
				SELECT 
					DF_PRS_ID, 
					Email.EM AS [ALT_EM],
					ROW_NUMBER() OVER (PARTITION BY Email.DF_PRS_ID ORDER BY Email.PriorityNumber) AS [EmailPriority] -- number in order of Email.PriorityNumber 
				FROM 
					( 
						SELECT 
							PD32.DF_PRS_ID,
							PD32.DX_ADR_EML AS [EM],
							CASE     
								WHEN DC_ADR_EML = 'H' THEN 1 -- home
								WHEN DC_ADR_EML = 'A' THEN 2 -- alternate
								WHEN DC_ADR_EML = 'W' THEN 3 -- work 
							END AS PriorityNumber
                        FROM 
							CDW..PD32_PRS_ADR_EML PD32 
						WHERE 
							PD32.DI_VLD_ADR_EML = 'Y' -- valid email address 
							AND PD32.DC_STA_PD32 = 'A' -- active email address record 
					) Email 
			) PD32 
				ON PD32.DF_PRS_ID = PD10.DF_PRS_ID
				AND PD32.EmailPriority = 1 --sends only to highest priority email
		LEFT JOIN CLS.emailbtcf.CampaignData EXISTING_DATA --flag to exclude existing data added today
			ON EXISTING_DATA.EmailCampaignId = @EmailCampaignId
			AND EXISTING_DATA.Recipient = COALESCE(PH05.DX_CNC_EML_ADR, PD32.ALT_EM)
			AND EXISTING_DATA.AccountNumber = PD10.DF_SPE_ACC_ID
			AND EXISTING_DATA.FirstName = SUBSTRING(PD10.DM_PRS_1,1,1) + RTRIM(LOWER(SUBSTRING(PD10.DM_PRS_1,2,13)))
			AND CAST(EXISTING_DATA.AddedAt AS DATE) =  @TODAY
	WHERE 
		LN10.LC_STA_LON10 = 'R'
		AND LN10.LA_CUR_PRI > 0.00
		AND MONTH(PD10.DD_BRT) = @THIS_MONTH
		AND DAY(PD10.DD_BRT) = @THIS_DAY
		AND COALESCE(PH05.DX_CNC_EML_ADR, PD32.ALT_EM) IS NOT NULL --Active email in PD32 or PH05
		AND EXISTING_DATA.AccountNumber IS NULL --wasn't already added today

		
--coborrowers
INSERT INTO CLS.emailbtcf.CampaignData(EmailCampaignId,AccountNumber,Recipient,FirstName,LastName,AddedAt,AddedBy,EmailSentAt,ArcProcessedAt,ArcAddProcessingId,InactivatedAt)
	SELECT DISTINCT
		@EmailCampaignId AS EmailCampaignId,
		PD10B.DF_SPE_ACC_ID AS AccountNumber, --borrower account number
		COALESCE(PH05.DX_CNC_EML_ADR, PD32.ALT_EM) AS Recipient,
		SUBSTRING(PD10.DM_PRS_1,1,1) + RTRIM(LOWER(SUBSTRING(PD10.DM_PRS_1,2,13))) AS FirstName, --coborrower name
		'' AS LastName,
		@CREATED_AT AS AddedAt,	--CurrentDate
		@SASID AS AddedBy,				--SASID
		NULL AS EmailSentAt,
		NULL AS ArcProcessedAt,
		NULL AS ArcAddProcessingId,
		NULL AS InactivatedAt

	FROM
		CDW..LN10_LON LN10
		INNER JOIN CDW..LN20_EDS LN20
			ON LN10.BF_SSN = LN20.BF_SSN
			AND LN10.LN_SEQ = LN20.LN_SEQ
			AND LN20.LC_EDS_TYP = 'M'
			AND LN20.LC_STA_LON20 = 'A'
		INNER JOIN CDW..PD10_PRS_NME PD10 --join to coborrower PD10 for coborrower name
			ON LN20.LF_EDS = PD10.DF_PRS_ID
		INNER JOIN CDW..PD10_PRS_NME PD10B --join to Borrower PD10 for borrower account number
			ON LN10.BF_SSN = PD10B.DF_PRS_ID
		LEFT JOIN CDW..PH05_CNC_EML PH05 
			ON PH05.DF_SPE_ID = PD10.DF_SPE_ACC_ID --join to coborrower PD10 for coborrower email
			AND PH05.DI_VLD_CNC_EML_ADR = 'Y' -- valid email
		LEFT JOIN
			( -- email address
				SELECT 
					DF_PRS_ID, 
					Email.EM AS [ALT_EM],
					ROW_NUMBER() OVER (PARTITION BY Email.DF_PRS_ID ORDER BY Email.PriorityNumber) AS [EmailPriority] -- number in order of Email.PriorityNumber 
				FROM 
					( 
						SELECT 
							PD32.DF_PRS_ID,
							PD32.DX_ADR_EML AS [EM],
							CASE     
								WHEN DC_ADR_EML = 'H' THEN 1 -- home
								WHEN DC_ADR_EML = 'A' THEN 2 -- alternate
								WHEN DC_ADR_EML = 'W' THEN 3 -- work 
							END AS PriorityNumber
                        FROM 
							CDW..PD32_PRS_ADR_EML PD32 
						WHERE 
							PD32.DI_VLD_ADR_EML = 'Y' -- valid email address 
							AND PD32.DC_STA_PD32 = 'A' -- active email address record 
					) Email 
			) PD32 
				ON PD32.DF_PRS_ID = PD10.DF_PRS_ID
				AND PD32.EmailPriority = 1 --sends only to highest priority email
		LEFT JOIN CLS.emailbtcf.CampaignData EXISTING_DATA --flag to exclude existing data added today
			ON EXISTING_DATA.EmailCampaignId = @EmailCampaignId
			AND EXISTING_DATA.Recipient = COALESCE(PH05.DX_CNC_EML_ADR, PD32.ALT_EM)
			AND EXISTING_DATA.AccountNumber = PD10B.DF_SPE_ACC_ID
			AND EXISTING_DATA.FirstName = SUBSTRING(PD10.DM_PRS_1,1,1) + RTRIM(LOWER(SUBSTRING(PD10.DM_PRS_1,2,13)))
			AND CAST(EXISTING_DATA.AddedAt AS DATE) =  @TODAY
	WHERE 
		LN10.LC_STA_LON10 = 'R'
		AND LN10.LA_CUR_PRI > 0.00
		AND MONTH(PD10.DD_BRT) = @THIS_MONTH
		AND DAY(PD10.DD_BRT) = @THIS_DAY
		AND COALESCE(PH05.DX_CNC_EML_ADR, PD32.ALT_EM) IS NOT NULL --Active email in PD32 or PH05
		AND EXISTING_DATA.AccountNumber IS NULL --wasn't already added today
