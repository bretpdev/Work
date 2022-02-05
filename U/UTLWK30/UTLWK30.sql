BEGIN TRY
	BEGIN TRANSACTION

		DECLARE @AddedAt DATETIME = GETDATE();
		DECLARE @TWO_YEARS_AGO DATE = DATEADD(YEAR, -2, @AddedAt);
		DECLARE @QueName VARCHAR(8) = 	'KDFLTSKP';
		DECLARE @ScriptId VARCHAR(10) = 'UTLWK30';

		--PRINT @TWO_YEARS_AGO; --for testing


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
			PD01.DF_PRS_ID	AS TargetId,
			@QueName		AS QueueName,
			NULL			AS InstitutionId,
			NULL			AS InstitutionType,
			NULL			AS DateDue,
			NULL			AS TimeDue,
			NULL			AS Comment,
			NULL			AS SourceFilename,
			@AddedAt		AS AddedAt,
			@ScriptId		AS AddedBy
		FROM 
			ODW..PD01_PDM_INF PD01
			INNER JOIN ODW..GA01_APP GA01
				ON GA01.DF_PRS_ID_BR = PD01.DF_PRS_ID
			INNER JOIN ODW..GA14_LON_STA GA14
				ON GA14.AF_APL_ID = GA01.AF_APL_ID
				AND GA14.AC_STA_GA14 = 'A'
				AND GA14.AC_LON_STA_TYP IN ('CP','DN')
				AND GA14.AC_LON_STA_REA IN ('DF','DB','DQ')
			INNER JOIN ODW..DC01_LON_CLM_INF DC01
				ON DC01.AF_APL_ID = GA14.AF_APL_ID
				AND DC01.LC_STA_DC10 = '03'
				AND DC01.LC_AUX_STA = ''
				AND DC01.LC_REA_CLM_ASN_DOE = '' 
				AND DC01.LD_LDR_POF <= @TWO_YEARS_AGO --been in default at least two years
			INNER JOIN 
			(
				SELECT
					DC01.AF_APL_ID,
					MIN(DC01.LD_LDR_POF) AS LD_LDR_POF
				FROM
					ODW..DC01_LON_CLM_INF DC01
				WHERE
					DC01.LC_STA_DC10 = '03'
					AND DC01.LC_AUX_STA = ''
					AND DC01.LC_REA_CLM_ASN_DOE = '' 
					AND DC01.LD_LDR_POF <= @TWO_YEARS_AGO --been in default at least two years
				GROUP BY
					DC01.AF_APL_ID
			) MinDC01
			ON MinDC01.AF_APL_ID = DC01.AF_APL_ID
			AND MinDC01.LD_LDR_POF = DC01.LD_LDR_POF
		--borrowers with a balance over 50K
			INNER JOIN
			(
				SELECT
					PRINAPP.DF_PRS_ID_BR AS SSN, 
					SUM(PRINLOAN.AA_CUR_PRI) AS TOTALCURPRIN
				FROM 
					ODW..GA01_APP PRINAPP
					INNER JOIN ODW..GA10_LON_APP PRINLOAN
						ON PRINLOAN.AF_APL_ID = PRINAPP.AF_APL_ID
				GROUP BY 
					PRINAPP.DF_PRS_ID_BR
			) SSNSWBIGPRINBALS
				ON SSNSWBIGPRINBALS.SSN = PD01.DF_PRS_ID
				AND SSNSWBIGPRINBALS.TOTALCURPRIN > 50000 --total balance over 50K
			LEFT JOIN
			(
				SELECT 
					DF_PRS_ID_BR
				FROM
					ODW..CT30_CALL_QUE CT30
				WHERE
					CT30.IF_WRK_GRP = @QueName
					AND CT30.IC_TSK_STA IN ('A','W')
			) QUE_TASK
				ON QUE_TASK.DF_PRS_ID_BR = PD01.DF_PRS_ID
		--check for existing record to add queue task for the current date
			LEFT JOIN OLS.olqtskbldr.Queues ExistingData
				ON ExistingData.TargetId = PD01.DF_PRS_ID
				AND ExistingData.QueueName = @QueName
				AND 
				(
					CAST(ExistingData.AddedAt AS DATE) = CAST(@AddedAt AS DATE)
					OR ExistingData.ProcessedAt IS NULL
				)
				AND ExistingData.DeletedAt IS NULL
				AND ExistingData.DeletedBy IS NULL
		WHERE 
			(
				PD01.DI_VLD_ADR = 'N' 
				OR PD01.DI_PHN_VLD = 'N'
			)
			AND QUE_TASK.DF_PRS_ID_BR IS NULL --borrower does not already have a queue task
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