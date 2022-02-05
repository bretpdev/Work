BEGIN TRY
	BEGIN TRANSACTION

		DECLARE @TODAY DATE = GETDATE(),
				@NOW DATETIME = GETDATE(),
				@ScriptId VARCHAR(8) = 'UTLWB06',
				@QueName VARCHAR(10) = 'KDFLTSKP';
		DECLARE @300DAYSAGO DATE = (DATEADD(DAY,-300,@TODAY)),
				@CurrentCohortYearBegin DATE = --FY year calc based on month
					(
						CASE
							WHEN MONTH(@TODAY) < 10
							THEN CONCAT(YEAR(DATEADD(YEAR,-2,@TODAY)),'1001')
							ELSE CONCAT(YEAR(DATEADD(YEAR,-1,@TODAY)),'1001')
						END
					),
				@CurrentCohortYearEnd DATE = 
					(
						CASE
							WHEN MONTH(@TODAY) < 10
							THEN CONCAT(YEAR(DATEADD(YEAR,-1,@TODAY)),'0930')
							ELSE CONCAT(YEAR(DATEADD(YEAR,0,@TODAY)),'0930')
						END
					)
		;
		--select @TODAY,@300DAYSAGO,@CurrentCohortYearBegin,@CurrentCohortYearEnd

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
		SELECT DISTINCT 
			LN10.BF_SSN AS TargetId,
			'PCHRT300' AS QueueName,
			' ' AS InstitutionId,
			' ' AS InstitutionType,
			NULL AS DateDue,
			NULL AS TimeDue,
			'DEFAULT COHORT LOAN FOR REVIEW' AS Comment,
			NULL AS SourceFilename,
			NULL AS ProcessedAt,
			@NOW AS AddedAt,
			@ScriptId AS AddedBy,
			NULL AS DeletedAt,
			NULL AS DeletedBy
		FROM
 			UDW..LN10_LON LN10
			INNER JOIN UDW..LN16_LON_DLQ_HST LN16
				ON LN10.BF_SSN = LN16.BF_SSN
				AND LN10.LN_SEQ = LN16.LN_SEQ
			INNER JOIN UDW..WQ20_TSK_QUE WQ20
				ON LN10.BF_SSN = WQ20.BF_SSN
			INNER JOIN 
			(--OneLINK data
				SELECT 
					GA01.DF_PRS_ID_BR AS BF_SSN
				FROM 
					ODW..GA01_APP GA01
					INNER JOIN ODW..GA10_LON_APP GA10
						ON GA01.AF_APL_ID = GA10.AF_APL_ID
					INNER JOIN ODW..GA15_NDS_ID GA15
						ON GA10.AF_APL_ID = GA15.AF_APL_ID
						AND GA10.AF_APL_ID_SFX = GA15.AF_APL_ID_SFX
				WHERE 
					GA10.AC_LON_TYP IN ('SF','SU','SL')
					AND GA10.AC_GTE_TRF != 'O' --to exclude guarantee transfered out
					AND GA15.AD_NDS_CLC_ENT_RPD BETWEEN @CurrentCohortYearBegin AND @CurrentCohortYearEnd
					AND GA15.AC_STA_GA15 = 'A'

				UNION ALL

				SELECT 
					CNSL.BF_SSN
				FROM
					(--CL type loans
						SELECT 
							GA01.DF_PRS_ID_BR AS BF_SSN
							,GA10.AF_APL_ID
							,GA10.AF_APL_ID_SFX
						FROM
							ODW..GA01_APP GA01
							INNER JOIN ODW..GA10_LON_APP GA10
								ON GA01.AF_APL_ID = GA10.AF_APL_ID
						WHERE 
							GA10.AC_LON_TYP = 'CL'
					) CNSL
					INNER JOIN 
					(--get entered repayment date
						SELECT 
							GA01.DF_PRS_ID_BR
							,GA15.AD_NDS_CLC_ENT_RPD
						FROM
							ODW..GA01_APP GA01
							INNER JOIN ODW..GA10_LON_APP GA10
								ON GA01.AF_APL_ID = GA10.AF_APL_ID
							INNER JOIN 
							(--active PN type loans during cohort year
								SELECT 
									AF_APL_ID
									,AF_APL_ID_SFX
								FROM
									ODW..GA14_LON_STA 
								WHERE
									AC_STA_GA14 = 'A'
									AND AC_LON_STA_TYP = 'PN'
									AND AD_LON_STA BETWEEN @CurrentCohortYearBegin AND @CurrentCohortYearEnd
							) GA14
								ON GA10.AF_APL_ID = GA14.AF_APL_ID
								AND GA10.AF_APL_ID_SFX = GA14.AF_APL_ID_SFX
							INNER JOIN 
							(--entered repayment during cohort year
								SELECT 
									AF_APL_ID
									,AF_APL_ID_SFX
									,AD_NDS_CLC_ENT_RPD
								FROM
									ODW..GA15_NDS_ID
								WHERE
									AC_STA_GA15 = 'A'
									AND AD_NDS_CLC_ENT_RPD BETWEEN @CurrentCohortYearBegin AND @CurrentCohortYearEnd
							) GA15
								ON GA10.AF_APL_ID = GA15.AF_APL_ID
								AND GA10.AF_APL_ID_SFX = GA15.AF_APL_ID_SFX
						WHERE 
							GA10.AC_LON_TYP IN ('SF','SU')
					) USTAF
						ON CNSL.BF_SSN = USTAF.DF_PRS_ID_BR
			) BLT
				ON LN10.BF_SSN = BLT.BF_SSN
			LEFT JOIN
			(--borrower does not already have a queue task
				SELECT 
					DF_PRS_ID_BR
				FROM
					ODW..CT30_CALL_QUE
				WHERE
					IF_WRK_GRP = @QueName
			) QUE_TASK
				ON QUE_TASK.DF_PRS_ID_BR = LN10.BF_SSN
			LEFT JOIN  OLS.olqtskbldr.Queues ExistingData
				ON ExistingData.TargetId = LN10.BF_SSN
				AND ExistingData.QueueName = @QueName
				AND CAST(ExistingData.AddedAt AS DATE) = @TODAY
				AND ExistingData.DeletedAt IS NULL
				AND ExistingData.DeletedBy IS NULL
		WHERE
			ExistingData.TargetId IS NULL --prevents current day duplicates
			AND QUE_TASK.DF_PRS_ID_BR IS NULL --borrower does not already have a queue task
			AND WQ20.WF_QUE = 'GF'
			AND WQ20.WF_SUB_QUE = 'CF'
			AND	LN10.LA_CUR_PRI > 0.00
			AND LN10.LC_STA_LON10 = 'R'
			AND CAST(LN16.LD_DLQ_OCC AS DATE) <= @300DAYSAGO
			AND LN16.LC_STA_LON16 = '1'
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
		
	INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(GETDATE(),GETDATE(),@ScriptId,'uheaa',SUSER_SNAME());
	SET @ProcessLogId = SCOPE_IDENTITY()

	INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId,@NotificationSeverityTypeId,@ProcessLogId, NULL, NULL)
	SET @ProcessNotificationId = SCOPE_IDENTITY()

	INSERT INTO ULS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId,@EM);

	THROW;
END CATCH;