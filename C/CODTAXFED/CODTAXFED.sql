/*********************************
	COD TAX CREDIT EMAIL - FED
*********************************/

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
USE CDW;

--DECLARE @RunMonth TINYINT = 10; --TEST change to current month
DECLARE @RunMonth TINYINT = 1; --JANUARY --LIVE
DECLARE @SackerID VARCHAR(100) = 'CODTAXFED';
DECLARE @LetterID VARCHAR(100) = 'TXCRDEMFED.html';
DECLARE @Campaign VARCHAR(50) = 'COD Tax Credit Email - FED';
DECLARE @Today DATE = GETDATE();
DECLARE @CurrentMonth TINYINT = MONTH(@Today);

IF @CurrentMonth = @RunMonth --run only in January
	BEGIN
	--attempt INSERT
		BEGIN TRY
			BEGIN TRANSACTION

			DECLARE @EmailCampaignId INT = (SELECT EmailCampaignId FROM CLS.emailbtcf.EmailCampaigns WHERE LetterId = @LetterID);
			--select @emailCampaignId --TEST
			DECLARE @EmailsToSend INT = (SELECT EmailsToSend FROM CLS.emailbtcf.EmailParameters WHERE Campaign = @Campaign);
			--select @EmailsToSend --TEST
			DECLARE @CurrentYear SMALLINT = YEAR(@Today);

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
			SELECT DISTINCT TOP (@EmailsToSend)
				@EmailCampaignId AS EmailCampaignId
				,COALESCE(PH05.DX_CNC_EML_ADR, PD32.ALT_EM) AS Recipient
				,PD10.DF_SPE_ACC_ID AS AccountNumber
				,CentralData.dbo.PascalCase(PD10.DM_PRS_1) AS FirstName
				,'' AS LastName
				,GETDATE() AS AddedAt
				,@SackerID AS AddedBy
			FROM 
				CDW..LN10_LON LN10
				INNER JOIN CDW..DW01_DW_CLC_CLU DW01
					ON LN10.BF_SSN = DW01.BF_SSN
					AND LN10.LN_SEQ = DW01.LN_SEQ
					AND DW01.WC_DW_LON_STA = '02' --In School	
				INNER JOIN CDW..PD10_PRS_NME PD10
					ON LN10.BF_SSN = PD10.DF_PRS_ID
				LEFT JOIN CDW..PH05_CNC_EML PH05 
					ON PH05.DF_SPE_ID = PD10.DF_SPE_ACC_ID
					AND	PH05.DI_VLD_CNC_EML_ADR = 'Y' -- valid email
					AND PH05.DI_CNC_ELT_OPI = 'Y'
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
				LEFT JOIN 
				(--flag for exclusion: CODTC in current year
					SELECT
						BF_SSN
						,MAX(LD_ATY_REQ_RCV) AS MAX_LD_ATY_REQ_RCV
					FROM
						CDW..AY10_BR_LON_ATY
					WHERE
						YEAR(LD_ATY_REQ_RCV) = @CurrentYear 
						AND PF_REQ_ACT = 'CODTC' --LIVE
						AND LC_STA_ACTY10 = 'A'
					GROUP BY
						BF_SSN
				) AY10X
					ON AY10X.BF_SSN = LN10.BF_SSN
				LEFT JOIN CLS.emailbtcf.CampaignData EXISTING_DATA --flag to exclude existing data added this year
					ON EXISTING_DATA.EmailCampaignId = @EmailCampaignId
					AND EXISTING_DATA.AccountNumber = PD10.DF_SPE_ACC_ID --AccountNumber
					AND YEAR(EXISTING_DATA.AddedAt) = YEAR(@Today) --to remove anyone added this year
			WHERE
				AY10X.BF_SSN IS NULL
				AND LN10.LC_STA_LON10 = 'R' --released
				AND LN10.LA_CUR_PRI > 0.00 --has a balance
				AND COALESCE(PH05.DX_CNC_EML_ADR, PD32.ALT_EM) IS NOT NULL
				AND EXISTING_DATA.AccountNumber IS NULL --wasn't already added this year

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


	END --run only in January
ELSE
	BEGIN
		PRINT @SackerID + '.sql is only run in January.'
	END
;