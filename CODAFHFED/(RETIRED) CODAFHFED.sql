--CODAFHFED
--Select the top 25,000 CornerStone Borrowers that are in an ‘In-School’ status and have a valid email address and do not have the CODAF previous left on their account in the current year 
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
USE CDW;

DECLARE @TODAY DATE = GETDATE();
DECLARE @RUNMONTH AS TINYINT = 12; --December
DECLARE @CURRENTMONTH AS TINYINT = MONTH(@TODAY);
DECLARE @CREATED_AT AS DATE = CONVERT(DATE,@TODAY);
DECLARE @SASID AS VARCHAR(10) = 'CODAFHFED';
DECLARE @Campaign VARCHAR(50) = 'COD Annual Financial Health - FED';

DECLARE @EmailCampaignId INT = (SELECT EmailCampaignId FROM CLS.emailbtcf.EmailCampaigns WHERE letterId = 'CODFINHFED.html');

IF @CURRENTMONTH = @RUNMONTH --run only in December
	BEGIN
	--attempt INSERT
		BEGIN TRY
			BEGIN TRANSACTION

			DECLARE @EmailsToSend INT = (SELECT EmailsToSend FROM CLS.emailbtcf.EmailParameters WHERE Campaign = @Campaign); --select @EmailsToSend
			INSERT INTO CLS.emailbtcf.CampaignData(EmailCampaignId,AccountNumber,Recipient,FirstName,LastName,AddedAt,AddedBy)
				SELECT DISTINCT TOP (@EmailsToSend)
					@EmailCampaignId AS EmailCampaignId,
					PD10.DF_SPE_ACC_ID AS AccountNumber,
					COALESCE(PH05.DX_CNC_EML_ADR, PD32.ALT_EM) AS Recipient,
					CentralData.dbo.PascalCase(PD10.DM_PRS_1) AS FirstName,
					'' AS LastName,
					@CREATED_AT AS AddedAt,	--CurrentDate
					@SASID AS AddedBy
				FROM
					CDW..LN10_LON LN10
					INNER JOIN CDW..PD10_PRS_NME PD10
						ON LN10.BF_SSN = PD10.DF_PRS_ID
					INNER JOIN CDW..DW01_DW_CLC_CLU DW01
						ON LN10.BF_SSN = DW01.BF_SSN
						AND LN10.LN_SEQ = DW01.LN_SEQ
						AND DW01.WC_DW_LON_STA = '02' --In School
					INNER JOIN CDW..SD10_STU_SPR SD10
						ON LN10.LF_STU_SSN = SD10.LF_STU_SSN
						AND SD10.LC_REA_SCL_SPR != '01'  --not Graduated
						AND SD10.LC_STA_STU10 = 'A'
					LEFT JOIN
					(
						SELECT
							BF_SSN,
							MAX(LD_ATY_REQ_RCV) AS MAX_LD_ATY_REQ_RCV
						FROM
							CDW..AY10_BR_LON_ATY
						WHERE
							PF_REQ_ACT = 'CODAF'
							AND LC_STA_ACTY10 = 'A'
						GROUP BY
							BF_SSN
					) AY10  --date of most recent CODAF ARC
						ON LN10.BF_SSN = AY10.BF_SSN
					LEFT JOIN CDW..PH05_CNC_EML PH05 
						ON PH05.DF_SPE_ID = PD10.DF_SPE_ACC_ID
						AND PH05.DI_VLD_CNC_EML_ADR = 'Y' -- valid email
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
					LEFT JOIN CLS.emailbtcf.CampaignData EXISTING_DATA --flag to exclude existing data added this year
						ON EXISTING_DATA.EmailCampaignId = @EmailCampaignId
						AND EXISTING_DATA.AccountNumber = PD10.DF_SPE_ACC_ID --AccountNumber
						AND YEAR(EXISTING_DATA.AddedAt) = YEAR(@Today) --to remove anyone added this year
				WHERE 
					LN10.LC_STA_LON10 = 'R'
					AND LN10.LA_CUR_PRI > 0.00
					AND YEAR(ISNULL(AY10.MAX_LD_ATY_REQ_RCV,'1900-01-01')) < YEAR(@CREATED_AT)
					AND COALESCE(PH05.DX_CNC_EML_ADR, PD32.ALT_EM) IS NOT NULL --Active email in PD32 or PH05
					AND EXISTING_DATA.AccountNumber IS NULL --wasn't already added this year
			;

			COMMIT TRANSACTION;

		END TRY
			--write message to process logger if an error occurs
			BEGIN CATCH
				DECLARE @EM VARCHAR(4000) = @SASID + ' encountered an error.  Transaction not committed. Error: ' + (SELECT ERROR_MESSAGE());

				ROLLBACK TRANSACTION;

				DECLARE @ProcessLogId INT;
				DECLARE @ProcessNotificationId INT;
				DECLARE @NotificationTypeId INT = (SELECT TOP 1 NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'); --Error report
				DECLARE @NotificationSeverityTypeId INT = (SELECT TOP 1 NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
		
				INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(GETDATE(),GETDATE(),@SASID,'cornerstone',SUSER_SNAME());
				SET @ProcessLogId = SCOPE_IDENTITY()

				INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId,@NotificationSeverityTypeId,@ProcessLogId, NULL, NULL)
				SET @ProcessNotificationId = SCOPE_IDENTITY()

				INSERT INTO CLS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId,@EM);

			THROW;
		END CATCH;
	END --only run in December
ELSE
	BEGIN
		PRINT 'CODAFHFED.sql is only run in December.'
	END
;
