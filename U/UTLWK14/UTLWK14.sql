BEGIN TRY
	BEGIN TRANSACTION

		DECLARE @TODAY DATE = GETDATE(),
				@NOW DATETIME = GETDATE();
		DECLARE @30DaysAgo DATE = DATEADD(DAY,-30,@TODAY),
				@180DaysAgo DATE = DATEADD(DAY,-180,@TODAY),
				@QueueName VARCHAR(8) = 'BKCP13RV',
				@Arc VARCHAR(5) = 'DBKRW',
				@ScriptId VARCHAR(7) = 'UTLWK14';

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
			BASEPOP.BF_SSN AS TargetId,
			@QueueName AS QueueName,
			NULL AS InstitutionId,
			NULL AS InstitutionType,
			NULL AS DateDue,
			NULL AS TimeDue,
			CONCAT(
					'Compass BK Filed Date: ', CONVERT(VARCHAR(10),BASEPOP.DD_BKR_FIL,101),',',
					'BK Status: ', CONVERT(VARCHAR(10),BASEPOP.DD_BKR_STA,101),',',
					'BK Chapter: ', BASEPOP.DC_BKR_TYP,',',
					'BK Doc: ', BASEPOP.DF_COU_DKT,',',
					'Last RVW: ', CONVERT(VARCHAR(10),BASEPOP.MAXREV,101),',',
					'Date Not: ', CONVERT(VARCHAR(10),BASEPOP.DD_BKR_NTF,101),','
				) AS Comment,
			NULL AS SourceFilename,
			@NOW AS AddedAt,
			@ScriptId AS AddedBy
		FROM
			(--base population
				SELECT 
					LN10.BF_SSN,
					PD24.DD_BKR_FIL,
					PD24.DD_BKR_STA,
					PD24.DC_BKR_TYP,
					PD24.DF_COU_DKT,
					DBKRW_MAX.MAXREV,
					PD24.DD_BKR_NTF,
					IIF(LN10.IC_LON_PGM = 'TILP', 1, 0) AS TILP
				FROM
					UDW..LN10_LON LN10
					INNER JOIN UDW..PD24_PRS_BKR PD24
						ON LN10.BF_SSN = PD24.DF_PRS_ID
						AND PD24.DD_BKR_FIL <= @180DaysAgo
						AND PD24.DC_BKR_TYP = '13'
						AND PD24.DC_BKR_STA = '06'
					LEFT JOIN 
					(--exclude DBKRW type 13
						SELECT DISTINCT
							AY01.DF_PRS_ID
						FROM
							ODW..AY01_BR_ATY AY01
						WHERE 
							AY01.PF_ACT = @Arc
							AND AY01.BD_ATY_PRF > @30DaysAgo
					) DBKRW_13
						ON LN10.BF_SSN = DBKRW_13.DF_PRS_ID
					LEFT JOIN 
					(--get most recent DBKRW arc
						SELECT 
							DF_PRS_ID,
							MAX(BD_ATY_PRF) AS MAXREV 
						FROM
							ODW..AY01_BR_ATY 
						WHERE
							PF_ACT = @Arc
						GROUP BY
							DF_PRS_ID
					) DBKRW_MAX
						ON DBKRW_MAX.DF_PRS_ID = LN10.BF_SSN
					LEFT JOIN
					(--exclude CT30
						SELECT 
							DF_PRS_ID_BR
						FROM
							ODW..CT30_CALL_QUE
						WHERE
							IF_WRK_GRP = @QueueName
							AND IC_TSK_STA NOT IN('X','C')
					) CT30
						ON CT30.DF_PRS_ID_BR = LN10.BF_SSN
				WHERE
					LN10.LA_CUR_PRI > 0.00
					AND LN10.LC_STA_LON10 = 'R'
					AND DBKRW_13.DF_PRS_ID IS NULL --exclude DBKRW type 13
					AND CT30.DF_PRS_ID_BR IS NULL --exclude CT30
			) BASEPOP
			LEFT JOIN OLS.olqtskbldr.Queues ExistingData
				ON ExistingData.TargetId = BASEPOP.BF_SSN
				AND ExistingData.QueueName = @QueueName
				AND
				(
					CAST(ExistingData.AddedAt AS DATE) = @TODAY
					OR ExistingData.ProcessedAt IS NULL
				)
				AND ExistingData.DeletedAt IS NULL
				AND ExistingData.DeletedBy IS NULL
				AND ExistingData.Comment = 
					CONCAT(
							'Compass BK Filed Date: ', CONVERT(VARCHAR(10),BASEPOP.DD_BKR_FIL,101),',',
							'BK Status: ', CONVERT(VARCHAR(10),BASEPOP.DD_BKR_STA,101),',',
							'BK Chapter: ', BASEPOP.DC_BKR_TYP,',',
							'BK Doc: ', BASEPOP.DF_COU_DKT,',',
							'Last RVW: ', CONVERT(VARCHAR(10),BASEPOP.MAXREV,101),',',
							'Date Not: ', CONVERT(VARCHAR(10),BASEPOP.DD_BKR_NTF,101),','
						)
		WHERE
			TILP = 0
			AND ExistingData.TargetId IS NULL
		;
		--select * from ols.olqtskbldr.Queues where QueueName = 'BKCP13RV'--test

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