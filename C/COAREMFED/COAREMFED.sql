/********************************************
	CREATE ONLINE ACCOUNT REMINDER - FED
*********************************************/

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @SackerID VARCHAR(100) = 'COAREMFED';
DECLARE @LetterID VARCHAR(50) = 'ONLACNTFED.html';

DECLARE @TODAY DATE = GETDATE();
DECLARE @PREVIOUSDAYCALC DATE = 
(--if run on Monday then do 62 days ago, else do 60 days
	SELECT
		CASE
			WHEN DATENAME(WEEKDAY,@TODAY) = 'Monday'
			THEN DATEADD(DAY,-62,@TODAY)
			ELSE DATEADD(DAY,-60,@TODAY)
		END
);

DECLARE @60DAYSAGO DATE = DATEADD(DAY,-60,@TODAY);

--select @TODAY,@PREVIOUSDAYCALC,@60DAYSAGO --TEST

--attempt INSERT
BEGIN TRY
	BEGIN TRANSACTION

	DECLARE @EmailCampaignId INT = (SELECT EmailCampaignId FROM CLS.emailbtcf.EmailCampaigns WHERE LetterId = @LetterID);

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
		 @EmailCampaignId AS EmailCampaignId
		,COALESCE(PH05.DX_CNC_EML_ADR, PD32.ALT_EM) AS Recipient
		,PD10.DF_SPE_ACC_ID AS AccountNumber
		,CentralData.dbo.PascalCase(PD10.DM_PRS_1) AS FirstName
		,'' AS LastName
		,GETDATE() AS AddedAt
		,@SackerID AS AddedBy
	FROM
		CDW..LN10_LON LN10
		INNER JOIN CDW..LN15_DSB LN15
			ON LN10.BF_SSN = LN15.BF_SSN
			AND LN10.LN_SEQ = LN15.LN_SEQ
			AND LN15.LD_DSB BETWEEN @PREVIOUSDAYCALC AND @60DAYSAGO
			AND LN15.LN_BR_DSB_SEQ = 1
			AND LN15.LC_DSB_TYP = '2'
			AND LN15.LC_STA_LON15 IN ('1','3') -- active, active reissue
			AND LN15.LA_DSB - ISNULL(LN15.LA_DSB_CAN, 0) > 0
		INNER JOIN CDW..PD10_PRS_NME PD10
			ON LN10.BF_SSN = PD10.DF_PRS_ID
		INNER JOIN CDW..DW01_DW_CLC_CLU DW01
			ON LN10.BF_SSN = DW01.BF_SSN
			AND LN10.LN_SEQ = DW01.LN_SEQ
			AND DW01.WC_DW_LON_STA = '02'
		LEFT JOIN CDW..WB24_CSM_USR_ACC WB24 -- web user account
			ON LN10.BF_SSN = WB24.DF_USR_SSN
			--flag not needed
		LEFT JOIN CDW..PH05_CNC_EML PH05 
			ON PH05.DF_SPE_ID = PD10.DF_SPE_ACC_ID
			AND	PH05.DI_VLD_CNC_EML_ADR = 'Y' -- valid email
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
			AND EXISTING_DATA.Recipient = COALESCE(PH05.DX_CNC_EML_ADR, PD32.ALT_EM) --Recipient
			AND EXISTING_DATA.AccountNumber = PD10.DF_SPE_ACC_ID --AccountNumber
			AND EXISTING_DATA.FirstName = CentralData.dbo.PascalCase(PD10.DM_PRS_1) --FirstName
			AND CAST(EXISTING_DATA.AddedAt AS DATE) = @Today
	WHERE
		COALESCE(PH05.DX_CNC_EML_ADR, PD32.ALT_EM) IS NOT NULL
		AND EXISTING_DATA.AccountNumber IS NULL --wasn't already added today
		AND WB24.DF_USR_SSN IS NULL --doesn't already have an active web user account
	;
	COMMIT TRANSACTION;

END TRY
	--write message to process logger if an error occurs
	BEGIN CATCH
		DECLARE @EM VARCHAR(4000) = @SackerID + ' encountered an error.  Transaction not committed. Error: ' + (SELECT ERROR_MESSAGE());

		ROLLBACK TRANSACTION;

		DECLARE @ProcessLogId INT;
		DECLARE @ProcessNotificationId INT;
		DECLARE @NotificationTypeId INT = (SELECT TOP 1 NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'); --Error report
		DECLARE @NotificationSeverityTypeId INT = (SELECT TOP 1 NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
		
		INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(GETDATE(),GETDATE(),@SackerID,'cornerstone',SUSER_SNAME());
		SET @ProcessLogId = SCOPE_IDENTITY()

		INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId,@NotificationSeverityTypeId,@ProcessLogId, NULL, NULL)
		SET @ProcessNotificationId = SCOPE_IDENTITY()

		INSERT INTO CLS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId,@EM);

	THROW;
END CATCH;
