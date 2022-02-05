BEGIN TRY
	BEGIN TRANSACTION

		DECLARE @TODAY DATE = GETDATE(),
				@NOW DATETIME = GETDATE(),
				@ScriptId VARCHAR(8) = 'UTLWB06',
				@QR2 VARCHAR(10) = 'RACTRFND',
				@QR3 VARCHAR(10) = 'REXTRFND'
		;

		INSERT INTO OLS.olqtskbldr.Queues
		(
			TargetId
			,QueueName
			,InstitutionId
			,InstitutionType
			,Comment
			,SourceFilename
			,ProcessedAt
			,AddedAt
			,AddedBy
			,DeletedAt
			,DeletedBy
		)
		SELECT DISTINCT
			PD01.DF_PRS_ID AS TargetId,
			IIF(FD01.LC_DSB_STA = 'A', @QR2, @QR3) AS QueueName, --R2 & R3 respectively
			'' AS InstitutionId,
			'' AS InstitutionType,
			IIF(FD01.LC_DSB_STA = 'A', 'EXTRACT BORROWER REFUND', 'EXTRACT ACTIVE REFUND') AS Comment, --R2 & R3, respectively
			IIF(FD01.LC_DSB_STA = 'A', 'R2', 'R3') AS SourceFilename,
			NULL AS ProcessedAt,
			@NOW AS AddedAt,
			@ScriptId AS AddedBy,
			NULL AS DeletedAt,
			NULL AS DeletedBy
		FROM
			ODW..FD01_QUE_TAB FD01
			INNER JOIN ODW..PD01_PDM_INF PD01
				 ON PD01.DF_PRS_ID = FD01.LF_PRS_ID
			LEFT JOIN
			(--borrower does not already have a queue task
				SELECT 
					DF_PRS_ID_BR
				FROM
					ODW..CT30_CALL_QUE
				WHERE
					IF_WRK_GRP IN (@QR2, @QR3)
			) QUE_TASK
				ON QUE_TASK.DF_PRS_ID_BR = PD01.DF_PRS_ID
			LEFT JOIN  OLS.olqtskbldr.Queues ExistingData
				ON ExistingData.TargetId = PD01.DF_PRS_ID
				AND ExistingData.QueueName = IIF(FD01.LC_DSB_STA = 'A', @QR2, @QR3) --QueueName
				AND CAST(ExistingData.AddedAt AS DATE) = @TODAY
				AND ExistingData.DeletedAt IS NULL
				AND ExistingData.DeletedBy IS NULL
		WHERE
			ExistingData.TargetId IS NULL --prevents current day duplicates
			AND QUE_TASK.DF_PRS_ID_BR IS NULL --borrower does not already have a queue task
			AND FD01.LC_DSB_STA IN ('A','E') --R2 & R3, respectively
			AND DATEDIFF(DAY,FD01.LD_STA_UPD,@TODAY) > 75
		;
		--select * from OLS.olqtskbldr.Queues where AddedBy = 'UTLWB06' --TEST

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