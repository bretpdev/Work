BEGIN TRY
	BEGIN TRANSACTION

		DECLARE @AddedAt DATETIME = GETDATE();
		DECLARE @TODAY DATE = @AddedAt;
		DECLARE @QueName VARCHAR(10) = 'PREREHAB';
		DECLARE @ScriptId VARCHAR(10) = 'UTLWD30';

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
			''			AS InstitutionId,
			''			AS InstitutionType,
			NULL		AS DateDue,
			NULL		AS TimeDue,
			''			AS Comment,
			''			AS SourceFilename,
			@AddedAt	AS AddedAt,
			@ScriptId	AS AddedBy
		FROM 
			ODW..DC01_LON_CLM_INF DC01
		--get most recent date aux status updated where aux status = 10 to compare to DC10 status update date in WHERE clause
			INNER JOIN
			(
				SELECT 
					DC01_IJ1.BF_SSN,
					DC01_IJ2.LD_AUX_STA_UPD
				FROM 
					ODW..DC01_LON_CLM_INF DC01_IJ1
					INNER JOIN 
					(
						SELECT 
							BF_SSN,
							MAX(LD_AUX_STA_UPD) AS LD_AUX_STA_UPD
						FROM
							ODW..DC01_LON_CLM_INF 
						WHERE 
							LD_AUX_STA_UPD IS NOT NULL
						GROUP BY 
							BF_SSN
					) DC01_IJ2
						ON DC01_IJ2.BF_SSN = DC01_IJ1.BF_SSN
						AND DC01_IJ2.LD_AUX_STA_UPD = DC01_IJ1.LD_AUX_STA_UPD
				WHERE 
					DC01_IJ1.LC_AUX_STA = '10'
			) AUX10
				ON AUX10.BF_SSN = DC01.BF_SSN
		--get most recent DC10 status date where status is 01
			INNER JOIN
			 (
				SELECT
					DC01_IJ3.BF_SSN,
					DC01_IJ4.LD_STA_UPD_DC10
				FROM
					ODW..DC01_LON_CLM_INF DC01_IJ3
					INNER JOIN
					(
						SELECT 
							BF_SSN,
							MAX(LD_STA_UPD_DC10) AS LD_STA_UPD_DC10
						FROM 
							ODW..DC01_LON_CLM_INF	
						GROUP BY 
							BF_SSN
					) DC01_IJ4
						ON DC01_IJ4.BF_SSN = DC01_IJ3.BF_SSN
						AND DC01_IJ4.LD_STA_UPD_DC10 = DC01_IJ3.LD_STA_UPD_DC10
				WHERE 
					DC01_IJ3.LC_STA_DC10 = '01'
					AND DC01_IJ3.LC_PCL_REA IN ('DB','DF','DQ')
			) STA01
				ON DC01.BF_SSN = STA01.BF_SSN
			INNER JOIN ODW..PD01_PDM_INF PD01
				ON DC01.BF_SSN = PD01.DF_PRS_ID
			INNER JOIN ODW..PD03_PRS_ADR_PHN PD03
				ON PD03.DF_PRS_ID = PD01.DF_PRS_ID
				AND PD03.DI_PHN_VLD = 'Y' 
			--check for existing record to add queue task for the current date
			LEFT JOIN  OLS.olqtskbldr.Queues ExistingData
				ON ExistingData.TargetId = PD01.DF_PRS_ID
				AND ExistingData.QueueName = @QueName
				AND CAST(ExistingData.AddedAt AS DATE) = @TODAY
				AND ExistingData.DeletedAt IS NULL
				AND ExistingData.DeletedBy IS NULL
		WHERE
			AUX10.LD_AUX_STA_UPD < STA01.LD_STA_UPD_DC10
			AND DC01.LD_PCL_SUP_LST_ATT < DATEADD(DAY,-3,@TODAY)
			AND DC01.LD_PCL_SUP_LST_CNC < DATEADD(DAY,-5,@TODAY)
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