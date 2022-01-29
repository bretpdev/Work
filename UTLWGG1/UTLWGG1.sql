BEGIN TRY
	BEGIN TRANSACTION

		DECLARE @AddedAt DATETIME = GETDATE();
		DECLARE @Today DATE = @AddedAt;
		DECLARE @QueName VARCHAR(10) = 'DEMCMP45';
		DECLARE @ScriptId VARCHAR(10) = 'UTLWGG1';

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
			POP.SSN	AS TargetId,
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
			(
				SELECT DISTINCT
					PD01.DF_PRS_ID AS SSN,
				--flag the borrower to be included (= 1) if criteria is met for any records for the borrower; replicates DATA DAY45 (KEEP=SSN); SAS datastep
					MAX
						(
							CASE
								WHEN
									LA10.BC_HLD_REA = ''
									AND
										(
											(AY01.PF_ACT = 'LUBEI' AND AY01.BD_ATY_PRF < DATEADD(DAY, -30, @Today))
											OR (AY01.PF_ACT = 'LSBEI' AND AY01.BD_ATY_PRF < DATEADD(DAY, -90, @Today))
										)
								THEN 1
								WHEN AY01.PF_ACT IS NULL THEN 1
								ELSE 0
							END 
						) AS INCLUDESSN
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
				--AY01: borrower has 'LUBEI' or'LSBEI' activity record
					LEFT JOIN 
					(
						SELECT 
							ATY.DF_PRS_ID,
							MAX(ATY.BD_ATY_PRF) AS BD_ATY_PRF,
							ATY.PF_ACT
						FROM 
							ODW..AY01_BR_ATY ATY
						WHERE 
							ATY.PF_ACT IN ('LUBEI','LSBEI')
						GROUP BY
							ATY.DF_PRS_ID,
							ATY.PF_ACT
					) AY01
						ON AY01.DF_PRS_ID = PD01.DF_PRS_ID
				--CT30: borrower has DEMCMP45 que task
					LEFT JOIN
					(
						SELECT
							QUE.DF_PRS_ID_BR
						FROM 
							ODW..CT30_CALL_QUE QUE
						WHERE 
							QUE.IF_WRK_GRP = 'DEMCMP45'
							AND QUE.IC_TSK_STA IN ('A', 'W')
					) CT30
						ON CT30.DF_PRS_ID_BR = DC01.BF_SSN
				WHERE 
					DC01.LC_AUX_STA = '' 
					AND DC01.LC_STA_DC10 = '03' 
					AND LA10.BC_WDR_REA = '' 
					AND LA10.BC_INA_REA = '' 
					AND	LA10.BC_LEG_ACT_REC_TYP = '1' 		--Legal Action Record Type = '1' (AWG).
					AND LA10.BD_NCP_FWP_LTR_1 <= DATEADD(DAY, -7, @Today) --The first follow up letter has been sent
					AND LA10.BD_NCP_FWP_LTR_2 IS NULL		--the second follow up letter has not been sent.
					AND LA10.BD_HLD < @Today	
					AND DC01.LC_GRN = '02' --AWG
					AND DC02.LA_CLM_BAL > 25.00
					AND --The last payment less than 60 days from current date or is null.
						(
							DC01.LD_LST_PAY < DATEADD(DAY, -60, @Today)
							OR	DC01.LD_LST_PAY IS NULL
						)
					AND CT30.DF_PRS_ID_BR IS NULL --no DEMCMP45 que task
				GROUP BY
					PD01.DF_PRS_ID
			) POP
		--check for existing record to add queue task for the current date
			LEFT JOIN  OLS.olqtskbldr.Queues ExistingData
				ON ExistingData.TargetId = POP.ssn
				AND ExistingData.QueueName = @QueName
				AND CAST(ExistingData.AddedAt AS DATE) = @Today
				AND ExistingData.DeletedAt IS NULL
				AND ExistingData.DeletedBy IS NULL
		WHERE
			POP.INCLUDESSN = 1 --include the borrower if criteria is met; replicates DATA DAY45 (KEEP=SSN); SAS datastep
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