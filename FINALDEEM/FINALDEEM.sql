--FINALDEEM.sql

--get EmailCampaignId:
DECLARE @Arc VARCHAR(5) = 'EMDFD';
DECLARE @LetterId VARCHAR(50) = 'FINDEMEFED.html';
DECLARE @CommentText VARCHAR(255) = 'Final demand email sent.';
DECLARE @EmailCampaignId INT = 
(
	SELECT 
		EmailCampaignId 
	FROM 
		CLS.emailbtcf.EmailCampaigns 
	WHERE 
		Arc = @Arc 
		AND LetterId = @LetterId
		AND CommentText = @CommentText 
);
--select @EmailCampaignId --TEST

--get day range:
DECLARE @CurrentDate DATE = GETDATE();
DECLARE @DateName VARCHAR(20) = (SELECT DATENAME(DW,@CurrentDate));
DECLARE @DaysBegin TINYINT = 240;
DECLARE @DaysEnd TINYINT = (SELECT CASE WHEN @DateName = 'Monday' THEN 242 ELSE 240 END);
--select @CurrentDate,@DateName,@DaysBegin,@DaysEnd,DATEADD(DAY,1,@CurrentDate) --TEST

INSERT INTO CLS.emailbtcf.CampaignData
(
	 EmailCampaignId
	,Recipient
	,AccountNumber
	,FirstName
	,LastName
	,AddedAt
	,AddedBy
)
SELECT DISTINCT
	@EmailCampaignId
	,COALESCE(PD32.DX_CNC_EML_ADR, PD32.DX_ADR_EML)-- AS Recipient
	,PD10.DF_SPE_ACC_ID
	,PD10.DM_PRS_1
	,''
	,GETDATE()
	,SYSTEM_USER
	--LN16.LN_DLQ_MAX --TEST
FROM
	CDW..PD10_PRS_NME PD10
	INNER JOIN CDW..LN10_LON LN10
		ON LN10.BF_SSN = PD10.DF_PRS_ID
	INNER JOIN CDW..LN16_LON_DLQ_HST LN16
		ON LN10.BF_SSN = LN16.BF_SSN
		AND LN10.LN_SEQ = LN16.LN_SEQ
	INNER JOIN CDW..DW01_DW_CLC_CLU DW01
		ON LN10.BF_SSN = DW01.BF_SSN
		AND LN10.LN_SEQ = DW01.LN_SEQ
	INNER JOIN
	( -- email address
		SELECT
			Email.DF_PRS_ID,
			Email.DX_ADR_EML,
			Email.DX_CNC_EML_ADR,
			Email.[PriorityNumber],
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
				CDW..PD10_PRS_NME PD10
				LEFT JOIN CDW..PD32_PRS_ADR_EML PD32
					ON PD10.DF_PRS_ID = PD32.DF_PRS_ID
					AND PD32.DI_VLD_ADR_EML = 'Y' -- valid email address
					AND PD32.DC_STA_PD32 = 'A' -- active email address record
				LEFT JOIN CDW..PH05_CNC_EML PH05
					ON PD10.DF_SPE_ACC_ID = PH05.DF_SPE_ID
					AND PH05.DI_VLD_CNC_EML_ADR = 'Y'
		) Email
	) PD32
		ON PD32.DF_PRS_ID = PD10.DF_PRS_ID
		AND PD32.EmailPriority = 1
	LEFT JOIN CLS.emailbtcf.CampaignData EXISTING_DATA --flag to exclude existing data added today
		ON EXISTING_DATA.EmailCampaignId = @EmailCampaignId
		AND EXISTING_DATA.AccountNumber = PD10.DF_SPE_ACC_ID
		AND EXISTING_DATA.AddedAt >= @CurrentDate --to remove anyone added today
		AND EXISTING_DATA.AddedAt < DATEADD(DAY,1,@CurrentDate) --to remove anyone added today
WHERE
	EXISTING_DATA.AccountNumber IS NULL --wasn't already today
	AND LN10.LC_STA_LON10 = 'R' --released
	AND LN10.LA_CUR_PRI > 0.00
	AND LN16.LC_STA_LON16 = 1 --active delinquency
	AND LN16.LN_DLQ_MAX >= @DaysBegin
	AND LN16.LN_DLQ_MAX <= @DaysEnd
	AND DW01.WC_DW_LON_STA = '03' --repayment
	AND (
			PD32.DX_CNC_EML_ADR IS NOT NULL
			OR PD32.DX_ADR_EML IS NOT NULL
		)
;
