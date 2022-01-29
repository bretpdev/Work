USE [CDW]
GO

DECLARE @CurrentDate DATETIME = GETDATE()
DECLARE @DateName VARCHAR(20) = (SELECT DATENAME(DW,GETDATE())) 
DECLARE @EmailCampaign VARCHAR(20) = '10DLACHFED.html' --Letter Id For Email Campaigns Table
DECLARE @SasId VARCHAR(20) = '10DELEMFD'

DECLARE @EmailCampaignId INT = (SELECT EmailCampaignId FROM CLS.emailbtcf.EmailCampaigns WHERE letterId = @EmailCampaign)

INSERT INTO [CLS].[emailbtcf].[CampaignData]
           ([EmailCampaignId]
           ,[Recipient]
           ,[AccountNumber]
           ,[FirstName]
           ,[LastName]
           ,[AddedAt]
           ,[AddedBy]
           ,[EmailSentAt]
           ,[ArcProcessedAt]
           ,[ArcAddProcessingId]
           ,[InactivatedAt]
           ,[LineDataId])
SELECT DISTINCT
	@EmailCampaignId AS EmailCampaingId,
	COALESCE(PD32.DX_CNC_EML_ADR, PD32.DX_ADR_EML) AS Recipient,
	PD10.DF_SPE_ACC_ID AS AccountNumber,
	PD10.DM_PRS_1 AS FirstName,
	'' AS LastName,
	@CurrentDate AS AddedAt,
	@SasId AS AddedBy,
	NULL AS EmailSentAt,
	NULL AS ArcProcessedAt,
	NULL AS ArcAddProcessingId,
	NULL AS InactivatedAt,
	NULL AS LineDataId
FROM
	PD10_PRS_NME PD10
	INNER JOIN
			( -- email address
				SELECT
					*,
					ROW_NUMBER() OVER (PARTITION BY Email.DF_PRS_ID ORDER BY Email.PriorityNumber) [EmailPriority] -- number in order of Email.PriorityNumber
				FROM
				(
					SELECT
						PD10.DF_PRS_ID,
						PD32.DX_ADR_EML,
						PH05.DX_CNC_EML_ADR,
						CASE
							WHEN PD32.DC_ADR_EML = 'H' THEN 1 -- home
							WHEN PD32.DC_ADR_EML = 'A' THEN 2 -- alternate
							WHEN PD32.DC_ADR_EML = 'W' THEN 3 -- work
						END [PriorityNumber]
					FROM
						PD10_PRS_NME PD10
						LEFT JOIN PD32_PRS_ADR_EML PD32
							ON PD10.DF_PRS_ID = PD32.DF_PRS_ID
							AND PD32.DI_VLD_ADR_EML = 'Y' -- valid email address
							AND PD32.DC_STA_PD32 = 'A' -- active email address record
						LEFT JOIN PH05_CNC_EML PH05
							ON PD10.DF_SPE_ACC_ID = PH05.DF_SPE_ID
							AND PH05.DI_VLD_CNC_EML_ADR = 'Y'
				) Email
			) PD32
			ON PD32.DF_PRS_ID = PD10.DF_PRS_ID
			AND PD32.EmailPriority = 1
	INNER JOIN LN10_LON LN10
		ON PD10.DF_PRS_ID = LN10.BF_SSN
	INNER JOIN LN16_LON_DLQ_HST LN16
		ON LN10.BF_SSN = LN16.BF_SSN
		AND LN10.LN_SEQ = LN16.LN_SEQ
	INNER JOIN DW01_DW_CLC_CLU DW01
		ON PD10.DF_PRS_ID = DW01.BF_SSN
		AND LN10.LN_SEQ = DW01.LN_SEQ
		AND DW01.WC_DW_LON_STA = '03' 
	LEFT JOIN CLS.emailbtcf.CampaignData EXISTING_DATA --flag to exclude existing data added today
		ON EXISTING_DATA.EmailCampaignId = @EmailCampaignId
		AND EXISTING_DATA.AccountNumber = PD10.DF_SPE_ACC_ID --AccountNumber
		AND CONVERT(DATE, EXISTING_DATA.AddedAt) = CONVERT(DATE, @CurrentDate) --to remove anyone added today
WHERE
	LN10.LC_STA_LON10 = 'R'
	AND LN10.LA_CUR_PRI > 0.00
	AND 
	(
		PD32.DX_CNC_EML_ADR IS NOT NULL
		OR PD32.DX_ADR_EML IS NOT NULL
	)
	AND
	(
		(
			@DateName = 'Monday'
			AND LN16.LN_DLQ_MAX + 1 BETWEEN  10 and 12
			AND LN16.LC_STA_LON16 = 1
		)
		OR
		(
			@DateName != 'Monday' --It is not necessary, but it is more clear
			AND LN16.LN_DLQ_MAX + 1 = 10
			AND LN16.LC_STA_LON16 = 1
		)
	)
	AND EXISTING_DATA.AccountNumber IS NULL --wasn't already today
ORDER BY AccountNumber