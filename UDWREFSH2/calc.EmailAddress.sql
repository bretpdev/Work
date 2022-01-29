USE UDW;
GO

BEGIN TRY
	
	DECLARE @ScriptId VARCHAR(30) = 'UDWREFSH2';
	
	BEGIN TRANSACTION
		
		EXEC TruncateTable'UDW.calc.EmailAddress_STAGE'; --not a historic table; clear out table each run

		--insert into staging table first
		INSERT INTO calc.EmailAddress_STAGE
		(
			DF_PRS_ID,
			EmailAddress
		)
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
							PD10_PRS_NME PD10
							LEFT JOIN PH05_CNC_EML PH05
								ON PH05.DF_SPE_ID = PD10.DF_SPE_ACC_ID
								AND PH05.DI_VLD_CNC_EML_ADR = 'Y' --valid email address
							LEFT JOIN PD32_PRS_ADR_EML PD32
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
		;
	COMMIT TRANSACTION

	BEGIN TRANSACTION
		--insert from staging to live
		EXEC TruncateTable'UDW.calc.EmailAddress'; --not a historic table; clear out table each run

		INSERT INTO calc.EmailAddress
		(
			DF_PRS_ID,
			EmailAddress
		)
		SELECT
			DF_PRS_ID,
			EmailAddress
		FROM
			calc.EmailAddress_STAGE
		;

		EXEC TruncateTable'UDW.calc.EmailAddress_STAGE'; --not a historic table; clear out table each run

	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	DECLARE @EM VARCHAR(4000) = @ScriptId + ' encountered an error.  Transaction not committed. Error: ' + (SELECT ERROR_MESSAGE());

	ROLLBACK TRANSACTION;

	DECLARE @ProcessLogId INT;
	DECLARE @ProcessNotificationId INT;
	DECLARE @NotificationTypeId INT = (SELECT TOP 1 NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'); --Error report
	DECLARE @NotificationSeverityTypeId INT = (SELECT TOP 1 NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
		
	INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(GETDATE(),GETDATE(),@ScriptId,'uheaa',SUSER_SNAME());
	SET @ProcessLogId = SCOPE_IDENTITY()

	INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId,@NotificationSeverityTypeId,@ProcessLogId, NULL, NULL)
	SET @ProcessNotificationId = SCOPE_IDENTITY()

	INSERT INTO ULS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId,@EM);

	THROW;
END CATCH;