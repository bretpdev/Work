USE [CDW]
GO

DECLARE @CurrentDate DATE = GETDATE()
DECLARE @DateName VARCHAR(20) = (SELECT DATENAME(DW,@CurrentDate)) 
DECLARE @EmailCampaign VARCHAR(20) = 'DLRCIDRFED.html' --Letter Id For Email Campaigns Table
DECLARE @SasId VARCHAR(20) = 'IDRDLEMFD'
DECLARE @DelinquencyDays TINYINT = 5
DECLARE @DaysBegin TINYINT = @DelinquencyDays - 1
DECLARE @DaysEnd TINYINT = (SELECT CASE WHEN @DateName = 'Monday' THEN @DelinquencyDays + 1 ELSE @DelinquencyDays - 1 END)

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
	--loans are in repayment
	INNER JOIN DW01_DW_CLC_CLU DW01
		ON PD10.DF_PRS_ID = DW01.BF_SSN
		AND DW01.WC_DW_LON_STA = '03'
	INNER JOIN LN10_LON LN10
		ON PD10.DF_PRS_ID = LN10.BF_SSN
		AND DW01.LN_SEQ = LN10.LN_SEQ
	INNER JOIN LN16_LON_DLQ_HST LN16
		ON LN10.BF_SSN = LN16.BF_SSN
		AND LN10.LN_SEQ = LN16.LN_SEQ
	INNER JOIN
	(
		--Get the reapyment schedule and check that it is valid, and then take the penultimate record in LN65 to see it's type
		SELECT
			LN65_A.*,
			RS10.LD_RPS_1_PAY_DU,
			LN66.LN_RPS_TRM
		FROM
			--Get the current active schedule 
			(
				SELECT
					LN65.BF_SSN,
					ROW_NUMBER() OVER (PARTITION BY LN65.BF_SSN ORDER BY LN65.LN_RPS_SEQ DESC) AS [Priority]
				FROM
					LN65_LON_RPS LN65
				WHERE
					LN65.LC_STA_LON65 = 'A'
					AND LN65.LC_TYP_SCH_DIS IN ('CQ','IL','CP','IP')
			) LN65_A
			--Get the previous inactive schedule
			INNER JOIN
			(
				SELECT
					LN65.BF_SSN,
					LN65.LN_SEQ,
					LN65.LN_RPS_SEQ,
					LC_TYP_SCH_DIS,
					ROW_NUMBER() OVER (PARTITION BY LN65.BF_SSN ORDER BY LN65.LN_RPS_SEQ DESC) AS [Priority]
				FROM
					LN65_LON_RPS LN65
					--Handle split repayment schedules by getting the loans that need to be join on for the previous plan
					INNER JOIN
					(
						SELECT 
							LN65.BF_SSN,
							LN65.LN_SEQ
						FROM
							LN65_LON_RPS LN65
							INNER JOIN
							(
								SELECT
									LN65_A_I.BF_SSN,
									LN65_A_I.LN_SEQ,
									MAX(LN65_A_I.LN_RPS_SEQ) AS LN_RPS_SEQ
								FROM
									LN65_LON_RPS LN65_A_I
								WHERE
									LN65_A_I.LC_STA_LON65 = 'A'
									AND LN65_A_I.LC_TYP_SCH_DIS IN ('CQ','IL','CP','IP')
								GROUP BY
									LN65_A_I.BF_SSN,
									LN65_A_I.LN_SEQ
							) AS LN65_MAX_RPS
								ON LN65.BF_SSN = LN65_MAX_RPS.BF_SSN
								AND LN65.LN_SEQ = LN65_MAX_RPS.LN_SEQ
								AND LN65.LN_RPS_SEQ = LN65_MAX_RPS.LN_RPS_SEQ	
					) LN65_MAX
						ON LN65.BF_SSN = LN65_MAX.BF_SSN
						AND LN65.LN_SEQ = LN65_MAX.LN_SEQ
				WHERE
					LN65.LC_STA_LON65 = 'I'
			) LN65_I
				ON LN65_A.BF_SSN = LN65_I.BF_SSN
				AND LN65_I.[Priority] = 1
			INNER JOIN LN66_LON_RPS_SPF LN66
				ON LN65_I.BF_SSN = LN66.BF_SSN
				AND LN65_I.LN_SEQ = LN66.LN_SEQ
				AND LN65_I.LN_RPS_SEQ = LN66.LN_RPS_SEQ
				AND LN66.LN_GRD_RPS_SEQ = 1	
			INNER JOIN RS10_BR_RPD RS10
				ON LN65_I.BF_SSN = RS10.BF_SSN
				AND LN65_I.LN_RPS_SEQ = RS10.LN_RPS_SEQ
		WHERE
			LN65_A.[Priority] = 1
			AND LN65_I.LC_TYP_SCH_DIS IN ('IB','C1','C2','C3','CA','IA','I3','I5')
	) LN65
		ON PD10.DF_PRS_ID = LN65.BF_SSN
	LEFT JOIN CLS.emailbtcf.CampaignData EXISTING_DATA --flag to exclude existing data added today
		ON EXISTING_DATA.EmailCampaignId = @EmailCampaignId
		AND EXISTING_DATA.AccountNumber = PD10.DF_SPE_ACC_ID --AccountNumber
		AND EXISTING_DATA.AddedAt >= @CurrentDate
		AND EXISTING_DATA.AddedAt < DATEADD(DAY, 1, @CurrentDate) --to remove anyone added today
WHERE
	LN10.LC_STA_LON10 = 'R'
	AND LN10.LA_CUR_PRI > 0.00
	AND 
	(
		PD32.DX_CNC_EML_ADR IS NOT NULL
		OR PD32.DX_ADR_EML IS NOT NULL
	)
	AND LN16.LC_STA_LON16 = 1
	--In the delinquency period
	AND LN16.LN_DLQ_MAX >= @DaysBegin
	AND LN16.LN_DLQ_MAX <= @DaysEnd
	AND EXISTING_DATA.AccountNumber IS NULL --wasn't already today
	--The 1st pay due date is x amount of days past the completion of the delinquency term
	AND LN16.LN_DLQ_MAX + 1 >= DATEDIFF(DAY, DATEADD(MONTH, LN65.LN_RPS_TRM, LN65.LD_RPS_1_PAY_DU), @CurrentDate)
	AND LN16.LN_DLQ_MAX + 1 <= DATEDIFF(DAY, DATEADD(MONTH, LN65.LN_RPS_TRM, LN65.LD_RPS_1_PAY_DU), @CurrentDate)