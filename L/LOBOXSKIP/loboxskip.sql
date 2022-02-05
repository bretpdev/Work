USE ULS

GO
BEGIN TRY
	BEGIN TRANSACTION
		DECLARE @ScriptId VARCHAR(10) = 'LOBOXSKIP'
		DECLARE @Arc VARCHAR(5) = 'SFNDM'
		DECLARE @ActivityType VARCHAR(2) = 'AM'
		DECLARE @ActivityContact VARCHAR(2) = '36'
		DECLARE @TODAY DATE = GETDATE()
		DECLARE @Comment VARCHAR(100) = 'Borrower no longer in a skip status, has valid address and phone.  Cancelling outstanding skip tasks.'
		DECLARE @ActionResponseId INT = (SELECT ActionResponseId FROM uls.quecomplet.ActionResponses WHERE ActionResponse = 'CANCL')
		DECLARE @TaskStatusId INT = (SELECT TaskStatusId FROM uls.quecomplet.TaskStatuses WHERE TaskStatus = 'X')
		/*Compass only and Compass + Onelink*/
		INSERT INTO ULS.quecomplet.Queues(Queue, SubQueue, TaskControlNumber, ARC, AccountIdentifier, TaskStatusId, ActionResponseId, AddedAt, AddedBy)
		SELECT DISTINCT
			WQ20.WF_QUE,
			WQ20.WF_SUB_QUE,
			WQ20.WN_CTL_TSK,
			@Arc AS ARC,
			PD10.DF_SPE_ACC_ID,
			@TaskStatusId AS TaskStatusId,
			@ActionResponseId AS ActionResponseId,
			@TODAY AS AddedAt,
			@ScriptId AS AddedBy
		FROM
			UDW..PD10_PRS_NME PD10
			INNER JOIN UDW..LN10_LON LN10
				ON LN10.BF_SSN = PD10.DF_PRS_ID
				AND LN10.LC_STA_LON10 = 'R'
			INNER JOIN UDW..PD30_PRS_ADR PD30
				ON PD30.DF_PRS_ID = PD10.DF_PRS_ID
				AND PD30.DC_ADR = 'L'
				AND PD30.DI_VLD_ADR = 'Y'
			INNER JOIN UDW..PD42_PRS_PHN PD42
				ON PD42.DF_PRS_ID = PD10.DF_PRS_ID
				AND PD42.DC_PHN = 'H'
				AND PD42.DI_PHN_VLD = 'Y'
			INNER JOIN UDW..WQ20_TSK_QUE WQ20
				ON WQ20.BF_SSN = LN10.BF_SSN
				AND WQ20.WC_STA_WQUE20 IN ('H','A','U','W','P','I')--Has open task
			INNER JOIN ULS.loboxskip.SkipQueue SQ
				ON SQ.SkipQueue = WQ20.WF_QUE
				AND SQ.Active = 1
			INNER JOIN ULS.loboxskip.Regions R
				ON R.RegionId = SQ.RegionId
				AND R.Active = 1
				AND R.Region = 'COMPASS'
			LEFT JOIN ODW..PD01_PDM_INF OneLinkExists --Checks to see if our account is Compass only or also exists in onelink
				ON OneLinkExists.DF_PRS_ID = PD10.DF_PRS_ID
			LEFT JOIN --If our account exists in onelink, we need to check to see if the addr and phone are valid
			(
				SELECT DISTINCT
					PD01.DF_PRS_ID AS Ssn
				FROM
					ODW..PD01_PDM_INF PD01
					INNER JOIN ODW..PD03_PRS_ADR_PHN PD03
						ON PD03.DF_PRS_ID = PD01.DF_PRS_ID
						AND PD03.DC_ADR = 'L'
						AND PD03.DI_VLD_ADR = 'Y'
						AND PD03.DI_PHN_VLD = 'Y'
			) OneLinkValid
				ON OneLinkValid.Ssn = PD10.DF_PRS_ID
			LEFT JOIN ULS.quecomplet.Queues Existing
				ON Existing.Queue = WQ20.WF_QUE
				AND Existing.SubQueue = WQ20.WF_SUB_QUE
				AND Existing.TaskControlNumber = WQ20.WN_CTL_TSK
				AND Existing.ARC = @Arc
				AND Existing.DeletedAt IS NULL
				AND
				(
					Existing.ProcessedAt IS NULL
					OR CONVERT(DATE,Existing.ProcessedAt) = @TODAY
				)
		WHERE
			Existing.AccountIdentifier IS NULL
			AND
			(
				OneLinkExists.DF_PRS_ID IS NULL --Borrower not on Onelink
				OR
				(
					OneLinkExists.DF_PRS_ID IS NOT NULL -- Borrower on Onelink
					AND OneLinkValid.Ssn IS NOT NULL --Borrower has valid address and Phone on OL
				)
			)

		UNION ALL --Compass zero bal doesnt care about the addr and phone validity as the record is closed, and the skip tied to the account can also be closed.

		SELECT DISTINCT
			WQ20.WF_QUE,
			WQ20.WF_SUB_QUE,
			WQ20.WN_CTL_TSK,
			@Arc AS ARC,
			PD10.DF_SPE_ACC_ID,
			@TaskStatusId AS TaskStatusId,
			@ActionResponseId AS ActionResponseId,
			@TODAY AS AddedAt,
			@ScriptId AS AddedBy
		FROM
			UDW..PD10_PRS_NME PD10
			INNER JOIN UDW..LN10_LON LN10
				ON LN10.BF_SSN = PD10.DF_PRS_ID
				AND LN10.LC_STA_LON10 = 'R'
			INNER JOIN
			(
				SELECT
					BF_SSN,
					SUM(LA_CUR_PRI) AS Bal
				FROM
					UDW..LN10_LON
				WHERE
					LC_STA_LON10 = 'R'
				GROUP BY
					BF_SSN
			) Summed
				ON Summed.BF_SSN = LN10.BF_SSN
			INNER JOIN UDW..WQ20_TSK_QUE WQ20
				ON WQ20.BF_SSN = LN10.BF_SSN
				AND WQ20.WC_STA_WQUE20 IN ('H','A','U','W','P','I')--Has open task
			INNER JOIN ULS.loboxskip.SkipQueue SQ
				ON SQ.SkipQueue = WQ20.WF_QUE
				AND SQ.Active = 1
			INNER JOIN ULS.loboxskip.Regions R
				ON R.RegionId = SQ.RegionId
				AND R.Active = 1
				AND R.Region = 'COMPASS'
			LEFT JOIN ODW..PD01_PDM_INF OneLinkExists --Checks to see if our account is Compass only or also exists in onelink
				ON OneLinkExists.DF_PRS_ID = PD10.DF_PRS_ID
			LEFT JOIN --If our account exists in onelink, we need to check to see if the addr and phone are valid
			(
				SELECT DISTINCT
					PD01.DF_PRS_ID AS Ssn
				FROM
					ODW..PD01_PDM_INF PD01
					INNER JOIN
					(
						SELECT
							BF_SSN,
							SUM(LA_CLM_BAL) AS Bal
						FROM
							ODW..DC02_BAL_INT
						GROUP BY
							BF_SSN
					) Summed
						ON Summed.BF_SSN = PD01.DF_PRS_ID
				WHERE
					Summed.Bal = 0.00
			) OneLinkValid
				ON OneLinkValid.Ssn = PD10.DF_PRS_ID
			LEFT JOIN ULS.quecomplet.Queues Existing
				ON Existing.Queue = WQ20.WF_QUE
				AND Existing.SubQueue = WQ20.WF_SUB_QUE
				AND Existing.TaskControlNumber = WQ20.WN_CTL_TSK
				AND Existing.ARC = @Arc
				AND Existing.DeletedAt IS NULL
				AND
				(
					Existing.ProcessedAt IS NULL
					OR CONVERT(DATE,Existing.ProcessedAt) = @TODAY
				)
		WHERE
			Summed.Bal = 0.00
			AND Existing.AccountIdentifier IS NULL
			AND
			(
				OneLinkExists.DF_PRS_ID IS NULL --Borrower not on Onelink
				OR
				(
					OneLinkExists.DF_PRS_ID IS NOT NULL -- Borrower on Onelink
					AND OneLinkValid.Ssn IS NOT NULL --Borrower has valid address and Phone on OL
				)
			)

		/*OneLink only and Compass + Onelink*/
		INSERT INTO OLS.qworkerlgp.Queues(Ssn,Department,WorkGroupId,ActionCode,ActivityType,ActivityContactType,TaskComment,AddedAt,AddedBy)
		SELECT DISTINCT
			PD01.DF_PRS_ID AS Ssn,
			CT30.IC_REC_TYP AS Department,
			CT30.IF_WRK_GRP AS WorkGroupId,
			@Arc AS ActionCode,
			@ActivityType AS ActivityType,
			@ActivityContact AS ActivityContactType,
			@Comment AS TaskComment,
			@TODAY AS AddedAt,
			@ScriptId AS AddedBy
		FROM
			ODW..PD01_PDM_INF PD01
			INNER JOIN ODW..PD03_PRS_ADR_PHN PD03
				ON PD03.DF_PRS_ID = PD01.DF_PRS_ID
				AND PD03.DC_ADR = 'L'
				AND PD03.DI_VLD_ADR = 'Y'
				AND PD03.DI_PHN_VLD = 'Y'
			INNER JOIN ODW..CT30_CALL_QUE CT30
				ON CT30.DF_PRS_ID_BR = PD01.DF_PRS_ID
				AND CT30.IC_TSK_STA IN ('A','W')
			INNER JOIN ULS.loboxskip.SkipQueue SQ
				ON SQ.SkipQueue = CT30.IF_WRK_GRP
				AND SQ.Active = 1
			INNER JOIN ULS.loboxskip.Regions R
				ON R.RegionId = SQ.RegionId
				AND R.Active = 1
				AND R.Region = 'ONELINK'
			LEFT JOIN UDW..PD10_PRS_NME CompassExists --Checks to see if our account is Onelink only or also exists in compass
				ON CompassExists.DF_PRS_ID = PD01.DF_PRS_ID
			LEFT JOIN --If our account exists in compass, we need to check to see if the addr and phone are valid
			(
				SELECT DISTINCT
					PD10.DF_PRS_ID
				FROM
					UDW..PD10_PRS_NME PD10
					INNER JOIN UDW..LN10_LON LN10
						ON LN10.BF_SSN = PD10.DF_PRS_ID
						AND LN10.LC_STA_LON10 = 'R'
					INNER JOIN UDW..PD30_PRS_ADR PD30
						ON PD30.DF_PRS_ID = PD10.DF_PRS_ID
						AND PD30.DC_ADR = 'L'
						AND PD30.DI_VLD_ADR = 'Y'
					INNER JOIN UDW..PD42_PRS_PHN PD42
						ON PD42.DF_PRS_ID = PD10.DF_PRS_ID
						AND PD42.DC_PHN = 'H'
						AND PD42.DI_PHN_VLD = 'Y'
			) CompassValid
				ON CompassValid.DF_PRS_ID = PD01.DF_PRS_ID
			LEFT JOIN OLS.qworkerlgp.Queues Existing
				ON Existing.Ssn = CT30.DF_PRS_ID_BR
				AND Existing.Department = CT30.IC_REC_TYP
				AND Existing.WorkGroupId = CT30.IF_WRK_GRP
				AND Existing.ActionCode = @Arc
				AND Existing.ActivityType = @ActivityType
				AND Existing.ActivityContactType = @ActivityContact
				AND Existing.DeletedAt IS NULL
				AND
				(
					Existing.ProcessedAt IS NULL
					OR CONVERT(DATE,Existing.ProcessedAt) = @TODAY
				)
		WHERE
			Existing.Ssn IS NULL
			AND
			(
				CompassExists.DF_PRS_ID IS NULL --Borrower not on Compass
				OR
				(
					CompassExists.DF_PRS_ID IS NOT NULL -- Borrower on Compass
					AND CompassValid.DF_PRS_ID IS NOT NULL --Borrower has valid address and Phone on Compass
				)
			)

		UNION ALL --Onelink zero bal doesnt care about address or phone as the record is closed, so the queue can also be closed

		SELECT DISTINCT
			PD01.DF_PRS_ID AS Ssn,
			CT30.IC_REC_TYP AS Department,
			CT30.IF_WRK_GRP AS WorkGroupId,
			@Arc AS ActionCode,
			@ActivityType AS ActivityType,
			@ActivityContact AS ActivityContact,
			@Comment AS TaskComment,
			@TODAY AS AddedAt,
			@ScriptId AS AddedBy
		FROM
			ODW..PD01_PDM_INF PD01
			INNER JOIN
			(
				SELECT
					BF_SSN,
					SUM(LA_CLM_BAL) AS Bal
				FROM
					ODW..DC02_BAL_INT
				GROUP BY
					BF_SSN
			) Summed
				ON Summed.BF_SSN = PD01.DF_PRS_ID
			INNER JOIN ODW..CT30_CALL_QUE CT30
				ON CT30.DF_PRS_ID_BR = PD01.DF_PRS_ID
				AND CT30.IC_TSK_STA IN ('A','W')
			INNER JOIN ULS.loboxskip.SkipQueue SQ
				ON SQ.SkipQueue = CT30.IF_WRK_GRP
				AND SQ.Active = 1
			INNER JOIN ULS.loboxskip.Regions R
				ON R.RegionId = SQ.RegionId
				AND R.Active = 1
				AND R.Region = 'ONELINK'
			LEFT JOIN UDW..PD10_PRS_NME CompassExists --Checks to see if our account is Onelink only or also exists in compass
				ON CompassExists.DF_PRS_ID = PD01.DF_PRS_ID
			LEFT JOIN --If our account exists in compass, we need to check to see if the addr and phone are valid
			(
				SELECT DISTINCT
					PD10.DF_PRS_ID
				FROM
					UDW..PD10_PRS_NME PD10
					INNER JOIN UDW..LN10_LON LN10
						ON LN10.BF_SSN = PD10.DF_PRS_ID
						AND LN10.LC_STA_LON10 = 'R'
					INNER JOIN
					(
						SELECT
							BF_SSN,
							SUM(LA_CUR_PRI) AS Bal
						FROM
							UDW..LN10_LON
						WHERE
							LC_STA_LON10 = 'R'
						GROUP BY
							BF_SSN
					) Summed
						ON Summed.BF_SSN = LN10.BF_SSN
				WHERE
					Summed.Bal = 0.00
			) CompassValid
				ON CompassValid.DF_PRS_ID = PD01.DF_PRS_ID
			LEFT JOIN OLS.qworkerlgp.Queues Existing
				ON Existing.Ssn = CT30.DF_PRS_ID_BR
				AND Existing.Department = CT30.IC_REC_TYP
				AND Existing.WorkGroupId = CT30.IF_WRK_GRP
				AND Existing.ActionCode = @Arc
				AND Existing.ActivityType = @ActivityType
				AND Existing.ActivityContactType = @ActivityContact
				AND Existing.DeletedAt IS NULL
				AND
				(
					Existing.ProcessedAt IS NULL
					OR CONVERT(DATE,Existing.ProcessedAt) = @TODAY
				)
		WHERE
			Summed.Bal = 0.00
			AND Existing.Ssn IS NULL
			AND
			(
				CompassExists.DF_PRS_ID IS NULL --Borrower not on Compass
				OR
				(
					CompassExists.DF_PRS_ID IS NOT NULL -- Borrower on Compass
					AND CompassValid.DF_PRS_ID IS NOT NULL --Borrower has valid address and Phone on Compass
				)
			)
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
DECLARE @EM VARCHAR(4000) = @ScriptId + ' encountered an error.  Transaction not committed. Error: ' + (SELECT ERROR_MESSAGE());

	ROLLBACK TRANSACTION;

	DECLARE @ProcessLogId INT;
	DECLARE @ProcessNotificationId INT;
	DECLARE @NotificationTypeId INT = (SELECT TOP 1 NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'); --Error report
	DECLARE @NotificationSeverityTypeId INT = (SELECT TOP 1 NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
		
	INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(@TODAY,@TODAY,@ScriptId,'uheaa',SUSER_SNAME());
	SET @ProcessLogId = SCOPE_IDENTITY()

	INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId,@NotificationSeverityTypeId,@ProcessLogId, NULL, NULL)
	SET @ProcessNotificationId = SCOPE_IDENTITY()

	INSERT INTO ULS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId,@EM);

	THROW;
END CATCH;