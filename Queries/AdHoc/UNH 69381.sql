BEGIN TRY
	BEGIN TRANSACTION

		DECLARE @NOW DATETIME = GETDATE(),
				@TODAY DATE = GETDATE(),
				@ScriptId VARCHAR(10) = 'UNH 69381',
				@HTMLFileId INT = (SELECT HTMLFileId FROM ULS.emailbatch.HTMLFiles WHERE HTMLFile = 'MOBLAPPMUH.html'),
				@FromAddressId INT = (SELECT FromAddressId FROM ULS.emailbatch.FromAddresses WHERE FromAddress = 'uheaa@utahsbr.edu'),
				@SubjectLineId INT = (SELECT SubjectLineId FROM ULS.emailbatch.SubjectLines WHERE SubjectLine = 'The UHEAA Mobile App Is Here!'),
				@ArcId INT = (SELECT ArcId FROM ULS.emailbatch.Arcs WHERE Arc = 'P203A'),
				@CommentId INT = (SELECT CommentId FROM ULS.emailbatch.Comments WHERE Comment = 'Mobile App Notification Sent')
		;
		--select @NOW,@TODAY,@ScriptId,@HTMLFileId,@FromAddressId,@SubjectLineId,@ArcId,@CommentId --test

		DECLARE @EmailCampaignId INT = 
		(
			SELECT 
				EmailCampaignId
			FROM 
				ULS.emailbatch.EmailCampaigns
			WHERE
				HTMLFileId = @HTMLFileId
				AND FromAddressId = @FromAddressId
				AND SubjectLineId = @SubjectLineId
				AND ArcId = @ArcId
				AND CommentId = @CommentId
		);
		--select @EmailCampaignId --test

		/*** EMAIL PROCESSING **/
		INSERT INTO ULS.emailbatch.EmailProcessing
		(
			EmailCampaignId, 
			AccountNumber, 
			EmailData, 
			ArcNeeded, 
			ProcessingAttempts, 
			AddedAt, 
			AddedBy
		)
		SELECT DISTINCT TOP 20000
			@EmailCampaignId,
			ALLPOP.AccountNumber,
			CONCAT(ALLPOP.AccountNumber,',',SUBSTRING(ALLPOP.DM_PRS_1,1,1),RTRIM(LOWER(SUBSTRING(ALLPOP.DM_PRS_1,2,13))),',',ALLPOP.EmailAddress) AS EmailData,
			1 AS ArcNeeded,
			0 AS ProcessingAttempts,
			@NOW AS AddedAt,
			@ScriptId AS AddedBy
		FROM
			(--query from UNH 69350
				SELECT DISTINCT
					PD10.DF_SPE_ACC_ID AS AccountNumber,
					PD10.DM_PRS_1,
					EM.EmailAddress
					--VALIDATION FIELDS:
					--,LN10.BF_SSN
					--,LN10.LN_SEQ
					--,LN10.LA_CUR_PRI
					--,DW01.WA_TOT_BRI_OTS
					--,ISNULL(LN10.LA_CUR_PRI,0) + ISNULL(DW01.WA_TOT_BRI_OTS,0) AS OutstandingBalance
				FROM
					UDW..LN10_LON LN10
					INNER JOIN UDW..PD10_PRS_NME PD10
						ON LN10.BF_SSN = PD10.DF_PRS_ID
					INNER JOIN UDW..DW01_DW_CLC_CLU DW01
						ON LN10.BF_SSN = DW01.BF_SSN
						AND LN10.LN_SEQ = DW01.LN_SEQ
					INNER JOIN
					(--get all valid emails (see UDWREFSH2 SASR_4768 for soon to be promoted job)
					--note: this section should all be OK
						SELECT DISTINCT
							EmailOrdering.DF_PRS_ID,
							EmailOrdering.EmailAddress
						FROM
							(--orders emails according to priority
								SELECT
									EmailPrioritizing.DF_PRS_ID,
									EmailPrioritizing.EmailAddress,
									ROW_NUMBER() OVER (PARTITION BY EmailPrioritizing.DF_PRS_ID ORDER BY EmailPrioritizing.PriorityNumber) AS EmailPriority --number in order of Email.PriorityNumber
								FROM
									(--assign priority to emails by type
										SELECT DISTINCT
											PD10.DF_PRS_ID,
											ISNULL(PH05.DX_CNC_EML_ADR, PD32.DX_ADR_EML) AS EmailAddress,
											CASE 
												WHEN PH05.DX_CNC_EML_ADR IS NOT NULL THEN 1 --PH05 takes highest priority
												WHEN PD32.DC_ADR_EML = 'H' THEN 2 --home
												WHEN PD32.DC_ADR_EML = 'A' THEN 3 --alternate
												WHEN PD32.DC_ADR_EML = 'W' THEN 4 --work
											END AS PriorityNumber
										FROM
											UDW..PD10_PRS_NME PD10
											LEFT JOIN UDW..PH05_CNC_EML PH05
												ON PH05.DF_SPE_ID = PD10.DF_SPE_ACC_ID
												AND PH05.DI_VLD_CNC_EML_ADR = 'Y' --valid email address
											LEFT JOIN UDW..PD32_PRS_ADR_EML PD32
												ON PD32.DF_PRS_ID = PD10.DF_PRS_ID
												AND PD32.DI_VLD_ADR_EML = 'Y' --valid email address
												AND PD32.DC_STA_PD32 = 'A' --active email address record
										WHERE
											PD10.DF_PRS_ID LIKE '[0-9]%' --to exclude acct#'s beginning with P
									) EmailPrioritizing
								WHERE
									EmailPrioritizing.EmailAddress IS NOT NULL --excludes borrowers without email address
							) EmailOrdering
						WHERE
							EmailOrdering.EmailPriority = 1 --highest priority email only
					) EM
						ON EM.DF_PRS_ID = LN10.BF_SSN
					LEFT JOIN UDW..PD21_GTR_DTH PD21
						ON PD10.DF_PRS_ID = PD21.DF_PRS_ID
						AND PD21.DC_DTH_STA = '02' --verified death
				WHERE
					PD21.DF_PRS_ID IS NULL --exclude verified death
					AND LN10.LA_CUR_PRI > 0.00
					AND LN10.LC_STA_LON10 = 'R'
					AND PD10.DF_PRS_ID LIKE '[0-9]%'
					AND DW01.WC_DW_LON_STA NOT IN 
						(
							'17', --Death Verified
							'19', --Disability Verified
							'21'  --Bankruptcy Verified
						)
			) ALLPOP
			LEFT JOIN ULS.emailbatch.EmailProcessing ExistingRequest
				ON ExistingRequest.EmailCampaignId = @EmailCampaignId
				AND ExistingRequest.AccountNumber = ALLPOP.AccountNumber
				AND ExistingRequest.EmailData = CONCAT(ALLPOP.AccountNumber,',',SUBSTRING(ALLPOP.DM_PRS_1,1,1),RTRIM(LOWER(SUBSTRING(ALLPOP.DM_PRS_1,2,13))),',',ALLPOP.EmailAddress)
				--AND CAST(ExistingRequest.AddedAt AS DATE) = @TODAY --commented out because only want to add borrowers one time ever
		WHERE
			ExistingRequest.AccountNumber IS NULL --Wasnt already added
		;
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	DECLARE @EM VARCHAR(4000) = @ScriptId + ' encountered an error.  Transaction not committed. Error: ' + (SELECT ERROR_MESSAGE());

	ROLLBACK TRANSACTION;

	DECLARE @ProcessLogId INT;
	DECLARE @ProcessNotificationId INT;
	DECLARE @NotificationTypeId INT = (SELECT TOP 1 NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'); --Error report
	DECLARE @NotificationSeverityTypeId INT = (SELECT TOP 1 NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
		
	INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(@NOW,@NOW,@ScriptId,'uheaa',SUSER_SNAME());
	SET @ProcessLogId = SCOPE_IDENTITY()

	INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId,@NotificationSeverityTypeId,@ProcessLogId, NULL, NULL)
	SET @ProcessNotificationId = SCOPE_IDENTITY()

	INSERT INTO ULS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId,@EM);

	THROW;
END CATCH;