BEGIN TRY
	BEGIN TRAN

		DECLARE @AddedAt DATETIME = GETDATE();
		DECLARE @Today DATE = @AddedAt,
				@QueName VARCHAR(10) = 'LRV4LAWG',
				@ScriptId VARCHAR(10) = 'UTLWGG7',
				@CT30Queue VARCHAR(50) = 'LRV4LAWG';

		INSERT INTO OLS.olqtskbldr.Queues
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
			POP.BF_SSN AS TargetId,
			@QueName AS QueueName,
			'' AS InstitutionId,
			'' AS InstitutionType,
			NULL AS DateDue,
			NULL AS TimeDue,
			'' AS Comment,
			'' AS SourceFilename,
			@AddedAt AS AddedAt,
			@ScriptId AS AddedBy
		FROM
			(
				SELECT
					DC01.BF_SSN,
					IIF(DC01.LC_AUX_STA NOT IN ('O5'), 1, 0) AS DC01_FLAG, --keep if true
					IIF(LA11.BC_LEG_ACT_ATY_TYP NOT IN ('EX'), 1, 0) AS LA11_FLAG --keep if true
				FROM 
					ODW..DC01_LON_CLM_INF DC01
					INNER JOIN ODW..GA14_LON_STA GA14
						ON GA14.AF_APL_ID = DC01.AF_APL_ID
					INNER JOIN ODW..LA11_LEG_ACT_ATY LA11
						ON LA11.DF_PRS_ID_BR = DC01.BF_SSN
					INNER JOIN ODW..GA10_LON_APP GA10
						ON GA10.AF_APL_ID = DC01.AF_APL_ID
				WHERE
					GA14.AC_LON_STA_TYP = 'CP'
					AND GA14.AC_STA_GA14 = 'A' 
					AND	LA11.BC_LEG_ACT_ATY_TYP = 'JD'
					AND	DC01.LC_PCL_REA IN ('DB', 'DQ', 'DF')
					AND	DC01.LC_REA_CLM_ASN_DOE IS NULL
					AND GA10.AA_CUR_PRI > 50.00
			) POP
			LEFT JOIN ODW..CT30_CALL_QUE CT30
				ON CT30.DF_PRS_ID_BR = POP.BF_SSN
				AND CT30.IF_WRK_GRP = @CT30Queue
				AND CT30.IC_TSK_STA IN ('A','W')
			--check for existing record to add queue task for the current date
			LEFT JOIN OLS.olqtskbldr.Queues ExistingData
				ON ExistingData.TargetId = POP.BF_SSN
				AND ExistingData.QueueName = @QueName
				AND 
				(
					CAST(ExistingData.AddedAt AS DATE) = @Today
					OR ExistingData.ProcessedAt IS NULL
				)
				AND ExistingData.DeletedAt IS NULL
				AND ExistingData.DeletedBy IS NULL
		WHERE
			POP.DC01_FLAG = 1
			AND POP.LA11_FLAG = 1
			AND ExistingData.TargetId IS NULL --record to create queue task does already exist for the current date
			AND CT30.DF_PRS_ID_BR IS NULL
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