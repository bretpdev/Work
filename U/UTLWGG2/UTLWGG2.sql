BEGIN TRY
	BEGIN TRANSACTION

		DECLARE @TODAY DATE = GETDATE(),
				@NOW DATETIME = GETDATE(),
				@QueueName VARCHAR(10) = 'DEMCMP75',
				@ScriptId VARCHAR(30) = 'UTLWGG2'
		;

		INSERT INTO OLS.olqtskbldr.Queues
		(
			TargetId
			,QueueName
			,InstitutionId
			,InstitutionType
			,DateDue
			,TimeDue
			,Comment
			,SourceFilename
			,ProcessedAt
			,AddedAt
			,AddedBy
			,DeletedAt
			,DeletedBy
		)
		SELECT
			POP.BF_SSN AS TargetId,
			@QueueName AS QueueName,
			'' AS InstitutionId,
			'' AS InstitutionType, 
			NULL AS DateDue,
			NULL AS TimeDue,
			'' AS Comment,
			'' AS SourceFilename,
			NULL AS ProcessedAt,
			@NOW AS AddedAt,
			@ScriptId AS AddedBy,
			NULL AS DeletedAt,
			NULL AS DeletedBy
		FROM
			(--main population
				SELECT DISTINCT
					PD01.DF_PRS_ID AS BF_SSN
					,AY01.BD_ATY_PRF
					,CASE
						WHEN LA10.BC_HLD_REA = '' 
						THEN
							CASE
								WHEN AY01.PF_ACT = 'LUBEI' AND AY01.BD_ATY_PRF < DATEADD(DAY,-30,@TODAY) THEN 1
								WHEN AY01.PF_ACT = 'LSBEI' AND AY01.BD_ATY_PRF < DATEADD(DAY,-90,@TODAY) THEN 1
							END
						WHEN AY01.PF_ACT IS NULL THEN 1
						ELSE 0
					END AS ARCFlag
					,MAX(AY01.BD_ATY_PRF) OVER(PARTITION BY PD01.DF_PRS_ID ORDER BY PD01.DF_PRS_ID) AS MaxDate --match on the most recent arc date (in where below)
					--validation fields for testing:
					--,LA10.BC_HLD_REA
					--,LA10.BD_HLD
					--,AY01.PF_ACT
				FROM
					ODW..PD01_PDM_INF PD01
					INNER JOIN ODW..IN01_LGS_IDM_MST IN01
						ON IN01.IF_IST = PD01.BF_EMP_ID_1
					INNER JOIN ODW..LA10_LEG_ACT LA10
						ON LA10.DF_PRS_ID_BR = PD01.DF_PRS_ID
					INNER JOIN ODW..DC01_LON_CLM_INF DC01
						ON DC01.BF_SSN = PD01.DF_PRS_ID
					INNER JOIN ODW..DC02_BAL_INT DC02
						ON DC02.AF_APL_ID = DC01.AF_APL_ID
						AND DC02.AF_APL_ID_SFX = DC01.AF_APL_ID_SFX
					LEFT JOIN 
					(--get most recent relevant arc
						SELECT 
							DF_PRS_ID,
							PF_ACT, 
							MAX(BD_ATY_PRF) AS BD_ATY_PRF
						FROM
							ODW..AY01_BR_ATY
						WHERE
							PF_ACT IN ('LUBEI','LSBEI')
						GROUP BY 
							DF_PRS_ID, 
							PF_ACT
					) AY01
						ON AY01.DF_PRS_ID = PD01.DF_PRS_ID
					LEFT JOIN
					(--exclude work group
						SELECT 
							DF_PRS_ID_BR
						FROM
							ODW..CT30_CALL_QUE
						WHERE 
							IF_WRK_GRP = 'DEMCMP75'
					) CT30
						ON CT30.DF_PRS_ID_BR = DC01.BF_SSN
				WHERE 
					CT30.DF_PRS_ID_BR IS NULL --exclude work group
					AND LA10.BC_WDR_REA = '' 
					AND LA10.BC_INA_REA = ''  
					AND LA10.BC_LEG_ACT_REC_TYP = '1'
					AND LA10.BD_NCP_FWP_LTR_1 IS NOT NULL
					AND LA10.BD_HLD < @TODAY
					AND DATEADD(DAY,7,LA10.BD_NCP_FWP_LTR_2) <= @TODAY
					AND DC01.LC_AUX_STA = '' 
					AND DC01.LC_STA_DC10 = '03'
					AND DC01.LC_GRN = '02' 
					AND DC02.LA_CLM_BAL > 25.00 
					AND (
							DC01.LD_LST_PAY < DATEADD(DAY,-60,@TODAY) 
							OR DC01.LD_LST_PAY IS NULL
						)
			) POP
			LEFT JOIN  OLS.olqtskbldr.Queues ExistingData
				ON ExistingData.TargetId = POP.BF_SSN
				AND ExistingData.QueueName = @QueueName
				AND ExistingData.DeletedAt IS NULL
				AND ExistingData.DeletedBy IS NULL
				AND  --If added today or still unprocessed
					(
						CAST(ExistingData.AddedAt AS DATE) = @TODAY
						OR ExistingData.ProcessedAt IS NULL
					)
		WHERE
			ExistingData.TargetId IS NULL --prevents current day duplicates and adding a queue if there is already a olqtskbldr request for the same queue
			AND POP.ARCFlag = 1
			AND ISNULL(POP.MaxDate,'') = ISNULL(POP.BD_ATY_PRF,'') --match on the most recent arc date
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
		
	INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(@NOW,@NOW,@ScriptId,'uheaa',SUSER_SNAME());
	SET @ProcessLogId = SCOPE_IDENTITY()

	INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId,@NotificationSeverityTypeId,@ProcessLogId, NULL, NULL)
	SET @ProcessNotificationId = SCOPE_IDENTITY()

	INSERT INTO ULS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId,@EM);

	THROW;
END CATCH;
