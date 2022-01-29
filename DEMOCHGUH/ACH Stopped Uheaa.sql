--DEMOCHGUH: ACH STOPPED UHEAA
BEGIN TRY
	BEGIN TRANSACTION

		DROP TABLE IF EXISTS #POP;

		DECLARE @NOW DATETIME = GETDATE();
		DECLARE @TODAY DATE = @NOW,
				@YESTERDAY DATE = DATEADD(DAY,-1,@NOW),
				@PREVIOUSWORKDAY DATE = (IIF(DATENAME(WEEKDAY,@NOW) = 'Monday', DATEADD(DAY,-3,@NOW), DATEADD(DAY,-1,@NOW))), --if run on Monday then do Friday, else do yesterday 
				@ScriptId VARCHAR(9) = 'DEMOCHGUH',
				@ARC VARCHAR(5) = 'MODEM',
				@EmailCampaignId INT = 
				(
					SELECT 
						EC.EmailCampaignId 
					FROM 
						ULS.emailbatch.EmailCampaigns EC 
						INNER JOIN ULS.emailbatch.HTMLFiles H 
							ON H.HTMLFileId = EC.HTMLFileId 
					WHERE 
						H.HTMLFile = 'DEMUPDNUH.html'
				);		
		--select @NOW,@TODAY,@YESTERDAY,@PREVIOUSWORKDAY,@ScriptId,@ARC,@EmailCampaignId --TEST

		--borrowers:
		SELECT
			PD10.DF_SPE_ACC_ID AS AccountNumber,
			CAST(PD10.DF_SPE_ACC_ID AS VARCHAR(10)) + ',' + COALESCE(LTRIM(RTRIM(PD10.DM_PRS_1)),'') + ',' + EMAIL.EmailAddress AS EmailData,
			CONCAT('Demo Change Notice sent to Borrower: ACH Stopped by ', RTRIM(LN83_History.LF_LST_USR_LN83), ' via ', RTRIM(LN83_History.LF_LST_SRC_LN83)) AS ArcAddData,
			IIF(EA.Arc IS NOT NULL, 1, 0) AS ArcNeeded
		INTO
			#POP
		FROM
			UDW..PD10_PRS_NME PD10
			INNER JOIN UDW..LN10_LON LN10
				ON LN10.BF_SSN = PD10.DF_PRS_ID
			INNER JOIN UDW.calc.EmailAddress EMAIL
				ON EMAIL.DF_PRS_ID = PD10.DF_PRS_ID
			INNER JOIN UDW..LN83_EFT_TO_LON LN83_History
				ON LN83_History.BF_SSN = PD10.DF_PRS_ID
				AND LN83_History.LC_STA_LN83 = 'D'
				AND CAST(LN83_History.LD_EFT_EFF_END AS DATE) BETWEEN @PREVIOUSWORKDAY AND @YESTERDAY
			INNER JOIN UDW..BR30_BR_EFT BR30_History
				ON BR30_History.BF_SSN = LN83_History.BF_SSN
				AND BR30_History.BN_EFT_SEQ = LN83_History.BN_EFT_SEQ
				AND BR30_History.BC_EFT_STA IN ('B','C','D','I','P') --everything but A
				AND CAST(BR30_History.BF_LST_DTS_BR30 AS DATE) BETWEEN @PREVIOUSWORKDAY AND @YESTERDAY
			LEFT JOIN UDW..LN83_EFT_TO_LON LN83 --flag for removal: active LN83's
				ON LN83.BF_SSN = PD10.DF_PRS_ID
				AND LN83.LC_STA_LN83 = 'A'
				AND LN83.LD_EFT_EFF_END IS NULL
			LEFT JOIN UDW..BR30_BR_EFT BR30 --flag for removal: active BR30's --TODO: ask BA if this is needed
				ON BR30.BF_SSN = LN83.BF_SSN
				AND BR30.BN_EFT_SEQ = LN83.BN_EFT_SEQ 
				AND BR30.BC_EFT_STA = 'A'
			LEFT JOIN ULS.emailbatch.EmailCampaigns EC
				ON EC.EmailCampaignId = @EmailCampaignId
				AND EC.DeletedAt IS NULL
			LEFT JOIN ULS.emailbatch.Arcs EA
				ON EA.ArcId = EC.ArcId
		WHERE
			LN83.BF_SSN IS NULL
			AND BR30.BF_SSN IS NULL --TODO: ask BA if this is needed
			AND PD10.DF_PRS_ID LIKE '[0-9]%'
			AND LN10.LC_STA_LON10 = 'R'
			AND LN10.LA_CUR_PRI > 0.00

		UNION ALL

		--coborrowers:
		SELECT
			PD10.DF_SPE_ACC_ID AS AccountNumber,
			CAST(PD10E.DF_SPE_ACC_ID AS VARCHAR(10)) + ',' + COALESCE(LTRIM(RTRIM(PD10E.DM_PRS_1)),'') + ',' + EMAIL.EmailAddress AS EmailData,
			CONCAT('Demo Change Notice sent to Co-Borrower: ACH Stopped by ', RTRIM(LN83_History.LF_LST_USR_LN83), ' via ', RTRIM(LN83_History.LF_LST_SRC_LN83)) AS ArcAddData,
			IIF(EA.Arc IS NOT NULL, 1, 0) AS ArcNeeded
		FROM
			UDW..PD10_PRS_NME PD10
			INNER JOIN UDW..LN10_LON LN10
				ON LN10.BF_SSN = PD10.DF_PRS_ID
			INNER JOIN UDW..LN20_EDS LN20
				ON LN20.BF_SSN = LN10.BF_SSN
				AND LN20.LN_SEQ = LN10.LN_SEQ
			INNER JOIN UDW..PD10_PRS_NME PD10E
				ON PD10E.DF_PRS_ID = LN20.LF_EDS
			INNER JOIN UDW.calc.EmailAddress EMAIL
				ON EMAIL.DF_PRS_ID = PD10E.DF_PRS_ID
			INNER JOIN UDW..LN83_EFT_TO_LON LN83_History
				ON LN83_History.BF_SSN = PD10.DF_PRS_ID
				AND LN83_History.LC_STA_LN83 = 'D'
				AND CAST(LN83_History.LD_EFT_EFF_END AS DATE) BETWEEN @PREVIOUSWORKDAY AND @YESTERDAY
			INNER JOIN UDW..BR30_BR_EFT BR30_History
				ON BR30_History.BF_SSN = LN83_History.BF_SSN
				AND BR30_History.BN_EFT_SEQ = LN83_History.BN_EFT_SEQ
				AND PD10E.DF_PRS_ID = LTRIM(RTRIM(BR30_History.BF_DDT_PAY_EDS))
				AND BR30_History.BC_DDT_PAY_PRS_TYP = 'E'
				AND BR30_History.BC_EFT_STA IN ('B','C','D','I','P') --everything but A
				AND CAST(BR30_History.BF_LST_DTS_BR30 AS DATE) BETWEEN @PREVIOUSWORKDAY AND @YESTERDAY
			LEFT JOIN UDW..LN83_EFT_TO_LON LN83 --flag for removal: active LN83's
				ON LN83.BF_SSN = PD10.DF_PRS_ID
				AND LN83.LC_STA_LN83 = 'A'
				AND LN83.LD_EFT_EFF_END IS NULL
			LEFT JOIN UDW..BR30_BR_EFT BR30 --flag for removal: active BR30's --TODO: ask BA if this is needed
				ON BR30.BF_SSN = LN83.BF_SSN
				AND BR30.BN_EFT_SEQ = LN83.BN_EFT_SEQ 
				AND BR30.BC_EFT_STA = 'A'
			LEFT JOIN ULS.emailbatch.EmailCampaigns EC
				ON EC.EmailCampaignId = @EmailCampaignId
				AND EC.DeletedAt IS NULL
			LEFT JOIN ULS.emailbatch.Arcs EA
				ON EA.ArcId = EC.ArcId
		WHERE
			LN83.BF_SSN IS NULL
			AND BR30.BF_SSN IS NULL --TODO: ask BA if this is needed
			AND PD10E.DF_PRS_ID LIKE '[0-9]%'
			AND LN10.LC_STA_LON10 = 'R'
			AND LN10.LA_CUR_PRI > 0.00
			AND LN20.LC_EDS_TYP = 'M'
			AND LN20.LC_STA_LON20 = 'A'
		;
		--select distinct * from #POP --TEST

		/*** EMAIL PROCESSING **/
		INSERT INTO ULS.emailbatch.EmailProcessing
		(
			EmailCampaignId, 
			AccountNumber, 
			ActualFile, 
			EmailData, 
			ArcNeeded, 
			ProcessingAttempts, 
			AddedAt, 
			AddedBy
		)
		SELECT DISTINCT
			@EmailCampaignId AS EmailCampaignId,
			P.AccountNumber,
			NULL AS ActualFile,
			P.EmailData,
			P.ArcNeeded,
			0 AS ProcessingAttempts,
			@NOW AS AddedAt,
			@ScriptId AS AddedBy
		FROM
			#POP P
			LEFT JOIN ULS.emailbatch.EmailProcessing ExistingRequest
				ON ExistingRequest.EmailCampaignId = @EmailCampaignId
				AND ExistingRequest.AccountNumber = P.AccountNumber
				AND ExistingRequest.EmailData = P.EmailData
				AND ExistingRequest.ArcNeeded = P.ArcNeeded
				AND CAST(ExistingRequest.AddedAt AS DATE) = @TODAY
		WHERE
			ExistingRequest.AccountNumber IS NULL --Wasnt already added today.
		;

		/*** ARC ADD PROCESSING **/
		INSERT INTO ULS..ArcAddProcessing 
		(
			ArcTypeId, 
			AccountNumber, 
			ARC, 
			ScriptId, 
			ProcessOn, 
			Comment, 
			IsReference, 
			IsEndorser, 
			ProcessingAttempts, 
			CreatedAt, 
			CreatedBy
		)
		SELECT DISTINCT
			1 AS ArcTypeId,
			P.AccountNumber,
			@ARC AS ARC,
			@ScriptId AS ScriptId,
			@NOW AS ProcessOn,
			P.ArcAddData AS Comment,
			0 AS IsReference,
			0 AS IsEndorser,
			0 AS ProcessingAttempts,
			@NOW AS CreatedAt,
			SUSER_SNAME() AS CreatedBy
		FROM
			#POP P
			LEFT JOIN ULS..ArcAddProcessing EXISTING_AAP
				ON EXISTING_AAP.AccountNumber = P.AccountNumber
				AND EXISTING_AAP.Comment = P.ArcAddData
				AND EXISTING_AAP.ARC = @ARC
				AND EXISTING_AAP.ScriptId = @ScriptId
				AND CAST(EXISTING_AAP.CreatedAt AS DATE) = @TODAY
		WHERE
			EXISTING_AAP.AccountNumber IS NULL
		;
		--select * from uls..ArcAddProcessing where arc = 'MODEM' --TEST

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