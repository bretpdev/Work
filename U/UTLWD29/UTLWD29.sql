BEGIN TRY
	BEGIN TRANSACTION

		DROP TABLE IF EXISTS #EMPLOYERS;

		DECLARE @AddedAt DATETIME = GETDATE();
		DECLARE @Today DATE = @AddedAt;
		DECLARE @QueName VARCHAR(10) = 'DACTEMPL';
		DECLARE @ScriptId VARCHAR(10) = 'UTLWD29';

		--insert normalized employer data into a temp table to be used later
		SELECT DISTINCT
			EMPL.BF_SSN,
			EMPL.EMP_ID,
			EMPL.WGE_YR,
			EMPL.WGE_QTR,
			CASE --flag records where yr = 2000 or qtr = 01 for exclusion
				WHEN TRY_CAST(EMPL.WGE_YR AS INT) < 2000 OR (TRY_CAST(EMPL.WGE_YR AS INT) = 2000 AND EMPL.WGE_QTR = '01') THEN 1
				ELSE 0
			END AS YR_2000_OR_QTR_01
		INTO #EMPLOYERS
		FROM
			ODW..DC01_LON_CLM_INF DC01
			INNER JOIN ODW..BR01_BR_CRF BR01
				ON DC01.BF_SSN = BR01.BF_SSN
			INNER JOIN
		--EMP: BR02 stores data for multiple employers all on the same row, these unioned selects normalize that data so each employer is on its own row
			(
				--employer 1
				SELECT
					BR02.BF_SSN,
					BR02.BF_EMP_ID_1 AS EMP_ID,
					BR02.BN_WGE_YR_1 AS WGE_YR,
					BR02.BN_WGE_QTR_1 AS WGE_QTR
				FROM
					ODW..BR02_BR_EMP BR02
				WHERE
					BR02.BI_EMP_INF_OVR = 'Y'

				UNION ALL

				-- employer 2
				SELECT
					BR02.BF_SSN,
					BR02.BF_EMP_ID_2 AS EMP_ID,
					BR02.BN_WGE_YR_2 AS WGE_YR,
					BR02.BN_WGE_QTR_2 AS WGE_QTR
				FROM
					ODW..BR02_BR_EMP BR02
				WHERE
					BR02.BI_EMP_INF_OVR = 'Y'

				UNION ALL

				-- employer 3
				SELECT
					BR02.BF_SSN,
					BR02.BF_EMP_ID_3 AS EMP_ID,
					BR02.BN_WGE_YR_3 AS WGE_YR,
					BR02.BN_WGE_QTR_3 AS WGE_QTR
				FROM
					ODW..BR02_BR_EMP BR02
				WHERE
					BR02.BI_EMP_INF_OVR = 'Y'

				UNION ALL

				-- employer 4
				SELECT
					BR02.BF_SSN,
					BR02.BF_EMP_ID_4 AS EMP_ID,
					BR02.BN_WGE_YR_4 AS WGE_YR,
					BR02.BN_WGE_QTR_4 AS WGE_QTR
				FROM
					ODW..BR02_BR_EMP BR02
				WHERE
					BR02.BI_EMP_INF_OVR = 'Y'

				UNION ALL

				-- employer 5
				SELECT
					BR02.BF_SSN,
					BR02.BF_EMP_ID_5 AS EMP_ID,
					BR02.BN_WGE_YR_5 AS WGE_YR,
					BR02.BN_WGE_QTR_5 AS WGE_QTR
				FROM
					ODW..BR02_BR_EMP BR02
				WHERE
					BR02.BI_EMP_INF_OVR = 'Y'

				UNION ALL

				-- employer 6
				SELECT
					BR02.BF_SSN,
					BR02.BF_EMP_ID_6 AS EMP_ID,
					BR02.BN_WGE_YR_6 AS WGE_YR,
					BR02.BN_WGE_QTR_6 AS WGE_QTR
				FROM
					ODW..BR02_BR_EMP BR02
				WHERE
					BR02.BI_EMP_INF_OVR = 'Y'

				UNION ALL

				-- employer 7
				SELECT
					BR02.BF_SSN,
					BR02.BF_EMP_ID_7 AS EMP_ID,
					BR02.BN_WGE_YR_7 AS WGE_YR,
					BR02.BN_WGE_QTR_7 AS WGE_QTR
				FROM
					ODW..BR02_BR_EMP BR02
				WHERE
					BR02.BI_EMP_INF_OVR = 'Y'

				UNION ALL

				-- employer 8
				SELECT
					BR02.BF_SSN,
					BR02.BF_EMP_ID_8 AS EMP_ID,
					BR02.BN_WGE_YR_8 AS WGE_YR,
					BR02.BN_WGE_QTR_8 AS WGE_QTR
				FROM
					ODW..BR02_BR_EMP BR02
				WHERE
					BR02.BI_EMP_INF_OVR = 'Y'

				UNION ALL

				-- employer 9
				SELECT
					BR02.BF_SSN,
					BR02.BF_EMP_ID_9 AS EMP_ID,
					BR02.BN_WGE_YR_9 AS WGE_YR,
					BR02.BN_WGE_QTR_9 AS WGE_QTR
				FROM
					ODW..BR02_BR_EMP BR02
				WHERE
					BR02.BI_EMP_INF_OVR = 'Y'

				UNION ALL

				-- employer 10
				SELECT
					BR02.BF_SSN,
					BR02.BF_EMP_ID_10 AS EMP_ID,
					BR02.BN_WGE_YR_10 AS WGE_YR,
					BR02.BN_WGE_QTR_10 AS WGE_QTR
				FROM
					ODW..BR02_BR_EMP BR02
				WHERE
					BR02.BI_EMP_INF_OVR = 'Y'

				UNION ALL

				-- employer 11
				SELECT
					BR02.BF_SSN,
					BR02.BF_EMP_ID_11 AS EMP_ID,
					BR02.BN_WGE_YR_11 AS WGE_YR,
					BR02.BN_WGE_QTR_11 AS WGE_QTR
				FROM
					ODW..BR02_BR_EMP BR02
				WHERE
					BR02.BI_EMP_INF_OVR = 'Y'

				UNION ALL

				-- employer 12
				SELECT
					BR02.BF_SSN,
					BR02.BF_EMP_ID_12 AS EMP_ID,
					BR02.BN_WGE_YR_12 AS WGE_YR,
					BR02.BN_WGE_QTR_12 AS WGE_QTR
				FROM
					ODW..BR02_BR_EMP BR02
				WHERE
					BR02.BI_EMP_INF_OVR = 'Y'
			) EMPL --normalized employer data
				ON EMPL.BF_SSN = DC01.BF_SSN
		WHERE 
			DC01.LC_STA_DC10 = '03'
			AND DC01.LC_AUX_STA = ''
			AND DC01.LC_REA_CLM_ASN_DOE = ''
			AND BR01.BC_HLD_REA IN ('13','15','16') --withdrawn, held, or inactivated
			AND EMPL.BF_SSN LIKE '[0-9]%' --EMPL.BF_SSN != ''
			AND EMPL.WGE_YR LIKE '[0-9]%' --EMPL.WGE_YR != ''
			AND EMPL.WGE_QTR LIKE '[0-9]%' --EMPL.WGE_QTR != ''

		--SELECT * FROM #EMPLOYERS --for testing

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
			EMP1.BF_SSN AS TargetId,
			@QueName	AS QueueName,
			NULL		AS InstitutionId,
			NULL		AS InstitutionType,
			NULL		AS DateDue,
			NULL		AS TimeDue,
			NULL		AS Comment,
			NULL		AS SourceFilename,
			@AddedAt	AS AddedAt,
			@ScriptId	AS AddedBy
		FROM
			--join normalized employers data to itself to find duplicates
			#EMPLOYERS EMP1
			INNER JOIN #EMPLOYERS EMP2
				ON EMP2.BF_SSN = EMP1.BF_SSN
			--check for existing record to add queue task for the current date
			LEFT JOIN  OLS.olqtskbldr.Queues ExistingData
				ON ExistingData.TargetId = EMP1.BF_SSN
				AND ExistingData.QueueName = @QueName
				AND 
				(
					CAST(ExistingData.AddedAt AS DATE) = @Today
					OR ExistingData.ProcessedAt IS NULL
				)
				AND ExistingData.DeletedAt IS NULL
				AND ExistingData.DeletedBy IS NULL
			LEFT JOIN ODW..CT30_CALL_QUE CT30
				ON CT30.DF_PRS_ID_BR = EMP1.BF_SSN
				AND CT30.IF_WRK_GRP = @QueName
				AND CT30.IC_TSK_STA IN ('A','W')
		WHERE
			EMP1.YR_2000_OR_QTR_01 = 0 --exclude records where yr = 2000 or qtr = 01
			AND EMP2.YR_2000_OR_QTR_01 = 0 --exclude records where yr = 2000 or qtr = 01
			--AND EMP2.WGE_YR != EMP1.WGE_YR
			--AND EMP2.WGE_QTR != EMP1.WGE_QTR
			AND ExistingData.TargetId IS NULL --record to create queue task does already exist for the current date
			AND CT30.DF_PRS_ID_BR IS NULL --there is no unworked queue on the system
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