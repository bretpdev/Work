BEGIN TRY
	BEGIN TRANSACTION

		DECLARE @AddedAt DATETIME = GETDATE();
		DECLARE @Today DATE = @AddedAt;
		DECLARE @QueName VARCHAR(10) = 'LINKING';
		DECLARE @ScriptId VARCHAR(10) = 'UTLWGG8';

		INSERT INTO OLS.olqtskbldr.Queues --TODO: restore insert for production
			(
				TargetId,
				QueueName,
				InstitutionId,
				InstitutionType,
				DateDue,
				TimeDue,
				Comment,
				SourceFilename,
				AddedAt,
				AddedBy
			)
		SELECT DISTINCT
			SD02.DF_PRS_ID_STU	AS TargetId,
			@QueName	AS QueueName,
			''			AS InstitutionId,
			''			AS InstitutionType,
			NULL		AS DateDue,
			NULL		AS TimeDue,
			''			AS Comment,
			''			AS SourceFilename,
			@AddedAt	AS AddedAt,
			@ScriptId	AS AddedBy
		FROM 
			ODW..SD02_STU_ENR SD02
			INNER JOIN ODW..GA01_APP GA01
				ON GA01.DF_PRS_ID_BR = SD02.DF_PRS_ID_STU
			INNER JOIN ODW..GA10_LON_APP GA10
				ON GA10.AF_APL_ID = GA01.AF_APL_ID
			LEFT JOIN ODW..PD01_PDM_INF PD01
				ON PD01.DF_PRS_ID = SD02.DF_PRS_ID_STU
		--LINKING:has a LINKING queue task
			LEFT JOIN
			(
				SELECT 
					CT30.DF_PRS_ID_BR
				FROM  
					ODW..CT30_CALL_QUE CT30
				WHERE
					CT30.IF_WRK_GRP = 'LINKING'
			) LINKING
				ON LINKING.DF_PRS_ID_BR = SD02.DF_PRS_ID_STU
		--check for existing record to add queue task for the current date
			LEFT JOIN  OLS.olqtskbldr.Queues ExistingData
				ON ExistingData.TargetId = SD02.DF_PRS_ID_STU
				AND ExistingData.QueueName = @QueName
				AND CAST(ExistingData.AddedAt AS DATE) = @Today
				AND ExistingData.DeletedAt IS NULL
				AND ExistingData.DeletedBy IS NULL
		WHERE
			CAST(SD02.LF_LST_DTS_SD02 AS DATE) >= CAST(DATEADD(DAY, -1, @Today) AS DATE)
			AND SD02.LC_STA_SD02 = 'A'
			AND GA10.AF_CUR_LON_OPS_LDR IN ('826717','829769','830248') --Nelnet
			AND LINKING.DF_PRS_ID_BR IS NULL --doesn't already have a LINKING queue task
			AND ExistingData.TargetId IS NULL --record to create queue task does already exist for the current date
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
		
	INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(@AddedAt,@AddedAt,@ScriptId,'uheaa',SUSER_SNAME());
	SET @ProcessLogId = SCOPE_IDENTITY()

	INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId,@NotificationSeverityTypeId,@ProcessLogId, NULL, NULL)
	SET @ProcessNotificationId = SCOPE_IDENTITY()

	INSERT INTO ULS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId,@EM);

	THROW;
END CATCH;