--DEMOCHGUH: ADDRESS CHANGE UHEAA
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
			CONCAT(--email data:
					CAST(PD10.DF_SPE_ACC_ID AS VARCHAR(10))
					,',',COALESCE(LTRIM(RTRIM(PD10.DM_PRS_1)),'')
					,',',EMAIL.EmailAddress,
					CONCAT(',Seq#',PD31.DN_ADR_SEQ,' - Demo Change Notice sent to Borrower: ',
							CASE WHEN PD31.DX_STR_ADR_1_HST != PD30.DX_STR_ADR_1 THEN CONCAT('Address Line 1 changed from "',RTRIM(PD31.DX_STR_ADR_1_HST),'" to "',RTRIM(PD30.DX_STR_ADR_1),'" ') END,
							CASE WHEN PD31.DX_STR_ADR_2_HST != PD30.DX_STR_ADR_2 THEN CONCAT('Address Line 2 changed from "',RTRIM(PD31.DX_STR_ADR_2_HST),'" to "',RTRIM(PD30.DX_STR_ADR_2),'" ') END,
							CASE WHEN PD31.DX_STR_ADR_3_HST != PD30.DX_STR_ADR_3 THEN CONCAT('Address Line 3 changed from "',RTRIM(PD31.DX_STR_ADR_3_HST),'" to "',RTRIM(PD30.DX_STR_ADR_3),'" ') END,
							CASE WHEN PD31.DM_CT_HST != PD30.DM_CT				 THEN CONCAT('City changed from "',RTRIM(PD31.DM_CT_HST),'" to "',RTRIM(PD30.DM_CT),'" ') END,
							CASE WHEN PD31.DC_DOM_ST_HST != PD30.DC_DOM_ST		 THEN CONCAT('State changed from "',PD31.DC_DOM_ST_HST,'" to "',PD30.DC_DOM_ST,'" ') END,
							CASE WHEN LEFT(PD31.DF_ZIP_CDE_HST, 5) != LEFT(PD30.DF_ZIP_CDE, 5) THEN CONCAT('Zip Code changed from "',LEFT(PD31.DF_ZIP_CDE_HST, 5),'" to "',LEFT(PD30.DF_ZIP_CDE, 5),'" ') END,
							CASE WHEN PD31.DM_FGN_ST_HST != PD30.DM_FGN_ST		 THEN CONCAT('Foreign State changed from "',RTRIM(PD31.DM_FGN_ST_HST),'" to "',RTRIM(PD30.DM_FGN_ST),'" ') END,
							CASE WHEN PD31.DM_FGN_CNY_HST != PD30.DM_FGN_CNY	 THEN CONCAT('Foreign Country changed from "',RTRIM(PD31.DM_FGN_CNY_HST),'" to "',RTRIM(PD30.DM_FGN_CNY),'" ') END
						)
				) AS EmailData,
			CONCAT('Seq#',PD31.DN_ADR_SEQ,' - Demo Change Notice sent to Borrower: ',
					CASE WHEN PD31.DX_STR_ADR_1_HST != PD30.DX_STR_ADR_1 THEN CONCAT('Address Line 1 changed from "',RTRIM(PD31.DX_STR_ADR_1_HST),'" to "',RTRIM(PD30.DX_STR_ADR_1),'" ') END,
					CASE WHEN PD31.DX_STR_ADR_2_HST != PD30.DX_STR_ADR_2 THEN CONCAT('Address Line 2 changed from "',RTRIM(PD31.DX_STR_ADR_2_HST),'" to "',RTRIM(PD30.DX_STR_ADR_2),'" ') END,
					CASE WHEN PD31.DX_STR_ADR_3_HST != PD30.DX_STR_ADR_3 THEN CONCAT('Address Line 3 changed from "',RTRIM(PD31.DX_STR_ADR_3_HST),'" to "',RTRIM(PD30.DX_STR_ADR_3),'" ') END,
					CASE WHEN PD31.DM_CT_HST != PD30.DM_CT				 THEN CONCAT('City changed from "',RTRIM(PD31.DM_CT_HST),'" to "',RTRIM(PD30.DM_CT),'" ') END,
					CASE WHEN PD31.DC_DOM_ST_HST != PD30.DC_DOM_ST		 THEN CONCAT('State changed from "',PD31.DC_DOM_ST_HST,'" to "',PD30.DC_DOM_ST,'" ') END,
					CASE WHEN LEFT(PD31.DF_ZIP_CDE_HST, 5) != LEFT(PD30.DF_ZIP_CDE, 5) THEN CONCAT('Zip Code changed from "',LEFT(PD31.DF_ZIP_CDE_HST, 5),'" to "',LEFT(PD30.DF_ZIP_CDE, 5),'" ') END,
					CASE WHEN PD31.DM_FGN_ST_HST != PD30.DM_FGN_ST		 THEN CONCAT('Foreign State changed from "',RTRIM(PD31.DM_FGN_ST_HST),'" to "',RTRIM(PD30.DM_FGN_ST),'" ') END,
					CASE WHEN PD31.DM_FGN_CNY_HST != PD30.DM_FGN_CNY	 THEN CONCAT('Foreign Country changed from "',RTRIM(PD31.DM_FGN_CNY_HST),'" to "',RTRIM(PD30.DM_FGN_CNY),'" ') END,
					CASE WHEN  PD31.DX_STR_ADR_1_HST != PD30.DX_STR_ADR_1
							OR PD31.DX_STR_ADR_2_HST != PD30.DX_STR_ADR_2
							OR PD31.DX_STR_ADR_3_HST != PD30.DX_STR_ADR_3
							OR PD31.DM_CT_HST != PD30.DM_CT
							OR PD31.DC_DOM_ST_HST != PD30.DC_DOM_ST
							OR LEFT(PD31.DF_ZIP_CDE_HST, 5) != LEFT(PD30.DF_ZIP_CDE, 5)
							OR PD31.DM_FGN_ST_HST != PD30.DM_FGN_ST
							OR PD31.DM_FGN_CNY_HST != PD30.DM_FGN_CNY
						THEN CONCAT('Updated by ', PD30.DF_LST_USR_PD30) END
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
			INNER JOIN UDW..PD30_PRS_ADR PD30
				ON PD10.DF_PRS_ID = PD30.DF_PRS_ID
			INNER JOIN
			(
				SELECT
					DF_PRS_ID,
					DC_ADR_HST,
					DN_ADR_SEQ,
					DX_STR_ADR_1_HST,
					DX_STR_ADR_2_HST,
					DX_STR_ADR_3_HST,
					DM_CT_HST,
					DC_DOM_ST_HST,
					DF_ZIP_CDE_HST,
					DM_FGN_ST_HST,
					DM_FGN_CNY_HST,
					DD_CRT_PD31
				FROM
					UDW..PD31_PRS_INA
				WHERE
					DF_PRS_ID LIKE '[0-9]%'
					AND DD_CRT_PD31 BETWEEN @PREVIOUSWORKDAY AND @YESTERDAY
			) PD31
				ON PD31.DF_PRS_ID = PD30.DF_PRS_ID
				AND PD31.DC_ADR_HST = PD30.DC_ADR
				AND (--non-matching addresses
						PD31.DX_STR_ADR_1_HST != PD30.DX_STR_ADR_1
						OR PD31.DX_STR_ADR_2_HST != PD30.DX_STR_ADR_2
						OR PD31.DX_STR_ADR_3_HST != PD30.DX_STR_ADR_3
						OR PD31.DM_CT_HST != PD30.DM_CT
						OR PD31.DC_DOM_ST_HST != PD30.DC_DOM_ST
						OR LEFT(PD31.DF_ZIP_CDE_HST, 5) != LEFT(PD30.DF_ZIP_CDE, 5)
						OR PD31.DM_FGN_ST_HST != PD30.DM_FGN_ST
						OR PD31.DM_FGN_CNY_HST != PD30.DM_FGN_CNY
					)
			LEFT JOIN ULS.emailbatch.EmailCampaigns EC
				ON EC.EmailCampaignId = @EmailCampaignId
				AND EC.DeletedAt IS NULL
			LEFT JOIN ULS.emailbatch.Arcs EA
				ON EA.ArcId = EC.ArcId
		WHERE
			CAST(PD30.DF_LST_DTS_PD30 AS DATE) BETWEEN @PREVIOUSWORKDAY AND @YESTERDAY
			AND PD10.DF_PRS_ID LIKE '[0-9]%'
			AND LN10.LC_STA_LON10 = 'R'
			AND LN10.LA_CUR_PRI > 0.00
			AND PD30.DC_ADR = 'L' --legal --TODO: BA NEEDS TO VERIFY IF THIS ACTIVE FLAG IS NEEDED

		UNION ALL

		--coborrowers:
		SELECT
			PD10.DF_SPE_ACC_ID AS AccountNumber,
			CONCAT(--email data:
					CAST(PD10E.DF_SPE_ACC_ID AS VARCHAR(10))
					,',',COALESCE(LTRIM(RTRIM(PD10E.DM_PRS_1)),'')
					,',',EMAIL.EmailAddress,
					CONCAT(',Seq#',PD31.DN_ADR_SEQ,' - Demo Change Notice sent to Co-Borrower: ',
							CASE WHEN PD31.DX_STR_ADR_1_HST != PD30.DX_STR_ADR_1 THEN CONCAT('Address Line 1 changed from "',RTRIM(PD31.DX_STR_ADR_1_HST),'" to "',RTRIM(PD30.DX_STR_ADR_1),'" ') END,
							CASE WHEN PD31.DX_STR_ADR_2_HST != PD30.DX_STR_ADR_2 THEN CONCAT('Address Line 2 changed from "',RTRIM(PD31.DX_STR_ADR_2_HST),'" to "',RTRIM(PD30.DX_STR_ADR_2),'" ') END,
							CASE WHEN PD31.DX_STR_ADR_3_HST != PD30.DX_STR_ADR_3 THEN CONCAT('Address Line 3 changed from "',RTRIM(PD31.DX_STR_ADR_3_HST),'" to "',RTRIM(PD30.DX_STR_ADR_3),'" ') END,
							CASE WHEN PD31.DM_CT_HST != PD30.DM_CT				 THEN CONCAT('City changed from "',RTRIM(PD31.DM_CT_HST),'" to "',RTRIM(PD30.DM_CT),'" ') END,
							CASE WHEN PD31.DC_DOM_ST_HST != PD30.DC_DOM_ST		 THEN CONCAT('State changed from "',PD31.DC_DOM_ST_HST,'" to "',PD30.DC_DOM_ST,'" ') END,
							CASE WHEN LEFT(PD31.DF_ZIP_CDE_HST, 5) != LEFT(PD30.DF_ZIP_CDE, 5) THEN CONCAT('Zip Code changed from "',LEFT(PD31.DF_ZIP_CDE_HST, 5),'" to "',LEFT(PD30.DF_ZIP_CDE, 5),'" ') END,
							CASE WHEN PD31.DM_FGN_ST_HST != PD30.DM_FGN_ST		 THEN CONCAT('Foreign State changed from "',RTRIM(PD31.DM_FGN_ST_HST),'" to "',RTRIM(PD30.DM_FGN_ST),'" ') END,
							CASE WHEN PD31.DM_FGN_CNY_HST != PD30.DM_FGN_CNY	 THEN CONCAT('Foreign Country changed from "',RTRIM(PD31.DM_FGN_CNY_HST),'" to "',RTRIM(PD30.DM_FGN_CNY),'" ') END
						)
				) AS EmailData,
			CONCAT('Seq#',PD31.DN_ADR_SEQ,' - Demo Change Notice sent to Co-Borrower: ',
					CASE WHEN PD31.DX_STR_ADR_1_HST != PD30.DX_STR_ADR_1 THEN CONCAT('Address Line 1 changed from "',RTRIM(PD31.DX_STR_ADR_1_HST),'" to "',RTRIM(PD30.DX_STR_ADR_1),'" ') END,
					CASE WHEN PD31.DX_STR_ADR_2_HST != PD30.DX_STR_ADR_2 THEN CONCAT('Address Line 2 changed from "',RTRIM(PD31.DX_STR_ADR_2_HST),'" to "',RTRIM(PD30.DX_STR_ADR_2),'" ') END,
					CASE WHEN PD31.DX_STR_ADR_3_HST != PD30.DX_STR_ADR_3 THEN CONCAT('Address Line 3 changed from "',RTRIM(PD31.DX_STR_ADR_3_HST),'" to "',RTRIM(PD30.DX_STR_ADR_3),'" ') END,
					CASE WHEN PD31.DM_CT_HST != PD30.DM_CT				 THEN CONCAT('City changed from "',RTRIM(PD31.DM_CT_HST),'" to "',RTRIM(PD30.DM_CT),'" ') END,
					CASE WHEN PD31.DC_DOM_ST_HST != PD30.DC_DOM_ST		 THEN CONCAT('State changed from "',PD31.DC_DOM_ST_HST,'" to "',PD30.DC_DOM_ST,'" ') END,
					CASE WHEN LEFT(PD31.DF_ZIP_CDE_HST, 5) != LEFT(PD30.DF_ZIP_CDE, 5) THEN CONCAT('Zip Code changed from "',LEFT(PD31.DF_ZIP_CDE_HST, 5),'" to "',LEFT(PD30.DF_ZIP_CDE, 5),'" ') END,
					CASE WHEN PD31.DM_FGN_ST_HST != PD30.DM_FGN_ST		 THEN CONCAT('Foreign State changed from "',RTRIM(PD31.DM_FGN_ST_HST),'" to "',RTRIM(PD30.DM_FGN_ST),'" ') END,
					CASE WHEN PD31.DM_FGN_CNY_HST != PD30.DM_FGN_CNY	 THEN CONCAT('Foreign Country changed from "',RTRIM(PD31.DM_FGN_CNY_HST),'" to "',RTRIM(PD30.DM_FGN_CNY),'" ') END,
					CASE WHEN  PD31.DX_STR_ADR_1_HST != PD30.DX_STR_ADR_1
							OR PD31.DX_STR_ADR_2_HST != PD30.DX_STR_ADR_2
							OR PD31.DX_STR_ADR_3_HST != PD30.DX_STR_ADR_3
							OR PD31.DM_CT_HST != PD30.DM_CT
							OR PD31.DC_DOM_ST_HST != PD30.DC_DOM_ST
							OR LEFT(PD31.DF_ZIP_CDE_HST, 5) != LEFT(PD30.DF_ZIP_CDE, 5)
							OR PD31.DM_FGN_ST_HST != PD30.DM_FGN_ST
							OR PD31.DM_FGN_CNY_HST != PD30.DM_FGN_CNY
						THEN CONCAT('Updated by ', PD30.DF_LST_USR_PD30) END
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
			INNER JOIN UDW..PD30_PRS_ADR PD30
				ON PD30.DF_PRS_ID = PD10E.DF_PRS_ID
			INNER JOIN
			(
				SELECT
					DF_PRS_ID,
					DC_ADR_HST,
					DN_ADR_SEQ,
					DX_STR_ADR_1_HST,
					DX_STR_ADR_2_HST,
					DX_STR_ADR_3_HST,
					DM_CT_HST,
					DC_DOM_ST_HST,
					DF_ZIP_CDE_HST,
					DM_FGN_ST_HST,
					DM_FGN_CNY_HST,
					DD_CRT_PD31
				FROM
					UDW..PD31_PRS_INA 
				WHERE
					DD_CRT_PD31 BETWEEN @PREVIOUSWORKDAY AND @YESTERDAY
					AND DF_PRS_ID LIKE '[0-9]%'
			) PD31
				ON PD31.DF_PRS_ID = PD30.DF_PRS_ID
				AND PD31.DC_ADR_HST = PD30.DC_ADR
				AND (--non-matching addresses
						PD31.DX_STR_ADR_1_HST != PD30.DX_STR_ADR_1
						OR PD31.DX_STR_ADR_2_HST != PD30.DX_STR_ADR_2
						OR PD31.DX_STR_ADR_3_HST != PD30.DX_STR_ADR_3
						OR PD31.DM_CT_HST != PD30.DM_CT
						OR PD31.DC_DOM_ST_HST != PD30.DC_DOM_ST
						OR LEFT(PD31.DF_ZIP_CDE_HST, 5) != LEFT(PD30.DF_ZIP_CDE, 5)
						OR PD31.DM_FGN_ST_HST != PD30.DM_FGN_ST
						OR PD31.DM_FGN_CNY_HST != PD30.DM_FGN_CNY
					)
			LEFT JOIN ULS.emailbatch.EmailCampaigns EC
				ON EC.EmailCampaignId = @EmailCampaignId
				AND EC.DeletedAt IS NULL
			LEFT JOIN ULS.emailbatch.Arcs EA
				ON EA.ArcId = EC.ArcId
		WHERE
			CAST(PD30.DF_LST_DTS_PD30 AS DATE) BETWEEN @PREVIOUSWORKDAY AND @YESTERDAY
			AND PD10E.DF_PRS_ID LIKE '[0-9]%'
			AND LN10.LC_STA_LON10 = 'R'
			AND LN10.LA_CUR_PRI > 0.00
			AND LN20.LC_EDS_TYP = 'M'
			AND LN20.LC_STA_LON20 = 'A'
			AND PD30.DC_ADR = 'L' --legal --TODO: BA NEEDS TO VERIFY IF THIS ACTIVE FLAG IS NEEDED
		;
		--select DISTINCT * from #POP; --TEST

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