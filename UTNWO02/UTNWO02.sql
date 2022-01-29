DECLARE @CURRENTDATE DATETIME = GETDATE();
DECLARE @SASID VARCHAR(10) = 'UTNWO02';

BEGIN TRY
	BEGIN TRANSACTION

	INSERT INTO 
		CLS.emailbtcf.CampaignData
			(
				EmailCampaignId
				,Recipient
				,AccountNumber
				,FirstName
				,LastName
				,AddedAt
				,AddedBy
				,EmailSentAt
				,ArcProcessedAt
				,ArcAddProcessingId
				,InactivatedAt
				,LineDataId
			)
	SELECT DISTINCT
		CAMP.EmailCampaignId,
		NEW.EMAIL,
		NEW.DF_SPE_ACC_ID,
		NEW.FNAME,
		NEW.LNAME,
		@CURRENTDATE AS AddedAt,
		@SASID AS AddedBy,
		NULL AS EmailSentAt,
		NULL AS ArcProcessedAt,
		NULL AS ArcAddProcessingId,
		NULL AS InactivatedAt,
		NULL AS LineDataId
	FROM
		(
			SELECT DISTINCT
				PD10.DF_SPE_ACC_ID,
				PD10.DM_PRS_1 AS FNAME,
				PD10.DM_PRS_LST AS LNAME,
				COALESCE(PH05.DX_CNC_EML_ADR, PD32.DX_ADR_EML) AS EMAIL,
				AY10.PF_REQ_ACT,
				CASE
					WHEN PF_REQ_ACT IN ('CODCA','CODPA') THEN 'IDREMRCVD.html' --'UNWO02.NWO02R2*'
					WHEN PF_REQ_ACT IN ('DIDFR','DICTD') THEN 'DEFEMLFED.html' --'UNWO02.NWO02R3*'
					WHEN PF_REQ_ACT IN ('DITLF','DICSK','DIFCR','DIUPR','DIS11') THEN 'DISEMLFED.html' --'UNWO02.NWO02R4*'
					ELSE 'FOREMLFED.html' --'UNWO02.NWO02R5*'
				END AS LETTER_ID,
				AY10.LD_ATY_REQ_RCV
			FROM
				CDW..AY10_BR_LON_ATY AY10
				INNER JOIN CDW..PD10_PRS_NME PD10
					ON AY10.BF_SSN = PD10.DF_PRS_ID
			-- PD32 email address for highest priority email address type if the borrower has a PD32 email address
				LEFT JOIN
				( 
					SELECT
						Email.DF_PRS_ID,
						Email.DX_ADR_EML,
						ROW_NUMBER() OVER (PARTITION BY Email.DF_PRS_ID ORDER BY Email.PriorityNumber) EmailPriority -- number in order of Email.PriorityNumber
					FROM
					(
						SELECT
							PD10.DF_PRS_ID,
							PD32.DX_ADR_EML,
							CASE
								WHEN PD32.DC_ADR_EML = 'H' THEN 2 -- home
								WHEN PD32.DC_ADR_EML = 'A' THEN 3 -- alternate
								WHEN PD32.DC_ADR_EML = 'W' THEN 4 -- work
							END PriorityNumber
						FROM
							CDW..PD10_PRS_NME PD10
							LEFT JOIN CDW..PD32_PRS_ADR_EML PD32
								ON PD10.DF_PRS_ID = PD32.DF_PRS_ID
								AND PD32.DI_VLD_ADR_EML = 'Y' -- valid email address
								AND PD32.DC_STA_PD32 = 'A' -- active email address record
					) Email
				) PD32
					ON PD32.DF_PRS_ID = PD10.DF_PRS_ID
					AND PD32.EmailPriority = 1
			--PH05 email address if it exists
				LEFT JOIN CDW..PH05_CNC_EML PH05
					ON PD10.DF_SPE_ACC_ID = PH05.DF_SPE_ID
					AND PH05.DI_VLD_CNC_EML_ADR = 'Y'
			WHERE
				AY10.PF_REQ_ACT IN('CODCA','CODPA','DIDFR','DIFRB','DITLF','DICSK','DIFCR','DIUPR','DIS11','DICTD')
				AND AY10.LC_STA_ACTY10 = 'A'
				AND (PH05.DX_CNC_EML_ADR IS NOT NULL OR PD32.DX_ADR_EML IS NOT NULL) --the borrower has a valid email address
		) NEW
	--join to EmailCampaigns to get the EmailCampaignId
		INNER JOIN CLS.emailbtcf.EmailCampaigns CAMP
			ON NEW.LETTER_ID = CAMP.LetterId
	--join to existing data to prevent duplicates
		LEFT JOIN
		(
			SELECT
				EmailCampaignId,
				AccountNumber,
				MAX(ADDEDAT) AS ADDEDAT
			FROM
				CLS.emailbtcf.CampaignData
			GROUP BY
				EmailCampaignId,
				AccountNumber
		) EXISTING
			ON CAMP.EmailCampaignId = EXISTING.EmailCampaignId
			AND NEW.DF_SPE_ACC_ID = EXISTING.AccountNumber
	WHERE
		DATEDIFF(DAY, NEW.LD_ATY_REQ_RCV, CAST(GETDATE() AS DATE)) <= 6
		AND CAST(NEW.LD_ATY_REQ_RCV AS DATE) < CAST(GETDATE() AS DATE) --DO NOT INCLUDE ANYTHING ADDED TODAY IT WILL BE PICKED UP TOMORROW
		AND
		(
			EXISTING.ACCOUNTNUMBER IS NULL
			OR 
			CAST(NEW.LD_ATY_REQ_RCV AS DATE) > CAST(EXISTING.ADDEDAT AS DATE)
		)


	COMMIT TRANSACTION;

END TRY
--write message to process logger if an error occurs
BEGIN CATCH
	DECLARE @EM VARCHAR(4000) = @SASID + '.sql encountered an error.  Transaction not committed. Error: ' + (SELECT ERROR_MESSAGE());

	ROLLBACK TRANSACTION;

	DECLARE @ProcessLogId INT;
	DECLARE @ProcessNotificationId INT;
	DECLARE @NotificationTypeId INT = (SELECT TOP 1 NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'); --Error report
	DECLARE @NotificationSeverityTypeId INT = (SELECT TOP 1 NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
		
	INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(@CURRENTDATE,@CURRENTDATE,@SASID,'cornerstone',SUSER_SNAME());
	SET @ProcessLogId = SCOPE_IDENTITY()

	INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId,@NotificationSeverityTypeId,@ProcessLogId, NULL, NULL)
	SET @ProcessNotificationId = SCOPE_IDENTITY()

	INSERT INTO CLS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId,@EM);

	THROW;
END CATCH;