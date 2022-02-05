SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @CurrentDateTime DATETIME = GETDATE();
DECLARE @RUNDATE DATE = DATEADD(DAY,-1,@CurrentDateTime);
DECLARE @LOOKBACK DATE = 
(--if run on Monday then lookback to Friday (3 days) otherwise just lookback 1 day to yesterday
	SELECT
		CASE
			WHEN DATENAME(WEEKDAY,@CurrentDateTime) = 'Monday'
			THEN DATEADD(DAY,-3,@CurrentDateTime)
			ELSE DATEADD(DAY,-1,@CurrentDateTime)
		END
);
DECLARE @ScriptId VARCHAR(50) = 'UTLWO38';
DECLARE @ARC VARCHAR(5) = 'RPACH';


BEGIN TRY
	BEGIN TRANSACTION

	INSERT INTO 
		ULS..ArcAddProcessing
			(
				ArcTypeId,
				ArcResponseCodeId,
				AccountNumber,
				RecipientId,
				ARC,
				ActivityType,
				ActivityContact,
				ScriptId,
				ProcessOn,
				Comment,
				IsReference,
				IsEndorser,
				ProcessFrom,
				ProcessTo,
				NeededBy,
				RegardsTo,
				RegardsCode,
				LN_ATY_SEQ,
				ProcessingAttempts,
				CreatedAt,
				CreatedBy,
				ProcessedAt
			)
	SELECT
		1 AS ArcTypeId,
		NULL AS ArcResponseCodeId,
		DF_SPE_ACC_ID,
		'' AS RecipientId,
		@ARC AS ARC,
		NULL AS ActivityType,
		NULL AS ActivityContact,
		@ScriptId AS ScriptId,
		@CurrentDateTime AS ProcessOn,
		CONCAT('INSTALL AMT = ', AMT, '; ADDITIONAL WITHDRAWAL AMOUNT = ', BA_EFT_ADD_WDR) AS Comment,
		0 AS IsReference,
		0 AS IsEndorser,
		NULL AS ProcessFrom,
		NULL AS ProcessTo,
		NULL AS NeededBy,
		NULL AS RegardsTo,
		NULL AS RegardsCode,
		NULL AS LN_ATY_SEQ,
		0 AS ProcessingAttempts, --this gets updated by the arc add script so it is initialized as 0 attempts made
		@CurrentDateTime AS CreatedAt,
		@ScriptId AS CreatedBy,
		NULL AS ProcessedAt
	FROM
	--FINAL_DATA: final data
		(
			SELECT DISTINCT 
				PD10.DF_SPE_ACC_ID,
				SUM(AMT) AS AMT,
				BA_EFT_ADD_WDR,
				PAY_DU_MIN_1_MONTH
			FROM
				UDW..PD10_PRS_NME PD10
			--CALC_PAY_DU_MIN_1_MONTH: calculate PAY_DU_MIN_1_MONTH for rows with MAX LN_RPS_TRM
				INNER JOIN 
				(
					SELECT 
						INIT_POP.SSN,
						INIT_POP.AMT,
						INIT_POP.BA_EFT_ADD_WDR,
						CAST(DATEADD(MONTH,INIT_POP.LN_RPS_TRM -1,INIT_POP.TERMSTARTDATE) AS DATE) AS PAY_DU_MIN_1_MONTH, --1 month before first payment of new tier is due
						INIT_POP.LC_STA_LON65,
						INIT_POP.LD_CRT_LON65,
						INIT_POP.LC_TYP_SCH_DIS
					FROM
					--INIT_POP: initial population
						(
							SELECT
								RS.BF_SSN AS SSN,
								RS.LA_RPS_ISL AS AMT,
								BR30.BA_EFT_ADD_WDR,
								RS.LD_RPS_1_PAY_DU,
								RS.LN_RPS_TRM,
								RS.TERMSTARTDATE,
								LN65.LC_STA_LON65,
								LN65.LD_CRT_LON65,
								RS.LC_TYP_SCH_DIS
							FROM
								UDW.calc.RepaymentSchedules RS
								INNER JOIN UDW..LN65_LON_RPS LN65
									ON RS.BF_SSN = LN65.BF_SSN
									AND RS.LN_SEQ = LN65.LN_SEQ
									AND RS.LN_RPS_SEQ = LN65.LN_RPS_SEQ
								INNER JOIN UDW..BR30_BR_EFT BR30
									ON RS.BF_SSN = BR30.BF_SSN
									AND BR30.BC_EFT_STA = 'A'
									AND BR30.BA_EFT_ADD_WDR > 0.00
								LEFT JOIN UDW..WQ20_TSK_QUE WQ20
									ON RS.BF_SSN = WQ20.BF_SSN
									AND WF_QUE = 'PM'
									AND PF_REQ_ACT = 'RPACH'
									AND WC_STA_WQUE20 in ('U','A','W','H')
 							WHERE
								RS.CurrentGradation = 1
								AND WQ20.BF_SSN IS NULL --exclude borrowers who already have an open queue task
						) INIT_POP
					--MAX_LN_RPS_TRM: initial population with MAX LN_RPS_TRM
						INNER JOIN 
						(
							SELECT
								LN65.BF_SSN AS SSN,
								BR30.BA_EFT_ADD_WDR,
								MAX(RS.LN_RPS_TRM) as LN_RPS_TRM
							FROM
								UDW.calc.RepaymentSchedules RS
								INNER JOIN UDW..LN65_LON_RPS LN65
									ON RS.BF_SSN = LN65.BF_SSN
									AND RS.LN_SEQ = LN65.LN_SEQ
									AND RS.LN_RPS_SEQ = LN65.LN_RPS_SEQ
								INNER JOIN UDW..BR30_BR_EFT BR30
									ON RS.BF_SSN = BR30.BF_SSN
									AND BR30.BC_EFT_STA = 'A'
									AND BR30.BA_EFT_ADD_WDR > 0.00
							WHERE
								RS.CurrentGradation = 1
							GROUP BY
								LN65.BF_SSN,
								BR30.BA_EFT_ADD_WDR
						) MAX_LN_RPS_TRM
							ON INIT_POP.SSN = MAX_LN_RPS_TRM.SSN
							AND INIT_POP.BA_EFT_ADD_WDR = MAX_LN_RPS_TRM.BA_EFT_ADD_WDR
							AND INIT_POP.LN_RPS_TRM = MAX_LN_RPS_TRM.LN_RPS_TRM
				) CALC_PAY_DU_MIN_1_MONTH
					ON PD10.DF_PRS_ID = CALC_PAY_DU_MIN_1_MONTH.SSN
			WHERE 
				(
					LC_STA_LON65 = 'A' 
					AND CAST(LD_CRT_LON65 AS DATE) BETWEEN @LOOKBACK AND @RUNDATE
				) 
				OR
				(
					LC_TYP_SCH_DIS IN('S2','S5') 
					AND PAY_DU_MIN_1_MONTH BETWEEN @LOOKBACK AND @RUNDATE
				)
			GROUP BY
				PD10.DF_SPE_ACC_ID,
				BA_EFT_ADD_WDR,
				PAY_DU_MIN_1_MONTH
		) FINAL_DATA
	--record already exists
		LEFT JOIN ULS..ArcAddProcessing ExistingData
			ON FINAL_DATA.DF_SPE_ACC_ID = ExistingData.AccountNumber
			AND ExistingData.ARC = @ARC
			AND 
				(
					(
						CAST(ExistingData.CreatedAt AS DATE) =  CAST(@CurrentDateTime AS DATE) --to remove anyone added today to prevent duplicates in recovery
						AND ExistingData.Comment = CONCAT('INSTALL AMT = ', AMT, '; ADDITIONAL WITHDRAWAL AMOUNT = ', BA_EFT_ADD_WDR)
					)
					OR ExistingData.ProcessedAt IS NULL
				)
	WHERE
		ExistingData.AccountNumber IS NULL --exclude borrowers who already have an arcadd record

		COMMIT TRANSACTION;

END TRY
	--write message to process logger if an error occurs
BEGIN CATCH
	DECLARE @EM_ VARCHAR(4000) = @ScriptId + ' encountered an error.  Transaction not committed. Error: ' + (SELECT ERROR_MESSAGE());

	ROLLBACK TRANSACTION;

	DECLARE @ProcessLogId_ INT;
	DECLARE @ProcessNotificationId_ INT;
	DECLARE @NotificationTypeId_ INT = (SELECT TOP 1 NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'); --Error report
	DECLARE @NotificationSeverityTypeId_ INT = (SELECT TOP 1 NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
		
	INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(@CurrentDateTime,@CurrentDateTime,@ScriptId,'uheaa',SUSER_SNAME());
	SET @ProcessLogId_ = SCOPE_IDENTITY()

	INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId_,@NotificationSeverityTypeId_,@ProcessLogId_, NULL, NULL)
	SET @ProcessNotificationId_ = SCOPE_IDENTITY()

	INSERT INTO ULS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId_,@EM_);

	THROW;
END CATCH;