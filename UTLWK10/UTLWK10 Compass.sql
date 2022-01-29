DECLARE @CurrentDateTime DATETIME = GETDATE();
DECLARE @Today DATE = @CurrentDateTime;
DECLARE @ScriptId VARCHAR(20) = 'UTLWK10';
DECLARE @ARC VARCHAR(5) = 'FRNAD';
DECLARE @BeginningOfTime DATE = '1900-01-01';

BEGIN TRY
	BEGIN TRANSACTION

	INSERT INTO 
		ULS..ArcAddProcessing
			(
				ArcTypeId,
				ArcResponseCodeId,
				AccountNumber,
				RecipientId,
				ARC,
				ActivityType,
				ActivityContact,
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
				LN_ATY_SEQ,
				ProcessingAttempts,
				CreatedAt,
				CreatedBy,
				ProcessedAt
			)
	SELECT DISTINCT
		1 AS ArcTypeId,
		NULL AS ArcResponseCodeId,
		FOREIGN_ADD.DF_SPE_ACC_ID AS AccountNumber,
		LN10.BF_SSN AS RecipientId,
		@ARC AS ARC,
		NULL AS ActivityType,
		NULL AS ActivityContact,
		@ScriptId AS ScriptId,
		@CurrentDateTime AS ProcessOn,
		RTRIM(CONCAT('COMPASS',',','Country: ',LTRIM(FOREIGN_ADD.DM_FGN_CNY))) AS Comment,
		0 AS IsReference,
		0 AS IsEndorser,
		NULL AS ProcessFrom,
		NULL AS ProcessTo,
		NULL AS NeededBy,
		NULL AS RegardsTo,
		NULL AS RegardsCode,
		NULL AS LN_ATY_SEQ,
		0 AS ProcessingAttempts, --this gets updated by the arc add script so it is initialized as 0 attempts made
		@CurrentDateTime AS CreatedAt,
		@ScriptId AS CreatedBy,
		NULL AS ProcessedAt
	FROM
		UDW..LN10_LON LN10 --active flags in WHERE clause
		--FOREIGN_ADD: borrowers with foreign addresses
		INNER JOIN
		(	
			SELECT
				PD10.DF_SPE_ACC_ID,
				PD30.DF_PRS_ID,
				PD30.DM_FGN_CNY,
				MAX(PD30.DD_VER_ADR) AS MX_UPDT
			FROM
				UDW..PD10_PRS_NME PD10
				INNER JOIN UDW..PD30_PRS_ADR PD30
					ON PD10.DF_PRS_ID = PD30.DF_PRS_ID
			WHERE
				PD30.DI_VLD_ADR = 'Y'
				AND PD30.DC_ADR = 'L'
				AND 
				(
					PD30.DC_DOM_ST = 'FC'
					OR ISNULL(PD30.DM_FGN_ST,'') != ''
					OR ISNULL(PD30.DM_FGN_CNY,'') != ''
				)
				AND SUBSTRING(PD30.DF_PRS_ID,1,1) != 'P'
			GROUP BY
				PD10.DF_SPE_ACC_ID,
				PD30.DF_PRS_ID,
				PD30.DM_FGN_CNY
		) FOREIGN_ADD
			ON LN10.BF_SSN = FOREIGN_ADD.DF_PRS_ID
	--AY10: has MFRGN ARC
		LEFT JOIN
		(
			SELECT
				BF_SSN,
				MAX(LD_ATY_REQ_RCV) AS MFRGN_MX_DT
			FROM
				UDW..AY10_BR_LON_ATY
			WHERE
				PF_REQ_ACT = 'MFRGN'
				AND LC_STA_ACTY10 = 'A'
			GROUP BY
				BF_SSN
		) AY10
			ON AY10.BF_SSN = LN10.BF_SSN
		LEFT JOIN ULS..ArcAddProcessing ExistingData ----exclude borrowers who already have an arcadd record --TODO (FOR TESTING): uncomment
			ON FOREIGN_ADD.DF_SPE_ACC_ID = ExistingData.AccountNumber
			AND ExistingData.ARC = @ARC
			AND 
				(
					(
						CAST(ExistingData.CreatedAt AS DATE) >= FOREIGN_ADD.MX_UPDT  --to prevent duplicates from being added
						AND ExistingData.Comment = RTRIM(CONCAT('COMPASS',',','Country: ',LTRIM(FOREIGN_ADD.DM_FGN_CNY)))
					)
					OR ExistingData.ProcessedAt IS NULL
				)
	WHERE 
		LN10.LC_STA_LON10 = 'R'
		AND LN10.LA_CUR_PRI > 0.00
		AND FOREIGN_ADD.MX_UPDT > ISNULL(AY10.MFRGN_MX_DT,@BeginningOfTime) --No arc after most recent address change
		AND ExistingData.AccountNumber IS NULL --exclude borrowers who already have an arcadd record --TODO (FOR TESTING): uncomment
COMMIT TRANSACTION;

END TRY
	--write message to process logger if an error occurs
	BEGIN CATCH
		DECLARE @EM_ VARCHAR(4000) = @ScriptId + ' encountered an error.  Transaction not committed. Error: ' + (SELECT ERROR_MESSAGE());

		ROLLBACK TRANSACTION;

		DECLARE @ProcessLogId_ INT;
		DECLARE @ProcessNotificationId_ INT;
		DECLARE @NotificationTypeId_ INT = (SELECT TOP 1 NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'); --Error report
		DECLARE @NotificationSeverityTypeId_ INT = (SELECT TOP 1 NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
		
		INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(GETDATE(),GETDATE(),@ScriptId,'uheaa',SUSER_SNAME());
		SET @ProcessLogId_ = SCOPE_IDENTITY()

		INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId_,@NotificationSeverityTypeId_,@ProcessLogId_, NULL, NULL)
		SET @ProcessNotificationId_ = SCOPE_IDENTITY()

		INSERT INTO ULS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId_,@EM_);

	THROW;
END CATCH;