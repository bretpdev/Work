--DEMOCHGUH: EMAIL CHANGE UHEAA
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

		--borrowers: PD32 history comparison
		SELECT
			PD10.DF_SPE_ACC_ID AS AccountNumber,
			CONCAT(--email data:
					CAST(PD10.DF_SPE_ACC_ID AS VARCHAR(10)) 
					,',',COALESCE(LTRIM(RTRIM(PD10.DM_PRS_1)),'') 
					,',',EMAIL.EmailAddress,
					CONCAT(',Demo Change Notice sent to Borrower: ',
							CASE
								WHEN PD32_History.DX_ADR_EML != EMAIL.EmailAddress
								THEN CONCAT('Email Address changed from "',PD32_History.DX_ADR_EML,'" to "',EMAIL.EmailAddress,'" ')
								ELSE NULL
							END
						)
				) AS EmailData,
			CONCAT('Demo Change Notice sent to Borrower: ',
					CASE
						WHEN PD32_History.DX_ADR_EML != EMAIL.EmailAddress
						THEN CONCAT('Email Address changed from "',PD32_History.DX_ADR_EML,'" to "',EMAIL.EmailAddress,'" by ',PD32_History.DF_LST_USR_PD32)
						ELSE NULL
					END
				) AS ArcAddData,
			IIF(EA.Arc IS NOT NULL, 1, 0) AS ArcNeeded
		INTO
			#POP
		FROM
			UDW..PD10_PRS_NME PD10
			INNER JOIN UDW..LN10_LON LN10
				ON LN10.BF_SSN = PD10.DF_PRS_ID
			INNER JOIN UDW.calc.EmailAddress EMAIL
				ON EMAIL.DF_PRS_ID = PD10.DF_PRS_ID
			INNER JOIN UDW..PD32_PRS_ADR_EML PD32_History
				ON PD32_History.DF_PRS_ID = EMAIL.DF_PRS_ID
				AND PD32_History.DX_ADR_EML != EMAIL.EmailAddress
				AND PD32_History.DC_STA_PD32 = 'H' --history
				AND CAST(PD32_History.DD_STA_PD32 AS DATE) BETWEEN @PREVIOUSWORKDAY AND @YESTERDAY
			LEFT JOIN ULS.emailbatch.EmailCampaigns EC
				ON EC.EmailCampaignId = @EmailCampaignId
				AND EC.DeletedAt IS NULL
			LEFT JOIN ULS.emailbatch.Arcs EA
				ON EA.ArcId = EC.ArcId
		WHERE
			PD10.DF_PRS_ID LIKE '[0-9]%'
			AND LN10.LC_STA_LON10 = 'R'
			AND LN10.LA_CUR_PRI > 0.00

		UNION ALL

		--borrowers: PH06 comparison
		SELECT
			PD10.DF_SPE_ACC_ID AS AccountNumber,
			CONCAT(--email data:
					CAST(PD10.DF_SPE_ACC_ID AS VARCHAR(10)) 
					,',',COALESCE(LTRIM(RTRIM(PD10.DM_PRS_1)),'') 
					,',',EMAIL.EmailAddress,
					CONCAT(',Demo Change Notice sent to Borrower: ',
							CASE
								WHEN PH06.HX_CNC_EML_ADR != EMAIL.EmailAddress
								THEN CONCAT('Email Address changed from "',PH06.HX_CNC_EML_ADR,'" to "',EMAIL.EmailAddress,'" ')
								ELSE NULL
							END
						)
				) AS EmailData,
			CONCAT('Demo Change Notice sent to Borrower: ',
					CASE
						WHEN PH06.HX_CNC_EML_ADR != EMAIL.EmailAddress
						THEN CONCAT('Email Address changed from "',PH06.HX_CNC_EML_ADR,'" to "',EMAIL.EmailAddress,'" by ',PH06.HF_LST_USR_PH05)
						ELSE NULL
					END
				) AS ArcAddData,
			IIF(EA.Arc IS NOT NULL, 1, 0) AS ArcNeeded
		FROM
			UDW..PD10_PRS_NME PD10
			INNER JOIN UDW..LN10_LON LN10
				ON LN10.BF_SSN = PD10.DF_PRS_ID
			INNER JOIN UDW.calc.EmailAddress EMAIL
				ON EMAIL.DF_PRS_ID = PD10.DF_PRS_ID
			INNER JOIN UDW..PH06_CNC_EML_HST PH06
				ON PH06.HF_SPE_ID = PD10.DF_SPE_ACC_ID
				AND PH06.HX_CNC_EML_ADR != EMAIL.EmailAddress
				AND CAST(PH06.DF_CRT_DTS_PH06 AS DATE) BETWEEN @PREVIOUSWORKDAY AND @YESTERDAY
			LEFT JOIN ULS.emailbatch.EmailCampaigns EC
				ON EC.EmailCampaignId = @EmailCampaignId
				AND EC.DeletedAt IS NULL
			LEFT JOIN ULS.emailbatch.Arcs EA
				ON EA.ArcId = EC.ArcId
		WHERE
			PD10.DF_PRS_ID LIKE '[0-9]%'
			AND LN10.LC_STA_LON10 = 'R'
			AND LN10.LA_CUR_PRI > 0.00

		UNION ALL

		--coborrowers: PD32 history comparison
		SELECT
			PD10.DF_SPE_ACC_ID AS AccountNumber,
			CONCAT(--email data:
					CAST(PD10E.DF_SPE_ACC_ID AS VARCHAR(10)) 
					,',',COALESCE(LTRIM(RTRIM(PD10E.DM_PRS_1)),'') 
					,',',EMAIL.EmailAddress,
					CONCAT(',Demo Change Notice sent to Co-Borrower: ',
							CASE
								WHEN PD32_History.DX_ADR_EML != EMAIL.EmailAddress
								THEN CONCAT('Email Address changed from "',PD32_History.DX_ADR_EML,'" to "',EMAIL.EmailAddress,'" ')
								ELSE NULL
							END
						)
				) AS EmailData,
			CONCAT('Demo Change Notice sent to Co-Borrower: ',
					CASE
						WHEN PD32_History.DX_ADR_EML != EMAIL.EmailAddress
						THEN CONCAT('Email Address changed from "',PD32_History.DX_ADR_EML,'" to "',EMAIL.EmailAddress,'" by ',PD32_History.DF_LST_USR_PD32)
						ELSE NULL
					END
				) AS ArcAddData,
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
			INNER JOIN UDW..PD32_PRS_ADR_EML PD32_History
				ON PD32_History.DF_PRS_ID = EMAIL.DF_PRS_ID
				AND PD32_History.DX_ADR_EML != EMAIL.EmailAddress
				AND CAST(PD32_History.DD_STA_PD32 AS DATE) BETWEEN @PREVIOUSWORKDAY AND @YESTERDAY
				AND PD32_History.DC_STA_PD32 = 'H' --history
			LEFT JOIN ULS.emailbatch.EmailCampaigns EC
				ON EC.EmailCampaignId = @EmailCampaignId
				AND EC.DeletedAt IS NULL
			LEFT JOIN ULS.emailbatch.Arcs EA
				ON EA.ArcId = EC.ArcId
		WHERE
			PD10E.DF_PRS_ID LIKE '[0-9]%'
			AND LN10.LC_STA_LON10 = 'R'
			AND LN10.LA_CUR_PRI > 0.00
			AND LN20.LC_EDS_TYP = 'M'
			AND LN20.LC_STA_LON20 = 'A'

		UNION ALL

		--coborrower: PH06 comparison
		SELECT
			PD10.DF_SPE_ACC_ID AS AccountNumber,
			CONCAT(--email data:
					CAST(PD10E.DF_SPE_ACC_ID AS VARCHAR(10)) 
					,',',COALESCE(LTRIM(RTRIM(PD10E.DM_PRS_1)),'') 
					,',',EMAIL.EmailAddress,
					CONCAT(',Demo Change Notice sent to Co-Borrower: ',
							CASE
								WHEN PH06.HX_CNC_EML_ADR != EMAIL.EmailAddress
								THEN CONCAT('Email Address changed from "',PH06.HX_CNC_EML_ADR,'" to "',EMAIL.EmailAddress,'" ')
								ELSE NULL
							END
						)
				) AS EmailData,
			CONCAT('Demo Change Notice sent to Co-Borrower: ',
					CASE
						WHEN PH06.HX_CNC_EML_ADR != EMAIL.EmailAddress
						THEN CONCAT('Email Address changed from "',PH06.HX_CNC_EML_ADR,'" to "',EMAIL.EmailAddress,'" by ',PH06.HF_LST_USR_PH05)
						ELSE NULL
					END
				) AS ArcAddData,
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
			INNER JOIN UDW..PH06_CNC_EML_HST PH06
				ON PH06.HF_SPE_ID = PD10E.DF_SPE_ACC_ID
				AND PH06.HX_CNC_EML_ADR != EMAIL.EmailAddress
				AND CAST(PH06.DF_CRT_DTS_PH06 AS DATE) BETWEEN @PREVIOUSWORKDAY AND @YESTERDAY
			LEFT JOIN ULS.emailbatch.EmailCampaigns EC
				ON EC.EmailCampaignId = @EmailCampaignId
				AND EC.DeletedAt IS NULL
			LEFT JOIN ULS.emailbatch.Arcs EA
				ON EA.ArcId = EC.ArcId
		WHERE
			PD10E.DF_PRS_ID LIKE '[0-9]%'
			AND LN10.LC_STA_LON10 = 'R'
			AND LN10.LA_CUR_PRI > 0.00
			AND LN20.LC_EDS_TYP = 'M'
			AND LN20.LC_STA_LON20 = 'A'
		;
		--select * from #POP; --TEST

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