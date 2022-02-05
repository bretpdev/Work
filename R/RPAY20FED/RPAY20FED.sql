/********************************************
	REPAYMENT IN 20 DAYS - FED
*********************************************/

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
USE CDW;

IF OBJECT_ID('tempdb..#RPAY20FED') IS NOT NULL 
	DROP TABLE #RPAY20FED
GO

DECLARE @SackerID VARCHAR(100) = 'RPAY20FED';
DECLARE @LetterID VARCHAR(100) = 'PYMNTDFED.html';

DECLARE @TODAY DATE = GETDATE();
DECLARE @PREVIOUSDAYCALC DATE = 
(--if run on Friday then do 22 days, else do 20 DAYS
	SELECT
		CASE
			WHEN DATENAME(WEEKDAY,@TODAY) = 'Friday'
			THEN DATEADD(DAY,22,@TODAY)
			ELSE DATEADD(DAY,20,@TODAY)
		END
);

DECLARE @20DAYSAWAY DATE = DATEADD(DAY,20,@TODAY);

--select @TODAY,@PREVIOUSDAYCALC, @20DAYSAWAY --TEST

DECLARE @EmailCampaignId INT = (SELECT EmailCampaignId FROM CLS.emailbtcf.EmailCampaigns WHERE LetterId = @LetterID);
--select @EmailCampaignId --TEST

--base population:
SELECT DISTINCT
	 BASEPOP.Recipient
	,BASEPOP.AccountNumber
	,BASEPOP.FirstName
	,(
		BASEPOP.FirstName + ',"$' + 
		CAST(BASEPOP.LA_RPS_ISL AS VARCHAR(20)) + '",' +  
		FORMAT(BASEPOP.LD_RPS_1_PAY_DU,'d','en-US')
	 ) AS LineData
INTO
	#RPAY20FED
FROM
(
	SELECT DISTINCT
		 COALESCE(PH05.DX_CNC_EML_ADR, PD32.ALT_EM) AS Recipient
		,PD10.DF_SPE_ACC_ID AS AccountNumber
		,CentralData.dbo.PascalCase(PD10.DM_PRS_1) AS FirstName
		,SUM(RPS.LA_RPS_ISL) AS LA_RPS_ISL
		,RPS.LD_RPS_1_PAY_DU
	FROM
		CDW..LN10_LON LN10
		INNER JOIN CDW.calc.RepaymentSchedules RPS
			ON LN10.BF_SSN = RPS.BF_SSN
			AND LN10.LN_SEQ = RPS.LN_SEQ
			AND RPS.CurrentGradation = 1
		INNER JOIN CDW..PD10_PRS_NME PD10
			ON LN10.BF_SSN = PD10.DF_PRS_ID
		INNER JOIN CDW..DW01_DW_CLC_CLU DW01
			ON LN10.BF_SSN = DW01.BF_SSN
			AND LN10.LN_SEQ = DW01.LN_SEQ
			AND DW01.WC_DW_LON_STA = '01' --in grace
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
	WHERE
		LN10.LD_END_GRC_PRD BETWEEN @20DAYSAWAY AND @PREVIOUSDAYCALC
		AND LN10.LC_STA_LON10 = 'R' --released
		AND LN10.LA_CUR_PRI > 0.00 --has a balance
		AND COALESCE(PH05.DX_CNC_EML_ADR, PD32.ALT_EM) IS NOT NULL
	GROUP BY
		 COALESCE(PH05.DX_CNC_EML_ADR, PD32.ALT_EM)
		,PD10.DF_SPE_ACC_ID
		,CentralData.dbo.PascalCase(PD10.DM_PRS_1)
		,RPS.LD_RPS_1_PAY_DU
)BASEPOP
;

--select * from #RPAY20FED

--attempt INSERT
BEGIN TRY
	BEGIN TRANSACTION

	--1st insert: put new data into table to assign ID
	INSERT INTO CLS.emailbtcf.LineData
	(
		LineData
	)
	SELECT DISTINCT
		RP20.LineData
	FROM
		#RPAY20FED RP20
		LEFT JOIN CLS.emailbtcf.LineData EXISTING_DATA
			ON EXISTING_DATA.LineData = RP20.LineData
	WHERE
		EXISTING_DATA.LineData IS NULL --wasn't already added
	;

	--select * from CLS.emailbtcf.LineData  --TEST
	--delete from CLS.emailbtcf.LineData	--TEST
	--truncate table CLS.emailbtcf.LineData	--TEST

	--2nd insert: put new daily data into table w/linedata ID reference
	INSERT INTO CLS.emailbtcf.CampaignData
	(
		 EmailCampaignId
		,Recipient
		,AccountNumber
		,FirstName
		,LastName
		,AddedAt
		,AddedBy
		,LineDataId
	)
	SELECT DISTINCT 
		 @EmailCampaignId AS EmailCampaignId
		,RP20.Recipient
		,RP20.AccountNumber
		,RP20.FirstName
		,'' AS LastName
		,GETDATE() AS AddedAt
		,@SackerID AS AddedBy
		,LD.LineDataId
	FROM
		#RPAY20FED RP20
		LEFT JOIN CLS.emailbtcf.LineData LD
			ON RP20.LineData = LD.LineData
		LEFT JOIN CLS.emailbtcf.CampaignData EXISTING_DATA --flag to exclude existing data added today
			ON EXISTING_DATA.EmailCampaignId = @EmailCampaignId
			AND EXISTING_DATA.Recipient = RP20.Recipient
			AND EXISTING_DATA.AccountNumber = RP20.AccountNumber
			AND EXISTING_DATA.FirstName = RP20.FirstName
			AND CAST(EXISTING_DATA.AddedAt AS DATE) = @TODAY
	WHERE
		EXISTING_DATA.AccountNumber IS NULL --wasn't already added today
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
