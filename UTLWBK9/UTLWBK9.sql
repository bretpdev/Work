--UTLWBK9 : TILP Chapter 13 Bankruptcy Review
BEGIN TRY
BEGIN TRANSACTION;

	DECLARE @ARC VARCHAR(5) = 'TLP13',
			@ArcTypeId INT = 1,
			@ScriptId VARCHAR(10) = 'UTLWBK9',
			@Comment VARCHAR(50) = 'Chapter 13 Bankruptcy Review for TILP loan(s)',
			@RegardsCode VARCHAR(1) = 'B', --borrower
			@TODAY DATE = GETDATE(),
			@NOW DATETIME = GETDATE();
	--select @ARC,@ArcTypeId,@ScriptId,@Comment,@TODAY,@NOW --test

	INSERT INTO ULS..ArcAddProcessing 
	(
		ArcTypeId,
		AccountNumber,
		RecipientId,
		ARC,
		ScriptId,
		ProcessOn,
		Comment,
		IsReference,
		IsEndorser,
		ProcessFrom,
		ProcessTo,
		NeededBy,
		RegardsTo,
		RegardsCode,
		ProcessingAttempts,
		CreatedAt,
		CreatedBy
	)
	SELECT DISTINCT
		@ArcTypeId AS ArcTypeId,
		NEW_DATA.DF_SPE_ACC_ID AS AccountNumber,
		NEW_DATA.BF_SSN AS RecipientId,
		@ARC AS ARC,
		@ScriptId AS ScriptId,
		@NOW AS ProcessOn,
		@Comment AS Comment,
		0 AS IsReference,
		0 AS IsEndorser,
		NULL AS ProcessFrom,
		NULL AS ProcessTo,
		NULL AS NeededBy,
		NEW_DATA.BF_SSN AS RegardsTo,
		@RegardsCode AS RegardsCode,
		0 AS ProcessingAttempts,
		@NOW AS CreatedAt,
		SUSER_SNAME() AS CreatedBy
	FROM 
		(
			SELECT
				PD10.DF_SPE_ACC_ID,
				LN10.BF_SSN --use borrower SSN as recipient ID because TLP13 ARC is set up as a borrower arc, so recipient SSN should match Borrower SSN
			FROM
				UDW..LN10_LON LN10
				INNER JOIN UDW..PD10_PRS_NME PD10
					ON PD10.DF_PRS_ID = LN10.BF_SSN
				INNER JOIN UDW..PD24_PRS_BKR PD24
					ON PD24.DF_PRS_ID = LN10.BF_SSN
				LEFT JOIN 
				(--exclude if TLP13 arc within last 30 days
	 				SELECT 
						AY10.BF_SSN
	 				FROM 
						UDW..AY10_BR_LON_ATY AY10
						INNER JOIN UDW..PD24_PRS_BKR PD24
							ON AY10.BF_SSN = PD24.DF_PRS_ID
	 				WHERE 
						PD24.DC_BKR_TYP = '13' --bankruptcy
						AND PD24.DC_BKR_STA = '06' --verified bankruptcy
						AND AY10.PF_REQ_ACT = @ARC
						AND AY10.LC_STA_ACTY10 = 'A'
	 					AND AY10.LD_ATY_REQ_RCV > DATEADD(DAY,-30,@TODAY)
				 ) Exclude30Day
					ON Exclude30Day.BF_SSN = LN10.BF_SSN
				LEFT JOIN
				(--exclude accounts with pending TLP13 arc
					SELECT
						BF_SSN
					FROM
						UDW..AY10_BR_LON_ATY
					WHERE
						PF_REQ_ACT = @ARC
						AND PF_RSP_ACT IS NULL --pending
						AND LC_STA_ACTY10 = 'A'
				) PendingArc
					ON PendingArc.BF_SSN = LN10.BF_SSN
			WHERE 
				LN10.LA_CUR_PRI > 0.00
				AND LN10.LC_STA_LON10 = 'R'
				AND LN10.IC_LON_PGM = 'TILP'
				AND PD24.DC_BKR_STA = '06' --verified bankruptcy
				AND PD24.DC_BKR_TYP = '13' --bankruptcy
				AND Exclude30Day.BF_SSN IS NULL
				AND PendingArc.BF_SSN IS NULL
		) NEW_DATA
		LEFT JOIN ULS..ArcAddProcessing EXISTING_DATA
			ON  EXISTING_DATA.AccountNumber = NEW_DATA.DF_SPE_ACC_ID
			AND EXISTING_DATA.RecipientId	= NEW_DATA.BF_SSN
			AND EXISTING_DATA.RegardsTo		= NEW_DATA.BF_SSN
			AND EXISTING_DATA.ArcTypeId		= @ArcTypeId
			AND EXISTING_DATA.ARC			= @ARC
			AND EXISTING_DATA.ScriptId		= @ScriptId
			AND EXISTING_DATA.Comment		= @Comment		
			AND EXISTING_DATA.RegardsCode	= @RegardsCode
			AND CAST(EXISTING_DATA.CreatedAt AS DATE) = @TODAY --prevents multiple arcs from being added on the same day
	WHERE
		EXISTING_DATA.AccountNumber IS NULL
	;
	--select * from uls..ArcAddProcessing where ScriptId = 'UTLWBK9'--TEST

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
