BEGIN TRY
	BEGIN TRANSACTION

		DECLARE @AddedAt DATETIME = GETDATE();
		DECLARE @TODAY DATE = @AddedAt;
		DECLARE @ScriptId VARCHAR(10) = 'UTLWGG9';
		DECLARE @QueName VARCHAR(10) = 'AWG5PMTS';
		DECLARE @Comment VARCHAR(200) = 'Review account to see if borrower has made 5 voluntary payments for rehab and if eligible for AWG release.';

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
			DC01.BF_SSN	AS TargetId,
			@QueName	AS QueueName,
			NULL		AS InstitutionId,
			NULL		AS InstitutionType,
			NULL		AS DateDue,
			NULL		AS TimeDue,
			@Comment	AS Comment,
			NULL		AS SourceFilename,
			@AddedAt	AS AddedAt,
			@ScriptId	AS AddedBy
		FROM 
			ODW..DC01_LON_CLM_INF DC01
			INNER JOIN ODW..BR01_BR_CRF BR01
				ON BR01.BF_SSN = DC01.BF_SSN
			LEFT JOIN ODW..AY01_BR_ATY DWGRL
				ON DWGRL.DF_PRS_ID = DC01.BF_SSN
				AND DWGRL.PF_ACT = 'DWGRL'
			LEFT JOIN ODW..AY01_BR_ATY DWGRB
				ON DWGRB.DF_PRS_ID = DC01.BF_SSN
				AND DWGRB.PF_ACT = 'DWGRB'
				AND DWGRB.BD_ATY_PRF >= DATEADD(DAY,-7,@TODAY)
			LEFT JOIN ODW..CT30_CALL_QUE CT30
				ON CT30.DF_PRS_ID_BR = DC01.BF_SSN
				AND CT30.IF_WRK_GRP = 'AWG5PMTS'
				AND CT30.IC_TSK_STA NOT IN('X','C')
		--check for existing record to add queue task for the current date
			LEFT JOIN  OLS.olqtskbldr.Queues ExistingData
				ON ExistingData.TargetId = DC01.BF_SSN
				AND ExistingData.QueueName = @QueName
				AND
				(
					CAST(ExistingData.AddedAt AS DATE) = @TODAY
					OR ExistingData.ProcessedAt IS NULL
				)
				AND ExistingData.DeletedAt IS NULL
				AND ExistingData.DeletedBy IS NULL
		WHERE
			DC01.LC_GRN IN ('02', '04', '05', '06', '07')
			AND BR01.BN_RHB_PAY_CTR = 4
			AND DWGRL.DF_PRS_ID IS NULL
			AND DWGRB.DF_PRS_ID IS NULL
			AND CT30.DF_PRS_ID_BR IS NULL
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